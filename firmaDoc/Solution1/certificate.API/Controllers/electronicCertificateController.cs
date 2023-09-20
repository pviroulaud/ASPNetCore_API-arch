using AutoMapper;
using certificateDTO;
using certificateEntities.certificateModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace certificateAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class electronicCertificateController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public electronicCertificateController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET electronicCertificate/5
        [HttpGet("{id}")]
        public async Task<ActionResult<readElectronicCertificateDTO>> Get(int id)
        {
            var cer = await(from c in _context.electronicCertificate
                            where
                            c.id == id 
                            select c).FirstOrDefaultAsync();
            if (cer == null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<readElectronicCertificateDTO>(cer));
        }

        // GET electronicCertificate/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<readElectronicCertificateDTO>> GetUserId(int id)
        {
            var cer = await (from c in _context.electronicCertificate
                             where 
                             c.userId == id &&
                             c.valid
                             select c).FirstOrDefaultAsync();
            if (cer == null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<readElectronicCertificateDTO>(cer));
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<readElectronicCertificateDTO>> Post(newElectronicCertificateDTO value)
        {
            electronicCertificate nCert = new electronicCertificate()
            {
                userId = value.userId,
                creationDate = DateTime.Now,
                pass= value.pass,
                expiratioDate = DateTime.Now.AddDays(value.validDays),
                valid=true
            };
            byte[] pfxBytes;
            byte[] cerBytes;
            helpers.certificate.electronicCertificate.generate(value.pass,value.userName, out pfxBytes, out cerBytes);
            nCert.pfx = Convert.ToBase64String(pfxBytes);
            nCert.cer = Convert.ToBase64String(cerBytes);


            var userCerts = (from c in _context.electronicCertificate where c.userId == value.userId select c).ToList();
            foreach (var item in userCerts)
            {
                item.valid = false;
            }
            _context.electronicCertificate.UpdateRange(userCerts);
            _context.SaveChanges();

            _context.electronicCertificate.Add(nCert);
            await _context.SaveChangesAsync();

           

            return Accepted(value);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("revokeUser/{id}")]
        public async Task<ActionResult> Put(int id)
        {
            var cer = await(from c in _context.electronicCertificate
                            where
                            c.userId == id 
                            select c).FirstOrDefaultAsync();
            if (cer == null)
            {
                return NotFound(id);
            }

            cer.valid = false;

            _context.electronicCertificate.Update(cer);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<readElectronicCertificateDTO>(cer));
        }

    }
}
