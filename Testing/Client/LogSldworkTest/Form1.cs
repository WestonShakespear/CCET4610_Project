using System;
using System.Windows.Forms;

using SWLib;


namespace LogSldworkTest
{
    public partial class Form1 : Form
    {

        private string path = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";

        private SW_Instance swC;
        private string[] args = { "" };
        public Form1()
        {
           swC = new SW_Instance();
            InitializeComponent();
            pathTextBox.Text = path;
            pidTextBox.Text = "52696";


        }

        private void log(string message)
        {
            textBox1.AppendText(message + "\r\n");
        }




        private void launchButton_Click(object sender, EventArgs e)
        {
            swC.startFromArgs(this.args);
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            
        }

        private void newPartButton_Click(object sender, EventArgs e)
        {

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
           
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            
        }
    }
    }




