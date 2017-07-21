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

namespace IESandDACadmt.View
{
    /// <summary>
    /// Interaction logic for WpfLauncher.xaml
    /// </summary>
    public partial class WpfLauncher : Window
    {
        public WpfLauncher()
        {
            InitializeComponent();
            LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = Model.ServerDetectionLogic.CheckServerType(LiveDbSpSqlController, _serverDetectionData);
            comboBoxServerType.Items.Add("EMSS");
            comboBoxServerType.Items.Add("ES");
            comboBoxServerType.SelectedItem = "EMSS";
            comboBoxSqlAuthType.Items.Add("Windows Authentication");
            comboBoxSqlAuthType.Items.Add("SQL Authentication");
            comboBoxSqlAuthType.SelectedItem = "Windows Authentication";
        }

        public volatile Model.DbSqlSpController LiveDbSpSqlController = new Model.DbSqlSpController();
        Model.Sql.SqlTestDbConnection _workerTestSql = null;
        Thread _testDbConnectionThread = null;
        FormRecordsProfiler _dataProfilerPage = null;
        FormRecordDeletion _recordPurgePage = null;
        ViewModel.ServerDetectionData _serverDetectionData = new ViewModel.ServerDetectionData();

        private void SetLauncherGui(Model.DbSqlSpController theLiveData)
        {
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.EMSS)
            {
                this.Title = "EMSS Advanced Database Maintenance Tool";
            }
            if (theLiveData.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.ES)
            {
                this.Title = "ES Advanced Database Maintenance Tool";
            }
            tbDbServerName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress;
            tbDatabaseName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName;
        }

        private void FormLauncher_Load(object sender, EventArgs e)
        {
            if (LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType == ViewModel.DbSqlSpControllerData.ServerType.UNKNOWN)
            {
                MessageBox.Show("Could not detect Heat Server Type. Please select it from the Heat Server Type drop-down list.", "Unknown Server Type", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                ModifyGuiOnUnknownServerType();
            }
            else
            {
                comboBoxServerType.SelectedItem = LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType.ToString();
                ModifyGuiOnKnownServerType();
                if (LiveDbSpSqlController.DbSqlSpControllerData.SqlConnectionStringFound)
                {
                    SetLauncherGui(LiveDbSpSqlController);
                    DialogResult okToUseFoundString = MessageBox.Show("Discovered Server Name " + LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress + ", do you want to use this?",
                                                                        "Discovered server details",
                                                                        MessageBoxButton.YesNo,
                                                                        MessageBoxImage.Question);
                    if (okToUseFoundString == DialogResult.Yes)
                    {
                        tbDbServerName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress;
                        tbDatabaseName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName;
                        //btnTestDBConnection_Click(this, e = new EventArgs());
                    }
                    else
                    {
                        PromptForSqlDetails();
                        tbDatabaseName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName;
                    }
                }
                else
                {
                    PromptForSqlDetails();
                    tbDatabaseName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName;
                }
            }
        }

        private void PromptForSqlDetails()
        {
            MessageBox.Show(@"Please provide SQL Server name\instance and Database Name details.", "Provide Details", MessageBoxButton.OK, MessageBoxImage.Information);
            tbDbServerName.Text = "";
            tbDatabaseName.Text = "";
        }

        private void ModifyGuiOnUnknownServerType()
        {
            panelServerType.Enabled = true;
            panelDbConnection.Enabled = false;
            panelProfiler.Enabled = false;
            panelPurge.Enabled = false;
            panelHealth.Enabled = false;
            toolStripStatusLabelLauncher.Text = "Unknown Server Type";

            panelServerType.BackColor = SystemColors.ControlLightLight;
            panelDbConnection.BackColor = SystemColors.Control;
            panelProfiler.BackColor = SystemColors.Control;
            panelPurge.BackColor = SystemColors.Control;
            panelHealth.BackColor = SystemColors.Control;
        }

        private void ModifyGuiOnKnownServerType()
        {
            panelServerType.Enabled = true;
            panelDbConnection.Enabled = true;
            panelProfiler.Enabled = false;
            panelPurge.Enabled = false;
            panelHealth.Enabled = false;
            toolStripStatusLabelLauncher.Text = " Server-Type:" + LiveDbSpSqlController.HeatServerType.ToString();

            panelServerType.BackColor = SystemColors.Control;
            panelDbConnection.BackColor = SystemColors.ControlLightLight;
            panelProfiler.BackColor = SystemColors.Control;
            panelPurge.BackColor = SystemColors.Control;
            panelHealth.BackColor = SystemColors.Control;

            switch (LiveDbSpSqlController.HeatServerType)
            {
                case DbSqlSpController.ServerType.UNKNOWN:
                    tbDatabaseName.Enabled = true;
                    break;
                case DbSqlSpController.ServerType.EMSS:
                    tbDatabaseName.Enabled = false;
                    break;
                case DbSqlSpController.ServerType.ES:
                    tbDatabaseName.Enabled = true;
                    break;
                default:
                    tbDatabaseName.Enabled = true;
                    break;
            }

        }

        private void btnTestDBConnection_Click(object sender, EventArgs e)
        {
            ModifyGuiOnTestButtonClick();
            Thread.Sleep(100);
            var inputOk = TestUserInput();
            if (inputOk)
            {
                toolStripStatusLabel1.Text = "Testing Connection...";
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " Test DB Connection button was clicked.");
                SetSqlSearchCommandsByServerType();
                if (LiveDbSpSqlController.AltCredentialsSelected == false)
                {
                    LiveDbSpSqlController.SqlConnUserName = WindowsIdentity.GetCurrent().Name;
                }
                TestSqlDbConnection();
            }
            else
            {
                ModifyGuiOnDbTestFail();
            }
        }

