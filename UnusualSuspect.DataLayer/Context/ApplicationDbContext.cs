using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Entities;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Entities.JcoSecurity;
using UnusualSuspect.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Entities.GameModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnusualSuspect.DataLayer.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>, IUnitOfWork
{
	public ApplicationDbContext(DbContextOptions options)
			: base(options)
	{

	}
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
		modelBuilder.Entity<RoleCard>().Property(x => x.Id)
				.ValueGeneratedNever();
		modelBuilder.Entity<SmsSendingStatus>().Property(x => x.Id)
				.ValueGeneratedNever();
		modelBuilder.Entity<GameType>().Property(x => x.Id)
				.ValueGeneratedNever();
		modelBuilder.Entity<ReadyToGameStatus>().Property(x => x.Id)
			.ValueGeneratedNever();
	}


	public override int SaveChanges()
	{
		ChangeTracker.DetectChanges();
		beforeSaveChanges();
		ChangeTracker.AutoDetectChangesEnabled = false;
		var result = base.SaveChanges();
		ChangeTracker.AutoDetectChangesEnabled = true;
		return result;
	}
	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		ChangeTracker.DetectChanges();
		beforeSaveChanges();
		ChangeTracker.AutoDetectChangesEnabled = false;
		var result = base.SaveChanges(acceptAllChangesOnSuccess);
		ChangeTracker.AutoDetectChangesEnabled = true;
		return result;
	}
	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		beforeSaveChanges();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		ChangeTracker.DetectChanges();
		beforeSaveChanges();
		ChangeTracker.AutoDetectChangesEnabled = false;
		var result = base.SaveChangesAsync(cancellationToken);
		ChangeTracker.AutoDetectChangesEnabled = true;
		return result;
	}
	public string GetConnectionString()
	{
		return this.Database.GetConnectionString();
	}

	private void beforeSaveChanges()
	{
		validateEntities();
		_cleanString();
	}


	private void _cleanString()
	{
		var changedEntities = ChangeTracker.Entries()
				.Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
		foreach (var item in changedEntities)
		{
			if (item.Entity == null)
				continue;

			var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

			foreach (var property in properties)
			{
				var propName = property.Name;
				var val = (string)property.GetValue(item.Entity, null);

				if (val.HasValue())
				{
					var newVal = val.Fa2En().FixPersianChars();
					if (newVal == val)
						continue;
					property.SetValue(item.Entity, newVal, null);
				}
			}
		}
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
