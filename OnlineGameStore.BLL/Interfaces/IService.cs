namespace OnlineGameStore.BLL.Interfaces;

public interface IService<T>
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> AddAsync(T dto);
    Task<bool> UpdateAsync(T dto);
    Task<bool> DeleteAsync(Guid id);
}