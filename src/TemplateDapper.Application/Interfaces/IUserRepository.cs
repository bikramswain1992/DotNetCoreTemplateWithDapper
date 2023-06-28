using TemplateDapper.Domain.Entities;

namespace TemplateDapper.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetAsync<TRequest>(string query, CancellationToken cancellationToken, TRequest request = default!)
        where TRequest : class, new();
}
