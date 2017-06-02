using System;
using System.Drawing;
using System.Windows.Forms;

namespace NASPerformanceTester
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Label driveLetterLabel = new Label();

        private Label networkPathLabel = new Label();

        private Label fileSizeLabel = new Label();

        private Label loopsLabel = new Label();

        private Button benchmarkButton = new Button();

        private TextBox networkPath = new TextBox();

        private ComboBox driveLetter = new ComboBox();

        private ComboBox fileSize = new ComboBox();

        private ComboBox loops = new ComboBox();

        private TextBox resultArea = new TextBox();

        private Label infoLabel = new Label();

        private LinkLabel urlLabel = new LinkLabel();

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "NAS performance tester 1.7";
            this.Size = new Size(558, 400);
            this.Font = new Font("Microsoft Sans Serif", 8);

            driveLetterLabel.Location = new Point(5, 8);
            driveLetterLabel.Text = "NAS drive letter";
            driveLetterLabel.Size = new Size(83, 20);
            driveLetter.Location = new Point(90, 5);
            driveLetter.Size = new Size(33, 15);

            networkPathLabel.Location = new Point(123, 8);
            networkPathLabel.Text = "or network path";
            networkPathLabel.Size = new Size(80, 20);

            loopsLabel.Location = new Point(407, 8);
            loopsLabel.Text = "Loops";
            loopsLabel.Size = new Size(36, 20);

            networkPath.Location = new Point(205, 5);
            networkPath.Text = "";
            networkPath.Size = new Size(90, 20);
            fileSizeLabel.Location = new Point(302, 8);
            fileSizeLabel.Text = "File size";
            fileSizeLabel.Size = new Size(50, 20);
            fileSize.Location = new Point(352, 5);
            fileSize.Size = new Size(49, 15);
            fileSize.Items.AddRange(
                new object[]
                {
                    "100",
                    "200",
                    "400",
                    "800",
                    "1000",
                    "2000",
                    "4000",
                    "8000"
                });
            fileSize.SelectedIndex = 2;

            loops.Location = new Point(443, 5);
            loops.Size = new Size(37, 15);
            loops.Items.AddRange(
                new object[]
                {
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "10",
                    "20",
                    "40"
                });
            loops.SelectedIndex = 4;
            benchmarkButton.Location = new Point(487, 5);
            benchmarkButton.Size = new Size(50, 20);
            benchmarkButton.Text = "Start";
            resultArea.Location = new Point(5, 30);
            resultArea.Size = new Size(533, 305);
            resultArea.ReadOnly = true;
            resultArea.Multiline = true;
            resultArea.ScrollBars = ScrollBars.Vertical;
            resultArea.WordWrap = false;
            resultArea.Text = "NAS performance tester 1.7 http://www.808.dk/?nastester\r\n";
            resultArea.Font = new Font("Courier New", 8);
            infoLabel.Location = new Point(5, 341);
            infoLabel.Text = "For more information, visit";
            infoLabel.Size = new Size(140, 20);
            urlLabel.Location = new Point(143, 341);
            urlLabel.Text = "http://www.808.dk/?code-csharp-nas-performance";
            urlLabel.Links.Add(0, 46, "http://www.808.dk/?code-csharp-nas-performance");
            urlLabel.Size = new Size(300, 20);

            benchmarkButton.Click += new EventHandler(benchmarkButton_Click);
            urlLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.urlLabel_LinkClicked);

            this.Controls.Add(loopsLabel);
            this.Controls.Add(driveLetterLabel);
            this.Controls.Add(networkPathLabel);
            this.Controls.Add(driveLetter);
            this.Controls.Add(networkPath);
            this.Controls.Add(fileSizeLabel);
            this.Controls.Add(fileSize);
            this.Controls.Add(loops);
            this.Controls.Add(benchmarkButton);
            this.Controls.Add(resultArea);
            this.Controls.Add(infoLabel);
            this.Controls.Add(urlLabel);
        }

        #endregion
    }
}

