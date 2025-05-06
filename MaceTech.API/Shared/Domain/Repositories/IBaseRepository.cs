namespace MaceTech.API.Shared.Domain.Repositories;

public interface IBaseRepository<TEntity>
{
    public Task AddAsync(TEntity entity);
    public Task<TEntity?> FindByIdAsync(long id);
    public void Update(TEntity entity);
    public void Remove(TEntity entity);
    public Task<IEnumerable<TEntity>> ListAsync();
}