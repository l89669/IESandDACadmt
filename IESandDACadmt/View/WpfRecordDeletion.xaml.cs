using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IESandDACadmt.Model;
using IESandDACadmt.Model.Logging;
using IESandDACadmt.Model.Sql;
using IESandDACadmt.ViewModel;
using IESandDACadmt.View;
using System.Globalization;
using System.Diagnostics;
using System.Data;

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfRecordDeletion.xaml
    /// </summary>
    public partial class WpfRecordDeletion : Window
    {
        public WpfRecordDeletion(Model.DbSqlSpController theDbSqlSpController, Model.Logging.ILogging theLogger)
        {
            LiveDbSpSqlController = theDbSqlSpController;
            LiveDbSpSqlControllerData = LiveDbSpSqlController.DbSqlSpControllerData;
            _theLogger = theLogger;
            this.DataContext = LiveDbSpSqlControllerData;
            LiveDbSpSqlController.BuildEventTypesDictionary();
            InitializeComponent();
            cbSpecificUser.Items.Clear();
            cbSpecificComputer.Items.Clear();
            Loaded += Form1_Load;
            processingStatsTimer.Interval = new TimeSpan(0,0,1);
            timerCalcTotRecToPurge.Interval = new TimeSpan(0, 0, 1);
            readByProcessInfoTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private Model.Logging.ILogging _theLogger;
        public volatile DbSqlSpController LiveDbSpSqlController;
        IESandDACadmt.ViewModel.DbSqlSpControllerData LiveDbSpSqlControllerData = new DbSqlSpControllerData();
        //Thread _testDbConnectionThread = null;
        Thread _readByProcessSqlInfoThread = null;
        Thread _calculateTotalRecordsToPurgeThread = null;
        Thread _sqlPurgeWorkerThread = null;

        System.Windows.Threading.DispatcherTimer processingStatsTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer timerCalcTotRecToPurge = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer readByProcessInfoTimer = new System.Windows.Threading.DispatcherTimer();
        
        delegate void UpdateGuiWithNewValuesCallBack(DbSqlSpController theLiveDbSpSqlController);


        private void SetGuiForServerType()
        {
            this.Title = "AC/DC Record Deletion Tool    Server: " + LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress;
        }

        private void LoadUsersComputersIntoGui()
        {
            cbSpecificUser.Items.Clear();
            cbSpecificComputer.Items.Clear();
            if (LiveDbSpSqlController.DbSqlSpControllerData.UserList.Count > 0)
            {
                foreach (var user in LiveDbSpSqlController.DbSqlSpControllerData.UserList)
                {
                    cbSpecificUser.Items.Add(user);
                }
            }
            else
            {
                cbSpecificUser.Items.Add("No Users Found");
            }
            if (LiveDbSpSqlController.DbSqlSpControllerData.ComputerList.Count > 0)
            {
                foreach (var computer in LiveDbSpSqlController.DbSqlSpControllerData.ComputerList)
                {
                    cbSpecificComputer.Items.Add(computer);
                }
            }
            else
            {
                cbSpecificComputer.Items.Add("No Computers Found");
            }
        }

        //private static void cbSpecificUser_DropDown(object sender, System.EventArgs e)
        //{
        //    ComboBox senderComboBox = (ComboBox)sender;
        //    int width = senderComboBox.DropDownWidth;
        //    Graphics g = senderComboBox.CreateGraphics();
        //    Font font = senderComboBox.Font;
        //    int vertScrollBarWidth =
        //        (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
        //        ? SystemInformation.VerticalScrollBarWidth : 0;

        //    foreach (string s in ((ComboBox)sender).Items)
        //    {
        //        var newWidth = (int)g.MeasureString(s, font).Width
        //                       + vertScrollBarWidth;
        //        if (width < newWidth)
        //        {
        //            width = newWidth;
        //        }
        //    }
        //    senderComboBox.DropDownWidth = width;
        //}

        //private void cbSpecificComputer_DropDown(object sender, System.EventArgs e)
        //{
        //    ComboBox senderComboBox = (ComboBox)sender;
        //    int width = senderComboBox.DropDownWidth;
        //    Graphics g = senderComboBox.CreateGraphics();
        //    Font font = senderComboBox.Font;
        //    int vertScrollBarWidth =
        //        (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
        //        ? SystemInformation.VerticalScrollBarWidth : 0;

        //    foreach (string s in ((ComboBox)sender).Items)
        //    {
        //        var newWidth = (int)g.MeasureString(s, font).Width
        //                       + vertScrollBarWidth;
        //        if (width < newWidth)
        //        {
        //            width = newWidth;
        //        }
        //    }
        //    senderComboBox.DropDownWidth = width;
        //}

        private void ModifyGuiAfterGoPromptCanceled()
        {
            if (rbSelectUser.IsChecked == true)
            {
                rbEveryone.IsEnabled = true;
                rbSelectUser.IsEnabled = true;
                cbSpecificUser.IsEnabled = true;
                rbEveryone.IsChecked = false;
                rbSelectUser.IsChecked = true;
            }
            else
            {
                rbEveryone.IsEnabled = true;
                rbSelectUser.IsEnabled = true;
                cbSpecificUser.IsEnabled = false;
                rbEveryone.IsChecked = true;
                rbSelectUser.IsChecked = false;
            }

            if (rbSelectComputer.IsChecked == true)
            {
                rbAllComputers.IsEnabled = true;
                rbSelectComputer.IsEnabled = true;
                cbSpecificComputer.IsEnabled = true;
                rbAllComputers.IsChecked = false;
                rbSelectComputer.IsChecked = true;
            }
            else
            {
                rbAllComputers.IsEnabled = true;
                rbSelectComputer.IsEnabled = true;
                cbSpecificComputer.IsEnabled = false;
                rbAllComputers.IsChecked = true;
                rbSelectComputer.IsChecked = false;
            }

            EventsCriteriaGrid.IsEnabled = true;
            StopStartButtonGrid.IsEnabled = true;
            ProgressGrid.IsEnabled = false;

            //AlterPurgeCriteriaPanel(true);
            //AlterStartStopPanel(true);
            //AlterPurgeStatsPanel(false);

            //btnStopCleanup.IsEnabled = false;
            //btnStartCleanup.IsEnabled = true;

            //EventsCriteriaGrid.BackColor = SystemColors.ControlLightLight;
            //StopStartButtonGrid.BackColor = SystemColors.ControlLightLight;
            //ProgressGrid.BackColor = SystemColors.Control;

            //btnStartCleanup.BackColor = SystemColors.Control;
            //btnStopCleanup.BackColor = SystemColors.Control;
            //btnStartCleanup.IsEnabled = true;
            //btnStopCleanup.IsEnabled = false;

            toolStripStatusLabel1.Text = "Record deletion canceled";
        }

        private void ModifyGuiAfterNoTargetRecordsFound()
        {
            if (rbSelectUser.IsChecked == true)
            {
                rbEveryone.IsEnabled = true;
                rbSelectUser.IsEnabled = true;
                cbSpecificUser.IsEnabled = true;
                rbEveryone.IsChecked = false;
                rbSelectUser.IsChecked = true;
            }
            else
            {
                rbEveryone.IsEnabled = true;
                rbSelectUser.IsEnabled = true;
                cbSpecificUser.IsEnabled = false;
                rbEveryone.IsChecked = true;
                rbSelectUser.IsChecked = false;
            }

            if (rbSelectComputer.IsChecked == true)
            {
                rbAllComputers.IsEnabled = true;
                rbSelectComputer.IsEnabled = true;
                cbSpecificComputer.IsEnabled = true;
                rbAllComputers.IsChecked = false;
                rbSelectComputer.IsChecked = true;
            }
            else
            {
                rbAllComputers.IsEnabled = true;
                rbSelectComputer.IsEnabled = true;
                cbSpecificComputer.IsEnabled = false;
                rbAllComputers.IsChecked = true;
                rbSelectComputer.IsChecked = false;
            }
            EventsCriteriaGrid.IsEnabled = true;
            StopStartButtonGrid.IsEnabled = true;
            ProgressGrid.IsEnabled = false;
            //AlterPurgeCriteriaPanel(true);
            //AlterStartStopPanel(true);
            //AlterPurgeStatsPanel(false);

            //btnStopCleanup.IsEnabled = false;
            //btnStartCleanup.IsEnabled = true;

            //EventsCriteriaGrid.BackColor = SystemColors.ControlLightLight;
            //StopStartButtonGrid.BackColor = SystemColors.ControlLightLight;
            //ProgressGrid.BackColor = SystemColors.Control;

            //btnStartCleanup.BackColor = SystemColors.Control;
            //btnStopCleanup.BackColor = SystemColors.Control;
            //btnStartCleanup.IsEnabled = true;
            //btnStopCleanup.IsEnabled = false;

            toolStripStatusLabel1.Text = "No target records found. Action canceled";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsersComputersIntoGui();
            ModifyGuiAfterFormLoad();
            _theLogger.SaveEventToLogFile(  " The known Users and Computers read from SQL and loaded into GUI.");
            //cbSpecificComputer.DropDown += new System.EventHandler(this.cbSpecificComputer_DropDown);
            //cbSpecificUser.DropDown += new System.EventHandler(cbSpecificUser_DropDown);
            toolStripStatusLabel1.Text = "Connected to:" + LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress + " User:" + LiveDbSpSqlController.DbSqlSpControllerData.SqlConnUserName;
        }

        private void ModifyGuiAfterFormLoad()
        {
            //EventsCriteriaGrid.BackColor = SystemColors.ControlLightLight;
            //StopStartButtonGrid.BackColor = SystemColors.ControlLightLight;
            //ProgressGrid.BackColor = SystemColors.Control;
            EventsCriteriaGrid.IsEnabled = true;
            StopStartButtonGrid.IsEnabled = true;
            ProgressGrid.IsEnabled = false;
            //AlterPurgeCriteriaPanel(true);
            //AlterStartStopPanel(true);
            //AlterPurgeStatsPanel(false);

            rbEveryone.IsChecked = true;
            rbEveryone.IsEnabled = true;
            rbSelectUser.IsChecked = false;
            rbSelectUser.IsEnabled = true;
            cbSpecificUser.IsEnabled = false;

            rbAllComputers.IsChecked = true;
            rbAllComputers.IsEnabled = true;
            rbSelectComputer.IsChecked = false;
            rbSelectComputer.IsEnabled = true;
            cbSpecificComputer.IsEnabled = false;

            rbAllProcesses.IsChecked = true;
            rbAllProcesses.IsEnabled = true;
            rbSpecificProcess.IsChecked = false;
            rbSpecificProcess.IsEnabled = true;
            cbSpecificProcess.IsEnabled = false;

            //btnStartCleanup.BackColor = SystemColors.Control;
            //btnStopCleanup.BackColor = SystemColors.Control;
            btnStartCleanup.IsEnabled = true;
            btnStopCleanup.IsEnabled = false;

            dtpCutOffDate.DisplayDate = DateTime.Today.AddDays(-1);
            rbCutOffDate.IsChecked = true;
            rbNoCutOffDate.IsChecked = false;
            cbBatchSize.SelectedIndex = 1;
            toolStripProgressBar1.Value = 0;
        }

        //private void AlterPurgeStatsPanel(bool state)
        //{
        //    ProgressGrid.IsEnabled = state;
        //    lblRecordsToPurge.IsEnabled = state;
        //    RecordsLeftToPurgeTextBox.IsEnabled = state;
        //    lblRecordsPurged.IsEnabled = state;
        //    RecordsPurgedTextBox.IsEnabled = state;
        //    lblPercentRecordsPurged.IsEnabled = state;
        //    PercentageRecordsProcessedTextBox.IsEnabled = state;
        //    lblRemainingRunTime.IsEnabled = state;
        //    remainingRunTimeMinutes.IsEnabled = state;

        //}

        //private void AlterStartStopPanel(bool state)
        //{
        //    StopStartButtonGrid.IsEnabled = state;
        //}

        //private void AlterPurgeCriteriaPanel(bool state)
        //{
        //    EventsCriteriaGrid.IsEnabled = state;
        //    lblPurgeCriteria.IsEnabled = state;
        //    gbUser.IsEnabled = state;
        //    gbComputer.IsEnabled = state;
        //    gbByProcess.IsEnabled = state;
        //    gbDataRetentionLimit.IsEnabled = state;
        //    gbBatchSize.IsEnabled = state;
        //    gbTaskRunTime.IsEnabled = state;
        //}

        private void btnStartCleanup_Click(object sender, EventArgs e)
        {
            _theLogger.SaveEventToLogFile(  " START CLEANUP button was clicked.");
            ModifyGuiAfterStartButtonClick();
            ActionOutcome theResult = LoadGuiValuesIntoDbSqlSpController();
            if (theResult.Success)
            {
                MessageBoxResult backupDone = MessageBox.Show("It is highly recommended to have a backup of the " + LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName + " database before deleting records. Do you want to continue with the deletion?", "Backup check for database " + LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (backupDone == MessageBoxResult.Yes)
                {
                    StartPurging();
                }
                else
                {
                    ModifyGuiAfterStopButtonClick();
                    toolStripStatusLabel1.Text = " Action canceled by user";
                }
            }
            else
            {
                MessageBox.Show(theResult.Message);
                ModifyGuiAfterStopButtonClick();
                toolStripStatusLabel1.Text = " Action canceled due to invalid criteria selection(s)";
            }
        }

        private void StartPurging()
        {
            LiveDbSpSqlController.DbSqlSpControllerData.StopController = false;
            toolStripStatusLabel1.Text = "Building required Stored Procedures...";
            var parameterValidationResult = LiveDbSpSqlController.ValidateParameters();
            if (parameterValidationResult == "success")
            {
                _theLogger.SaveEventToLogFile(  " GUI Parameters validation and SQL Stored Procedure creation successful.");
                _calculateTotalRecordsToPurgeThread = new Thread(LiveDbSpSqlController.CalculateTotalRecordsToPurge);
                _calculateTotalRecordsToPurgeThread.IsBackground = true;
                _calculateTotalRecordsToPurgeThread.Start();
                Thread.Sleep(100);
                timerCalcTotRecToPurge.IsEnabled = true;
                toolStripStatusLabel1.Text = "Identifying target records...";
            }
            else
            {
                _theLogger.SaveErrorToLogFile(  parameterValidationResult.ToString());
                MessageBox.Show("Error validating the GUI parameters or Creating SQL Stored Procedures. See the Log File for further details.", "Error Processing Criteria", MessageBoxButton.OK, MessageBoxImage.Error);
                _theLogger.SaveErrorToLogFile(  parameterValidationResult);
                ModifyGuiAfterStopButtonClick();
            }
        }

        private void SetGuiStatsToZeros()
        {
            RecordsLeftToPurgeTextBox.Text = "0";
            RecordsPurgedTextBox.Text = "0";
            RemainingRunTimeMinutes.Text = "0";
            PercentageRecordsProcessedTextBox.Text = "0";
            toolStripProgressBar1.Value = 100;
        }

        private string BuildWarningMessage()
        {
            StringBuilder whoSpWillCleanfor = new StringBuilder();
            whoSpWillCleanfor.Append("This action will delete " + LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge.ToString(CultureInfo.InvariantCulture) + " records for ");
            if (LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser.ToLower() == "everyone")
            {
                whoSpWillCleanfor.Append(" All Users");
            }
            else
            {
                whoSpWillCleanfor.Append(" " + LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser);
            }
            if (LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer.ToLower() == "all")
            {
                whoSpWillCleanfor.Append(" from All Computers");
            }
            else
            {
                whoSpWillCleanfor.Append(" from " + LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer);
            }
            whoSpWillCleanfor.Append(" older than " + LiveDbSpSqlController.DbSqlSpControllerData.CutOffDate.ToString(CultureInfo.InvariantCulture));

            whoSpWillCleanfor.Append(" Do you want to proceed?");
            return whoSpWillCleanfor.ToString();
        }

        private ActionOutcome LoadGuiValuesIntoDbSqlSpController()
        {
            ActionOutcome result = new ActionOutcome();
            bool userFound = false;
            bool computerFound = false;
            bool processFound = false;
            string errorMessage = "";
            LiveDbSpSqlController.DbSqlSpControllerData.RunTime = (((int)runtimeHours.SelectedValue * 60) + (int)runtimeMinutes.SelectedValue);
            LiveDbSpSqlController.DbSqlSpControllerData.BatchSize = cbBatchSize.Items[cbBatchSize.SelectedIndex].ToString();
            if (rbNoCutOffDate.IsChecked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDate = DateTime.Today;
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDays = false;
            }
            else
            {
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDate = dtpCutOffDate.SelectedDate.Value.Date;
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDays = true;
            }
            if (rbEveryone.IsChecked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser = "everyone";
                _theLogger.SaveEventToLogFile(  " Selected User: EVERYONE.");
                userFound = true;
            }
            if (rbSelectUser.IsChecked == true)
            {
                string selectedUserEntry = cbSpecificUser.Items[cbSpecificUser.SelectedIndex].ToString();
                if (selectedUserEntry != "No Users Found")
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.UserSid = selectedUserEntry.Substring((selectedUserEntry.IndexOf(":", StringComparison.Ordinal) + 1),
                                                                                        (selectedUserEntry.Length - selectedUserEntry.IndexOf(":", StringComparison.Ordinal) - 1));
                    LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser = selectedUserEntry.Substring(0, selectedUserEntry.IndexOf(":", StringComparison.Ordinal));
                    _theLogger.SaveEventToLogFile(  " Selected UserSID: " + LiveDbSpSqlController.DbSqlSpControllerData.UserSid.ToString() +
                                                           " . Selected User Name: " + LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser.ToString() + ".");
                    userFound = true;
                }
                else
                {
                    _theLogger.SaveEventToLogFile(  " Invalid User target criteria selected. Please review the selection.");
                    userFound = false;
                    errorMessage = " Invalid User target criteria selected. Please review the selection.";
                }
            }
            if (rbAllComputers.IsChecked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer = "all";
                _theLogger.SaveEventToLogFile(  " Selected Computer: ALL.");
                computerFound = true;
            }
            if (rbSelectComputer.IsChecked == true)
            {
                string selectedComputerEntry = cbSpecificComputer.Items[cbSpecificComputer.SelectedIndex].ToString();
                if (selectedComputerEntry != "No Computers Found")
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.EpsGuid = selectedComputerEntry.Substring((selectedComputerEntry.IndexOf(":", StringComparison.Ordinal) + 1),
                                                                                            (selectedComputerEntry.Length - selectedComputerEntry.IndexOf(":", StringComparison.Ordinal) - 1));
                    LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer = selectedComputerEntry.Substring(0, selectedComputerEntry.IndexOf(":", StringComparison.Ordinal));
                    _theLogger.SaveEventToLogFile(  " Selected EPSGUID: " + LiveDbSpSqlController.DbSqlSpControllerData.EpsGuid.ToString() +
                                                           " . Selected Endpoint Name: " + LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer.ToString() + ".");
                    computerFound = true;
                }
                else
                {
                    _theLogger.SaveEventToLogFile(  " Invalid Computer target criteria selected. Please review the selection.");
                    computerFound = false;
                    errorMessage += " .Invalid Computer target criteria selected. Please review the selection.";
                }
            }

            if (rbAllProcesses.IsChecked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.SelectedProcess = "all";
                _theLogger.SaveEventToLogFile(  " Selected Process: ALL.");
                processFound = true;
            }
            if (rbSpecificProcess.IsChecked == true)
            {
                if (cbSpecificProcess.Items[cbSpecificProcess.SelectedIndex].ToString() != "No Processes Found")
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.SelectedProcess = cbSpecificProcess.Items[cbSpecificProcess.SelectedIndex].ToString();
                    processFound = true;
                }
                else
                {
                    _theLogger.SaveEventToLogFile(  " Invalid Process target criteria selected. Please review the selection.");
                    processFound = false;
                    errorMessage += " .Invalid Process target criteria selected. Please review the selection.";
                }
            }
            if (userFound && computerFound && processFound)
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = errorMessage;
            }
            return result;
        }

        private void ModifyGuiAfterStartButtonClick()
        {
            EventTypesMenuItem.IsEnabled = false;

            EventsCriteriaGrid.IsEnabled = false;
            StopStartButtonGrid.IsEnabled = true;
            ProgressGrid.IsEnabled = true;

            //AlterPurgeCriteriaPanel(false);
            //AlterStartStopPanel(true);
            //AlterPurgeStatsPanel(true);

            btnStopCleanup.IsEnabled = true;
            btnStartCleanup.IsEnabled = false;

            //EventsCriteriaGrid.BackColor = SystemColors.Control;
            //StopStartButtonGrid.BackColor = SystemColors.ControlLightLight;
            //ProgressGrid.BackColor = SystemColors.ControlLightLight;

            //btnStartCleanup.BackColor = SystemColors.Control;
            //btnStopCleanup.BackColor = SystemColors.Control;

            btnStartCleanup.IsEnabled = false;
            btnStopCleanup.IsEnabled = true;
        }

        private void btnStopCleanup_Click(object sender, EventArgs e)
        {
            _theLogger.SaveEventToLogFile(  " STOP CLEANUP button was clicked.");
            StopPurgeThreadAndWaitForThreadStop();
            btnStartCleanup.IsEnabled = false;
            btnStopCleanup.IsEnabled = false;
        }

        private void StopPurgeThreadAndWaitForThreadStop()
        {
            LiveDbSpSqlController.RequestStop();
            while (!LiveDbSpSqlController.DbSqlSpControllerData.WorkerCompleted)
            {
                Thread.Sleep(100);
                //Application.DoEvents();  // Does this hand back to the calling method to then allow GUI button changes?
            }
        }

        private void ModifyGuiAfterStopButtonClick()
        {
            EventTypesMenuItem.IsEnabled = true;

            EventsCriteriaGrid.IsEnabled = true;
            StopStartButtonGrid.IsEnabled = true;
            ProgressGrid.IsEnabled = false;

            //AlterPurgeCriteriaPanel(true);
            //AlterStartStopPanel(true);
            //AlterPurgeStatsPanel(false);

            btnStopCleanup.IsEnabled = false;
            btnStartCleanup.IsEnabled = true;

            //EventsCriteriaGrid.BackColor = SystemColors.ControlLightLight;
            //StopStartButtonGrid.BackColor = SystemColors.ControlLightLight;
            //ProgressGrid.BackColor = SystemColors.Control;

            //btnStartCleanup.BackColor = SystemColors.Control;
            //btnStopCleanup.BackColor = SystemColors.Control;

            btnStartCleanup.IsEnabled = true;
            btnStopCleanup.IsEnabled = false;
        }

        private void rbEveryone_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEveryone.IsChecked != true) return;
            rbSelectUser.IsChecked = false;
            cbSpecificUser.IsEnabled = false;
        }

        private void rbSelectUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectUser.IsChecked != true) return;
            rbEveryone.IsChecked = false;
            cbSpecificUser.IsEnabled = true;
            cbSpecificUser.SelectedIndex = 0;
        }

        private void rbAllComputers_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllComputers.IsChecked != true) return;
            rbSelectComputer.IsChecked = false;
            cbSpecificComputer.IsEnabled = false;
        }

        private void rbSpecificComputer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectComputer.IsChecked != true) return;
            rbAllComputers.IsChecked = false;
            cbSpecificComputer.IsEnabled = true;
            cbSpecificComputer.SelectedIndex = 0;
        }

        //private void StopDbConnTestThreadAndWaitForThreadStop()
        //{
        //    _testDbConnectionThread.Abort();
        //    while (_testDbConnectionThread.IsAlive)
        //    {
        //        //Application.DoEvents();
        //    }
        //}


        private void processingStatsTimer_Tick(object sender, EventArgs e)
        {
            if (_sqlPurgeWorkerThread.IsAlive)
            {
                int curProgBarValue = Convert.ToInt32(toolStripProgressBar1.Value);
                if (curProgBarValue <= 100)
                {
                    curProgBarValue = curProgBarValue + 10;
                    if (curProgBarValue >= 100)
                    {
                        curProgBarValue = 0;
                    }
                }
                toolStripProgressBar1.Value = curProgBarValue;
                if (LiveDbSpSqlController.DbSqlSpControllerData.StopController)
                {

                    toolStripStatusLabel1.Text = "Stopping current processing...";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Processing Records...";
                }
                double daysRemaining = Math.Floor((double)LiveDbSpSqlController.DbSqlSpControllerData.ProcessingEndTime.Subtract(DateTime.Now).Days);
                int theDaysRemaining = Convert.ToInt32(daysRemaining);
                double hoursRemaining = Math.Floor((double)LiveDbSpSqlController.DbSqlSpControllerData.ProcessingEndTime.Subtract(DateTime.Now).Hours);
                int theHoursRemaining = Convert.ToInt32(hoursRemaining);
                double minsRemaining = Math.Floor((double)LiveDbSpSqlController.DbSqlSpControllerData.ProcessingEndTime.Subtract(DateTime.Now).Minutes);
                int theMinsRemaining = Convert.ToInt32(minsRemaining);
                int secsRemaining = LiveDbSpSqlController.DbSqlSpControllerData.ProcessingEndTime.Subtract(DateTime.Now).Seconds;
                string timeLeft = PadStringIfBelowTen(theDaysRemaining) + "d " + PadStringIfBelowTen(theHoursRemaining) + "h " + PadStringIfBelowTen(theMinsRemaining) + "m " + PadStringIfBelowTen(secsRemaining) + "s";
                LiveDbSpSqlControllerData.RuntimeRemaining = (timeLeft);
            }
            else
            {
                processingStatsTimer.IsEnabled = false;
                if (LiveDbSpSqlController.DbSqlSpControllerData.WorkerCompleted)
                {
                    //RecordsLeftToPurgeTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge.ToString();
                    //PercentageRecordsProcessedTextBox.Text = (((LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge - LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge) * 100) / LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge).ToString();
                    RemainingRunTimeMinutes.Text = "0:00";
                    //RecordsPurgedTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar.ToString();
                    toolStripProgressBar1.Value = 100;
                    toolStripStatusLabel1.Text = "Processing complete";
                    _theLogger.SaveEventToLogFile(  " The Worker Thread has completed cleaning.");
                    _theLogger.SaveEventToLogFile(  " --SUMMARY:-- ");
                    _theLogger.SaveEventToLogFile(  " Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar + ".");
                    MessageBox.Show("Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar,
                                                                      "Record Processing Complete",
                                                                      MessageBoxButton.OK, MessageBoxImage.Information);


                }
                else
                {
                    toolStripStatusLabel1.Text = "Processing stopped unexpectedly";
                    _theLogger.SaveEventToLogFile(  " The Worker Thread is Dead. Enabling GUI.");
                    _theLogger.SaveEventToLogFile(  " --SUMMARY:-- ");
                    _theLogger.SaveEventToLogFile(  " Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar + ".");
                    MessageBox.Show("Record processing stopped unexpectedly. Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar + ".",
                                                                      "Record Processing Warning",
                                                                      MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                ModifyGuiAfterStopButtonClick();
            }
        }


        private static string PadStringIfBelowTen(int secsRemaining)
        {
            if (secsRemaining < 10)
            {
                return "0" + secsRemaining.ToString();
            }
            else
            {
                return secsRemaining.ToString();
            }
        }

        private void runtimeHours_ValueChanged(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

        private void runtimeMinutes_ValueChanged(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

        private void RuntimesLogicCheck()
        {
           
            if ((Convert.ToInt32(runtimeHours.SelectedValue) == 0) && (Convert.ToInt32(runtimeMinutes.SelectedValue) == 0))
            {
                runtimeMinutes.SelectedValue = 1;
            }
            if ((Convert.ToInt32(runtimeHours.SelectedValue) == 90) && (Convert.ToInt32(runtimeMinutes.SelectedValue) > 0))
            {
                runtimeMinutes.SelectedValue = 0;
            }
            //if (runtimeHours.Text == "")
            //{
            //    runtimeHours.Text = "0";
            //    runtimeHours.Value = 0;
            //}
            //if (runtimeMinutes.Text == "")
            //{
            //    runtimeMinutes.Text = "0";
            //    runtimeMinutes.Value = 0;
            //}
            //if ((runtimeHours.Value == 0) && (runtimeMinutes.Value == 0))
            //{
            //    runtimeMinutes.Value = 1;
            //}
            //if ((runtimeHours.Value == 90) && (runtimeMinutes.Value > 0))
            //{
            //    runtimeMinutes.Value = 0;
            //}
        }

        private void dtpCutOffDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpCutOffDate.SelectedDate.Value.Date < DateTime.Today.Date) return;
            MessageBox.Show("You cannot select today or a future date as the cut-off date. Please select an older date.", "Invalid date selection", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            _theLogger.SaveEventToLogFile(  " User tried to select " + dtpCutOffDate.SelectedDate.Value.Date.ToString() + " as the cut-off date.");
            dtpCutOffDate.DisplayDate = DateTime.Today.AddDays(-1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void eventTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IESandDACadmt.View.WpfEventTypeSelection eventTypeForm = new WpfEventTypeSelection(LiveDbSpSqlController, _theLogger);
            //eventTypeForm.StartPosition = FormStartPosition.CenterParent;
            eventTypeForm.Show();
        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation);
        }

        private void rbSpecificProcess_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSpecificProcess.IsChecked == true)
            {
                rbAllProcesses.IsChecked = false;
                if (LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQueryAlreadyRan)
                {
                    // Sort the already gotten results into GUI CB alphabetically.
                    LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults.Sort();
                    cbSpecificProcess.Items.Clear();
                    if (LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults.Count > 0)
                    {
                        foreach (string processString in LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults)
                        {
                            cbSpecificProcess.Items.Add(processString);
                        }
                        cbSpecificProcess.IsEnabled = true;
                        cbSpecificProcess.SelectedIndex = 0;
                    }
                    else
                    {
                        ModifyByProcessControlsForNoProcessInfo();
                    }
                }
                else
                {
                    MessageBoxResult result1 = MessageBox.Show("This data will need to be retrieved from the database and this could take some time. Do you want to proceed with this?", "Process-related SQL query", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result1 == MessageBoxResult.Yes)
                    {
                        // Run the Query in a new thread
                        toolStripStatusLabel1.Text = "Querying for top processes...";
                        try
                        {
                            IESandDACadmt.Model.Sql.SqlReadByProcessInfoThread workerReadSqlByProcess = new IESandDACadmt.Model.Sql.SqlReadByProcessInfoThread(LiveDbSpSqlController,
                                                                                                                       LiveDbSpSqlController.DbSqlSpControllerData.SqlConnectionString,
                                                                                                                       3600,
                                                                                                                       IESandDACadmt.Model.RecordsProfilingQueryLogic.EmssByProcessQuery,
                                                                                                                       LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQueryDataTable);
                            _readByProcessSqlInfoThread = new Thread(workerReadSqlByProcess.ReadByProcessSqlInfo);
                            _readByProcessSqlInfoThread.IsBackground = true;
                            _readByProcessSqlInfoThread.Start();
                            readByProcessInfoTimer.IsEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            _theLogger.SaveErrorToLogFile(  " " + ex.Message.ToString());
                        }
                    }
                    else
                    {
                        ModifyByProcessControlsForNoProcessInfo();
                    }
                }
            }


        }

        private void ModifyByProcessControlsForNoProcessInfo()
        {
            rbAllProcesses.IsChecked = true;
            rbAllProcesses.IsEnabled = true;
            rbSpecificProcess.IsChecked = false;
            rbSpecificProcess.IsEnabled = true;
            cbSpecificProcess.IsEnabled = false;
        }

        private void readByProcessInfoTimer_Tick(object sender, EventArgs e)
        {
            // Enable the CB when the SQL read has completed and load the values into it.
            // If query doesn't come back OK or it's canceled, switch GUI bits back to ALL Processes.
            if (LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQuerySuccess == true)
            {
                if (LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQueryDataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQueryDataTable.Rows)
                    {
                        LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults.Add(row["ProcessName"].ToString());
                    }
                    LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQueryAlreadyRan = true;
                }
                else
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults.Add("No Processes Found");
                }
                LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults.Sort();
                if (LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults.Count > 0)
                {
                    cbSpecificProcess.Items.Clear();
                    foreach (string processString in LiveDbSpSqlController.DbSqlSpControllerData.ByProcessResults)
                    {
                        cbSpecificProcess.Items.Add(processString);
                    }
                    cbSpecificProcess.IsEnabled = true;
                    cbSpecificProcess.SelectedIndex = 0;
                }
                else
                {
                    ModifyByProcessControlsForNoProcessInfo();
                }
                toolStripProgressBar1.Value = 100;
                toolStripStatusLabel1.Text = "Finished reading processes";
                readByProcessInfoTimer.IsEnabled = false;
            }
            else
            {
                // Make the progress bar increment.
                int curProgBarValue = Convert.ToInt32(toolStripProgressBar1.Value);
                if (curProgBarValue <= 100)
                {
                    curProgBarValue = curProgBarValue + 10;
                    if (curProgBarValue >= 100)
                    {
                        curProgBarValue = 0;
                    }
                }
                toolStripProgressBar1.Value = curProgBarValue;
            }
        }

        private void rbAllProcesses_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllProcesses.IsChecked == true)
            {
                rbSpecificProcess.IsChecked = false;
                cbSpecificProcess.IsEnabled = false;
            }

        }

        public bool StartCleanup()
        {
            bool wasTheThreadStarted = false;
            LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar = 0;
            try
            {
                SqlDbCleanupThread worker = new SqlDbCleanupThread(LiveDbSpSqlController, _theLogger);
                worker.BatchProcessed += UpdateAfterBatchProcessed;
                _sqlPurgeWorkerThread = new Thread(worker.StartProcessing);
                _sqlPurgeWorkerThread.IsBackground = true;
                _sqlPurgeWorkerThread.Start();
                Thread.Sleep(100);
                _theLogger.SaveEventToLogFile(  " SQL Worker Thread Started.");
                wasTheThreadStarted = true;
            }
            catch (Exception ex)
            {
                _theLogger.SaveErrorToLogFile(  " " + ex.Message.ToString());
            }
            return wasTheThreadStarted;
        }

        public void UpdateAfterBatchProcessed(object sender, SqlDeletionEventargs e)
        {
            // Update the values that get saved to LiveSpSql....
            if (e.RecordsDeletedThisBatch == LiveDbSpSqlController.DbSqlSpControllerData.RecordsForBatchSize)
            {
                _theLogger.SaveEventToLogFile(  " Batch of " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsForBatchSize.ToString() + " processed.");
                LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge -= e.RecordsDeletedThisBatch;
                LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar += e.RecordsDeletedThisBatch;
            }
            else
            {
                _theLogger.SaveEventToLogFile(  " Last Batch of " + e.RecordsDeletedThisBatch.ToString() + " processed.");
                LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge = 0;
                LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar += e.RecordsDeletedThisBatch;
            }
            // Update the GUI with the new values......
            UpdateGuiWithNewValues(LiveDbSpSqlController);
        }

        private void UpdateGuiWithNewValues(DbSqlSpController updatedResults)
        {
            //if (this.RecordsLeftToPurgeTextBox.InvokeRequired)
            //{
            //    UpdateGuiWithNewValuesCallBack del = new UpdateGuiWithNewValuesCallBack(UpdateGuiWithNewValues);
            //    this.Invoke(del, new object[] { updatedResults });
            //}
            //else
            //{
            //    RecordsLeftToPurgeTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge.ToString();
                double percentage = (((LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge - LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge) * 100) / LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge);
                updatedResults.DbSqlSpControllerData.PercentageRecordsProcessed = Math.Round(percentage, 2).ToString();
            //    RecordsPurgedTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar.ToString();
            //}
        }

        private void timerCalcTotRecToPurge_Tick(object sender, EventArgs e)
        {
            if (_calculateTotalRecordsToPurgeThread.IsAlive)
            {
                int curProgBarValue = Convert.ToInt32(toolStripProgressBar1.Value);
                if (curProgBarValue <= 100)
                {
                    curProgBarValue = curProgBarValue + 10;
                    if (curProgBarValue >= 100)
                    {
                        curProgBarValue = 0;
                    }
                }
                toolStripProgressBar1.Value = curProgBarValue;

            }
            else
            {
                timerCalcTotRecToPurge.IsEnabled = false;
                if (LiveDbSpSqlController.DbSqlSpControllerData.Result.Success)
                {
                    toolStripProgressBar1.Value = 100;
                    if (LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge > 0)
                    {
                        _theLogger.SaveEventToLogFile(  " Calculation of total Records to Purge: " + LiveDbSpSqlController.DbSqlSpControllerData.Result.Message.ToString() + " : " + LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge.ToString() + ".");
                        string whoSpWillCleanfor = BuildWarningMessage();
                        MessageBoxResult goNoGoResponse = MessageBox.Show(whoSpWillCleanfor,
                                                                      "Record Deletion Warning",
                                                                      MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (goNoGoResponse == MessageBoxResult.Yes)
                        {
                            processingStatsTimer.IsEnabled = true;
                            //btnChangeSqlServer.IsEnabled = false;
                            if (StartCleanup() == false)
                            {
                                // Thread did not start so run stop button code
                                processingStatsTimer.IsEnabled = false;
                                ModifyGuiAfterStopButtonClick();
                            }
                        }
                        else
                        {
                            ModifyGuiAfterStopButtonClick();
                            ModifyGuiAfterGoPromptCanceled();
                            _theLogger.SaveEventToLogFile(  " START DELETION confirmation-message canceled by user.");
                        }
                    }
                    else
                    {
                        processingStatsTimer.IsEnabled = false;
                        MessageBox.Show("No records match these characteristics.", "Target records calculation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        SetGuiStatsToZeros();
                        ModifyGuiAfterStopButtonClick();
                        ModifyGuiAfterNoTargetRecordsFound();
                        _theLogger.SaveEventToLogFile(  " No events match these selected characteristics.");
                    }
                }
                else
                {
                    SetGuiStatsToZeros();
                    ModifyGuiAfterStopButtonClick();
                    ModifyGuiAfterGoPromptCanceled();
                    _theLogger.SaveErrorToLogFile(  " " + LiveDbSpSqlController.DbSqlSpControllerData.Result.Message);
                    toolStripStatusLabel1.Text = "Failed to calculate records to purge. See Log File";

                }

            }
        }

        private void rbNoCutOffDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoCutOffDate.IsChecked == true)
            {
                rbCutOffDate.IsChecked = false;
                dtpCutOffDate.IsEnabled = false;
            }
        }

        private void rbCutOffDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCutOffDate.IsChecked == true)
            {
                rbNoCutOffDate.IsChecked = false;
                dtpCutOffDate.IsEnabled = true;
            }
        }

        private void runtimeMinutes_SelectionChanged(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

        private void runtimeHours_SelectionChanged(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

    }
}
