using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Lumension_Advanced_DB_Maintenance.Logging;


namespace Lumension_Advanced_DB_Maintenance.Data
{
    public class DbSqlSpController
	{
		public DbSqlSpController()
		{
			_byProcessQueryAlreadyRan = false;
		}

        public enum ServerType
        {
            UNKNOWN = 0,
            EMSS,
            ES
        };

        public volatile bool StopController = false;

		public DataTable DtComputerNameEpsguid = new DataTable();
		public DataTable DtUserNameSid = new DataTable();

        private string _applicationName = "Heat Support Adv DB Maintenance Tool";
		private string _dbServerAddress = @"server\instance";
		private string _selectedUser = "everyone";
		private string _selectedComputer = "all";
		private string _batchSize = "small";
		private int _runTime = 1;
		private bool _cutOffDays = false;
		private bool _workerPurgerComplete = false;
		private List<string> _userList = new List<string>();
		private List<string> _computerList = new List<string>();

        private string _logFileLocation = System.IO.Directory.GetCurrentDirectory() + @"\Heat_EMSS_ES_DB_Maintenance_Tool.log";

        public string LogFileLocation
        {
            get { return _logFileLocation; }
            set { _logFileLocation = value; }
        }

    
        private string _spCheckReturnString = "";
        public string SpCheckReturnString
        {
            get { return _spCheckReturnString; }
            set { _spCheckReturnString = value; }
        }

        private double _returnedTotalRowsToPurge = 0;
		private double _remainingRowsToPurge = 0;
		private int _recordsForBatchSize = 50;
		private DateTime _processingEndTime = new DateTime();
		private volatile string _batchRunResults = "";

        private string _recordDeletionStoredProcedureName = "sp_DbMaintenanceTool_PurgeLogEntries";
        public string RecordDeletionStoredProcedureName
        {
            get { return _recordDeletionStoredProcedureName; }
        }

        private string _totalRecordsCalcStoredProcedureName = "sp_DbMaintenanceTool_CalculateRecordsToPurge";
        public string TotalRecordsCalcStoredProcedureName
        {
            get { return _totalRecordsCalcStoredProcedureName; }
            set { _totalRecordsCalcStoredProcedureName = value; }
        }

        private string _sqlCommandEsPullActionTypes = "SELECT ActionId, ActionName FROM [ActivityLog].[Action] ORDER BY ActionId";
        private string _sqlCommandEmssPullActionTypes = "SELECT ActionId, ActionName FROM dbo.LogAction ORDER BY ActionId";

        private ServerType _heatServerType;

        public ServerType HeatServerType
        {
            get { return _heatServerType; }
            set { _heatServerType = value; }
        }

        private List<string> _byProcessQueryResults = new List<string>();

		public List<string> ByProcessResults
		{
			get { return _byProcessQueryResults; }
			set { _byProcessQueryResults = value; }
		}

		private bool _byProcessQueryAlreadyRan;

		public bool ByProcessQueryAlreadyRan
		{
			get { return _byProcessQueryAlreadyRan; }
			set { _byProcessQueryAlreadyRan = value; }
		}

		private bool _byProcessQuerySuccess;

		public bool ByProcessQuerySuccess
		{
			get { return _byProcessQuerySuccess; }
			set { _byProcessQuerySuccess = value; }
		}

		private string _byProcessQueryMessage;

		public string ByProcessQueryMessage
		{
			get { return _byProcessQueryMessage; }
			set { _byProcessQueryMessage = value; }
		}

		private DataTable _byProcessQueryDataTable = new DataTable();

		public DataTable ByProcessQueryDataTable
		{
			get { return _byProcessQueryDataTable; }
			set { _byProcessQueryDataTable = value; }
		}

		private Logging.ActionOutcome _result = new Logging.ActionOutcome();

		public Logging.ActionOutcome Result
		{
			get { return _result; }
			set { _result = value; }
		}

		private string _eventsToExclude;

