using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace ConexionBaseDatos.Utilidades
{
    public class Log
    {
        public enum TipoError
        {
            Mensaje = 1, Advertencia = 2, Error = 3
        }

        public static string RutaLog { get { return string.Concat(Environment.CurrentDirectory,"\\Log ",DateTime.Now.ToString("yyyy-MM-dd"),".txt"); } }

        public static void guardarLog(MensajeLog mensaje)
        {
            StreamWriter log;
            FileStream fileStream = null;

            string tmp = RutaLog;
            if (!File.Exists(tmp)) { File.Create(tmp); }
            fileStream = new FileStream(tmp, FileMode.Append);
            log = new StreamWriter(fileStream);
            log.WriteLine(string.Format("[{0}] -- Mensaje: {1}",DateTime.Now.ToShortTimeString(),mensaje.ToString()));
            log.Flush();
            log.Close();
        }

        public class MensajeLog
        {
            public int TipoError { set; get; }

            public string Mensaje { set; get; }

            public string Error { set; get; }

            public override string ToString()
            {
                return JsonSerializer.Serialize<MensajeLog>(this);
            }
        }

    }
}
