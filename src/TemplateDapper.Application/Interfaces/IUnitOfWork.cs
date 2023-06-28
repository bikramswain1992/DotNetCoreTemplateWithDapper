using System.Data;

namespace TemplateDapper.Application.Interfaces;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    IDbTransaction BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
