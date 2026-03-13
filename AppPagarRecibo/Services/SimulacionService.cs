using System;

namespace AppPagarRecibo.Services
{
    public class SimulacionService
    {
        public string ExtraerUltimosDigitos(string numeroTarjeta)
        {
            if (string.IsNullOrEmpty(numeroTarjeta) || numeroTarjeta.Length < 4)
                return null;
            var limpio = numeroTarjeta.Replace(" ", "");
            return limpio.Substring(limpio.Length - 4);
        }

        public string GenerarReferencia()
        {
            var random = new Random();
            var timestamp = DateTime.Now.ToString("yyMMdd");
            var aleatorio = random.Next(1000, 9999);
            return $"{timestamp}{aleatorio}";
        }

        public string GenerarCodigoConfirmacion()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public string GenerarConceptoSPEI(string nombre, string matricula)
        {
            return $"{nombre.ToUpper()} - {matricula.ToUpper()}";
        }

        public string EnmascararTarjeta(string ultimosDigitos)
        {
            return $"**** **** **** {ultimosDigitos}";
        }
    }
}
