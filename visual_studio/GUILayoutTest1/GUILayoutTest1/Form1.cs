using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;

namespace GUILayoutTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void s(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Debug.Write("done");
            string connectionString = "Data Source=database.db;";
            string sql = "SELECT * FROM References";
            SQLiteConnection sqlite_conn = new SQLiteConnection(connectionString);
       
            sqlite_conn.Open();
            SQLiteCommand comm = new SQLiteCommand("Select * From [Ref]", sqlite_conn);
            using (SQLiteDataReader read = comm.ExecuteReader())
            {
                while (read.Read())
                {
                    dataGridView1.Rows.Add(new object[] {
            read.GetValue(0),  // U can use column index
            read.GetValue(read.GetOrdinal("PatientName")),  // Or column name like this
            read.GetValue(read.GetOrdinal("PatientAge")),
            read.GetValue(read.GetOrdinal("PhoneNumber"))
            });
                }
            }
            sqlite_conn.Close();


            Debug.Write("done");
        


           
        }
    }
}