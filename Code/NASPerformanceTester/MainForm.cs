using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NASPerformanceTester
{
    public partial class MainForm : Form
    {
        private string localStoragePath;

        private bool loopBreak;

        private ulong testFileSize;

        private bool testIsActive;

        private ulong testIterations;

        private string testPath;

        private string testType;

        private Thread workerThread;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            driveLetter.Items.AddRange(GetNetworkDriveLetters());
            if (GetNetworkDriveLetters().Length > 0)
            {
                driveLetter.SelectedIndex = 0;
            }

            ActiveControl = networkPath;
            if (IsFolderWritable(Path.GetDirectoryName(Application.ExecutablePath)))
            {
                localStoragePath = Path.GetDirectoryName(Application.ExecutablePath);
            }
            else // Write access to program folder denied, falling back to Windows temp folder as defined in the environment variables
            {
                localStoragePath = Path.GetTempPath();
            }

            base.OnLoad(e);
        }

        private async void BenchmarkHandler()
        {
            testIsActive = true;
            loopBreak = false;
            benchmarkButton.Text = "Stop";
            await Task.Run(() => BenchmarkAsync());
            testIsActive = false;
            loopBreak = false;
            benchmarkButton.Text = "Start";
        }

        private void BenchmarkAsync()
        {
            if (string.IsNullOrEmpty(networkPath.Text))
            {
                testPath = driveLetter.Text + ":";
            }
            else
            {
                testPath = networkPath.Text;
            }

            if (!loopBreak)
            {
                // Run warmup
                testFileSize = 0; // for warmup run
                testIterations = 1;
                testType = "write";
                TestPerf();
            }

            testFileSize = Convert.ToUInt64(fileSize.Text) * 1000000;
            testIterations = Convert.ToUInt64(loops.Text);
            if (!loopBreak)
            {
                // Run write test
                testType = "write";
                TestPerf();
            }

            if (!loopBreak)
            {
                // Run read test
                testType = "read";
                TestPerf();
            }
        }

        private string[] GetNetworkDriveLetters()
        {
            ArrayList NetworkDriveLetters = new ArrayList();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Network)
                {
                    NetworkDriveLetters.Add(d.Name.Substring(0, 1));
                }
            }

            return NetworkDriveLetters.ToArray(typeof(string)) as string[];
        }

        private bool IsFolderWritable(string folderPath)
        {
            try
            {
                using (FileStream fs = File.Create(Path.Combine(folderPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                {
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private void TestPerf()
        {
            try
            {
                string firstPath;
                string secondPath;
                string shortType;
                string iterText;
                ulong appendIterations;
                double totalPerf;
                DateTime startTime;
                DateTime stopTime;
                string randomText = RandomString(100000);
                if (testType == "read")
                {
                    firstPath = testPath;
                    secondPath = localStoragePath;
                    shortType = "R";
                }
                else
                {
                    firstPath = localStoragePath;
                    secondPath = testPath;
                    shortType = "W";
                }
                if (testIterations == 1)
                {
                    iterText = "once";
                }
                else if (testIterations == 2)
                {
                    iterText = "twice";
                }
                else
                {
                    iterText = testIterations + " times";
                }
                if (testFileSize == 0)
                {
                    OutputText("Running warmup...\r\n");
                    appendIterations = 100; // equals a 10MB warmup file.
                }
                else
                {
                    OutputText(
                        "Running a " + testFileSize / 1000000 + "MB file " + testType +
                        " on " + testPath + " " + iterText + "...\r\n");
                    appendIterations = testFileSize / 100000;

                    // Note: dividing integers in C# always produce a whole number,
                    // so no explicit rounding or type conversion is needed
                }
                totalPerf = 0;
                for (ulong j = 1; j <= testIterations; j++)
                {
                    Application.DoEvents();
                    if (File.Exists(firstPath + "\\" + j + "test.tmp"))
                    {
                        File.Delete(firstPath + "\\" + j + "test.tmp");
                    }
                    if (File.Exists(secondPath + "\\" + j + "test.tmp"))
                    {
                        File.Delete(secondPath + "\\" + j + "test.tmp");
                    }
                    if (loopBreak)
                    {
                        OutputText("Benchmark cancelled.\r\n");
                        break;
                    }
                    StreamWriter sWriter = new StreamWriter(
                        firstPath +
                        "\\" + j + "test.tmp",
                        true,
                        Encoding.UTF8,
                        1048576);
                    for (ulong i = 1; i <= appendIterations; i++)
                    {
                        sWriter.Write(randomText);
                    }
                    sWriter.Close();
                    startTime = DateTime.Now;
                    File.Copy(
                        firstPath + "\\" + j + "test.tmp",
                        secondPath + "\\" + j + "test.tmp");
                    stopTime = DateTime.Now;
                    File.Delete(firstPath + "\\" + j + "test.tmp");
                    File.Delete(secondPath + "\\" + j + "test.tmp");
                    TimeSpan interval = stopTime - startTime;
                    if (testIterations > 1)
                    {
                        OutputText(
                            ("Iteration " + j + ":").PadRight(15) +
                            (testFileSize / 1000 / interval.TotalMilliseconds).ToString("F2").PadLeft(7) +
                            " MB/sec\r\n");
                    }

                    totalPerf += testFileSize / 1000 / interval.TotalMilliseconds;
                }

                if (testFileSize != 0 && loopBreak == false)
                {
                    OutputText("-----------------------------\r\n");
                    OutputText(
                        "Average (" + shortType + "):" +
                        (totalPerf / testIterations).ToString("F2").PadLeft(10) + " MB/sec\r\n");
                    OutputText("-----------------------------\r\n");
                }
            }
            catch (Exception e)
            {
                OutputText("An error occured: " + e.Message + "\r\n");
            }
        }

        private void OutputText(string text)
        {
            this.Execute(
                () =>
                {
                    resultArea.AppendText(text);
                });
        }

        private void benchmarkButton_Click(object sender, EventArgs e)
        {
            if (!testIsActive)
            {
                int checkFileSize;
                int checkLoops;
                if ((!string.IsNullOrEmpty(driveLetter.Text) || networkPath.Text.IndexOf("\\\\") == 0 && networkPath.Text.IndexOf("\\", 2) > 2) &&
                    int.TryParse(fileSize.Text, out checkFileSize) &&
                    int.TryParse(loops.Text, out checkLoops))
                {
                    if (checkFileSize <= 64000)
                    {
                        BenchmarkHandler();
                    }
                    else
                    {
                        OutputText("The maximum test file size is 64GB.\r\n");
                    }
                }
                else
                {
                    OutputText("Input invalid. Check drive letter or network path, file size and loops.\r\n");
                }
            }
            else
            {
                loopBreak = true;
                OutputText("Cancelling. Please wait for current loop to finish...\r\n");
            }
        }

        private void urlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            Process.Start(target);
        }
    }
}
