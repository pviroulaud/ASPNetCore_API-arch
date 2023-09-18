using logEntities.logModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace log.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class operationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public operationController(AppDbContext context)
        {
            _context = context;

        }
    }
}
