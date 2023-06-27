using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using TemplateDapper.Application.Interfaces;

namespace TemplateDapper.Infrastructure.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger<GenericRepository<TEntity>> _logger;

    public GenericRepository(IDbConnection dbConnection, ILogger<GenericRepository<TEntity>> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var query = $"select * from {typeof(TEntity).Name.ToLower()} where Id = @Id";

                var result = await _dbConnection.QuerySingleOrDefaultAsync<TEntity>(query, new { Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get data for {typeof(TEntity).Name} using id {id}.");
            }

        }
        return default!;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var query = $"select * from {typeof(TEntity).Name.ToLower()}";

                var result = await _dbConnection.QueryAsync<TEntity>(query);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get data for {typeof(TEntity).Name}.");
            }

        }
        return Enumerable.Empty<TEntity>();
    }

    public async Task<int> CreateAsync(TEntity request, CancellationToken cancellationToken, IDbTransaction dbTransaction = default!)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var columns = GetColumns<TEntity>();
                var stringOfColumns = string.Join(", ", columns);
                var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
                var query = $"insert into {typeof(TEntity).Name.ToLower()} ({stringOfColumns}) output inserted.Id values ({stringOfParameters})";

                var result = dbTransaction == default ?
                    await _dbConnection.ExecuteScalarAsync<int>(query, request)
                    : await _dbConnection.ExecuteScalarAsync<int>(query, request, dbTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create new {typeof(TEntity).Name}.");
            }
        }
        return -1;
    }

    public async Task<int> UpdateAsync(TEntity request, CancellationToken cancellationToken, IDbTransaction dbTransaction = default!)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var columns = GetColumns<TEntity>();
                var stringColumnParameters = string.Join(", ", columns.Select(x => $"{x}=@{x}"));
                var query = $"update {typeof(TEntity).Name.ToLower()} set {stringColumnParameters} where Id = @Id";

                var result = dbTransaction == default ?
                    await _dbConnection.ExecuteAsync(query, request)
                    : await _dbConnection.ExecuteAsync(query, request, dbTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create new {typeof(TEntity).Name}.");
            }
        }
        return -1;
    }

    public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken, IDbTransaction dbTransaction = default!)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var query = $"delete from {typeof(TEntity).Name.ToLower()} where Id = @Id";

                var result = dbTransaction == default ?
                    await _dbConnection.ExecuteAsync(query, new { Id = id })
                    : await _dbConnection.ExecuteAsync(query, new { Id = id }, dbTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete {typeof(TEntity).Name}.");
            }
        }
        return -1;
    }

    private IEnumerable<string> GetColumns<T>()
    {
        return typeof(T)
                .GetProperties()
                .Where(e => e.Name != "Id")
                .Select(e => e.Name);
    }
}
