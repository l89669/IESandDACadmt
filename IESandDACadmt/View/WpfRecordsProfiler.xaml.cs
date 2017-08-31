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
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using IESandDACadmt.Model;
using IESandDACadmt.Model.Sql;
using IESandDACadmt.Model.Logging;
using IESandDACadmt.ViewModel;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfRecordsProfiler.xaml
    /// </summary>
    public partial class WpfRecordsProfiler : Window
    {
        public WpfRecordsProfiler(Model.DbSqlSpController liveDbSqlController, Model.Logging.ILogging theLogger)
        {
            theDbSqlController = liveDbSqlController;

            InitializeComponent();

            Loaded += WpfRecordsProfilerUi_Load;

            _theLogger = theLogger;

            ByDateChart.Series.Add("Series1");
            ByDateChart.Series.Add("SeriesRollingAverage");
            ByDateChart.ChartAreas.Add("ChartArea1");

            SetGuiForServerType();
        }

        private Model.Logging.ILogging _theLogger;
        private DbSqlSpController theDbSqlController;
        private volatile ViewModel.RecordsProfilingData _currentQueryData = new ViewModel.RecordsProfilingData();
        private double _maxValue = 1;
        private int _rollingAverageSpan = 30;

        public int ProfilerActivityCountForProgressBars = 0;
        public string ActivityTimerToPrint = "";

        private Thread _profilingSqlReadThread = null;

        public volatile int _numQueriesStillRunning = 0;

        delegate void SetByUserQueryDataCallBack(DataTable results);
        delegate void SetByComputerQueryDataCallBack(DataTable results);
        delegate void SetByDateQueryDataCallBack(DataTable results);
        delegate void SetByTypeQueryDataCallBack(DataTable results);
        delegate void SetByProcessQueryDataCallBack(DataTable results);
        delegate void SetByDeviceQueryDataCallBack(DataTable results);

        System.Windows.Threading.DispatcherTimer timeProfilerActivity = new System.Windows.Threading.DispatcherTimer();

        private void SetGuiForServerType()
        {
            this.Title = "AC/DC Record Profiling Tool    Server: " + theDbSqlController.DbSqlSpControllerData.DbServeraddress;
        }

        private void BuildByDateLineGraph(DataTable queryResults)
        {
            ByDateChart.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            ByDateChart.ChartAreas["ChartArea1"].AxisX.Maximum = queryResults.Rows.Count;
            if (queryResults.Rows.Count < 30)
            {
                ByDateChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            }
            if (queryResults.Rows.Count > 30 && queryResults.Rows.Count < 210)
            {
                ByDateChart.ChartAreas["ChartArea1"].AxisX.Interval = 7;
            }
            if (queryResults.Rows.Count > 210)
            {
                ByDateChart.ChartAreas["ChartArea1"].AxisX.Interval = 30;
            }
        }


        private double FindMaxEventCount(DataTable queryResults)
        {
            foreach (DataRow row in queryResults.Rows)
            {
                if (Double.Parse(row["Count"].ToString()) > _maxValue)
                {
                    _maxValue = Double.Parse(row["Count"].ToString());
                }
            }
            _maxValue = (_maxValue * 110) / 100;
            _maxValue = Math.Round(_maxValue, 1);
            return _maxValue;
        }

        private void ProfilerSqlWorker(DbSqlSpController theDbSqlSpController, ViewModel.RecordsProfilingData theCurrentQueryData)
        {
            _numQueriesStillRunning = 6;
            if (theDbSqlSpController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.EMSS)
            {
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EmssByDateQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByDateDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByDate);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EmssByTypeQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByEventTypeDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByType);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EmssByUserQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                        theCurrentQueryData.ByUserDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByUser);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EmssByComputerQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByComputerDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByComputer);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EmssByProcessQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByProcessDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByProcess);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EmssByDeviceQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByDeviceDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByDevice);
            }
            if (theDbSqlSpController.DbSqlSpControllerData.HeatServerType == DbSqlSpControllerData.ServerType.ES)
            {
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EsByDateQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByDateDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByDate);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EsByTypeQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByEventTypeDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByType);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EsByUserQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                        theCurrentQueryData.ByUserDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByUser);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EsByComputerQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByComputerDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByComputer);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EsByProcessQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByProcessDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByProcess);
                RunSqlQuery(Model.RecordsProfilingQueryLogic.EsByDeviceQuery, theDbSqlSpController.DbSqlSpControllerData.SqlConnectionString,
                            theCurrentQueryData.ByDeviceDataRecords, Model.RecordsProfilingQueryLogic.DataQueryType.ByDevice);
            }

        }

        public void RunSqlQuery(string sqlQuery, string sqlConnectionString, DataTable queryResults, Model.RecordsProfilingQueryLogic.DataQueryType queryType)
        {
            bool querySuccess = false;
            try
            {
                using (var theSqlConnection = new SqlConnection(sqlConnectionString))
                using (var theSqlCommand = new SqlCommand(sqlQuery, theSqlConnection))
                {
                    // Open Connection
                    theSqlConnection.Open();
                    _theLogger.SaveEventToLogFile(" SQL connection to profile " + queryType.ToString() + " is OPEN.");
                    theSqlCommand.CommandTimeout = 10800;
                    // Run query and fill in data table
                    queryResults.Load(theSqlCommand.ExecuteReader());
                    if (queryResults.Rows.Count >= 0)
                    {
                        _theLogger.SaveEventToLogFile(  " SQL Query Type " + queryType.ToString() + " succeeded.");
                        querySuccess = true;
                    }
                    theSqlCommand.Dispose();
                    theSqlConnection.Close();
                    if (querySuccess)
                    {
                        switch (queryType)
                        {
                            case Model.RecordsProfilingQueryLogic.DataQueryType.ByDate:
                                SetNewByDateDataResults(queryResults);
                                break;
                            case Model.RecordsProfilingQueryLogic.DataQueryType.ByType:
                                SetNewByTypeDataResults(queryResults);
                                break;
                            case Model.RecordsProfilingQueryLogic.DataQueryType.ByUser:
                                SetNewByUserDataResults(queryResults);
                                break;
                            case Model.RecordsProfilingQueryLogic.DataQueryType.ByComputer:
                                SetNewByComputerDataResults(queryResults);
                                break;
                            case Model.RecordsProfilingQueryLogic.DataQueryType.ByProcess:
                                SetNewByProcessDataResults(queryResults);
                                break;
                            case Model.RecordsProfilingQueryLogic.DataQueryType.ByDevice:
                                SetNewByDeviceDataResults(queryResults);
                                break;
                            default:
                                _theLogger.SaveErrorToLogFile(  " No Data Query Type Specified. Cannot same results.");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _theLogger.SaveErrorToLogFile(  " Error with " + queryType.ToString() + ":: " + ex.Message);
            }
        }

        private void SetNewByDateDataResults(DataTable queryResults)
        {
            if (ByDateRawStackPanel.Dispatcher.CheckAccess() == false)
            {
                SetByDateQueryDataCallBack del = new SetByDateQueryDataCallBack(SetNewByDateDataResults);
                ByDateRawStackPanel.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, del, queryResults);
            }
            else
            {
                _currentQueryData.ByDateDataRecords = queryResults;
                ByDateDataGrid.DataContext = _currentQueryData.ByDateDataRecords.DefaultView;
                ByDateRawStackPanel.IsEnabled = true;
                UpdateByDateCharts(_currentQueryData.ByDateDataRecords);
                _numQueriesStillRunning -= 1;
            }
        }

        private void UpdateByDateCharts(DataTable queryResults)
        {
            _theLogger.SaveEventToLogFile(  " " + queryResults.Rows.Count + " records to add to graph.");
            ByDateDataGrid.DataContext = queryResults.DefaultView;
            FillEventSelectionList(queryResults);
            BuildFilteredChartData("All");
            UpdateChartData();
            //labelGraphedDateCountsProcessing.Visible = false;
            ByDateChart.Visible = true;
        }

        private void FillEventSelectionList(DataTable queryResults)
        {
            var foundEventTypes = (from DataRow row in queryResults.Rows
                                   select (string)row["ActionName"]).Distinct();
            EventTypesList.Items.Clear();
            EventTypesList.Items.Add("All");
            foreach (string eventTypeFound in foundEventTypes)
            {
                EventTypesList.Items.Add(eventTypeFound);
            }
        }

        private void SetNewByUserDataResults(DataTable queryResults)
        {
            if (ByUserStackPanel.Dispatcher.CheckAccess() == false)
            {
                SetByUserQueryDataCallBack del = new SetByUserQueryDataCallBack(SetNewByUserDataResults);
                ByUserStackPanel.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, del, queryResults);
            }
            else
            {
                _currentQueryData.ByUserDataRecords = queryResults;
                ByUserDataGrid.DataContext = _currentQueryData.ByUserDataRecords.DefaultView;
                ByUserStackPanel.IsEnabled = true;
                _numQueriesStillRunning -= 1;
            }
        }

        private void SetNewByComputerDataResults(DataTable queryResults)
        {
            if (ByComputerStackPanel.Dispatcher.CheckAccess() == false)
            {
                SetByComputerQueryDataCallBack del = new SetByComputerQueryDataCallBack(SetNewByComputerDataResults);
                ByComputerStackPanel.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, del, queryResults);
            }
            else
            {
                _currentQueryData.ByComputerDataRecords = queryResults;
                ByComputerDataGrid.DataContext = _currentQueryData.ByComputerDataRecords.DefaultView;
                ByComputerStackPanel.IsEnabled = true;
                _numQueriesStillRunning -= 1;
            }
            //if (this.byComputerDataGridView.InvokeRequired)
             //{
             //    SetByComputerQueryDataCallBack del = new SetByComputerQueryDataCallBack(SetNewByComputerDataResults);
             //    this.Invoke(del, new object[] { queryResults });
             //}
             //else
             //{
                //_currentQueryData.ByComputerDataRecords = queryResults;
                //ByComputerDataGrid.DataContext = _currentQueryData.ByComputerDataRecords.DefaultView;
                //ByComputerStackPanel.IsEnabled = true;
                //byComputerDataGridView.AutoResizeColumns();
                //byComputerDataGridView.Visible = true;
                //labelComputerDataProcessing.Visible = false;
                //_numQueriesStillRunning -= 1;
            //}
        }

        private void SetNewByTypeDataResults(DataTable queryResults)
        {
            if (ByEventTypeStackPanel.Dispatcher.CheckAccess() == false)
            {
                SetByTypeQueryDataCallBack del = new SetByTypeQueryDataCallBack(SetNewByTypeDataResults);
                ByEventTypeStackPanel.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, del, queryResults);
            }
            else
            {
                _currentQueryData.ByEventTypeDataRecords = queryResults;
                ByEventTypeDataGrid.DataContext = _currentQueryData.ByEventTypeDataRecords.DefaultView;
                ByEventTypeStackPanel.IsEnabled = true;
                _numQueriesStillRunning -= 1;
            }
            //if (this.byTypeDataGridView.InvokeRequired)
             //{
             //    SetByTypeQueryDataCallBack del = new SetByTypeQueryDataCallBack(SetNewByTypeDataResults);
             //    this.Invoke(del, new object[] { queryResults });
             //}
             //else
             //{
                //_currentQueryData.ByEventTypeDataRecords = queryResults;
                //ByEventTypeDataGrid.DataContext = _currentQueryData.ByEventTypeDataRecords.DefaultView;
                //ByEventTypeStackPanel.IsEnabled = true;
                //byTypeDataGridView.AutoResizeColumns();
                //byTypeDataGridView.Visible = true;
                //labelTypeDataProcessing.Visible = false;
                //_numQueriesStillRunning -= 1;
            //}
        }

        private void SetNewByProcessDataResults(DataTable queryResults)
        {
            if (ByProcessStackPanel.Dispatcher.CheckAccess() == false)
            {
                SetByProcessQueryDataCallBack del = new SetByProcessQueryDataCallBack(SetNewByProcessDataResults);
                ByProcessStackPanel.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, del, queryResults);
            }
            else
            {
                if (queryResults.Rows.Count >= 0)
                {
                    theDbSqlController.DbSqlSpControllerData.ByProcessResults.Clear();
                    foreach (DataRow row in queryResults.Rows)
                    {
                        theDbSqlController.DbSqlSpControllerData.ByProcessResults.Add(row["ProcessName"].ToString());
                    }
                    theDbSqlController.DbSqlSpControllerData.ByProcessQueryAlreadyRan = true;
                }
                _currentQueryData.ByProcessDataRecords = queryResults;
                ByProcessDataGrid.DataContext = _currentQueryData.ByProcessDataRecords.DefaultView;
                ByProcessStackPanel.IsEnabled = true;
                _numQueriesStillRunning -= 1;
            }
            //if (this.byProcessDataGridView.InvokeRequired)
            //{
            //    SetByProcessQueryDataCallBack del = new SetByProcessQueryDataCallBack(SetNewByProcessDataResults);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            // This can be an expensive set of results to get, so best to reuse them where possible,
            // so we set them into the DbSqlController data set.
            //if (queryResults.Rows.Count >= 0)
            //    {
            //        theDbSqlController.DbSqlSpControllerData.ByProcessResults.Clear();
            //        foreach (DataRow row in queryResults.Rows)
            //        {
            //            theDbSqlController.DbSqlSpControllerData.ByProcessResults.Add(row["ProcessName"].ToString());
            //        }
            //        theDbSqlController.DbSqlSpControllerData.ByProcessQueryAlreadyRan = true;
            //    }
            //    _currentQueryData.ByProcessDataRecords = queryResults;
            //    ByProcessDataGrid.DataContext = _currentQueryData.ByProcessDataRecords.DefaultView;
            //    ByProcessStackPanel.IsEnabled = true;
                //byProcessDataGridView.AutoResizeColumns();
                //byProcessDataGridView.Visible = true;
                //labelProcessDataProcessing.Visible = false;
                //_numQueriesStillRunning -= 1;
            //}
        }

        private void SetNewByDeviceDataResults(DataTable queryResults)
        {
            if (ByDeviceStackPanel.Dispatcher.CheckAccess() == false)
            {
                SetByDeviceQueryDataCallBack del = new SetByDeviceQueryDataCallBack(SetNewByDeviceDataResults);
                ByDeviceStackPanel.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, del, queryResults);
            }
            else
            {
                _currentQueryData.ByDeviceDataRecords = queryResults;
                ByDeviceDataGrid.DataContext = _currentQueryData.ByDeviceDataRecords.DefaultView;
                ByDeviceStackPanel.IsEnabled = true;
                _numQueriesStillRunning -= 1;
            }
            //if (this.byDeviceDataGridView.InvokeRequired)
            //{
            //    SetByDeviceQueryDataCallBack del = new SetByDeviceQueryDataCallBack(SetNewByDeviceDataResults);
            //    this.Invoke(del, new object[] { queryResults });
            //}
            //else
            //{
            //_currentQueryData.ByDeviceDataRecords = queryResults;
            //    ByDeviceDataGrid.DataContext = _currentQueryData.ByDeviceDataRecords.DefaultView;
            //    ByDeviceStackPanel.IsEnabled = true;
            //    //byDeviceDataGridView.AutoResizeColumns();
            //    //byDeviceDataGridView.Visible = true;
            //    //labelDeviceDataProcessing.Visible = false;
            //    _numQueriesStillRunning -= 1;
            //}
        }

        private void WpfRecordsProfilerUi_Load(object sender, EventArgs e)
        {
            ModifyGuiForLoad();
            _currentQueryData.ByDateDataRecords = new DataTable();
            _currentQueryData.ByComputerDataRecords = new DataTable();
            _currentQueryData.ByDeviceDataRecords = new DataTable();
            _currentQueryData.ByProcessDataRecords = new DataTable();
            _currentQueryData.ByEventTypeDataRecords = new DataTable();
            _currentQueryData.ByUserDataRecords = new DataTable();
            _profilingSqlReadThread = new Thread(() => ProfilerSqlWorker(theDbSqlController, _currentQueryData));
            _profilingSqlReadThread.IsBackground = true;
            _profilingSqlReadThread.Start();
            timeProfilerActivity.IsEnabled = true;
        }

        private void ModifyGuiForLoad()
        {
            //labelGraphedDateCountsProcessing.Visible = true;
            //labelLineGraphProcessing.Visible = true;
            //labelDateRawDataProcessing.Visible = true;
            //labelComputerDataProcessing.Visible = true;
            //labelUserDataProcessing.Visible = true;
            //labelTypeDataProcessing.Visible = true;
            //labelProcessDataProcessing.Visible = true;
            //labelDeviceDataProcessing.Visible = true;
            //ByDateChart.Visible = false;

            //byDateDataGridView.Visible = false;
            //byComputerDataGridView.Visible = false;
            //byUserDataGridView.Visible = false;
            //byTypeDataGridView.Visible = false;
            //byProcessDataGridView.Visible = false;
            //byDeviceDataGridView.Visible = false;

            ByDateRawStackPanel.IsEnabled = false;
            ByDateGraphedStackPanel.IsEnabled = false;
            ByUserStackPanel.IsEnabled = false;
            ByComputerStackPanel.IsEnabled = false;
            ByEventTypeStackPanel.IsEnabled = false;
            ByProcessStackPanel.IsEnabled = false;
            ByDeviceStackPanel.IsEnabled = false;
        }

        private void timeProfilerActivity_Tick(object sender, EventArgs e)
        {
            if (ProfilerActivityCountForProgressBars == 10)
            {
                ProfilerActivityCountForProgressBars = 1;
            }
            else
            {
                ProfilerActivityCountForProgressBars += 1;
            }
            //ActivityTimerToPrint = "Querying " + theDbSqlController.DbSqlSpControllerData.DbServeraddress + ": ";
            for (int i = 0; i < ProfilerActivityCountForProgressBars; i++)
            {
                ActivityTimerToPrint += "I";
            }
            if (ByDateGraphedStackPanel.IsEnabled == true)
            {
                ByDateGraphedTab.Header = "By Date (Graphed): " + ActivityTimerToPrint;
            }
            if (ByDateRawStackPanel.IsEnabled == true)
            {
                ByDateRawTab.Header = "By Date (Raw): " + ActivityTimerToPrint;
            }
            if (ByUserStackPanel.IsEnabled == true)
            {
                ByUserTab.Header = "By User: " + ActivityTimerToPrint;
            }
            if (ByComputerStackPanel.IsEnabled == true)
            {
                ByComputerTab.Header = "By Computer: " + ActivityTimerToPrint;
            }
            if (ByEventTypeStackPanel.IsEnabled == true)
            {
                ByEventTypeTab.Header = "By Event Type: " + ActivityTimerToPrint;
            }
            if (ByProcessStackPanel.IsEnabled == true)
            {
                ByProcessTab.Header = "By Process: " + ActivityTimerToPrint;
            }
            if (ByDeviceStackPanel.IsEnabled == true)
            {
                ByComputerTab.Header = "By Device: " + ActivityTimerToPrint;
            }
            if (_numQueriesStillRunning <= 0)
            {
                timeProfilerActivity.IsEnabled = false;
            }
        }

        private void buttonCloseProfiler_Click(object sender, EventArgs e)
        {
            _profilingSqlReadThread = null;
            this.Close();
        }

        private void buttonRerunAnalysis_Click(object sender, EventArgs e)
        {
            WpfRecordsProfilerUi_Load(this, new EventArgs());
        }

        private void byDateDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void EventTypesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildFilteredChartData((string)EventTypesList.Items[EventTypesList.SelectedIndex]);
            UpdateChartData();
        }

        private void UpdateChartData()
        {
            ByDateChart.Series["Series1"].Points.Clear();
            ByDateChart.Series["SeriesRollingAverage"].Points.Clear();
            foreach (KeyValuePair<string, double> entry in _currentQueryData.FilteredChartData)
            {
                ByDateChart.Series["Series1"].Points.AddXY(entry.Key.ToString(), entry.Value);
            }
            CalculateRollingAverage(_currentQueryData.FilteredChartData);
        }

        private void CalculateRollingAverage(Dictionary<string, double> filteredChartData)
        {
            Queue<double> rollingAverageQueue = new Queue<double>();
            double dequeued = 0.0;
            double rollingAverage = 0.0;
            foreach (KeyValuePair<string, double> entry in _currentQueryData.FilteredChartData)
            {
                if (rollingAverageQueue.Count <= _rollingAverageSpan)
                {
                    rollingAverageQueue.Enqueue(entry.Value);
                }
                else
                {
                    dequeued = rollingAverageQueue.Dequeue();
                    rollingAverageQueue.Enqueue(entry.Value);
                }
                rollingAverage = rollingAverageQueue.Sum() / rollingAverageQueue.Count;
                ByDateChart.Series["SeriesRollingAverage"].Points.AddXY(entry.Key.ToString(), rollingAverage);
            }
            _theLogger.SaveEventToLogFile(  " Rolling Average calculated for events.");
        }

        private void BuildFilteredChartData(string selectedEventFilter)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.ToString(), true);
            _currentQueryData.FilteredChartData.Clear();
            if (selectedEventFilter == "All")
            {
                foreach (DataRow row in _currentQueryData.ByDateDataRecords.Rows)
                {
                    string[] splitDateTime = row["Date"].ToString().Split(null, 2);
                    string theDate = splitDateTime[0];
                    double theCount = Double.Parse(row["count"].ToString());
                    if (_currentQueryData.FilteredChartData.ContainsKey(theDate))
                    {
                        _currentQueryData.FilteredChartData[theDate] = theCount + _currentQueryData.FilteredChartData[theDate];
                    }
                    else
                    {
                        _currentQueryData.FilteredChartData.Add(theDate, theCount);
                    }
                }
            }
            else
            {
                foreach (DataRow row in _currentQueryData.ByDateDataRecords.Rows)
                {
                    double theCount = Double.Parse(row["count"].ToString());
                    if (row["ActionName"].ToString() == selectedEventFilter)
                    {
                        string[] splitDateTime = row["Date"].ToString().Split(null, 2);
                        string theDate = splitDateTime[0];
                        _currentQueryData.FilteredChartData.Add(theDate, theCount);
                    }
                }
            }

        }

        private void ByDateChart_Click(object sender, EventArgs e)
        {

        }

        private void byDeviceDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonExportToFile_Click(object sender, EventArgs e)
        {
            DataSet outputData = new DataSet("Excel Output");
            // Ask question to user to save all tabs or just this tab?         
            //pump all DGV's a into multi-sheet XLS file
            DataTable byDate = _currentQueryData.ByDateDataRecords.Copy();
            outputData.Tables.Add(byDate);
            DataTable byUser = _currentQueryData.ByUserDataRecords.Copy();
            outputData.Tables.Add(byUser);
            DataTable byComputer = _currentQueryData.ByComputerDataRecords.Copy();
            outputData.Tables.Add(byComputer);
            DataTable byType = _currentQueryData.ByEventTypeDataRecords.Copy();
            outputData.Tables.Add(byType);
            DataTable byProcess = _currentQueryData.ByProcessDataRecords.Copy();
            outputData.Tables.Add(byProcess);
            DataTable byDevice = _currentQueryData.ByDeviceDataRecords.Copy();
            outputData.Tables.Add(byDevice);
            // Ask User where to save the file to?
            SaveFileDialog outputFileLocation = new SaveFileDialog();
            string filename = theDbSqlController.DbSqlSpControllerData.DbServeraddress + "_AC-DC_Records_Profile_" + DateTime.Now;
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
                System.Windows.MessageBox.Show("Error when saving data to excel file. Please see Log File for more details.", "Error saving file", MessageBoxButton.OK, MessageBoxImage.Error);
                _theLogger.SaveErrorToLogFile(  ex.Message);
            }
        }

        private void buttonOpenLogFile_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", theDbSqlController.DbSqlSpControllerData.LogFileLocation);
        }
    }
}
