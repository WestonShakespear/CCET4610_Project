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
    public partial class TestForm : Form
    {

        private Dictionary<string, Label> names = new Dictionary<string, Label>();
        private Dictionary<string, Label> versions = new Dictionary<string, Label>();
        private Dictionary<string, CheckBox> buttons = new Dictionary<string, CheckBox>();

        private Dictionary<string, int> reference = new Dictionary<string, int>();

        private int currentRow = 1;


        private Color statusColorError = Color.FromArgb(220, 50, 47);
        private Color statusColorWarning = Color.FromArgb(181, 137, 0);
        private Color statusColorGood = Color.FromArgb(133, 153, 0);

        public TestForm()
        {
            InitializeComponent();
        }



        public void addDocEntry(string name)
        {
            names.Add(name, new Label());
            names[name].Text = name;
            names[name].Dock = DockStyle.Fill;

            versions.Add(name, new Label());
            versions[name].Text = "0";
            versions[name].Dock = DockStyle.Fill;
            versions[name].BackColor = this.statusColorWarning;

            buttons.Add(name, new CheckBox());
            buttons[name].Dock = DockStyle.Fill;

            reference.Add(name, this.currentRow);


            this.activeDocsTableLayout.Controls.Add(names[name], 0, this.currentRow);
            this.activeDocsTableLayout.Controls.Add(versions[name], 1, this.currentRow);
            this.activeDocsTableLayout.Controls.Add(buttons[name], 2, this.currentRow);


            this.currentRow++;
        }
    }
}
