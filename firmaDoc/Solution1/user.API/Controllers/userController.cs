using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using userEntities.userModel;
using usersDTO;

namespace users.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public userController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<userDTO>>> Get()
        {
            var usr = await (from u in _context.user select u).ToListAsync();


            return Ok(_mapper.Map<List<userDTO>>(usr));
        }


        // GET electronicCertificate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<userDTO>> Get(int id)
        {
            var usr = await (from u in _context.user
                             where
                             u.id == id
                             select u).FirstOrDefaultAsync();
            if (usr == null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<userDTO>(usr));
        }

      

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<newUserDTO>> Post(newUserDTO value)
        {
            user usr = _mapper.Map<user>(value);

            _context.user.Add(usr);
            _context.SaveChanges();

            return Accepted(value);
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<ActionResult> Put(userDTO value)
        {
            var usr = await (from u in _context.user
                             where
                             u.id == value.id
                             select u).FirstOrDefaultAsync();
            if (usr == null)
            {
                return NotFound(value);
            }
            usr.email = value.email;
            usr.nick=value.nick;

            _context.user.Update(usr);
            _context.SaveChanges();


            return Ok(_mapper.Map<newUserDTO>(usr));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var usr = await (from u in _context.user
                             where
                             u.id == id
                             select u).FirstOrDefaultAsync();
            if (usr == null)
            {
                return NotFound(id);
            }

            var usPerm = from up in _context.userPermits where up.userId==id select up;
            _context.userPermits.RemoveRange(usPerm);
            _context.SaveChanges();

            _context.user.Remove(usr);
            _context.SaveChanges();

            return Ok();
        }
    }

}
