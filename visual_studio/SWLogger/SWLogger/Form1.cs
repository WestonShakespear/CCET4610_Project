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

        private string loadedTitle = "";

        public Form1()
        {
            swC = new SW_Instance();
            InitializeComponent();
            pathTextBox.Text = path;
            pidTextBox.Text = "37588";


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

        private void updateText(Label host, string message)
        {
            if (host.InvokeRequired)
            {
                textBox1.Invoke(new MethodInvoker(delegate { host.Text = message; }));
            }
            else
            {
                host.Text = message;
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

        private void updateActive(ModelDoc2 model)
        {
            log(model.GetPathName());

            int currentType = model.GetType();
            string typeString = currentType switch
            {
                1 => "Part",
                2 => "Assembly",
                3 => "Drawing",
                _ => ""
            };
            string currentTitle = model.GetTitle();

            this.updateText(activeDocNameLabel, currentTitle);
            this.updateText(activeDocTypeLabel, typeString);

            if (currentTitle == this.loadedTitle)
            {
                log("attach events to doc" + this.loadedTitle);
                bool result = currentType switch
                {
                    1 => this.attachPartEvents(model),
                    2 => this.attachAssemblyEvents(model),
                    3 => this.attachDrawingEvents(model),
                    _ => false
                };
            }
            this.loadedTitle = "";
        }

        private bool attachPartEvents(ModelDoc2 model)
        {
            PartDoc doc = (PartDoc)model;
            doc.FileSaveNotify += this.PartDoc_FileSaveNotify;
            doc.EquationEditorPostNotify += this.PartDoc_EquationEditorPostNotify;
            doc.FileSaveAsNotify2 += this.PartDoc_FileSaveAsNotify2;
            return true;
        }

        private bool attachDrawingEvents(ModelDoc2 model)
        {
            DrawingDoc doc = (DrawingDoc)model;
            doc.FileSaveNotify += this.DrawingDoc_FileSaveNotify;
            doc.EquationEditorPostNotify += this.DrawingDoc_EquationEditorPostNotify;
            doc.FileSaveAsNotify2 += this.DrawingDoc_FileSaveAsNotify2;
            return true;
        }

        private bool attachAssemblyEvents(ModelDoc2 model)
        {
            AssemblyDoc doc = (AssemblyDoc)model;
            doc.FileSaveNotify += this.AssemblyDoc_FileSaveNotify;
            doc.EquationEditorPostNotify += this.AssemblyDoc_EquationEditorPostNotify;
            doc.FileSaveAsNotify2 += this.AssemblyDoc_FileSaveAsNotify2;

            doc.AddItemNotify += this.AssemblyDoc_AddItemNotify;
            doc.AddMatePostNotify2 += this.AssemblyDoc_AddMatePostNotify2;

            return true;
        }

        private int PartDoc_FileSaveNotify(string name)
        {
            log("Event: PartDoc: Document " + name + " Saved");
            return 1;
        }

        private int PartDoc_EquationEditorPostNotify(bool changed)
        {
            log("Event: PartDoc: Equation editor closed, Modified: " + changed.ToString());
            return 1;
        }

        private int PartDoc_FileSaveAsNotify2(string filename)
        {
            log("Event: PartDoc: Document " + filename + " SaveAs triggered");
            return 1;
        }

        private int DrawingDoc_FileSaveNotify(string name)
        {
            log("Event: DrawingDoc: Document " + name + " Saved");
            return 1;
        }

        private int DrawingDoc_EquationEditorPostNotify(bool changed)
        {
            log("Event: DrawingDoc: Equation editor closed, Modified: " + changed.ToString());
            return 1;
        }

        private int DrawingDoc_FileSaveAsNotify2(string filename)
        {
            log("Event: DrawingDoc: Document " + filename + " SaveAs triggered");
            return 1;
        }

        private int AssemblyDoc_FileSaveNotify(string name)
        {
            log("Event: AssemblyDoc: Document " + name + " Saved");
            return 1;
        }

        private int AssemblyDoc_EquationEditorPostNotify(bool changed)
        {
            log("Event: AssemblyDoc: Equation editor closed, Modified: " + changed.ToString());
            return 1;
        }

        private int AssemblyDoc_FileSaveAsNotify2(string filename)
        {
            log("Event: AssemblyDoc: Document " + filename + " SaveAs triggered");
            return 1;
        }

        private int AssemblyDoc_AddItemNotify(int EntityType, string itemName)
        {
            log("Event: AssemblyDoc: Component " + itemName + " added");
            return 1;
        }

        private int AssemblyDoc_AddMatePostNotify2(ref object mates)
        {
            log("Event: AssemblyDoc: Mate Post Notify");
            return 1;
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
  
            updateActive((ModelDoc2)app.IActiveDoc2);
            return 1;
        }
        private int SldWorks_DocumentLoadNotify(string docTitle, string docPath)
        {
            log("Event: SldWorks: " + docTitle + " Loaded");
            this.loadedTitle = docTitle;
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