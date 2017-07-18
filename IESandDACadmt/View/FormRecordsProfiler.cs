using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Lumension_Advanced_DB_Maintenance.Data;
using Lumension_Advanced_DB_Maintenance.Logging;
using System.Diagnostics;


namespace Lumension_Advanced_DB_Maintenance.Forms
{
    public partial class FormRecordsProfiler : Form
    {
        private DbSqlSpController theDbSqlController;
        private volatile Data.RecordsProfilingData _currentQueryData = new Data.RecordsProfilingData();
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
        

        public FormRecordsProfiler(DbSqlSpController liveDbSqlController)
        {
            theDbSqlController = liveDbSqlController;
            InitializeComponent();
            SetGuiForServerType();
        }

        private void SetGuiForServerType()
        {
            this.Text = "AC/DC Record Profiling Tool    Server: " + theDbSqlController.DbServeraddress;
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
            _maxValue = (_maxValue * 110)/100;
            _maxValue = Math.Round(_maxValue, 1);
            return _maxValue;
        }

        private void ProfilerSqlWorker(DbSqlSpController theDbSqlSpController, Data.RecordsProfilingData theCurrentQueryData)
        {
            _numQueriesStillRunning = 6;
            if (theDbSqlSpController.HeatServerType == DbSqlSpController.ServerType.EMSS)
            {
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EmssByDateQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByDateDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByDate);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EmssByTypeQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByTypeDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByType);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EmssByUserQuery, theDbSqlSpController.SqlConnectionString,
                        theCurrentQueryData.ByUserDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByUser);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EmssByComputerQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByComputerDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByComputer);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EmssByProcessQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByProcessDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByProcess);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EmssByDeviceQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByDeviceDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByDevice);
            }
            if (theDbSqlSpController.HeatServerType == DbSqlSpController.ServerType.ES)
            {
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EsByDateQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByDateDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByDate);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EsByTypeQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByTypeDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByType);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EsByUserQuery, theDbSqlSpController.SqlConnectionString,
                        theCurrentQueryData.ByUserDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByUser);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EsByComputerQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByComputerDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByComputer);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EsByProcessQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByProcessDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByProcess);
                RunSqlQuery(BL.RecordsProfilingQueryLogic.EsByDeviceQuery, theDbSqlSpController.SqlConnectionString,
                            theCurrentQueryData.ByDeviceDataRecords, BL.RecordsProfilingQueryLogic.DataQueryType.ByDevice);
            }
            
        }

        public void RunSqlQuery(string sqlQuery, string sqlConnectionString, DataTable queryResults, BL.RecordsProfilingQueryLogic.DataQueryType queryType)
        {
            bool querySuccess = false;
            try
            {
                using (var theSqlConnection = new SqlConnection(sqlConnectionString))
                using (var theSqlCommand = new SqlCommand(sqlQuery, theSqlConnection))
                {
                    // Open Connection
                    theSqlConnection.Open();
                    LoggingClass.SaveEventToLogFile(theDbSqlController.LogFileLocation, " SQL connection to profile " + queryType.ToString() + " is OPEN.");
                    theSqlCommand.CommandTimeout = 10800;
                    // Run query and fill in data table
                    queryResults.Load(theSqlCommand.ExecuteReader());
                    if (queryResults.Rows.Count >= 0)
                    {
                        LoggingClass.SaveEventToLogFile(theDbSqlController.LogFileLocation, " SQL Query Type " + queryType.ToString() + " succeeded.");
                        querySuccess = true;
                    }
                    theSqlCommand.Dispose();
                    theSqlConnection.Close();
                    if (querySuccess)
                    {
                        switch (queryType)
                        {
                            case BL.RecordsProfilingQueryLogic.DataQueryType.ByDate:
                                SetNewByDateDataResults(queryResults);
                                break;
                            case BL.RecordsProfilingQueryLogic.DataQueryType.ByType:
                                SetNewByTypeDataResults(queryResults);
                                break;
                            case BL.RecordsProfilingQueryLogic.DataQueryType.ByUser:
                                SetNewByUserDataResults(queryResults);
                                break;
                            case BL.RecordsProfilingQueryLogic.DataQueryType.ByComputer:
                                SetNewByComputerDataResults(queryResults);
                                break;
                            case BL.RecordsProfilingQueryLogic.DataQueryType.ByProcess:
                                SetNewByProcessDataResults(queryResults);
                                break;
                            case BL.RecordsProfilingQueryLogic.DataQueryType.ByDevice:
                                SetNewByDeviceDataResults(queryResults);
                                break;
                            default:
                                LoggingClass.SaveErrorToLogFile(theDbSqlController.LogFileLocation, " No Data Query Type Specified. Cannot same results.");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(theDbSqlController.LogFileLocation, " Error with " + queryType.ToString() + ":: " + ex.Message);
            }
        }

        private void SetNewByDateDataResults(DataTable queryResults)
        {
            if (this.ByDateChart.InvokeRequired)
            {
                SetByDateQueryDataCallBack del = new SetByDateQueryDataCallBack(SetNewByDateDataResults);
                this.Invoke(del, new object[] { queryResults });
            }
            else
            {
                _currentQueryData.ByDateDataRecords = queryResults;
                byDateDataGridView.Visible = true;
                labelDateRawDataProcessing.Visible = false;
                UpdateByDateCharts(_currentQueryData.ByDateDataRecords);
                _numQueriesStillRunning -= 1;
            }            
        }

        private void UpdateByDateCharts(DataTable queryResults)
        {
            LoggingClass.SaveEventToLogFile(theDbSqlController.LogFileLocation, " " + queryResults.Rows.Count + " records to add to graph.");
            byDateDataGridView.DataSource = queryResults;
            FillEventSelectionList(queryResults);
            BuildFilteredChartData("All");
            UpdateChartData();
            labelGraphedDateCountsProcessing.Visible = false;
            ByDateChart.Visible = true;
            }

        private void FillEventSelectionList(DataTable queryResults)
        {
            var foundEventTypes = (from DataRow row in queryResults.Rows
                select (string) row["ActionName"]).Distinct();
            cbEventTypesList.Items.Clear();
            cbEventTypesList.Items.Add("All");
            foreach (string eventTypeFound in foundEventTypes)
            {
                cbEventTypesList.Items.Add(eventTypeFound);
            }
        }

        private void SetNewByUserDataResults(DataTable queryResults)
        {
            if (this.byUserDataGridView.InvokeRequired)
            {
                SetByUserQueryDataCallBack del = new SetByUserQueryDataCallBack(SetNewByUserDataResults);
                this.Invoke(del, new object[] { queryResults });
            }
            else
            {
                _currentQueryData.ByUserDataRecords = queryResults;
                byUserDataGridView.DataSource = _currentQueryData.ByUserDataRecords;
                byUserDataGridView.AutoResizeColumns();
                byUserDataGridView.Visible = true;
                labelUserDataProcessing.Visible = false;
                _numQueriesStillRunning -= 1;
            }            
        }

        private void SetNewByComputerDataResults(DataTable queryResults)
        {
            if (this.byComputerDataGridView.InvokeRequired)
            {
                SetByComputerQueryDataCallBack del = new SetByComputerQueryDataCallBack(SetNewByComputerDataResults);
                this.Invoke(del, new object[] { queryResults });
            }
            else
            {
                _currentQueryData.ByComputerDataRecords = queryResults;
                byComputerDataGridView.DataSource = _currentQueryData.ByComputerDataRecords;
                byComputerDataGridView.AutoResizeColumns();
                byComputerDataGridView.Visible = true;
                labelComputerDataProcessing.Visible = false;
                _numQueriesStillRunning -= 1;
            }
        }

        private void SetNewByTypeDataResults(DataTable queryResults)
        {
            if (this.byTypeDataGridView.InvokeRequired)
            {
                SetByTypeQueryDataCallBack del = new SetByTypeQueryDataCallBack(SetNewByTypeDataResults);
                this.Invoke(del, new object[] { queryResults });
            }
            else
            {
                _currentQueryData.ByTypeDataRecords = queryResults;
                byTypeDataGridView.DataSource = _currentQueryData.ByTypeDataRecords;
                byTypeDataGridView.AutoResizeColumns();
                byTypeDataGridView.Visible = true;
                labelTypeDataProcessing.Visible = false;
                _numQueriesStillRunning -= 1;
            }
        }

        private void SetNewByProcessDataResults(DataTable queryResults)
        {
            if (this.byProcessDataGridView.InvokeRequired)
            {
                SetByProcessQueryDataCallBack del = new SetByProcessQueryDataCallBack(SetNewByProcessDataResults);
                this.Invoke(del, new object[] { queryResults });
            }
            else
            {
                // This can be an expensive set of results to get, so best to reuse them where possible,
                // so we set them into the DbSqlController data set.
                if (queryResults.Rows.Count >= 0)
                {
                    theDbSqlController.ByProcessResults.Clear();
                    foreach (DataRow row in queryResults.Rows)
                    {
                        theDbSqlController.ByProcessResults.Add(row["ProcessName"].ToString());
                    }
                    theDbSqlController.ByProcessQueryAlreadyRan = true;
                }
                _currentQueryData.ByProcessDataRecords = queryResults;
                byProcessDataGridView.DataSource = _currentQueryData.ByProcessDataRecords;
                byProcessDataGridView.AutoResizeColumns();
                byProcessDataGridView.Visible = true;
                labelProcessDataProcessing.Visible = false;
                _numQueriesStillRunning -= 1;
            }
        }

        private void SetNewByDeviceDataResults(DataTable queryResults)
        {
            if (this.byDeviceDataGridView.InvokeRequired)
            {
                SetByDeviceQueryDataCallBack del = new SetByDeviceQueryDataCallBack(SetNewByDeviceDataResults);
                this.Invoke(del, new object[] { queryResults });
            }
            else
            {
                _currentQueryData.ByDeviceDataRecords = queryResults;
                byDeviceDataGridView.DataSource = _currentQueryData.ByDeviceDataRecords;
                byDeviceDataGridView.AutoResizeColumns();
                byDeviceDataGridView.Visible = true;
                labelDeviceDataProcessing.Visible = false;
                _numQueriesStillRunning -= 1;
            }
        }

        private void FormSqlDataProfiler_Load(object sender, EventArgs e)
        {
            ModifyGuiForLoad();
            _currentQueryData.ByDateDataRecords = new DataTable();
            _currentQueryData.ByComputerDataRecords = new DataTable();
            _currentQueryData.ByDeviceDataRecords = new DataTable();
            _currentQueryData.ByProcessDataRecords = new DataTable();
            _currentQueryData.ByTypeDataRecords = new DataTable();
            _currentQueryData.ByUserDataRecords = new DataTable();
            _profilingSqlReadThread = new Thread(() => ProfilerSqlWorker(theDbSqlController, _currentQueryData));
            _profilingSqlReadThread.IsBackground = true;
            _profilingSqlReadThread.Start();
            timeProfilerActivity.Enabled = true;
        }

        private void ModifyGuiForLoad()
        {
            labelGraphedDateCountsProcessing.Visible = true;
            labelLineGraphProcessing.Visible = true;
            labelDateRawDataProcessing.Visible = true;
            labelComputerDataProcessing.Visible = true;
            labelUserDataProcessing.Visible = true;
            labelTypeDataProcessing.Visible = true;
            labelProcessDataProcessing.Visible = true;
            labelDeviceDataProcessing.Visible = true;
            ByDateChart.Visible = false;
            byDateDataGridView.Visible = false;
            byComputerDataGridView.Visible = false;
            byUserDataGridView.Visible = false;
            byTypeDataGridView.Visible = false;
            byProcessDataGridView.Visible = false;
            byDeviceDataGridView.Visible = false;
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
            ActivityTimerToPrint = "Querying "+ theDbSqlController.DbServeraddress + ": ";
            for (int i = 0; i < ProfilerActivityCountForProgressBars; i++)
            {
                ActivityTimerToPrint += "I";
            }
            if (labelGraphedDateCountsProcessing.Visible == true)
            {
                labelGraphedDateCountsProcessing.Text = ActivityTimerToPrint;
            }
            if (labelComputerDataProcessing.Visible == true)
            {
                labelComputerDataProcessing.Text = ActivityTimerToPrint;
            }
            if (labelDateRawDataProcessing.Visible == true)
            {
                labelDateRawDataProcessing.Text = ActivityTimerToPrint;
            }
            if (labelDeviceDataProcessing.Visible == true)
            {
                labelDeviceDataProcessing.Text = ActivityTimerToPrint;
            }
            if (labelLineGraphProcessing.Visible == true)
            {
                labelLineGraphProcessing.Text = ActivityTimerToPrint;
            }
            if (labelProcessDataProcessing.Visible == true)
            {
                labelProcessDataProcessing.Text = ActivityTimerToPrint;
            }
            if (labelUserDataProcessing.Visible == true)
            {
                labelUserDataProcessing.Text = ActivityTimerToPrint;
            }
            if (labelTypeDataProcessing.Visible == true)
            {
                labelTypeDataProcessing.Text = ActivityTimerToPrint;
            }
            if (_numQueriesStillRunning <= 0)
            {
                timeProfilerActivity.Enabled = false;
            }
        }

        private void buttonCloseProfiler_Click(object sender, EventArgs e)
        {
            _profilingSqlReadThread = null;
            this.Close();
        }

        private void buttonRerunAnalysis_Click(object sender, EventArgs e)
        {
            FormSqlDataProfiler_Load(this, new EventArgs());
        }

        private void byDateDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbEventTypesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildFilteredChartData((string)cbEventTypesList.Items[cbEventTypesList.SelectedIndex]);
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
            Logging.LoggingClass.SaveEventToLogFile(theDbSqlController.LogFileLocation, " Rolling Average calculated for events.");
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
                        _currentQueryData.FilteredChartData[theDate] =  theCount + _currentQueryData.FilteredChartData[theDate];
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
            DataTable byType = _currentQueryData.ByTypeDataRecords.Copy();
            outputData.Tables.Add(byType);
            DataTable byProcess = _currentQueryData.ByProcessDataRecords.Copy();
            outputData.Tables.Add(byProcess);
            DataTable byDevice = _currentQueryData.ByDeviceDataRecords.Copy();
            outputData.Tables.Add(byDevice);
            // Ask User where to save the file to?
            SaveFileDialog outputFileLocation = new SaveFileDialog();
            string filename = theDbSqlController.DbServeraddress + "_AC-DC_Records_Profile_" + DateTime.Now;
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
                MessageBox.Show("Error when saving data to excel file. Please see Log File for more details.", "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggingClass.SaveErrorToLogFile(theDbSqlController.LogFileLocation, ex.Message);
            }
        }

        private void buttonOpenLogFile_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", theDbSqlController.LogFileLocation);
        }
    }
}
