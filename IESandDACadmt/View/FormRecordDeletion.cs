using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using IESandDACadmt.Data;
using IESandDACadmt.Logging;
using IESandDACadmt.Sql;


namespace IESandDACadmt.Forms
{
    public partial class FormRecordDeletion : Form
	{
		public volatile DbSqlSpController LiveDbSpSqlController = new DbSqlSpController();
		Thread _testDbConnectionThread = null;
		Thread _readByProcessSqlInfoThread = null;
		Thread _calculateTotalRecordsToPurgeThread = null;
        Thread _sqlPurgeWorkerThread = null;

        delegate void UpdateGuiWithNewValuesCallBack(DbSqlSpController theLiveDbSpSqlController);

        public FormRecordDeletion(DbSqlSpController theDbSqlSpController)
		{
            LiveDbSpSqlController = theDbSqlSpController;
            LiveDbSpSqlController.BuildEventTypesDictionary();
            InitializeComponent();
			SetGuiForServerType();
            cbSpecificUser.Items.Clear();
            cbSpecificComputer.Items.Clear();
        }
	
		private void SetGuiForServerType()
		{
            this.Text = "AC/DC Record Deletion Tool    Server: " + LiveDbSpSqlController.DbServeraddress;
		}
	
		private void LoadUsersComputersIntoGui()
		{
			cbSpecificUser.Items.Clear();
			cbSpecificComputer.Items.Clear();
            if (LiveDbSpSqlController.UserList.Count > 0)
            {
                foreach (var user in LiveDbSpSqlController.UserList)
                {
                    cbSpecificUser.Items.Add(user);
                }
            }
            else
            {
                cbSpecificUser.Items.Add("No Users Found");
            }
            if (LiveDbSpSqlController.ComputerList.Count > 0)
            {
                foreach (var computer in LiveDbSpSqlController.ComputerList)
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
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " The known Users and Computers read from SQL and loaded into GUI.");
            cbSpecificComputer.DropDown += new System.EventHandler(this.cbSpecificComputer_DropDown);
			cbSpecificUser.DropDown += new System.EventHandler(cbSpecificUser_DropDown);
            toolStripStatusLabel1.Text = "Connected to:" + LiveDbSpSqlController.DbServeraddress + " User:" + LiveDbSpSqlController.SqlConnUserName;
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
			LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " START CLEANUP button was clicked.");
			ModifyGuiAfterStartButtonClick();
			ActionOutcome theResult = LoadGuiValuesIntoDbSqlSpController();
            if (theResult.Success)
            {
                DialogResult backupDone = MessageBox.Show("It is highly recommended to have a backup of the " + LiveDbSpSqlController.DataBaseName + " database before deleting records. Do you want to continue with the deletion?", "Backup check for database " + LiveDbSpSqlController.DataBaseName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
			LiveDbSpSqlController.StopController = false;
			toolStripStatusLabel1.Text = "Building required Stored Procedures...";
			var parameterValidationResult = LiveDbSpSqlController.ValidateParameters();
			if (parameterValidationResult == "success")
			{
				LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " GUI Parameters validation and SQL Stored Procedure creation successful.");
				_calculateTotalRecordsToPurgeThread = new Thread(LiveDbSpSqlController.CalculateTotalRecordsToPurge);
				_calculateTotalRecordsToPurgeThread.IsBackground = true;
				_calculateTotalRecordsToPurgeThread.Start();
				Thread.Sleep(100);
				timerCalcTotRecToPurge.Enabled = true;
				toolStripStatusLabel1.Text = "Identifying target records...";
			}
			else
			{
				LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, parameterValidationResult.ToString());
                MessageBox.Show("Error validating the GUI parameters or Creating SQL Stored Procedures. See the Log File for further details.", "Error Processing Criteria", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, parameterValidationResult);
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
            whoSpWillCleanfor.Append("This action will delete " + LiveDbSpSqlController.ReturnedTotalRowsToPurge.ToString(CultureInfo.InvariantCulture) + " records for ");
            if (LiveDbSpSqlController.SelectedUser.ToLower() == "everyone")
            {
                whoSpWillCleanfor.Append(" All Users");
            }
            else
            {
                whoSpWillCleanfor.Append(" " + LiveDbSpSqlController.SelectedUser);
            }
            if (LiveDbSpSqlController.SelectedComputer.ToLower() == "all")
            {
                whoSpWillCleanfor.Append(" from All Computers");
            }
            else
            {
                whoSpWillCleanfor.Append(" from " + LiveDbSpSqlController.SelectedComputer);
            }
            whoSpWillCleanfor.Append(" older than " + LiveDbSpSqlController.CutOffDate.ToString(CultureInfo.InvariantCulture));

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
            LiveDbSpSqlController.RunTime = (((int)runtimeHours.Value * 60) + (int)runtimeMinutes.Value);
			LiveDbSpSqlController.BatchSize = cbBatchSize.Items[cbBatchSize.SelectedIndex].ToString();
			if (rbNoCutOffDate.Checked == true)
			{
                LiveDbSpSqlController.CutOffDate = DateTime.Today;
                LiveDbSpSqlController.CutOffDays = false;
			}
			else
			{
                LiveDbSpSqlController.CutOffDate = dtpCutOffDate.Value.Date;
				LiveDbSpSqlController.CutOffDays = true;
			}
			if (rbEveryone.Checked == true)
			{
				LiveDbSpSqlController.SelectedUser = "everyone";
				LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Selected User: EVERYONE.");
                userFound = true;
            }
			if (rbSelectUser.Checked == true)
			{
				string selectedUserEntry = cbSpecificUser.Items[cbSpecificUser.SelectedIndex].ToString();
                if (selectedUserEntry != "No Users Found")
                {
                    LiveDbSpSqlController.UserSid = selectedUserEntry.Substring((selectedUserEntry.IndexOf(":", StringComparison.Ordinal) + 1),
                                                                                        (selectedUserEntry.Length - selectedUserEntry.IndexOf(":", StringComparison.Ordinal) - 1));
                    LiveDbSpSqlController.SelectedUser = selectedUserEntry.Substring(0, selectedUserEntry.IndexOf(":", StringComparison.Ordinal));
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Selected UserSID: " + LiveDbSpSqlController.UserSid.ToString() +
                                                           " . Selected User Name: " + LiveDbSpSqlController.SelectedUser.ToString() + ".");
                    userFound = true; 
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Invalid User target criteria selected. Please review the selection.");
                    userFound = false;
                    errorMessage = " Invalid User target criteria selected. Please review the selection.";
                }
			}
			if (rbAllComputers.Checked == true)
			{
				LiveDbSpSqlController.SelectedComputer = "all";
				LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Selected Computer: ALL.");
                computerFound = true;
            }
			if (rbSelectComputer.Checked == true)
			{
				string selectedComputerEntry = cbSpecificComputer.Items[cbSpecificComputer.SelectedIndex].ToString();
                if (selectedComputerEntry != "No Computers Found")
                {
                    LiveDbSpSqlController.EpsGuid = selectedComputerEntry.Substring((selectedComputerEntry.IndexOf(":", StringComparison.Ordinal) + 1),
                                                                                            (selectedComputerEntry.Length - selectedComputerEntry.IndexOf(":", StringComparison.Ordinal) - 1));
                    LiveDbSpSqlController.SelectedComputer = selectedComputerEntry.Substring(0, selectedComputerEntry.IndexOf(":", StringComparison.Ordinal));
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Selected EPSGUID: " + LiveDbSpSqlController.EpsGuid.ToString() +
                                                           " . Selected Endpoint Name: " + LiveDbSpSqlController.SelectedComputer.ToString() + ".");
                    computerFound = true;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Invalid Computer target criteria selected. Please review the selection.");
                    computerFound = false;
                    errorMessage += " .Invalid Computer target criteria selected. Please review the selection.";
                }
			}

