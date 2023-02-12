using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogSldworkTest
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            this.testButton1 = new System.Windows.Forms.Button();
            this.testButton2 = new System.Windows.Forms.Button();
            this.testButton3 = new System.Windows.Forms.Button();
            this.mainGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();

            



            this.mainGroupBox.SuspendLayout();
            this.mainGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.mainGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.mainGroupBox.Location = new System.Drawing.Point(3, 340);
            this.mainGroupBox.Name = "mainGroupBox";
            //this.mainGroupBox.Size = new System.Drawing.Size(353, 107);
            this.mainGroupBox.TabIndex = 0;
            this.mainGroupBox.TabStop = false;
            this.mainGroupBox.Text = "Wow it's a WinForm!";

            this.testButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.testButton1.Location = new System.Drawing.Point(3, 129);
            this.testButton1.Name = "testButton1";
            //this.testButton1.Size = new System.Drawing.Size(163, 120);
            this.testButton1.TabIndex = 2;
            this.testButton1.Text = "Test 1";
            this.testButton1.UseVisualStyleBackColor = true;

            this.testButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.testButton2.Location = new System.Drawing.Point(3, 129);
            this.testButton2.Name = "testButton2";
            //this.testButton2.Size = new System.Drawing.Size(163, 120);
            this.testButton2.TabIndex = 2;
            this.testButton2.Text = "Test 2";
            this.testButton2.UseVisualStyleBackColor = true;

            this.testButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.testButton3.Location = new System.Drawing.Point(3, 129);
            this.testButton3.Name = "testButton3";
            //this.testButton3.Size = new System.Drawing.Size(163, 120);
            this.testButton3.TabIndex = 2;
            this.testButton3.Text = "Test 3";
            this.testButton3.UseVisualStyleBackColor = true;

            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.testButton1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.testButton2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.testButton3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            //this.tableLayoutPanel1.Size = new System.Drawing.Size(359, 450);
            this.tableLayoutPanel1.TabIndex = 0;





            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Text = String.Empty;
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(this.mainGroupBox);
            this.mainGroupBox.ResumeLayout(false);
            this.ResumeLayout(true);

            
        }

        private GroupBox mainGroupBox;
        public Button testButton1;
        public Button testButton2;
        public Button testButton3;
        private TableLayoutPanel tableLayoutPanel1;

    }
}



