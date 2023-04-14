using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;

using FileManager;
using System.Security.Policy;
using Newtonsoft.Json.Linq;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

using SWLib;
using System.Configuration;

namespace GUILayoutTest1
{
    public partial class Form1 : Form
    {
        private API? api = null;

        private LocalFileManage lfm = null;

        // application settings
        private string settingsFilename = "prog-settings.json";
        private string templateDir = @"D:\School\4610\sld resource files\";
        private string url = "";
        private string user = "";
        private string localHead = "";
        private string pid = "";
 
        // parent folder of executable
        private string executableDirectory;


        
        private Dictionary<string, List<string>> tree = new Dictionary<string, List<string>>();


        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> localFiles
            = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> cloudTree
            = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        // <name, path>
        // <project, C:\\project\head>
        private Dictionary<string, string> projectRoots = new Dictionary<string, string>();

        private string currentProject = "";
        private string currentSelectedProject = "";
        private string currentFile = "";


        // <name, path>
        // <testmodel, project//folder//testmodel>
        private Dictionary<string, string> trackedFiles = new Dictionary<string, string>();

        // checks for files that need to be saved
        private System.Windows.Forms.Timer checkTrackedTimer = new System.Windows.Forms.Timer();

        // list of file names that need to be saved
        // reference to tracked files
        private List<string> saveQueue = new List<string>();



        // list of custom colors
        private Color greenColor = Color.FromArgb(42, 161, 152);
        private Color orangeColor = Color.FromArgb(181, 137, 0);
        private Color redColor = Color.FromArgb(220, 50, 47);



        
        private Color statusNew;
        
        private Color statusSaved;
        
        private Color statusUpToDate;

        
        private Color statusUpstream;

        // file status
        //0 // indicator when file is fresh and unsaved
        //1 // indicator file is saved locally but not backed up
        //2 // indicator file is the same locally and on cloud
        //3 // indicator file is only on cloud





        static TaskpaneView? swTaskPane;
        static int buttonIdx;

        TestForm taskPaneWinFormControl;



        private ISldWorks app;
        private PartDoc doc;
        private SldWorks sld;




        // solidworks api variables
        SW_Instance swC = new SW_Instance();
        SW_DocMgr docM;

        bool swConnected = false;

        

        public Form1()
        {

            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(this.ApplicationClose);

            // init any colors
            this.sldConnectLabel.BackColor = this.redColor;
            this.cloudConnectLabel.BackColor = this.redColor;

            this.solidConnectButton.ForeColor = this.greenColor;
            this.apiConnectButton.ForeColor = this.greenColor;

            // find and set location of executable
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (location != null)
            {
                var path = Path.GetDirectoryName(location);
                if (path != null)
                {
                    this.executableDirectory = path;
                }

            }

            if (this.executableDirectory == null)
            {
                MessageBox.Show("Error location executable directory, exiting");
                Application.Exit();
            }


            // try to load global settings, if not found prompt for file
            this.initSettings(false);
            this.initSolidConnect();

            // try to connect to server with settings in config
            this.connectToServer(this.url, this.user, this.localHead);
            //this.update();


            // create timers and start
            this.checkTrackedTimer.Tick += new EventHandler(TimerEventProcessor);
            this.checkTrackedTimer.Interval = 1000;
            this.checkTrackedTimer.Start();

        }

        


        private void update()
        {
            if (api != null)
            {
                this.tree = new Dictionary<string, List<string>>();
                this.cloudTree = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();



                dynamic res2 = api.listProjects();
                foreach (dynamic project in res2)
                {
                    List<string> data = new List<string>();
                    string pname = project.name;
                    dynamic files = project.files;

                    this.cloudTree[pname] = new Dictionary<string, Dictionary<string, string>>();

                    // iterate over filenames
                    foreach (var file in project["files"].Children())
                    {
                        var filename = file.Name;
                        this.cloudTree[pname][filename] = new Dictionary<string, string>();

                        var value = file.Value;

                        string version = value.version;
                        this.cloudTree[pname][filename].Add("version", version);
                        //string relations = value.relations;
                        //string filename = file["name"];
                        
                        //string version = file.version;
                        data.Add(filename);
                    }

                    tree.Add(pname, data);
                }


                this.updateProjects();
                this.updateFiles();
            }
            else
            {
                this.connectToServer(this.url, this.user, this.localHead);
            }

            this.updateLocal();


        }



