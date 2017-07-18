namespace Lumension_Advanced_DB_Maintenance.Forms
{
    partial class FormHealthReview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHealthReview));
            this.tabControlHealthReview = new System.Windows.Forms.TabControl();
            this.tabPageServerConfig = new System.Windows.Forms.TabPage();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonOpenLogFile = new System.Windows.Forms.Button();
            this.progressBarSql = new System.Windows.Forms.ProgressBar();
            this.dataGridViewServerConfig = new System.Windows.Forms.DataGridView();
            this.buttonExportToFile = new System.Windows.Forms.Button();
            this.buttonReRunAnalysis = new System.Windows.Forms.Button();
            this.tabPageWaitStatsInfo = new System.Windows.Forms.TabPage();
            this.dataGridViewOverallWaitStats = new System.Windows.Forms.DataGridView();
            this.tabPageStoredProcStatsInfo = new System.Windows.Forms.TabPage();
            this.dataGridViewStoredProcedureStats = new System.Windows.Forms.DataGridView();
            this.tabPageLogTableIndexHealth = new System.Windows.Forms.TabPage();
            this.buttonStopIndexChanges = new System.Windows.Forms.Button();
            this.buttonStartIndexChanges = new System.Windows.Forms.Button();
            this.dataGridViewIndex = new System.Windows.Forms.DataGridView();
            this.tabPageStatisticsHealth = new System.Windows.Forms.TabPage();
            this.buttonStopUpdatingTableStats = new System.Windows.Forms.Button();
            this.buttonUpdateTableStats = new System.Windows.Forms.Button();
            this.dataGridViewLogTableStatisticsHealth = new System.Windows.Forms.DataGridView();
            this.timerProcessing = new System.Windows.Forms.Timer(this.components);
            this.tabControlHealthReview.SuspendLayout();
            this.tabPageServerConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServerConfig)).BeginInit();
            this.tabPageWaitStatsInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOverallWaitStats)).BeginInit();
            this.tabPageStoredProcStatsInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStoredProcedureStats)).BeginInit();
            this.tabPageLogTableIndexHealth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIndex)).BeginInit();
            this.tabPageStatisticsHealth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLogTableStatisticsHealth)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlHealthReview
            // 
            this.tabControlHealthReview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlHealthReview.Controls.Add(this.tabPageServerConfig);
            this.tabControlHealthReview.Controls.Add(this.tabPageWaitStatsInfo);
            this.tabControlHealthReview.Controls.Add(this.tabPageStoredProcStatsInfo);
            this.tabControlHealthReview.Controls.Add(this.tabPageLogTableIndexHealth);
            this.tabControlHealthReview.Controls.Add(this.tabPageStatisticsHealth);
            this.tabControlHealthReview.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlHealthReview.Location = new System.Drawing.Point(0, 0);
            this.tabControlHealthReview.Name = "tabControlHealthReview";
            this.tabControlHealthReview.SelectedIndex = 0;
            this.tabControlHealthReview.Size = new System.Drawing.Size(1024, 498);
            this.tabControlHealthReview.TabIndex = 0;
            this.tabControlHealthReview.SelectedIndexChanged += new System.EventHandler(this.tabControlHealthReview_SelectedIndexChanged);
            // 
            // tabPageServerConfig
            // 
            this.tabPageServerConfig.Controls.Add(this.buttonClose);
            this.tabPageServerConfig.Controls.Add(this.buttonOpenLogFile);
            this.tabPageServerConfig.Controls.Add(this.progressBarSql);
            this.tabPageServerConfig.Controls.Add(this.dataGridViewServerConfig);
            this.tabPageServerConfig.Controls.Add(this.buttonExportToFile);
            this.tabPageServerConfig.Controls.Add(this.buttonReRunAnalysis);
            this.tabPageServerConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageServerConfig.Name = "tabPageServerConfig";
            this.tabPageServerConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServerConfig.Size = new System.Drawing.Size(1016, 472);
            this.tabPageServerConfig.TabIndex = 0;
            this.tabPageServerConfig.Text = "Server Configuration";
            this.tabPageServerConfig.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonClose.ForeColor = System.Drawing.Color.White;
            this.buttonClose.Location = new System.Drawing.Point(801, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(88, 23);
            this.buttonClose.TabIndex = 15;
            this.buttonClose.Text = "CLOSE";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonCloseProfiler_Click);
            // 
            // buttonOpenLogFile
            // 
            this.buttonOpenLogFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonOpenLogFile.Location = new System.Drawing.Point(675, 3);
            this.buttonOpenLogFile.Name = "buttonOpenLogFile";
            this.buttonOpenLogFile.Size = new System.Drawing.Size(113, 23);
            this.buttonOpenLogFile.TabIndex = 6;
            this.buttonOpenLogFile.Text = "VIEW LOG FILE";
            this.buttonOpenLogFile.UseVisualStyleBackColor = true;
            this.buttonOpenLogFile.Click += new System.EventHandler(this.buttonOpenLogFile_Click);
            // 
            // progressBarSql
            // 
            this.progressBarSql.Location = new System.Drawing.Point(3, 3);
            this.progressBarSql.Name = "progressBarSql";
            this.progressBarSql.Size = new System.Drawing.Size(67, 23);
            this.progressBarSql.TabIndex = 5;
            this.progressBarSql.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // dataGridViewServerConfig
            // 
            this.dataGridViewServerConfig.AllowUserToAddRows = false;
            this.dataGridViewServerConfig.AllowUserToDeleteRows = false;
            this.dataGridViewServerConfig.AllowUserToResizeRows = false;
            this.dataGridViewServerConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewServerConfig.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewServerConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServerConfig.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewServerConfig.Name = "dataGridViewServerConfig";
            this.dataGridViewServerConfig.Size = new System.Drawing.Size(1016, 440);
            this.dataGridViewServerConfig.TabIndex = 0;
            // 
            // buttonExportToFile
            // 
            this.buttonExportToFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonExportToFile.Location = new System.Drawing.Point(544, 3);
            this.buttonExportToFile.Name = "buttonExportToFile";
            this.buttonExportToFile.Size = new System.Drawing.Size(113, 23);
            this.buttonExportToFile.TabIndex = 4;
            this.buttonExportToFile.Text = "EXPORT TO FILE";
            this.buttonExportToFile.UseVisualStyleBackColor = true;
            this.buttonExportToFile.Click += new System.EventHandler(this.buttonExportToFile_Click);
            // 
            // buttonReRunAnalysis
            // 
            this.buttonReRunAnalysis.BackColor = System.Drawing.Color.Green;
            this.buttonReRunAnalysis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.buttonReRunAnalysis.ForeColor = System.Drawing.Color.White;
            this.buttonReRunAnalysis.Location = new System.Drawing.Point(76, 3);
            this.buttonReRunAnalysis.Name = "buttonReRunAnalysis";
            this.buttonReRunAnalysis.Size = new System.Drawing.Size(147, 23);
            this.buttonReRunAnalysis.TabIndex = 3;
            this.buttonReRunAnalysis.Text = "RE-RUN ANALYSIS";
            this.buttonReRunAnalysis.UseVisualStyleBackColor = false;
            this.buttonReRunAnalysis.Click += new System.EventHandler(this.buttonReRunAnalysis_Click);
            // 
            // tabPageWaitStatsInfo
            // 
            this.tabPageWaitStatsInfo.Controls.Add(this.dataGridViewOverallWaitStats);
            this.tabPageWaitStatsInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageWaitStatsInfo.Name = "tabPageWaitStatsInfo";
            this.tabPageWaitStatsInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWaitStatsInfo.Size = new System.Drawing.Size(1016, 472);
            this.tabPageWaitStatsInfo.TabIndex = 1;
            this.tabPageWaitStatsInfo.Text = "Wait Stats Info";
            this.tabPageWaitStatsInfo.UseVisualStyleBackColor = true;
            // 
            // dataGridViewOverallWaitStats
            // 
            this.dataGridViewOverallWaitStats.AllowUserToAddRows = false;
            this.dataGridViewOverallWaitStats.AllowUserToDeleteRows = false;
            this.dataGridViewOverallWaitStats.AllowUserToResizeRows = false;
            this.dataGridViewOverallWaitStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewOverallWaitStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewOverallWaitStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOverallWaitStats.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewOverallWaitStats.Name = "dataGridViewOverallWaitStats";
            this.dataGridViewOverallWaitStats.Size = new System.Drawing.Size(1016, 444);
            this.dataGridViewOverallWaitStats.TabIndex = 0;
            // 
            // tabPageStoredProcStatsInfo
            // 
            this.tabPageStoredProcStatsInfo.Controls.Add(this.dataGridViewStoredProcedureStats);
            this.tabPageStoredProcStatsInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageStoredProcStatsInfo.Name = "tabPageStoredProcStatsInfo";
            this.tabPageStoredProcStatsInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStoredProcStatsInfo.Size = new System.Drawing.Size(1016, 472);
            this.tabPageStoredProcStatsInfo.TabIndex = 2;
            this.tabPageStoredProcStatsInfo.Text = "Stored Procedure Stats Info";
            this.tabPageStoredProcStatsInfo.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStoredProcedureStats
            // 
            this.dataGridViewStoredProcedureStats.AllowUserToAddRows = false;
            this.dataGridViewStoredProcedureStats.AllowUserToDeleteRows = false;
            this.dataGridViewStoredProcedureStats.AllowUserToResizeRows = false;
            this.dataGridViewStoredProcedureStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStoredProcedureStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewStoredProcedureStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStoredProcedureStats.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewStoredProcedureStats.Name = "dataGridViewStoredProcedureStats";
            this.dataGridViewStoredProcedureStats.Size = new System.Drawing.Size(1016, 440);
            this.dataGridViewStoredProcedureStats.TabIndex = 0;
            // 
            // tabPageLogTableIndexHealth
            // 
            this.tabPageLogTableIndexHealth.Controls.Add(this.buttonStopIndexChanges);
            this.tabPageLogTableIndexHealth.Controls.Add(this.buttonStartIndexChanges);
            this.tabPageLogTableIndexHealth.Controls.Add(this.dataGridViewIndex);
            this.tabPageLogTableIndexHealth.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogTableIndexHealth.Name = "tabPageLogTableIndexHealth";
            this.tabPageLogTableIndexHealth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLogTableIndexHealth.Size = new System.Drawing.Size(1016, 472);
            this.tabPageLogTableIndexHealth.TabIndex = 3;
            this.tabPageLogTableIndexHealth.Text = "Log Table Index Health";
            this.tabPageLogTableIndexHealth.UseVisualStyleBackColor = true;
            // 
            // buttonStopIndexChanges
            // 
            this.buttonStopIndexChanges.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStopIndexChanges.Location = new System.Drawing.Point(388, 3);
            this.buttonStopIndexChanges.Name = "buttonStopIndexChanges";
            this.buttonStopIndexChanges.Size = new System.Drawing.Size(147, 23);
            this.buttonStopIndexChanges.TabIndex = 2;
            this.buttonStopIndexChanges.Text = "STOP INDEX CHANGES";
            this.buttonStopIndexChanges.UseVisualStyleBackColor = true;
            this.buttonStopIndexChanges.Click += new System.EventHandler(this.buttonStopIndexChanges_Click);
            // 
            // buttonStartIndexChanges
            // 
            this.buttonStartIndexChanges.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStartIndexChanges.Location = new System.Drawing.Point(232, 3);
            this.buttonStartIndexChanges.Name = "buttonStartIndexChanges";
            this.buttonStartIndexChanges.Size = new System.Drawing.Size(147, 23);
            this.buttonStartIndexChanges.TabIndex = 1;
            this.buttonStartIndexChanges.Text = "START INDEX CHANGES";
            this.buttonStartIndexChanges.UseVisualStyleBackColor = true;
            this.buttonStartIndexChanges.Click += new System.EventHandler(this.buttonStartIndexChanges_Click);
            // 
            // dataGridViewIndex
            // 
            this.dataGridViewIndex.AllowUserToAddRows = false;
            this.dataGridViewIndex.AllowUserToDeleteRows = false;
            this.dataGridViewIndex.AllowUserToResizeRows = false;
            this.dataGridViewIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewIndex.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewIndex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIndex.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewIndex.Name = "dataGridViewIndex";
            this.dataGridViewIndex.Size = new System.Drawing.Size(1016, 440);
            this.dataGridViewIndex.TabIndex = 0;
            this.dataGridViewIndex.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIndex_CellClick);
            this.dataGridViewIndex.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIndex_CellContentClick);
            this.dataGridViewIndex.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIndex_CellEndEdit);
            this.dataGridViewIndex.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIndex_CellValueChanged);
            this.dataGridViewIndex.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridViewIndex_CurrentCellDirtyStateChanged);
            // 
            // tabPageStatisticsHealth
            // 
            this.tabPageStatisticsHealth.Controls.Add(this.buttonStopUpdatingTableStats);
            this.tabPageStatisticsHealth.Controls.Add(this.buttonUpdateTableStats);
            this.tabPageStatisticsHealth.Controls.Add(this.dataGridViewLogTableStatisticsHealth);
            this.tabPageStatisticsHealth.Location = new System.Drawing.Point(4, 22);
            this.tabPageStatisticsHealth.Name = "tabPageStatisticsHealth";
            this.tabPageStatisticsHealth.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStatisticsHealth.Size = new System.Drawing.Size(1016, 472);
            this.tabPageStatisticsHealth.TabIndex = 4;
            this.tabPageStatisticsHealth.Text = "Log Table Statistics Health";
            this.tabPageStatisticsHealth.UseVisualStyleBackColor = true;
            // 
            // buttonStopUpdatingTableStats
            // 
            this.buttonStopUpdatingTableStats.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonStopUpdatingTableStats.Location = new System.Drawing.Point(388, 3);
            this.buttonStopUpdatingTableStats.Name = "buttonStopUpdatingTableStats";
            this.buttonStopUpdatingTableStats.Size = new System.Drawing.Size(147, 23);
            this.buttonStopUpdatingTableStats.TabIndex = 5;
            this.buttonStopUpdatingTableStats.Text = "STOP UPDATING STATS";
            this.buttonStopUpdatingTableStats.UseVisualStyleBackColor = true;
            this.buttonStopUpdatingTableStats.Click += new System.EventHandler(this.buttonStopUpdatingTableStats_Click);
            // 
            // buttonUpdateTableStats
            // 
            this.buttonUpdateTableStats.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonUpdateTableStats.Location = new System.Drawing.Point(232, 3);
            this.buttonUpdateTableStats.Name = "buttonUpdateTableStats";
            this.buttonUpdateTableStats.Size = new System.Drawing.Size(147, 23);
            this.buttonUpdateTableStats.TabIndex = 4;
            this.buttonUpdateTableStats.Text = "UPDATE TABLE STATS";
            this.buttonUpdateTableStats.UseVisualStyleBackColor = true;
            this.buttonUpdateTableStats.Click += new System.EventHandler(this.buttonUpdateTableStats_Click);
            // 
            // dataGridViewLogTableStatisticsHealth
            // 
            this.dataGridViewLogTableStatisticsHealth.AllowUserToAddRows = false;
            this.dataGridViewLogTableStatisticsHealth.AllowUserToDeleteRows = false;
            this.dataGridViewLogTableStatisticsHealth.AllowUserToResizeRows = false;
            this.dataGridViewLogTableStatisticsHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLogTableStatisticsHealth.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewLogTableStatisticsHealth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLogTableStatisticsHealth.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewLogTableStatisticsHealth.Name = "dataGridViewLogTableStatisticsHealth";
            this.dataGridViewLogTableStatisticsHealth.Size = new System.Drawing.Size(1020, 440);
            this.dataGridViewLogTableStatisticsHealth.TabIndex = 3;
            // 
            // timerProcessing
            // 
            this.timerProcessing.Tick += new System.EventHandler(this.timerProcessing_Tick);
            // 
            // FormHealthReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 498);
            this.Controls.Add(this.tabControlHealthReview);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1040, 536);
            this.Name = "FormHealthReview";
            this.Text = "EMSS SQL Server Health Analysis Tool";
            this.Load += new System.EventHandler(this.FormHealthReview_Load);
            this.tabControlHealthReview.ResumeLayout(false);
            this.tabPageServerConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServerConfig)).EndInit();
            this.tabPageWaitStatsInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOverallWaitStats)).EndInit();
            this.tabPageStoredProcStatsInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStoredProcedureStats)).EndInit();
            this.tabPageLogTableIndexHealth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIndex)).EndInit();
            this.tabPageStatisticsHealth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLogTableStatisticsHealth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlHealthReview;
        private System.Windows.Forms.TabPage tabPageServerConfig;
        private System.Windows.Forms.TabPage tabPageWaitStatsInfo;
        private System.Windows.Forms.TabPage tabPageStoredProcStatsInfo;
        private System.Windows.Forms.TabPage tabPageLogTableIndexHealth;
        private System.Windows.Forms.TabPage tabPageStatisticsHealth;
        private System.Windows.Forms.Button buttonExportToFile;
        private System.Windows.Forms.Button buttonReRunAnalysis;
        private System.Windows.Forms.Button buttonStopIndexChanges;
        private System.Windows.Forms.Button buttonStartIndexChanges;
        private System.Windows.Forms.DataGridView dataGridViewIndex;
        private System.Windows.Forms.DataGridView dataGridViewServerConfig;
        private System.Windows.Forms.Button buttonStopUpdatingTableStats;
        private System.Windows.Forms.Button buttonUpdateTableStats;
        private System.Windows.Forms.DataGridView dataGridViewLogTableStatisticsHealth;
        private System.Windows.Forms.DataGridView dataGridViewOverallWaitStats;
        private System.Windows.Forms.DataGridView dataGridViewStoredProcedureStats;
        private System.Windows.Forms.ProgressBar progressBarSql;
        private System.Windows.Forms.Timer timerProcessing;
        private System.Windows.Forms.Button buttonOpenLogFile;
        private System.Windows.Forms.Button buttonClose;
    }
}