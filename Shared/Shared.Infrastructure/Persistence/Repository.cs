using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Entities;
using Shared.Core.Repositories;

namespace Shared.Infrastructure.Persistence;

public class Repository<T, TContext> : IRepository<T>
    where T : BaseEntity
    where TContext : DbContext
{
    protected readonly TContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(TContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbSet.Where(e => !e.IsDeleted).ToListAsync(cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = DbSet.Where(e => !e.IsDeleted);
        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        => await DbSet.Where(e => !e.IsDeleted).AnyAsync(filter, cancellationToken);

    public async Task<(List<T> Items, int TotalCount)> GetPagedAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = DbSet.Where(e => !e.IsDeleted);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (pageSize == -1)
        {
            var all = await query.ToListAsync(cancellationToken);
            return (all, totalCount);
        }

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity) => DbSet.Update(entity);

    public void Remove(T entity)
    {
        entity.IsDeleted = true;
        DbSet.Update(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await Context.SaveChangesAsync(cancellationToken);
}
