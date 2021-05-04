using LanguageCenter.DataLayer.Object;
using LanguageCenter.DataLayer.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.DataLayer.SqlServer
{
    public class SqlServerUser
    {

        private const string SequenceUser= "[dbo].[Seq_User_UserID]";
        /// <summary>
        /// Gets the training class identifier.
        /// </summary>
        /// <returns>System.Int64.</returns>

        public long GetRefDetailId()
        {
            return (long)ForeignLanguageCenterAdapter.GetSequence(SequenceUser);
        }

        public User Get_Users(string userName, string Pass)
        {
            const string procedure = "uspGet_User";
            object[] parms = { "@UserName", userName,"@PassWord", Pass };
            return ForeignLanguageCenterAdapter.Read(procedure,Make, parms);
        }


        



        private static readonly Func<IDataReader, User> Make = reader =>
           new User
           {
               UserID = reader["UserID"].AsLong(),
               Username = reader["Username"].AsString(),
               Password = reader["Password"].AsString(),
               Email = reader["Email"].AsString(),
               Role = reader["Role"].AsInt(),
               Phone = reader["Phone"].AsString(),
               FullName = reader["FullName"].AsString(),
               TypeUser = reader["TypeUser"].AsShort(),

           };
    }
}