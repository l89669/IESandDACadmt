using IESandDACadmt.Model.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IESandDACadmt.Model.Sql
{
    public class SqlAccessChecks
    {
        /// <summary>
        ///  Checks if the user context has rights on the direct SQL securable of securable class for the permimission specified
        /// </summary>
        /// <param name="sqlServerConnectionString"></param>
        /// <param name="sqlCommandTimeout"></param>
        /// <param name="sqlSecurable"></param>
        /// <param name="sqlSecurableClass"></param>
        /// <param name="sqlPermission"></param>
        /// <returns></returns>
        public static Model.Logging.ActionOutcome TestUserSqlObjectAccess(string sqlServerConnectionString, int sqlCommandTimeout, string sqlSecurable, string sqlPermission)
        {
            Model.Logging.ActionOutcome currentOutcome = new Model.Logging.ActionOutcome();
            currentOutcome.Success = false;
            string sqlQuery = "SELECT HAS_PERMS_BY_NAME('" + sqlSecurable + "','OBJECT','" + sqlPermission + "')";
            // Try to connect to DB
            SqlConnection dbConnection = new SqlConnection(sqlServerConnectionString);
            // Try to run query against DB
            try
            {
                dbConnection.Open();
                SqlCommand readTableCommand = new SqlCommand(sqlQuery, dbConnection);
                readTableCommand.CommandTimeout = sqlCommandTimeout * 60;
                Int32 result = (Int32)readTableCommand.ExecuteScalar();
                if (result == 1)
                {
                    currentOutcome.Success = true;
                }
            }
            catch (SqlException ex)
            {
                currentOutcome.Success = false;
                currentOutcome.Message = "User access to resource could not be verified:" + ex.Message;
            }
            return currentOutcome;
        }

        /// <summary>
        /// Return true if the current user context is a member of this role 
        /// </summary>
        /// <param name="sqlServerConnectionString"></param>
        /// <param name="sqlCommandTimeout"></param>
        /// <param name="sqlRole"></param>
        /// <returns></returns>
        public static bool IsUserInThisSqlRole(string sqlServerConnectionString, int sqlCommandTimeout, string sqlRole)
        {
            string sqlSrvRoleQuery = "SELECT IS_SRVROLEMEMBER('" + sqlRole + "')";
            string sqlRoleQuery = "SELECT IS_ROLEMEMBER('" + sqlRole + "')";
            // Try to connect to DB
            using (SqlConnection dbConnection = new SqlConnection(sqlServerConnectionString))
            {
                // Try to run query against DB
                try
                {
                    dbConnection.Open();
                    if (RunSqlRoleCheck(sqlSrvRoleQuery, dbConnection, sqlCommandTimeout))
                    {
                        return true;
                    }
                    if (RunSqlRoleCheck(sqlRoleQuery, dbConnection, sqlCommandTimeout))
                    {
                        return true;
                    }
                    dbConnection.Close();
                }
                catch (SqlException)
                {
                    if (dbConnection.State != ConnectionState.Broken || dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    return false;
                }
            }
            return false;
        }

        private static bool RunSqlRoleCheck(string theSqlRoleQuery, SqlConnection theSqlConnection, int sqlTimeout)
        {
            Int32 queryResult = 0;
            SqlCommand readTableCommand;

            readTableCommand = new SqlCommand(theSqlRoleQuery, theSqlConnection);
            readTableCommand.CommandTimeout = sqlTimeout * 60;
            try
            {
                object objQueryResult = readTableCommand.ExecuteScalar();
                queryResult = (Int32)objQueryResult;
            }
            catch (Exception)
            {
                queryResult = 0;
            }          
            if (queryResult == 1)
            {
                return true;
            }
            return false;
        }

    }
}
