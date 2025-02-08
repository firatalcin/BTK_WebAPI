using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RepositoryContext _context;

    public RepositoryBase(RepositoryContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll(bool trackChanges)
    {
        return !trackChanges ? 
            _context.Set<T>().AsNoTracking() :
            _context.Set<T>();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate, bool trackChanges)
    {
        return !trackChanges ? 
            _context.Set<T>().Where(predicate).AsNoTracking() : 
            _context.Set<T>().Where(predicate);
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}