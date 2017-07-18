namespace Lumension_Advanced_DB_Maintenance.Forms
{
    partial class FormRecordsProfiler
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecordsProfiler));
            this.byUserDataGridView = new System.Windows.Forms.DataGridView();
            this.byComputerDataGridView = new System.Windows.Forms.DataGridView();
            this.byTypeDataGridView = new System.Windows.Forms.DataGridView();
            this.byDeviceDataGridView = new System.Windows.Forms.DataGridView();
            this.byProcessDataGridView = new System.Windows.Forms.DataGridView();
            this.labelLineGraphProcessing = new System.Windows.Forms.Label();
            this.labelUserDataProcessing = new System.Windows.Forms.Label();
            this.labelComputerDataProcessing = new System.Windows.Forms.Label();
            this.labelTypeDataProcessing = new System.Windows.Forms.Label();
            this.labelDeviceDataProcessing = new System.Windows.Forms.Label();
            this.labelProcessDataProcessing = new System.Windows.Forms.Label();
            this.ByDateChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timeProfilerActivity = new System.Windows.Forms.Timer(this.components);
            this.tabControlProfiler = new System.Windows.Forms.TabControl();
            this.tabPageByDateGraphed = new System.Windows.Forms.TabPage();
            this.labelGraphedDateCountsProcessing = new System.Windows.Forms.Label();
            this.labelEventTypeSelector = new System.Windows.Forms.Label();
            this.cbEventTypesList = new System.Windows.Forms.ComboBox();
            this.tabPageByDateRaw = new System.Windows.Forms.TabPage();
            this.labelDateRawDataProcessing = new System.Windows.Forms.Label();
            this.byDateDataGridView = new System.Windows.Forms.DataGridView();
            this.tabPageByUser = new System.Windows.Forms.TabPage();
            this.tabPageByComputer = new System.Windows.Forms.TabPage();
            this.tabPageByType = new System.Windows.Forms.TabPage();
            this.tabPageByProcess = new System.Windows.Forms.TabPage();
            this.tabPageByDevice = new System.Windows.Forms.TabPage();
            this.buttonRerunAnalysis = new System.Windows.Forms.Button();
            this.buttonCloseProfiler = new System.Windows.Forms.Button();
            this.buttonExportToFile = new System.Windows.Forms.Button();
            this.buttonOpenLogFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.byUserDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byComputerDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byTypeDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byDeviceDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byProcessDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ByDateChart)).BeginInit();
            this.tabControlProfiler.SuspendLayout();
            this.tabPageByDateGraphed.SuspendLayout();
            this.tabPageByDateRaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byDateDataGridView)).BeginInit();
            this.tabPageByUser.SuspendLayout();
            this.tabPageByComputer.SuspendLayout();
            this.tabPageByType.SuspendLayout();
            this.tabPageByProcess.SuspendLayout();
            this.tabPageByDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // byUserDataGridView
            // 
            this.byUserDataGridView.AllowUserToAddRows = false;
            this.byUserDataGridView.AllowUserToDeleteRows = false;
            this.byUserDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.byUserDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.byUserDataGridView.Location = new System.Drawing.Point(0, 0);
            this.byUserDataGridView.Name = "byUserDataGridView";
            this.byUserDataGridView.ReadOnly = true;
            this.byUserDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.byUserDataGridView.Size = new System.Drawing.Size(866, 578);
            this.byUserDataGridView.TabIndex = 0;
            // 
            // byComputerDataGridView
            // 
            this.byComputerDataGridView.AllowUserToAddRows = false;
            this.byComputerDataGridView.AllowUserToDeleteRows = false;
            this.byComputerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.byComputerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.byComputerDataGridView.Location = new System.Drawing.Point(0, 0);
            this.byComputerDataGridView.Name = "byComputerDataGridView";
            this.byComputerDataGridView.ReadOnly = true;
            this.byComputerDataGridView.Size = new System.Drawing.Size(866, 581);
            this.byComputerDataGridView.TabIndex = 1;
            // 
            // byTypeDataGridView
            // 
            this.byTypeDataGridView.AllowUserToAddRows = false;
            this.byTypeDataGridView.AllowUserToDeleteRows = false;
            this.byTypeDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.byTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.byTypeDataGridView.Location = new System.Drawing.Point(0, 0);
            this.byTypeDataGridView.Name = "byTypeDataGridView";
            this.byTypeDataGridView.ReadOnly = true;
            this.byTypeDataGridView.Size = new System.Drawing.Size(866, 581);
            this.byTypeDataGridView.TabIndex = 2;
            // 
            // byDeviceDataGridView
            // 
            this.byDeviceDataGridView.AllowUserToAddRows = false;
            this.byDeviceDataGridView.AllowUserToDeleteRows = false;
            this.byDeviceDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.byDeviceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.byDeviceDataGridView.Location = new System.Drawing.Point(0, 0);
            this.byDeviceDataGridView.Name = "byDeviceDataGridView";
            this.byDeviceDataGridView.ReadOnly = true;
            this.byDeviceDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.byDeviceDataGridView.Size = new System.Drawing.Size(866, 579);
            this.byDeviceDataGridView.TabIndex = 3;
            this.byDeviceDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.byDeviceDataGridView_CellContentClick);
            // 
            // byProcessDataGridView
            // 
            this.byProcessDataGridView.AllowUserToAddRows = false;
            this.byProcessDataGridView.AllowUserToDeleteRows = false;
            this.byProcessDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.byProcessDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.byProcessDataGridView.Location = new System.Drawing.Point(0, 0);
            this.byProcessDataGridView.Name = "byProcessDataGridView";
            this.byProcessDataGridView.ReadOnly = true;
            this.byProcessDataGridView.Size = new System.Drawing.Size(866, 581);
            this.byProcessDataGridView.TabIndex = 4;
            // 
            // labelLineGraphProcessing
            // 
            this.labelLineGraphProcessing.AutoSize = true;
            this.labelLineGraphProcessing.Location = new System.Drawing.Point(207, 385);
            this.labelLineGraphProcessing.Name = "labelLineGraphProcessing";
            this.labelLineGraphProcessing.Size = new System.Drawing.Size(118, 13);
            this.labelLineGraphProcessing.TabIndex = 9;
            this.labelLineGraphProcessing.Text = "Processing SQL Data...";
            // 
            // labelUserDataProcessing
            // 
            this.labelUserDataProcessing.AutoSize = true;
            this.labelUserDataProcessing.Location = new System.Drawing.Point(181, 145);
            this.labelUserDataProcessing.Name = "labelUserDataProcessing";
            this.labelUserDataProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelUserDataProcessing.TabIndex = 10;
            this.labelUserDataProcessing.Text = "Processing SQL Data...";
            // 
            // labelComputerDataProcessing
            // 
            this.labelComputerDataProcessing.AutoSize = true;
            this.labelComputerDataProcessing.Location = new System.Drawing.Point(236, 193);
            this.labelComputerDataProcessing.Name = "labelComputerDataProcessing";
            this.labelComputerDataProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelComputerDataProcessing.TabIndex = 10;
            this.labelComputerDataProcessing.Text = "Processing SQL Data...";
            // 
            // labelTypeDataProcessing
            // 
            this.labelTypeDataProcessing.AutoSize = true;
            this.labelTypeDataProcessing.Location = new System.Drawing.Point(299, 245);
            this.labelTypeDataProcessing.Name = "labelTypeDataProcessing";
            this.labelTypeDataProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelTypeDataProcessing.TabIndex = 10;
            this.labelTypeDataProcessing.Text = "Processing SQL Data...";
            // 
            // labelDeviceDataProcessing
            // 
            this.labelDeviceDataProcessing.AutoSize = true;
            this.labelDeviceDataProcessing.Location = new System.Drawing.Point(444, 293);
            this.labelDeviceDataProcessing.Name = "labelDeviceDataProcessing";
            this.labelDeviceDataProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelDeviceDataProcessing.TabIndex = 10;
            this.labelDeviceDataProcessing.Text = "Processing SQL Data...";
            // 
            // labelProcessDataProcessing
            // 
            this.labelProcessDataProcessing.AutoSize = true;
            this.labelProcessDataProcessing.Location = new System.Drawing.Point(383, 257);
            this.labelProcessDataProcessing.Name = "labelProcessDataProcessing";
            this.labelProcessDataProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelProcessDataProcessing.TabIndex = 10;
            this.labelProcessDataProcessing.Text = "Processing SQL Data...";
            // 
            // ByDateChart
            // 
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.LineColor = System.Drawing.Color.LimeGreen;
            chartArea1.AxisX.LineWidth = 4;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Lime;
            chartArea1.AxisX.Title = "Date";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LineColor = System.Drawing.Color.LimeGreen;
            chartArea1.AxisY.LineWidth = 4;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Lime;
            chartArea1.AxisY.Title = "Event Count";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.BackColor = System.Drawing.Color.DimGray;
            chartArea1.Name = "ChartArea1";
            this.ByDateChart.ChartAreas.Add(chartArea1);
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "LegendByDate";
            this.ByDateChart.Legends.Add(legend1);
            this.ByDateChart.Location = new System.Drawing.Point(0, 0);
            this.ByDateChart.Name = "ByDateChart";
            series1.BorderColor = System.Drawing.Color.Gray;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.CornflowerBlue;
            series1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "LegendByDate";
            series1.LegendText = "Event Count";
            series1.LegendToolTip = "This is the exact event count are recorded by EMSS for the \\nspecified day.";
            series1.Name = "Series1";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Yellow;
            series2.Legend = "LegendByDate";
            series2.LegendText = "Moving Average";
            series2.LegendToolTip = "This is the moving average of the event counts to help identify if \\nthe average " +
    "counts are increasing over time.";
            series2.Name = "SeriesRollingAverage";
            this.ByDateChart.Series.Add(series1);
            this.ByDateChart.Series.Add(series2);
            this.ByDateChart.Size = new System.Drawing.Size(866, 543);
            this.ByDateChart.TabIndex = 11;
            this.ByDateChart.Text = "ByDateChart";
            this.ByDateChart.Click += new System.EventHandler(this.ByDateChart_Click);
            // 
            // timeProfilerActivity
            // 
            this.timeProfilerActivity.Interval = 200;
            this.timeProfilerActivity.Tick += new System.EventHandler(this.timeProfilerActivity_Tick);
            // 
            // tabControlProfiler
            // 
            this.tabControlProfiler.Controls.Add(this.tabPageByDateGraphed);
            this.tabControlProfiler.Controls.Add(this.tabPageByDateRaw);
            this.tabControlProfiler.Controls.Add(this.tabPageByUser);
            this.tabControlProfiler.Controls.Add(this.tabPageByComputer);
            this.tabControlProfiler.Controls.Add(this.tabPageByType);
            this.tabControlProfiler.Controls.Add(this.tabPageByProcess);
            this.tabControlProfiler.Controls.Add(this.tabPageByDevice);
            this.tabControlProfiler.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.tabControlProfiler.Location = new System.Drawing.Point(1, 1);
            this.tabControlProfiler.Name = "tabControlProfiler";
            this.tabControlProfiler.SelectedIndex = 0;
            this.tabControlProfiler.Size = new System.Drawing.Size(874, 607);
            this.tabControlProfiler.TabIndex = 12;
            // 
            // tabPageByDateGraphed
            // 
            this.tabPageByDateGraphed.Controls.Add(this.labelGraphedDateCountsProcessing);
            this.tabPageByDateGraphed.Controls.Add(this.labelEventTypeSelector);
            this.tabPageByDateGraphed.Controls.Add(this.cbEventTypesList);
            this.tabPageByDateGraphed.Controls.Add(this.ByDateChart);
            this.tabPageByDateGraphed.Location = new System.Drawing.Point(4, 22);
            this.tabPageByDateGraphed.Name = "tabPageByDateGraphed";
            this.tabPageByDateGraphed.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByDateGraphed.Size = new System.Drawing.Size(866, 581);
            this.tabPageByDateGraphed.TabIndex = 0;
            this.tabPageByDateGraphed.Text = "By Date: Graphed";
            this.tabPageByDateGraphed.UseVisualStyleBackColor = true;
            // 
            // labelGraphedDateCountsProcessing
            // 
            this.labelGraphedDateCountsProcessing.AutoSize = true;
            this.labelGraphedDateCountsProcessing.Location = new System.Drawing.Point(3, 87);
            this.labelGraphedDateCountsProcessing.Name = "labelGraphedDateCountsProcessing";
            this.labelGraphedDateCountsProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelGraphedDateCountsProcessing.TabIndex = 14;
            this.labelGraphedDateCountsProcessing.Text = "Processing SQL Data...";
            // 
            // labelEventTypeSelector
            // 
            this.labelEventTypeSelector.AutoSize = true;
            this.labelEventTypeSelector.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEventTypeSelector.Location = new System.Drawing.Point(292, 552);
            this.labelEventTypeSelector.Name = "labelEventTypeSelector";
            this.labelEventTypeSelector.Size = new System.Drawing.Size(71, 15);
            this.labelEventTypeSelector.TabIndex = 13;
            this.labelEventTypeSelector.Text = "Event Type:";
            // 
            // cbEventTypesList
            // 
            this.cbEventTypesList.FormattingEnabled = true;
            this.cbEventTypesList.Location = new System.Drawing.Point(374, 549);
            this.cbEventTypesList.Name = "cbEventTypesList";
            this.cbEventTypesList.Size = new System.Drawing.Size(161, 21);
            this.cbEventTypesList.TabIndex = 12;
            this.cbEventTypesList.SelectedIndexChanged += new System.EventHandler(this.cbEventTypesList_SelectedIndexChanged);
            // 
            // tabPageByDateRaw
            // 
            this.tabPageByDateRaw.Controls.Add(this.labelDateRawDataProcessing);
            this.tabPageByDateRaw.Controls.Add(this.byDateDataGridView);
            this.tabPageByDateRaw.Location = new System.Drawing.Point(4, 22);
            this.tabPageByDateRaw.Name = "tabPageByDateRaw";
            this.tabPageByDateRaw.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByDateRaw.Size = new System.Drawing.Size(866, 581);
            this.tabPageByDateRaw.TabIndex = 1;
            this.tabPageByDateRaw.Text = "By Date: Raw";
            this.tabPageByDateRaw.UseVisualStyleBackColor = true;
            // 
            // labelDateRawDataProcessing
            // 
            this.labelDateRawDataProcessing.AutoSize = true;
            this.labelDateRawDataProcessing.Location = new System.Drawing.Point(95, 108);
            this.labelDateRawDataProcessing.Name = "labelDateRawDataProcessing";
            this.labelDateRawDataProcessing.Size = new System.Drawing.Size(120, 13);
            this.labelDateRawDataProcessing.TabIndex = 1;
            this.labelDateRawDataProcessing.Text = "Processing SQL Data...";
            // 
            // byDateDataGridView
            // 
            this.byDateDataGridView.AllowUserToAddRows = false;
            this.byDateDataGridView.AllowUserToDeleteRows = false;
            this.byDateDataGridView.AllowUserToOrderColumns = true;
            this.byDateDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.byDateDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.byDateDataGridView.Location = new System.Drawing.Point(0, 0);
            this.byDateDataGridView.Name = "byDateDataGridView";
            this.byDateDataGridView.ReadOnly = true;
            this.byDateDataGridView.Size = new System.Drawing.Size(866, 563);
            this.byDateDataGridView.TabIndex = 0;
            this.byDateDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.byDateDataGridView_CellContentClick);
            // 
            // tabPageByUser
            // 
            this.tabPageByUser.Controls.Add(this.byUserDataGridView);
            this.tabPageByUser.Controls.Add(this.labelUserDataProcessing);
            this.tabPageByUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageByUser.Name = "tabPageByUser";
            this.tabPageByUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByUser.Size = new System.Drawing.Size(866, 581);
            this.tabPageByUser.TabIndex = 2;
            this.tabPageByUser.Text = "By User";
            this.tabPageByUser.UseVisualStyleBackColor = true;
            // 
            // tabPageByComputer
            // 
            this.tabPageByComputer.Controls.Add(this.byComputerDataGridView);
            this.tabPageByComputer.Controls.Add(this.labelComputerDataProcessing);
            this.tabPageByComputer.Location = new System.Drawing.Point(4, 22);
            this.tabPageByComputer.Name = "tabPageByComputer";
            this.tabPageByComputer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByComputer.Size = new System.Drawing.Size(866, 581);
            this.tabPageByComputer.TabIndex = 3;
            this.tabPageByComputer.Text = "By Computer";
            this.tabPageByComputer.UseVisualStyleBackColor = true;
            // 
            // tabPageByType
            // 
            this.tabPageByType.Controls.Add(this.byTypeDataGridView);
            this.tabPageByType.Controls.Add(this.labelTypeDataProcessing);
            this.tabPageByType.Location = new System.Drawing.Point(4, 22);
            this.tabPageByType.Name = "tabPageByType";
            this.tabPageByType.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByType.Size = new System.Drawing.Size(866, 581);
            this.tabPageByType.TabIndex = 4;
            this.tabPageByType.Text = "By Event Type";
            this.tabPageByType.UseVisualStyleBackColor = true;
            // 
            // tabPageByProcess
            // 
            this.tabPageByProcess.Controls.Add(this.byProcessDataGridView);
            this.tabPageByProcess.Controls.Add(this.labelProcessDataProcessing);
            this.tabPageByProcess.Location = new System.Drawing.Point(4, 22);
            this.tabPageByProcess.Name = "tabPageByProcess";
            this.tabPageByProcess.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByProcess.Size = new System.Drawing.Size(866, 581);
            this.tabPageByProcess.TabIndex = 5;
            this.tabPageByProcess.Text = "By Process";
            this.tabPageByProcess.UseVisualStyleBackColor = true;
            // 
            // tabPageByDevice
            // 
            this.tabPageByDevice.Controls.Add(this.byDeviceDataGridView);
            this.tabPageByDevice.Controls.Add(this.labelDeviceDataProcessing);
            this.tabPageByDevice.Location = new System.Drawing.Point(4, 22);
            this.tabPageByDevice.Name = "tabPageByDevice";
            this.tabPageByDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByDevice.Size = new System.Drawing.Size(866, 581);
            this.tabPageByDevice.TabIndex = 6;
            this.tabPageByDevice.Text = "By Device";
            this.tabPageByDevice.UseVisualStyleBackColor = true;
            // 
            // buttonRerunAnalysis
            // 
            this.buttonRerunAnalysis.BackColor = System.Drawing.Color.Green;
            this.buttonRerunAnalysis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRerunAnalysis.ForeColor = System.Drawing.Color.White;
            this.buttonRerunAnalysis.Location = new System.Drawing.Point(5, 610);
            this.buttonRerunAnalysis.Name = "buttonRerunAnalysis";
            this.buttonRerunAnalysis.Size = new System.Drawing.Size(143, 27);
            this.buttonRerunAnalysis.TabIndex = 13;
            this.buttonRerunAnalysis.Text = "RERUN ANALYSIS";
            this.buttonRerunAnalysis.UseVisualStyleBackColor = false;
            this.buttonRerunAnalysis.Click += new System.EventHandler(this.buttonRerunAnalysis_Click);
            // 
            // buttonCloseProfiler
            // 
            this.buttonCloseProfiler.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonCloseProfiler.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCloseProfiler.ForeColor = System.Drawing.Color.White;
            this.buttonCloseProfiler.Location = new System.Drawing.Point(717, 610);
            this.buttonCloseProfiler.Name = "buttonCloseProfiler";
            this.buttonCloseProfiler.Size = new System.Drawing.Size(154, 27);
            this.buttonCloseProfiler.TabIndex = 14;
            this.buttonCloseProfiler.Text = "CLOSE PROFILER";
            this.buttonCloseProfiler.UseVisualStyleBackColor = false;
            this.buttonCloseProfiler.Click += new System.EventHandler(this.buttonCloseProfiler_Click);
            // 
            // buttonExportToFile
            // 
            this.buttonExportToFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExportToFile.Location = new System.Drawing.Point(260, 614);
            this.buttonExportToFile.Name = "buttonExportToFile";
            this.buttonExportToFile.Size = new System.Drawing.Size(113, 23);
            this.buttonExportToFile.TabIndex = 15;
            this.buttonExportToFile.Text = "EXPORT TO FILE";
            this.buttonExportToFile.UseVisualStyleBackColor = true;
            this.buttonExportToFile.Click += new System.EventHandler(this.buttonExportToFile_Click);
            // 
            // buttonOpenLogFile
            // 
            this.buttonOpenLogFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenLogFile.Location = new System.Drawing.Point(484, 614);
            this.buttonOpenLogFile.Name = "buttonOpenLogFile";
            this.buttonOpenLogFile.Size = new System.Drawing.Size(113, 23);
            this.buttonOpenLogFile.TabIndex = 16;
            this.buttonOpenLogFile.Text = "VIEW LOG FILE";
            this.buttonOpenLogFile.UseVisualStyleBackColor = true;
            this.buttonOpenLogFile.Click += new System.EventHandler(this.buttonOpenLogFile_Click);
            // 
            // FormRecordsProfiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 642);
            this.Controls.Add(this.buttonOpenLogFile);
            this.Controls.Add(this.buttonExportToFile);
            this.Controls.Add(this.buttonCloseProfiler);
            this.Controls.Add(this.buttonRerunAnalysis);
            this.Controls.Add(this.tabControlProfiler);
            this.Controls.Add(this.labelLineGraphProcessing);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(893, 681);
            this.MinimumSize = new System.Drawing.Size(893, 681);
            this.Name = "FormRecordsProfiler";
            this.Text = "SQL Event Data Profiler";
            this.Load += new System.EventHandler(this.FormSqlDataProfiler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.byUserDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byComputerDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byTypeDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byDeviceDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byProcessDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ByDateChart)).EndInit();
            this.tabControlProfiler.ResumeLayout(false);
            this.tabPageByDateGraphed.ResumeLayout(false);
            this.tabPageByDateGraphed.PerformLayout();
            this.tabPageByDateRaw.ResumeLayout(false);
            this.tabPageByDateRaw.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byDateDataGridView)).EndInit();
            this.tabPageByUser.ResumeLayout(false);
            this.tabPageByUser.PerformLayout();
            this.tabPageByComputer.ResumeLayout(false);
            this.tabPageByComputer.PerformLayout();
            this.tabPageByType.ResumeLayout(false);
            this.tabPageByType.PerformLayout();
            this.tabPageByProcess.ResumeLayout(false);
            this.tabPageByProcess.PerformLayout();
            this.tabPageByDevice.ResumeLayout(false);
            this.tabPageByDevice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView byUserDataGridView;
        private System.Windows.Forms.DataGridView byComputerDataGridView;
        private System.Windows.Forms.DataGridView byTypeDataGridView;
        private System.Windows.Forms.DataGridView byDeviceDataGridView;
        private System.Windows.Forms.DataGridView byProcessDataGridView;
        private System.Windows.Forms.Label labelLineGraphProcessing;
        private System.Windows.Forms.Label labelUserDataProcessing;
        private System.Windows.Forms.Label labelComputerDataProcessing;
        private System.Windows.Forms.Label labelTypeDataProcessing;
        private System.Windows.Forms.Label labelDeviceDataProcessing;
        private System.Windows.Forms.Label labelProcessDataProcessing;
        private System.Windows.Forms.DataVisualization.Charting.Chart ByDateChart;
        private System.Windows.Forms.Timer timeProfilerActivity;
        private System.Windows.Forms.TabControl tabControlProfiler;
        private System.Windows.Forms.TabPage tabPageByDateGraphed;
        private System.Windows.Forms.TabPage tabPageByDateRaw;
        private System.Windows.Forms.DataGridView byDateDataGridView;
        private System.Windows.Forms.TabPage tabPageByUser;
        private System.Windows.Forms.TabPage tabPageByComputer;
        private System.Windows.Forms.TabPage tabPageByType;
        private System.Windows.Forms.TabPage tabPageByProcess;
        private System.Windows.Forms.TabPage tabPageByDevice;
        private System.Windows.Forms.Label labelDateRawDataProcessing;
        private System.Windows.Forms.Button buttonRerunAnalysis;
        private System.Windows.Forms.Button buttonCloseProfiler;
        private System.Windows.Forms.Label labelEventTypeSelector;
        private System.Windows.Forms.ComboBox cbEventTypesList;
        private System.Windows.Forms.Label labelGraphedDateCountsProcessing;
        private System.Windows.Forms.Button buttonExportToFile;
        private System.Windows.Forms.Button buttonOpenLogFile;
    }
}