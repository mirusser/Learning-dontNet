using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticsearchDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public UserController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpGet("{name}")]
        public async Task<User> Get(string name)
        {
            var response = await _elasticClient
                .SearchAsync<User>(s => s
                .Index("users")
                .Query(q => 
                    q.Term(t => t.Name, name) || 
                    q.Match(m => m.Field(f => f.Name).Query(name))));

            return response?.Documents?.FirstOrDefault();
        }

        [HttpGet("{id}")]
        public async Task<User> DirectGet(string id)
        {
            var response = await _elasticClient
                .GetAsync<User>(
                    new DocumentPath<User>(new Id(id)),
                    x => x.Index("users"));

            return response?.Source;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] User value)
        {
            var response = await _elasticClient
                .IndexAsync<User>(value, x => x.Index("users"));

            return response.Id;
        }
    }
}
