using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IESandDACadmt.ViewModel;
using IESandDACadmt.Model;
using IESandDACadmt.Model.Sql;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfHealthReview.xaml
    /// </summary>
    public partial class WpfHealthReview : Window
    {
        public WpfHealthReview(Model.DbSqlSpController theLiveDbSqlSpModel)
        {
            _theLiveDbSqlSpData = theLiveDbSqlSpModel;
            _theHealthQueries = new Model.SqlHealthQueries(_theLiveDbSqlSpData);
#if DEBUG
            WriteSqlQueriesToLogFile(_theHealthQueries.SqlHealthConfigQueryList);
            WriteSqlQueriesToLogFile(_theHealthQueries.SqlHealthWaitStatsQueryList);
            WriteSqlQueriesToLogFile(_theHealthQueries.SqlHealthStoredProcedureStatsQueryList);
            WriteSqlQueriesToLogFile(_theHealthQueries.SqlHealthLogTableIndexStatsQueryList);
            WriteSqlQueriesToLogFile(_theHealthQueries.SqlLogTableStatisticsQueryList);
#endif
            InitializeComponent();
            switch (theLiveDbSqlSpModel.DbSqlSpControllerData.HeatServerType)
            {
                case DbSqlSpControllerData.ServerType.UNKNOWN:
                    _appServerServicesNames = "Unknown";
                    break;
                case DbSqlSpControllerData.ServerType.EMSS:
                    _appServerServicesNames = _emssServiceNames;
                    break;
                case DbSqlSpControllerData.ServerType.ES:
                    _appServerServicesNames = _esServiceNames;
                    break;
                default:
                    _appServerServicesNames = "Unknown";
                    break;
            }
            _sqlConnectionString = _theLiveDbSqlSpData.DbSqlSpControllerData.SqlConnectionString;
            _dbServerName = _theLiveDbSqlSpData.DbSqlSpControllerData.DbServeraddress;
            this.DataContext = _sqlHealthData;
            token = cancelSource.Token;
        }

        private readonly ViewModel.SqlHealthReviewData _sqlHealthData = new ViewModel.SqlHealthReviewData();
        //private Model.SqlHealthReviewLogic _theHealthQueries = null;
        private Model.SqlHealthQueries _theHealthQueries = null;
        private Model.DbSqlSpController _theLiveDbSqlSpData = null;
        private readonly string _sqlConnectionString;
        private readonly string _dbServerName;
        private int _processingOngoing = 0;
        private string _appServerServicesNames = "";
        private string _esServiceNames = "Endpoint Security Application Server";
        private string _emssServiceNames = "EDS Server, EDS Installer Service, Replication Service, STATEngine,";

        private bool _allowTabSwitch = true;
        private int _previousTab = 0;
        System.Windows.Threading.DispatcherTimer timerProcessing = new System.Windows.Threading.DispatcherTimer();

        CancellationTokenSource cancelSource = new CancellationTokenSource();
        CancellationToken token = new CancellationToken();

        delegate void SetServerConfigInfoDataCallBack(DataTable results);
        delegate void SetWaitStatsInfoDataCallBack(DataTable results);
        delegate void SetStoredProcWaitStatsInfoDataCallBack(DataTable results);
        delegate void SetLogTableIndexHealthInfoDataCallBack(DataTable results);
        delegate void SetLogTableStatisiticsHealthInfoDataCallBack(DataTable results);

        private void WriteSqlQueriesToLogFile(List<Model.singleSqlHealthQuery> theQueryList)
        {
            foreach (Model.singleSqlHealthQuery singleQuery in theQueryList)
            {
                Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, singleQuery.QueryName);
                Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, singleQuery.QueryCode);
            }
        }

        private void FormHealthReview_Load(object sender, EventArgs e)
        {
            SetGuiForServerType(_dbServerName);
            PrepForAndRunAllAnalysis();
        }

        private void SetGuiForServerType(string dbServerName)
        {
            this.Title = "Health Review Tool    Server: " + _theLiveDbSqlSpData.DbSqlSpControllerData.DbServeraddress;
        }

        private void PrepForAndRunAllAnalysis()
        {
            _processingOngoing = 5;
            timerProcessing.IsEnabled = true;
            _allowTabSwitch = true;
            ClearDownAllDataTableRows();
            Task.Factory.StartNew(() => { RunAllAnalysis(_sqlHealthData); });
        }

        private void RunAllAnalysis(ViewModel.SqlHealthReviewData theHealthReviewData)
        {
            RunSqlQuery(_theHealthQueries.SqlHealthConfigQueryList, _sqlConnectionString,
                        _sqlHealthData.ServerConfigResults, Model.SqlHealthReviewLogic.SqlQueryType.ServerConfig);
            RunSqlQuery(_theHealthQueries.SqlHealthWaitStatsQueryList, _sqlConnectionString,
                        _sqlHealthData.WaitStatsResults, Model.SqlHealthReviewLogic.SqlQueryType.AllWaitStats);
            RunSqlQuery(_theHealthQueries.SqlHealthStoredProcedureStatsQueryList, _sqlConnectionString,
                        _sqlHealthData.SpWaitStatsResults, Model.SqlHealthReviewLogic.SqlQueryType.SpWaitStats);
            RunSqlQuery(_theHealthQueries.SqlHealthLogTableIndexStatsQueryList, _sqlConnectionString,
                        _sqlHealthData.LogTableIndexStatsResults, Model.SqlHealthReviewLogic.SqlQueryType.IndexStats);
            RunSqlQuery(_theHealthQueries.SqlLogTableStatisticsQueryList, _sqlConnectionString,
                        _sqlHealthData.LogTableStatisticsResults, Model.SqlHealthReviewLogic.SqlQueryType.TableStats);
            _processingOngoing = 0;
        }

        private void ClearDownAllDataTableRows()
        {
            ServerConfigDataGrid.DataContext = null;
            WaitStatsDataGrid.DataContext = null;
            StoredProcedureStatsDataGrid.DataContext = null;
            LogTableIndexDataGrid.DataContext = null;
            LogTableStatsDataGrid.DataContext = null;
            _sqlHealthData.ServerConfigResults.Clear();
            _sqlHealthData.WaitStatsResults.Clear();
            _sqlHealthData.SpWaitStatsResults.Clear();
            _sqlHealthData.LogTableIndexStatsResults.Clear();
            _sqlHealthData.LogTableStatisticsResults.Clear();
        }

        public void RunSqlQuery(List<Model.singleSqlHealthQuery> sqlQueries, string sqlConnectionString, DataTable queryResults, Model.SqlHealthReviewLogic.SqlQueryType queryType)
        {
            queryResults = ExecuteSqlQueryList(sqlQueries, sqlConnectionString, queryResults, queryType);
            if (!(token.IsCancellationRequested))
            {
                if (queryResults.Rows.Count > 1)
                {
                    switch (queryType)
                    {
                        case Model.SqlHealthReviewLogic.SqlQueryType.ServerConfig:
                            SetNewServerConfigInfoDataCallBack(queryResults);
                            break;
                        case Model.SqlHealthReviewLogic.SqlQueryType.AllWaitStats:
                            SetNewWaitStatsInfoDataCallBack(queryResults);
                            break;
                        case Model.SqlHealthReviewLogic.SqlQueryType.SpWaitStats:
                            SetNewStoredProcWaitStatsInfoDataCallBack(queryResults);
                            break;
                        case Model.SqlHealthReviewLogic.SqlQueryType.IndexStats:
                            SetNewLogTableIndexHealthInfoDataCallBack(queryResults);
                            break;
                        case Model.SqlHealthReviewLogic.SqlQueryType.TableStats:
                            SetNewLogTableStatisiticsHealthInfoDataCallBack(queryResults);
                            break;
                        default:
                            Model.Logging.LoggingClass.SaveErrorToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " No Data Query Type Specified.");
                            break;
                    }
                }
            }
        }

        private DataTable ExecuteSqlQueryList(List<Model.singleSqlHealthQuery> theQueryList, string sqlConnectionString, DataTable theResults, Model.SqlHealthReviewLogic.SqlQueryType theQueryType)
        {

            if (theQueryType == Model.SqlHealthReviewLogic.SqlQueryType.ServerConfig)
            {
                if (theResults.Columns.Count == 0)
                {
                    DataColumn itemColumn = new DataColumn { ColumnName = "Item", DataType = Type.GetType("System.String") };
                    DataColumn valueColumn = new DataColumn { ColumnName = "Value", DataType = Type.GetType("System.String") };
                    theResults.Columns.Add(itemColumn);
                    theResults.Columns.Add(valueColumn);
                }
            }
            switch (theQueryType)
            {
                case Model.SqlHealthReviewLogic.SqlQueryType.ServerConfig:
                    RunServerConfigSqlQueries(theQueryList, sqlConnectionString, theResults);
                    break;
                default:
                    RunOtherTabsSqlQueries(theQueryList, sqlConnectionString, theResults, theQueryType);
                    break;
            }
            return theResults;
        }

        private void RunServerConfigSqlQueries(List<singleSqlHealthQuery> theQueryList, string sqlConnectionString, DataTable theResults)
        {
            foreach (Model.singleSqlHealthQuery singleQuery in theQueryList)
            {
                if (!(token.IsCancellationRequested))
                {
                    try
                    {
                        using (var theSqlConnection = new SqlConnection(sqlConnectionString))
                        using (var theSqlCommand = new SqlCommand(singleQuery.QueryCode, theSqlConnection))
                        {
                            // Open Connection
                            theSqlConnection.Open();
                            Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " SQL connection for " + singleQuery.QueryName + " is OPEN.");
                            theSqlCommand.CommandTimeout = 10800;
                            // Run query and fill in data table
                            string singleResult = theSqlCommand.ExecuteScalar().ToString();
                            if (!string.IsNullOrEmpty(singleResult))
                            {
                                Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " SQL Query for " + singleQuery.QueryName + " succeeded.");
                                DataRow singleRow = theResults.NewRow();
                                singleRow["Item"] = singleQuery.QueryName;
                                singleRow["Value"] = singleResult;
                                theResults.Rows.Add(singleRow);
                            }
                            else
                            {
                                Model.Logging.LoggingClass.SaveErrorToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " Error with Query Type " + singleQuery.QueryName + ":: Could not read value");
                                DataRow singleRow = theResults.NewRow();
                                singleRow["Item"] = singleQuery.QueryName;
                                singleRow["Value"] = "*** Could not read value for Query Type " + singleQuery.QueryName + ". Please check log file for more information";
                                theResults.Rows.Add(singleRow);
                            }
                            theSqlCommand.Dispose();
                            theSqlConnection.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        Model.Logging.LoggingClass.SaveErrorToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " Error with Query Type " + singleQuery.QueryName + ":: " + ex.Message);
                        DataRow singleRow = theResults.NewRow();
                        singleRow["Item"] = singleQuery.QueryName;
                        singleRow["Value"] = "*** Could not read value for Query Type " + singleQuery.QueryName + ". Please check log file for more information";
                        theResults.Rows.Add(singleRow);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public void RunOtherTabsSqlQueries(List<singleSqlHealthQuery> theQueryList, string sqlConnectionString, DataTable theResults, Model.SqlHealthReviewLogic.SqlQueryType theQueryType)
        {
            foreach (Model.singleSqlHealthQuery singleQuery in theQueryList)
            {
                if (!(token.IsCancellationRequested))
                {
                    try
                    {
                        using (var theSqlConnection = new SqlConnection(sqlConnectionString))
                        using (var theSqlCommand = new SqlCommand(singleQuery.QueryCode, theSqlConnection))
                        {
                            // Open Connection
                            theSqlConnection.Open();
                            Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " SQL connection for " + theQueryType.ToString() + " is OPEN.");
                            theSqlCommand.CommandTimeout = 10800;
                            // Run query and fill in data table
                            theResults.Load(theSqlCommand.ExecuteReader());
                            if (theResults.Rows.Count >= 1)
                            {
                                Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " SQL Query Type " + theQueryType.ToString() + " succeeded.");
                            }
                            theSqlCommand.Dispose();
                            theSqlConnection.Close();
                        }
                    }
                    catch (SqlException ex)
                    {
                        Model.Logging.LoggingClass.SaveErrorToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, " Error with Query Type " + theQueryType.ToString() + ":: " + ex.Message);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void SetNewServerConfigInfoDataCallBack(DataTable queryResults)
        {
            //if (this.dataGridViewServerConfig.InvokeRequired)
            //{
            //    SetServerConfigInfoDataCallBack del = new SetServerConfigInfoDataCallBack(SetNewServerConfigInfoDataCallBack);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            //    dataGridViewServerConfig.Visible = true;
            //    if (!token.IsCancellationRequested)
            //    {
            //        dataGridViewServerConfig.DataSource = _sqlHealthData.ServerConfigResults;
            //    }
                _processingOngoing = _processingOngoing - 1;
            //}

        }

        private void SetNewWaitStatsInfoDataCallBack(DataTable queryResults)
        {
            //if (this.dataGridViewOverallWaitStats.InvokeRequired)
            //{
            //    SetWaitStatsInfoDataCallBack del = new SetWaitStatsInfoDataCallBack(SetNewWaitStatsInfoDataCallBack);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            //    dataGridViewOverallWaitStats.Visible = true;
            //    if (!token.IsCancellationRequested)
            //    {
            //        dataGridViewOverallWaitStats.DataSource = _sqlHealthData.WaitStatsResults;
            //    }
                _processingOngoing = _processingOngoing - 1;
            //}
        }

        private void SetNewStoredProcWaitStatsInfoDataCallBack(DataTable queryResults)
        {
            //if (this.dataGridViewStoredProcedureStats.InvokeRequired)
            //{
            //    SetStoredProcWaitStatsInfoDataCallBack del = new SetStoredProcWaitStatsInfoDataCallBack(SetNewStoredProcWaitStatsInfoDataCallBack);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            //    dataGridViewStoredProcedureStats.Visible = true;
            //    if (!token.IsCancellationRequested)
            //    {
            //        dataGridViewStoredProcedureStats.DataSource = _sqlHealthData.SpWaitStatsResults;
            //    }
                _processingOngoing = _processingOngoing - 1;
            //}
        }

        private void SetNewLogTableIndexHealthInfoDataCallBack(DataTable queryResults)
        {
            //if (this.dataGridViewIndex.InvokeRequired)
            //{
            //    SetLogTableIndexHealthInfoDataCallBack del = new SetLogTableIndexHealthInfoDataCallBack(SetNewLogTableIndexHealthInfoDataCallBack);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            //    dataGridViewIndex.Visible = true;
            //    if (!token.IsCancellationRequested)
            //    {
            //        dataGridViewIndex.DataSource = _sqlHealthData.LogTableIndexStatsResults;
            //    }
                _processingOngoing = _processingOngoing - 1;
            //}
        }

        private void SetNewLogTableStatisiticsHealthInfoDataCallBack(DataTable queryResults)
        {
            //if (this.dataGridViewLogTableStatisticsHealth.InvokeRequired)
            //{
            //    SetLogTableStatisiticsHealthInfoDataCallBack del = new SetLogTableStatisiticsHealthInfoDataCallBack(SetNewLogTableStatisiticsHealthInfoDataCallBack);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            //    dataGridViewLogTableStatisticsHealth.Visible = true;
            //    if (!token.IsCancellationRequested)
            //    {
            //        dataGridViewLogTableStatisticsHealth.DataSource = _sqlHealthData.LogTableStatisticsResults;
            //    }
                _processingOngoing = _processingOngoing - 1;
            //}
        }

        private void buttonReRunAnalysis_Click(object sender, EventArgs e)
        {
            cancelSource = new CancellationTokenSource();
            token = cancelSource.Token;
            MessageBoxResult dialogResult = MessageBox.Show("Re-run ALL analysis (YES) or just current tab (NO)?", "Re-run Analysis", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Yes)
            {
                PrepForAndRunAllAnalysis();
            }
            else if (dialogResult == MessageBoxResult.No)
            {

                PrepForAndRunSingleTabAnalysis();
            }
        }

        private void PrepForAndRunSingleTabAnalysis()
        {
            _processingOngoing = 1;
            timerProcessing.IsEnabled = true;
            switch (MasterTabControl.SelectedIndex)
            {
                case 0:
                    {
                        ServerConfigDataGrid.DataContext = null;
                        _sqlHealthData.ServerConfigResults.Clear();
                        Task.Factory.StartNew(() =>
                        {
                            RunSqlQuery(_theHealthQueries.SqlHealthConfigQueryList, _sqlConnectionString,
                                        _sqlHealthData.ServerConfigResults, Model.SqlHealthReviewLogic.SqlQueryType.ServerConfig);
                        });
                        break;
                    }
                case 1:
                    {
                        WaitStatsDataGrid.DataContext = null;
                        _sqlHealthData.WaitStatsResults.Clear();
                        Task.Factory.StartNew(() =>
                        {
                            RunSqlQuery(_theHealthQueries.SqlHealthWaitStatsQueryList, _sqlConnectionString,
                                        _sqlHealthData.WaitStatsResults, Model.SqlHealthReviewLogic.SqlQueryType.AllWaitStats);
                        });
                        break;
                    }
                case 2:
                    {
                        StoredProcedureStatsDataGrid.DataContext = null;
                        _sqlHealthData.SpWaitStatsResults.Clear();
                        Task.Factory.StartNew(() =>
                        {
                            RunSqlQuery(_theHealthQueries.SqlHealthStoredProcedureStatsQueryList, _sqlConnectionString,
                                        _sqlHealthData.SpWaitStatsResults, Model.SqlHealthReviewLogic.SqlQueryType.SpWaitStats);
                        });
                        break;
                    }
                case 3:
                    {
                        LogTableIndexDataGrid.DataContext = null;
                        _sqlHealthData.LogTableIndexStatsResults.Clear();
                        Task.Factory.StartNew(() =>
                        {
                            RunSqlQuery(_theHealthQueries.SqlHealthLogTableIndexStatsQueryList, _sqlConnectionString,
                                        _sqlHealthData.LogTableIndexStatsResults, Model.SqlHealthReviewLogic.SqlQueryType.IndexStats);
                        });
                        break;
                    }
                case 4:
                    {
                        LogTableStatsDataGrid.DataContext = null;
                        _sqlHealthData.LogTableStatisticsResults.Clear();
                        Task.Factory.StartNew(() =>
                        {
                            RunSqlQuery(_theHealthQueries.SqlLogTableStatisticsQueryList, _sqlConnectionString,
                                        _sqlHealthData.LogTableStatisticsResults, Model.SqlHealthReviewLogic.SqlQueryType.TableStats);
                        });
                        break;
                    }
                default:
                    {
                        // nothing
                        _processingOngoing = 0;
                        break;
                    }
            }
        }

        private void buttonExportToFile_Click(object sender, EventArgs e)
        {
            string exportType = "";
            DataSet outputData = new DataSet("Excel Output");
            // Ask question to user to save all tabs or just this tab?         
            MessageBoxResult dialogResult = MessageBox.Show("Export All tabs (YES) to .xls or just current tab (NO)?", "Export Tabs", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (dialogResult == MessageBoxResult.Cancel)
            {
                return;
            }
            if (dialogResult == MessageBoxResult.Yes)
            {
                //pump all DGV's a into multi-sheet XLS file
                DataTable serverConfig = _sqlHealthData.ServerConfigResults.Copy();
                outputData.Tables.Add(serverConfig);
                DataTable waitStatsResults = _sqlHealthData.WaitStatsResults.Copy();
                outputData.Tables.Add(waitStatsResults);
                DataTable spWaitStatsResults = _sqlHealthData.SpWaitStatsResults.Copy();
                outputData.Tables.Add(spWaitStatsResults);
                DataTable logTableIndexStatsResults = _sqlHealthData.LogTableIndexStatsResults.Copy();
                outputData.Tables.Add(logTableIndexStatsResults);
                DataTable logTableStatisticsResults = _sqlHealthData.LogTableStatisticsResults.Copy();
                outputData.Tables.Add(logTableStatisticsResults);
                exportType = "EADMT_ALL_HEALTH_TABS";
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                //pump only the top tab into .xls
                switch (MasterTabControl.SelectedIndex)
                {
                    case 0:
                        {
                            DataTable serverConfig = _sqlHealthData.ServerConfigResults.Copy();
                            outputData.Tables.Add(serverConfig);
                            exportType = "EADMT_SERVER_CONFIG_TAB";
                            break;
                        }
                    case 1:
                        {
                            DataTable waitStatsResults = _sqlHealthData.WaitStatsResults.Copy();
                            outputData.Tables.Add(waitStatsResults);
                            exportType = "EADMT_WAIT_STATS_TAB";
                            break;
                        }
                    case 2:
                        {
                            DataTable spWaitStatsResults = _sqlHealthData.SpWaitStatsResults.Copy();
                            outputData.Tables.Add(spWaitStatsResults);
                            exportType = "EADMT_STORED_PROCS_STATS_TAB";
                            break;
                        }
                    case 3:
                        {
                            DataTable logTableIndexStatsResults = _sqlHealthData.LogTableIndexStatsResults.Copy();
                            outputData.Tables.Add(logTableIndexStatsResults);
                            exportType = "EADMT_INDEX_STATS_TAB";
                            break;
                        }
                    case 4:
                        {
                            DataTable logTableStatisticsResults = _sqlHealthData.LogTableStatisticsResults.Copy();
                            outputData.Tables.Add(logTableStatisticsResults);
                            exportType = "EADMT_LOG_TABLE_STATS_TAB";
                            break;
                        }
                    default:
                        {
                            // nothing
                            break;
                        }
                }
            }
            // Ask User where to save the file to?
            SaveFileDialog outputFileLocation = new SaveFileDialog();
            string filename = _dbServerName + "_" + exportType + "_" + DateTime.Now;
            filename = filename.Replace(':', '-');
            filename = filename.Replace('/', '-');
            filename = filename.Replace('\\', '-');
            outputFileLocation.FileName = filename;
            outputFileLocation.Filter = "Excel|*.xlsx";
            outputFileLocation.Title = "Save Excel File to where?";
            outputFileLocation.CheckPathExists = true;
            outputFileLocation.OverwritePrompt = true;
            outputFileLocation.ShowDialog();
            // Create XLS file and pump DataGridViews or DataTables to it?
            try
            {
                ExportToExcel.CreateExcelFile.CreateExcelDocument(outputData, outputFileLocation.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when saving data to excel file. Please see Log File for more details.", "Error Saving File", MessageBoxButton.OK, MessageBoxImage.Error);
                Model.Logging.LoggingClass.SaveErrorToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, ex.Message);
            }

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timerProcessing_Tick(object sender, EventArgs e)
        {
            if (_processingOngoing > 0)
            {
                if (progressBarSql.Value < 100)
                {
                    progressBarSql.Value = progressBarSql.Value + 1;
                }
                else
                {
                    progressBarSql.Value = 0;
                }
            }
            else
            {
                if (_sqlHealthData.IsTabRefreshNeeded)
                {
                    if (_sqlHealthData.TabIndex == 10)
                    {
                        _sqlHealthData.IsTabRefreshNeeded = false;
                        PrepForAndRunAllAnalysis();
                    }
                    else
                    {
                        _sqlHealthData.IsTabRefreshNeeded = false;
                        PrepForAndRunSingleTabAnalysis();
                    }
                }
                else
                {
                    timerProcessing.IsEnabled = false;
                    progressBarSql.Value = 100;
                    _allowTabSwitch = true;
                }
            }
        }

        private void buttonStartIndexChanges_Click(object sender, EventArgs e)
        {
            bool needOffline = CheckForRebuilds();
            if (needOffline)
            {
                // Give need for off line message
                MessageBoxResult requireOfflineResponse = MessageBox.Show("Index Rebuilds lock the tables so you need to have the " + _theLiveDbSqlSpData.DbSqlSpControllerData.HeatServerType.ToString()
                                                                        + " Services (" + _appServerServicesNames + ") stopped to prevent data corruption. Click OK if you have done this and it is safe to proceed.",
                                                                        "Index Rebuild requests detected", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (requireOfflineResponse == MessageBoxResult.OK)
                {
                    PrepAndRunIndexCommands();
                }
            }
            else
            {
                PrepAndRunIndexCommands();
            }

        }

        private void PrepAndRunIndexCommands()
        {
            ConfigureSubThreadControlItems();
            Task.Factory.StartNew(() => { RunAllIndexCommands(); }, token);
        }

        private bool CheckForRebuilds()
        {
            int rebuildsSelectedCount = 0;
            foreach (System.Data.DataRow row in LogTableIndexDataGrid.ItemsSource)
            {
                if (Convert.ToBoolean(row["Rebuild"]) == true)
                {
                    rebuildsSelectedCount += 1;
                }
            }
            if (rebuildsSelectedCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RunAllIndexCommands()
        {
            List<string> rebuildStringsList = BuildIndexChangeCommands("REBUILD", LogTableIndexDataGrid, "Rebuild");
            List<string> reorgStringsList = BuildIndexChangeCommands("REORGANIZE", LogTableIndexDataGrid, "Reorganize");
            if (!token.IsCancellationRequested)
            {
                ProcessSqlCommands(rebuildStringsList);
            }
            if (!token.IsCancellationRequested)
            {
                ProcessSqlCommands(reorgStringsList);
            }
            _processingOngoing = 0;
            _allowTabSwitch = true;
        }

        private List<string> BuildIndexChangeCommands(string changeType, DataGrid theDGV, string recommendationRowName)
        {
            List<string> indexChangeStrings = new List<string>();
            foreach (System.Data.DataRow dRow in theDGV.ItemsSource)
            {
                if (Convert.ToBoolean(dRow[recommendationRowName]) == true)
                {
                    indexChangeStrings.Add("ALTER INDEX " + dRow["IndexName"].ToString() + " ON " + _theLiveDbSqlSpData.DbSqlSpControllerData.DataBaseName + "." + dRow["SchemaName"].ToString() + "." + dRow["TableName"].ToString() + " " + changeType);
                }
            }



            //foreach (DataGridRow row in theDGV. .Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells[recommendationRowName].Value) == true)
            //    {
            //        rebuildStrings.Add("ALTER INDEX " + row.Cells["IndexName"].Value.ToString() + " ON " + _theLiveDbSqlSpData.DbSqlSpControllerData.DataBaseName + "." + row.Cells["SchemaName"].Value.ToString() + "." + row.Cells["TableName"].Value.ToString() + " " + changeType);
            //    }
            //}
            return indexChangeStrings.ToList();
        }


        private void ProcessSqlCommands(List<string> theCommands)
        {
            foreach (string indexCommand in theCommands)
            {
                if (!(token.IsCancellationRequested))
                {
                    Model.Logging.ActionOutcome result = Model.Sql.QuerySqlServer.RunSqlQuery(_sqlConnectionString, 0, indexCommand);
                    if (result.Success)
                    {
                        Model.Logging.LoggingClass.SaveEventToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, "Successfully Ran '" + indexCommand + "'::" + result.Message);
                    }
                    else
                    {
                        Model.Logging.LoggingClass.SaveErrorToLogFile(_theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation, "Error running '" + indexCommand + "'::" + result.Message);
                    }

                }
                else
                {
                    break;
                }

            }
        }

        private void buttonStopIndexChanges_Click(object sender, EventArgs e)
        {
            StopSqlProcessing();
        }

        private void StopSqlProcessing()
        {
            if (!token.IsCancellationRequested)
            {
                cancelSource.Cancel();
                _sqlHealthData.IsTabRefreshNeeded = false;
            }
        }

        private void tabControlHealthReview_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This needs to be updated to disable/enable the menuitem buttons for the respective tabs.
            if (_allowTabSwitch)
            {
                //progressBarSql.Parent = tabControlHealthReview.TabPages[tabControlHealthReview.SelectedIndex];
                //buttonReRunAnalysis.Parent = tabControlHealthReview.TabPages[tabControlHealthReview.SelectedIndex];
                //buttonExportToFile.Parent = tabControlHealthReview.TabPages[tabControlHealthReview.SelectedIndex];
                //buttonOpenLogFile.Parent = tabControlHealthReview.TabPages[tabControlHealthReview.SelectedIndex];
                //buttonClose.Parent = tabControlHealthReview.TabPages[tabControlHealthReview.SelectedIndex];
                _previousTab = MasterTabControl.SelectedIndex;
            }
            else
            {
                MasterTabControl.SelectedIndex = _previousTab;
                MessageBox.Show("You cannot switch tabs while tasks are processing. Please use the TASKS menu item to stop current running tasks.", "Tab switch not possible", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void buttonUpdateTableStats_Click(object sender, EventArgs e)
        {
            ConfigureSubThreadControlItems();
            Task.Factory.StartNew(() => { RunAllTableStatsCommands(); }, token);
        }

        private void ConfigureSubThreadControlItems()
        {
            cancelSource = new CancellationTokenSource();
            token = cancelSource.Token;
            _allowTabSwitch = false;
            _processingOngoing = 1;
            _sqlHealthData.TabIndex = MasterTabControl.SelectedIndex;
            _sqlHealthData.IsTabRefreshNeeded = true;
            timerProcessing.IsEnabled = true;
        }

        private void buttonStopUpdatingTableStats_Click(object sender, EventArgs e)
        {
            StopSqlProcessing();
        }

        private void RunAllTableStatsCommands()
        {
            List<string> updateStatsStringsList = BuildTableStatsUpdateCommands(LogTableStatsDataGrid, "StatsRecommendation");
            if (!token.IsCancellationRequested)
            {
                ProcessSqlCommands(updateStatsStringsList);
            }
            _allowTabSwitch = true;
            _processingOngoing = 0;
        }

        private List<string> BuildTableStatsUpdateCommands(DataGrid theDGV, string recommendationRowName)
        {
            List<string> tableStatsStrings = new List<string>();
            //foreach (DataGridViewRow row in theDGV.Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells[recommendationRowName].Value) == true)
            //    {
            //        rebuildStrings.Add("UPDATE STATISTICS " + _theLiveDbSqlSpData.DbSqlSpControllerData.DataBaseName + "." + row.Cells["SchemaName"].Value.ToString() + "." + row.Cells["TableName"].Value.ToString() + " " + row.Cells["IndexName"].Value.ToString());
            //    }
            //}

            foreach(System.Data.DataRow dRow in theDGV.ItemsSource)
            {
                if (Convert.ToBoolean(dRow[recommendationRowName]) == true)
                {
                    tableStatsStrings.Add("UPDATE STATISTICS " + _theLiveDbSqlSpData.DbSqlSpControllerData.DataBaseName + "." + dRow["SchemaName"].ToString() + "." + dRow["TableName"].ToString() + " " + dRow["IndexName"].ToString());
                }
            }
            return tableStatsStrings.ToList();
        }

        private void buttonOpenLogFile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", _theLiveDbSqlSpData.DbSqlSpControllerData.LogFileLocation);
        }

        private void buttonCloseProfiler_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Checks if One DGV boolean column is checked and it if it, it unchecks the second
        /// </summary>
        /// <param name="theRowIndex"></param>
        /// <param name="theColIndex"></param>
        /// <param name="theColOneName"></param>
        /// <param name="theColTwoName"></param>
        private void CheckOneBoolColumnAgainstAnother(int theRowIndex, int theColIndex, string theColOneName, string theColTwoName)
        {
            DataGridRow clickedRow = LogTableIndexDataGrid [theRowIndex];
            DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)clickedRow.Cells[theColIndex];
            if ((bool)chk.Value == true)
            {
                int reorgCol = 0;
                foreach (DataGridViewColumn column in dataGridViewIndex.Columns)
                {
                    if (column.Name == theColTwoName)
                    {
                        reorgCol = column.Index;
                    }
                }
                DataGridViewCheckBoxCell reorgCell = (DataGridViewCheckBoxCell)clickedRow.Cells[reorgCol];
                if ((bool)reorgCell.Value == true)
                {
                    reorgCell.Value = false;
                }

            }
        }

        private void dataGridViewIndex_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewIndex.Columns[e.ColumnIndex].Name == "Rebuild" || dataGridViewIndex.Columns[e.ColumnIndex].Name == "Reorganize")
            {
                //MessageBox.Show("Clicked a monitored row");
                if (dataGridViewIndex.Columns[e.ColumnIndex].Name == "Rebuild")
                {
                    CheckOneBoolColumnAgainstAnother(e.RowIndex, e.ColumnIndex, "Rebuild", "Reorganize");
                }
                else
                {
                    CheckOneBoolColumnAgainstAnother(e.RowIndex, e.ColumnIndex, "Reorganize", "Rebuild");
                }
            }
        }

        private void dataGridViewIndex_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewIndex_CellValueChanged(this, e);
        }

        private void dataGridViewIndex_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewIndex_CellValueChanged(this, e);
        }

        private void dataGridViewIndex_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewIndex_CellValueChanged(this, e);
        }

        private void dataGridViewIndex_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewIndex.IsCurrentCellDirty)
            {
                dataGridViewIndex.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
