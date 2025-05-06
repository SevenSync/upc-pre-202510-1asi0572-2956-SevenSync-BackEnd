namespace MaceTech.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    public Task CompleteAsync();
}