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
    public partial class MainWindow : System.Windows.Forms.Form
    {

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        private ISldWorks sldApp = null;
        private PartDoc doc = null;
        private SldWorks sld = null;

        static TaskpaneView swTaskPane;
        static int buttonIdx;

        TestForm taskPaneWinFormControl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int pid = 52696;
            sldApp = GetSwAppFromProcess(pid);
            if (sldApp != null)
            {
                log("Solidworks task created");
                createTaskPane(sldApp);
                createEvent();

                //TestForm TaskPaneWinFormControl = new TestForm();
                //TaskPaneWinFormControl.Show();
            }
            else
            {
                MessageBox.Show("Error attaching to PID!!");
            }

        }
        private void createEvent()
        {
            this.doc = (PartDoc)this.sldApp.ActiveDoc;
            this.doc.FileSaveNotify += this.PartDoc_FileSaveNotify;

            this.sld = (SldWorks)this.sldApp;
            this.sld.ActiveDocChangeNotify += this.SldWorks_ActiveDocChangeNotify;
        }
        private int SldWorks_ActiveDocChangeNotify()
        {
            log("doc changed");
            this.createEvent();
            return 1;
        }
        private int PartDoc_FileSaveNotify(string name)
        {
            log(name + " saved");
            return 1;
        }
        private void formButton1(object sender, EventArgs e)
        {
            log("form button1");
        }
        private void formButton2(object sender, EventArgs e)
        {
            log("form button2");
        }
        private void formButton3(object sender, EventArgs e)
        {
            log("form button3");
        }
        void createTaskPane(ISldWorks app)
        {
            string bitmap = "";
            string toolTip = "Test Task Pane";
            string ctrlName = "Test.Task";
            string ctrlLicKey = "";

            swTaskPane = (TaskpaneView)app.CreateTaskpaneView2(bitmap, toolTip);

            taskPaneWinFormControl = new TestForm();
            taskPaneWinFormControl.testButton1.Click += new System.EventHandler(formButton1);
            taskPaneWinFormControl.testButton2.Click += new System.EventHandler(formButton2);
            taskPaneWinFormControl.testButton3.Click += new System.EventHandler(formButton3);

            swTaskPane.DisplayWindowFromHandlex64(taskPaneWinFormControl.Handle.ToInt64());

            



            //bool result;
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Next, "Next (standard)");
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Back, "Back (standard)");
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK (standard)");
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close (standard)");


            swTaskPane.TaskPaneActivateNotify += swTaskPane_TaskPaneActivateNotify;
            swTaskPane.TaskPaneDestroyNotify += swTaskPane_TaskPaneDestroyNotify;
            swTaskPane.TaskPaneToolbarButtonClicked += swTaskPane_TaskPaneToolbarButtonClicked;
        }


        private void destroyButton_Click(object sender, EventArgs e)
        {
            swTaskPane.DeleteView();
            
        }
        private void refreshButton_Click(object sender, EventArgs e)
        {
            swTaskPane.ShowView();
        }

        int swTaskPane_TaskPaneActivateNotify()
        {
            if (swTaskPane.GetButtonState(0) == false)
            {
                for (buttonIdx = 0; buttonIdx <= 20; buttonIdx++)
                {
                    swTaskPane.SetButtonState(buttonIdx, true);
                }
            }
            else
            {
                for (buttonIdx = 0; buttonIdx <= 20; buttonIdx++)
                {
                    swTaskPane.SetButtonState(buttonIdx, false);
                }
            }
            return 0;
        }

        int swTaskPane_TaskPaneDestroyNotify()
        {
            return 1;
        }

        int swTaskPane_TaskPaneToolbarButtonClicked(int ButtonIndex)
        {
            switch ((ButtonIndex + 1))
            {
                case 1:
                    log("Save (custom png) button clicked.");
                    break;
                case 2:
                    log("Next button clicked.");
                    break;
                case 3:
                    log("Back button clicked.");
                    break;
                case 4:
                    log("Okay button clicked.");
                    break;
                case 5:
                    log("Close button clicked and tab deleted.");
                    swTaskPane.DeleteView();
                    break;
            }
            return 1;
        }



























        private void log(string message)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new MethodInvoker(delegate { textBox1.AppendText(message + "\r\n"); }));
            } else
            {
                textBox1.AppendText(message + "\r\n");
            }
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

        
    }
    }




