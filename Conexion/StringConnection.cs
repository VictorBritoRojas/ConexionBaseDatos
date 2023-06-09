﻿using System.Collections.Generic;
using System.Text.Json;
using System.Data.SqlClient;

namespace ConexionBaseDatos.Conexion
{
    public class StringConnection
    {
        #region Constructores

        /// <summary>
        /// 
        /// </summary>
        public StringConnection() { }

        /// <summary>
        /// Constructor que permite agregar cadena de conexion
        /// </summary>
        /// <param name="cadenaConexion">Cadena de conexion que se agrega a la clase</param>
        /// <param name="nombre">Nombre de la cadena de conexion</param>
        public StringConnection(string cadenaConexion, string nombre = "")
        {
            _nombreCadena = obtenerNombre(nombre);
            _stringConnect = new SqlConnectionStringBuilder(cadenaConexion);
            _listStringConnect.Add(_nombreCadena, _stringConnect);
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Objeto que almacena la ultima cadena de conexion agregada
        /// </summary>
        private SqlConnectionStringBuilder _stringConnect = new SqlConnectionStringBuilder();

        /// <summary>
        /// Nombre de la ultima cadena de conexion agregada
        /// </summary>
        private string _nombreCadena = string.Empty;

        /// <summary>
        /// Listado de las cadenas de conexion agregadas al objeto
        /// </summary>
        private Dictionary<string, SqlConnectionStringBuilder> _listStringConnect = new Dictionary<string, SqlConnectionStringBuilder>();

        /// <summary>
        /// Regresa el listado de las cadenas de conexion
        /// </summary>
        public Dictionary<string, SqlConnectionStringBuilder> ListaCadenaConexion
        {
            get { return _listStringConnect; }
        }

        /// <summary>
        /// Regresa la ultima cadena de conexion agregada
        /// </summary>
        public string CadenaDeConexion
        {
            get { return _stringConnect.ConnectionString; }
        }

        /// <summary>
        /// Regresa el nombre de la ultima cadena de conexion agregada
        /// </summary>
        public string NombreConexion
        {
            get { return _nombreCadena; }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        private string obtenerNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrWhiteSpace(nombre))
            {
                nombre = "Conexion " + _listStringConnect.Count.ToString();
            }
            return nombre;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadena"></param>
        /// <param name="nombre"></param>
        public void agregarCadenaConexion(SqlConnectionStringBuilder cadena, string nombre = "")
        {
            if (cadena != null)
            {
                _nombreCadena = obtenerNombre(nombre);
                _stringConnect = cadena;
                _listStringConnect.Add(nombre, _stringConnect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadena"></param>
        /// <param name="nombre"></param>
        public void agregarCadenaConexion(string cadena, string nombre = "")
        {
            if (!string.IsNullOrEmpty(cadena) && !string.IsNullOrWhiteSpace(cadena))
            {
                _nombreCadena = obtenerNombre(nombre);
                _stringConnect = new SqlConnectionStringBuilder(cadena);
                _listStringConnect.Add(nombre, _stringConnect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="servidor"></param>
        /// <param name="usuario"></param>
        /// <param name="pass"></param>
        /// <param name="db"></param>
        /// <param name="nombre"></param>
        public void agregarCadenaConexion(string servidor, string usuario, string pass, string db, string nombre = "")
        {
            _nombreCadena = obtenerNombre(nombre);
            _stringConnect = new SqlConnectionStringBuilder();
            _stringConnect.InitialCatalog = db;
            _stringConnect.DataSource = servidor;
            _stringConnect.UserID = usuario;
            _stringConnect.Password = pass;
            _listStringConnect.Add(nombre, _stringConnect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreConexion"></param>
        /// <returns></returns>
        public string obtenerCadenaConexion(string nombreConexion)
        {
            if (_listStringConnect.ContainsKey(nombreConexion))
            {
                return _listStringConnect[nombreConexion].ConnectionString;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string serealizarConexiones()
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            foreach (KeyValuePair<string, SqlConnectionStringBuilder> it in _listStringConnect)
            {
                keyValues.Add(it.Key,it.Value.ConnectionString);
            }
            return JsonSerializer.Serialize<Dictionary<string, string>>(keyValues);
        }

        #endregion

    }
}
