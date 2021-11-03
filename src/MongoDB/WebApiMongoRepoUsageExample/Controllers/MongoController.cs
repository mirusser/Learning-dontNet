using System.Threading;
using System.Threading.Tasks;
using DocumentsExampleLib.Documents;
using Microsoft.AspNetCore.Mvc;
using MongoDbRepository.Repository;

namespace WebApiMongoRepoUsageExample.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MongoController : ControllerBase
    {
        private readonly IMongoRepository<IconDocument> _mongoRepository;

        public MongoController(IMongoRepository<IconDocument> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IconDocument document, CancellationToken canellationToken = default)
        {
            await _mongoRepository.CreateOneAsync(document);

            return Ok(document);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id, CancellationToken cancellation = default)
        {
            return Ok(await _mongoRepository.FindOneAsync(x => x.Id == id, cancellation));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellation = default)
        {
            return Ok(await _mongoRepository.GetAllAsync(cancellation));
        }
    }
}