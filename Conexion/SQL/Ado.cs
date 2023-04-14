using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConexionBaseDatos.Conexion.SQL
{
    public class Ado
    {
        #region Constructor

        public Ado() { }

        public Ado(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        private SqlCommand Comando { set; get; }

        /// <summary>
        /// 
        /// </summary>
        private SqlConnection conexion { set; get; }

        /// <summary>
        /// Temporalmente sin uso
        /// </summary>
        private SqlDataReader reader;

        /// <summary>
        /// 
        /// </summary>
        private SqlDataAdapter _dataAdapter { set; get; }

        /// <summary>
        /// 
        /// </summary>
        private DataSet _dataSet { set; get; }

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
        /// metodo que abre la conexion a la base de datos
        /// </summary>
        private void abrirConexion()
        {
            conexion = new SqlConnection(_cadenaConexion);
            Comando = new SqlCommand();
            conexion.Close();
            Comando.Connection = conexion;
            conexion.Open();
        }

        /// <summary>
        /// metodo que cierra la conexion a la base de datos
        /// </summary>
        private void cerrarConexion() { conexion.Close(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opc"></param>
        /// <returns></returns>
        public DataSet spDataSet(ParametrosSP opc)
        {
            _msg = string.Empty;
            try
            {
                DataTable tbl = new DataTable();
                abrirConexion();
                Comando.CommandText = opc.Sp;
                Comando.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter it in opc.Parametros) { Comando.Parameters.Add(it); }

                _dataAdapter = new SqlDataAdapter(Comando);
                _dataSet = new DataSet("Table");
                _dataAdapter.Fill(_dataSet);
                cerrarConexion();

                return _dataSet;
            }
            catch (Exception e)
            {
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error ejecutando spDataSet",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                cerrarConexion();
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Ejecuta Sp y regresa solo el primer dataTable encontrado
        /// </summary>
        /// <param name="opc"></param>
        /// <returns></returns>
        public DataTable spDataTable(ParametrosSP opc)
        {
            _msg = string.Empty;
            try
            {
                DataTable tbl = new DataTable();
                DataSet tblList = spDataSet(opc);
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
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error ejecutando spDataTable",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                cerrarConexion();
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opc"></param>
        /// <returns></returns>
        public DataTable spPlantillaCRUD(ParametrosSP opc)
        {
            _msg = string.Empty;
            DataTable tbl = spDataTable(opc);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="opc"></param>
        /// <returns></returns>
        public DataSet queryDataSet(ParametrosSP opc)
        {
            _msg = string.Empty;
            try
            {
                abrirConexion();
                Comando.CommandText = opc.Query;
                Comando.CommandType = CommandType.Text;
                foreach (SqlParameter it in opc.Parametros) { Comando.Parameters.Add(it); }

                _dataAdapter = new SqlDataAdapter(Comando);
                _dataSet = new DataSet("Table");
                _dataAdapter.Fill(_dataSet);
                cerrarConexion();

                return _dataSet;
            }
            catch (Exception e)
            {
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error ejecutando queryDataSet",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                cerrarConexion();
                return null;
            }
            finally { GC.Collect(); }
        }

        /// <summary>
        /// Metodo que directamente ejecuta un sql especificado en el objeto parametrosSP
        /// </summary>
        /// <param name="opc"></param>
        /// <returns></returns>
        public DataTable queryDataTable(ParametrosSP opc)
        {
            _msg = string.Empty;
            try
            {
                DataTable tbl = new DataTable();
                abrirConexion();
                Comando.CommandText = opc.Query;
                Comando.CommandType = CommandType.Text;
                foreach (SqlParameter it in opc.Parametros) { Comando.Parameters.Add(it); }
                tbl.Load(Comando.ExecuteReader());
                cerrarConexion();
                return tbl;
            }
            catch (Exception e)
            {
                Utilidades.Log.MensajeLog msg = new Utilidades.Log.MensajeLog()
                {
                    TipoError = (int)Utilidades.Log.TipoError.Error,
                    Mensaje = "Error ejecutando queryDataTable",
                    Error = e.Message
                };
                Utilidades.Log.guardarLog(msg);
                cerrarConexion();
                return null;
            }
            finally { GC.Collect(); }
        }

        #endregion
    }
}
