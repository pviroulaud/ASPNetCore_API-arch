using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace helpers.cipher
{
    /// <summary>
    /// Clase para el cifrado compatible con Javascript para la comunicacion desde y hacia las API
    /// </summary>
    public class CifradoAES_JS
    {
        /* En JavaScript:
        
        function Encriptar(texto) {
            var key = CryptoJS.enc.Utf8.parse('8080808080808080');
            var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

            var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(texto), key,
                {
                    keySize: 128 / 8,
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7
                });
            return encrypted;
        }
        function DesEncriptar(cifrado) {
            var key = CryptoJS.enc.Utf8.parse('8080808080808080');
            var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

            var decrypted = CryptoJS.AES.decrypt(cifrado, key,
                {
                    keySize: 128 / 8,
                    iv: iv,
                    mode: CryptoJS.mode.CBC,
                    padding: CryptoJS.pad.Pkcs7
                });

            return decrypted.toString(CryptoJS.enc.Utf8);
        }  
            function encriptarFormulario(frmData, clave) {
            var frmDataCipher = new FormData();
            for (let kvp of frmData.entries()) {
                frmDataCipher.append(Encriptar(kvp[0], clave), Encriptar(kvp[1], clave))
            }
            return frmDataCipher;

          En el HTML:
            <script src="../src/core.js"></script>
            <script src="../src/enc-base64.js"></script>
            <script src="../src/cipher-core.js"></script>
            <script src="../src/aes.js"></script>
        */
        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("ADDOC-UCM-CIPHER");
            var iv = Encoding.UTF8.GetBytes      ("ADDOC-UCM-CIPHER");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        public static string EncryptStringAES(string text)
        {
            var keybytes = Encoding.UTF8.GetBytes("ADDOC-UCM-CIPHER");
            var iv = Encoding.UTF8.GetBytes("ADDOC-UCM-CIPHER");

            byte[] cipherBytes = EncryptStringToBytes(text, keybytes, iv);

            string cipherText = Convert.ToBase64String(cipherBytes);

            return cipherText;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }
    }

    public class Cipher
    {
        /// <summary>
        /// Encripta la cadena de texto dado utilizando clave simetrica AES.
        /// utilizando la clave de la empresa especificada (encriptacionDocumentos.clave)
        /// La clave esta encriptada con:
        /// Helpers.Encriptacion.AES.ClaveAddoc()
        /// Helpers.Encriptacion.AES.VectorDeInicializacionAddoc()
        /// </summary>
        /// <param name="empresaPrincipalID"></param>
        /// <param name="cadenaDeTexto"></param>
        /// <returns></returns>
        public static string EncriptarValor(string clavePlain, string cadenaDeTexto)
        {
            //encriptacionDocumentos claveEmpresa = (from c in _context.encriptacionDocumentos where c.empresaPrincipalID == empresaPrincipalID select c).FirstOrDefault();
            string archivo = cadenaDeTexto;
            if (clavePlain != "")
            {

                byte[] clavePlainArr = System.Text.Encoding.ASCII.GetBytes(clavePlain); // se obtiene la clave en formato byte[]
                byte[] archivoEncriptadoArr = AES.EncryptStringToBytes_Aes(archivo,
                                                                            clavePlainArr,
                                                                            AES.VectorDeInicializacionAddoc()); // Se encripta el documento
                archivo = Convert.ToBase64String(archivoEncriptadoArr);// Se obtiene el base64 del documento encriptado
            }
            return archivo;
        }
        /// <summary>
        /// Desencripta la cadena de texto dado utilizando clave simetrica AES.
        /// utilizando la clave de la empresa especificada (encriptacionDocumentos.clave)
        /// La clave esta encriptada con:
        /// Helpers.Encriptacion.AES.ClaveAddoc()
        /// Helpers.Encriptacion.AES.VectorDeInicializacionAddoc()
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="empresaPrincipalID"></param>
        /// <param name="cadenaDeTexto"></param>
        /// <returns></returns>
        public static string DesencriptarValor(string clavePlain, string cadenaDeTexto)
        {
            //encriptacionDocumentos claveEmpresa = (from c in _context.encriptacionDocumentos where c.empresaPrincipalID == empresaPrincipalID select c).FirstOrDefault();
            string archivo = cadenaDeTexto;

            if (clavePlain != "")
            {

                byte[] clavePlainArr = System.Text.Encoding.ASCII.GetBytes(clavePlain); // se obtiene la clave en formato byte[]

                byte[] documentoEncriptado = Convert.FromBase64String(cadenaDeTexto);


                archivo = AES.DecryptStringFromBytes_Aes(documentoEncriptado,
                                                        clavePlainArr,
                                                        AES.VectorDeInicializacionAddoc());


            }

            return archivo;
        }

        /// <summary>
        /// Encripta la cadena de texto dado utilizando clave simetrica AES.
        /// utilizando la clave de la empresa especificada (encriptacionDocumentos.clave)
        /// La clave esta encriptada con:
        /// Helpers.Encriptacion.AES.ClaveAddoc()
        /// Helpers.Encriptacion.AES.VectorDeInicializacionAddoc()
        /// </summary>
        /// <param name="empresaPrincipalID"></param>
        /// <param name="cadenaDeTexto"></param>
        /// <returns></returns>
        public static byte[] EncriptarValorBytes(string clavePlain, string cadenaDeTexto)
        {
            
            string archivo = cadenaDeTexto;
            if (clavePlain != "")
            {

                byte[] clavePlainArr = System.Text.Encoding.ASCII.GetBytes(clavePlain); // se obtiene la clave en formato byte[]
                byte[] archivoEncriptadoArr = AES.EncryptStringToBytes_Aes(archivo,
                                                                            clavePlainArr,
                                                                            AES.VectorDeInicializacionAddoc()); // Se encripta el documento
                return archivoEncriptadoArr;
            }
            else
            {
                return Encoding.ASCII.GetBytes(archivo);
            }
            
        }
        /// <summary>
        /// Desencripta la cadena de texto dado utilizando clave simetrica AES.
        /// utilizando la clave de la empresa especificada (encriptacionDocumentos.clave)
        /// La clave esta encriptada con:
        /// Helpers.Encriptacion.AES.ClaveAddoc()
        /// Helpers.Encriptacion.AES.VectorDeInicializacionAddoc()
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="empresaPrincipalID"></param>
        /// <param name="cadenaDeTexto"></param>
        /// <returns></returns>
        public static string DesencriptarValorBytes(string clavePlain, byte[] cadenaDeTexto)
        {

            if (clavePlain != "")
            {

                byte[] clavePlainArr = System.Text.Encoding.ASCII.GetBytes(clavePlain); // se obtiene la clave en formato byte[]

                var archivo = AES.DecryptStringFromBytes_Aes(cadenaDeTexto,
                                                            clavePlainArr,
                                                            AES.VectorDeInicializacionAddoc());

                return archivo;
            }

            return Encoding.ASCII.GetString( cadenaDeTexto);
        }

    }
    public class AES
    {
        /// <summary>
        /// Encripta el texto dado utilizando la clave indicada y el vector de inicializacion Addoc (Helpers.Encriptacion.AES.VectorDeInicializacionAddoc())
        /// </summary>
        /// <param name="clave">clave de encriptacion se utilizan 32 bytes (se completa con * si faltan)</param>
        /// <param name="cadenaDeTexto">texto a encriptar</param>
        /// <returns>Devuelve un base64String</returns>
        public static string EncriptarValor(string clave, string cadenaDeTexto)
        {
            string archivo = cadenaDeTexto;

            byte[] clavePlainArr = GenerarVectorDeBytes(clave, 32);

            byte[] archivoEncriptadoArr = EncryptStringToBytes_Aes(archivo,
                                                                   clavePlainArr,
                                                                   VectorDeInicializacionAddoc()); // Se encripta el documento
            archivo = Convert.ToBase64String(archivoEncriptadoArr);// Se obtiene el base64 del documento encriptado
            return archivo;
        }


        public static string DesencriptarValor(string textoEncriptado_base64, string clave)
        {
            string archivo = textoEncriptado_base64;

            byte[] clavePlainArr = GenerarVectorDeBytes(clave, 32); // se obtiene la clave en formato byte[]

            byte[] documentoEncriptado = Convert.FromBase64String(textoEncriptado_base64);


            archivo = AES.DecryptStringFromBytes_Aes(documentoEncriptado,
                                                    clavePlainArr,
                                                    VectorDeInicializacionAddoc());
            return archivo;
        }


        public static byte[] VectorDeInicializacionAddoc()
        {
            return GenerarVectorDeBytes("Addoc+AdeA+ContractManager", 16);
        }

        public static byte[] ClaveAddoc()
        {
            return GenerarVectorDeBytes("CifrarContrasenias+Addoc+AdeA+ContractManager", 32);
        }

        /// <summary>
        /// Adapta la clave especificada a una longitud de 32 bytes, completando con * los bytes faltantes.
        /// si no se especifica una clave se utiliza una por defecto.
        /// </summary>
        /// <param name="clave"></param>
        /// <returns></returns>
        public static string ClaveDocumentos(string clave)
        {
            if (clave=="")
            {
                clave = "CifrarDocumentos+Addoc+AdeA+ContractManager";
            }
            return System.Text.Encoding.ASCII.GetString(GenerarVectorDeBytes(clave, 32));
        }

        /// <summary>
        /// Genera un vector de inicializacion de 16 bytes a partir del texto especificado
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        private static byte[] GenerarVectorDeBytes(string texto,int longitud)
        {

            texto = texto.Replace('-', '+');
            texto = texto.Replace('_', '+');
            if (texto.Length < longitud)
            {
                for (int n=0;n<longitud;n++)
                {
                    texto += "*";
                }  
            }
            texto = texto.Substring(0, longitud);
            return System.Text.Encoding.ASCII.GetBytes(texto);
        }


        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES con el vector de inicializacion dado y genera una clave aleatoria
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave de 32 bytes</param>
        /// <param name="IV">vector de inicializacion de 16 bytes</param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText,out byte[] Key, byte[] IV)
        {
            if (IV.Length != 16)
            {
                throw new Exception("La clave debe ser de 32 bits");
            }
            Aes myAes = Aes.Create();
            Key = myAes.Key;
            

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }



        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES con la clave dada y genera el vector de inicializacion
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave de 32 bytes</param>
        /// <param name="IV">vector de inicializacion</param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, out byte[] IV)
        {
            if (Key.Length!=32)
            {
                throw new Exception("La clave debe ser de 32 bits");
            }
            Aes myAes = Aes.Create();
            
            IV = myAes.IV;

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES devuelve la clave y el vector de inicializacion generados de forma aleatoria
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave</param>
        /// <param name="IV">vector de inicializacion </param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText,out byte[] Key,out byte[] IV)
        {
            Aes myAes = Aes.Create();
            Key = myAes.Key;
            IV = myAes.IV;

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        /// <summary>
        /// Encripta la cadena de texto utilizando encriptacion simetrica AES con la clave y el vector de inicializacion dado
        /// </summary>
        /// <param name="plainText">Caden ade texto a encriptar</param>
        /// <param name="Key">clave</param>
        /// <param name="IV">vector de inicializacion (debe ser de longitud Key.Lenght/2)</param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        /// <summary>
        /// Desencripta el byte[] utilizando la clave y el vector de inicializacion dado.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
