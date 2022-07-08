using Jay.Core.Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Helpers
{
    public static class Stuff
    {
        public static string EncryptSHA256(string pass)
        {
            //Create a SHA256
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));

                //Convert To String
                StringBuilder builder = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string SetDate(DateTime fecha)
        {
            long time = (fecha.Second) + (fecha.Minute * 60) + (fecha.Hour * 60 * 60)
            + (fecha.Day * 24 * 60 * 60) + (fecha.Month * 30 * 24 * 60 * 60)
            + (fecha.Year * 365 * 24 * 60 * 60);

            DateTime now = DateTime.Now;
            long present = (now.Second) + (now.Minute * 60) + (now.Hour * 60 * 60)
            + (now.Day * 24 * 60 * 60) + (now.Month * 30 * 24 * 60 * 60)
            + (now.Year * 365 * 24 * 60 * 60);

            long plazo = present - time;

            if (plazo < 59)
            {
                return plazo == 1 ? "Hace 1 segundo" : "Hace " + plazo + " segundos";
            }
            else if (plazo < (60 * 60))
            {
                return plazo < 120 ? "Hace 1 minuto" : "Hace " + (int)(plazo / 60) + " minutos";
            }
            else if (plazo < (24 * 60 * 60))
            {
                return plazo < (2 * 60 * 60) ? "Hace 1 hora" : "Hace " + (int)(plazo / (60 * 60)) + " horas";
            }
            else if (plazo < (7 * 24 * 60 * 60))
            {
                return plazo < (2 * 24 * 60 * 60) ? "Hace 1 dia" : "Hace " + (int)(plazo / (24 * 60 * 60)) + " dias";
            }
            else if (plazo < (30 * 24 * 60 * 60))
            {
                return plazo < (2 * 7 * 24 * 60 * 60) ? "Hace 1 semana" : "Hace " + (int)(plazo / (7 * 24 * 60 * 60)) + " semanas";
            }
            else if (plazo < (365 * 24 * 60 * 60))
            {
                return plazo < (2 * 30 * 24 * 60 * 60) ? "Hace 1 mes" : "Hace " + (int)(plazo / (30 * 24 * 60 * 60)) + " meses";
            }
            else
            {
                return plazo < (2 * 365 * 24 * 60 * 60) ? "Hace 1 año" : "Hace " + (int)(plazo / (365 * 24 * 60 * 60)) + " años";
            }
        }
    }
}