        private void initSettings(bool prompt)
        {
            string internalSettings = "";
            bool settingsLoaded = false;

            if (prompt)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult res = new DialogResult();

                res = dialog.ShowDialog();

                if (res == DialogResult.OK)
                {
                    internalSettings = dialog.FileName;
                }
            } else
            {
                string[] subs = { this.executableDirectory, this.settingsFilename };
                internalSettings = Path.Combine(subs);
            }

            
            try
            {
                dynamic o1 = JObject.Parse(File.ReadAllText(internalSettings));

                var address = o1.address;
                var user = o1.user;
                var root = o1.root;
                var pid = o1.pid;

                bool result = (address != null) && (user != null) &&
                              (root != null) && (pid != null);

                if (result)
                {
                    this.url = address;
                    this.user = user;
                    this.localHead = root + "\\";
                    this.pid = pid;

                    settingsLoaded = true;
                } else
                {
                    MessageBox.Show("Settings file is/has become corrupt");
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Settings file not found");
                settingsLoaded = false;
            }
            
            if (!settingsLoaded)
            {
                this.initSettings(true);
            }
        }

        // launch settings dialog and create settings
        private void launchSettingsDialog()
        {
            var settings = new Settings(this.url, this.user, this.localHead, this.pid);
            settings.ShowDialog();

            //Debug.WriteLine("window closed");

            string url = settings.address;
            string user = settings.user;
            string localHead = settings.path;
            string pid = settings.pid;

            bool res = this.connectToServer(url, user, localHead);

            if (res)
            {
                this.url = url;
                this.user = user;
                this.localHead = localHead;
                this.pid = pid;
                this.writeInternalSettings();
            }
            else
            {
                MessageBox.Show("Error Connecting");
            }
        }

        // write to internal settings
        private void writeInternalSettings()
        {

        }



        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            this.debugTimerEvents();

            List<string> complete = new List<string>();

            foreach( string item in this.saveQueue)
            {
                this.fileSaved(this.trackedFiles[item]);
                // pop item
                complete.Add(item);
            }

            foreach (string item in complete)
            {
                this.saveQueue.Remove(item);
            }

            
        }