        private bool TestUserInput()
        {
            //  Update this method to ensure no SQL injection possible
            if ((string.IsNullOrEmpty(tbDbServerName.Text)) || (string.IsNullOrEmpty(tbDatabaseName.Text)))
            {
                MessageBox.Show("SQL Server name and/or Database name is invalid. Please enter them again.", "Invalid Server Details", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, "SQL Server name and/or Database name is invalid.");
                return false;

            }
            else
            {
                LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress = tbDbServerName.Text.ToString();
                LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName = tbDatabaseName.Text.ToString();
                return true;
            }
        }

        private void TestSqlDbConnection()
        {
            TestDbConnectionAndReadUcData();
            Thread.Sleep(100);
            dbConnectionTestTimer.Enabled = true;
        }

        private void SetSqlSearchCommandsByServerType()
        {
            switch (LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType)
            {
                case ViewModel.DbSqlSpControllerData.ServerType.EMSS:
                    LiveDbSpSqlController.DbSqlSpControllerData.ComputerReadSqlCode = _serverDetectionData.EmssComputerReadQuery;
                    LiveDbSpSqlController.DbSqlSpControllerData.UserReadSqlCode = _serverDetectionData.EmssUserReadQuery;
                    break;
                case ViewModel.DbSqlSpControllerData.ServerType.ES:
                    LiveDbSpSqlController.DbSqlSpControllerData.ComputerReadSqlCode = _serverDetectionData.EsComputerReadQuery;
                    LiveDbSpSqlController.DbSqlSpControllerData.UserReadSqlCode = _serverDetectionData.EsUserReadQuery;
                    break;
                default:
                    break;
            }

        }

