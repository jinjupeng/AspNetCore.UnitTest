using AspNetCore.UnitTest.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Persistence
{
    public interface IRepository
    {
        int Version { get; set; }

        int GetCount();

        Task<int> GetCountAsync();

        TestModel GetById(int id);

        TestModel[] GetArray();

        List<TestModel> GetList();

        TResult GetResult<TResult>(string sql);

        int GetNum<T>();

        bool Delete(int id);
    }
}
