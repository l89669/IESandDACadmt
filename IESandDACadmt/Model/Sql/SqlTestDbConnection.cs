using System;
using System.Data;
using System.Data.SqlClient;
using IESandDACadmt.Model.Logging;
using IESandDACadmt.ViewModel;

namespace IESandDACadmt.Model.Sql
{
    class SqlTestDbConnection
    {
        private DbSqlSpController _liveDbSqlSpController;
        private Model.Logging.ILogging _theLogger;

        public SqlTestDbConnection(DbSqlSpController theDbSqlSpController, Model.Logging.ILogging theLogger)
        {
            _liveDbSqlSpController = theDbSqlSpController;
            _theLogger = theLogger;
        }

        public void TestDbConnection()
        {
            _liveDbSqlSpController.DbSqlSpControllerData.OperationResult = "";
            _liveDbSqlSpController.DbSqlSpControllerData.DbTestStillRunning = true;
            ActionOutcome theResult = new ActionOutcome();
            SqlConnection dbConnection = new SqlConnection(_liveDbSqlSpController.DbSqlSpControllerData.SqlConnectionString);
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
                _liveDbSqlSpController.DbSqlSpControllerData.OperationResult = theResult.Message;
                _liveDbSqlSpController.DbSqlSpControllerData.DbTestStillRunning = false;
            }
        }

        public void ReadSqlUserComputerData()
        {
            bool userRead = false;
            bool computerRead = false;

            // 1. Check if USer Creds has DBowner or SysAdmin
            bool isSysAdmin = Sql.SqlAccessChecks.IsUserInThisSqlRole(_liveDbSqlSpController.DbSqlSpControllerData.SqlConnectionString, 1, "sysadmin");
            bool isDbowner = Sql.SqlAccessChecks.IsUserInThisSqlRole(_liveDbSqlSpController.DbSqlSpControllerData.SqlConnectionString, 1, "db_owner");

            if (isDbowner || isSysAdmin)
            {
                // 2. Check if Log User table exists
                ActionOutcome logTableTestResult = new ActionOutcome();
                switch (_liveDbSqlSpController.DbSqlSpControllerData.HeatServerType)
                {
                    case DbSqlSpControllerData.ServerType.UNKNOWN:
                        break;
                    case DbSqlSpControllerData.ServerType.EMSS:
                        logTableTestResult = Sql.QuerySqlServer.RunSqlQueryScalar(_liveDbSqlSpController.DbSqlSpControllerData.SqlConnectionString, 2, "SELECT COUNT(*) FROM dbo.LogUser");
                        break;
                    case DbSqlSpControllerData.ServerType.ES:
                        logTableTestResult = Sql.QuerySqlServer.RunSqlQueryScalar(_liveDbSqlSpController.DbSqlSpControllerData.SqlConnectionString, 2, "SELECT COUNT(*) FROM ActivityLog.[User]");
                        break;
                    default:
                        break;
                }
                if (logTableTestResult.Success)
                {
                    // 3. Read in th eUSer and Computers to the _liveDbSqlSpController
                    SqlConnection dbConnection = new SqlConnection(_liveDbSqlSpController.DbSqlSpControllerData.SqlConnectionString);
                    ReadInUsersFromSql(ref userRead, dbConnection, _liveDbSqlSpController);
                    ReadInComputersFromSql(ref computerRead, dbConnection, _liveDbSqlSpController);
                    if (userRead && computerRead)
                    {
                        _liveDbSqlSpController.DbSqlSpControllerData.OperationResult = "success";
                        _liveDbSqlSpController.DbSqlSpControllerData.DbTestStillRunning = false;
                    }
                }
                else
                {
                    _liveDbSqlSpController.DbSqlSpControllerData.OperationResult = "Could not read the Log Users table from the specified server/database so cannot proceed further. Please recheck the server/database name you specified.";
                    _liveDbSqlSpController.DbSqlSpControllerData.DbTestStillRunning = false;
                }
            }
            else
            {
                _liveDbSqlSpController.DbSqlSpControllerData.OperationResult = "Could not detect SysAdmin or DB_Owner access to the specified server/database so cannot proceed further. Please recheck the user credentials used for this tool.";
                _liveDbSqlSpController.DbSqlSpControllerData.DbTestStillRunning = false;
            }
        }

        
        private void ReadInComputersFromSql(ref bool computerRead, SqlConnection sqlDbConnection, DbSqlSpController liveDbSqlSpController)
        {
            try
            {
                sqlDbConnection.Open();
                _theLogger.SaveEventToLogFile(" SQL connection to read Computer names OPEN.");
                SqlCommand computerListCommand = new SqlCommand(liveDbSqlSpController.DbSqlSpControllerData.ComputerReadSqlCode,
                    sqlDbConnection) {CommandTimeout = 0};
                liveDbSqlSpController.DbSqlSpControllerData.DtComputerNameEpsguid.Load(computerListCommand.ExecuteReader());
                if (liveDbSqlSpController.DbSqlSpControllerData.DtComputerNameEpsguid.Rows.Count >= 1)
                {
                    foreach (DataRow row in liveDbSqlSpController.DbSqlSpControllerData.DtComputerNameEpsguid.Rows)
                    {
                        string combinedNameEpsguid = "";
                        if (liveDbSqlSpController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.EMSS)
                        {
                            combinedNameEpsguid = row["ComputerName"].ToString() + ":" + row["EPSGUID"].ToString();
                        }
                        if (liveDbSqlSpController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.ES)
                        {
                            combinedNameEpsguid = row["ComputerName"].ToString() + ":" + row["ComputerID"].ToString();
                        }
                        
                        liveDbSqlSpController.DbSqlSpControllerData.ComputerList.Add(combinedNameEpsguid);
                    }
                }
                _theLogger.SaveEventToLogFile(" SQL reading of Computer names is finished.");
                computerListCommand.Dispose();
                computerRead = true;
                sqlDbConnection.Close();
            }
            catch (Exception ex)
            {
                computerRead = false;
                _theLogger.SaveErrorToLogFile(" " + ex.Message.ToString());
                liveDbSqlSpController.DbSqlSpControllerData.OperationResult = ex.Message.ToString();
                sqlDbConnection.Close();
            }
        }

