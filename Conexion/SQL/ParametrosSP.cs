using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConexionBaseDatos.Conexion.SQL
{
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
        /// <param name="k"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tipoDato"></param>
        /// <param name="val"></param>
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
        /// permite agregar cualquier parametro al sp
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
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
        /// <param name="key"></param>
        /// <param name="img"></param>
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
