using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using UnusualSuspect.Entities.Common;

namespace UnusualSuspect.DataLayer.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>, IUnitOfWork
{
	public ApplicationDbContext(DbContextOptions options)
			: base(options)
	{

	}
	//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	//{
	//	//enable sensitive data logging
	//	optionsBuilder.EnableSensitiveDataLogging();
	//}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		var entitiesAssembly = typeof(IEntity).Assembly;
		modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
		modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);

		var cascadeFKs = modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

		foreach (var fk in cascadeFKs)
			fk.DeleteBehavior = DeleteBehavior.Restrict;

		modelBuilder.Entity<SoftSection>().HasData(
			new SoftSection
			{
				Id = 1,
				TypeName = "پنل مدیریت",
				AreaName = "AdminPanel"
			});

		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		//modelBuilder.Entity<ReadyToGameStatus>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<PreGameGroupStatus>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<SmsSendingStatus>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<RoleCard>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<GameStatus>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<ScoreType>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<GameType>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<Sticker>().Property(x => x.Id)
		//		.ValueGeneratedNever();
		//modelBuilder.Entity<CharacterCard>().Property(x => x.Id)
		//		.ValueGeneratedNever();
	}

	// EF Core's parameterless SaveChanges()/SaveChangesAsync(ct) call the (bool) overloads internally.
	// All the work therefore lives in the (bool) overloads only; the parameterless ones just delegate,
	// so validation and string cleaning run exactly once per save.
	public override int SaveChanges() => SaveChanges(acceptAllChangesOnSuccess: true);

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		ChangeTracker.DetectChanges();
		var autoDetectWasEnabled = ChangeTracker.AutoDetectChangesEnabled;
		ChangeTracker.AutoDetectChangesEnabled = false;
		try
		{
			beforeSaveChanges();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}
		finally
		{
			ChangeTracker.AutoDetectChangesEnabled = autoDetectWasEnabled;
		}
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

	public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		ChangeTracker.DetectChanges();
		var autoDetectWasEnabled = ChangeTracker.AutoDetectChangesEnabled;
		ChangeTracker.AutoDetectChangesEnabled = false;
		try
		{
			beforeSaveChanges();
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		finally
		{
			ChangeTracker.AutoDetectChangesEnabled = autoDetectWasEnabled;
		}
	}
	public string GetConnectionString()
	{
		return this.Database.GetConnectionString();
	}

	// Called with AutoDetectChanges already switched off (the callers ran DetectChanges once just before),
	// so the ChangeTracker.Entries() calls below do not trigger further DetectChanges passes.
	private void beforeSaveChanges()
	{
		validateEntities();
		// Cleaning may rewrite string values; run DetectChanges again only when something actually changed
		// so the new values are picked up before saving.
		if (_cleanString())
			ChangeTracker.DetectChanges();
	}

	// Public, readable and writable string properties per entity type. Reflection is done once per type
	// instead of on every save for every changed entity.
	private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _stringPropertiesCache = new();

	private static PropertyInfo[] GetStringProperties(Type entityType)
	{
		return _stringPropertiesCache.GetOrAdd(entityType, static type =>
			type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string))
				.ToArray());
	}

	/// <returns>true if at least one property value was changed.</returns>
	private bool _cleanString()
	{
		var anyChanged = false;
		var changedEntities = ChangeTracker.Entries()
				.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
		foreach (var item in changedEntities)
		{
			if (item.Entity == null)
				continue;

			foreach (var property in GetStringProperties(item.Entity.GetType()))
			{
				var val = (string)property.GetValue(item.Entity, null);

				if (val.HasValue())
				{
					var newVal = val.Fa2En().FixPersianChars();
					if (newVal == val)
						continue;
					property.SetValue(item.Entity, newVal, null);
					anyChanged = true;
				}
			}
		}
		return anyChanged;
	}

	private void validateEntities()
	{
		var errors = this.GetValidationErrors();
		if (!string.IsNullOrWhiteSpace(errors))
		{
			var loggerFactory = this.GetService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
			logger.LogError(errors);
			throw new InvalidOperationException(errors);
		}
	}
	public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
	{
		Set<TEntity>().AddRange(entities);
	}
	public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
	{
		Set<TEntity>().RemoveRange(entities);
	}
	public async Task<TEntity> GetFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
																														 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
																														 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
																														 bool disableTracking = true,
																														 bool ignoreQueryFilters = false,
																														 CancellationToken cancellationToken = default) where TEntity : class
	{
		IQueryable<TEntity> query = Set<TEntity>().AsQueryable();

		if (disableTracking)
			query = query.AsNoTracking();

		if (include != null)
			query = include(query);

		if (predicate != null)
			query = query.Where(predicate);

		if (ignoreQueryFilters)
			query = query.IgnoreQueryFilters();

		if (orderBy != null)
			return await orderBy(query).FirstOrDefaultAsync(cancellationToken);
		else
			return await query.FirstOrDefaultAsync(cancellationToken);
	}

	public IQueryable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
																						 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
																						 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
																						 bool disableTracking = true,
																						 bool ignoreQueryFilters = false) where TEntity : class
	{

		IQueryable<TEntity> query = Set<TEntity>().AsQueryable();

		if (disableTracking)
			query = query.AsNoTracking();

		if (include != null)
			query = include(query);

		if (predicate != null)
			query = query.Where(predicate);

		if (ignoreQueryFilters)
			query = query.IgnoreQueryFilters();

		if (orderBy != null)
			return orderBy(query);

		return query;
	}
}
