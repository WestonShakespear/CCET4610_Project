namespace GUILayoutTest1
{
    partial class NewFileTypeDialog
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
            this.partButton = new System.Windows.Forms.Button();
            this.assemblyButton = new System.Windows.Forms.Button();
            this.drawingButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.partButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.assemblyButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.drawingButton, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 161);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // partButton
            // 
            this.partButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partButton.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.partButton.Location = new System.Drawing.Point(3, 3);
            this.partButton.Name = "partButton";
            this.partButton.Size = new System.Drawing.Size(188, 155);
            this.partButton.TabIndex = 0;
            this.partButton.Text = "Part";
            this.partButton.UseVisualStyleBackColor = true;
            this.partButton.Click += new System.EventHandler(this.partButton_Click);
            // 
            // assemblyButton
            // 
            this.assemblyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assemblyButton.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.assemblyButton.Location = new System.Drawing.Point(197, 3);
            this.assemblyButton.Name = "assemblyButton";
            this.assemblyButton.Size = new System.Drawing.Size(188, 155);
            this.assemblyButton.TabIndex = 1;
            this.assemblyButton.Text = "Assembly";
            this.assemblyButton.UseVisualStyleBackColor = true;
            this.assemblyButton.Click += new System.EventHandler(this.assemblyButton_Click);
            // 
            // drawingButton
            // 
            this.drawingButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawingButton.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.drawingButton.Location = new System.Drawing.Point(391, 3);
            this.drawingButton.Name = "drawingButton";
            this.drawingButton.Size = new System.Drawing.Size(190, 155);
            this.drawingButton.TabIndex = 2;
            this.drawingButton.Text = "Drawing";
            this.drawingButton.UseVisualStyleBackColor = true;
            this.drawingButton.Click += new System.EventHandler(this.drawingButton_Click);
            // 
            // NewFileTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 161);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "NewFileTypeDialog";
            this.Text = "NewFileTypeDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button partButton;
        private Button assemblyButton;
        private Button drawingButton;
    }
}