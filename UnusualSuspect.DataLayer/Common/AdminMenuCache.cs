using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using System.Runtime.CompilerServices;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Common;

/// <summary>
/// Change-token source used to invalidate every cached admin menu at once.
/// Cache entries add <see cref="CreateChangeToken"/> as an expiration token; <see cref="Invalidate"/> expires them all.
/// State is per process: with several instances the entries also have a short absolute expiration as a fallback.
/// </summary>
public static class AdminMenuCache
{
    private static CancellationTokenSource _cts = new();

    /// <summary>Token that fires the next time <see cref="Invalidate"/> is called.</summary>
    public static IChangeToken CreateChangeToken() => new CancellationChangeToken(Volatile.Read(ref _cts).Token);

    /// <summary>Expires all cached admin menus.</summary>
    public static void Invalidate()
    {
        var previous = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
        previous.Cancel();
        previous.Dispose();
    }
}

/// <summary>
/// Expires the cached admin menus whenever a save changes data the menu is built from
/// (menus, actions/controllers, role/user permissions and role membership). Hooking the save
/// instead of individual service methods also covers changes made through ASP.NET Core Identity.
/// </summary>
public sealed class AdminMenuCacheInvalidationInterceptor : SaveChangesInterceptor
{
    private static readonly HashSet<Type> MenuRelatedTypes =
    [
        typeof(CustomMenu),
        typeof(AmAction),
        typeof(AMController),
        typeof(ActionForRole),
        typeof(ActionForUser),
        typeof(SoftwarerRoleForUser),
        typeof(Role),
        typeof(Microsoft.AspNetCore.Identity.IdentityUserRole<int>)
    ];

    // The interceptor instance is shared by all contexts, so "this save touches menu data" is remembered per context.
    private readonly ConditionalWeakTable<DbContext, object> _pending = new();

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        MarkIfMenuRelated(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        MarkIfMenuRelated(eventData.Context);
        return ValueTask.FromResult(result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        InvalidateIfMarked(eventData.Context);
        return result;
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        InvalidateIfMarked(eventData.Context);
        return ValueTask.FromResult(result);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        Unmark(eventData.Context);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        Unmark(eventData.Context);
        return Task.CompletedTask;
    }

    private void MarkIfMenuRelated(DbContext? context)
    {
        if (context == null)
            return;

        foreach (EntityEntry entry in context.ChangeTracker.Entries())
        {
            if ((entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                && MenuRelatedTypes.Contains(entry.Metadata.ClrType))
            {
                _pending.AddOrUpdate(context, true);
                return;
            }
        }
    }

    private void InvalidateIfMarked(DbContext? context)
    {
        if (context != null && _pending.Remove(context))
            AdminMenuCache.Invalidate();
    }

    private void Unmark(DbContext? context)
    {
        if (context != null)
            _pending.Remove(context);
    }
}