        private void debugTimerEvents()
        {
            Debug.WriteLine("save queue: " + this.saveQueue.Count.ToString());
            foreach (string item in this.saveQueue)
            {
                Debug.WriteLine("    -" + item);
            }



            Debug.WriteLine("tracked files: " + this.trackedFiles.Count.ToString());
            foreach (KeyValuePair<string, string> item in this.trackedFiles)
            {
                Debug.WriteLine("    -" + item.Key + "    | " + item.Value);
            }

            Debug.WriteLine("");
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

            //swTaskPane.DisplayWindowFromHandlex64(taskPaneWinFormControl.Handle.ToInt64());

            Taskpane tp = new Taskpane();
            swTaskPane.DisplayWindowFromHandlex64(tp.getHandle());




            //bool result;
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Back, "Back (standard)");
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Next, "Next (standard)");
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close (standard)");
            //result = swTaskPane.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK (standard)");



            swTaskPane.TaskPaneActivateNotify += swTaskPane_TaskPaneActivateNotify;
            swTaskPane.TaskPaneDestroyNotify += swTaskPane_TaskPaneDestroyNotify;
            swTaskPane.TaskPaneToolbarButtonClicked += swTaskPane_TaskPaneToolbarButtonClicked;

        }

        // Taskpane events
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

        // TestForm Taskpane control events
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















        private int FileSavePostNotify(string name)
        {
            log("Event: PartDoc: Document " + name + " Saved");
            this.saveQueue.Add(Path.GetFileNameWithoutExtension(name));
            //return 1 if you don't want to save
            return 0;
        }


        private bool attachPartEvents(ModelDoc2 model)
        {
            PartDoc doc = (PartDoc)model;
            doc.FileSaveNotify += this.FileSavePostNotify;
            return true;
        }

        private bool attachAssemblyEvents(ModelDoc2 model)
        {
            AssemblyDoc doc = (AssemblyDoc)model;
            doc.FileSaveNotify += this.FileSavePostNotify;

            return true;
        }

        private bool attachDrawingEvents(ModelDoc2 model)
        {
            DrawingDoc doc = (DrawingDoc)model;
            doc.FileSaveNotify += this.FileSavePostNotify;
            return true;
        }


        



        private void fileSaved(string name)
        {
            string fullPath = name;
            string imPath = fullPath + ".BMP";
            bool res = this.createPreview(fullPath, imPath, false);

            if (res == true)
            {
                string[] pathArray = fullPath.Split(@"\");

                string fileName = pathArray[pathArray.Length - 1];
                string imName = fileName + ".BMP";

                string filePath = "";

                for (int i = 0; i < pathArray.Length - 1; i++)
                {
                    filePath += pathArray[i] + @"\";
                }

                string remoteDir = this.splitRootFromFile(this.localHead + this.currentProject, fullPath);

               // string tempLocation = this.localHead + @"temp\" + fileName;


                //File.Copy(fullPath, tempLocation);

                //ModelDoc2 model = (ModelDoc2)this.app.ActiveDoc;
                //this.docM.close(model.GetTitle());
                api.sendFile(filePath, currentProject, this.currentProject + @"\" + remoteDir, fileName, false);
                api.sendFile(filePath, currentProject, this.currentProject + @"\" + remoteDir, imName, true);

                //this.docM.openDoc(fullPath, false);
            }
            this.update();
        }



        private void log(string text)
        {
            Debug.WriteLine(text);
        }

















        private void s(object sender, EventArgs e)
        {

        }

        private void apiConnect()
        {

        }

        private void sldConnect()
        {

        }


        private bool check()
        {
            if (api != null && this.swConnected == true)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private bool checkSW()
        {
            if (this.swConnected == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        private void settingsButton_Click(object sender, EventArgs e)
        {
            this.launchSettingsDialog();
        }
        
        private bool connectToServer(string url, string user, string localHead)
        {
            bool result = false;

            try
            {
                this.api = new API(url, localHead, user);
                bool res = api.testConnection();

                if (res == true)
                {
                    result = true;
                    this.update();
                    
                }
            }
            catch (Exception ex)
            {
            }

            if (result == true)
            {
                this.cloudConnectLabel.BackColor = this.greenColor;
                this.apiConnectButton.ForeColor = this.redColor;
            } else
            {
                this.cloudConnectLabel.BackColor = this.redColor;
                this.apiConnectButton.ForeColor = this.greenColor;
            }

            return result;
        }

     

        private void newProjectButton_Click(object sender, EventArgs e)
        {
            if (api != null)
            {
                var newProject = new NewProject();

                newProject.ShowDialog();

                string name = newProject.name;
                string units = newProject.units;
                string prefix = newProject.prefix;
                string suffix = newProject.suffix;

                if (name != "")
                {
                    dynamic settings = new JObject();
                    settings.name = name;
                    settings.units = units;
                    settings.prefix = prefix;
                    settings.suffix = suffix;

                    bool res = this.api.createProject(settings);
                    Console.WriteLine(res);

                    this.lfm.createProject(name);

                    this.update();
                }
            } else
            {
                MessageBox.Show("Not connected to API!!");
            }
            
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            this.update();
        }

        


        private void updateLocal()
        {
            if (this.lfm == null)
            {
                if (this.localHead != "")
                {
                    this.lfm = new LocalFileManage(this.localHead, this.templateDir);


                    List<string> projectNames = new List<string>();

                    this.projectRoots = new Dictionary<string, string>();

                    foreach (KeyValuePair<string, List<string>> projectRoot in this.tree)
                    {
                        projectNames.Add(projectRoot.Key);
                        this.projectRoots.Add(projectRoot.Key, this.localHead + this.lfm.getPathFromName(projectRoot.Key));
                    }


                    this.lfm.createRootsFromList(projectNames);




                }
            }
            else
            {
                

                this.lfm.refreshLocalFileList();
            }
        }

        private void updateFiles()
        {
            if (this.currentSelectedProject != null)
            {
                if (tree.ContainsKey(this.currentSelectedProject))
                {
                    fileTreeView.BeginUpdate();
                    fileTreeView.Nodes.Clear();

                    foreach (var file in tree[this.currentSelectedProject])
                    {
                        TreeNode node = new TreeNode(file);
                        fileTreeView.Nodes.Add(node);

                    }

                    fileTreeView.EndUpdate();
                }
            }
            
        }

        private void updateProjects()
        {
            projectTreeView.BeginUpdate();
            projectTreeView.Nodes.Clear();

            foreach (KeyValuePair<string, List<string>> entry in this.tree)
            {
                TreeNode node = new TreeNode(entry.Key);
                node.ForeColor = this.greenColor;
                if (entry.Key == this.currentSelectedProject)
                {
                    node.NodeFont = new Font("Verdana", 12, FontStyle.Bold);
                } 
                else
                {
                    node.NodeFont = new Font("Verdana", 12);
                }
                
                projectTreeView.Nodes.Add(node);
            }

            projectTreeView.EndUpdate();
        }

        private void updateSelected()
        {

        }




        private void projectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                currentSelectedProject = e.Node.Text;
                
                this.updateFiles();
            }
        }

        private void projectTreeView_DoubleClick(object sender, EventArgs e)
        {
            currentProject = currentSelectedProject;
            currentProjectLabel.Text = currentProject;
        }




        private void fileTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                this.currentFile = e.Node.Text;
                currentFileLabel.Text = currentFile;
                this.updateSelected();

                string urlData = this.url;
                urlData += "preview/" + currentProject + "/" + this.currentFile;

                Debug.WriteLine(urlData);
                try
                {
                    this.previewPictureBox.Load(urlData);
                    this.previewPictureBox.Update();
                }
                catch (System.Net.WebException)
                {
                    Debug.WriteLine("404");
                }
                
               
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            // check for solidworks connection
            if (this.swConnected)
            {
                // check if a project is selected
                if (this.currentSelectedProject == "")
                {
                    return;
                }

                Dictionary<string, string> refr = new Dictionary<string, string>();

                if (this.lfm != null && this.currentProject != "")
                {
                    NewFileTypeDialog newTypeDialog = new NewFileTypeDialog(currentProject);
                    newTypeDialog.ShowDialog();

                    if (newTypeDialog.type != "")
                    {
                        List<string> templates = this.lfm.getTemplateNames(newTypeDialog.type);

                        // todo add default template support
                        NewFile newFileDialog = new NewFile(this.localHead, currentProject, newTypeDialog.type, templates);
                        newFileDialog.ShowDialog();


                        string type = newTypeDialog.type;
                        string fileName = newFileDialog.fileName;
                        string fileTemplate = lfm.getPathFromTemplateName(type, newFileDialog.fileTemplate);

                        if (fileName != "" && fileTemplate != "")
                        {
                            string fileOutputName = this.localHead + currentProject + @"\" + fileName;
                            this.lfm.createFileFromTemplate(fileTemplate, fileOutputName);
                            // todo use the solidworks create





                            this.docM.openDoc(fileOutputName, false);

                            string modelName = Path.GetFileNameWithoutExtension(fileOutputName);


                            ModelDoc2 newModel = this.docM.getModelFromName(modelName);



                            this.trackedFiles.Add(modelName, fileOutputName);


                            this.taskPaneWinFormControl.addDocEntry(modelName);


                            bool result = type switch
                            {
                                "SLDPRT" => this.attachPartEvents(newModel),
                                "SLDASM" => this.attachAssemblyEvents(newModel),
                                "SLDDRW" => this.attachDrawingEvents(newModel),
                                _ => false
                            };

                        }
                    }
                }
            } else
            {
                MessageBox.Show("Please initialize the SolidWorks connection");
            }
            
        }

        private bool createPreview(string filePath, string imPath, bool closeA)
        {
            string modelName = filePath;
            if (closeA) {
                modelName = docM.openDoc(filePath, false);
            } 

            docM.savePreviewBMP(Path.GetFileNameWithoutExtension(modelName), imPath, 1920, 1080);

            if (closeA)
            {
                docM.close(modelName);
            }
            

            return true;
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            if (api != null)
            {
                OpenFileDialog uploadFileDialog = new OpenFileDialog();
                DialogResult result = uploadFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string fullPath = uploadFileDialog.FileName;
                    string imPath = fullPath + ".BMP";
                    bool res = this.createPreview(fullPath, imPath, true);

                    if (res == true)
                    {
                        string[] pathArray = fullPath.Split(@"\");

                        string fileName = pathArray[pathArray.Length - 1];
                        string imName = fileName + ".BMP";

                        string filePath = "";

                        for (int i = 0; i < pathArray.Length - 1; i++)
                        {
                            filePath += pathArray[i] + @"\";
                        }

                        if (!projectRoots.ContainsKey(this.currentProject))
                        {
                            this.getRootForProject();
                        }

                        string remoteDir = this.splitRootFromFile(projectRoots[this.currentProject], fullPath);
                        api.sendFile(filePath, currentProject, this.currentProject + @"\" + remoteDir, fileName, false);
                        api.sendFile(filePath, currentProject, this.currentProject + @"\" + remoteDir, imName, true);
                    }

                    
                }
            }
            
            
        }

        private string splitRootFromFile(string root, string filePath)
        {
            string retPath = "";

            string[] rootSplit = root.Split(@"\", StringSplitOptions.RemoveEmptyEntries);
            string[] filePathSplit = filePath.Split(@"\", StringSplitOptions.RemoveEmptyEntries);

            for(int i = rootSplit.Length; i < filePathSplit.Length - 1; i++)
            {
                retPath += filePathSplit[i] + @"\";
            }

            
            return retPath;
        }

        private void getRootForProject()
        {
            FolderBrowserDialog rootFolder = new FolderBrowserDialog();
            DialogResult result = rootFolder.ShowDialog();

            if (result == DialogResult.OK)
            {
                string root = rootFolder.SelectedPath;
                this.projectRoots.Add(this.currentProject, root);
            }
        }

        private void projectTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button) {
                case MouseButtons.Right:
                    {
                        var coordinates = new Point(Cursor.Position.X - this.Left, Cursor.Position.Y - this.Top);
                        projectContextMenu.Show(this, new Point(coordinates.X, coordinates.Y));
                    }
                    break;
            }
        }

        

















        /**
         * 
         * RIGHT CLICK CONTEXT MENU EVENTS
         * 
         */

        // Project Context Menu
        private void projectContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                Debug.WriteLine(e.ClickedItem.Text);


                switch (e.ClickedItem.Text)
                {
                    case "Download All":
                            contextMenuDownloadAll();
                        break;
                    case "Upload All":
                            contextMenuUploadAll();
                        break;
                    case "Open All":
                            contextMenuOpenAll();
                        break;
                    case "Close All":
                            contextMenuCloseAll();
                        break;
                }
            }
            
        }


        /**
         * PROJECT CONTEXT MENU ACTIONS
         */
        private bool contextMenuDownloadAll()
        {


            return true;
        }

        private bool contextMenuUploadAll()
        {


            return true;
        }

        private bool contextMenuOpenAll()
        {


            return true;
        }

        private bool contextMenuCloseAll()
        {


            return true;
        }





        // File Context Menu
        private void fileTreeView_MouseDown(object sender, MouseEventArgs e)
        {

        }

        /**
         * PROJECT CONTEXT MENU ACTIONS
         */
        private bool contextMenuDownloadSingle()
        {


            return true;
        }

        private bool contextMenuUploadSingle()
        {


            return true;
        }

        private bool contextMenuOpenSingle()
        {


            return true;
        }

        private bool contextMenuCloseSingle()
        {


            return true;
        }

        private void solidConnectButton_Click(object sender, EventArgs e)
        {
            this.swConnected = false;

            var sldwrkSettings = new SolidworksSettings(this.pid);

            sldwrkSettings.ShowDialog();



            if (sldwrkSettings.create == true)
            {
                // start
                this.pid = this.swC.startSW().ToString();
                this.initSolidConnect();
            }
            else if (sldwrkSettings.pid != 0)
            {
                // connect
                bool res = this.swC.connectToProcess(sldwrkSettings.pid);
                this.swConnected = res;

                if (!this.swConnected)
                {
                    // error connecting
                    MessageBox.Show("Error attaching to PID");
                }
            }



            
        }



        private void initSolidConnect()
        {
            bool res = this.swC.connectToProcess(Int32.Parse(this.pid));
            this.swConnected = res;

            if (this.swConnected == true)
            {
                this.docM = new SW_DocMgr(this.swC);
                this.app = this.swC.getApp();

                this.createTaskPane(this.app);

                this.sldConnectLabel.BackColor = this.greenColor;
                this.solidConnectButton.ForeColor = this.redColor;
            }
            else if (this.pid != "")
            {
                this.sldConnectLabel.BackColor = this.orangeColor;
                this.solidConnectButton.ForeColor = this.orangeColor;
            }

            else
            {
                this.sldConnectLabel.BackColor = this.redColor;
                this.solidConnectButton.ForeColor = this.greenColor;
            }
        }

        private void apiConnectButton_Click(object sender, EventArgs e)
        {
            if (this.api == null)
            {
                this.connectToServer(this.url, this.user, this.localHead);
                this.update();
            } else
            {
                this.api = null;
                this.cloudConnectLabel.BackColor = this.redColor;
                this.apiConnectButton.ForeColor = this.greenColor;
            }
            
        }

        private void ApplicationClose(Object sender, FormClosingEventArgs e)
        {
            if (swTaskPane != null)
            {
                swTaskPane.DeleteView();
            }
        }
    }
}