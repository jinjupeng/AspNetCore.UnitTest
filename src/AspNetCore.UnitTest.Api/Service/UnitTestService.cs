using AspNetCore.UnitTest.Api.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Service
{
    public class UnitTestService
    {

        public IEnumerable<UnitTestRequest> GetUserList(string userid)
        {
            if (string.IsNullOrWhiteSpace(userid))
            {
                throw new ArgumentException("User Id Cannot be null");
            }
            var s = new List<UnitTestRequest>() {
                new UnitTestRequest{ UserName = "1", UserName2 = "2" },
                new UnitTestRequest{ UserName = "3", UserName2 = "4" }
            };
            return s;
        }
    }
}
