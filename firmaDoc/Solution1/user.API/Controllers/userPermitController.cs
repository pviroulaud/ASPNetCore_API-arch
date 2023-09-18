using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using userEntities.userModel;

namespace users.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class userPermitController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public userPermitController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<userPermitController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<userPermitController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<userPermitController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<userPermitController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<userPermitController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
