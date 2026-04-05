using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UrunSatisPlatformu.Data.Abstract;

namespace UrunSatisPlatformu.Data.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return (await _dbSet.FindAsync(id))!;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            // _context sendeki DbContext'in adı neyse o olmalı (ApplicationDbContext gibi)
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include); // Ürünleri listeye dahil et (Paket servis)
                }
            }

            return await query.ToListAsync();
        }
    }
}