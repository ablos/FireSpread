﻿namespace FireSpread
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.fireBox = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.burningLabel = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.treeChanceBox = new System.Windows.Forms.NumericUpDown();
            this.burstChanceBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.plotView = new OxyPlot.WindowsForms.PlotView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.waterBodyDropdown = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fireBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeChanceBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.burstChanceBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // fireBox
            // 
            this.fireBox.Location = new System.Drawing.Point(12, 12);
            this.fireBox.Name = "fireBox";
            this.fireBox.Size = new System.Drawing.Size(500, 250);
            this.fireBox.TabIndex = 0;
            this.fireBox.TabStop = false;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(518, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(518, 68);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Burning Trees: ";
            // 
            // burningLabel
            // 
            this.burningLabel.AutoSize = true;
            this.burningLabel.Location = new System.Drawing.Point(109, 265);
            this.burningLabel.Name = "burningLabel";
            this.burningLabel.Size = new System.Drawing.Size(13, 15);
            this.burningLabel.TabIndex = 4;
            this.burningLabel.Text = "0";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(518, 40);
            this.resetButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 22);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // treeChanceBox
            // 
            this.treeChanceBox.Location = new System.Drawing.Point(518, 118);
            this.treeChanceBox.Name = "treeChanceBox";
            this.treeChanceBox.Size = new System.Drawing.Size(75, 23);
            this.treeChanceBox.TabIndex = 7;
            this.treeChanceBox.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // burstChanceBox
            // 
            this.burstChanceBox.Location = new System.Drawing.Point(518, 163);
            this.burstChanceBox.Name = "burstChanceBox";
            this.burstChanceBox.Size = new System.Drawing.Size(75, 23);
            this.burstChanceBox.TabIndex = 8;
            this.burstChanceBox.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(518, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Tree Chance:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(518, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Burst Chance:";
            // 
            // plotView
            // 
            this.plotView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.plotView.Location = new System.Drawing.Point(622, 12);
            this.plotView.Name = "plotView";
            this.plotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView.Size = new System.Drawing.Size(624, 433);
            this.plotView.TabIndex = 11;
            this.plotView.Text = "plotView";
            this.plotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(622, 443);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(624, 13);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(604, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 425);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // waterBodyDropdown
            // 
            this.waterBodyDropdown.FormattingEnabled = true;
            this.waterBodyDropdown.Location = new System.Drawing.Point(12, 315);
            this.waterBodyDropdown.Name = "waterBodyDropdown";
            this.waterBodyDropdown.Size = new System.Drawing.Size(199, 23);
            this.waterBodyDropdown.TabIndex = 14;
            this.waterBodyDropdown.SelectedIndexChanged += new System.EventHandler(this.waterBodyDropdown_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 297);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Water Body Type:";
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 468);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.waterBodyDropdown);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.plotView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.burstChanceBox);
            this.Controls.Add(this.treeChanceBox);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.burningLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.fireBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form";
            this.Text = "Forest Fire Simulation";
            ((System.ComponentModel.ISupportInitialize)(this.fireBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeChanceBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.burstChanceBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox fireBox;
        private Button startButton;
        private Button pauseButton;
        private Label label1;
        private Label burningLabel;
        private Button resetButton;
        private NumericUpDown treeChanceBox;
        private NumericUpDown burstChanceBox;
        private Label label2;
        private Label label3;
        private OxyPlot.WindowsForms.PlotView plotView;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private ComboBox waterBodyDropdown;
        private Label label4;
    }
}