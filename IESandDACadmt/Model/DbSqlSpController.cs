using System;

using System.Data;
using System.Data.SqlClient;
using System.Globalization;


namespace IESandDACadmt.Model
{
    public class DbSqlSpController
	{
        private ViewModel.DbSqlSpControllerData _dbSqlSpControllerData = new ViewModel.DbSqlSpControllerData();
        public ViewModel.DbSqlSpControllerData DbSqlSpControllerData
        {
            get { return _dbSqlSpControllerData; }
            //set { _dbSqlSDpControllerData = value; }
        }

        private Model.Logging.ILogging _theLogger;
        public DbSqlSpController(ViewModel.DbSqlSpControllerData theDbSqlSDpControllerData, Model.Logging.ILogging theLogger)
		{
            _dbSqlSpControllerData = theDbSqlSDpControllerData;
            _dbSqlSpControllerData.ByProcessQueryAlreadyRan = false;
            _theLogger = theLogger;
		}

        public DbSqlSpController(Model.Logging.ILogging theLogger)
        {
            _dbSqlSpControllerData.ByProcessQueryAlreadyRan = false;
            _theLogger = theLogger;
        }


        private string CalculateBatchSize()
		{
			string result = "failure";
			switch (_dbSqlSpControllerData.BatchSize.ToString().ToLower())
			{
				case "1,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 1000;
					result = "success";
					break;
				case "2,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 2000;
					result = "success";
					break;
				case "4,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 4000;
					result = "success";
					break;
				case "8,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 8000;
					result = "success";
					break;
				case "16,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 16000;
					result = "success";
					break;
				case "32,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 32000;
					result = "success";
					break;
				case "64,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 64000;
					result = "success";
					break;
				case "128,000":
                    _dbSqlSpControllerData.RecordsForBatchSize = 128000;
					result = "success";
					break;
				default:
					break;
			}
			return result;
		}

		public void BuildEventTypesDictionary()
		{
            DataTable sqlActionItems = new DataTable();
            // Connect to SQL Server and read in the .Action table to a DataTable.
            if (_dbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                Sql.QuerySqlServer.RunSqlQueryIntoDataTable(_dbSqlSpControllerData.SqlConnectionString, 0, _dbSqlSpControllerData.SqlCommandEmssPullActionTypes, sqlActionItems);   
            }
            else if (_dbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                Sql.QuerySqlServer.RunSqlQueryIntoDataTable(_dbSqlSpControllerData.SqlConnectionString, 0, _dbSqlSpControllerData.SqlCommandEsPullActionTypes, sqlActionItems);
            }

            // Parse each line into the formation "ID: NAME", then add it to _eventTypesToDelete Dict using this Logic:
            //      DEVICE-ATTACHED,GRANTED and WRITE-GRANTED get FALSE,
            //      MEDIUM-ENCRYPTED gets omitted from the Dict
            //      All others get a TRUE.
            _dbSqlSpControllerData.EventTypesToDelete.Clear();
            foreach (DataRow row in sqlActionItems.Rows)
            {
                if (row["ActionName"].ToString() != "MEDIUM-ENCRYPTED")
                {
                    if (row["ActionName"].ToString() == "DEVICE-ATTACHED" || row["ActionName"].ToString() == "GRANTED" || row["ActionName"].ToString() == "WRITE-GRANTED")
                    {
                        _dbSqlSpControllerData.EventTypesToDelete.Add(row["ActionId"].ToString() + ": " + row["ActionName"].ToString(), false);
                    }
                    else
                    {
                        _dbSqlSpControllerData.EventTypesToDelete.Add(row["ActionId"].ToString() + ": " + row["ActionName"].ToString(), true);
                    }
                }                
            }
		}

		private string CalculateProcessingEndtime()
		{
			string result = "failure";
            _dbSqlSpControllerData.ProcessingEndTime = DateTime.Now;
            _dbSqlSpControllerData.ProcessingEndTime = _dbSqlSpControllerData.ProcessingEndTime.AddMinutes(_dbSqlSpControllerData.RunTime);
			result = "success";
			return result;
		}