        private void ReadInUsersFromSql(ref bool userRead, SqlConnection sqlDbConnection, DbSqlSpController liveDbSqlSpController)
        {
            try
            {
                sqlDbConnection.Open();
                _theLogger.SaveEventToLogFile(" SQL connection to read User-names is OPEN.");
                SqlCommand userListCommand = new SqlCommand(liveDbSqlSpController.DbSqlSpControllerData.UserReadSqlCode, sqlDbConnection);
                userListCommand.CommandTimeout = 0;
                liveDbSqlSpController.DbSqlSpControllerData.DtUserNameSid.Load(userListCommand.ExecuteReader());
                if (liveDbSqlSpController.DbSqlSpControllerData.DtUserNameSid.Rows.Count >= 1)
                {
                    foreach (DataRow row in liveDbSqlSpController.DbSqlSpControllerData.DtUserNameSid.Rows)
                    {
                        string combinedNameUsersid = row["NTUserName"].ToString() + ":" + row["UserSID"].ToString();
                        liveDbSqlSpController.DbSqlSpControllerData.UserList.Add(combinedNameUsersid);
                    }
                }
                _theLogger.SaveEventToLogFile(" SQL reading of User-names is finished.");
                userListCommand.Dispose();
                userRead = true;
                sqlDbConnection.Close();
            }
            catch (Exception ex)
            {
                userRead = false;
                _theLogger.SaveErrorToLogFile(ex.Message.ToString());
                liveDbSqlSpController.DbSqlSpControllerData.OperationResult = " Error with SQL connection and/or reading: " + ex.Message.ToString();
                sqlDbConnection.Close();
            }
        }

    }
}
