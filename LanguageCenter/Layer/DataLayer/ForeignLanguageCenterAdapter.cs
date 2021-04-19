using LanguageCenter.DataLayer.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.DataLayer
{
    public static class ForeignLanguageCenterAdapter
    {
        private static readonly string ConnectionString = @"Data Source=DESKTOP-RCFES7K;Initial Catalog=ForeignLanguageCenter;Integrated Security=True";

        #region Fast data readers

        /// <summary>
        /// Fast read of individual item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="make">The make.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static T Read<T>(string procedure, Func<IDataReader, T> make, object[] parms = null)
        {
            return Db.Read(procedure, make, ConnectionString, parms);
        }

        /// <summary>
        /// Fast read of list of items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="make">The make.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static List<T> ReadList<T>(string procedure, Func<IDataReader, T> make, object[] parms = null)
        {
            return Db.ReadList(procedure, make, ConnectionString, parms);
        }

        public static Tuple<List<T>, object[]> ReadListWithOutputParam<T>(string procedure, Func<IDataReader, T> make, object[] parms = null, object[] outputParms = null)
        {
            return Db.ReadListWithOutputParam(procedure, make, ConnectionString, parms, outputParms);
        }

        /// <summary>
        /// Reads the data table.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="parms">The parms.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ReadDataTable(string procedure, object[] parms = null)
        {
            return Db.ReadDataTable(procedure, ConnectionString, parms);
        }

        /// <summary>
        /// Gets a record count.
        /// </summary>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static int GetCount(string procedure, object[] parms = null)
        {
            return GetScalar(procedure, parms).AsInt();
        }

        /// <summary>
        /// Gets any scalar value from the database.
        /// </summary>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static object GetScalar(string procedure, object[] parms = null)
        {
            return Db.GetScalar(procedure, ConnectionString, parms);
        }

        /// <summary>
        ///     Gets the sequence.
        /// </summary>
        /// <param name="sequence">Name of the sequence.</param>
        /// <returns></returns>
        public static object GetSequence(string sequence)
        {
            return Db.GetSequence(sequence, ConnectionString);
        }

        #endregion

        #region Data update section

        /// <summary>
        /// Inserts an item into the database
        /// </summary>
        /// <param name="procedure">Name of the procedure.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        public static object Insert(string procedure, object[] parms = null)
        {
            return Db.Insert(procedure, ConnectionString, parms);
        }

        /// <summary>
        /// Updates an item in the database
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="parms">The parms.</param>
        public static object Update(string procedure, object[] parms = null)
        {
            return Db.Update(procedure, ConnectionString, parms);
        }

        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="procedure">The procedure.</param>
        /// <param name="parms">The parms.</param>
        public static object Delete(string procedure, object[] parms = null)
        {
            return Db.Delete(procedure, ConnectionString, parms);
        }

        #endregion
    }

}