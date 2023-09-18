using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace helpers.cipher
{
    public class JWT
    {
        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO 
        public static string GenerarTokenJWT(Dictionary<string, string> ClaimsInfo,
                                            int ID, string correoElectronico,
                                            IConfiguration _configuration,
                                            int horasExpiracion, int minutosExpiracion)
        {
            DateTime expiracion = System.DateTime.UtcNow.AddHours(horasExpiracion).AddMinutes(minutosExpiracion);


            // CREAMOS EL HEADER 
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"]));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _Header = new JwtHeader(_signingCredentials);



            //// CREAMOS LOS CLAIMS // 
            List<Claim> cls = new List<Claim>();
            string jti = System.Guid.NewGuid().ToString();
            cls.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
            cls.Add(new Claim(JwtRegisteredClaimNames.NameId, ID.ToString()));
            foreach (var informacion in ClaimsInfo)
            {
                cls.Add(new Claim(informacion.Key.ToString(), informacion.Value.ToString()));
            }
            cls.Add(new Claim(JwtRegisteredClaimNames.Email, correoElectronico));


            var _Claims = cls.ToArray();


            // CREAMOS EL PAYLOAD // 
            var _Payload = new JwtPayload(issuer: _configuration["JWT:Issuer"],
                                            audience: _configuration["JWT:Audience"],
                                            claims: _Claims,
                                            notBefore: System.DateTime.UtcNow,
                                            expires: expiracion);
            
            // GENERAMOS EL TOKEN // 
            var _Token = new JwtSecurityToken(_Header, _Payload);
            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }


        public static string GenerarTokenJWT(Claim[] _Claims, int ID, string correoElectronico, IConfiguration _configuration, int horasExpiracion, int minutosExpiracion)
        {
            DateTime expiracion = System.DateTime.UtcNow.AddHours(horasExpiracion).AddMinutes(minutosExpiracion);

            // CREAMOS EL HEADER 
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ClaveSecreta"]));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _Header = new JwtHeader(_signingCredentials);

            string jti = System.Guid.NewGuid().ToString();
            List<Claim> cls = _Claims.ToList();
            cls.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
            cls.Add(new Claim(JwtRegisteredClaimNames.NameId, ID.ToString()));
            try
            {
                cls.Add(new Claim("Id", ID.ToString()));

            }
            catch (Exception)
            {            }            
           

            var _clms = cls.ToArray();

            // CREAMOS EL PAYLOAD // 
            var _Payload = new JwtPayload(issuer: _configuration["JWT:Issuer"],
                                            audience: _configuration["JWT:Audience"],
                                            claims: _clms,
                                            notBefore: System.DateTime.UtcNow,
                                            expires: expiracion);
            
            // GENERAMOS EL TOKEN // 
            var _Token = new JwtSecurityToken(_Header, _Payload);
            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }


        public static Dictionary<string, string> getInformacionToken(string JWT_Token)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();

            //var token = "[encoded jwt]";
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(JWT_Token);
            if (JWT_Token != "")
            {
                foreach (var item in jwtSecurityToken.Claims.ToList())
                {
                    try
                    {
                        info.Add(item.Type, item.Value.ToString());
                    }
                    catch (Exception)
                    {

                    }

                }
            }


            return info;

        }

        public static Dictionary<string, string> getInformacionToken(System.Security.Principal.IIdentity identidad)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            if (identidad != null)
            {
                var id = identidad as ClaimsIdentity;
                if (id != null)
                {

                    foreach (var item in id.Claims.ToList())
                    {
                        try
                        {
                            info.Add(item.Type, item.Value.ToString());
                        }
                        catch (Exception )
                        {

                        }

                    }
                }
            }
            return info;

        }
    }
}
