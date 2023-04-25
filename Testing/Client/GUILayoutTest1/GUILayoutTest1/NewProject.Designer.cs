namespace GUILayoutTest1
{
    partial class NewProject
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
            this.projectGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.partTemplateButton = new System.Windows.Forms.Button();
            this.assemblyTemplateButton = new System.Windows.Forms.Button();
            this.drawingTemplateButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.prefixTextBox = new System.Windows.Forms.TextBox();
            this.unitsLabel = new System.Windows.Forms.Label();
            this.prefixLabel = new System.Windows.Forms.Label();
            this.suffixTextBox = new System.Windows.Forms.TextBox();
            this.suffixLabel = new System.Windows.Forms.Label();
            this.unitsComboBox = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.projectGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.settingsGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // projectGroupBox
            // 
            this.projectGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.projectGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectGroupBox.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.projectGroupBox.Location = new System.Drawing.Point(3, 3);
            this.projectGroupBox.Name = "projectGroupBox";
            this.projectGroupBox.Size = new System.Drawing.Size(794, 196);
            this.projectGroupBox.TabIndex = 4;
            this.projectGroupBox.TabStop = false;
            this.projectGroupBox.Text = "Project:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.nameTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.nameLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(788, 170);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "Templates:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nameTextBox.Location = new System.Drawing.Point(239, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(546, 22);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.nameLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(230, 14);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name:";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel5.Controls.Add(this.partTemplateButton, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.assemblyTemplateButton, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.drawingTemplateButton, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(239, 88);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(546, 79);
            this.tableLayoutPanel5.TabIndex = 5;
            // 
            // partTemplateButton
            // 
            this.partTemplateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partTemplateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.partTemplateButton.Location = new System.Drawing.Point(3, 3);
            this.partTemplateButton.Name = "partTemplateButton";
            this.partTemplateButton.Size = new System.Drawing.Size(175, 73);
            this.partTemplateButton.TabIndex = 0;
            this.partTemplateButton.Text = "Part";
            this.partTemplateButton.UseVisualStyleBackColor = true;
            // 
            // assemblyTemplateButton
            // 
            this.assemblyTemplateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assemblyTemplateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.assemblyTemplateButton.Location = new System.Drawing.Point(184, 3);
            this.assemblyTemplateButton.Name = "assemblyTemplateButton";
            this.assemblyTemplateButton.Size = new System.Drawing.Size(176, 73);
            this.assemblyTemplateButton.TabIndex = 1;
            this.assemblyTemplateButton.Text = "Assembly";
            this.assemblyTemplateButton.UseVisualStyleBackColor = true;
            // 
            // drawingTemplateButton
            // 
            this.drawingTemplateButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingTemplateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.drawingTemplateButton.Location = new System.Drawing.Point(366, 3);
            this.drawingTemplateButton.Name = "drawingTemplateButton";
            this.drawingTemplateButton.Size = new System.Drawing.Size(177, 73);
            this.drawingTemplateButton.TabIndex = 2;
            this.drawingTemplateButton.Text = "Drawing";
            this.drawingTemplateButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.projectGroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.settingsGroupBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.tableLayoutPanel3);
            this.settingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsGroupBox.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.settingsGroupBox.Location = new System.Drawing.Point(3, 205);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(794, 196);
            this.settingsGroupBox.TabIndex = 3;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Settings:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.Controls.Add(this.prefixTextBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.unitsLabel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.prefixLabel, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.suffixTextBox, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.suffixLabel, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.unitsComboBox, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(788, 170);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // prefixTextBox
            // 
            this.prefixTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.prefixTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.prefixTextBox.Location = new System.Drawing.Point(239, 59);
            this.prefixTextBox.Name = "prefixTextBox";
            this.prefixTextBox.Size = new System.Drawing.Size(546, 22);
            this.prefixTextBox.TabIndex = 2;
            this.prefixTextBox.Leave += new System.EventHandler(this.prefixTextBox_Leave);
            // 
            // unitsLabel
            // 
            this.unitsLabel.AutoSize = true;
            this.unitsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.unitsLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.unitsLabel.Location = new System.Drawing.Point(3, 0);
            this.unitsLabel.Name = "unitsLabel";
            this.unitsLabel.Size = new System.Drawing.Size(230, 14);
            this.unitsLabel.TabIndex = 3;
            this.unitsLabel.Text = "Units:";
            // 
            // prefixLabel
            // 
            this.prefixLabel.AutoSize = true;
            this.prefixLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.prefixLabel.Location = new System.Drawing.Point(3, 56);
            this.prefixLabel.Name = "prefixLabel";
            this.prefixLabel.Size = new System.Drawing.Size(72, 14);
            this.prefixLabel.TabIndex = 4;
            this.prefixLabel.Text = "File Prefix:";
            // 
            // suffixTextBox
            // 
            this.suffixTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.suffixTextBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.suffixTextBox.Location = new System.Drawing.Point(239, 115);
            this.suffixTextBox.Name = "suffixTextBox";
            this.suffixTextBox.Size = new System.Drawing.Size(546, 22);
            this.suffixTextBox.TabIndex = 6;
            this.suffixTextBox.Leave += new System.EventHandler(this.suffixTextBox_Leave);
            // 
            // suffixLabel
            // 
            this.suffixLabel.AutoSize = true;
            this.suffixLabel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.suffixLabel.Location = new System.Drawing.Point(3, 112);
            this.suffixLabel.Name = "suffixLabel";
            this.suffixLabel.Size = new System.Drawing.Size(71, 14);
            this.suffixLabel.TabIndex = 5;
            this.suffixLabel.Text = "File Suffix:";
            // 
            // unitsComboBox
            // 
            this.unitsComboBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.unitsComboBox.FormattingEnabled = true;
            this.unitsComboBox.Location = new System.Drawing.Point(239, 3);
            this.unitsComboBox.Name = "unitsComboBox";
            this.unitsComboBox.Size = new System.Drawing.Size(121, 26);
            this.unitsComboBox.TabIndex = 7;
            this.unitsComboBox.SelectedValueChanged += new System.EventHandler(this.unitsComboBox_SelectedValueChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.cancelButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.createButton, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 407);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(794, 40);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // cancelButton
            // 
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancelButton.Location = new System.Drawing.Point(3, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(391, 34);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // createButton
            // 
            this.createButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createButton.Location = new System.Drawing.Point(400, 3);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(391, 34);
            this.createButton.TabIndex = 0;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "NewProject";
            this.Text = "NewProject";
            this.projectGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.settingsGroupBox.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox projectGroupBox;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox nameTextBox;
        private Label nameLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox settingsGroupBox;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox prefixTextBox;
        private Label unitsLabel;
        private Label prefixLabel;
        private TableLayoutPanel tableLayoutPanel4;
        private Button cancelButton;
        private Button createButton;
        private TextBox suffixTextBox;
        private Label suffixLabel;
        private ComboBox unitsComboBox;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel5;
        private Button partTemplateButton;
        private Button assemblyTemplateButton;
        private Button drawingTemplateButton;
    }
}