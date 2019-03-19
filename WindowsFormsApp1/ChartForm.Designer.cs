namespace WindowsFormsApp1
{
    partial class ChartForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.EfficiencyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DataPointsLabel = new System.Windows.Forms.Label();
            this.DataPointsUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.EfficiencyChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataPointsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // EfficiencyChart
            // 
            this.EfficiencyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EfficiencyChart.BackColor = System.Drawing.Color.Transparent;
            chartArea6.Name = "ChartArea1";
            this.EfficiencyChart.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.EfficiencyChart.Legends.Add(legend6);
            this.EfficiencyChart.Location = new System.Drawing.Point(144, 34);
            this.EfficiencyChart.Name = "EfficiencyChart";
            this.EfficiencyChart.Size = new System.Drawing.Size(431, 312);
            this.EfficiencyChart.TabIndex = 0;
            this.EfficiencyChart.Text = "chart1";
            // 
            // SizeUpDown
            // 
            this.SizeUpDown.Location = new System.Drawing.Point(11, 50);
            this.SizeUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.SizeUpDown.Name = "SizeUpDown";
            this.SizeUpDown.Size = new System.Drawing.Size(115, 20);
            this.SizeUpDown.TabIndex = 1;
            this.SizeUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Location = new System.Drawing.Point(8, 34);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(68, 13);
            this.SizeLabel.TabIndex = 2;
            this.SizeLabel.Text = "Problem Size";
            // 
            // ReloadButton
            // 
            this.ReloadButton.Location = new System.Drawing.Point(11, 162);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(115, 23);
            this.ReloadButton.TabIndex = 5;
            this.ReloadButton.Text = "Apply";
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.update_chart);
            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(158, 9);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(208, 20);
            this.TitleLabel.TabIndex = 7;
            this.TitleLabel.Text = "Size vs Basic Operations";
            // 
            // DataPointsLabel
            // 
            this.DataPointsLabel.AutoSize = true;
            this.DataPointsLabel.Location = new System.Drawing.Point(14, 91);
            this.DataPointsLabel.Name = "DataPointsLabel";
            this.DataPointsLabel.Size = new System.Drawing.Size(62, 13);
            this.DataPointsLabel.TabIndex = 9;
            this.DataPointsLabel.Text = "Data Points";
            // 
            // DataPointsUpDown
            // 
            this.DataPointsUpDown.Location = new System.Drawing.Point(12, 107);
            this.DataPointsUpDown.Name = "DataPointsUpDown";
            this.DataPointsUpDown.Size = new System.Drawing.Size(115, 20);
            this.DataPointsUpDown.TabIndex = 8;
            this.DataPointsUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(598, 358);
            this.Controls.Add(this.DataPointsLabel);
            this.Controls.Add(this.DataPointsUpDown);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.ReloadButton);
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.SizeUpDown);
            this.Controls.Add(this.EfficiencyChart);
            this.Name = "ChartForm";
            this.Text = "Algorithm Efficiency";
            this.Load += new System.EventHandler(this.ChartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EfficiencyChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataPointsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart EfficiencyChart;
        private System.Windows.Forms.NumericUpDown SizeUpDown;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Label DataPointsLabel;
        private System.Windows.Forms.NumericUpDown DataPointsUpDown;
    }
}

