using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;

using FileManager;
using System.Security.Policy;
using Newtonsoft.Json.Linq;



using SWLib;



namespace GUILayoutTest1
{
    public partial class Form1 : Form
    {
        private API api = null;

        

        private string url = "";
        private string user = "";
        private string localHead = "";
        private string pid = "";


        private string templateRoot = @"D:\School\4610\sld resource files\";

        private string currentProject;
        private string currentFile;

        private Dictionary<string, List<string>> tree = new Dictionary<string, List<string>>();
        private Dictionary<string, string> projectRoots = new Dictionary<string, string>();




        // solidworks api variables
        SW_Instance swC = new SW_Instance();
        SW_DocMgr docM = null;

        bool swConnected = false;

        public Form1()
        {
            InitializeComponent();

            //for testing
            dynamic o1 = JObject.Parse(File.ReadAllText(@"C:\Users\wes\github-repos\ccet4610_project\test-prog-settings.json"));
            string address = o1.address;

            if (address != null)
            {
                this.url = address;
            }

            string user = o1.user;
            if (user != null)
            {
                this.user = user;
            }

            string root = o1.root;
            if (root != null)
            {
                this.localHead = root;
            }

            string pid = o1.pid;
            if (pid != null)
            {
                this.pid = pid;
            }

            this.connectToServer(this.url, this.user, this.localHead);
            this.update();

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
            } else
            {
                MessageBox.Show("Error Connecting");
            }
            

        }
        
        private bool connectToServer(string url, string user, string localHead)
        {
            try
            {
                this.api = new API(url, localHead, user);
                bool res = api.testConnection();

                if (res == true)
                {                    
                    MessageBox.Show("Connection to API successful");
                    this.update();
                    return true;
                } else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
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

        private void update()
        {
            if (api != null)
            {
                tree = new Dictionary<string, List<string>>();



                var res2 = api.listProjects();
                foreach (var project in res2)
                {
                    List<string> data = new List<string>();
                    string name = project.name;

                    foreach (var file in project.files)
                    {
                        string filename = file;
                        data.Add(filename);
                    }

                    tree.Add(name, data);
                }


                this.updateProjects();
                this.updateFiles();
            }
            else
            {
                this.connectToServer(this.url, this.user, this.localHead);
            }
        }

        private void updateFiles()
        {
            if (this.currentProject != null)
            {
                if (tree.ContainsKey(this.currentProject))
                {
                    fileTreeView.BeginUpdate();
                    fileTreeView.Nodes.Clear();

                    foreach (var file in tree[this.currentProject])
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
                currentProject = e.Node.Text;
                currentProjectLabel.Text = currentProject;
                this.updateFiles();
            }
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
            if (this.checkSW() == true)
            {
                //this.docM.newDoc("prt", @"C:\Users\Initec\source\github\ccet4610_project\testFiles\create\testNew");

                
            }
        }

        private bool createPreview(string filePath, string imPath)
        {
            string modelName = docM.openDoc(filePath, false);

            docM.savePreviewBMP(modelName, imPath, 1920, 1080);
            docM.close(modelName);

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
                    bool res = this.createPreview(fullPath, imPath);

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

        private void solidSettingsButton_Click(object sender, EventArgs e)
        {
            var sldwrkSettings = new SolidworksSettings(this.pid);

            sldwrkSettings.ShowDialog();

            var goodBack = Color.FromArgb(133, 153, 0);

            var startBack = Color.FromArgb(181, 137, 0);

            var badBack = Color.FromArgb(220, 50, 47);

            if (sldwrkSettings.create == true)
            {
                //start
                this.pid = this.swC.startSW().ToString();
            
                this.swConnected = false;

            } else if (sldwrkSettings.pid != 0)
            {
                //connect
                bool res = this.swC.connectToProcess(sldwrkSettings.pid);

                if (res == true)
                {
                    this.swConnected = true;
                } else
                {
                    MessageBox.Show("Error attaching to PID");
                    this.swConnected = false;
                }
                
            } else
            {
                this.swConnected = false;
            }



            if (this.swConnected == true)
            {
                this.solidSettingsButton.BackColor = goodBack;
                this.docM = new SW_DocMgr(this.swC);
            } else if (this.pid != "")
            {
                this.solidSettingsButton.BackColor = startBack;
            }
            
            else
            {
                this.solidSettingsButton.BackColor = badBack;
            }

        }
    }
}