		public void BuildSqlConnectionString()
		{
            if (_dbSqlSpControllerData.AltCredentialsSelected == false)
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder
                {
                    ["Data Source"] = _dbSqlSpControllerData.DbServerAddress,
                    ["integrated Security"] = true,
                    ["Initial Catalog"] = _dbSqlSpControllerData.DatabaseName,
                    ["Application Name"] = _dbSqlSpControllerData.ApplicationName
                };
                _dbSqlSpControllerData.SqlConnectionString = builder.ConnectionString;
            }
            else
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.DataSource = _dbSqlSpControllerData.DbServerAddress;
                builder.IntegratedSecurity = false;
                builder.UserID = _dbSqlSpControllerData.SqlConnUserName;
                builder.Password = _dbSqlSpControllerData.SqlConnPassword;
                builder.InitialCatalog = _dbSqlSpControllerData.DatabaseName;
                _dbSqlSpControllerData.SqlConnectionString = builder.ConnectionString;
            }
            _theLogger.SaveEventToLogFile(  " SQL Server Connection String: " + _dbSqlSpControllerData.SqlConnectionString);
        }

		public string ValidateParameters()
		{
			string result = "failure";
            _theLogger.SaveEventToLogFile(  " Records Cut Off Date:" + _dbSqlSpControllerData.CutOffDate.ToString());
			result = CalculateBatchSize();
            _theLogger.SaveEventToLogFile(  " Batch Size:" + _dbSqlSpControllerData.BatchSize);
			if (result == "success")
			{
				result = CalculateProcessingEndtime();
                _theLogger.SaveEventToLogFile(  " Calculated Processing End Time:" + _dbSqlSpControllerData.ProcessingEndTime.ToString(CultureInfo.InvariantCulture));
				if (result == "success")
				{
					result = Model.RecordsDeletionQueryLogic.CreateRequiredStoredProcedures(this, _theLogger);
                    _theLogger.SaveEventToLogFile(  " SQL Stored Procedures check/creation:" + result);
				}
				else
				{
                    _theLogger.SaveErrorToLogFile(  "Calculation of Processing-Endtime failed:" + result);
				}
			}
			else
			{
                _theLogger.SaveErrorToLogFile(  "Calculation of Batch Size failed:" + result);
			}
			
			return result;
		}

		public void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
		{
            _dbSqlSpControllerData.SpCheckReturnString = e.Message.ToString();
		}
		
		public void CalculateTotalRecordsToPurge()
		{
			try
			{
				using (var conn = new SqlConnection(_dbSqlSpControllerData.SqlConnectionString))
				using (var command = new SqlCommand(_dbSqlSpControllerData.TotalRecordsCalcStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure })
				{
				    command.CommandTimeout = 0;
				    SqlParameter spReturnParam1 = new SqlParameter("@RecordCount", SqlDbType.Int, 11)
				    {
				        Direction = ParameterDirection.ReturnValue
				    };
				    command.Parameters.Add(spReturnParam1);

                    if (_dbSqlSpControllerData.CutOffDays)
                    {
                        SqlParameter spParam1 = new SqlParameter("@cutOffDate", SqlDbType.DateTime, 11) { Value = _dbSqlSpControllerData.CutOffDate };
                        command.Parameters.Add(spParam1);
                    }

                    if (_dbSqlSpControllerData.SelectedComputer != "all")
					{
					    SqlParameter spParam2 = new SqlParameter("@WorkstationId", SqlDbType.NVarChar, 255)
					    {
					        Value = _dbSqlSpControllerData.EpsGuid.ToString()
					    };
					    command.Parameters.Add(spParam2);
					    SqlParameter spParam3 = new SqlParameter("@theWorkstationName", SqlDbType.NVarChar, 255)
					    {
					        Value = _dbSqlSpControllerData.SelectedComputer.ToString()
					    };
					    command.Parameters.Add(spParam3);
					}

					if (_dbSqlSpControllerData.SelectedUser != "everyone")
					{
					    SqlParameter spParam4 = new SqlParameter("@theUserAccountSid", SqlDbType.NVarChar, 200)
					    {
					        Value = _dbSqlSpControllerData.UserSid.ToString()
					    };
					    command.Parameters.Add(spParam4);
					}

					if (_dbSqlSpControllerData.SelectedProcess != "all")
					{
					    SqlParameter spParam5 = new SqlParameter("@theProcessName", SqlDbType.NVarChar, 255)
					    {
					        Value = _dbSqlSpControllerData.SelectedProcess.ToString()
					    };
					    command.Parameters.Add(spParam5);
					}

					conn.Open();
					command.ExecuteNonQuery();
                    _dbSqlSpControllerData.ReturnedTotalRowsToPurge = Convert.ToDouble(spReturnParam1.Value);
                    _dbSqlSpControllerData.RemainingRowsToPurge = _dbSqlSpControllerData.ReturnedTotalRowsToPurge;
					conn.Close();
                    _dbSqlSpControllerData.Result.Success = true;

				}
			}
			catch (Exception ex)
			{
                _dbSqlSpControllerData.Result.Message = " Error calculating Total-Records-To-Purge:" + ex.Message.ToString();
                _dbSqlSpControllerData.Result.Success = false;
			}
		}

		public void RequestStop()
		{
			_theLogger.SaveEventToLogFile(  " SQL Worker Thread stop requested.");
            _dbSqlSpControllerData.StopController = true;
		}


	}
}
