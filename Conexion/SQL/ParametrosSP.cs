using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConexionBaseDatos.Conexion.SQL
{
    /// <summary>
    /// Esta clase carga los parametros nesesarios para la ejecucion de un Sp y tambien permite cargar una consulta sql 
    /// </summary>
    public class ParametrosSP
    {
        #region Constructor

        public ParametrosSP() { }

        public ParametrosSP(string _sp, string query) { Sp = _sp; Query = query; }

        #endregion

        #region Propiedades

        /// <summary>
        /// nombre del Store Procedure que se usara 
        /// </summary>
        public string Sp { set; get; } = string.Empty;

        /// <summary>
        /// Consulta de sql a ejecutar
        /// </summary>
        public string Query { set; get; } = string.Empty;

        /// <summary>
        /// Default @opc
        /// </summary>
        public string OpcVarCRUD { set; get; } = "@opc";

        /// <summary>
        /// Listado de parametros 
        /// </summary>
        public List<SqlParameter> Parametros = new List<SqlParameter>();

        #endregion

        #region Metodos

        /// <summary>
        /// Verifica si el nombre del parametro es valido para usar
        /// </summary>
        /// <param name="key">Nombre del parametro</param>
        /// <param name="outKey">Regresa el nombre del parametro con el formato adecuado</param>
        /// <returns>Indica true en caso de que el valor de "key" sea un valor no vacio ni nulo, en caso contrario regresa false</returns>
        private bool validKey(string key, out string outKey)
        {
            outKey = key;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key)) { return false; }
            else
            {
                if (!key.Contains("@")) { outKey = string.Concat("@", key); }
                return true;
            }
        }

        /// <summary>
        /// Permite agregar parametros
        /// </summary>
        /// <param name="key">Nombre del parametro</param>
        /// <param name="tipoDato">Enumerador SqlDbType que indica el tipo de dato</param>
        /// <param name="val">Valor que se agrega al parametro</param>
        public void AddParametro(string key, SqlDbType tipoDato, object val)
        {
            string outKey = string.Empty;
            if (validKey(key, out outKey))
            {
                SqlParameter par = new SqlParameter(outKey, tipoDato);
                par.Value = val;
                Parametros.Add(par);
            }
        }

        /// <summary>
        /// Permite agregar el valor para la opcion del sp "@opc"
        /// </summary>
        /// <param name="val">Numero de la opcion </param>
        public void AddParametro(object val)
        {
            string outKey = string.Empty;
            if (validKey(OpcVarCRUD, out outKey))
            {
                SqlParameter par = new SqlParameter(outKey, val);
                Parametros.Add(par);
            }
        }

        /// <summary>
        /// Permite agregar cualquier parametro al sp
        /// </summary>
        /// <param name="key">Nombre del parametro</param>
        /// <param name="val">Valor del parametro</param>
        public void AddParametro(string key, object val)
        {
            string outKey = string.Empty;
            if (validKey(key, out outKey))
            {
                SqlParameter par = new SqlParameter(outKey, val);
                Parametros.Add(par);
            }
        }

        /// <summary>
        /// Permite agregar imagenes a la base de datos
        /// </summary>
        /// <param name="key">Nombre del parametro</param>
        /// <param name="img">Imagen en un arreglo de byte</param>
        public void AddParametro(string key, byte[] img)
        {
            string outKey = string.Empty;
            if (validKey(key, out outKey))
            {
                SqlParameter param = new SqlParameter(outKey, SqlDbType.Image);
                param.Value = img;
                Parametros.Add(param);
            }
        }

        #endregion

    }
}
