namespace GUILayoutTest1
{
    partial class SolidworksSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.programPIDGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pidTextBox = new System.Windows.Forms.TextBox();
            this.pidLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.createButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.programPIDGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.programPIDGroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 250);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // programPIDGroupBox
            // 
            this.programPIDGroupBox.Controls.Add(this.tableLayoutPanel3);
            this.programPIDGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.programPIDGroupBox.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.programPIDGroupBox.Location = new System.Drawing.Point(3, 3);
            this.programPIDGroupBox.Name = "programPIDGroupBox";
            this.programPIDGroupBox.Size = new System.Drawing.Size(794, 196);
            this.programPIDGroupBox.TabIndex = 3;
            this.programPIDGroupBox.TabStop = false;
            this.programPIDGroupBox.Text = "SldWrks Instance:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.Controls.Add(this.pidTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pidLabel, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(788, 170);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // pidTextBox
            // 
            this.pidTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.pidTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pidTextBox.Location = new System.Drawing.Point(239, 3);
            this.pidTextBox.Name = "pidTextBox";
            this.pidTextBox.Size = new System.Drawing.Size(546, 22);
            this.pidTextBox.TabIndex = 2;
            this.pidTextBox.Leave += new System.EventHandler(this.pidTextBox_Leave);
            // 
            // pidLabel
            // 
            this.pidLabel.AutoSize = true;
            this.pidLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pidLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pidLabel.Location = new System.Drawing.Point(3, 0);
            this.pidLabel.Name = "pidLabel";
            this.pidLabel.Size = new System.Drawing.Size(230, 14);
            this.pidLabel.TabIndex = 3;
            this.pidLabel.Text = "PID:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.createButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.connectButton, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 205);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(794, 42);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // createButton
            // 
            this.createButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createButton.Location = new System.Drawing.Point(3, 3);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(391, 36);
            this.createButton.TabIndex = 0;
            this.createButton.Text = "Start";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connectButton.Enabled = false;
            this.connectButton.Location = new System.Drawing.Point(400, 3);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(391, 36);
            this.connectButton.TabIndex = 1;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // SolidworksSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 250);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SolidworksSettings";
            this.Text = "SolidworksSettings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.programPIDGroupBox.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox programPIDGroupBox;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox pidTextBox;
        private Label pidLabel;
        private TableLayoutPanel tableLayoutPanel4;
        private Button createButton;
        private Button connectButton;
    }
}