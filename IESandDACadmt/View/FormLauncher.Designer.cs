namespace Lumension_Advanced_DB_Maintenance.Forms
{
    partial class FormLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLauncher));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requirementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelProfiler = new System.Windows.Forms.Panel();
            this.buttonLaunchProfiler = new System.Windows.Forms.Button();
            this.panelPurge = new System.Windows.Forms.Panel();
            this.buttonLaunchPurger = new System.Windows.Forms.Button();
            this.panelHealth = new System.Windows.Forms.Panel();
            this.buttonLaunchHealthReview = new System.Windows.Forms.Button();
            this.dbConnectionTestTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelLauncher = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbDbServerName = new System.Windows.Forms.TextBox();
            this.btnTestDBConnection = new System.Windows.Forms.Button();
            this.lblSqlServerAddress = new System.Windows.Forms.Label();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.tbDatabaseName = new System.Windows.Forms.TextBox();
            this.btnChangeSqlServer = new System.Windows.Forms.Button();
            this.labelSqlServerTesting = new System.Windows.Forms.Label();
            this.panelDbConnection = new System.Windows.Forms.Panel();
            this.labelSqlAuthType = new System.Windows.Forms.Label();
            this.comboBoxSqlAuthType = new System.Windows.Forms.ComboBox();
            this.panelServerType = new System.Windows.Forms.Panel();
            this.buttonChangeServerType = new System.Windows.Forms.Button();
            this.comboBoxServerType = new System.Windows.Forms.ComboBox();
            this.labelHeatServerType = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panelProfiler.SuspendLayout();
            this.panelPurge.SuspendLayout();
            this.panelHealth.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelDbConnection.SuspendLayout();
            this.panelServerType.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(548, 24);
            this.menuStrip1.TabIndex = 72;
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
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.requirementsToolStripMenuItem,
            this.viewLogFileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // requirementsToolStripMenuItem
            // 
            this.requirementsToolStripMenuItem.Name = "requirementsToolStripMenuItem";
            this.requirementsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.requirementsToolStripMenuItem.Text = "&Requirements";
            this.requirementsToolStripMenuItem.Click += new System.EventHandler(this.requirementsToolStripMenuItem_Click);
            // 
            // viewLogFileToolStripMenuItem
            // 
            this.viewLogFileToolStripMenuItem.Name = "viewLogFileToolStripMenuItem";
            this.viewLogFileToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.viewLogFileToolStripMenuItem.Text = "&View Log File";
            this.viewLogFileToolStripMenuItem.Click += new System.EventHandler(this.viewLogFileToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panelProfiler
            // 
            this.panelProfiler.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProfiler.Controls.Add(this.buttonLaunchProfiler);
            this.panelProfiler.Location = new System.Drawing.Point(0, 194);
            this.panelProfiler.Name = "panelProfiler";
            this.panelProfiler.Size = new System.Drawing.Size(184, 100);
            this.panelProfiler.TabIndex = 73;
            // 
            // buttonLaunchProfiler
            // 
            this.buttonLaunchProfiler.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLaunchProfiler.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLaunchProfiler.Location = new System.Drawing.Point(30, 29);
            this.buttonLaunchProfiler.Name = "buttonLaunchProfiler";
            this.buttonLaunchProfiler.Size = new System.Drawing.Size(120, 38);
            this.buttonLaunchProfiler.TabIndex = 8;
            this.buttonLaunchProfiler.Text = "LAUNCH PROFILE TOOL";
            this.buttonLaunchProfiler.UseVisualStyleBackColor = false;
            this.buttonLaunchProfiler.Click += new System.EventHandler(this.buttonLaunchProfiler_Click);
            // 
            // panelPurge
            // 
            this.panelPurge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPurge.Controls.Add(this.buttonLaunchPurger);
            this.panelPurge.Location = new System.Drawing.Point(183, 194);
            this.panelPurge.Name = "panelPurge";
            this.panelPurge.Size = new System.Drawing.Size(184, 100);
            this.panelPurge.TabIndex = 74;
            // 
            // buttonLaunchPurger
            // 
            this.buttonLaunchPurger.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLaunchPurger.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLaunchPurger.Location = new System.Drawing.Point(30, 29);
            this.buttonLaunchPurger.Name = "buttonLaunchPurger";
            this.buttonLaunchPurger.Size = new System.Drawing.Size(120, 38);
            this.buttonLaunchPurger.TabIndex = 9;
            this.buttonLaunchPurger.Text = "LAUNCH DELETION TOOL";
            this.buttonLaunchPurger.UseVisualStyleBackColor = false;
            this.buttonLaunchPurger.Click += new System.EventHandler(this.buttonLaunchPurger_Click);
            // 
            // panelHealth
            // 
            this.panelHealth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHealth.Controls.Add(this.buttonLaunchHealthReview);
            this.panelHealth.Location = new System.Drawing.Point(366, 194);
            this.panelHealth.Name = "panelHealth";
            this.panelHealth.Size = new System.Drawing.Size(183, 100);
            this.panelHealth.TabIndex = 75;
            // 
            // buttonLaunchHealthReview
            // 
            this.buttonLaunchHealthReview.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLaunchHealthReview.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLaunchHealthReview.Location = new System.Drawing.Point(30, 29);
            this.buttonLaunchHealthReview.Name = "buttonLaunchHealthReview";
            this.buttonLaunchHealthReview.Size = new System.Drawing.Size(120, 38);
            this.buttonLaunchHealthReview.TabIndex = 10;
            this.buttonLaunchHealthReview.Text = "LAUNCH HEALTH TOOL";
            this.buttonLaunchHealthReview.UseVisualStyleBackColor = false;
            this.buttonLaunchHealthReview.Click += new System.EventHandler(this.buttonLaunchHealthReview_Click);
            // 
            // dbConnectionTestTimer
            // 
            this.dbConnectionTestTimer.Interval = 200;
            this.dbConnectionTestTimer.Tick += new System.EventHandler(this.dbConnectionTestTimer_Tick);
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
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelLauncher});
            this.statusStrip1.Location = new System.Drawing.Point(0, 294);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(548, 22);
            this.statusStrip1.TabIndex = 76;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelLauncher
            // 
            this.toolStripStatusLabelLauncher.Name = "toolStripStatusLabelLauncher";
            this.toolStripStatusLabelLauncher.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabelLauncher.Text = ":";
            // 
            // tbDbServerName
            // 
            this.tbDbServerName.Location = new System.Drawing.Point(170, 57);
            this.tbDbServerName.Name = "tbDbServerName";
            this.tbDbServerName.Size = new System.Drawing.Size(216, 20);
            this.tbDbServerName.TabIndex = 4;
            // 
            // btnTestDBConnection
            // 
            this.btnTestDBConnection.BackColor = System.Drawing.SystemColors.Control;
            this.btnTestDBConnection.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestDBConnection.Location = new System.Drawing.Point(396, 57);
            this.btnTestDBConnection.Name = "btnTestDBConnection";
            this.btnTestDBConnection.Size = new System.Drawing.Size(120, 23);
            this.btnTestDBConnection.TabIndex = 6;
            this.btnTestDBConnection.Text = "TEST CONNECTION";
            this.btnTestDBConnection.UseVisualStyleBackColor = false;
            this.btnTestDBConnection.Click += new System.EventHandler(this.btnTestDBConnection_Click);
            // 
            // lblSqlServerAddress
            // 
            this.lblSqlServerAddress.AutoSize = true;
            this.lblSqlServerAddress.Location = new System.Drawing.Point(12, 60);
            this.lblSqlServerAddress.Name = "lblSqlServerAddress";
            this.lblSqlServerAddress.Size = new System.Drawing.Size(118, 13);
            this.lblSqlServerAddress.TabIndex = 15;
            this.lblSqlServerAddress.Text = "Server Name\\Instance:";
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.AutoSize = true;
            this.lblDatabaseName.Location = new System.Drawing.Point(43, 86);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(87, 13);
            this.lblDatabaseName.TabIndex = 19;
            this.lblDatabaseName.Text = "Database Name:";
            // 
            // tbDatabaseName
            // 
            this.tbDatabaseName.Location = new System.Drawing.Point(170, 83);
            this.tbDatabaseName.Name = "tbDatabaseName";
            this.tbDatabaseName.Size = new System.Drawing.Size(216, 20);
            this.tbDatabaseName.TabIndex = 5;
            // 
            // btnChangeSqlServer
            // 
            this.btnChangeSqlServer.BackColor = System.Drawing.SystemColors.Control;
            this.btnChangeSqlServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeSqlServer.Location = new System.Drawing.Point(396, 81);
            this.btnChangeSqlServer.Name = "btnChangeSqlServer";
            this.btnChangeSqlServer.Size = new System.Drawing.Size(120, 23);
            this.btnChangeSqlServer.TabIndex = 7;
            this.btnChangeSqlServer.Text = "CHANGE SERVER";
            this.btnChangeSqlServer.UseVisualStyleBackColor = false;
            this.btnChangeSqlServer.Click += new System.EventHandler(this.btnChangeSqlServer_Click);
            // 
            // labelSqlServerTesting
            // 
            this.labelSqlServerTesting.AutoSize = true;
            this.labelSqlServerTesting.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSqlServerTesting.Location = new System.Drawing.Point(10, 12);
            this.labelSqlServerTesting.Name = "labelSqlServerTesting";
            this.labelSqlServerTesting.Size = new System.Drawing.Size(121, 15);
            this.labelSqlServerTesting.TabIndex = 20;
            this.labelSqlServerTesting.Text = "SQL Connection Test:";
            // 
            // panelDbConnection
            // 
            this.panelDbConnection.BackColor = System.Drawing.SystemColors.Control;
            this.panelDbConnection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDbConnection.Controls.Add(this.labelSqlAuthType);
            this.panelDbConnection.Controls.Add(this.comboBoxSqlAuthType);
            this.panelDbConnection.Controls.Add(this.labelSqlServerTesting);
            this.panelDbConnection.Controls.Add(this.btnChangeSqlServer);
            this.panelDbConnection.Controls.Add(this.tbDatabaseName);
            this.panelDbConnection.Controls.Add(this.lblDatabaseName);
            this.panelDbConnection.Controls.Add(this.lblSqlServerAddress);
            this.panelDbConnection.Controls.Add(this.btnTestDBConnection);
            this.panelDbConnection.Controls.Add(this.tbDbServerName);
            this.panelDbConnection.Location = new System.Drawing.Point(0, 73);
            this.panelDbConnection.Name = "panelDbConnection";
            this.panelDbConnection.Size = new System.Drawing.Size(549, 122);
            this.panelDbConnection.TabIndex = 71;
            // 
            // labelSqlAuthType
            // 
            this.labelSqlAuthType.AutoSize = true;
            this.labelSqlAuthType.Location = new System.Drawing.Point(25, 36);
            this.labelSqlAuthType.Name = "labelSqlAuthType";
            this.labelSqlAuthType.Size = new System.Drawing.Size(105, 13);
            this.labelSqlAuthType.TabIndex = 22;
            this.labelSqlAuthType.Text = "Authentication Type:";
            // 
            // comboBoxSqlAuthType
            // 
            this.comboBoxSqlAuthType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBoxSqlAuthType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSqlAuthType.FormattingEnabled = true;
            this.comboBoxSqlAuthType.Location = new System.Drawing.Point(170, 33);
            this.comboBoxSqlAuthType.Name = "comboBoxSqlAuthType";
            this.comboBoxSqlAuthType.Size = new System.Drawing.Size(216, 21);
            this.comboBoxSqlAuthType.TabIndex = 3;
            this.comboBoxSqlAuthType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSqlAuthType_SelectedIndexChanged);
            // 
            // panelServerType
            // 
            this.panelServerType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelServerType.Controls.Add(this.buttonChangeServerType);
            this.panelServerType.Controls.Add(this.comboBoxServerType);
            this.panelServerType.Controls.Add(this.labelHeatServerType);
            this.panelServerType.Location = new System.Drawing.Point(0, 26);
            this.panelServerType.Name = "panelServerType";
            this.panelServerType.Size = new System.Drawing.Size(549, 50);
            this.panelServerType.TabIndex = 77;
            // 
            // buttonChangeServerType
            // 
            this.buttonChangeServerType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonChangeServerType.Location = new System.Drawing.Point(396, 13);
            this.buttonChangeServerType.Name = "buttonChangeServerType";
            this.buttonChangeServerType.Size = new System.Drawing.Size(120, 23);
            this.buttonChangeServerType.TabIndex = 2;
            this.buttonChangeServerType.Text = "CHANGE TYPE";
            this.buttonChangeServerType.UseVisualStyleBackColor = false;
            this.buttonChangeServerType.Click += new System.EventHandler(this.buttonChangeServerType_Click);
            // 
            // comboBoxServerType
            // 
            this.comboBoxServerType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.comboBoxServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxServerType.FormattingEnabled = true;
            this.comboBoxServerType.Location = new System.Drawing.Point(170, 15);
            this.comboBoxServerType.Name = "comboBoxServerType";
            this.comboBoxServerType.Size = new System.Drawing.Size(216, 21);
            this.comboBoxServerType.TabIndex = 1;
            // 
            // labelHeatServerType
            // 
            this.labelHeatServerType.AutoSize = true;
            this.labelHeatServerType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeatServerType.Location = new System.Drawing.Point(32, 17);
            this.labelHeatServerType.Name = "labelHeatServerType";
            this.labelHeatServerType.Size = new System.Drawing.Size(99, 15);
            this.labelHeatServerType.TabIndex = 0;
            this.labelHeatServerType.Text = "Heat Server Type:";
            // 
            // FormLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 316);
            this.Controls.Add(this.panelServerType);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelHealth);
            this.Controls.Add(this.panelPurge);
            this.Controls.Add(this.panelProfiler);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelDbConnection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(564, 354);
            this.MinimumSize = new System.Drawing.Size(564, 354);
            this.Name = "FormLauncher";
            this.Text = "Heat Database Tool Launcher";
            this.Load += new System.EventHandler(this.FormLauncher_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelProfiler.ResumeLayout(false);
            this.panelPurge.ResumeLayout(false);
            this.panelHealth.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelDbConnection.ResumeLayout(false);
            this.panelDbConnection.PerformLayout();
            this.panelServerType.ResumeLayout(false);
            this.panelServerType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem requirementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panelProfiler;
        private System.Windows.Forms.Button buttonLaunchProfiler;
        private System.Windows.Forms.Panel panelPurge;
        private System.Windows.Forms.Button buttonLaunchPurger;
        private System.Windows.Forms.Panel panelHealth;
        private System.Windows.Forms.Button buttonLaunchHealthReview;
        private System.Windows.Forms.Timer dbConnectionTestTimer;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox tbDbServerName;
        private System.Windows.Forms.Button btnTestDBConnection;
        private System.Windows.Forms.Label lblSqlServerAddress;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.TextBox tbDatabaseName;
        private System.Windows.Forms.Button btnChangeSqlServer;
        private System.Windows.Forms.Label labelSqlServerTesting;
        private System.Windows.Forms.Panel panelDbConnection;
        private System.Windows.Forms.Panel panelServerType;
        private System.Windows.Forms.ComboBox comboBoxServerType;
        private System.Windows.Forms.Label labelHeatServerType;
        private System.Windows.Forms.Button buttonChangeServerType;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLauncher;
        private System.Windows.Forms.Label labelSqlAuthType;
        private System.Windows.Forms.ComboBox comboBoxSqlAuthType;
    }
}