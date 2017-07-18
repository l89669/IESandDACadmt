namespace IESandDACadmt.Forms
{
    partial class FormRecordDeletion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecordDeletion));
            this.lblPurgeCriteria = new System.Windows.Forms.Label();
            this.rbEveryone = new System.Windows.Forms.RadioButton();
            this.rbSelectUser = new System.Windows.Forms.RadioButton();
            this.rbAllComputers = new System.Windows.Forms.RadioButton();
            this.rbSelectComputer = new System.Windows.Forms.RadioButton();
            this.cbSpecificUser = new System.Windows.Forms.ComboBox();
            this.cbSpecificComputer = new System.Windows.Forms.ComboBox();
            this.dtpCutOffDate = new System.Windows.Forms.DateTimePicker();
            this.lblRunPeriodMinutes = new System.Windows.Forms.Label();
            this.btnStartCleanup = new System.Windows.Forms.Button();
            this.btnStopCleanup = new System.Windows.Forms.Button();
            this.lblRecordsToPurge = new System.Windows.Forms.Label();
            this.lblPercentRecordsPurged = new System.Windows.Forms.Label();
            this.RecordsLeftToPurgeTextBox = new System.Windows.Forms.TextBox();
            this.percentageRecordsProcessedTextBox = new System.Windows.Forms.TextBox();
            this.pCleanupCriteria = new System.Windows.Forms.Panel();
            this.gbTaskRunTime = new System.Windows.Forms.GroupBox();
            this.runtimeHours = new System.Windows.Forms.NumericUpDown();
            this.runtimeMinutes = new System.Windows.Forms.NumericUpDown();
            this.lblRunPeriodHours = new System.Windows.Forms.Label();
            this.gbBatchSize = new System.Windows.Forms.GroupBox();
            this.cbBatchSize = new System.Windows.Forms.ComboBox();
            this.gbDataRetentionLimit = new System.Windows.Forms.GroupBox();
            this.rbCutOffDate = new System.Windows.Forms.RadioButton();
            this.rbNoCutOffDate = new System.Windows.Forms.RadioButton();
            this.gbByProcess = new System.Windows.Forms.GroupBox();
            this.cbSpecificProcess = new System.Windows.Forms.ComboBox();
            this.rbSpecificProcess = new System.Windows.Forms.RadioButton();
            this.rbAllProcesses = new System.Windows.Forms.RadioButton();
            this.gbComputer = new System.Windows.Forms.GroupBox();
            this.gbUser = new System.Windows.Forms.GroupBox();
            this.pCleanupStats = new System.Windows.Forms.Panel();
            this.labelProgress = new System.Windows.Forms.Label();
            this.RecordsPurgedTextBox = new System.Windows.Forms.TextBox();
            this.lblRecordsPurged = new System.Windows.Forms.Label();
            this.remainingRunTimeMinutes = new System.Windows.Forms.TextBox();
            this.lblRemainingRunTime = new System.Windows.Forms.Label();
            this.processingStatsTimer = new System.Windows.Forms.Timer(this.components);
            this.pStartStopButtons = new System.Windows.Forms.Panel();
            this.dbConnectionTestTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readByProcessInfoTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerCalcTotRecToPurge = new System.Windows.Forms.Timer(this.components);
            this.toolTipDataRetentionGroup = new System.Windows.Forms.ToolTip(this.components);
            this.pCleanupCriteria.SuspendLayout();
            this.gbTaskRunTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeMinutes)).BeginInit();
            this.gbBatchSize.SuspendLayout();
            this.gbDataRetentionLimit.SuspendLayout();
            this.gbByProcess.SuspendLayout();
            this.gbComputer.SuspendLayout();
            this.gbUser.SuspendLayout();
            this.pCleanupStats.SuspendLayout();
            this.pStartStopButtons.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPurgeCriteria
            // 
            this.lblPurgeCriteria.AutoSize = true;
            this.lblPurgeCriteria.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPurgeCriteria.Location = new System.Drawing.Point(10, 7);
            this.lblPurgeCriteria.Name = "lblPurgeCriteria";
            this.lblPurgeCriteria.Size = new System.Drawing.Size(179, 17);
            this.lblPurgeCriteria.TabIndex = 18;
            this.lblPurgeCriteria.Text = "Select Event details to Delete:";
            // 
            // rbEveryone
            // 
            this.rbEveryone.AutoSize = true;
            this.rbEveryone.Location = new System.Drawing.Point(6, 14);
            this.rbEveryone.Name = "rbEveryone";
            this.rbEveryone.Size = new System.Drawing.Size(69, 17);
            this.rbEveryone.TabIndex = 6;
            this.rbEveryone.TabStop = true;
            this.rbEveryone.Text = "All Users";
            this.rbEveryone.UseVisualStyleBackColor = true;
            this.rbEveryone.CheckedChanged += new System.EventHandler(this.rbEveryone_CheckedChanged);
            // 
            // rbSelectUser
            // 
            this.rbSelectUser.AutoSize = true;
            this.rbSelectUser.Location = new System.Drawing.Point(406, 14);
            this.rbSelectUser.Name = "rbSelectUser";
            this.rbSelectUser.Size = new System.Drawing.Size(93, 17);
            this.rbSelectUser.TabIndex = 7;
            this.rbSelectUser.TabStop = true;
            this.rbSelectUser.Text = "Specific User:";
            this.rbSelectUser.UseVisualStyleBackColor = true;
            this.rbSelectUser.CheckedChanged += new System.EventHandler(this.rbSelectUser_CheckedChanged);
            // 
            // rbAllComputers
            // 
            this.rbAllComputers.AutoSize = true;
            this.rbAllComputers.Location = new System.Drawing.Point(6, 14);
            this.rbAllComputers.Name = "rbAllComputers";
            this.rbAllComputers.Size = new System.Drawing.Size(97, 17);
            this.rbAllComputers.TabIndex = 9;
            this.rbAllComputers.TabStop = true;
            this.rbAllComputers.Text = "All Computers";
            this.rbAllComputers.UseVisualStyleBackColor = true;
            this.rbAllComputers.CheckedChanged += new System.EventHandler(this.rbAllComputers_CheckedChanged);
            // 
            // rbSelectComputer
            // 
            this.rbSelectComputer.AutoSize = true;
            this.rbSelectComputer.Location = new System.Drawing.Point(406, 14);
            this.rbSelectComputer.Name = "rbSelectComputer";
            this.rbSelectComputer.Size = new System.Drawing.Size(121, 17);
            this.rbSelectComputer.TabIndex = 10;
            this.rbSelectComputer.TabStop = true;
            this.rbSelectComputer.Text = "Specific Computer:";
            this.rbSelectComputer.UseVisualStyleBackColor = true;
            this.rbSelectComputer.CheckedChanged += new System.EventHandler(this.rbSpecificComputer_CheckedChanged);
            // 
            // cbSpecificUser
            // 
            this.cbSpecificUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSpecificUser.FormattingEnabled = true;
            this.cbSpecificUser.Location = new System.Drawing.Point(6, 35);
            this.cbSpecificUser.Name = "cbSpecificUser";
            this.cbSpecificUser.Size = new System.Drawing.Size(517, 21);
            this.cbSpecificUser.TabIndex = 8;
            // 
            // cbSpecificComputer
            // 
            this.cbSpecificComputer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSpecificComputer.FormattingEnabled = true;
            this.cbSpecificComputer.Location = new System.Drawing.Point(6, 36);
            this.cbSpecificComputer.Name = "cbSpecificComputer";
            this.cbSpecificComputer.Size = new System.Drawing.Size(517, 21);
            this.cbSpecificComputer.TabIndex = 11;
            // 
            // dtpCutOffDate
            // 
            this.dtpCutOffDate.Location = new System.Drawing.Point(106, 19);
            this.dtpCutOffDate.Name = "dtpCutOffDate";
            this.dtpCutOffDate.Size = new System.Drawing.Size(201, 22);
            this.dtpCutOffDate.TabIndex = 12;
            this.dtpCutOffDate.ValueChanged += new System.EventHandler(this.dtpCutOffDate_ValueChanged);
            // 
            // lblRunPeriodMinutes
            // 
            this.lblRunPeriodMinutes.AutoSize = true;
            this.lblRunPeriodMinutes.Location = new System.Drawing.Point(198, 20);
            this.lblRunPeriodMinutes.Name = "lblRunPeriodMinutes";
            this.lblRunPeriodMinutes.Size = new System.Drawing.Size(31, 13);
            this.lblRunPeriodMinutes.TabIndex = 39;
            this.lblRunPeriodMinutes.Text = "mins";
            // 
            // btnStartCleanup
            // 
            this.btnStartCleanup.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCleanup.Location = new System.Drawing.Point(62, 23);
            this.btnStartCleanup.Name = "btnStartCleanup";
            this.btnStartCleanup.Size = new System.Drawing.Size(160, 41);
            this.btnStartCleanup.TabIndex = 16;
            this.btnStartCleanup.Text = "START DELETION";
            this.btnStartCleanup.UseVisualStyleBackColor = true;
            this.btnStartCleanup.Click += new System.EventHandler(this.btnStartCleanup_Click);
            // 
            // btnStopCleanup
            // 
            this.btnStopCleanup.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopCleanup.Location = new System.Drawing.Point(321, 23);
            this.btnStopCleanup.Name = "btnStopCleanup";
            this.btnStopCleanup.Size = new System.Drawing.Size(160, 41);
            this.btnStopCleanup.TabIndex = 17;
            this.btnStopCleanup.Text = "STOP DELETION";
            this.btnStopCleanup.UseVisualStyleBackColor = true;
            this.btnStopCleanup.Click += new System.EventHandler(this.btnStopCleanup_Click);
            // 
            // lblRecordsToPurge
            // 
            this.lblRecordsToPurge.AutoSize = true;
            this.lblRecordsToPurge.Location = new System.Drawing.Point(7, 53);
            this.lblRecordsToPurge.Name = "lblRecordsToPurge";
            this.lblRecordsToPurge.Size = new System.Drawing.Size(130, 13);
            this.lblRecordsToPurge.TabIndex = 42;
            this.lblRecordsToPurge.Text = "Total Records To Delete:";
            // 
            // lblPercentRecordsPurged
            // 
            this.lblPercentRecordsPurged.AutoSize = true;
            this.lblPercentRecordsPurged.Location = new System.Drawing.Point(286, 77);
            this.lblPercentRecordsPurged.Name = "lblPercentRecordsPurged";
            this.lblPercentRecordsPurged.Size = new System.Drawing.Size(117, 13);
            this.lblPercentRecordsPurged.TabIndex = 43;
            this.lblPercentRecordsPurged.Text = "% Records Processed:";
            // 
            // RecordsLeftToPurgeTextBox
            // 
            this.RecordsLeftToPurgeTextBox.Location = new System.Drawing.Point(140, 50);
            this.RecordsLeftToPurgeTextBox.Name = "RecordsLeftToPurgeTextBox";
            this.RecordsLeftToPurgeTextBox.ReadOnly = true;
            this.RecordsLeftToPurgeTextBox.Size = new System.Drawing.Size(115, 22);
            this.RecordsLeftToPurgeTextBox.TabIndex = 44;
            // 
            // percentageRecordsProcessedTextBox
            // 
            this.percentageRecordsProcessedTextBox.Location = new System.Drawing.Point(403, 74);
            this.percentageRecordsProcessedTextBox.Name = "percentageRecordsProcessedTextBox";
            this.percentageRecordsProcessedTextBox.ReadOnly = true;
            this.percentageRecordsProcessedTextBox.Size = new System.Drawing.Size(115, 22);
            this.percentageRecordsProcessedTextBox.TabIndex = 45;
            // 
            // pCleanupCriteria
            // 
            this.pCleanupCriteria.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pCleanupCriteria.Controls.Add(this.gbTaskRunTime);
            this.pCleanupCriteria.Controls.Add(this.gbBatchSize);
            this.pCleanupCriteria.Controls.Add(this.gbDataRetentionLimit);
            this.pCleanupCriteria.Controls.Add(this.gbByProcess);
            this.pCleanupCriteria.Controls.Add(this.gbComputer);
            this.pCleanupCriteria.Controls.Add(this.gbUser);
            this.pCleanupCriteria.Controls.Add(this.lblPurgeCriteria);
            this.pCleanupCriteria.Location = new System.Drawing.Point(1, 25);
            this.pCleanupCriteria.Name = "pCleanupCriteria";
            this.pCleanupCriteria.Size = new System.Drawing.Size(550, 388);
            this.pCleanupCriteria.TabIndex = 47;
            // 
            // gbTaskRunTime
            // 
            this.gbTaskRunTime.Controls.Add(this.runtimeHours);
            this.gbTaskRunTime.Controls.Add(this.lblRunPeriodMinutes);
            this.gbTaskRunTime.Controls.Add(this.runtimeMinutes);
            this.gbTaskRunTime.Controls.Add(this.lblRunPeriodHours);
            this.gbTaskRunTime.Location = new System.Drawing.Point(294, 328);
            this.gbTaskRunTime.Name = "gbTaskRunTime";
            this.gbTaskRunTime.Size = new System.Drawing.Size(245, 47);
            this.gbTaskRunTime.TabIndex = 55;
            this.gbTaskRunTime.TabStop = false;
            this.gbTaskRunTime.Text = "Deletion Run Time";
            // 
            // runtimeHours
            // 
            this.runtimeHours.Location = new System.Drawing.Point(23, 18);
            this.runtimeHours.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.runtimeHours.Name = "runtimeHours";
            this.runtimeHours.Size = new System.Drawing.Size(55, 22);
            this.runtimeHours.TabIndex = 14;
            this.runtimeHours.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.runtimeHours.ValueChanged += new System.EventHandler(this.runtimeHours_ValueChanged);
            this.runtimeHours.Enter += new System.EventHandler(this.runtimeHours_Enter);
            this.runtimeHours.Leave += new System.EventHandler(this.runtimeHours_Leave);
            // 
            // runtimeMinutes
            // 
            this.runtimeMinutes.Location = new System.Drawing.Point(135, 18);
            this.runtimeMinutes.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.runtimeMinutes.Name = "runtimeMinutes";
            this.runtimeMinutes.Size = new System.Drawing.Size(57, 22);
            this.runtimeMinutes.TabIndex = 15;
            this.runtimeMinutes.ValueChanged += new System.EventHandler(this.runtimeMinutes_ValueChanged);
            this.runtimeMinutes.Enter += new System.EventHandler(this.runtimeMinutes_Enter);
            this.runtimeMinutes.Leave += new System.EventHandler(this.runtimeMinutes_Leave);
            // 
            // lblRunPeriodHours
            // 
            this.lblRunPeriodHours.AutoSize = true;
            this.lblRunPeriodHours.Location = new System.Drawing.Point(84, 20);
            this.lblRunPeriodHours.Name = "lblRunPeriodHours";
            this.lblRunPeriodHours.Size = new System.Drawing.Size(30, 13);
            this.lblRunPeriodHours.TabIndex = 51;
            this.lblRunPeriodHours.Text = "Hr(s)";
            // 
            // gbBatchSize
            // 
            this.gbBatchSize.Controls.Add(this.cbBatchSize);
            this.gbBatchSize.Location = new System.Drawing.Point(10, 328);
            this.gbBatchSize.Name = "gbBatchSize";
            this.gbBatchSize.Size = new System.Drawing.Size(212, 47);
            this.gbBatchSize.TabIndex = 54;
            this.gbBatchSize.TabStop = false;
            this.gbBatchSize.Text = "Batch size";
            // 
            // cbBatchSize
            // 
            this.cbBatchSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBatchSize.FormattingEnabled = true;
            this.cbBatchSize.Items.AddRange(new object[] {
            "1,000",
            "2,000",
            "4,000",
            "8,000",
            "16,000",
            "32,000",
            "64,000",
            "128,000"});
            this.cbBatchSize.Location = new System.Drawing.Point(31, 16);
            this.cbBatchSize.Name = "cbBatchSize";
            this.cbBatchSize.Size = new System.Drawing.Size(150, 21);
            this.cbBatchSize.TabIndex = 13;
            this.cbBatchSize.SelectedIndexChanged += new System.EventHandler(this.cbBatchSize_SelectedIndexChanged);
            // 
            // gbDataRetentionLimit
            // 
            this.gbDataRetentionLimit.Controls.Add(this.rbCutOffDate);
            this.gbDataRetentionLimit.Controls.Add(this.rbNoCutOffDate);
            this.gbDataRetentionLimit.Controls.Add(this.dtpCutOffDate);
            this.gbDataRetentionLimit.Location = new System.Drawing.Point(10, 260);
            this.gbDataRetentionLimit.Name = "gbDataRetentionLimit";
            this.gbDataRetentionLimit.Size = new System.Drawing.Size(529, 50);
            this.gbDataRetentionLimit.TabIndex = 53;
            this.gbDataRetentionLimit.TabStop = false;
            this.gbDataRetentionLimit.Text = "Data Retention Criteria:";
            this.toolTipDataRetentionGroup.SetToolTip(this.gbDataRetentionLimit, "The Cut-off Date is the point in time where events older than that date are subje" +
        "ct to deletion");
            // 
            // rbCutOffDate
            // 
            this.rbCutOffDate.AutoSize = true;
            this.rbCutOffDate.Location = new System.Drawing.Point(6, 19);
            this.rbCutOffDate.Name = "rbCutOffDate";
            this.rbCutOffDate.Size = new System.Drawing.Size(94, 17);
            this.rbCutOffDate.TabIndex = 38;
            this.rbCutOffDate.TabStop = true;
            this.rbCutOffDate.Text = "Cut-Off Date:";
            this.rbCutOffDate.UseVisualStyleBackColor = true;
            this.rbCutOffDate.CheckedChanged += new System.EventHandler(this.rbCutOffDate_CheckedChanged);
            // 
            // rbNoCutOffDate
            // 
            this.rbNoCutOffDate.AutoSize = true;
            this.rbNoCutOffDate.Location = new System.Drawing.Point(406, 19);
            this.rbNoCutOffDate.Name = "rbNoCutOffDate";
            this.rbNoCutOffDate.Size = new System.Drawing.Size(109, 17);
            this.rbNoCutOffDate.TabIndex = 37;
            this.rbNoCutOffDate.TabStop = true;
            this.rbNoCutOffDate.Text = "No Cut-Off Date";
            this.rbNoCutOffDate.UseVisualStyleBackColor = true;
            this.rbNoCutOffDate.CheckedChanged += new System.EventHandler(this.rbNoCutOffDate_CheckedChanged);
            // 
            // gbByProcess
            // 
            this.gbByProcess.Controls.Add(this.cbSpecificProcess);
            this.gbByProcess.Controls.Add(this.rbSpecificProcess);
            this.gbByProcess.Controls.Add(this.rbAllProcesses);
            this.gbByProcess.Location = new System.Drawing.Point(10, 184);
            this.gbByProcess.Name = "gbByProcess";
            this.gbByProcess.Size = new System.Drawing.Size(529, 64);
            this.gbByProcess.TabIndex = 52;
            this.gbByProcess.TabStop = false;
            this.gbByProcess.Text = "Process Criteria";
            // 
            // cbSpecificProcess
            // 
            this.cbSpecificProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSpecificProcess.FormattingEnabled = true;
            this.cbSpecificProcess.Location = new System.Drawing.Point(6, 37);
            this.cbSpecificProcess.Name = "cbSpecificProcess";
            this.cbSpecificProcess.Size = new System.Drawing.Size(517, 21);
            this.cbSpecificProcess.TabIndex = 2;
            // 
            // rbSpecificProcess
            // 
            this.rbSpecificProcess.AutoSize = true;
            this.rbSpecificProcess.Location = new System.Drawing.Point(406, 14);
            this.rbSpecificProcess.Name = "rbSpecificProcess";
            this.rbSpecificProcess.Size = new System.Drawing.Size(108, 17);
            this.rbSpecificProcess.TabIndex = 1;
            this.rbSpecificProcess.TabStop = true;
            this.rbSpecificProcess.Text = "Specific Process:";
            this.rbSpecificProcess.UseVisualStyleBackColor = true;
            this.rbSpecificProcess.CheckedChanged += new System.EventHandler(this.rbSpecificProcess_CheckedChanged);
            // 
            // rbAllProcesses
            // 
            this.rbAllProcesses.AutoSize = true;
            this.rbAllProcesses.Location = new System.Drawing.Point(6, 14);
            this.rbAllProcesses.Name = "rbAllProcesses";
            this.rbAllProcesses.Size = new System.Drawing.Size(90, 17);
            this.rbAllProcesses.TabIndex = 0;
            this.rbAllProcesses.TabStop = true;
            this.rbAllProcesses.Text = "All Processes";
            this.rbAllProcesses.UseVisualStyleBackColor = true;
            this.rbAllProcesses.CheckedChanged += new System.EventHandler(this.rbAllProcesses_CheckedChanged);
            // 
            // gbComputer
            // 
            this.gbComputer.Controls.Add(this.cbSpecificComputer);
            this.gbComputer.Controls.Add(this.rbSelectComputer);
            this.gbComputer.Controls.Add(this.rbAllComputers);
            this.gbComputer.Location = new System.Drawing.Point(10, 108);
            this.gbComputer.Name = "gbComputer";
            this.gbComputer.Size = new System.Drawing.Size(529, 64);
            this.gbComputer.TabIndex = 43;
            this.gbComputer.TabStop = false;
            this.gbComputer.Text = "Computer Criteria:";
            // 
            // gbUser
            // 
            this.gbUser.Controls.Add(this.cbSpecificUser);
            this.gbUser.Controls.Add(this.rbSelectUser);
            this.gbUser.Controls.Add(this.rbEveryone);
            this.gbUser.Location = new System.Drawing.Point(10, 36);
            this.gbUser.Name = "gbUser";
            this.gbUser.Size = new System.Drawing.Size(529, 64);
            this.gbUser.TabIndex = 42;
            this.gbUser.TabStop = false;
            this.gbUser.Text = "User Criteria:";
            // 
            // pCleanupStats
            // 
            this.pCleanupStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pCleanupStats.Controls.Add(this.labelProgress);
            this.pCleanupStats.Controls.Add(this.RecordsPurgedTextBox);
            this.pCleanupStats.Controls.Add(this.lblRecordsPurged);
            this.pCleanupStats.Controls.Add(this.remainingRunTimeMinutes);
            this.pCleanupStats.Controls.Add(this.lblRemainingRunTime);
            this.pCleanupStats.Controls.Add(this.percentageRecordsProcessedTextBox);
            this.pCleanupStats.Controls.Add(this.RecordsLeftToPurgeTextBox);
            this.pCleanupStats.Controls.Add(this.lblPercentRecordsPurged);
            this.pCleanupStats.Controls.Add(this.lblRecordsToPurge);
            this.pCleanupStats.Location = new System.Drawing.Point(1, 501);
            this.pCleanupStats.Name = "pCleanupStats";
            this.pCleanupStats.Size = new System.Drawing.Size(550, 116);
            this.pCleanupStats.TabIndex = 48;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgress.Location = new System.Drawing.Point(10, 15);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(115, 17);
            this.labelProgress.TabIndex = 54;
            this.labelProgress.Text = "Deletion Progress:";
            // 
            // RecordsPurgedTextBox
            // 
            this.RecordsPurgedTextBox.Location = new System.Drawing.Point(140, 74);
            this.RecordsPurgedTextBox.Name = "RecordsPurgedTextBox";
            this.RecordsPurgedTextBox.ReadOnly = true;
            this.RecordsPurgedTextBox.Size = new System.Drawing.Size(115, 22);
            this.RecordsPurgedTextBox.TabIndex = 53;
            // 
            // lblRecordsPurged
            // 
            this.lblRecordsPurged.AutoSize = true;
            this.lblRecordsPurged.Location = new System.Drawing.Point(9, 77);
            this.lblRecordsPurged.Name = "lblRecordsPurged";
            this.lblRecordsPurged.Size = new System.Drawing.Size(94, 13);
            this.lblRecordsPurged.TabIndex = 52;
            this.lblRecordsPurged.Text = "Records Deleted:";
            // 
            // remainingRunTimeMinutes
            // 
            this.remainingRunTimeMinutes.Location = new System.Drawing.Point(403, 50);
            this.remainingRunTimeMinutes.Name = "remainingRunTimeMinutes";
            this.remainingRunTimeMinutes.ReadOnly = true;
            this.remainingRunTimeMinutes.Size = new System.Drawing.Size(115, 22);
            this.remainingRunTimeMinutes.TabIndex = 48;
            // 
            // lblRemainingRunTime
            // 
            this.lblRemainingRunTime.AutoSize = true;
            this.lblRemainingRunTime.Location = new System.Drawing.Point(286, 53);
            this.lblRemainingRunTime.Name = "lblRemainingRunTime";
            this.lblRemainingRunTime.Size = new System.Drawing.Size(115, 13);
            this.lblRemainingRunTime.TabIndex = 47;
            this.lblRemainingRunTime.Text = "Remaining Run-time:";
            // 
            // processingStatsTimer
            // 
            this.processingStatsTimer.Interval = 333;
            this.processingStatsTimer.Tick += new System.EventHandler(this.processingStatsTimer_Tick);
            // 
            // pStartStopButtons
            // 
            this.pStartStopButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pStartStopButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pStartStopButtons.Controls.Add(this.btnStartCleanup);
            this.pStartStopButtons.Controls.Add(this.btnStopCleanup);
            this.pStartStopButtons.Location = new System.Drawing.Point(1, 412);
            this.pStartStopButtons.Name = "pStartStopButtons";
            this.pStartStopButtons.Size = new System.Drawing.Size(550, 90);
            this.pStartStopButtons.TabIndex = 50;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(552, 24);
            this.menuStrip1.TabIndex = 71;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eventTypesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // eventTypesToolStripMenuItem
            // 
            this.eventTypesToolStripMenuItem.Name = "eventTypesToolStripMenuItem";
            this.eventTypesToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.eventTypesToolStripMenuItem.Text = "&Event Types";
            this.eventTypesToolStripMenuItem.Click += new System.EventHandler(this.eventTypesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogFileToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // viewLogFileToolStripMenuItem
            // 
            this.viewLogFileToolStripMenuItem.Name = "viewLogFileToolStripMenuItem";
            this.viewLogFileToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.viewLogFileToolStripMenuItem.Text = "&View Log File";
            this.viewLogFileToolStripMenuItem.Click += new System.EventHandler(this.viewLogFileToolStripMenuItem_Click);
            // 
            // readByProcessInfoTimer
            // 
            this.readByProcessInfoTimer.Tick += new System.EventHandler(this.readByProcessInfoTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 616);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(552, 22);
            this.statusStrip1.TabIndex = 72;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // timerCalcTotRecToPurge
            // 
            this.timerCalcTotRecToPurge.Tick += new System.EventHandler(this.timerCalcTotRecToPurge_Tick);
            // 
            // toolTipDataRetentionGroup
            // 
            this.toolTipDataRetentionGroup.AutoPopDelay = 7000;
            this.toolTipDataRetentionGroup.InitialDelay = 500;
            this.toolTipDataRetentionGroup.ReshowDelay = 500;
            this.toolTipDataRetentionGroup.ShowAlways = true;
            // 
            // FormRecordDeletion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 638);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pStartStopButtons);
            this.Controls.Add(this.pCleanupStats);
            this.Controls.Add(this.pCleanupCriteria);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(568, 676);
            this.MinimumSize = new System.Drawing.Size(568, 676);
            this.Name = "FormRecordDeletion";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pCleanupCriteria.ResumeLayout(false);
            this.pCleanupCriteria.PerformLayout();
            this.gbTaskRunTime.ResumeLayout(false);
            this.gbTaskRunTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.runtimeMinutes)).EndInit();
            this.gbBatchSize.ResumeLayout(false);
            this.gbDataRetentionLimit.ResumeLayout(false);
            this.gbDataRetentionLimit.PerformLayout();
            this.gbByProcess.ResumeLayout(false);
            this.gbByProcess.PerformLayout();
            this.gbComputer.ResumeLayout(false);
            this.gbComputer.PerformLayout();
            this.gbUser.ResumeLayout(false);
            this.gbUser.PerformLayout();
            this.pCleanupStats.ResumeLayout(false);
            this.pCleanupStats.PerformLayout();
            this.pStartStopButtons.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblPurgeCriteria;
        private System.Windows.Forms.RadioButton rbEveryone;
        private System.Windows.Forms.RadioButton rbSelectUser;
        private System.Windows.Forms.RadioButton rbAllComputers;
        private System.Windows.Forms.RadioButton rbSelectComputer;
        private System.Windows.Forms.ComboBox cbSpecificUser;
        private System.Windows.Forms.ComboBox cbSpecificComputer;
        private System.Windows.Forms.DateTimePicker dtpCutOffDate;
        private System.Windows.Forms.Label lblRunPeriodMinutes;
        private System.Windows.Forms.Button btnStartCleanup;
        private System.Windows.Forms.Button btnStopCleanup;
        private System.Windows.Forms.Label lblRecordsToPurge;
        private System.Windows.Forms.Label lblPercentRecordsPurged;
        private System.Windows.Forms.TextBox RecordsLeftToPurgeTextBox;
        private System.Windows.Forms.TextBox percentageRecordsProcessedTextBox;
        private System.Windows.Forms.Panel pCleanupCriteria;
        private System.Windows.Forms.Panel pCleanupStats;
        private System.Windows.Forms.GroupBox gbUser;
        private System.Windows.Forms.GroupBox gbComputer;
        private System.Windows.Forms.ComboBox cbBatchSize;
        private System.Windows.Forms.TextBox remainingRunTimeMinutes;
        private System.Windows.Forms.Label lblRemainingRunTime;
        private System.Windows.Forms.Timer processingStatsTimer;
        private System.Windows.Forms.TextBox RecordsPurgedTextBox;
        private System.Windows.Forms.Label lblRecordsPurged;
        private System.Windows.Forms.NumericUpDown runtimeHours;
        private System.Windows.Forms.Label lblRunPeriodHours;
        private System.Windows.Forms.NumericUpDown runtimeMinutes;
        private System.Windows.Forms.Panel pStartStopButtons;
        private System.Windows.Forms.Timer dbConnectionTestTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbByProcess;
        private System.Windows.Forms.RadioButton rbSpecificProcess;
        private System.Windows.Forms.RadioButton rbAllProcesses;
        private System.Windows.Forms.ComboBox cbSpecificProcess;
        private System.Windows.Forms.Timer readByProcessInfoTimer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timerCalcTotRecToPurge;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.GroupBox gbTaskRunTime;
        private System.Windows.Forms.GroupBox gbBatchSize;
        private System.Windows.Forms.GroupBox gbDataRetentionLimit;
        private System.Windows.Forms.RadioButton rbCutOffDate;
        private System.Windows.Forms.RadioButton rbNoCutOffDate;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eventTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTipDataRetentionGroup;
    }
}

