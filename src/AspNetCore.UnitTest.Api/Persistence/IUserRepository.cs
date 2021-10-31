using AspNetCore.UnitTest.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Persistence
{
    public interface IUserRepository : IRepositoryBase<Users>
    {
        Task<List<Users>> GetUsers();

        Task<List<Users>> GetUsersByProcedure();

        Task<int> AddUser(Users entity);

        Task DeleteUser(int d);

        Task<Users> GetUserDetail(int id);

        Task<Users> GetUserDetailByEmail(string email);
    }
}
