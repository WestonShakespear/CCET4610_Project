using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace LogSldworkTest
{
    public partial class Form1 : Form
    {

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        private string path = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";

        private Process process;
        private ISldWorks sldApp = null;

        public Form1()
        {
            InitializeComponent();
            pathTextBox.Text = path;
            pidTextBox.Text = "1988";

        }

        private void log(string message)
        {
            textBox1.AppendText(message + "\r\n");
        }




        private void launchButton_Click(object sender, EventArgs e)
        {
            log("Launching");

            path = pathTextBox.Text;

            try
            {
                process = Process.Start(path);
                log("SolidWorks Loaded with PID: " + process.Id.ToString());
                processButton.Enabled = true;
            }
            catch
            {
                log("Error starting");
            }
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            log("Precheck: " + (sldApp != null).ToString());
            sldApp = GetSwAppFromProcess(process.Id);
            if (sldApp != null)
            {
                newPartButton.Enabled = true;
                log("Version: " + sldApp.RevisionNumber().ToString());
            }
            
        }

        private void newPartButton_Click(object sender, EventArgs e)
        {
            sldApp.NewPart();
        }

        private static ISldWorks GetSwAppFromProcess(int processId)
        {
            var monikerName = "SolidWorks_PID_" + processId.ToString();

            IBindCtx context = null;
            IRunningObjectTable rot = null;
            IEnumMoniker monikers = null;

            try
            {
                CreateBindCtx(0, out context);

                context.GetRunningObjectTable(out rot);
                rot.EnumRunning(out monikers);

                var moniker = new IMoniker[1];

                while (monikers.Next(1, moniker, IntPtr.Zero) == 0)
                {
                    var curMoniker = moniker.First();

                    string name = null;

                    if (curMoniker != null)
                    {
                        try
                        {
                            curMoniker.GetDisplayName(context, null, out name);
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }
                    }

                    if (string.Equals(monikerName,
                        name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        object app;
                        rot.GetObject(curMoniker, out app);
                        return app as ISldWorks;
                    }
                }
            }
            finally
            {
                if (monikers != null)
                {
                    Marshal.ReleaseComObject(monikers);
                }

                if (rot != null)
                {
                    Marshal.ReleaseComObject(rot);
                }

                if (context != null)
                {
                    Marshal.ReleaseComObject(context);
                }
            }

            return null;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            log("Precheck: " + (sldApp != null).ToString());
            sldApp = GetSwAppFromProcess(Int32.Parse(pidTextBox.Text));
            if (sldApp != null)
            {
                newPartButton.Enabled = true;
                log("Version: " + sldApp.RevisionNumber().ToString());
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            //string stepPath = stepPathTextBox.Text.ToString();
            string stepPath = @"C:\Users\Initec\Downloads\sma washer .SLDPRT";

            log(stepPath);
            int errors = 0;
            int warnings = 0;

            
            
        }

       // sldApp.OpenDoc6(
               // stepPath,
               // (int) swDocumentTypes_e.swDocPART,
               // (int) swOpenDocOptions_e.swOpenDocOptions_LoadModel,
              //  "", ref errors, ref warnings);
    }
    }




