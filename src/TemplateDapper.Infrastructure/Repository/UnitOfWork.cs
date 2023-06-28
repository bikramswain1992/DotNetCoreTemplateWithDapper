using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TemplateDapper.Application.Interfaces;

namespace TemplateDapper.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlConnection _dbConnection;
    private DbTransaction _dbTransaction = default!;

    public IUserRepository UserRepository { get; }

    public UnitOfWork(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        UserRepository = new UserRepository(_dbConnection, serviceProvider.GetRequiredService<ILogger<UserRepository>>());
    }

    public IDbTransaction BeginTransaction()
    {
        if (_dbConnection.State == ConnectionState.Closed)
        {
            _dbConnection.Open();
        }
        _dbTransaction = _dbConnection.BeginTransaction();

        return _dbTransaction;
    }

    public void CommitTransaction()
    {
        try
        {
            _dbTransaction.Commit();
        }
        catch
        {
            _dbTransaction.Rollback();
            throw;
        }
        finally
        {
            _dbTransaction.Dispose();
        }
    }

    public void RollbackTransaction()
    {
        _dbTransaction.Rollback();
    }
}
