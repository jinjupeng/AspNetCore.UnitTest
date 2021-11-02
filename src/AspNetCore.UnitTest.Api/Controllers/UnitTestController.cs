using AspNetCore.UnitTest.Api.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCore.UnitTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitTestController : ControllerBase
    {
        // GET: api/<UnitTestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UnitTestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UnitTestController>
        [HttpPost]
        public ActionResult Post([FromBody] UnitTestRequest request)
        {
            return Ok("");
        }

        // PUT api/<UnitTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UnitTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
