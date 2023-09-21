using AutoMapper;
using documentAPI.SyncServices;
using documentDTO;
using documentEntities.documentModel;
using fileDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace documentAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class userDocumentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpLocalClient _httpClient;

        public userDocumentController(AppDbContext context, IMapper mapper, IHttpLocalClient httpClient)
        {
            _context = context;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<userDocumentDTO>>> Get(int userId)
        {
            var usrDoc = await (from d in _context.document where d.userId == userId select d).ToListAsync();

            return Ok(_mapper.Map<List<userDocumentDTO>>(usrDoc));
        }

        [HttpGet("download/{fileId}/{userId}")]
        public async Task<ActionResult<userFileDTO>> GetFile(int fileId, int userId)
        {
            // Get userPermits userService usersAPI-userPermitController-check SYNC
            bool auth= await _httpClient.checkUserPermit(userId, 2);
            if (auth)
            {
                // Get file from storageservice (fileAPI) SYNC
                userFileDTO? usFile = await _httpClient.getFile(fileId);
                if (usFile != null) 
                {
                    return Ok(usFile);
                }
                else
                {
                    return NotFound(fileId);
                }
            }
            else 
            {
                return Unauthorized();
            }
           
        }

        [HttpPost]
        public async Task<ActionResult<userDocumentDTO>> Post(newUserDocumentDTO value)
        {
            // Get userPermits userService usersAPI-userPermitController-check SYNC
            bool auth = await _httpClient.checkUserPermit(value.userId, 1);
            bool authSign = true;
            if (value.sign)
            {
                authSign = await _httpClient.checkUserPermit(value.userId, 3);
            }

            if (auth && authSign)
            {

                document doc = new document()
                {
                    title = value.title,
                    userId = value.userId
                };
                _context.document.Add(doc);
                await _context.SaveChangesAsync();

                var file = Request.Form.Files.FirstOrDefault();
                if ((file == null) || (file.Length<=0))
                {
                    return BadRequest();
                }
                string fileGuid = storeTempFile(file);
                // Send RabbitMQ message to StorageService (fileAPI) ASYNC
                Console.WriteLine($">>> Send Rabbit message to sotre file documentId: {doc.id}, userId: {doc.userId}, cipherFile: {value.storeCipher}, fileGUID: {fileGuid}, requireSign: {value.sign}");

            }
            else
            {
                return Unauthorized();
            }


            return Accepted(value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileGuid"></param>
        private void deleteTempFile(string fileGuid)
        {
            var file = (from f in _context.rabbitMqTempFile where f.guid == fileGuid select f).ToList();
            if (file != null) 
            {
                _context.rabbitMqTempFile.RemoveRange(file);
                _context.SaveChanges();
            }
        }

        private string storeTempFile(IFormFile file)
        {

            //Variable donde se coloca la ruta relativa de la carpeta de destino
            //del archivo cargado
            string NombreCarpeta = "/Archivos/";

            //Variable donde se coloca la ruta raíz de la aplicacion
            //para esto se emplea la variable "_env" antes de declarada
            string RutaRaiz = AppDomain.CurrentDomain.BaseDirectory;// _env.ContentRootPath;

            //Se concatena las variables "RutaRaiz" y "NombreCarpeta"
            //en una otra variable "RutaCompleta"
            string RutaCompleta = RutaRaiz + NombreCarpeta;


            //Se valida con la variable "RutaCompleta" si existe dicha carpeta            
            if (!Directory.Exists(RutaCompleta))
            {
                //En caso de no existir se crea esa carpeta
                Directory.CreateDirectory(RutaCompleta);
            }

            rabbitMqTempFile nuevo = new rabbitMqTempFile();            
                    
            var provider = new FileExtensionContentTypeProvider();


            //Se declara en esta variable el nombre del archivo cargado
            string NombreArchivo = file.FileName;

            string guidTemporal = Guid.NewGuid().ToString() + ".tmp";
            //Se declara en esta variable la ruta completa con el nombre del archivo
            string RutaFullCompleta = Path.Combine(RutaCompleta, guidTemporal);

            //Se crea una variable FileStream para carlo en la ruta definida
            using (var stream = new FileStream(RutaFullCompleta, FileMode.Create))
            {

                file.CopyTo(stream);

                FileStream fs = new FileStream(RutaFullCompleta, FileMode.Open);
                byte[] arr = new byte[fs.Length];
                fs.Read(arr, 0, (int)fs.Length);

                nuevo.guid = Guid.NewGuid().ToString();
                nuevo.fileName = file.FileName;
                nuevo.size = Convert.ToInt32(file.Length);
                nuevo.content = arr;
                nuevo.contentType = file.ContentType;

                fs.Close();

                _context.rabbitMqTempFile.Add(nuevo);
                _context.SaveChanges();
            }


            if (System.IO.File.Exists(RutaFullCompleta))
            {
                System.IO.File.Delete(RutaFullCompleta);
            }
            return nuevo.guid;
        }

    }
}
