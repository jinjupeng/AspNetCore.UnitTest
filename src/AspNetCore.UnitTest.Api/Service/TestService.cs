using AspNetCore.UnitTest.Api.Models;
using AspNetCore.UnitTest.Api.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Service
{
    public interface IUserIdProvider
    {
        string GetUserId();
    }

    public class TestService
    {
        private readonly IRepository _repository;

        public TestService(IRepository repository)
        {
            _repository = repository;
        }

        public int Version
        {
            get => _repository.Version;
            set => _repository.Version = value;
        }

        public List<TestModel> GetList() => _repository.GetList();

        public TResult GetResult<TResult>(string sql) => _repository.GetResult<TResult>(sql);

        public int GetResult(string sql) => _repository.GetResult<int>(sql);

        public int GetNum<T>() => _repository.GetNum<T>();

        public int GetCount() => _repository.GetCount();

        public Task<int> GetCountAsync() => _repository.GetCountAsync();

        public TestModel GetById(int id) => _repository.GetById(id);

        public bool Delete(TestModel model) => _repository.Delete(model.Id);
    }
}
