namespace GUILayoutTest1
{
    partial class Settings
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
            this.apiGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.urlLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.fileSettings = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.apiGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.fileSettings.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.apiGroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.fileSettings, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(431, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // apiGroupBox
            // 
            this.apiGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.apiGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.apiGroupBox.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.apiGroupBox.Location = new System.Drawing.Point(3, 3);
            this.apiGroupBox.Name = "apiGroupBox";
            this.apiGroupBox.Size = new System.Drawing.Size(425, 196);
            this.apiGroupBox.TabIndex = 4;
            this.apiGroupBox.TabStop = false;
            this.apiGroupBox.Text = "API Settings:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Controls.Add(this.userTextBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.addressTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.urlLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.userLabel, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(419, 170);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // userTextBox
            // 
            this.userTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.userTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.userTextBox.Location = new System.Drawing.Point(128, 88);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(288, 22);
            this.userTextBox.TabIndex = 2;
            this.userTextBox.Leave += new System.EventHandler(this.userTextBox_Leave);
            // 
            // addressTextBox
            // 
            this.addressTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.addressTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.addressTextBox.Location = new System.Drawing.Point(128, 3);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(288, 22);
            this.addressTextBox.TabIndex = 2;
            this.addressTextBox.Leave += new System.EventHandler(this.addressTextBox_Leave);
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.urlLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.urlLabel.Location = new System.Drawing.Point(3, 0);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(119, 14);
            this.urlLabel.TabIndex = 3;
            this.urlLabel.Text = "IP Address:";
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.userLabel.Location = new System.Drawing.Point(3, 85);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(41, 14);
            this.userLabel.TabIndex = 4;
            this.userLabel.Text = "User:";
            // 
            // fileSettings
            // 
            this.fileSettings.Controls.Add(this.tableLayoutPanel3);
            this.fileSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileSettings.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fileSettings.Location = new System.Drawing.Point(3, 205);
            this.fileSettings.Name = "fileSettings";
            this.fileSettings.Size = new System.Drawing.Size(425, 196);
            this.fileSettings.TabIndex = 3;
            this.fileSettings.TabStop = false;
            this.fileSettings.Text = "File Settings:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.Controls.Add(this.pathTextBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.pathLabel, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(419, 170);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.pathTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pathTextBox.Location = new System.Drawing.Point(128, 3);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(288, 22);
            this.pathTextBox.TabIndex = 2;
            this.pathTextBox.Leave += new System.EventHandler(this.pathTextBox_Leave);
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pathLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.pathLabel.Location = new System.Drawing.Point(3, 0);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(119, 14);
            this.pathLabel.TabIndex = 3;
            this.pathLabel.Text = "Local Path:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.applyButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cancelButton, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 407);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(425, 40);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // applyButton
            // 
            this.applyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applyButton.Location = new System.Drawing.Point(3, 3);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(206, 34);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(215, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(207, 34);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.apiGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.fileSettings.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox apiGroupBox;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox userTextBox;
        private TextBox addressTextBox;
        private Label urlLabel;
        private Label userLabel;
        private GroupBox fileSettings;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox pathTextBox;
        private Label pathLabel;
        private TableLayoutPanel tableLayoutPanel4;
        private Button applyButton;
        private Button cancelButton;
    }
}