using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GUILayoutTest1
{
    public partial class Taskpane : Component
    {
        public GroupBox mainGroupBox;
        public GroupBox exportGroupBox;

        private TableLayoutPanel mainLayoutPanel;

        private TableLayoutPanel macroTablePanel;
        private TableLayoutPanel fileTablePanel;
        private TableLayoutPanel fileHeadingTablePanel;
        private TableLayoutPanel fileInfoTablePanel;

        private TableLayoutPanel actionTablePanel;
        private TableLayoutPanel exportTablePanel;


        private Label filenameLabel;
        private Label versionLabel;
        private Label autoLabel;

        private Button[] filenameButtons = new Button[24];
        private Label[] versionLabels = new Label[24];
        private Button[] autoButtons = new Button[24];


        private Button syncButton;

        private Button exportSTLButton;
        private Button exportParasolidButton;
        private Button exportSTEPButton;
        private Button exportDXFButton;


        private Color statusNewFile = Color.FromArgb(220, 50, 47);



        private Color greenColor = Color.FromArgb(42, 161, 152);
        private Color redColor = Color.FromArgb(181, 137, 0);
        private Color orangeColor = Color.FromArgb(181, 137, 0);


        private Color backColor = Color.FromArgb(253, 246, 227);
        private Color foreColor = Color.FromArgb(0, 43, 54);



        private Dictionary<string, string> versions = new Dictionary<string, string>();
        private Dictionary<string, bool> auto = new Dictionary<string, bool>();

        private Dictionary<string, int> reference = new Dictionary<string, int>();
        private string[] backRef = new string[24];

        private int currentRow = 0;

        public delegate void AutoSyncHandler(object sender, EventArgs e, string name, bool value);
        public event AutoSyncHandler AutoSyncChanged;

        public delegate void NameClickHandler(object sender, EventArgs e, string name);
        public event NameClickHandler NameClicked;


        public Taskpane()
        {
            //InitializeComponent();

            //--
            this.mainGroupBox = new GroupBox();
            this.mainGroupBox.Dock = DockStyle.Fill;
            this.mainGroupBox.BackColor = backColor;
            this.mainGroupBox.ForeColor = foreColor;

            //--
            this.mainLayoutPanel = new TableLayoutPanel();
            this.mainLayoutPanel.Dock = DockStyle.Fill;
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.RowCount = 4;
            this.mainLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            this.mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            this.mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 3F));
            this.mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 7F));

            //--
            this.macroTablePanel = new TableLayoutPanel();
            this.macroTablePanel.Dock = DockStyle.Fill;
            this.macroTablePanel.ColumnCount = 1;
            this.macroTablePanel.RowCount = 6;
            this.macroTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            //--
            this.fileTablePanel = new TableLayoutPanel();
            this.fileTablePanel.Dock = DockStyle.Fill;
            this.fileTablePanel.ColumnCount = 1;
            this.fileTablePanel.RowCount = 2;
            this.fileTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.fileTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 4F));
            this.fileTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 96F));


            //--
            this.filenameLabel = new Label();
            this.filenameLabel.Text = "Filename";
            this.filenameLabel.Dock = DockStyle.Fill;
            this.filenameLabel.TextAlign = ContentAlignment.BottomCenter;

            this.versionLabel = new Label();
            this.versionLabel.Text = "Version";
            this.versionLabel.Dock = DockStyle.Fill;
            this.versionLabel.TextAlign = ContentAlignment.BottomCenter;

            this.autoLabel = new Label();
            this.autoLabel.Text = "Auto";
            this.autoLabel.Dock = DockStyle.Fill;
            this.autoLabel.TextAlign = ContentAlignment.BottomCenter;


            //--
            this.fileHeadingTablePanel = new TableLayoutPanel();
            this.fileHeadingTablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            this.fileHeadingTablePanel.Dock = DockStyle.Fill;
            this.fileHeadingTablePanel.ColumnCount = 3;
            this.fileHeadingTablePanel.RowCount = 1;
            this.fileHeadingTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.fileHeadingTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.fileHeadingTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            //--
            this.fileHeadingTablePanel.Controls.Add(filenameLabel, 0, 0);
            this.fileHeadingTablePanel.Controls.Add(versionLabel, 1, 0);
            this.fileHeadingTablePanel.Controls.Add(autoLabel, 2, 0);


            //--
            this.fileInfoTablePanel = new TableLayoutPanel();
            this.fileInfoTablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            this.fileInfoTablePanel.Dock = DockStyle.Fill;
            this.fileInfoTablePanel.ColumnCount = 3;
            this.fileInfoTablePanel.RowCount = 24;
            this.fileInfoTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.fileInfoTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.fileInfoTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));

            for (int i = 0; i < this.fileInfoTablePanel.RowCount;i++)
            {
                this.filenameButtons[i] = new Button();
                this.filenameButtons[i].Name = i.ToString();
                this.filenameButtons[i].Dock = DockStyle.Fill;
                this.filenameButtons[i].FlatStyle = FlatStyle.Flat;
                this.filenameButtons[i].FlatAppearance.BorderSize = 0;
                this.filenameButtons[i].Click += new EventHandler(this.FileButtonClicked);

                this.versionLabels[i] = new Label();
                this.versionLabels[i].Dock = DockStyle.Fill;
                this.versionLabels[i].TextAlign = ContentAlignment.MiddleCenter;
                this.versionLabels[i].ForeColor = this.backColor;
                this.versionLabels[i].Font = new Font("Times New Roman", 12, FontStyle.Bold);

                this.autoButtons[i] = new Button();
                this.autoButtons[i].Name = i.ToString();
                this.autoButtons[i].Dock = DockStyle.Fill;
                this.autoButtons[i].FlatStyle = FlatStyle.Flat;
                this.autoButtons[i].FlatAppearance.BorderSize = 0;
                this.autoButtons[i].Click += new EventHandler(this.AutoButtonClicked);



                this.fileInfoTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 4F));
                this.fileInfoTablePanel.Controls.Add(this.filenameButtons[i], 0, i);
                this.fileInfoTablePanel.Controls.Add(this.versionLabels[i], 1, i);
                this.fileInfoTablePanel.Controls.Add(this.autoButtons[i], 2, i);
            }
            





            //-- Add file sub tables to main file table
            this.fileTablePanel.Controls.Add(this.fileHeadingTablePanel, 0, 0);
            this.fileTablePanel.Controls.Add(this.fileInfoTablePanel, 0, 1);




            //--
            this.syncButton = new Button();
            this.syncButton.Text = "Sync";
            this.syncButton.Dock = DockStyle.Fill;

            //--
            this.actionTablePanel = new TableLayoutPanel();
            this.actionTablePanel.Dock = DockStyle.Fill;
            this.actionTablePanel.ColumnCount = 1;
            this.actionTablePanel.RowCount = 1;

            //--
            this.actionTablePanel.Controls.Add(this.syncButton, 0, 0);




            //-- export buttons
            this.exportSTLButton = new Button();
            this.exportParasolidButton = new Button();
            this.exportSTEPButton = new Button();
            this.exportDXFButton = new Button();

            this.exportSTLButton.Text = "STL";
            this.exportParasolidButton.Text = "Parasolid";
            this.exportSTEPButton.Text = "STEP";
            this.exportDXFButton.Text = "DXF (Sketch)";

            this.exportSTLButton.Dock = DockStyle.Fill;
            this.exportParasolidButton.Dock = DockStyle.Fill;
            this.exportSTEPButton.Dock = DockStyle.Fill;
            this.exportDXFButton.Dock = DockStyle.Fill;

            //-- export table
            this.exportTablePanel = new TableLayoutPanel();
            this.exportTablePanel.Dock = DockStyle.Fill;
            this.exportTablePanel.ColumnCount = 2;
            this.exportTablePanel.RowCount = 2;
            this.exportTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.exportTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.exportTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.exportTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));




            this.exportTablePanel.Controls.Add(this.exportSTLButton, 0, 0);
            this.exportTablePanel.Controls.Add(this.exportSTEPButton, 0, 1);
            this.exportTablePanel.Controls.Add(this.exportParasolidButton, 1, 0);
            this.exportTablePanel.Controls.Add(this.exportDXFButton, 1, 1);

            //-- 
            this.exportGroupBox = new GroupBox();
            this.exportGroupBox.Text = "Export";
            this.exportGroupBox.Dock = DockStyle.Fill;
            this.exportGroupBox.Controls.Add(this.exportTablePanel);








            // add all three tables to main table
            this.mainLayoutPanel.Controls.Add(this.macroTablePanel, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.fileTablePanel, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.actionTablePanel, 0, 2);
            this.mainLayoutPanel.Controls.Add(this.exportGroupBox, 0, 3);

            // add main table to main groupbox
            this.mainGroupBox.Controls.Add(this.mainLayoutPanel);

            
        }

        public long getHandle()
        {
            return this.mainGroupBox.Handle.ToInt64();
        }



        public void addDocEntry(string name, string version, int status)
        {

            versions.Add(name, version);
            reference.Add(name, this.currentRow);
            auto.Add(name, true);

            backRef[this.currentRow] = name;


            if (this.currentRow < 24)
            {
                this.filenameButtons[this.currentRow].Text = name;
                this.versionLabels[this.currentRow].Text = version;
                this.versionLabels[this.currentRow].BackColor = this.orangeColor;
            }

            this.updateLabel(this.currentRow, status);
            this.updateAutoButtons();
            this.currentRow++;
        }

        //status 0 good
        //status 1 new
        //status 2 old
        public void updateDocEntry(string name, string version, int status)
        {
            this.versions[name] = version;
            this.versionLabels[this.reference[name]].Text = version;

            int index = this.reference[name];

            this.updateLabel(index, status);
        }

        public void updateLabel(int index, int status)
        {
            this.versionLabels[index].BackColor = status switch
            {
                0 => this.greenColor,
                1 => this.orangeColor,
                _ => this.redColor
            };
        }

        public void removeDocEntry(string name)
        {

        }





        public void FileButtonClicked(object sender, EventArgs args)
        {
            if (sender != null)
            {
                Button? btn = sender as Button;
                if (btn != null)
                {
                    string name = this.backRef[Int32.Parse(btn.Name)];
                    this.NameClicked(sender, args, name);
                }
            }
            
            

        }

        public void AutoButtonClicked(object sender, EventArgs args)
        {
            if (sender != null)
            {
                Button? btn = sender as Button;
                if (btn != null)
                {
                    string name = this.backRef[Int32.Parse(btn.Name)];

                    this.auto[name] = !this.auto[name];
                    this.AutoSyncChanged(sender, args, name, this.auto[name]);

                    this.updateAutoButtons();


                }
            }
        }

        private void updateAutoButtons()
        {
            foreach (KeyValuePair<string, bool> autoCell in this.auto)
            {
                Color back = this.greenColor;

                if (!autoCell.Value)
                {
                    back = this.redColor;
                }
                int index = this.reference[autoCell.Key];
                this.autoButtons[index].BackColor = back;
            }
        }

        
    }
}
