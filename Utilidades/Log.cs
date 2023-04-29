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
        public Log() { _ruta = Environment.CurrentDirectory; }

        public Log(string rutaLog) { _ruta = rutaLog; }

        private static string _ruta = Environment.CurrentDirectory;

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
        public static string RutaLog { get { return string.Concat(_ruta, "\\Log ", DateTime.Now.ToString("yyyy-MM-dd"), ".txt"); } }

        /// <summary>
        /// Metodo que guarda el registro dentro del archivo log
        /// </summary>
        /// <param name="mensaje">Objeto que guarda el mensaje a guardar</param>
        public static void guardarLog(MensajeLog mensaje)
        {
            if (!File.Exists(RutaLog)) { File.Create(RutaLog); }

            using (StreamWriter log = File.AppendText(RutaLog))
            {
                log.WriteLine(string.Format("[{0}] -- Mensaje: {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), mensaje.ToString()));
            }
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
