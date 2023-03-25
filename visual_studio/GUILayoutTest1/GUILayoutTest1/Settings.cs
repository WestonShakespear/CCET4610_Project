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
    public partial class Settings : Form
    {
        private string currentAddress = "";
        private string currentUser = "";
        private string currentPath = "";

        public string address = "";
        public string user = "";
        public string localHead = "";

        public Settings()
        {
            InitializeComponent();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            address = currentAddress;
            user = currentUser;
            localHead = currentPath;

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            this.currentAddress = addressTextBox.Text;
        }

        private void userTextBox_Leave(object sender, EventArgs e)
        {
            this.currentUser = userTextBox.Text;
        }

        private void pathTextBox_Leave(object sender, EventArgs e)
        {
            this.currentPath = pathTextBox.Text;
        }
    }
}
