using System;
using System.Data;
using System.Data.SqlClient;
using IESandDACadmt.Model.Logging;

namespace IESandDACadmt.Model.Sql
{
    public class QuerySqlServer
    {
        // Create methods to do the follow:
        /*
         * 1. RunQueryGetSuccess
         * 2. RunQueryIntoDataTable
         * 3. TestDbConnection
         * 4. BuildSqlServerConnectionString
         */

        /// <summary>
        /// Loads result of Query into DataTable
        /// </summary>
        /// <param name="sqlServerConnectionString"></param>
        /// <param name="sqlQuery"></param>
        /// <param name="theDataTable"></param>
        /// <returns></returns>
        public static ActionOutcome RunSqlQueryIntoDataTable(string sqlServerConnectionString, int sqlCommandTimeout, string sqlQuery, DataTable theDataTable)
        {
            ActionOutcome currentOutcome = new ActionOutcome();
            currentOutcome.Success = false;
            theDataTable.Rows.Clear();
            // Try to connect to DB
            SqlConnection dbConnection = new SqlConnection(sqlServerConnectionString);
            // Try to run query against DB
            try
            {
                dbConnection.Open();
                SqlCommand readTableCommand = new SqlCommand(sqlQuery, dbConnection);
                readTableCommand.CommandTimeout = sqlCommandTimeout * 60;
                theDataTable.Load(readTableCommand.ExecuteReader());
                if (theDataTable.Rows.Count >= 0)
                {
                    currentOutcome.Success = true;
                    currentOutcome.Message = theDataTable.Rows.Count + " rows read in.";
                }
            }
            catch (Exception ex)
            {
                currentOutcome.Success = false;
                currentOutcome.Message = ex.Message;
            }
            return currentOutcome;
        }


        /// <summary>
        /// Runs the sql query and returns the number of rows affected
        /// </summary>
        /// <param name="sqlServerConnectionString"></param>
        /// <param name="sqlCommandTimeout"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static ActionOutcome RunSqlQuery(string sqlServerConnectionString, int sqlCommandTimeout, string sqlQuery)
        {
            ActionOutcome currentOutcome = new ActionOutcome();
            currentOutcome.Success = false;
            // Try to connect to DB
            SqlConnection dbConnection = new SqlConnection(sqlServerConnectionString);
            // Try to run query against DB
            try
            {
                dbConnection.Open();
                SqlCommand readTableCommand = new SqlCommand(sqlQuery, dbConnection);
                readTableCommand.CommandTimeout = sqlCommandTimeout * 60;
                Int32 result = (Int32)readTableCommand.ExecuteNonQuery();
                if (result >= 0)
                {
                    currentOutcome.Success = true;
                    currentOutcome.Message = result.ToString() + " rows affected.";
                }
                else
                {
                    currentOutcome.Success = true;
                    currentOutcome.Message = " completed with result value:" + result.ToString();
                }

            }
            catch (Exception ex)
            {
                currentOutcome.Success = false;
                currentOutcome.Message = ex.Message;
            }
            return currentOutcome;
        }

        /// <summary>
        /// Runs the sql query and returns the number of rows affected
        /// </summary>
        /// <param name="sqlServerConnectionString"></param>
        /// <param name="sqlCommandTimeout"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static ActionOutcome RunSqlQueryScalar(string sqlServerConnectionString, int sqlCommandTimeout, string sqlQuery)
        {
            ActionOutcome currentOutcome = new ActionOutcome();
            currentOutcome.Success = false;
            // Try to connect to DB
            SqlConnection dbConnection = new SqlConnection(sqlServerConnectionString);
            // Try to run query against DB
            try
            {
                dbConnection.Open();
                SqlCommand readTableCommand = new SqlCommand(sqlQuery, dbConnection);
                readTableCommand.CommandTimeout = sqlCommandTimeout * 60;
                Int32 result = (Int32)readTableCommand.ExecuteScalar();
                if (result >= 0)
                {
                    currentOutcome.Success = true;
                    currentOutcome.Message = " completed with result value:" + result.ToString();
                }
            }
            catch (Exception ex)
            {
                currentOutcome.Success = false;
                currentOutcome.Message = ex.Message;
            }
            return currentOutcome;
        }
    }
}