		public string EventsToExclude
		{
			get { return _eventsToExclude; }
			set { _eventsToExclude = value; }
		}
		
		private volatile string _operationResult;

		public string OperationResult
		{
			get { return _operationResult; }
			set { _operationResult = value; }
		}

		private bool _dbTestStillRunning;

		public bool DbTestStillRunning
		{
			get { return _dbTestStillRunning; }
			set { _dbTestStillRunning = value; }
		}
		
		private string _computerReadSqlCode = "";
		public string ComputerReadSqlCode
		{
			get { return _computerReadSqlCode; }
			set { _computerReadSqlCode = value; }
		}

		private string _userReadSqlCode = "";
		public string UserReadSqlCode
		{
			get { return _userReadSqlCode; }
			set { _userReadSqlCode = value; }
		}

        private string _databaseName;
		private string _epsGuid;
		private string _userSid;
		private double _recordsProcessedSoFar;

		public double RecordsProcessedSoFar
		{
			get { return _recordsProcessedSoFar; }
			set { _recordsProcessedSoFar = value; }
		}

		public string UserSid
		{
			get { return _userSid; }
			set { _userSid = value; }
		}

		public string EpsGuid
		{
			get { return _epsGuid; }
			set { _epsGuid = value; }
		}

		public string DataBaseName
		{
			get { return _databaseName; }
			set { _databaseName = value; }
		}
		
        private bool _altCredentialsSelected = false;

        public bool AltCredentialsSelected
        {
            get { return _altCredentialsSelected; }
            set { _altCredentialsSelected = value; }
        }

        private string _sqlConnUserName = "";

        public string SqlConnUserName
        {
            get { return _sqlConnUserName; }
            set { _sqlConnUserName = value; }
        }

        private string _sqlConnPassword = "";

        public string SqlConnPassword
        {
            get { return _sqlConnPassword; }
            set { _sqlConnPassword = value; }
        }

        private string _sqlConnectionString;

		public string SqlConnectionString
		{
			get { return _sqlConnectionString; }
			set { _sqlConnectionString = value; }
		}

        private bool _sqlConnectionStringFound;

        public bool SqlConnectionStringFound
        {
            get { return _sqlConnectionStringFound; }
            set { _sqlConnectionStringFound = value; }
        }

        public string BatchRunResults
		{
			get { return _batchRunResults; }
			set { _batchRunResults = value; }
		}

		public DateTime ProcessingEndTime
		{
			get { return _processingEndTime; }
		}

		public double ReturnedTotalRowsToPurge
		{
			get { return _returnedTotalRowsToPurge; }
			set { _returnedTotalRowsToPurge = value; }
		}

		public int RecordsForBatchSize
		{
			get { return _recordsForBatchSize; }
		}

		public double RemainingRowsToPurge
		{
			get { return _remainingRowsToPurge; }
			set { _remainingRowsToPurge = value; }
		}

		public List<string> UserList
		{
			get { return _userList; }
		}

		public List<string> ComputerList
		{
			get { return _computerList; }
		}

		public bool WorkerCompleted
		{
			get { return _workerPurgerComplete; }
			set { _workerPurgerComplete = value; }
		}

		public string DbServeraddress
		{
			get { return _dbServerAddress; }
			set
			{
				if (!(String.IsNullOrEmpty(value)))
				{
					_dbServerAddress = value;
				}
			}
		}

		public string SelectedUser
		{
			get { return _selectedUser; }
			set 
			{
				if (!(String.IsNullOrEmpty(value)))
				{
					_selectedUser = value;
				}
			}
		}

		public string SelectedComputer
		{
			get { return _selectedComputer; }
			set
			{
				if (!(String.IsNullOrEmpty(value)))
				{
					_selectedComputer = value;
				}
			}
		}

		private string _selectedProcess;

		public string SelectedProcess
		{
			get { return _selectedProcess; }
			set 
			{ 
				if (!(String.IsNullOrEmpty(value)))
				{
					_selectedProcess = value;
				} 
			}
		}
		
