using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using Microsoft.VisualBasic.ApplicationServices;
using System.Net;

using FileManager;
using System.Security.Policy;
using Newtonsoft.Json.Linq;



namespace GUILayoutTest1
{
    public partial class Form1 : Form
    {
        private API api = null;

        

        private string url = "http://127.0.0.1:5000/";
        private string user = "weston";
        private string localHead = @"D:\School\4610\API\local\";

        private string currentProject;
        private string currentFile;

        private Dictionary<string, List<string>> tree = new Dictionary<string, List<string>>();
        public Form1()
        {
            InitializeComponent();
        }

        private void s(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Debug.Write("done");
            string connectionString = "Data Source=database.db;";
            string sql = "SELECT * FROM References";
            SQLiteConnection sqlite_conn = new SQLiteConnection(connectionString);
       
            sqlite_conn.Open();
            SQLiteCommand comm = new SQLiteCommand("Select * From [Ref]", sqlite_conn);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    dataGridView1.Rows.Add(new object[] {
            read.GetValue(0),  // U can use column index
            read.GetValue(read.GetOrdinal("PatientName")),  // Or column name like this
            read.GetValue(read.GetOrdinal("PatientAge")),
            read.GetValue(read.GetOrdinal("PhoneNumber"))
            });
                }
            }
            sqlite_conn.Close();


            Debug.Write("done");
        


           
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            var settings = new Settings(this.url, this.localHead, this.user);
            settings.ShowDialog();

            //Debug.WriteLine("window closed");

            string url = settings.address;
            string user = settings.user;
            string localHead = settings.localHead;

                
            try
            {

                this.api = new API(url, localHead, user);
                bool res = api.testConnection();

                if (res == true)
                {
                    this.url = url;
                    this.user = user;
                    this.localHead = localHead;

                    MessageBox.Show("Connection to API successful");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error");

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
                MessageBox.Show("Not connected to API!!");
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
                currentFile = e.Node.Text;
                currentFileLabel.Text = currentFile;
                this.updateSelected();
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {

        }

        private void uploadButton_Click(object sender, EventArgs e)
        {

        }
    }
}