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
    public partial class SolidworksSettings : Form
    {
        public int pid;
        public bool create;

        public SolidworksSettings(string pid)
        {
            InitializeComponent();

            if (pid == "")
            {
                this.pid = 0;
            } else
            {
                this.pid = Int32.Parse(pid);
                this.pidTextBox.Text = pid;
                this.connectButton.Enabled = true;
                this.createButton.Enabled = false;
            }
            this.create = false;


        }

        private void createButton_Click(object sender, EventArgs e)
        {
            this.create = true;
            this.Close();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (pidTextBox.Text != "")
            {
                try
                {
                    this.pid = Int32.Parse(pidTextBox.Text);
                } catch (FormatException)
                {
                    this.pid = 0;
                    MessageBox.Show("Error in PID format");
                }

                this.Close();
            }
            
        }

        private void pidTextBox_Leave(object sender, EventArgs e)
        {
            if (pidTextBox.Text != "")
            {
                connectButton.Enabled = true;
            } else
            {
                connectButton.Enabled = false;
                createButton.Enabled = true;
            }
        }
    }
}
