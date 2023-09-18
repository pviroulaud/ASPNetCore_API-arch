using AutoMapper;
using fileDTO;
using fileEntities.fileModel;
using helpers.cipher;
using helpers.validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace file.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class userFileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public userFileController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET api/<tabla3Controller>/5
        [HttpGet("{id}")]
        public ActionResult<userFileDTO> Get([Required(ErrorMessage = textDecorator.campoRequerido),
                                                                Range(1, int.MaxValue, ErrorMessage = textDecorator.campoMayorQue0)]int id)
        {
            var arch = (from a in _context.userFile
                        where a.id == id
                        select a).FirstOrDefault();

            if (arch != null)
            {
                var file = _mapper.Map<userFileDTO>(arch);
                if (!arch.cipher)
                {
                    return file;
                }
                else
                {
                    // desencriptar archivo

                    var b64Enc = Cipher.DesencriptarValorBytes(AES.ClaveDocumentos("999"), arch.content);
                    file.content = Convert.FromBase64String(b64Enc);

                    return file;
                }
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]

        public ActionResult uploadArchivo([Required(ErrorMessage = textDecorator.campoRequerido)] bool cipher)
        {

            //Variable que retorna el valor del resultado del metodo
            //El valor predeterminado es Falso (false)
            bool resultado = false;

            //La variable "file" recibe el archivo en el objeto Request.Form
            //Del POST que realiza la aplicacion a este servicio.
            //Se envia un formulario completo donde uno de los valores es el archivo
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return BadRequest();
            }
            //foreach (var file in Request.Form.Files)
            //{
            //}


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

            userFile nuevo = new userFile();
            //Se valida si la variable "file" tiene algun archivo
            if (file.Length > 0)
            {

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

                    //Como se cargo correctamente el archivo
                    //la variable "resultado" llena el valor "true"
                    resultado = true;
                }
                if (resultado)
                {
                    // El array de bytes no se carga directamente desde memoria porque la funcionalidad FILESTREAM no lo soporta. 
                    // Debe ser guardada de forma temporal en un archivo fisico y luego ser cargada en la entidad
                    FileStream fs = new FileStream(RutaFullCompleta, FileMode.Open);
                    byte[] arr = new byte[fs.Length];
                    fs.Read(arr, 0, (int)fs.Length);

                    if (cipher)
                    {
                        string b64 = Convert.ToBase64String(arr);
                        arr = Cipher.EncriptarValorBytes(AES.ClaveDocumentos("999"), b64);
                    }

                    nuevo.fileName = file.FileName;
                    nuevo.size = Convert.ToInt32(file.Length);
                    nuevo.content = arr;
                    nuevo.contentType = file.ContentType;
                    nuevo.cipher = cipher;

                    fs.Close();

                    _context.userFile.Add(nuevo);
                    _context.SaveChanges();


                }

                if (System.IO.File.Exists(RutaFullCompleta))
                {
                    System.IO.File.Delete(RutaFullCompleta);
                }
                return Accepted(nuevo.id);
            }
            else
            {
                return BadRequest();

            }

        }


    }
}
