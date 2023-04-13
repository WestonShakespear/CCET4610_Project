using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUILayoutTest1
{
    public partial class NewFile : Form
    {
        List<string> templateList = new List<string>();

        public string fileName = "";

        public string fileTemplate = "";

        public string fileType = "";

        private string rootDir = "";
        private string project = "";


        public NewFile(string rootDir, string project, string type, List<string> templates)
        {
            InitializeComponent();
            this.rootDir = rootDir;
            this.templateList = templates;
            this.fileType = type;
            this.project = project;
            this.Text = this.project;

            foreach (var template in this.templateList)
            {
                templateComboBox.Items.Add(template);
            }
            templateComboBox.SelectedIndex = 0;

            this.selectFile();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            this.done();

        }



        private void done()
        {
            if (nameTextBox.Text != "")
            {
                this.fileName = nameTextBox.Text;
                this.fileTemplate = this.templateList[templateComboBox.SelectedIndex];

                string ext = Path.GetExtension(this.fileName);

                if (ext != "." + this.fileType)
                {
                    this.fileName = Path.GetFileNameWithoutExtension(this.fileName) + "." + this.fileType;
                }
                this.Close();
            }
            
        }


        private void nameTextBox_DoubleClick(object sender, EventArgs e)
        {
            this.selectFile();
        }

        private void selectFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = this.rootDir + this.project;


            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                nameTextBox.Text = saveFileDialog.FileName;
            }
        }
    }
}