		public string BatchSize
		{
			get { return _batchSize; }
			set
			{
				if (!(String.IsNullOrEmpty(value)))
				{
					_batchSize = value;
				}
			}
		}

		public int RunTime
		{
			get { return _runTime; }
			set { _runTime = value; }
		}

		public bool CutOffDays
		{
			get { return _cutOffDays; }
			set { _cutOffDays = value; }
		}

        private DateTime _cutOffDate;

        public DateTime CutOffDate
        {
            get { return _cutOffDate; }
            set { _cutOffDate = value; }
        }


        private Dictionary<string, bool> _eventTypesToDelete = new Dictionary<string,bool>();

		public Dictionary<string, bool> EventTypesToDelete
		{
			get { return _eventTypesToDelete; }
			set { _eventTypesToDelete = value; }
		}

		private string CalculateBatchSize()
		{
			string result = "failure";
			switch (_batchSize.ToString().ToLower())
			{
				case "1,000":
					_recordsForBatchSize = 1000;
					result = "success";
					break;
				case "2,000":
					_recordsForBatchSize = 2000;
					result = "success";
					break;
				case "4,000":
					_recordsForBatchSize = 4000;
					result = "success";
					break;
				case "8,000":
					_recordsForBatchSize = 8000;
					result = "success";
					break;
				case "16,000":
					_recordsForBatchSize = 16000;
					result = "success";
					break;
				case "32,000":
					_recordsForBatchSize = 32000;
					result = "success";
					break;
				case "64,000":
					_recordsForBatchSize = 64000;
					result = "success";
					break;
				case "128,000":
					_recordsForBatchSize = 128000;
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
            if (_heatServerType == ServerType.EMSS)
            {
                Sql.QuerySqlServer.RunSqlQueryIntoDataTable(_sqlConnectionString, 0, _sqlCommandEmssPullActionTypes, sqlActionItems);   
            }
            else if (_heatServerType == ServerType.ES)
            {
                Sql.QuerySqlServer.RunSqlQueryIntoDataTable(_sqlConnectionString, 0, _sqlCommandEsPullActionTypes, sqlActionItems);
            }

            // Parse each line into the formation "ID: NAME", then add it to _eventTypesToDelete Dict using this Logic:
            //      DEVICE-ATTACHED,GRANTED and WRITE-GRANTED get FALSE,
            //      MEDIUM-ENCRYPTED gets omitted from the Dict
            //      All others get a TRUE.
            _eventTypesToDelete.Clear();
            foreach (DataRow row in sqlActionItems.Rows)
            {
                if (row["ActionName"].ToString() != "MEDIUM-ENCRYPTED")
                {
                    if (row["ActionName"].ToString() == "DEVICE-ATTACHED" || row["ActionName"].ToString() == "GRANTED" || row["ActionName"].ToString() == "WRITE-GRANTED")
                    {
                        _eventTypesToDelete.Add(row["ActionId"].ToString() + ": " + row["ActionName"].ToString(), false);
                    }
                    else
                    {
                        _eventTypesToDelete.Add(row["ActionId"].ToString() + ": " + row["ActionName"].ToString(), true);
                    }
                }                
            }
		}

		private string CalculateProcessingEndtime()
		{
			string result = "failure";
			_processingEndTime = DateTime.Now;
			_processingEndTime = _processingEndTime.AddMinutes(_runTime);
			result = "success";
			return result;
		}

		public void BuildSqlConnectionString()
		{
            if (_altCredentialsSelected == false)
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder
                {
                    ["Data Source"] = _dbServerAddress,
                    ["integrated Security"] = true,
                    ["Initial Catalog"] = _databaseName,
                    ["Application Name"] = _applicationName
                };
                _sqlConnectionString = builder.ConnectionString;
            }
            else
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder.DataSource = _dbServerAddress;
                builder.IntegratedSecurity = false;
                builder.UserID = _sqlConnUserName;
                builder.Password = _sqlConnPassword;
                builder.InitialCatalog = _databaseName;
                _sqlConnectionString = builder.ConnectionString;
            }
            LoggingClass.SaveEventToLogFile(_logFileLocation, " SQL Server Connection String: " + _sqlConnectionString);
        }

