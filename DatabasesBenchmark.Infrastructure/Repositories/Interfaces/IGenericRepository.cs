namespace DatabasesBenchmark.Infrastructure.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
    }
}