			if (rbAllProcesses.Checked == true)
			{
				LiveDbSpSqlController.SelectedProcess = "all";
				LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Selected Process: ALL.");
                processFound = true;
			}
			if (rbSpecificProcess.Checked == true)
			{
                if (cbSpecificProcess.Items[cbSpecificProcess.SelectedIndex].ToString() != "No Processes Found")
                {
                    LiveDbSpSqlController.SelectedProcess = cbSpecificProcess.Items[cbSpecificProcess.SelectedIndex].ToString();
                    processFound = true;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Invalid Process target criteria selected. Please review the selection.");
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
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " STOP CLEANUP button was clicked.");
            StopPurgeThreadAndWaitForThreadStop();
			btnStartCleanup.Enabled = false;
			btnStopCleanup.Enabled = false;
		}

		private void StopPurgeThreadAndWaitForThreadStop()
		{
			LiveDbSpSqlController.RequestStop();
			while (!LiveDbSpSqlController.WorkerCompleted)
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
				if (LiveDbSpSqlController.StopController)
				{

                    toolStripStatusLabel1.Text = "Stopping current processing...";
				}
				else
				{
					toolStripStatusLabel1.Text = "Processing Records...";
				}
                double daysRemaining = Math.Floor((double)LiveDbSpSqlController.ProcessingEndTime.Subtract(DateTime.Now).Days);
                int theDaysRemaining = Convert.ToInt32(daysRemaining);
                double hoursRemaining = Math.Floor((double)LiveDbSpSqlController.ProcessingEndTime.Subtract(DateTime.Now).Hours);
                int theHoursRemaining = Convert.ToInt32(hoursRemaining);
                double minsRemaining = Math.Floor((double)LiveDbSpSqlController.ProcessingEndTime.Subtract(DateTime.Now).Minutes);
                int theMinsRemaining = Convert.ToInt32(minsRemaining);
                int secsRemaining = LiveDbSpSqlController.ProcessingEndTime.Subtract(DateTime.Now).Seconds;
                string timeLeft = PadStringIfBelowTen(theDaysRemaining) + "d " + PadStringIfBelowTen(theHoursRemaining) + "h " + PadStringIfBelowTen(theMinsRemaining) + "m " + PadStringIfBelowTen(secsRemaining) + "s";
                remainingRunTimeMinutes.Text = (timeLeft);
            }
			else
			{
				processingStatsTimer.Enabled = false;
				if (LiveDbSpSqlController.WorkerCompleted)
				{
					RecordsLeftToPurgeTextBox.Text = LiveDbSpSqlController.RemainingRowsToPurge.ToString();
					percentageRecordsProcessedTextBox.Text = (((LiveDbSpSqlController.ReturnedTotalRowsToPurge - LiveDbSpSqlController.RemainingRowsToPurge) * 100) / LiveDbSpSqlController.ReturnedTotalRowsToPurge).ToString();
					remainingRunTimeMinutes.Text = "0:00";
					RecordsPurgedTextBox.Text = LiveDbSpSqlController.RecordsProcessedSoFar.ToString();
					toolStripProgressBar1.Value = 100;
					toolStripStatusLabel1.Text = "Processing complete";
					LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " The Worker Thread has completed cleaning.");
					LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " --SUMMARY:-- ");
					LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Total Records Deleted: " + LiveDbSpSqlController.RecordsProcessedSoFar + ".");
					MessageBox.Show("Total Records Deleted: " + LiveDbSpSqlController.RecordsProcessedSoFar,
																	  "Record Processing Complete",
																	  MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


				}
				else
				{
					toolStripStatusLabel1.Text = "Processing stopped unexpectedly";
					LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " The Worker Thread is Dead. Enabling GUI.");
					LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " --SUMMARY:-- ");
					LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Total Records Deleted: " + LiveDbSpSqlController.RecordsProcessedSoFar + ".");
					MessageBox.Show("Record processing stopped unexpectedly. Total Records Deleted: " + LiveDbSpSqlController.RecordsProcessedSoFar + ".",
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
			LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " User tried to select " + dtpCutOffDate.Value.Date.ToString() + " as the cut-off date.");
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
			Process.Start("notepad.exe", LiveDbSpSqlController.LogFileLocation);
		}

		private void rbSpecificProcess_CheckedChanged(object sender, EventArgs e)
		{
			if (rbSpecificProcess.Checked == true)
			{
				rbAllProcesses.Checked = false;
				if (LiveDbSpSqlController.ByProcessQueryAlreadyRan)
				{
					// Sort the already gotten results into GUI CB alphabetically.
					LiveDbSpSqlController.ByProcessResults.Sort();
					cbSpecificProcess.Items.Clear();
					if (LiveDbSpSqlController.ByProcessResults.Count > 0)
					{
						foreach (string processString in LiveDbSpSqlController.ByProcessResults)
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
																													   LiveDbSpSqlController.SqlConnectionString,
																													   3600,
																													   IESandDACadmt.BL.RecordsProfilingQueryLogic.EmssByProcessQuery,
																													   LiveDbSpSqlController.ByProcessQueryDataTable);
							_readByProcessSqlInfoThread = new Thread(workerReadSqlByProcess.ReadByProcessSqlInfo);
							_readByProcessSqlInfoThread.IsBackground = true;
							_readByProcessSqlInfoThread.Start();
							readByProcessInfoTimer.Enabled = true;
						}
						catch (Exception ex)
						{
							LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, " " + ex.Message.ToString());
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
			if (LiveDbSpSqlController.ByProcessQuerySuccess == true)
			{
				if (LiveDbSpSqlController.ByProcessQueryDataTable.Rows.Count > 0)
				{
					foreach (DataRow row in LiveDbSpSqlController.ByProcessQueryDataTable.Rows)
					{
						LiveDbSpSqlController.ByProcessResults.Add(row["ProcessName"].ToString());
					}
					LiveDbSpSqlController.ByProcessQueryAlreadyRan = true;
				}
                else
                {
                    LiveDbSpSqlController.ByProcessResults.Add("No Processes Found");
                }
				LiveDbSpSqlController.ByProcessResults.Sort();
				if (LiveDbSpSqlController.ByProcessResults.Count > 0)
				{
					cbSpecificProcess.Items.Clear();
					foreach (string processString in LiveDbSpSqlController.ByProcessResults)
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
            LiveDbSpSqlController.RecordsProcessedSoFar = 0;
            try
            {
                SqlDbCleanupThread worker = new SqlDbCleanupThread(LiveDbSpSqlController);
                worker.BatchProcessed += UpdateAfterBatchProcessed;
                _sqlPurgeWorkerThread = new Thread(worker.StartProcessing);
                _sqlPurgeWorkerThread.IsBackground = true;
                _sqlPurgeWorkerThread.Start();
                Thread.Sleep(100);
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " SQL Worker Thread Started.");
                wasTheThreadStarted = true;
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, " " + ex.Message.ToString());
            }
            return wasTheThreadStarted;
        }

        public void UpdateAfterBatchProcessed(object sender, SqlDeletionEventargs e)
        {
            // Update the values that get saved to LiveSpSql....
            if (e.RecordsDeletedThisBatch == LiveDbSpSqlController.RecordsForBatchSize)
            {
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Batch of " + LiveDbSpSqlController.RecordsForBatchSize.ToString() + " processed.");
                LiveDbSpSqlController.RemainingRowsToPurge -= e.RecordsDeletedThisBatch;
                LiveDbSpSqlController.RecordsProcessedSoFar += e.RecordsDeletedThisBatch;
            }
            else
            {
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Last Batch of " + e.RecordsDeletedThisBatch.ToString() + " processed.");
                LiveDbSpSqlController.RemainingRowsToPurge = 0;
                LiveDbSpSqlController.RecordsProcessedSoFar += e.RecordsDeletedThisBatch;
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
                RecordsLeftToPurgeTextBox.Text = LiveDbSpSqlController.RemainingRowsToPurge.ToString();
                double percentage = (((LiveDbSpSqlController.ReturnedTotalRowsToPurge - LiveDbSpSqlController.RemainingRowsToPurge) * 100) / LiveDbSpSqlController.ReturnedTotalRowsToPurge);
                percentageRecordsProcessedTextBox.Text = Math.Round(percentage, 2).ToString();
                RecordsPurgedTextBox.Text = LiveDbSpSqlController.RecordsProcessedSoFar.ToString();
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
				if (LiveDbSpSqlController.Result.Success)
				{
					toolStripProgressBar1.Value = 100;
					if (LiveDbSpSqlController.ReturnedTotalRowsToPurge > 0)
					{
						LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Calculation of total Records to Purge: " + LiveDbSpSqlController.Result.Message.ToString() + " : " + LiveDbSpSqlController.ReturnedTotalRowsToPurge.ToString() + ".");
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
							LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " START DELETION confirmation-message canceled by user.");
						}
					}
					else
					{
						processingStatsTimer.Enabled = false;
						MessageBox.Show("No records match these characteristics.", "Target records calculation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						SetGuiStatsToZeros();
						ModifyGuiAfterStopButtonClick();
						ModifyGuiAfterNoTargetRecordsFound();
						LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " No events match these selected characteristics.");
					}
				}
				else
				{
					SetGuiStatsToZeros();
					ModifyGuiAfterStopButtonClick();
					ModifyGuiAfterGoPromptCanceled();
					LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, " " + LiveDbSpSqlController.Result.Message);
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
    }   //End of Class
}   