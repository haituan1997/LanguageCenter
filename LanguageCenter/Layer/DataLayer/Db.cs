using LanguageCenter.DataLayer.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LanguageCenter.DataLayer
{
    public static class Db
    {
        // Note: Static initializers are thread safe.
        // If this class had a static constructor then these static variables 
        // would need to be initialized there.
        private static readonly string DataProvider = ConfigurationManager.AppSettings.Get("DataProvider");
        private static readonly DbProviderFactory Factory = DbProviderFactories.GetFactory(DataProvider);

        #region Fast data readers

        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="make">The make.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static T Read<T>(string procedure, Func<IDataReader, T> make, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);  // Extension method

                    connection.Open();

                    var t = default(T);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                        t = make(reader);

                    return t;
                }
            }
        }

        /// <summary>
        /// Fast read of list of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="make">The make.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> ReadList<T>(string procedure, Func<IDataReader, T> make, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);
                    command.CommandTimeout = 60;

                    connection.Open();

                    var list = new List<T>();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        list.Add(make(reader));

                    return list;
                }
            }
        }

        public static Tuple<List<T>, object[]> ReadListWithOutputParam<T>(string procedure, Func<IDataReader, T> make, string connectionString, object[] parms = null, IList<object> outputParms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParametersWithOutputParam(parms, outputParms);
                    command.CommandTimeout = 60;

                    connection.Open();

                    var list = new List<T>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(make(reader));
                        }
                    }

                    var obj = new object[outputParms.Count / 2];
                    for (var i = 0; i < outputParms.Count; i += 2)
                    {
                        var name = outputParms[i].ToString();
                        var value = command.Parameters[name].Value;
                        obj[i] = value;
                    }

                    return Tuple.Create(list, obj);
                }
            }
        }

        //Load data báo cáo động cột
        /// <summary>
        /// Reads the data table.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ReadDataTable(string procedure, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);

                    connection.Open();

                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    return dt;
                }
            }
        }

        /// <summary>
        /// Reads the data table to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">The procedure.</param>
        /// <param name="make">The make.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> ReadDataTableToList<T>(string procedure, Func<IDataReader, T> make, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);

                    connection.Open();

                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    var list = new List<T>();
                    DataTableReader reader = dt.CreateDataReader();
                    while (reader.Read())
                        list.Add(make(reader));

                    return list;
                }
            }
        }

        public static T ReadDataAndMapToObject<T>(string procedure, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);  // Extension method

                    connection.Open();

                    var t = default(T);

                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    foreach (DataRow row in dt.Rows)
                    {
                        if (dt.Rows.IndexOf(row) == 0)
                        {
                            t = GetItem<T>(row);
                            break;
                        }
                    }

                    return t;
                }
            }
        }

        public static Tuple<List<T>, object[]> ReadDataTableToListByEntityAndOutput<T>(string procedure, string connectionString, object[] parms = null, IList<object> outputParms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParametersWithOutputParam(parms, outputParms);

                    connection.Open();

                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    List<T> data = new List<T>();

                    foreach (DataRow row in dt.Rows)
                    {
                        T item = GetItem<T>(row);
                        data.Add(item);
                    }

                    var obj = new object[outputParms.Count / 2];
                    for (var i = 0; i < outputParms.Count; i += 2)
                    {
                        var name = outputParms[i].ToString();
                        var value = command.Parameters[name].Value;
                        obj[i] = value;
                    }

                    return Tuple.Create(data, obj);
                }
            }
        }

        public static List<T> ReadDataTableToListByEntity<T>(string procedure, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);

                    connection.Open();

                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    List<T> data = new List<T>();

                    foreach (DataRow row in dt.Rows)
                    {
                        T item = GetItem<T>(row);
                        data.Add(item);
                    }

                    return data;
                }
            }
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                        if (!string.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        else
                            continue;
                }
            }
            return obj;
        }
        /// <summary>
        /// Gets a record count.
        /// </summary>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int GetCount(string procedure, string connectionString, object[] parms = null)
        {
            return GetScalar(procedure, connectionString).AsInt();
        }

        /// <summary>
        /// Gets any scalar value from the database.
        /// </summary>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="isStoredProcedure">if set to <c>true</c> [is stored procedure].</param>
        /// <returns>System.Object.</returns>
        public static object GetScalar(string procedure, string connectionString, object[] parms = null, bool isStoredProcedure = true)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    command.CommandText = procedure;
                    command.SetParameters(parms);
                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }


        /// <summary>
        /// Gets the sequence.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>System.Object.</returns>
        public static object GetSequence(string sequence, string connectionString)
        {
            return GetScalar(AppendSequenceSelect(sequence), connectionString, null, false);
        }

        /// <summary>
        /// Appends the sequence select.
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        private static string AppendSequenceSelect(string sequence)
        {
            switch (DataProvider)
            {
                // Microsoft Access does not support multi statement batch commands
                case "System.Data.OleDb":
                    return sequence;
                case "System.Data.SqlClient":
                    return "SELECT NEXT VALUE FOR " + sequence;
                default:
                    return "SELECT NEXT VALUE FOR " + sequence;
            }
        }

        #endregion

        #region Data update section

        /// <summary>
        /// Inserts an item into the database
        /// </summary>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static object Insert(string procedure, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.SetParameters(parms);                     // Extension method  
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        public static object Update(string procedure, string connectionString, object[] parms = null)
        {
            using (var connection = Factory.CreateConnection())
            {
                Debug.Assert(connection != null, "connection != null");
                connection.ConnectionString = connectionString;

                using (var command = Factory.CreateCommand())
                {
                    Debug.Assert(command != null, "command != null");
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = procedure;
                    command.SetParameters(parms);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="parms">The parms.</param>
        public static object Delete(string procedure, string connectionString, object[] parms = null)
        {
            return Update(procedure, connectionString, parms);
        }

        #endregion

        #region Extension methods

        /// <summary>
        /// Extention method. Adds query parameters to command object.
        /// </summary>
        /// <param name="command">Command object.</param>
        /// <param name="parms">Array of name-value query parameters.</param>
        private static void SetParameters(this DbCommand command, IList<object> parms)
        {
            if (parms == null || parms.Count <= 0) return;
            // NOTE: Processes a name/value pair at each iteration
            for (var i = 0; i < parms.Count; i += 2)
            {
                var name = parms[i].ToString();

                // No empty strings to the database
                var s = parms[i + 1] as string;
                if (s != null && s == "")
                    parms[i + 1] = null;

                // If null, set to DbNull
                var value = parms[i + 1] ?? DBNull.Value;

                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = name;
                dbParameter.Value = value;

                command.Parameters.Add(dbParameter);
            }
        }

        private static void SetParametersWithOutputParam(this DbCommand command, IList<object> parms, IList<object> outputParms)
        {
            if (parms == null || parms.Count <= 0) return;
            // NOTE: Processes a name/value pair at each iteration
            for (var i = 0; i < parms.Count; i += 2)
            {
                var name = parms[i].ToString();

                // No empty strings to the database
                var s = parms[i + 1] as string;
                if (s != null && s == "")
                    parms[i + 1] = null;

                // If null, set to DbNull
                var value = parms[i + 1] ?? DBNull.Value;

                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = name;
                dbParameter.Value = value;

                command.Parameters.Add(dbParameter);
            }

            for (var i = 0; i < outputParms.Count; i += 2)
            {
                var name = outputParms[i].ToString();
                var dbType = (DbType)outputParms[i + 1];

                var outPutParameter = command.CreateParameter();
                outPutParameter.ParameterName = name;
                outPutParameter.Direction = ParameterDirection.Output;
                outPutParameter.DbType = dbType;

                command.Parameters.Add(outPutParameter);
            }
        }


        #endregion
    }
}