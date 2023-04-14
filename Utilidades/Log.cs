using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace ConexionBaseDatos.Utilidades
{
    /// <summary>
    /// Clase encargada de generar Logs por el nuget en especifico
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Enumerador que indica el tipo de registro
        /// </summary>
        public enum TipoError
        {
            Mensaje = 1, Advertencia = 2, Error = 3
        }

        /// <summary>
        /// Ruta que indica el lugar donde se guardara el archivo
        /// </summary>
        public static string RutaLog { get { return string.Concat(Environment.CurrentDirectory,"\\Log ",DateTime.Now.ToString("yyyy-MM-dd"),".txt"); } }

        /// <summary>
        /// Metodo que guarda el registro dentro del archivo log
        /// </summary>
        /// <param name="mensaje">Objeto que guarda el mensaje a guardar</param>
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

        /// <summary>
        /// Clase donde se indica el tipo de mensaje generado
        /// </summary>
        public class MensajeLog
        {
            /// <summary>
            /// Tipo de error Basado en el enumerador TipoError
            /// </summary>
            public int TipoError { set; get; }

            /// <summary>
            /// Texto que indica un mensaje personalizado 
            /// </summary>
            public string Mensaje { set; get; }

            /// <summary>
            /// Texto que indica el mensaje de error generado por una excepcion
            /// </summary>
            public string Error { set; get; }

            /// <summary>
            /// Permite serealizar la clase como un objeto JSON
            /// </summary>
            /// <returns>Cadena con la informacion de la clase en formato JSON</returns>
            public override string ToString()
            {
                return JsonSerializer.Serialize<MensajeLog>(this);
            }
        }

    }
}
