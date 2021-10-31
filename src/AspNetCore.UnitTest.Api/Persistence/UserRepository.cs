using AspNetCore.UnitTest.Api.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Persistence
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public async Task DeleteUser(int id)
        {
            string deleteSql = "DELETE FROM [dbo].[Users] WHERE Id=@Id";
            await Delete(id, deleteSql);
        }


        public async Task<Users> GetUserDetail(int id)
        {
            string detailSql = @"SELECT Id, Email, UserName, Mobile, Password, Age, Gender, CreateTime,Salt, IsDelete FROM Users WHERE Id=@Id";
            return await Detail(id, detailSql);
        }

        public async Task<Users> GetUserDetailByEmail(string email)
        {
            string detailSql = @"SELECT Id, Email, UserName, Mobile, Password, Age, Gender, CreateTime, Salt, IsDelete FROM Users WHERE Email=@email";

            using (IDbConnection conn = DataBaseConfig.GetMySqlConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<Users>(detailSql, new { email });
            }
        }

        public async Task<List<Users>> GetUsers()
        {
            string selectSql = @"SELECT * FROM Users";
            return await Select(selectSql);
        }

        public async Task<int> AddUser(Users entity)
        {
            string insertSql = @"INSERT INTO Users (UserName, Gender, Avatar, Mobile, CreateTime, Password, Salt, IsDelete, Email) VALUES (@UserName, @Gender, @Avatar, @Mobile, now(),@Password, @Salt, @IsDelete,@Email);SELECT @id= LAST_INSERT_ID();";
            return await Insert(entity, insertSql);
        }

        public async Task<List<Users>> GetUsersByProcedure()
        {
            using (IDbConnection conn = DataBaseConfig.GetMySqlConnection())
            {
                var userList = await conn.QueryAsync<Users>("QueryUsersNoParms",null, commandType: CommandType.StoredProcedure);
                return userList.ToList();
            }
        }
    }
}
