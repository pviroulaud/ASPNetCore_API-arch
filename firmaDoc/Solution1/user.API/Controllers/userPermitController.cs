using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using userEntities.userModel;
using usersDTO;

namespace usersAPI.Controllers
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

        [HttpGet("check/{userId}/{permitId}")]
        public ActionResult GetUserPermits(int userId,int permitId)
        {
            bool autorized = (from up in _context.userPermits
                           where
                           up.userId == userId
                           && up.permitId == permitId
                           && up.enabled
                           select up).Count() > 0;

            return Ok(autorized);
        }


        // GET: api/<userPermitController>
        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<permitDTO>>> GetUserPermits(int id)
        {
            var usrPerm = await (from p in _context.permit
                                 join up in _context.userPermits on p.id equals up.id
                                 where
                                 up.userId == id
                                 && up.enabled
                                 select p).ToListAsync();

            return Ok(_mapper.Map<List<permitDTO>>(usrPerm));
        }

        [HttpGet("permits")]
        public async Task<ActionResult<List<permitDTO>>> GetPermits()
        {
            var perm = await (from p in _context.permit select p).ToListAsync();

            return Ok(_mapper.Map<List<permitDTO>>(perm));
        }

        [HttpPost]
        public async Task<ActionResult<userPermitDTO>> Post(userPermitDTO value)
        {
            var usPerm = (from up in _context.userPermits
                          where
                          up.userId == value.userId &&
                          up.permitId == value.permitId
                          select up).FirstOrDefault();
            if (usPerm != null)
            {
                usPerm.enabled = value.enabled;
            }
            else
            {
                usPerm = new userPermits()
                {
                    userId = value.userId,
                    permitId = value.permitId,
                    enabled = value.enabled
                };
                _context.userPermits.Add(usPerm);
                await _context.SaveChangesAsync();
            }

            return Ok(value);
        }

    }
}
