using SWLib;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Xml.Linq;

namespace SWLogger
{
    public partial class Form1 : Form
    {
        private string path = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";

        private SW_Instance swC;


        private ISldWorks app = null;
        private PartDoc doc = null;
        private SldWorks sld = null;

        static TaskpaneView swTaskPane;
        static int buttonIdx;

        TestForm taskPaneWinFormControl = null;

        public Form1()
        {
            swC = new SW_Instance();
            InitializeComponent();
            pathTextBox.Text = path;
            pidTextBox.Text = "17412";


        }

        private void log(string message)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new MethodInvoker(delegate { textBox1.AppendText(message + "\r\n"); }));
            }
            else
            {
                textBox1.AppendText(message + "\r\n");
            }
        }




        private void launchButton_Click(object sender, EventArgs e)
        {
            string[] args = { };
            this.start(swC.startFromArgs(args));
        }

        private void processButton_Click(object sender, EventArgs e)
        {

        }

        private void buildTaskButton_Click(object sender, EventArgs e)
        {
            createTaskPane(this.app);
            createEvent();
            destroyTaskButton.Enabled = true;
        }
        private void destroyTaskButton_Click(object sender, EventArgs e)
        {
            if (this.taskPaneWinFormControl != null)
            {
                swTaskPane.DeleteView();
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string[] args = { pidTextBox.Text };
            this.start(swC.startFromArgs(args));
        }
        private void start(bool value)
        {
            log(value.ToString());

            if (value == true)
            {
                app = this.swC.getApp();
                buildTaskButton.Enabled = true;
                
            }
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





            bool result;
            result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Back, "Back (standard)");
            result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Next, "Next (standard)");
            result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close (standard)");
            result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK (standard)");
            


            swTaskPane.TaskPaneActivateNotify += swTaskPane_TaskPaneActivateNotify;
            swTaskPane.TaskPaneDestroyNotify += swTaskPane_TaskPaneDestroyNotify;
            swTaskPane.TaskPaneToolbarButtonClicked += swTaskPane_TaskPaneToolbarButtonClicked;
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
                    log("Task back button: clicked");
                    break;
                case 2:
                    log("Task next button: clicked");
                    break;
                case 3:
                    log("Task cancel button: clicked");
                    break;
                case 4:
                    log("Task ok button: clicked");
                    break;
            }
            return 1;
        }

        private void formButton1(object sender, EventArgs e)
        {
            log("Taskpane test button 1: clicked");
        }
        private void formButton2(object sender, EventArgs e)
        {
            log("Taskpane test button 2: clicked");
        }
        private void formButton3(object sender, EventArgs e)
        {
            log("Taskpane test button 3: clicked");
        }


        private void createEvent()
        {
            this.sld = (SldWorks)this.app;
            this.sld.ActiveDocChangeNotify += this.SldWorks_ActiveDocChangeNotify;
            this.sld.DocumentLoadNotify2 += this.SldWorks_DocumentLoadNotify;
            this.sld.FileNewNotify2 += this.SldWorks_FileNewNotify;
        }
        private int SldWorks_ActiveDocChangeNotify()
        {
            log("Event: SldWorks: Document Changed");
            return 1;
        }
        private int SldWorks_DocumentLoadNotify(string docTitle, string docPath)
        {
            log("Event: SldWorks: " + docTitle + " Loaded");
            return 1;
        }
        private int SldWorks_FileNewNotify(object NewDoc, int DocType, string TemplateName)
        {
            log("Event: SldWorks: New " + TemplateName + " created");
            return 1;
        }


        private void convertButton_Click(object sender, EventArgs e)
        {

        }
    }
}