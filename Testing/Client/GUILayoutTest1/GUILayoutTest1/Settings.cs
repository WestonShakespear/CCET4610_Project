using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUILayoutTest1
{
    public partial class Settings : Form
    {
        public string address = "";
        public string user = "";
        public string path = "";
        public string pid = "";

        private string url = @"D:\";

        public Settings(string address, string user, string path, string pid)
        {
            InitializeComponent();
            this.address = address;
            this.user = user;
            this.path = path;
            this.pid = pid;

        }

        private void updateValues()
        {
            this.address = addressTextBox.Text;
            this.user = userTextBox.Text;
            this.path = pathTextBox.Text;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            this.updateValues();
            this.Close();
        }

        private void pathTextBox_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog rootFolder = new FolderBrowserDialog();
            DialogResult result = rootFolder.ShowDialog();

            if (result == DialogResult.OK)
            {
                pathTextBox.Text = rootFolder.SelectedPath;
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog uploadFileDialog = new OpenFileDialog();
            DialogResult result = uploadFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                dynamic o1 = JObject.Parse(File.ReadAllText(uploadFileDialog.FileName));
                string address = o1.address;

                if (address != null)
                {
                    this.addressTextBox.Text = address;
                }

                string user = o1.user;
                if (user != null)
                {
                    this.userTextBox.Text = user;
                }

                string root = o1.root;
                if (root != null)
                {
                    this.pathTextBox.Text = root;
                }
            }

            this.updateValues();
            //this.Close();
        }
    }
}
