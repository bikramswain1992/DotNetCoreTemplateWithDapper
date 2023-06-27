using System.Data;

namespace TemplateDapper.Application.Interfaces;

public interface IGenericRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<int> CreateAsync(TEntity request, CancellationToken cancellationToken, IDbTransaction dbTransaction = default!);
    Task<int> UpdateAsync(TEntity request, CancellationToken cancellationToken, IDbTransaction dbTransaction = default!);
    Task<int> DeleteAsync(int id, CancellationToken cancellationToken, IDbTransaction dbTransaction = default!);
}
