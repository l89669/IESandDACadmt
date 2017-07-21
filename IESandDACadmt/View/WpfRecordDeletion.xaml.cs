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

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfRecordDeletion.xaml
    /// </summary>
    public partial class WpfRecordDeletion : Window
    {
        public WpfRecordDeletion(Model.DbSqlSpController theDbSqlSpController)
        {
            LiveDbSpSqlController = theDbSqlSpController;
            LiveDbSpSqlController.BuildEventTypesDictionary();
            InitializeComponent();
            cbSpecificUser.Items.Clear();
            cbSpecificComputer.Items.Clear();
        }

        public volatile DbSqlSpController LiveDbSpSqlController = new DbSqlSpController();
        Thread _testDbConnectionThread = null;
        Thread _readByProcessSqlInfoThread = null;
        Thread _calculateTotalRecordsToPurgeThread = null;
        Thread _sqlPurgeWorkerThread = null;

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

        private static void cbSpecificUser_DropDown(object sender, System.EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            foreach (string s in ((ComboBox)sender).Items)
            {
                var newWidth = (int)g.MeasureString(s, font).Width
                               + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }

        private void cbSpecificComputer_DropDown(object sender, System.EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            foreach (string s in ((ComboBox)sender).Items)
            {
                var newWidth = (int)g.MeasureString(s, font).Width
                               + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }

        private void ModifyGuiAfterGoPromptCanceled()
        {
            if (rbSelectUser.Checked == true)
            {
                rbEveryone.Enabled = true;
                rbSelectUser.Enabled = true;
                cbSpecificUser.Enabled = true;
                rbEveryone.Checked = false;
                rbSelectUser.Checked = true;
            }
            else
            {
                rbEveryone.Enabled = true;
                rbSelectUser.Enabled = true;
                cbSpecificUser.Enabled = false;
                rbEveryone.Checked = true;
                rbSelectUser.Checked = false;
            }

            if (rbSelectComputer.Checked == true)
            {
                rbAllComputers.Enabled = true;
                rbSelectComputer.Enabled = true;
                cbSpecificComputer.Enabled = true;
                rbAllComputers.Checked = false;
                rbSelectComputer.Checked = true;
            }
            else
            {
                rbAllComputers.Enabled = true;
                rbSelectComputer.Enabled = true;
                cbSpecificComputer.Enabled = false;
                rbAllComputers.Checked = true;
                rbSelectComputer.Checked = false;
            }
            AlterPurgeCriteriaPanel(true);
            AlterStartStopPanel(true);
            AlterPurgeStatsPanel(false);

            btnStopCleanup.Enabled = false;
            btnStartCleanup.Enabled = true;

            pCleanupCriteria.BackColor = SystemColors.ControlLightLight;
            pStartStopButtons.BackColor = SystemColors.ControlLightLight;
            pCleanupStats.BackColor = SystemColors.Control;

            btnStartCleanup.BackColor = SystemColors.Control;
            btnStopCleanup.BackColor = SystemColors.Control;
            btnStartCleanup.Enabled = true;
            btnStopCleanup.Enabled = false;

            toolStripStatusLabel1.Text = "Record deletion canceled";
        }

        private void ModifyGuiAfterNoTargetRecordsFound()
        {
            if (rbSelectUser.Checked == true)
            {
                rbEveryone.Enabled = true;
                rbSelectUser.Enabled = true;
                cbSpecificUser.Enabled = true;
                rbEveryone.Checked = false;
                rbSelectUser.Checked = true;
            }
            else
            {
                rbEveryone.Enabled = true;
                rbSelectUser.Enabled = true;
                cbSpecificUser.Enabled = false;
                rbEveryone.Checked = true;
                rbSelectUser.Checked = false;
            }

            if (rbSelectComputer.Checked == true)
            {
                rbAllComputers.Enabled = true;
                rbSelectComputer.Enabled = true;
                cbSpecificComputer.Enabled = true;
                rbAllComputers.Checked = false;
                rbSelectComputer.Checked = true;
            }
            else
            {
                rbAllComputers.Enabled = true;
                rbSelectComputer.Enabled = true;
                cbSpecificComputer.Enabled = false;
                rbAllComputers.Checked = true;
                rbSelectComputer.Checked = false;
            }
            AlterPurgeCriteriaPanel(true);
            AlterStartStopPanel(true);
            AlterPurgeStatsPanel(false);

            btnStopCleanup.Enabled = false;
            btnStartCleanup.Enabled = true;

            pCleanupCriteria.BackColor = SystemColors.ControlLightLight;
            pStartStopButtons.BackColor = SystemColors.ControlLightLight;
            pCleanupStats.BackColor = SystemColors.Control;

            btnStartCleanup.BackColor = SystemColors.Control;
            btnStopCleanup.BackColor = SystemColors.Control;
            btnStartCleanup.Enabled = true;
            btnStopCleanup.Enabled = false;

            toolStripStatusLabel1.Text = "No target records found. Action canceled";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsersComputersIntoGui();
            ModifyGuiAfterFormLoad();
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " The known Users and Computers read from SQL and loaded into GUI.");
            cbSpecificComputer.DropDown += new System.EventHandler(this.cbSpecificComputer_DropDown);
            cbSpecificUser.DropDown += new System.EventHandler(cbSpecificUser_DropDown);
            toolStripStatusLabel1.Text = "Connected to:" + LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress + " User:" + LiveDbSpSqlController.DbSqlSpControllerData.SqlConnUserName;
        }

        private void ModifyGuiAfterFormLoad()
        {
            pCleanupCriteria.BackColor = SystemColors.ControlLightLight;
            pStartStopButtons.BackColor = SystemColors.ControlLightLight;
            pCleanupStats.BackColor = SystemColors.Control;
            AlterPurgeCriteriaPanel(true);
            AlterStartStopPanel(true);
            AlterPurgeStatsPanel(false);

            rbEveryone.Checked = true;
            rbEveryone.Enabled = true;
            rbSelectUser.Checked = false;
            rbSelectUser.Enabled = true;
            cbSpecificUser.Enabled = false;

            rbAllComputers.Checked = true;
            rbAllComputers.Enabled = true;
            rbSelectComputer.Checked = false;
            rbSelectComputer.Enabled = true;
            cbSpecificComputer.Enabled = false;

            rbAllProcesses.Checked = true;
            rbAllProcesses.Enabled = true;
            rbSpecificProcess.Checked = false;
            rbSpecificProcess.Enabled = true;
            cbSpecificProcess.Enabled = false;

            btnStartCleanup.BackColor = SystemColors.Control;
            btnStopCleanup.BackColor = SystemColors.Control;
            btnStartCleanup.Enabled = true;
            btnStopCleanup.Enabled = false;

            dtpCutOffDate.Value = DateTime.Today.AddDays(-1);
            rbCutOffDate.Checked = true;
            rbNoCutOffDate.Checked = false;
            cbBatchSize.SelectedIndex = 1;
            toolStripProgressBar1.Value = 0;
        }

        private void AlterPurgeStatsPanel(bool state)
        {
            pCleanupStats.Enabled = state;
            lblRecordsToPurge.Enabled = state;
            RecordsLeftToPurgeTextBox.Enabled = state;
            lblRecordsPurged.Enabled = state;
            RecordsPurgedTextBox.Enabled = state;
            lblPercentRecordsPurged.Enabled = state;
            percentageRecordsProcessedTextBox.Enabled = state;
            lblRemainingRunTime.Enabled = state;
            remainingRunTimeMinutes.Enabled = state;

        }

        private void AlterStartStopPanel(bool state)
        {
            pStartStopButtons.Enabled = state;
        }

        private void AlterPurgeCriteriaPanel(bool state)
        {
            pCleanupCriteria.Enabled = state;
            lblPurgeCriteria.Enabled = state;
            gbUser.Enabled = state;
            gbComputer.Enabled = state;
            gbByProcess.Enabled = state;
            gbDataRetentionLimit.Enabled = state;
            gbBatchSize.Enabled = state;
            gbTaskRunTime.Enabled = state;
        }

        private void btnStartCleanup_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " START CLEANUP button was clicked.");
            ModifyGuiAfterStartButtonClick();
            ActionOutcome theResult = LoadGuiValuesIntoDbSqlSpController();
            if (theResult.Success)
            {
                DialogResult backupDone = MessageBox.Show("It is highly recommended to have a backup of the " + LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName + " database before deleting records. Do you want to continue with the deletion?", "Backup check for database " + LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (backupDone == DialogResult.Yes)
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
            var parameterValidationResult = LiveDbSpSqlController.DbSqlSpControllerData.ValidateParameters();
            if (parameterValidationResult == "success")
            {
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " GUI Parameters validation and SQL Stored Procedure creation successful.");
                _calculateTotalRecordsToPurgeThread = new Thread(LiveDbSpSqlController.DbSqlSpControllerData.CalculateTotalRecordsToPurge);
                _calculateTotalRecordsToPurgeThread.IsBackground = true;
                _calculateTotalRecordsToPurgeThread.Start();
                Thread.Sleep(100);
                timerCalcTotRecToPurge.Enabled = true;
                toolStripStatusLabel1.Text = "Identifying target records...";
            }
            else
            {
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, parameterValidationResult.ToString());
                MessageBox.Show("Error validating the GUI parameters or Creating SQL Stored Procedures. See the Log File for further details.", "Error Processing Criteria", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, parameterValidationResult);
                ModifyGuiAfterStopButtonClick();
            }
        }

        private void SetGuiStatsToZeros()
        {
            RecordsLeftToPurgeTextBox.Text = "0";
            RecordsPurgedTextBox.Text = "0";
            remainingRunTimeMinutes.Text = "0";
            percentageRecordsProcessedTextBox.Text = "0";
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
            LiveDbSpSqlController.DbSqlSpControllerData.RunTime = (((int)runtimeHours.Value * 60) + (int)runtimeMinutes.Value);
            LiveDbSpSqlController.DbSqlSpControllerData.BatchSize = cbBatchSize.Items[cbBatchSize.SelectedIndex].ToString();
            if (rbNoCutOffDate.Checked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDate = DateTime.Today;
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDays = false;
            }
            else
            {
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDate = dtpCutOffDate.Value.Date;
                LiveDbSpSqlController.DbSqlSpControllerData.CutOffDays = true;
            }
            if (rbEveryone.Checked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser = "everyone";
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Selected User: EVERYONE.");
                userFound = true;
            }
            if (rbSelectUser.Checked == true)
            {
                string selectedUserEntry = cbSpecificUser.Items[cbSpecificUser.SelectedIndex].ToString();
                if (selectedUserEntry != "No Users Found")
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.UserSid = selectedUserEntry.Substring((selectedUserEntry.IndexOf(":", StringComparison.Ordinal) + 1),
                                                                                        (selectedUserEntry.Length - selectedUserEntry.IndexOf(":", StringComparison.Ordinal) - 1));
                    LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser = selectedUserEntry.Substring(0, selectedUserEntry.IndexOf(":", StringComparison.Ordinal));
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Selected UserSID: " + LiveDbSpSqlController.DbSqlSpControllerData.UserSid.ToString() +
                                                           " . Selected User Name: " + LiveDbSpSqlController.DbSqlSpControllerData.SelectedUser.ToString() + ".");
                    userFound = true;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Invalid User target criteria selected. Please review the selection.");
                    userFound = false;
                    errorMessage = " Invalid User target criteria selected. Please review the selection.";
                }
            }
            if (rbAllComputers.Checked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer = "all";
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Selected Computer: ALL.");
                computerFound = true;
            }
            if (rbSelectComputer.Checked == true)
            {
                string selectedComputerEntry = cbSpecificComputer.Items[cbSpecificComputer.SelectedIndex].ToString();
                if (selectedComputerEntry != "No Computers Found")
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.EpsGuid = selectedComputerEntry.Substring((selectedComputerEntry.IndexOf(":", StringComparison.Ordinal) + 1),
                                                                                            (selectedComputerEntry.Length - selectedComputerEntry.IndexOf(":", StringComparison.Ordinal) - 1));
                    LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer = selectedComputerEntry.Substring(0, selectedComputerEntry.IndexOf(":", StringComparison.Ordinal));
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Selected EPSGUID: " + LiveDbSpSqlController.DbSqlSpControllerData.EpsGuid.ToString() +
                                                           " . Selected Endpoint Name: " + LiveDbSpSqlController.DbSqlSpControllerData.SelectedComputer.ToString() + ".");
                    computerFound = true;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Invalid Computer target criteria selected. Please review the selection.");
                    computerFound = false;
                    errorMessage += " .Invalid Computer target criteria selected. Please review the selection.";
                }
            }

            if (rbAllProcesses.Checked == true)
            {
                LiveDbSpSqlController.DbSqlSpControllerData.SelectedProcess = "all";
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Selected Process: ALL.");
                processFound = true;
            }
            if (rbSpecificProcess.Checked == true)
            {
                if (cbSpecificProcess.Items[cbSpecificProcess.SelectedIndex].ToString() != "No Processes Found")
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.SelectedProcess = cbSpecificProcess.Items[cbSpecificProcess.SelectedIndex].ToString();
                    processFound = true;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Invalid Process target criteria selected. Please review the selection.");
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
            eventTypesToolStripMenuItem.Enabled = false;

            AlterPurgeCriteriaPanel(false);
            AlterStartStopPanel(true);
            AlterPurgeStatsPanel(true);

            btnStopCleanup.Enabled = true;
            btnStartCleanup.Enabled = false;

            pCleanupCriteria.BackColor = SystemColors.Control;
            pStartStopButtons.BackColor = SystemColors.ControlLightLight;
            pCleanupStats.BackColor = SystemColors.ControlLightLight;

            btnStartCleanup.BackColor = SystemColors.Control;
            btnStopCleanup.BackColor = SystemColors.Control;

            btnStartCleanup.Enabled = false;
            btnStopCleanup.Enabled = true;
        }

        private void btnStopCleanup_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " STOP CLEANUP button was clicked.");
            StopPurgeThreadAndWaitForThreadStop();
            btnStartCleanup.Enabled = false;
            btnStopCleanup.Enabled = false;
        }

        private void StopPurgeThreadAndWaitForThreadStop()
        {
            LiveDbSpSqlController.DbSqlSpControllerData.RequestStop();
            while (!LiveDbSpSqlController.DbSqlSpControllerData.WorkerCompleted)
            {
                Thread.Sleep(100);
                Application.DoEvents();  // Does this hand back to the calling method to then allow GUI button changes?
            }
        }

        private void ModifyGuiAfterStopButtonClick()
        {
            eventTypesToolStripMenuItem.Enabled = true;

            AlterPurgeCriteriaPanel(true);
            AlterStartStopPanel(true);
            AlterPurgeStatsPanel(false);

            btnStopCleanup.Enabled = false;
            btnStartCleanup.Enabled = true;

            pCleanupCriteria.BackColor = SystemColors.ControlLightLight;
            pStartStopButtons.BackColor = SystemColors.ControlLightLight;
            pCleanupStats.BackColor = SystemColors.Control;

            btnStartCleanup.BackColor = SystemColors.Control;
            btnStopCleanup.BackColor = SystemColors.Control;

            btnStartCleanup.Enabled = true;
            btnStopCleanup.Enabled = false;
        }

        private void rbEveryone_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEveryone.Checked != true) return;
            rbSelectUser.Checked = false;
            cbSpecificUser.Enabled = false;
        }

        private void rbSelectUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectUser.Checked != true) return;
            rbEveryone.Checked = false;
            cbSpecificUser.Enabled = true;
            cbSpecificUser.SelectedIndex = 0;
        }

        private void rbAllComputers_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllComputers.Checked != true) return;
            rbSelectComputer.Checked = false;
            cbSpecificComputer.Enabled = false;
        }

        private void rbSpecificComputer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelectComputer.Checked != true) return;
            rbAllComputers.Checked = false;
            cbSpecificComputer.Enabled = true;
            cbSpecificComputer.SelectedIndex = 0;
        }

        private void StopDbConnTestThreadAndWaitForThreadStop()
        {
            _testDbConnectionThread.Abort();
            while (_testDbConnectionThread.IsAlive)
            {
                Application.DoEvents();
            }
        }


        private void processingStatsTimer_Tick(object sender, EventArgs e)
        {
            if (_sqlPurgeWorkerThread.IsAlive)
            {
                int curProgBarValue = toolStripProgressBar1.Value;
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
                remainingRunTimeMinutes.Text = (timeLeft);
            }
            else
            {
                processingStatsTimer.Enabled = false;
                if (LiveDbSpSqlController.DbSqlSpControllerData.WorkerCompleted)
                {
                    RecordsLeftToPurgeTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge.ToString();
                    percentageRecordsProcessedTextBox.Text = (((LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge - LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge) * 100) / LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge).ToString();
                    remainingRunTimeMinutes.Text = "0:00";
                    RecordsPurgedTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar.ToString();
                    toolStripProgressBar1.Value = 100;
                    toolStripStatusLabel1.Text = "Processing complete";
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " The Worker Thread has completed cleaning.");
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " --SUMMARY:-- ");
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar + ".");
                    MessageBox.Show("Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar,
                                                                      "Record Processing Complete",
                                                                      MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


                }
                else
                {
                    toolStripStatusLabel1.Text = "Processing stopped unexpectedly";
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " The Worker Thread is Dead. Enabling GUI.");
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " --SUMMARY:-- ");
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar + ".");
                    MessageBox.Show("Record processing stopped unexpectedly. Total Records Deleted: " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar + ".",
                                                                      "Record Processing Warning",
                                                                      MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
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
            if (runtimeHours.Text == "")
            {
                runtimeHours.Text = "0";
                runtimeHours.Value = 0;
            }
            if (runtimeMinutes.Text == "")
            {
                runtimeMinutes.Text = "0";
                runtimeMinutes.Value = 0;
            }
            if ((runtimeHours.Value == 0) && (runtimeMinutes.Value == 0))
            {
                runtimeMinutes.Value = 1;
            }
            if ((runtimeHours.Value == 90) && (runtimeMinutes.Value > 0))
            {
                runtimeMinutes.Value = 0;
            }
        }

        private void dtpCutOffDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpCutOffDate.Value.Date < DateTime.Today.Date) return;
            MessageBox.Show("You cannot select today or a future date as the cut-off date. Please select an older date.", "Invalid date selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " User tried to select " + dtpCutOffDate.Value.Date.ToString() + " as the cut-off date.");
            dtpCutOffDate.Value = DateTime.Today.AddDays(-1);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void eventTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IESandDACadmt.Forms.FormEventtypeSelection eventTypeForm = new IESandDACadmt.Forms.FormEventtypeSelection(LiveDbSpSqlController);
            eventTypeForm.StartPosition = FormStartPosition.CenterParent;
            eventTypeForm.Show();
        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation);
        }

        private void rbSpecificProcess_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSpecificProcess.Checked == true)
            {
                rbAllProcesses.Checked = false;
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
                        cbSpecificProcess.Enabled = true;
                        cbSpecificProcess.SelectedIndex = 0;
                    }
                    else
                    {
                        ModifyByProcessControlsForNoProcessInfo();
                    }
                }
                else
                {
                    DialogResult result1 = MessageBox.Show("This data will need to be retrieved from the database and this could take some time. Do you want to proceed with this?", "Process-related SQL query", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result1 == System.Windows.Forms.DialogResult.Yes)
                    {
                        // Run the Query in a new thread
                        toolStripStatusLabel1.Text = "Querying for top processes...";
                        try
                        {
                            IESandDACadmt.Sql.SqlReadByProcessInfoThread workerReadSqlByProcess = new IESandDACadmt.Sql.SqlReadByProcessInfoThread(LiveDbSpSqlController,
                                                                                                                       LiveDbSpSqlController.DbSqlSpControllerData.SqlConnectionString,
                                                                                                                       3600,
                                                                                                                       IESandDACadmt.BL.RecordsProfilingQueryLogic.EmssByProcessQuery,
                                                                                                                       LiveDbSpSqlController.DbSqlSpControllerData.ByProcessQueryDataTable);
                            _readByProcessSqlInfoThread = new Thread(workerReadSqlByProcess.ReadByProcessSqlInfo);
                            _readByProcessSqlInfoThread.IsBackground = true;
                            _readByProcessSqlInfoThread.Start();
                            readByProcessInfoTimer.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " " + ex.Message.ToString());
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
            rbAllProcesses.Checked = true;
            rbAllProcesses.Enabled = true;
            rbSpecificProcess.Checked = false;
            rbSpecificProcess.Enabled = true;
            cbSpecificProcess.Enabled = false;
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
                    cbSpecificProcess.Enabled = true;
                    cbSpecificProcess.SelectedIndex = 0;
                }
                else
                {
                    ModifyByProcessControlsForNoProcessInfo();
                }
                toolStripProgressBar1.Value = 100;
                toolStripStatusLabel1.Text = "Finished reading processes";
                readByProcessInfoTimer.Enabled = false;
            }
            else
            {
                // Make the progress bar increment.
                int curProgBarValue = toolStripProgressBar1.Value;
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
            if (rbAllProcesses.Checked == true)
            {
                rbSpecificProcess.Checked = false;
                cbSpecificProcess.Enabled = false;
            }

        }

        public bool StartCleanup()
        {
            bool wasTheThreadStarted = false;
            LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar = 0;
            try
            {
                SqlDbCleanupThread worker = new SqlDbCleanupThread(LiveDbSpSqlController);
                worker.BatchProcessed += UpdateAfterBatchProcessed;
                _sqlPurgeWorkerThread = new Thread(worker.StartProcessing);
                _sqlPurgeWorkerThread.IsBackground = true;
                _sqlPurgeWorkerThread.Start();
                Thread.Sleep(100);
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " SQL Worker Thread Started.");
                wasTheThreadStarted = true;
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " " + ex.Message.ToString());
            }
            return wasTheThreadStarted;
        }

        public void UpdateAfterBatchProcessed(object sender, SqlDeletionEventargs e)
        {
            // Update the values that get saved to LiveSpSql....
            if (e.RecordsDeletedThisBatch == LiveDbSpSqlController.DbSqlSpControllerData.RecordsForBatchSize)
            {
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Batch of " + LiveDbSpSqlController.DbSqlSpControllerData.RecordsForBatchSize.ToString() + " processed.");
                LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge -= e.RecordsDeletedThisBatch;
                LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar += e.RecordsDeletedThisBatch;
            }
            else
            {
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Last Batch of " + e.RecordsDeletedThisBatch.ToString() + " processed.");
                LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge = 0;
                LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar += e.RecordsDeletedThisBatch;
            }
            // Update the GUI with the new values......
            UpdateGuiWithNewValues(LiveDbSpSqlController);
        }

        private void UpdateGuiWithNewValues(DbSqlSpController updatedResults)
        {
            if (this.RecordsLeftToPurgeTextBox.InvokeRequired)
            {
                UpdateGuiWithNewValuesCallBack del = new UpdateGuiWithNewValuesCallBack(UpdateGuiWithNewValues);
                this.Invoke(del, new object[] { updatedResults });
            }
            else
            {
                RecordsLeftToPurgeTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge.ToString();
                double percentage = (((LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge - LiveDbSpSqlController.DbSqlSpControllerData.RemainingRowsToPurge) * 100) / LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge);
                percentageRecordsProcessedTextBox.Text = Math.Round(percentage, 2).ToString();
                RecordsPurgedTextBox.Text = LiveDbSpSqlController.DbSqlSpControllerData.RecordsProcessedSoFar.ToString();
            }
        }

        private void timerCalcTotRecToPurge_Tick(object sender, EventArgs e)
        {
            if (_calculateTotalRecordsToPurgeThread.IsAlive)
            {
                int curProgBarValue = toolStripProgressBar1.Value;
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
                timerCalcTotRecToPurge.Enabled = false;
                if (LiveDbSpSqlController.DbSqlSpControllerData.Result.Success)
                {
                    toolStripProgressBar1.Value = 100;
                    if (LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge > 0)
                    {
                        LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Calculation of total Records to Purge: " + LiveDbSpSqlController.DbSqlSpControllerData.Result.Message.ToString() + " : " + LiveDbSpSqlController.DbSqlSpControllerData.ReturnedTotalRowsToPurge.ToString() + ".");
                        string whoSpWillCleanfor = BuildWarningMessage();
                        DialogResult goNoGoResponse = MessageBox.Show(whoSpWillCleanfor,
                                                                      "Record Deletion Warning",
                                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (goNoGoResponse == DialogResult.Yes)
                        {
                            processingStatsTimer.Enabled = true;
                            //btnChangeSqlServer.Enabled = false;
                            if (StartCleanup() == false)
                            {
                                // Thread did not start so run stop button code
                                processingStatsTimer.Enabled = false;
                                ModifyGuiAfterStopButtonClick();
                            }
                        }
                        else
                        {
                            ModifyGuiAfterStopButtonClick();
                            ModifyGuiAfterGoPromptCanceled();
                            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " START DELETION confirmation-message canceled by user.");
                        }
                    }
                    else
                    {
                        processingStatsTimer.Enabled = false;
                        MessageBox.Show("No records match these characteristics.", "Target records calculation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SetGuiStatsToZeros();
                        ModifyGuiAfterStopButtonClick();
                        ModifyGuiAfterNoTargetRecordsFound();
                        LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " No events match these selected characteristics.");
                    }
                }
                else
                {
                    SetGuiStatsToZeros();
                    ModifyGuiAfterStopButtonClick();
                    ModifyGuiAfterGoPromptCanceled();
                    LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " " + LiveDbSpSqlController.DbSqlSpControllerData.Result.Message);
                    toolStripStatusLabel1.Text = "Failed to calculate records to purge. See Log File";

                }

            }
        }

        private void rbNoCutOffDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoCutOffDate.Checked == true)
            {
                rbCutOffDate.Checked = false;
                dtpCutOffDate.Enabled = false;
            }
        }

        private void rbCutOffDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCutOffDate.Checked == true)
            {
                rbNoCutOffDate.Checked = false;
                dtpCutOffDate.Enabled = true;
            }
        }

        private void cbBatchSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void runtimeMinutes_Enter(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

        private void runtimeHours_Enter(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

        private void runtimeMinutes_Leave(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }

        private void runtimeHours_Leave(object sender, EventArgs e)
        {
            RuntimesLogicCheck();
        }
    }
}
