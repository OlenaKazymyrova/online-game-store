namespace OnlineGameStore.BLL.Interfaces;

public interface IService<in T1, T2>
{
    Task<T2?> GetByIdAsync(Guid id);
    Task<IEnumerable<T2>> GetAllAsync();
    Task<T2?> AddAsync(T1 entity);
    Task<bool> UpdateAsync(T1 entity);
    Task<bool> DeleteAsync(Guid id);
}