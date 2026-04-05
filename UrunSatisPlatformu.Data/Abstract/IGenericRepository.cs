using System.Linq.Expressions;

namespace UrunSatisPlatformu.Data.Abstract
{
    // <T> ifadesi buraya Product, Category vb. herhangi bir class'ın gelebileceğini belirtir.
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); // Şarta göre arama (örn: isme göre)
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        // Bu metod, "Kategoriyi getirirken Ürünleri de getir" dememize yarayacak
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
    }
}