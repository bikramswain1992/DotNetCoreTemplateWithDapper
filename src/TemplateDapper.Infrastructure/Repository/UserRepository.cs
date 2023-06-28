using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using TemplateDapper.Application.Interfaces;
using TemplateDapper.Domain.Entities;

namespace TemplateDapper.Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbConnection dbConnection, ILogger<UserRepository> logger)
        : base(dbConnection, logger)
    {
    }

    public async Task<User> GetAsync<TRequest>(string query, CancellationToken cancellationToken, TRequest request = default!)
        where TRequest : class, new()
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = await _dbConnection.QueryFirstOrDefaultAsync<User>(query, request);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get User data using query {query}.");
            }
        }
        return default!;
    }
}
