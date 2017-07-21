using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using IESandDACadmt.Model.Logging;
using IESandDACadmt.Model.Sql;
using IESandDACadmt.ViewModel;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;

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
            dbConnectionTestTimer = new System.Windows.Threading.DispatcherTimer();
            dbConnectionTestTimer.Tick += dbConnectionTestTimer_Tick;
            LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = Model.ServerDetectionLogic.CheckServerType(LiveDbSpSqlController, _serverDetectionData);
            ListboxServerType.Items.Add("EMSS");
            ListboxServerType.Items.Add("ES");
            ListboxServerType.SelectedItem = "EMSS";
            ListBoxSqlAuthType.Items.Add("Windows Authentication");
            ListBoxSqlAuthType.Items.Add("SQL Authentication");
            ListBoxSqlAuthType.SelectedItem = "Windows Authentication";
        }

        public volatile Model.DbSqlSpController LiveDbSpSqlController = new Model.DbSqlSpController();
        Model.Sql.SqlTestDbConnection _workerTestSql = null;
        Thread _testDbConnectionThread = null;
        WpfRecordsProfiler _dataProfilerPage = null;
        WpfRecordDeletion _recordPurgePage = null;
        ViewModel.ServerDetectionData _serverDetectionData = new ViewModel.ServerDetectionData();
        System.Windows.Threading.DispatcherTimer dbConnectionTestTimer = null;


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
                ListboxServerType.SelectedItem = LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType.ToString();
                ModifyGuiOnKnownServerType();
                if (LiveDbSpSqlController.DbSqlSpControllerData.SqlConnectionStringFound)
                {
                    SetLauncherGui(LiveDbSpSqlController);
                    MessageBoxResult okToUseFoundString = MessageBox.Show("Discovered Server Name " + LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress + ", do you want to use this?",
                                                                        "Discovered server details",
                                                                        MessageBoxButton.YesNo,
                                                                        MessageBoxImage.Question);
                    if (okToUseFoundString == MessageBoxResult.Yes)
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
            SetServerTypeRowTo(true);
            SetSqlConnectionTestRowTo(false);
            SetToolsRowTo(false);
            ToolBarLabel.Text = "Unknown Server Type";
            

            //panelServerType.BackColor = SystemColors.ControlLightLight;
            //panelDbConnection.BackColor = SystemColors.Control;
            //panelProfiler.BackColor = SystemColors.Control;
            //panelPurge.BackColor = SystemColors.Control;
            //panelHealth.BackColor = SystemColors.Control;
        }

        private void SetToolsRowTo(bool v)
        {
            ServerTypeGrid.IsEnabled = v;
            SetGridRowColoutToActive(v);

            //ButtonChangeType.IsEnabled = v;
            //ListboxServerType.IsEnabled = v;
        }

        private void SetGridRowColoutToActive(bool v)
        {
            if (v)
            {
                ServerTypeGrid.Background = new SolidColorBrush(Color.FromArgb(1, 240, 240, 240)); // Light grey
            }
            else
            {
                ServerTypeGrid.Background = new SolidColorBrush(Color.FromArgb(1, 210, 210, 210)); // Dark grey
            }
        }

        private void SetSqlConnectionTestRowTo(bool v)
        {
            SqlConnectionTestGrid.IsEnabled = v;
            //ListBoxSqlAuthType.IsEnabled = v;
            //tbDatabaseName.IsEnabled = v;
            //btnTestDBConnection.IsEnabled = v;
            //tbDbServerName.IsEnabled = v;
            //btnChangeSqlServer.IsEnabled = v;
        }

        private void SetServerTypeRowTo(bool v)
        {
            LaunchButtonsGrid.IsEnabled = v;
            //btnLaunchDeletion.IsEnabled = v;
            //btnLaunchHealth.IsEnabled = v;
            //btnLaunchProfiler.IsEnabled = v;
        }

        private void ModifyGuiOnKnownServerType()
        {
            SetServerTypeRowTo(true);
            SetSqlConnectionTestRowTo(true);
            SetToolsRowTo(false);
            //panelServerType.Enabled = true;
            //panelDbConnection.Enabled = true;
            //panelProfiler.Enabled = false;
            //panelPurge.Enabled = false;
            //panelHealth.Enabled = false;
            ToolBarLabel.Text = " Server-Type:" + LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType.ToString();

            //panelServerType.BackColor = SystemColors.Control;
            //panelDbConnection.BackColor = SystemColors.ControlLightLight;
            //panelProfiler.BackColor = SystemColors.Control;
            //panelPurge.BackColor = SystemColors.Control;
            //panelHealth.BackColor = SystemColors.Control;

            switch (LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType)
            {
                case DbSqlSpControllerData.ServerType.UNKNOWN:
                    tbDatabaseName.IsEnabled = true;
                    break;
                case DbSqlSpControllerData.ServerType.EMSS:
                    tbDatabaseName.IsEnabled = false;
                    break;
                case DbSqlSpControllerData.ServerType.ES:
                    tbDatabaseName.IsEnabled = true;
                    break;
                default:
                    tbDatabaseName.IsEnabled = true;
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
                ToolBarLabel.Text = "Testing Connection...";
                LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " Test DB Connection button was clicked.");
                SetSqlSearchCommandsByServerType();
                if (LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected == false)
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.SqlConnUserName = WindowsIdentity.GetCurrent().Name;
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
                MessageBox.Show("SQL Server name and/or Database name is invalid. Please enter them again.", "Invalid Server Details", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, "SQL Server name and/or Database name is invalid.");
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
            dbConnectionTestTimer.IsEnabled = true;
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
                LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " " + ex.Message.ToString());
            }
        }

        private void btnChangeSqlServer_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " CHANGE SQL SERVER button was clicked.");
            dbConnectionTestTimer.IsEnabled = false;
            ToolBarLabel.Text = "Connection attempt stopped";
            ModifyGuiOnFormLoad();
            ToolBarProgressBar.Value = 0;
            ViewModel.DbSqlSpControllerData.ServerType tempServerType = LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType;
            Dictionary<string, bool> tempo = LiveDbSpSqlController.DbSqlSpControllerData.EventTypesToDelete;
            LiveDbSpSqlController = new Model.DbSqlSpController();
            LiveDbSpSqlController.DbSqlSpControllerData.EventTypesToDelete = tempo;
            LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = tempServerType;
        }

        private void ModifyGuiOnFormLoad()
        {
            SetServerTypeRowTo(true);
            SetSqlConnectionTestRowTo(true);
            SetToolsRowTo(false);

            //btnChangeSqlServer.IsEnabled = false;
            //btnTestDBConnection.IsEnabled = true;
            //tbDbServerName.IsEnabled = true;
            //tbDatabaseName.IsEnabled = true;

            //panelHealth.Enabled = false;
            //panelProfiler.Enabled = false;
            //panelPurge.Enabled = false;

            //panelDbConnection.BackColor = SystemColors.ControlLightLight;
            //panelProfiler.BackColor = SystemColors.Control;
            //panelPurge.BackColor = SystemColors.Control;
            //panelHealth.BackColor = SystemColors.Control;
        }

        private void ModifyGuiOnTestButtonClick()
        {
            
            btnChangeSqlServer.IsEnabled = true;
            btnTestDBConnection.IsEnabled = false;
            tbDbServerName.IsEnabled = false;
            tbDatabaseName.IsEnabled = false;

            SetServerTypeRowTo(false);
            SetToolsRowTo(false);
            //panelHealth.Enabled = false;
            //panelProfiler.Enabled = false;
            //panelPurge.Enabled = false;

            //panelDbConnection.BackColor = SystemColors.Control;
            //panelProfiler.BackColor = SystemColors.Control;
            //panelPurge.BackColor = SystemColors.Control;
            //panelHealth.BackColor = SystemColors.Control;
        }

        private void ModifyGuiOnDbTestSuccess()
        {
            btnChangeSqlServer.IsEnabled = true;
            btnTestDBConnection.IsEnabled = false;
            tbDbServerName.IsEnabled = false;
            tbDatabaseName.IsEnabled = false;

            SetServerTypeRowTo(false);
            SetSqlConnectionTestRowTo(true);
            SetToolsRowTo(true);
            
            //panelHealth.Enabled = true;
            //panelProfiler.Enabled = true;
            //panelPurge.Enabled = true;

            //panelDbConnection.BackColor = SystemColors.Control;
            //panelProfiler.BackColor = SystemColors.ControlLightLight;
            //panelPurge.BackColor = SystemColors.ControlLightLight;
            //panelHealth.BackColor = SystemColors.ControlLightLight;
        }

        private void ModifyGuiOnDbTestFail()
        {
            btnChangeSqlServer.IsEnabled = false;
            btnTestDBConnection.IsEnabled = true;
            tbDbServerName.IsEnabled = true;
            tbDatabaseName.IsEnabled = true;

            SetServerTypeRowTo(false);
            SetSqlConnectionTestRowTo(true);
            SetToolsRowTo(false);

            //panelHealth.Enabled = false;
            //panelProfiler.Enabled = false;
            //panelPurge.Enabled = false;

            //panelDbConnection.BackColor = SystemColors.ControlLightLight;
            //panelProfiler.BackColor = SystemColors.Control;
            //panelPurge.BackColor = SystemColors.Control;
            //panelHealth.BackColor = SystemColors.Control;
        }

        private void buttonLaunchProfiler_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " SQL AC/DC Record Profiler Tool launched.");
            _dataProfilerPage = new WpfRecordsProfiler(LiveDbSpSqlController);
            _dataProfilerPage.ShowDialog();
        }

        private void buttonLaunchPurger_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " SQL AC/DC Record Purge Tool launched.");
            _recordPurgePage = new WpfRecordDeletion(LiveDbSpSqlController);
            _recordPurgePage.ShowDialog();
        }

        private void buttonLaunchHealthReview_Click(object sender, EventArgs e)
        {
            LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " SQL Health Review Tool launched.");
            WpfHealthReview healthReviewForm = new WpfHealthReview(LiveDbSpSqlController);
            healthReviewForm.ShowDialog();
        }

        private void dbConnectionTestTimer_Tick(object sender, EventArgs e)
        {
            if (LiveDbSpSqlController.DbSqlSpControllerData.DbTestStillRunning)
            {
                int curProgBarValue = Convert.ToInt32(ToolBarProgressBar.Value);
                if (curProgBarValue <= 100)
                {
                    curProgBarValue = curProgBarValue + 10;
                    if (curProgBarValue >= 100)
                    {
                        curProgBarValue = 0;
                    }
                }
                ToolBarProgressBar.Value = curProgBarValue;
            }
            else
            {
                dbConnectionTestTimer.IsEnabled = false;
                if (LiveDbSpSqlController.DbSqlSpControllerData.OperationResult != "success")
                {
                    LoggingClass.SaveErrorToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, LiveDbSpSqlController.DbSqlSpControllerData.OperationResult.ToString());
                    MessageBox.Show("Error connecting to " + tbDbServerName.Text + ". Please check the Log File for further details.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ToolBarLabel.Text = "Connection Failed";
                    ModifyGuiOnFormLoad();
                    ToolBarProgressBar.Value = 0;
                }
                else
                {
                    LoggingClass.SaveEventToLogFile(LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation, " The Database test succeeded.");
                    ToolBarLabel.Text = "Connected to:" + LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress + " User:" + LiveDbSpSqlController.DbSqlSpControllerData.SqlConnUserName;
                    ModifyGuiOnDbTestSuccess();
                    ToolBarProgressBar.Value = 100;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _testDbConnectionThread = null;
            _workerTestSql = null;
            _dataProfilerPage = null;
            _recordPurgePage = null;
            System.Windows.Application.Current.Shutdown();
        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", LiveDbSpSqlController.DbSqlSpControllerData.LogFileLocation);
        }

        private void requirementsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IESandDACadmt.View.WpfHelpRequirements helpRequirements = new IESandDACadmt.View.WpfHelpRequirements();
            helpRequirements.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IESandDACadmt.View.WpfAbout about = new IESandDACadmt.View.WpfAbout();
            about.Show();
        }


        private void buttonChangeServerType_Click(object sender, EventArgs e)
        {
            btnChangeSqlServer_Click(this, new EventArgs());
            string userSelectedServerType = ListboxServerType.Items[ListboxServerType.SelectedIndex].ToString();
            if (userSelectedServerType == "EMSS")
            {
                LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = DbSqlSpControllerData.ServerType.EMSS;
                LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName = "UPCCommon";
            }
            else if (userSelectedServerType == "ES")
            {
                LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = DbSqlSpControllerData.ServerType.ES;
                LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName = "SX";
            }
            else
            {
                LiveDbSpSqlController.DbSqlSpControllerData.HeatServerType = DbSqlSpControllerData.ServerType.UNKNOWN;
            }
            FormLauncher_Load(this, new EventArgs());
        }

        private void comboBoxSqlAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBoxSqlAuthType.SelectedItem.ToString() == "SQL Authentication")
            {
                View.WpfAlternateCredentials altCredsForm = new WpfAlternateCredentials(LiveDbSpSqlController);
                bool credsResult = Convert.ToBoolean(altCredsForm.ShowDialog());
                if (credsResult == true)
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected = true;
                    tbDbServerName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DbServeraddress;
                    tbDatabaseName.Text = LiveDbSpSqlController.DbSqlSpControllerData.DataBaseName;
                }
                else
                {
                    LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected = false;
                    ListBoxSqlAuthType.SelectedItem = "Windows Authentication";
                }
            }
            else
            {
                LiveDbSpSqlController.DbSqlSpControllerData.AltCredentialsSelected = false;
            }
        }
    }
}
