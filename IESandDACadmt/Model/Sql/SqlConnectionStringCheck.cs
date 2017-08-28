using System.Data.SqlClient;
using System.Data.OleDb;

namespace IESandDACadmt.Model.Sql
{
    public static class SqlConnectionStringCheck
    {
        
        public static bool CheckForSqlServerString(string dbConnStringRegLocation, string dbConnStringRegWowLocation, string dbConnStringRegItem, DbSqlSpController theLiveData,
                                                    Model.Logging.ILogging theLogger)
        {
            Model.Logging.ActionOutcome regSqlConnSearchResult = Model.RegistryReader.ReadRegistryLocalMachineString(dbConnStringRegLocation, dbConnStringRegItem);
            if (regSqlConnSearchResult.Success == false)
            {
                Model.Logging.ActionOutcome regWowSqlConnSearchResult = Model.RegistryReader.ReadRegistryLocalMachineString(dbConnStringRegWowLocation, dbConnStringRegItem);
                if (regWowSqlConnSearchResult.Success == false)
                {
                    theLiveData.DbSqlSpControllerData.SqlConnectionStringFound = false;
                    theLogger.SaveEventToLogFile("Problem reading SQL Server connection string from registry:" + regSqlConnSearchResult.Message + ". " + regWowSqlConnSearchResult.Message);
                    theLogger.SaveEventToLogFile("Will trigger manual user-input request.");
                }
                else
                {
                    theLiveData.DbSqlSpControllerData.SqlConnectionStringFound = true;
                    theLiveData.DbSqlSpControllerData.SqlConnectionString = regWowSqlConnSearchResult.Message;
                    ParseSqlConnStringIntoServerAndDatabase(theLiveData);
                }
            }
            else
            {
                theLiveData.DbSqlSpControllerData.SqlConnectionStringFound = true;
                theLiveData.DbSqlSpControllerData.SqlConnectionString = regSqlConnSearchResult.Message;
                ParseSqlConnStringIntoServerAndDatabase(theLiveData);
            }
            return theLiveData.DbSqlSpControllerData.SqlConnectionStringFound;
        }

        private static void ParseSqlConnStringIntoServerAndDatabase(DbSqlSpController theLiveData)
        {
            if (theLiveData.DbSqlSpControllerData.SqlConnectionString.Contains("sqloledb"))
            {
                OleDbConnectionStringBuilder _oleDbConnectionString = new OleDbConnectionStringBuilder(theLiveData.DbSqlSpControllerData.SqlConnectionString);
                theLiveData.DbSqlSpControllerData.DbServeraddress = _oleDbConnectionString.DataSource.ToString();
                theLiveData.DbSqlSpControllerData.DataBaseName = _oleDbConnectionString["Initial Catalog"].ToString();
            }
            else
            {
                SqlConnectionStringBuilder _sqlConnectionString = new SqlConnectionStringBuilder(theLiveData.DbSqlSpControllerData.SqlConnectionString);
                theLiveData.DbSqlSpControllerData.DbServeraddress = _sqlConnectionString.DataSource.ToString();
                theLiveData.DbSqlSpControllerData.DataBaseName = _sqlConnectionString.InitialCatalog.ToString();
            }
        }
    }
}
