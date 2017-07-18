using System;
using System.Data;
using System.Data.SqlClient;
using Lumension_Advanced_DB_Maintenance.Data;
using Lumension_Advanced_DB_Maintenance.Logging;

namespace Lumension_Advanced_DB_Maintenance.Sql
{
    class SqlTestDbConnection
    {
        private DbSqlSpController _liveDbSqlSpController = null;

        public SqlTestDbConnection(DbSqlSpController liveDbSqlSpController)
        {
            _liveDbSqlSpController = liveDbSqlSpController;
        }

        public void TestDbConnection()
        {
            _liveDbSqlSpController.OperationResult = "";
            _liveDbSqlSpController.DbTestStillRunning = true;
            ActionOutcome theResult = new ActionOutcome();
            SqlConnection dbConnection = new SqlConnection(_liveDbSqlSpController.SqlConnectionString);
            try
            {
                dbConnection.Open();
                dbConnection.Close();
                theResult.Success = true;
            }
            catch (InvalidOperationException ex)
            {
                theResult.Success = false;
                theResult.Message = ex.Message;
            }
            catch (SqlException ex)
            {
                theResult.Success = false;
                theResult.Message = ex.Message;
            }

            if (theResult.Success)
            {
                ReadSqlUserComputerData();
            }
            else
            {
                _liveDbSqlSpController.OperationResult = theResult.Message;
                _liveDbSqlSpController.DbTestStillRunning = false;
            }
        }

        public void ReadSqlUserComputerData()
        {
            bool userRead = false;
            bool computerRead = false;

            // 1. Check if USer Creds has DBowner or SysAdmin
            bool isSysAdmin = Sql.SqlAccessChecks.IsUserInThisSqlRole(_liveDbSqlSpController.SqlConnectionString, 1, "sysadmin");
            bool isDbowner = Sql.SqlAccessChecks.IsUserInThisSqlRole(_liveDbSqlSpController.SqlConnectionString, 1, "db_owner");

            if (isDbowner || isSysAdmin)
            {
                // 2. Check if Log User table exists
                ActionOutcome logTableTestResult = new ActionOutcome();
                switch (_liveDbSqlSpController.HeatServerType)
                {
                    case DbSqlSpController.ServerType.UNKNOWN:
                        break;
                    case DbSqlSpController.ServerType.EMSS:
                        logTableTestResult = Sql.QuerySqlServer.RunSqlQueryScalar(_liveDbSqlSpController.SqlConnectionString, 2, "SELECT COUNT(*) FROM dbo.LogUser");
                        break;
                    case DbSqlSpController.ServerType.ES:
                        logTableTestResult = Sql.QuerySqlServer.RunSqlQueryScalar(_liveDbSqlSpController.SqlConnectionString, 2, "SELECT COUNT(*) FROM ActivityLog.[User]");
                        break;
                    default:
                        break;
                }
                if (logTableTestResult.Success)
                {
                    // 3. Read in th eUSer and Computers to the _liveDbSqlSpController
                    SqlConnection dbConnection = new SqlConnection(_liveDbSqlSpController.SqlConnectionString);
                    ReadInUsersFromSql(ref userRead, dbConnection, _liveDbSqlSpController);
                    ReadInComputersFromSql(ref computerRead, dbConnection, _liveDbSqlSpController);
                    if (userRead && computerRead)
                    {
                        _liveDbSqlSpController.OperationResult = "success";
                        _liveDbSqlSpController.DbTestStillRunning = false;
                    }
                }
                else
                {
                    _liveDbSqlSpController.OperationResult = "Could not read the Log Users table from the specified server/database so cannot proceed further. Please recheck the server/database name you specified.";
                    _liveDbSqlSpController.DbTestStillRunning = false;
                }
            }
            else
            {
                _liveDbSqlSpController.OperationResult = "Could not detect SysAdmin or DB_Owner access to the specified server/database so cannot proceed further. Please recheck the user credentials used for this tool.";
                _liveDbSqlSpController.DbTestStillRunning = false;
            }
        }

        
        private void ReadInComputersFromSql(ref bool computerRead, SqlConnection sqlDbConnection, DbSqlSpController liveDbSqlSpController)
        {
            try
            {
                sqlDbConnection.Open();
                LoggingClass.SaveEventToLogFile( liveDbSqlSpController.LogFileLocation, " SQL connection to read Computer names OPEN.");
                SqlCommand computerListCommand = new SqlCommand(liveDbSqlSpController.ComputerReadSqlCode,
                    sqlDbConnection) {CommandTimeout = 0};
                liveDbSqlSpController.DtComputerNameEpsguid.Load(computerListCommand.ExecuteReader());
                if (liveDbSqlSpController.DtComputerNameEpsguid.Rows.Count >= 1)
                {
                    foreach (DataRow row in liveDbSqlSpController.DtComputerNameEpsguid.Rows)
                    {
                        string combinedNameEpsguid = "";
                        if (liveDbSqlSpController.HeatServerType == DbSqlSpController.ServerType.EMSS)
                        {
                            combinedNameEpsguid = row["ComputerName"].ToString() + ":" + row["EPSGUID"].ToString();
                        }
                        if (liveDbSqlSpController.HeatServerType == DbSqlSpController.ServerType.ES)
                        {
                            combinedNameEpsguid = row["ComputerName"].ToString() + ":" + row["ComputerID"].ToString();
                        }
                        
                        liveDbSqlSpController.ComputerList.Add(combinedNameEpsguid);
                    }
                }
                LoggingClass.SaveEventToLogFile(liveDbSqlSpController.LogFileLocation, " SQL reading of Computer names is finished.");
                computerListCommand.Dispose();
                computerRead = true;
                sqlDbConnection.Close();
            }
            catch (Exception ex)
            {
                computerRead = false;
                LoggingClass.SaveErrorToLogFile(liveDbSqlSpController.LogFileLocation, " " + ex.Message.ToString());
                liveDbSqlSpController.OperationResult = ex.Message.ToString();
                sqlDbConnection.Close();
            }
        }

        private void ReadInUsersFromSql(ref bool userRead, SqlConnection sqlDbConnection, DbSqlSpController liveDbSqlSpController)
        {
            try
            {
                sqlDbConnection.Open();
                LoggingClass.SaveEventToLogFile(liveDbSqlSpController.LogFileLocation, " SQL connection to read User-names is OPEN.");
                SqlCommand userListCommand = new SqlCommand(liveDbSqlSpController.UserReadSqlCode, sqlDbConnection);
                userListCommand.CommandTimeout = 0;
                liveDbSqlSpController.DtUserNameSid.Load(userListCommand.ExecuteReader());
                if (liveDbSqlSpController.DtUserNameSid.Rows.Count >= 1)
                {
                    foreach (DataRow row in liveDbSqlSpController.DtUserNameSid.Rows)
                    {
                        string combinedNameUsersid = row["NTUserName"].ToString() + ":" + row["UserSID"].ToString();
                        liveDbSqlSpController.UserList.Add(combinedNameUsersid);
                    }
                }
                LoggingClass.SaveEventToLogFile(liveDbSqlSpController.LogFileLocation, " SQL reading of User-names is finished.");
                userListCommand.Dispose();
                userRead = true;
                sqlDbConnection.Close();
            }
            catch (Exception ex)
            {
                userRead = false;
                LoggingClass.SaveErrorToLogFile(liveDbSqlSpController.LogFileLocation, ex.Message.ToString());
                liveDbSqlSpController.OperationResult = " Error with SQL connection and/or reading: " + ex.Message.ToString();
                sqlDbConnection.Close();
            }
        }

    }
}
