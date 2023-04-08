using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUILayoutTest1
{
    public partial class NewProject : Form
    {
        private string currentName = "";
        private string currentUnits = "";
        private string currentPrefix = "";
        private string currentSuffix = "";

        public string name = "";
        public string units = "";
        public string prefix = "";
        public string suffix = "";

        private string[] unitList = { "Millimeter", "Inch", "Meter" };


        public string[] templates = {"", "", ""};

        public NewProject()
        {
            InitializeComponent();
            foreach(var unit in unitList)
            {
                unitsComboBox.Items.Add(unit);
            }
           
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            currentName = nameTextBox.Text;
        }
        private void unitsComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            currentUnits = unitList[unitsComboBox.SelectedIndex];
            Debug.WriteLine("selected changed");
            Debug.WriteLine(currentUnits);


            for (int i = 0; i < templates.Length; i++)
            {
                if (this.templates[i] == "")
                {
                    this.templates[i] = currentUnits.ToLower();

                    // workaround add support for drawing templates
                    if (i == 2)
                    {
                        this.templates[i] += "_a landscape";
                    }
                }

                
                
            }

            
        }

        private void prefixTextBox_Leave(object sender, EventArgs e)
        {
            currentPrefix = prefixTextBox.Text;
        }

        private void suffixTextBox_Leave(object sender, EventArgs e)
        {
            currentSuffix = suffixTextBox.Text;
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            name = currentName;
            units = currentUnits;
            prefix = currentPrefix;
            suffix = currentSuffix;

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
