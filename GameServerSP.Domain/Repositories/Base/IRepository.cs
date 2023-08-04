using GameServerSP.Domain.Entities.Base;

namespace GameServerSP.Domain.Repositories.Base;

public interface IRepository<T, TId> where T : EntityBase<TId>
{
    Task<T> GetByIdAsync(TId id);
}
