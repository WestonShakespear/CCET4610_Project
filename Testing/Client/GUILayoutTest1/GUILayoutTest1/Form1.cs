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
using System.Runtime.CompilerServices;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace GUILayoutTest1
{
    public partial class Form1 : Form
    {
        //private API? api = null;

        private LocalFileManage lfm = null;

        // application settings
        private string settingsFilename = "prog-settings.json";

        
        private string url = "";
        private string user = "";
        private string localHead = "";
        private string templateDir = "";
        private string pid = "";

        // parent folder of executable
        private string executableDirectory;



        private Dictionary<string, List<string>> tree = new Dictionary<string, List<string>>();


        //private Dictionary<string, Dictionary<string, Dictionary<string, string>>> localFiles
        //    = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

        //private Dictionary<string, Dictionary<string, Dictionary<string, string>>> cloudTree
        //    = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        //private Dictionary<string, string> cloudLookup = new Dictionary<string, string>();

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

        private System.Windows.Forms.Timer checkDocTimer = new System.Windows.Forms.Timer();

        // list of file names that need to be saved
        // reference to tracked files
        private List<string> saveQueue = new List<string>();
        private Dictionary<string, bool> openQueue = new Dictionary<string, bool>();



        // list of custom colors
        private Color greenColor = Color.FromArgb(42, 161, 152);
        private Color orangeColor = Color.FromArgb(181, 137, 0);
        private Color redColor = Color.FromArgb(220, 50, 47);



        // file status
        //0 // indicator when file is fresh and unsaved
        //1 // indicator file is saved locally but not backed up
        //2 // indicator file is the same locally and on cloud
        //3 // indicator file is only on cloud




        // Taskpane
        static TaskpaneView? swTaskPane;
        Taskpane taskPaneView;
        static int buttonIdx;




        // Solidworks instance
        private ISldWorks app;
        private PartDoc doc;
        private SldWorks sld;


        // solidworks api
        SW_Instance swC = new SW_Instance();
        SW_DocMgr docM;

        bool swConnected = false;
        bool apiConnected = false;


        private bool checkDocFlag = false;



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
                System.Windows.Forms.Application.Exit();
            }


            // try to load global settings, if not found prompt for file
            this.initSettings(false);
            

            // try to connect to server with settings in config
            this.initAPIConnect(this.url, this.user, this.localHead);
            this.initSolidConnect();
            //this.update();


            // create timers and start
            this.checkTrackedTimer.Tick += new EventHandler(TimerTrackedEventProcessor);
            this.checkTrackedTimer.Interval = 1000;
            this.checkTrackedTimer.Start();

            this.checkDocTimer.Tick += new EventHandler(TimerCheckDocEventProcessor);
            this.checkDocTimer.Interval = 1000;

        }




        //----------------------------------UPDATE----------------------------------//

        private void update()
        {
            if (!this.isAPIConnected())
            {
                return;
            }
  
            var treeNew = this.lfm.updateCloudTree();
            if (treeNew != null)
            {
                this.tree = treeNew;

                this.updateProjectTree();
                this.updateFileTree();
            }


            this.updateLocal();

            this.refreshPreview();
        }


        

        private void updateLocal()
        {
            if (this.localHead != "")
            {
                List<string> projectNames = new List<string>();

                foreach (KeyValuePair<string, List<string>> projectRoot in this.tree)
                {
                    projectNames.Add(projectRoot.Key);
                }


                this.lfm.updateLocalProjectsFromList(projectNames);




            }

            this.lfm.updateCloudTree();
            this.lfm.refreshLocalFileList();
        }




        private void saveQueueProcessing()
        {
            List<string> complete = new List<string>();

            foreach (string item in this.saveQueue)
            {
                this.fileSaved(item);
                // pop item
                complete.Add(item);
            }

            foreach (string item in complete)
            {
                this.saveQueue.Remove(item);
            }
        }

        private void openQueueProcessing()
        {
            List<string> exist = new List<string>();

            foreach (KeyValuePair<string, bool> item in this.openQueue)
            {
                if (Path.Exists(item.Key))
                {
                    exist.Add(item.Key);
                    this.openFile(item.Key, item.Value);
                }

            }

            foreach (string item in exist)
            {
                this.openQueue.Remove(item);
            }
        }

        private void closeCheckProcessing()
        {
            if (this.sld == null)
            {
                return;
            }
            var value = this.sld.EnumDocuments2();

            List<string> openDocNames = new List<string>();

            if (value != null)
            {
                ModelDoc2 model;

                while (1 == 1)
                {
                    int fetched = 0;
                    value.Next(1, out model, ref fetched);

                    if (model == null)
                    {
                        break;
                    }
                    try
                    {
                        openDocNames.Add(Path.GetFileName(model.GetPathName()));
                    }
                    catch (System.Runtime.InteropServices.COMException e)
                    {

                    }
                    

                }
            }

            // loop and remove
            // if value was null this will be all
            foreach (KeyValuePair<string, string> trackedFile in this.trackedFiles)
            {
                string name = trackedFile.Key;

                if (!openDocNames.Contains(name))
                {
                    this.removeClosedDocument(name);
                }
            }
        }

        private void removeClosedDocument(string name)
        {
            this.autoSync[name] = true;
            this.taskPaneView.removeDocEntry(name);

            this.trackedFiles.Remove(name);
            this.autoSync.Remove(name);
        }

        private void TimerTrackedEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            this.debugTimerEvents();

            this.saveQueueProcessing();

            this.openQueueProcessing();

            this.closeCheckProcessing();

        }


        private void debugTimerEvents()
        {
            Debug.WriteLine("save queue: " + this.saveQueue.Count.ToString());
            foreach (string item in this.saveQueue)
            {
                Debug.WriteLine("    -" + item);
            }

            Debug.WriteLine("open queue: " + this.saveQueue.Count.ToString());
            foreach (KeyValuePair<string, bool> item in this.openQueue)
            {
                Debug.WriteLine("    -" + item.Key);
            }



            Debug.WriteLine("tracked files: " + this.trackedFiles.Count.ToString());
            foreach (KeyValuePair<string, string> item in this.trackedFiles)
            {
                Debug.WriteLine("    -" + item.Key + "    | " + item.Value);
            }

            Debug.WriteLine("");
        }




        private string getFileDependents(string name)
        {
            dynamic rJSON = new JObject();
            rJSON.relationCount = 0;
            rJSON.relations = new JArray();

            string modelname = Path.GetFileNameWithoutExtension(name);

            var dep = this.docM.getDependents(modelname);
            if (dep != null)
            {
                Dictionary<string, Dictionary<string, string>> dependents = dep;

                int num = 0;
                foreach (KeyValuePair<string, Dictionary<string, string>> depSingle in dependents)
                {
                    rJSON.relations.Add(depSingle.Value["name"]);
                    num++;
                }
                rJSON.relationCount = num;
            }


            return JsonConvert.SerializeObject(rJSON);
        }

        private void fileSaved(string name)
        {
            //name is filename
            if (!this.isAPIConnected())
            {
                return;
            }

            if (this.autoSync.ContainsKey(name))
            {
                if (this.autoSync[name] == false)
                {
                    this.taskPaneView.updateDocEntry(name, this.lfm.getLocalFileVersion(name), 1);
                    return;
                } else
                {
                    string fullPath = this.lfm.getFullPathFromName(name);
                    bool res = this.createPreview(fullPath, fullPath + ".BMP", false);

                    
                    string dependents = this.getFileDependents(name);
                    Debug.WriteLine(dependents);

                    if (res)
                    {
                        this.lfm.uploadFile(name, dependents);
                    }

                    this.update();
                    //status 0 good
                    //status 1 new
                    //status 2 old
                    this.taskPaneView.updateDocEntry(name, this.lfm.getLocalFileVersion(name), 0);
                }

            }

            
        }





        private void log(string text)
        {
            Debug.WriteLine(text);
        }

        private bool isAPIConnected()
        {
            if (!apiConnected)
            {
                MessageBox.Show("Not connected to API!!");
                return false;
            } else
            {
                return true;
            }
        }

        private bool isSolidConnected()
        {
            if (!swConnected)
            {
                MessageBox.Show("Not connected to SolidWorks!!");
                return false;
            }
            else
            {
                return true;
            }
        }













        
        private void addTrackedFile(string name, int status)
        {
            
            string? version = this.lfm.getLocalFileVersion(name);

            if (version == null)
            {
                version = "0";
            }
            this.trackedFiles.Add(name, version);
            this.autoSync.Add(name, true);
            this.taskPaneView.addDocEntry(name, version, status);
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

        private void uploadFile()
        {
            if (!this.isAPIConnected())
            {
                return;
            }

            if (this.currentProject != "")
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
                        //this.lfm.serverUploadNewFile(this.currentProject, fullPath, imPath);
                    }


                }              
            }
        }


        





        









        /**
         * PROJECT CONTEXT MENU ACTIONS
         */

        private bool contextMenuUploadAll()
        {


            return true;
        }

        private bool contextMenuCloseAll()
        {


            return true;
        }




        

        private void openExistingFile(string name, bool ro)
        {
            //try
           // {
                string openPath = this.lfm.getFullPathFromName(name);
                this.openQueue.Add(openPath, ro);
            //}
           // catch (System.ArgumentException e)
            //{

            //}
            
        }

        
        private void openTreeProject(bool ro)
        {
            foreach (string filename in this.tree[this.currentSelectedProject])
            {
                this.openFileAndPrompt(filename, ro, false);
            }
        }
        private void downloadTreeProject()
        {
            foreach (string a in this.tree[this.currentSelectedProject])
            {
                this.downloadTreeFile(a);
            }
        }

        private void downloadTreeFile(string name)
        {
            string openPath = this.lfm.getFullPathFromName(name);

            if(!lfm.downloadfile(name))
            {
                MessageBox.Show("Error downloading: " + name);
                return;
            }
            this.updateLocal();

        }
        




        private void openFileAndPrompt(string filename, bool ro, bool prompt)
        {
            if (this.lfm == null)
            {
                return;
            }
            
            if (!this.lfm.testForLocalFile(filename))
            {
                if (prompt)
                {
                    DialogResult res = MessageBox.Show("File does not exist locally, download?", "", MessageBoxButtons.YesNo);

                    if (res != DialogResult.Yes)
                    {
                        return;
                    }
                }
                this.downloadTreeFile(filename);
                
            }
            this.openExistingFile(filename, ro);
        }


        private void openFile(string openPath, bool ro)
        {
            string ext = Path.GetExtension(openPath);
            string modelname = Path.GetFileNameWithoutExtension(openPath);
            string filename = Path.GetFileName(openPath);

            this.docM.openDoc(openPath, ro);
            ModelDoc2 newModel = this.docM.getModelFromName(modelname);

            bool result = ext switch
            {
                ".SLDPRT" => this.attachPartEvents(newModel),
                ".SLDASM" => this.attachAssemblyEvents(newModel),
                ".SLDDRW" => this.attachDrawingEvents(newModel),
                _ => false
            };


            int? status = this.lfm.getFileStatus(filename);
            if (status.HasValue)
            {
                this.addTrackedFile(filename, (int)status);
                this.createEvent();
                Debug.WriteLine(this.docM.getDependents(modelname));
            }
        }





        private void refreshPreview()
        {
            string urlData = this.url;
            urlData += "preview/" + this.currentSelectedProject + "/" + this.currentFile;

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
            catch (System.ArgumentException)
            {
                Debug.WriteLine("fatal");
            }
        }








        //----------------------------------PROJECT TREE LOGIC----------------------------------//
        private void updateProjectTree()
        {
            projectTreeView.BeginUpdate();
            projectTreeView.Nodes.Clear();

            foreach (KeyValuePair<string, List<string>> entry in this.tree)
            {
                TreeNode node = new TreeNode(entry.Key);
                node.ForeColor = this.greenColor;
                if (entry.Key == this.currentSelectedProject)
                {
                    node.NodeFont = new Font("Calibri", 16, FontStyle.Bold);
                }
                else
                {
                    node.NodeFont = new Font("Calibri", 16);
                }

                projectTreeView.Nodes.Add(node);
            }

            projectTreeView.EndUpdate();
        }


        //----------------------------------PROJECT TREE VIEW LOGIC----------------------------------//
        private void projectTreeSelected(TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                currentSelectedProject = e.Node.Text;

                this.updateFileTree();
                //this.updateProjectTree();
            }
        }
        private void projectTreeClicked(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        var coordinates = new Point(Cursor.Position.X - this.Left, Cursor.Position.Y - this.Top);
                        projectContextMenuStrip.Show(this, new Point(coordinates.X - 20, coordinates.Y - 40));
                    }
                    break;
            }
        }
        private void projectTreeProjectSelected(EventArgs e)
        {
            currentProject = currentSelectedProject;
            currentProjectLabel.Text = currentProject;

            this.updateProjectTree();
        }


        private Color[] status = { Color.FromArgb(42, 161, 152), Color.FromArgb(181, 137, 0), Color.FromArgb(220, 50, 47) };

        //----------------------------------FILE TREE LOGIC----------------------------------//
        private void updateFileTree()
        {
            if (this.currentSelectedProject != null)
            {
                if (tree.ContainsKey(this.currentSelectedProject))
                {
                    fileTreeView.BeginUpdate();
                    fileTreeView.Nodes.Clear();

                    Dictionary<string, string> nodeTypes = new Dictionary<string, string>();
                    nodeTypes.Add("Parts", ".SLDPRT");
                    nodeTypes.Add("Assemblies", ".SLDASM");
                    nodeTypes.Add("Drawings", ".SLDDRW");

                    Dictionary<string, TreeNode> topNodes = new Dictionary<string, TreeNode>();

                    foreach(KeyValuePair<string, string> defNode in nodeTypes)
                    {
                        topNodes.Add(defNode.Value, new TreeNode(defNode.Key));

                        this.fileTreeView.Nodes.Add(topNodes[defNode.Value]);
                    }


                    foreach (var file in tree[this.currentSelectedProject])
                    {
                        TreeNode node = new TreeNode(file);

                        int? st = this.lfm.getFileStatus(file);
                        if (st != null)
                        {
                            node.ForeColor = status[(int)st];
                        }

                        string ext = Path.GetExtension(file);
                        topNodes[ext].Nodes.Add(node);

                    }

                    fileTreeView.EndUpdate();
                }
            }

        }


        //----------------------------------FILE TREE VIEW EVENT LOGIC----------------------------------//
        private void fileTreeSelected(TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                this.currentFile = e.Node.Text;
                currentFileLabel.Text = currentFile;

                this.refreshPreview();
            }
        }
        private void fileTreeClicked(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        var coordinates = new Point(Cursor.Position.X - this.Left, Cursor.Position.Y - this.Top);
                        fileContextMenuStrip.Show(this, new Point(coordinates.X - 20, coordinates.Y - 40));
                    }
                    break;
            }
        }







        private void TimerCheckDocEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            


        }





        private void createEvent()
        {
            this.sld = (SldWorks)this.app;
            this.sld.ActiveDocChangeNotify += this.SldWorks_ActiveDocChangeNotify;
            this.sld.DocumentLoadNotify2 += this.SldWorks_DocumentLoadNotify;
            this.sld.FileNewNotify2 += this.SldWorks_FileNewNotify;
            //this.sld.FileCloseNotify += this.SldWorks_FileCloseNotify;
        }

        private int SldWorks_ActiveDocChangeNotify()
        {
            log("Event: SldWorks: Document Changed");

            if (this.checkDocFlag == false)
            {
                this.checkDocFlag = true;
            }
            

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








        //----------------------------------MAIN BUTTON LOGIC----------------------------------//
        private bool initAPIConnect(string url, string user, string localHead)
        {
            if (this.lfm != null)
            {
                bool result = this.lfm.connectToServer(url, user, localHead);

                if (result)
                {
                    this.apiConnected = true;
                    this.update();
                    this.cloudConnectLabel.BackColor = this.greenColor;
                    this.apiConnectButton.ForeColor = this.redColor;
                } else {
                    this.cloudConnectLabel.BackColor = this.redColor;
                    this.apiConnectButton.ForeColor = this.greenColor;
                }

                return result;
            } else
            {
                return false;
            }
            
        }
        private void initSolidConnect()
        {
            if (this.pid == "")
            {
                return;
            }
            bool res = this.swC.connectToProcess(Int32.Parse(this.pid));
            this.swConnected = res;

            if (this.swConnected == true)
            {
                this.docM = new SW_DocMgr(this.swC);
                this.app = this.swC.getApp();
                this.sld = (SldWorks)this.app;

                

                this.createTaskPane(this.app);
                //this.connectAppEventHandlers();
                

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
        private void createNewProject()
        {
            if (!this.isAPIConnected())
            {
                return;
            }

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

                this.lfm.createProject(settings);

                this.update();
            }
        }
        private void createNewFile()
        {
            if (!this.isSolidConnected() || this.currentProject == "" || this.lfm == null)
            {
                return;
            }

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
                string fileTemplate = newFileDialog.fileTemplate;



                if (fileName != "" && fileTemplate != null)
                {
                    string fileOutputName = this.localHead + currentProject + @"\" + fileName;

                    this.lfm.createFileFromTemplate(fileTemplate, fileOutputName, false);

                    this.docM.openDoc(fileOutputName, false);

                    ModelDoc2 newModel = this.docM.getModelFromName(Path.GetFileNameWithoutExtension(fileOutputName));



                    this.addTrackedFile(Path.GetFileName(fileOutputName), 1);


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


























        /**
         * 
         * RIGHT CLICK CONTEXT MENU EVENTS
         * 
         */

        // Project Context Menu
        private void projectContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                Debug.WriteLine(e.ClickedItem.Text);


                switch (e.ClickedItem.Text)
                {
                    case "Download All":
                        this.downloadTreeProject();
                        break;
                    case "Upload All":
                        contextMenuUploadAll();
                        break;
                    case "Open All":
                        this.openTreeProject(false);
                        break;
                    case "Read Only":
                        MessageBox.Show("readonlyy");
                        break;
                    case "Close All":
                        contextMenuCloseAll();
                        break;
                }
            }

        }

        // File Context Menu
        

        private void fileContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                Debug.WriteLine(e.ClickedItem.Text);


                switch (e.ClickedItem.Text)
                {
                    case "Open":
                        //contextMenuDownloadAll();
                        this.openFileAndPrompt(this.currentFile, false, true);
                        break;
                    case "Download":
                        //contextMenuDownloadAll();
                        this.downloadTreeFile(this.currentFile);
                        break;
                    case "Sync":
                        //contextMenuUploadAll();
                        break;
                    case "Merge":
                        //contextMenuOpenAll();
                        break;
                    case "Read Only":
                        //this.openTreeFile(true);
                        break;
                    case "Close":
                        //contextMenuCloseAll();
                        break;
                }
            }
        }


        












        //----------------------------------PROJECT TREE VIEW EVENTS----------------------------------//
        private void projectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.projectTreeSelected(e);
        }
        private void projectTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            this.projectTreeClicked(e);
        }
        private void projectTreeView_DoubleClick(object sender, EventArgs e)
        {
            this.projectTreeProjectSelected(e);
        }










        //----------------------------------FILE TREE VIEW EVENTS----------------------------------//
        private void fileTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.fileTreeSelected(e);
        }
        private void fileTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            this.fileTreeClicked(e);
        }














        //----------------------------------MAIN BUTTON EVENTS----------------------------------//
        private void apiConnectButton_Click(object sender, EventArgs e)
        {
            if (!this.apiConnected)
            {
                this.initAPIConnect(this.url, this.user, this.localHead);
                this.update();
            }
            else
            {
                this.apiConnected = false;
                this.cloudConnectLabel.BackColor = this.redColor;
                this.apiConnectButton.ForeColor = this.greenColor;
            }

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
                this.initSolidConnect();
            }

        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            this.uploadFile();
        }
        private void settingsButton_Click(object sender, EventArgs e)
        {
            this.launchSettingsDialog();
        }
        private void updateButton_Click(object sender, EventArgs e)
        {
            this.update();
        }
        private void newProjectButton_Click(object sender, EventArgs e)
        {
            this.createNewProject();
        }
        private void newButton_Click(object sender, EventArgs e)
        {
            this.createNewFile();
        }

        private void ApplicationClose(Object sender, FormClosingEventArgs e)
        {
            if (swTaskPane != null)
            {
                swTaskPane.DeleteView();
            }
        }



        private Dictionary<string, bool> autoSync = new Dictionary<string, bool>();


        private void taskAutoSyncChanged(object sender, EventArgs args, string name, bool value)
        {
            if (this.autoSync.ContainsKey(name))
            {
                this.autoSync[name] = value;
            }
        }
        private void taskNameClicked(object sender, EventArgs args, string name)
        {
            this.docM.activate(Path.GetFileNameWithoutExtension(name));
        }



        //----------------------------------TASKPANE----------------------------------//
        void createTaskPane(ISldWorks app)
        {
            string[] a = { this.executableDirectory, "small.ico" };
            string bitmap = Path.Combine(a);
            string toolTip = "API Manager";
            string ctrlName = "API.Manager";
            string ctrlLicKey = "";

            swTaskPane = (TaskpaneView)app.CreateTaskpaneView2(bitmap, toolTip);

            this.taskPaneView = new Taskpane();
            this.taskPaneView.exportSTLButton.Click += this.exportSTLButton;
            this.taskPaneView.exportParasolidButton.Click += this.exportParasolidButton;
            this.taskPaneView.exportSTEPButton.Click += this.exportSTEPButton;
            this.taskPaneView.exportDXFButton.Click += this.exportDXFButton;


            this.taskPaneView.AutoSyncChanged += this.taskAutoSyncChanged;
            this.taskPaneView.NameClicked += this.taskNameClicked;
            swTaskPane.DisplayWindowFromHandlex64(this.taskPaneView.getHandle());

            swTaskPane.TaskPaneActivateNotify += swTaskPane_TaskPaneActivateNotify;
            swTaskPane.TaskPaneDestroyNotify += swTaskPane_TaskPaneDestroyNotify;
            swTaskPane.TaskPaneToolbarButtonClicked += swTaskPane_TaskPaneToolbarButtonClicked;

        }

        //----------------------------------TASKPANE EVENTS----------------------------------//
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
        private void exportSTLButton(object sender, EventArgs e)
        {
            this.docM.exportActiveDoc("-" + this.getDateTimeString(), "STL");
        }
        private void exportParasolidButton(object sender, EventArgs e)
        {
            this.docM.exportActiveDoc("-" + this.getDateTimeString(), "x_t");
        }
        private void exportSTEPButton(object sender, EventArgs e)
        {
            this.docM.exportActiveDoc("-" + this.getDateTimeString(), "STEP");
        }
        private void exportDXFButton(object sender, EventArgs e)
        {
            this.docM.exportDWG("-" + this.getDateTimeString());
        }

        private string getDateTimeString()
        {
            return DateTime.Now.ToString("MMddyyyy-hmm");
        }








        //----------------------------------DOCUMENT EVENTS----------------------------------//

        private int PartDocFileSavePostNotify(string name)
        {
            this.saveQueue.Add(Path.GetFileNameWithoutExtension(name) + ".SLDPRT");
            //return 1 if you don't want to save
            return 0;
        }
        private int AssemblyDocFileSavePostNotify(string name)
        {
            this.saveQueue.Add(Path.GetFileNameWithoutExtension(name) + ".SLDASM");
            return 0;
        }
        private int DrawingDocFileSavePostNotify(string name)
        {
            this.saveQueue.Add(Path.GetFileNameWithoutExtension(name) + ".SLDDRW");
            return 0;
        }


        private bool attachPartEvents(ModelDoc2 model)
        {
            PartDoc doc = (PartDoc)model;
            doc.FileSaveNotify += this.PartDocFileSavePostNotify;
            return true;
        }

        private bool attachAssemblyEvents(ModelDoc2 model)
        {
            AssemblyDoc doc = (AssemblyDoc)model;
            doc.FileSaveNotify += this.AssemblyDocFileSavePostNotify;

            return true;
        }

        private bool attachDrawingEvents(ModelDoc2 model)
        {
            DrawingDoc doc = (DrawingDoc)model;
            doc.FileSaveNotify += this.DrawingDocFileSavePostNotify;
            return true;
        }








        //----------------------------------SETTINGS----------------------------------//



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
            }
            else
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
                var temp = o1.template;
                var pid = o1.pid;

                bool result = (address != null) && (user != null) &&
                              (root != null) && (pid != null) && (temp != null);

                if (result)
                {
                    this.url = address;
                    this.user = user;
                    this.localHead = root + "\\";
                    this.templateDir = temp + "\\";
                    this.pid = pid;

                    settingsLoaded = true;
                    this.lfm = new LocalFileManage(this.localHead, this.templateDir);

                }
                else
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

            bool res = this.initAPIConnect(url, user, localHead);

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




        //Form load event
        private void s(object sender, EventArgs e)
        {

        }

        private void projectTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }
        
    }
}







