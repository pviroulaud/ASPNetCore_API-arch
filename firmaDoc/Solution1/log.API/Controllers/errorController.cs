using logEntities.logModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace log.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class errorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public errorController(AppDbContext context)
        {
            _context=context;

        }
    }
}
