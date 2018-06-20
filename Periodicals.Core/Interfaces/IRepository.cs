using Periodicals.Core.SharedKernel;
using System.Collections.Generic;

namespace Periodicals.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        List<T> List();
        T Add(T entity);
        void Update(T entity);
        void Delete(int entityId);
    }
}
