using System;
using System.Collections.Generic;
using System.Data;

namespace IESandDACadmt.ViewModel
{
    public partial class DbSqlSpControllerData
    {

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
        public string SqlCommandEsPullActionTypes
        {
            get { return _sqlCommandEsPullActionTypes; }
            set { _sqlCommandEsPullActionTypes = value; }
        }

        private string _sqlCommandEmssPullActionTypes = "SELECT ActionId, ActionName FROM dbo.LogAction ORDER BY ActionId";
        public string SqlCommandEmssPullActionTypes
        {
            get { return _sqlCommandEmssPullActionTypes; }
            set { _sqlCommandEmssPullActionTypes = value; }
        }

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

        private Model.Logging.ActionOutcome _result = new Model.Logging.ActionOutcome();

        public Model.Logging.ActionOutcome Result
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
            set { _processingEndTime = value; }
        }

        public double ReturnedTotalRowsToPurge
        {
            get { return _returnedTotalRowsToPurge; }
            set { _returnedTotalRowsToPurge = value; }
        }

        public int RecordsForBatchSize
        {
            get { return _recordsForBatchSize; }
            set { _recordsForBatchSize = value; }
        }

        public double RemainingRowsToPurge
        {
            get { return _remainingRowsToPurge; }
            set { _remainingRowsToPurge = value; }
        }

        public List<string> UserList
        {
            get { return _userList; }
            set { _userList = value; }
        }

        public List<string> ComputerList
        {
            get { return _computerList; }
            set { _computerList = value; }
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

        private string _percentageRecordsProcessed;

        public string PercentageRecordsProcessed
        {
            get { return _percentageRecordsProcessed; }
            set { _percentageRecordsProcessed = value; }
        }

        private string _runtimeRemaining;

        public string RuntimeRemaining
        {
            get { return _runtimeRemaining; }
            set { _runtimeRemaining = value; }
        }


        private Dictionary<string, bool> _eventTypesToDelete = new Dictionary<string, bool>();

        public Dictionary<string, bool> EventTypesToDelete
        {
            get { return _eventTypesToDelete; }
            set { _eventTypesToDelete = value; }
        }

        public string ApplicationName { get => _applicationName; set => _applicationName = value; }
        public string DbServerAddress { get => _dbServerAddress; set => _dbServerAddress = value; }
        public bool WorkerPurgerComplete { get => _workerPurgerComplete; set => _workerPurgerComplete = value; }
        public string DatabaseName { get => _databaseName; set => _databaseName = value; }
        
    }
}
