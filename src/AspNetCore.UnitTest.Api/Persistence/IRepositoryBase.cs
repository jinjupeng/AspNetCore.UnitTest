using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Persistence
{
    public interface IRepositoryBase<T>
    {
        Task<int> Insert(T entity, string insertSql);

        Task Update(T entity, string updateSql);

        Task Delete(int Id, string deleteSql);

        Task<List<T>> Select(string selectSql);

        Task<T> Detail(int Id, string detailSql);
    }
}
