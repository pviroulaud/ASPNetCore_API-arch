using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helpers.validations
{
    public static class cuitCuil
    {
		public static bool validar(string cuit)
		{
			if (string.IsNullOrEmpty(cuit))
			{
				return false;//	throw new ArgumentNullException(nameof(cuit)); 
			}
			//if (cuit.Length != 13) throw new ArgumentException(nameof(cuit)); sacamos los guiones
			if (cuit.Length != 11)
			{
				return false;   //throw new ArgumentException(nameof(cuit)); 
			}
			bool rv = false;
			int verificador;
			int resultado = 0;
			//string cuit_nro = cuit.Replace("-", string.Empty); sacamos los guiones
			string cuit_nro = cuit;
			string codes = "6789456789";
			long cuit_long = 0;
			if (long.TryParse(cuit_nro, out cuit_long))
			{
				verificador = int.Parse(cuit_nro[cuit_nro.Length - 1].ToString());
				int x = 0;
				while (x < 10)
				{
					int digitoValidador = int.Parse(codes.Substring((x), 1));
					int digito = int.Parse(cuit_nro.Substring((x), 1));
					int digitoValidacion = digitoValidador * digito;
					resultado += digitoValidacion;
					x++;
				}
				resultado = resultado % 11;
				rv = (resultado == verificador);
			}
			return rv;
		}
	}
}
