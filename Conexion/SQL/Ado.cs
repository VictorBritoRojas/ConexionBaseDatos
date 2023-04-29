using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConexionBaseDatos.Conexion.SQL
{
    public class Ado
    {
        #region Constructor

        public Ado(string cadenaConexion) { _cadenaConexion = cadenaConexion; }

        #endregion

        #region Propiedades

        /// <summary>
        /// Cadena de conexion
        /// </summary>
        private string _cadenaConexion = string.Empty;

        /// <summary>
        /// Propiedad que almacena los mensajes generados
        /// </summary>
        private object _msg = string.Empty;

        /// <summary>
        /// Propieda que indica si todo esta bien(true) o esta mal(false)
        /// </summary>
        private object _flag = string.Empty;

        /// <summary>
        /// Propiedad que regresa mensajes de la base de datos o en caso de existir un error mostrara ese mensaje
        /// </summary>
        public object Mensaje { get { return this._msg; } }

        /// <summary>
        /// Regresa la bandera que regresa la consulta desde la base de datos en caso contrario sera un campo vacio
        /// </summary>
        public object Bandera { get { return this._flag; } }

        #endregion

        #region Metodos

        /// <summary>
        /// Ejecuta un Sp o Query
        /// </summary>
        /// <param name="opc">Objeto ParametrosSp que contiene la informacion del sp o consulta a ejecutar</param>
        /// <param name="tipo">Especifica si se ejecutara un query o un Sp, el valor por default es CommandType.StoredProcedure</param>
        /// <returns>Regresa un DataSet con los resultados o null en caso de existir algun problema</returns>
        public DataSet obtenerDataSet(ParametrosSP opc, CommandType tipo = CommandType.StoredProcedure)
        {
            DataSet _dataSet = null;
            _msg = string.Empty;
            try
            {
                using (SqlConnection _conexion = new SqlConnection(_cadenaConexion))
                {
                    SqlCommand _comando = new SqlCommand(opc.Texto, _conexion);
                    _comando.CommandType = tipo;
                    foreach (SqlParameter it in opc.Parametros) { _comando.Parameters.Add(it); }
                    _conexion.Open();
                    SqlDataAdapter _dataAdapter = new SqlDataAdapter(_comando);
                    _dataSet = new DataSet("Table");
                    _dataAdapter.Fill(_dataSet);
                    _conexion.Close();
                }
                return _dataSet;
            }
            catch (Exception e)
            {
                _msg = e.Message;
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error durante ejecutaDataSet",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un Sp o Query
        /// </summary>
        /// <param name="opc">Objeto ParametrosSp que contiene la informacion del sp o consulta a ejecutar</param>
        /// <param name="tipo">Especifica si se ejecutara un query o un Sp, el valor por default es CommandType.StoredProcedure</param>
        /// <returns>Regresa un DataTable con la primera tabla encontrada con los resultados o null en caso de existir algun problema</returns>
        public DataTable obtenerDataTable(ParametrosSP opc, CommandType tipo = CommandType.StoredProcedure)
        {
            DataTable tbl = new DataTable();
            _msg = string.Empty;
            try
            {
                
                DataSet tblList = obtenerDataSet(opc,tipo);
                if (tblList != null)
                {
                    foreach (DataTable it in tblList.Tables)
                    {
                        tbl = it;
                    }
                }
                else { tbl = null; }
                return tbl;
            }
            catch (Exception e)
            {
                _msg = e.Message;
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error ejecutando spDataTable",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un sp PlantillaCRUD
        /// </summary>
        /// <param name="opc">Objeto ParametrosSp que contiene la informacion del sp o consulta a ejecutar</param>
        /// <returns>Regresa un DataTable con la primera tabla encontrada con los resultados o null en caso de existir algun problema</returns>
        public DataTable obtenerPlantillaCRUD(ParametrosSP opc)
        {
            DataTable tbl = new DataTable();
            _msg = string.Empty;
            try
            {
                tbl = obtenerDataTable(opc, CommandType.StoredProcedure);
                if (tbl != null)
                {
                    if (tbl.Columns.Contains("flag"))
                    {
                        _flag = tbl.Rows[0]["flag"].ToString();
                        _msg = tbl.Rows[0]["msg"].ToString();
                        if (_flag != null && _flag.ToString() == "-1")
                        {
                            tbl = null;
                        }
                    }
                }
                return tbl;
            }
            catch (Exception e)
            {
                _msg = e.Message;
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error ejecutando obtenerPlantillaCRUD",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un Sp o Query
        /// </summary>
        /// <param name="opc">Objeto ParametrosSp que contiene la informacion del sp o consulta a ejecutar</param>
        /// <param name="tipo">Especifica si se ejecutara un query o un Sp, el valor por default es CommandType.StoredProcedure</param>
        /// <returns>Regresa arraglo de cadenas que es obtenido a partir del dataReader o null en caso de existir algun problema</returns>
        public IEnumerable<string[]> ejecutaDataReader(ParametrosSP opc, CommandType tipo = CommandType.StoredProcedure)
        {
            using (SqlConnection _conexion = new SqlConnection(_cadenaConexion))
            {
                SqlCommand _comando = new SqlCommand(opc.Texto, _conexion);
                _comando.CommandType = tipo;
                foreach (SqlParameter it in opc.Parametros) { _comando.Parameters.Add(it); }
                _conexion.Open();
                SqlDataReader _reader = _comando.ExecuteReader();
                if (_reader != null)
                {
                    while (_reader.Read())
                    {
                        DataTable schemaTbl = _reader.GetSchemaTable();
                        string[] row = new string[schemaTbl.Columns.Count];
                        for (int i = 0; i < schemaTbl.Rows.Count; i++)
                        {
                            row[i] = _reader[i].ToString();
                        }
                        yield return row;
                    }
                }
                _conexion.Close();
            }
        }

        #endregion
    }
}