		public string ValidateParameters()
		{
			string result = "failure";
			LoggingClass.SaveEventToLogFile(_logFileLocation, " Records Cut Off Date:" + _cutOffDate.ToString());
			result = CalculateBatchSize();
			LoggingClass.SaveEventToLogFile(_logFileLocation, " Batch Size:" + _batchSize);
			if (result == "success")
			{
				result = CalculateProcessingEndtime();
				LoggingClass.SaveEventToLogFile(_logFileLocation, " Calculated Processing End Time:" + _processingEndTime.ToString(CultureInfo.InvariantCulture));
				if (result == "success")
				{
					result = BL.RecordsDeletionQueryLogic.CreateRequiredStoredProcedures(this);
					LoggingClass.SaveEventToLogFile(_logFileLocation, " SQL Stored Procedures check/creation:" + result);
				}
				else
				{
					LoggingClass.SaveErrorToLogFile(_logFileLocation, "Calculation of Processing-Endtime failed:" + result);
				}
			}
			else
			{
				LoggingClass.SaveErrorToLogFile(_logFileLocation, "Calculation of Batch Size failed:" + result);
			}
			
			return result;
		}

		public void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
		{
			_spCheckReturnString = e.Message.ToString();
		}
		
		public void CalculateTotalRecordsToPurge()
		{
			try
			{
				using (var conn = new SqlConnection(_sqlConnectionString))
				using (var command = new SqlCommand(_totalRecordsCalcStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure })
				{
				    command.CommandTimeout = 0;
				    SqlParameter spReturnParam1 = new SqlParameter("@RecordCount", SqlDbType.Int, 11)
				    {
				        Direction = ParameterDirection.ReturnValue
				    };
				    command.Parameters.Add(spReturnParam1);

                    if (_cutOffDays)
                    {
                        SqlParameter spParam1 = new SqlParameter("@cutOffDate", SqlDbType.DateTime, 11) { Value = _cutOffDate };
                        command.Parameters.Add(spParam1);
                    }

                    if (_selectedComputer != "all")
					{
					    SqlParameter spParam2 = new SqlParameter("@WorkstationId", SqlDbType.NVarChar, 255)
					    {
					        Value = _epsGuid.ToString()
					    };
					    command.Parameters.Add(spParam2);
					    SqlParameter spParam3 = new SqlParameter("@theWorkstationName", SqlDbType.NVarChar, 255)
					    {
					        Value = _selectedComputer.ToString()
					    };
					    command.Parameters.Add(spParam3);
					}

					if (_selectedUser != "everyone")
					{
					    SqlParameter spParam4 = new SqlParameter("@theUserAccountSid", SqlDbType.NVarChar, 200)
					    {
					        Value = _userSid.ToString()
					    };
					    command.Parameters.Add(spParam4);
					}

					if (_selectedProcess != "all")
					{
					    SqlParameter spParam5 = new SqlParameter("@theProcessName", SqlDbType.NVarChar, 255)
					    {
					        Value = _selectedProcess.ToString()
					    };
					    command.Parameters.Add(spParam5);
					}

					conn.Open();
					command.ExecuteNonQuery();
					_returnedTotalRowsToPurge = Convert.ToDouble(spReturnParam1.Value);
					_remainingRowsToPurge = _returnedTotalRowsToPurge;
					conn.Close();
					_result.Success = true;

				}
			}
			catch (Exception ex)
			{
				_result.Message = " Error calculating Total-Records-To-Purge:" + ex.Message.ToString();
				_result.Success = false;
			}
		}

		public void RequestStop()
		{
			LoggingClass.SaveEventToLogFile(_logFileLocation, " SQL Worker Thread stop requested.");
			StopController = true;
		}


	}
}
