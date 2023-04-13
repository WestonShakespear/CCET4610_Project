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
    public partial class NewFileTypeDialog : Form
    {
        public string type = "";
        public NewFileTypeDialog(string currentProject)
        {
            InitializeComponent();
            this.Text = currentProject;
        }

        private void partButton_Click(object sender, EventArgs e)
        {
            this.type = "SLDPRT";
            this.Close();
        }

        private void assemblyButton_Click(object sender, EventArgs e)
        {
            this.type = "SLDASM";
            this.Close();
        }

        private void drawingButton_Click(object sender, EventArgs e)
        {
            this.type = "SLDDRW";
            this.Close();
        }
    }
}
