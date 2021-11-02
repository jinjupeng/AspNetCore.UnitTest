using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.UnitTest.Api.Models.Request
{
    public class UnitTestRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserName2 { get; set; }
    }
}
