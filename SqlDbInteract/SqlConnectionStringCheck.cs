using System.Data.SqlClient;
using System.Data.OleDb;

namespace Lumension_Advanced_DB_Maintenance.Sql
{
    public static class SqlConnectionStringCheck
    {
        
        public static bool CheckForSqlServerString(string dbConnStringRegLocation, string dbConnStringRegWowLocation, string dbConnStringRegItem, Data.DbSqlSpController theLiveData)
        {
            Logging.ActionOutcome regSqlConnSearchResult = BL.RegistryReader.ReadRegistryLocalMachineString(dbConnStringRegLocation, dbConnStringRegItem);
            if (regSqlConnSearchResult.Success == false)
            {
                Logging.ActionOutcome regWowSqlConnSearchResult = BL.RegistryReader.ReadRegistryLocalMachineString(dbConnStringRegWowLocation, dbConnStringRegItem);
                if (regWowSqlConnSearchResult.Success == false)
                {
                    theLiveData.SqlConnectionStringFound = false;
                    Logging.LoggingClass.SaveEventToLogFile(theLiveData.LogFileLocation,"Problem reading SQL Server connection string from registry:" + regSqlConnSearchResult.Message + ". " + regWowSqlConnSearchResult.Message);
                    Logging.LoggingClass.SaveEventToLogFile(theLiveData.LogFileLocation, "Will trigger manual user-input request.");
                }
                else
                {
                    theLiveData.SqlConnectionStringFound = true;
                    theLiveData.SqlConnectionString = regWowSqlConnSearchResult.Message;
                    ParseSqlConnStringIntoServerAndDatabase(theLiveData);
                }
            }
            else
            {
                theLiveData.SqlConnectionStringFound = true;
                theLiveData.SqlConnectionString = regSqlConnSearchResult.Message;
                ParseSqlConnStringIntoServerAndDatabase(theLiveData);
            }
            return theLiveData.SqlConnectionStringFound;
        }

        private static void ParseSqlConnStringIntoServerAndDatabase(Data.DbSqlSpController theLiveData)
        {
            if (theLiveData.SqlConnectionString.Contains("sqloledb"))
            {
                OleDbConnectionStringBuilder _oleDbConnectionString = new OleDbConnectionStringBuilder(theLiveData.SqlConnectionString);
                theLiveData.DbServeraddress = _oleDbConnectionString.DataSource.ToString();
                theLiveData.DataBaseName = _oleDbConnectionString["Initial Catalog"].ToString();
            }
            else
            {
                SqlConnectionStringBuilder _sqlConnectionString = new SqlConnectionStringBuilder(theLiveData.SqlConnectionString);
                theLiveData.DbServeraddress = _sqlConnectionString.DataSource.ToString();
                theLiveData.DataBaseName = _sqlConnectionString.InitialCatalog.ToString();
            }
        }
    }
}