        public void TestDbConnectionAndReadUcData()
        {
            LiveDbSpSqlController.BuildSqlConnectionString();
            try
            {
                _workerTestSql = new SqlTestDbConnection(LiveDbSpSqlController);
                _testDbConnectionThread = new Thread(_workerTestSql.TestDbConnection) { IsBackground = true };
                _testDbConnectionThread.Start();
            }
            catch (Exception ex)
            {
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, " " + ex.Message.ToString());
            }
        }

        private void btnChangeSqlServer_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " CHANGE SQL SERVER button was clicked.");
            dbConnectionTestTimer.Enabled = false;
            toolStripStatusLabel1.Text = "Connection attempt stopped";
            ModifyGuiOnFormLoad();
            toolStripProgressBar1.Value = 0;
            ViewModel.DbSqlSpControllerData.ServerType tempServerType = LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType;
            Dictionary<string, bool> tempo = LiveDbSpSqlController.DbSqlSpControllerData.EventTypesToDelete;
            LiveDbSpSqlController = new Model.DbSqlSpController();
            LiveDbSpSqlController.DbSqlSpControllerData.EventTypesToDelete = tempo;
            LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = tempServerType;
        }

        private void ModifyGuiOnFormLoad()
        {
            btnChangeSqlServer.Enabled = false;
            btnTestDBConnection.Enabled = true;
            tbDbServerName.Enabled = true;
            tbDatabaseName.Enabled = true;

            panelHealth.Enabled = false;
            panelProfiler.Enabled = false;
            panelPurge.Enabled = false;

            panelDbConnection.BackColor = SystemColors.ControlLightLight;
            panelProfiler.BackColor = SystemColors.Control;
            panelPurge.BackColor = SystemColors.Control;
            panelHealth.BackColor = SystemColors.Control;
        }

        private void ModifyGuiOnTestButtonClick()
        {
            btnChangeSqlServer.Enabled = true;
            btnTestDBConnection.Enabled = false;
            tbDbServerName.Enabled = false;
            tbDatabaseName.Enabled = false;

            panelHealth.Enabled = false;
            panelProfiler.Enabled = false;
            panelPurge.Enabled = false;

            panelDbConnection.BackColor = SystemColors.Control;
            panelProfiler.BackColor = SystemColors.Control;
            panelPurge.BackColor = SystemColors.Control;
            panelHealth.BackColor = SystemColors.Control;
        }

        private void ModifyGuiOnDbTestSuccess()
        {
            btnChangeSqlServer.Enabled = true;
            btnTestDBConnection.Enabled = false;
            tbDbServerName.Enabled = false;
            tbDatabaseName.Enabled = false;

            panelHealth.Enabled = true;
            panelProfiler.Enabled = true;
            panelPurge.Enabled = true;

            panelDbConnection.BackColor = SystemColors.Control;
            panelProfiler.BackColor = SystemColors.ControlLightLight;
            panelPurge.BackColor = SystemColors.ControlLightLight;
            panelHealth.BackColor = SystemColors.ControlLightLight;
        }

        private void ModifyGuiOnDbTestFail()
        {
            btnChangeSqlServer.Enabled = false;
            btnTestDBConnection.Enabled = true;
            tbDbServerName.Enabled = true;
            tbDatabaseName.Enabled = true;

            panelHealth.Enabled = false;
            panelProfiler.Enabled = false;
            panelPurge.Enabled = false;

            panelDbConnection.BackColor = SystemColors.ControlLightLight;
            panelProfiler.BackColor = SystemColors.Control;
            panelPurge.BackColor = SystemColors.Control;
            panelHealth.BackColor = SystemColors.Control;
        }

        private void buttonLaunchProfiler_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " SQL AC/DC Record Profiler Tool launched.");
            _dataProfilerPage = new FormRecordsProfiler(LiveDbSpSqlController);
            _dataProfilerPage.ShowDialog();
        }

        private void buttonLaunchPurger_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " SQL AC/DC Record Purge Tool launched.");
            _recordPurgePage = new FormRecordDeletion(LiveDbSpSqlController);
            _recordPurgePage.ShowDialog();
        }

        private void buttonLaunchHealthReview_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " SQL Health Review Tool launched.");
            Form healthReviewForm = new FormHealthReview(LiveDbSpSqlController);
            healthReviewForm.ShowDialog();
        }

        private void dbConnectionTestTimer_Tick(object sender, EventArgs e)
        {
            if (LiveDbSpSqlController.DbSqlSpControllerData.DbTestStillRunning)
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
                dbConnectionTestTimer.Enabled = false;
                if (LiveDbSpSqlController.OperationResult != "success")
                {
                    LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.LogFileLocation, LiveDbSpSqlController.OperationResult.ToString());
                    MessageBox.Show("Error connecting to " + tbDbServerName.Text + ". Please check the Log File for further details.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel1.Text = "Connection Failed";
                    ModifyGuiOnFormLoad();
                    toolStripProgressBar1.Value = 0;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.LogFileLocation, " The Database test succeeded.");
                    toolStripStatusLabel1.Text = "Connected to:" + LiveDbSpSqlController.DbServeraddress + " User:" + LiveDbSpSqlController.SqlConnUserName;
                    ModifyGuiOnDbTestSuccess();
                    toolStripProgressBar1.Value = 100;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _testDbConnectionThread = null;
            _workerTestSql = null;
            _dataProfilerPage = null;
            _recordPurgePage = null;
            Application.Exit();
        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation);
        }

        private void requirementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IESandDACadmt.Forms.FormHelpRequirements helpRequirements = new IESandDACadmt.Forms.FormHelpRequirements();
            helpRequirements.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IESandDACadmt.Forms.FormAbout about = new IESandDACadmt.Forms.FormAbout();
            about.Show();
        }



        private void buttonChangeServerType_Click(object sender, EventArgs e)
        {
            btnChangeSqlServer_Click(this, new EventArgs());
            string userSelectedServerType = comboBoxServerType.Items[comboBoxServerType.SelectedIndex].ToString();
            if (userSelectedServerType == "EMSS")
            {
                LiveDbSpSqlController.HeatServerType = DbSqlSpController.ServerType.EMSS;
                LiveDbSpSqlController.DataBaseName = "UPCCommon";
            }
            else if (userSelectedServerType == "ES")
            {
                LiveDbSpSqlController.HeatServerType = DbSqlSpController.ServerType.ES;
                LiveDbSpSqlController.DataBaseName = "SX";
            }
            else
            {
                LiveDbSpSqlController.HeatServerType = DbSqlSpController.ServerType.UNKNOWN;
            }
            FormLauncher_Load(this, new EventArgs());
        }

        private void comboBoxSqlAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSqlAuthType.SelectedItem.ToString() == "SQL Authentication")
            {
                Forms.FormAlternateCredentials altCredsForm = new FormAlternateCredentials(LiveDbSpSqlController);
                DialogResult credsResult = altCredsForm.ShowDialog();
                if (credsResult == DialogResult.OK)
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected = true;
                    tbDbServerName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress;
                    tbDatabaseName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName;
                }
                else
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected = false;
                    comboBoxSqlAuthType.SelectedItem = "Windows Authentication";
                }
            }
            else
            {
                LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected = false;
            }
        }
    }
}
