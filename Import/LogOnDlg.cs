using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;


namespace Import
{
    public partial class LoginDlg : Form
    {
        public LoginDlg()
        {
            InitializeComponent();
            bRun = false;

        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            bRun = true;
            Properties.Settings1.Default.ServerName = ServerName.Text;
            Properties.Settings1.Default.PowerName = pdbUserName.Text;
            Properties.Settings1.Default.PowerPassword = pdbPassword.Text;
            Properties.Settings1.Default.Database = dbslist.SelectedValue.ToString();
            try
            {
                string str = "";
                if (regionslist.CheckedItems.Count > 0)
                {
                    for (int i = 0; i < regionslist.CheckedItems.Count; i++)
                    {
                        if (str == "")
                        {
                            str = regionslist.CheckedItems[i].ToString();
                        }
                        else
                        {
                            str += "," + regionslist.CheckedItems[i].ToString();
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Please select at least one region!");
                    bRun = false;
                }
             Properties.Settings1.Default.Regions = str;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.ToString());
            }
            
            Properties.Settings1.Default.Save();
            if (bRun)
            {
                Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            bRun = false;
            Close();
        }

        private void ServerName_Leave(object sender, EventArgs e)
        {

            List<String> databases = new List<String>();
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("Data Source=" + ServerName.Text + ";User ID=powerdb;Password=howdy;");
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                conn.Open();
                string select = "select name from sys.databases where (name like ('%DEV%') OR name like('%STAGE%') OR name like('%PROD%')) AND NAME LIKE '%EMEA%' ";
                System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand(select, conn);

                System.Data.SqlClient.SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    databases.Add(dr[0].ToString());
                }

                dbslist.DataSource = databases;
                Cursor.Current = Cursors.Default;

            }

            catch (Exception ex)
            {
                if(ServerName.Text=="")
                MessageBox.Show("Please enter a server name!");
                else
                {
                    MessageBox.Show("Invalid server name!");
                }
            }
        }

        private void dbslist_SelectedIndexChanged(object sender, EventArgs e)
        {
                List<String> regions = new List<String>();
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection("Data Source=" + ServerName.Text + ";User ID=powerdb;Password=howdy;Initial Catalog=" + dbslist.SelectedValue);
                conn.Open();
                string select = "select RegionDescrip from PdbRegions where RegionDescrip like '23%'";
                System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand(select, conn);

                System.Data.SqlClient.SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    regions.Add(dr[0].ToString());
                }

                regionslist.DataSource = regions;
            
        }
    }
}
