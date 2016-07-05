using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
//using System.Windows.Documents;
using System.Windows.Controls;
using System.Resources;
using System.Reflection;
using System.Collections;
using ADOX;
using PdbAPI;
namespace Import
{
    public partial class MainForm : Form
    {
        public MainForm(LoginDlg dlg)
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(CompareData);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            pdbhash = new System.Collections.Hashtable();
            smshash = new System.Collections.Hashtable();
            lastrunsmshash = new System.Collections.Hashtable();
            regionhash = new System.Collections.Hashtable();
            subtypehash = new System.Collections.Hashtable();
            NewTags = new System.Windows.Forms.BindingSource();
            PowerDBNewTags = new System.Windows.Forms.BindingSource();
            Logdlg = dlg;
            serverName = Logdlg.ServerName.Text;
            pdbname = Logdlg.pdbUserName.Text;
            pdbpswd = Logdlg.pdbPassword.Text;
            database = Logdlg.dbslist.Text;
            regions = Properties.Settings1.Default.Regions;
            PowerDBLoaded = SMSLoaded = NewTagsLoaded = LastRunSMSLoaded = bViewer = false;
            InvalidRegions = new System.Collections.Generic.List<string>();
            InvalidSubType = new System.Collections.Generic.List<string>();
            textBoxLastSMSFile.Text = Properties.Settings1.Default.smsdir;
            LoadViewerAccounts();
            foreach (string ua in accounts)
            {
                if (pdbname == ua)
                {
                    addNewTagsToolStripMenuItem.Enabled = false;
                    compareDataToolStripMenuItem1.Enabled = false;
                    addNewTagsToolStripMenuItem1.Enabled = false;
                    button3.Enabled = false;
                    bViewer = true;
                }
            }

        }

        /*
/----------------------------------------------------------------------------\
| MainForm::Import()                                                         |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method imports database info and populates hashtable for datatable |
|    rows                                                                    |
|  Parameters:                                                               |
|    txtFileName                                                             |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void Import(string txtFileName)
        {
            if (txtFileName.Trim() != string.Empty)
            {
                try
                {
                    DataTable dt = GetDataTable(txtFileName);
                    SMSData.DataSource = dt.DefaultView;
  
                    int index = 0;
                    smshash.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        smshash.Add(dr["TAG"].ToString(), index++);
                    }
                    labelCount.Text = dt.Rows.Count.ToString();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                Properties.Settings1.Default.lastrundir = txtFileName;
                Properties.Settings1.Default.Save();
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::ImportLastRun()                                                  |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method imports database and SMS last run info and populates        |
|    hashtables for datatable rows                                           |
|                                                                            |
|  Parameters:                                                               |
|    txtFileName                                                             |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private int ImportLastRun(string txtFileName)
        {
           
            if (txtFileName.Trim() != string.Empty)
            {
                try
                {
                    DataTable dt = GetDataTable(txtFileName);
                    LastRunSMSData.DataSource = dt.DefaultView;
                    int index = 0;
                    lastrunsmshash.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        lastrunsmshash.Add(dr["TAG"].ToString(), index++);
                    }
                    labelCount.Text = dt.Rows.Count.ToString();
                    return 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /*
        /----------------------------------------------------------------------------\
        | MainForm::RemoveDuplicateRows()                                            |
        |----------------------------------------------------------------------------|
        |  Description:                                                              |
        |    This method removes duplicate rows from datatable                       |
        |                                                                            |
        |  Parameters:                                                               |
        |    dTable, colName                                                         |
        |                                                                            |
        |  Return Value:                                                             |
        |    dTable - Datatable which contains unique records                        |
        |                                                                            |
        \----------------------------------------------------------------------------/
        */

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }

        /*
        /----------------------------------------------------------------------------\
        | MainForm::GetDataTableExcel()                                              |
        |----------------------------------------------------------------------------|
        |  Description:                                                              |
        |    This method retrieves a table from an excel workbook query              |
        |                                                                            |
        |  Parameters:                                                               |
        |    strFileName, Table                                                      |
        |                                                                            |
        |  Return Value:                                                             |
        |    ds.Tables[0] - table in excel returning results                         |
        |                                                                            |
        \----------------------------------------------------------------------------/
        */

        public static DataTable GetDataTableExcel(string strFileName, string Table)
        {
            Cursor.Current = Cursors.WaitCursor;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";");
            conn.Open();
            string strQuery = "SELECT CStr(SITE) AS SITE,NAME,DISTRICT,CStr(TAG) AS TAG,ADDRESS1,ADDRESS2,CITY,STATE, iif(ZIP is null,' ',cstr(ZIP)) AS ZIP ,SERIAL,CONTACT,(CStr([AREA CODE]) + CStr([Phone])) AS [PHONENUM]  FROM [" + Table + "]";
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::GetTableExcel()                                                  |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method retrieves tab names from excel into a string array          |
|                                                                            |
|  Parameters:                                                               |
|    strFileName                                                             |
|                                                                            |
|  Return Value:                                                             |
|    strTables - string array of excel tab names                             |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        public static string[] GetTableExcel(string strFileName)
        {
            Cursor.Current = Cursors.WaitCursor;
            string[] strTables = new string[100];
            Catalog oCatlog = new Catalog();
            ADOX.Table oTable = new ADOX.Table();
            ADODB.Connection oConn = new ADODB.Connection();
            oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";", "", "", 0);
            oCatlog.ActiveConnection = oConn;
            if (oCatlog.Tables.Count > 0)
            {
                int item = 0;
                foreach (ADOX.Table tab in oCatlog.Tables)
                {
                    if (tab.Type == "TABLE")
                    {
                        strTables[item] = tab.Name;
                        item++;
                    }
                }
            }
            return strTables;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::GetDataTable()                                                   |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method creates and populates a data table from powerdb db query    |
|    with customer info                                                      |
|  Parameters:                                                               |
|    strFileName                                                             |
|                                                                            |
|  Return Value:                                                             |
|    dt -                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/

        public static DataTable GetDataTable(string strFileName)
        {
            OleDbConnection oConn = null;
            string strConnStr = "Provider=Microsoft.ACE.OleDb.12.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";";
            string strQuery = "SELECT SITE, [SITE NAME] AS NAME, [Dist# & Name] AS DISTRICT,iif(ISNULL([Syste/Tag Number]),'',cstr([Syste/Tag Number])) AS TAG, [Address Line 1] as ADDRESS1,[Address Line 2] AS ADDRESS2,CITY,STATE,ZIP,iif(ISNULL([SERIAL #]),'',cstr([SERIAL #])) AS SERIAL,CONTACT, iif([AREA CODE] = 0,'',cstr([AREA CODE])) + iif([PHONE] = 0,'',cstr([PHONE])) AS [PHONENUM],[Email Address] AS EMAIL, SUBTYPE, Model, Manufacturer, [System Number] AS SystemNumber,[System Name] AS SYSTEMNAME, Description AS SYSTEMDESCR, Parent, Children FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
            oConn = new OleDbConnection(strConnStr);
            oConn.Open();
            OleDbCommand cmd = new OleDbCommand(strQuery, oConn);
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.FillSchema(dt, SchemaType.Source);

            if (dt != null)
            {
                writeSchema(dt, System.IO.Path.GetDirectoryName(strFileName), System.IO.Path.GetFileName(strFileName));
            }
            adapter.Fill(dt);
            return dt;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::writeSchema()                                                   |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method creates and populates a data table from powerdb db query    |
|    with customer info                                                      |
|  Parameters:                                                               |
|    dt                                                                      |
|    path                                                                    |
|    filename                                                                |
|                                                                            |
|  Return Value:                                                             |
|                                                                            |
|                                                                            |
\----------------------------------------------------------------------------/
*/

        private static void writeSchema(DataTable dt, string path, string filename)
        {
            try
            {
                FileStream fsOutput = new FileStream(path + "\\schema.ini", FileMode.Create, FileAccess.Write);
                StreamWriter srOutput = new StreamWriter(fsOutput);
                string s1, s2, s3, s4, s5;
                int ndx;
                s1 = "[\"" + filename + "\"]";
                s2 = "ColNameHeader=True";
                s3 = "Format=CSVDelimited";
                srOutput.WriteLine(s1 + "\r\n"+ s2 + "\r\n" + s3 + "\r\n");// + '\n' + s4 + '\n' + s5);
                StringBuilder strB = new StringBuilder();
                if (dt != null)
                {
                    for (Int32 ColIndex = 0; ColIndex <= dt.Columns.Count - 1; ColIndex++)
                    {
                        ndx = ColIndex + 1;
                        if (ndx > 13)
                        {
                            strB.Append("Col" + ndx.ToString());
                            strB.Append("=" + dt.Columns[ColIndex].ColumnName);
                            strB.Append(" Text\n");
                            srOutput.WriteLine(strB.ToString());
                            strB = new StringBuilder();
                        }
                    }
                    
                }


                srOutput.Close();
                fsOutput.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::GetRegionDataTable()                                             |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method creates and populates a data table from powerdb db query    |
|    with region assets                                                      |
|  Parameters:                                                               |
|    strFileName                                                             |
|                                                                            |
|  Return Value:                                                             |
|    dt - data table of all records from regionmap                           |
|                                                                            |
\----------------------------------------------------------------------------/
*/        
        //public static DataTable GetRegionDataTable(string strFileName)
        //{
        //    ADODB.Connection oConn = new ADODB.Connection();
        //    oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";", "", "", 0);
        //    string strQuery = "SELECT * FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
        //    ADODB.Recordset rs = new ADODB.Recordset();
        //    System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter();
        //    DataTable dt = new DataTable();
        //    rs.Open(strQuery, "Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";",
        //        ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, 1);
        //    adapter.Fill(dt, rs);
        //    return dt;
        //}

        /*
/----------------------------------------------------------------------------\
| MainForm::GetSubTypeDataTable()                                            |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method creates and populates a data table from given file          |
|                                                                            |
|  Parameters:                                                               |
|    strFileName                                                             |
|                                                                            |
|  Return Value:                                                             |
|    dt - data table of all records from regionmap                           |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        public static DataTable GetSubTypeDataTable(string strFileName)
        {
            ADODB.Connection oConn = new ADODB.Connection();
            oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";", "", "", 0);
            string strQuery = "SELECT * FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
            ADODB.Recordset rs = new ADODB.Recordset();
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter();
            DataTable dt = new DataTable();
            rs.Open(strQuery, "Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\";",
                ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, 1);
            adapter.Fill(dt, rs);
            return dt;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::GetDataTableSQL()                                                |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method creates and populates a data table from given sql database  |
|                                                                            |
|  Parameters:                                                               |
|    serverName, Table, sqltxt                                               |
|                                                                            |
|  Return Value:                                                             |
|    ds.Tables[0]                                                            |
|                                                                            |
\----------------------------------------------------------------------------/
*/        
        public static DataTable GetDataTableSQL(string serverName, string Table, string sqltxt)
        {
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=SQLOLEDB;Data Source=" + serverName + ";User ID=powerdb;Password=howdy;Initial Catalog=" + Table);
            conn.Open();
            ResourceManager rm = new ResourceManager("Import.Properties.Resources", Assembly.GetExecutingAssembly());
            string strQuery = rm.GetString("SiteView") + sqltxt;
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::LoadPowerDBData()                                                |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads powerdb data and adds corresponding regionhash to list|
|                                                                            |
|  Parameters:                                                               |
|    bUseDate                                                                |
|                                                                            |
|  Return Value:                                                             |
|    RetVal - determines if load successful                                  |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private bool LoadPowerDBData(bool bUseDate)
        {
            bool RetVal = true;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DataTable dt = new DataTable();
                List<string> list = new List<string>();
                bool bAdd = true;
                string[] regs = regions.Split(',');
                foreach(string region in regs)
                //foreach (DictionaryEntry pair in regionhash)
                {
                    string sqlstrdistrict = " WHERE [RegionDescrip]='" + region + "'";
                    bAdd = true;
                    if ( bUseDate && bAdd )
                    {                        
                        dt.Merge(GetDataTableSQL(serverName, database, sqlstrdistrict + "' AND WHERE (AHDate1 >= '" + dateTimePicker1.Text + "' OR AHDate2 >= '" + dateTimePicker1.Text + "' OR AddDate1 >= '" + dateTimePicker1.Text + "' OR AddDate2 >= '" + dateTimePicker1.Text + "' OR RelDate1 >= '" + dateTimePicker1.Text + "' OR RelDate2 >= '" + dateTimePicker1.Text + "' OR AADate1 >= '" + dateTimePicker1.Text + "' OR AADate2 >= '" + dateTimePicker1.Text + "' OR ) AND (AHDiff > 1 OR AddDiff > 1 OR RelDiff > 1)")); // AND (AI.LastModBy <> 'PDBAPI' OR AI.LastModBy IS NULL)"));
                        list.Add(database);
                    }
                    else if ( bAdd )
                    {
                        
                        dt.Merge(GetDataTableSQL(serverName, database, sqlstrdistrict));
                        list.Add(database);
                    }
                }
                dt = RemoveDuplicateRows(dt, "TAG");
                int index = 0;
                pdbhash.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    pdbhash.Add(dr["TAG"].ToString(), index++);
                }
                PowerDBData.DataSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDB LOAD FAIL: "+ ex.Message.ToString() );
                RetVal = false;
            }
            return RetVal;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::bw_ProgressChanged()                                             |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method updates the progress bar                                    |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/ 
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label1.Text = (e.ProgressPercentage.ToString() + "%");
            this.label1.Refresh();
            progressBar1.PerformStep();
            progressBar1.Refresh();
            if (e.ProgressPercentage > 99)
            {
                progressBar1.Visible = false;
                label1.Visible = false;
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::CompareData()                                                    |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method compares sms tagname, address 1 & 2, phone, contact, etc. to|
|    dataGrid entry. Logs problem columns                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        public void CompareData(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            DataGridViewRow dgvr;
            int smsindex = -1;
            int lastrunsmsndx = -1;
            double percent = 0;
            bool bOK;
            int NumProblems = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                NumProblems = 0;
                bOK = true;
                dgvr = dataGridView1.Rows[i];
                smsindex = -1;
                lastrunsmsndx = -1;
                if (smshash.ContainsKey(dgvr.Cells["TAG"].Value.ToString()))
                {
                    smsindex = (int)smshash[dgvr.Cells["TAG"].Value.ToString()];                    
                }
                if (lastrunsmshash.ContainsKey(dgvr.Cells["TAG"].Value.ToString()))
                {
                    lastrunsmsndx = (int)lastrunsmshash[dgvr.Cells["TAG"].Value.ToString()];
                }

                if ( smsindex > -1)
                {

                    string Site = dgvr.Cells["SITE"].Value.ToString();
                    string Tag = dgvr.Cells["TAG"].Value.ToString();

                    string[] ColNames= { "NAME", "ADDRESS1", "ADDRESS2", "CITY", "STATE", "ZIP", "SERIAL","PHONENUM", "CONTACT", "EMAIL", "MODEL", "MANUFACTURER", "SYSTEMNUMBER", "SYSTEMNAME","SYSTEMDESCR", "PARENT", "CHILDREN"};



                    foreach( string ColName in ColNames)
                    {
                        int CompRes = CompareField(dgvr.Index, smsindex, lastrunsmsndx, ColName);
                        if (CompRes != 0 )
                        {
                            switch (CompRes)
                            {
                                case 1:
                                    dgvr.Cells[ColName].Style.BackColor = Color.LightGreen;
                                    break;
                                case 2:
                                    dgvr.Cells[ColName].Style.BackColor = Color.Yellow;
                                    break;
                                case 3:
                                    dgvr.Cells[ColName].Style.BackColor = Color.Red;
                                    break;
                            }
                            
                            bOK = false;
                            NumProblems++;
                        }                        
                    }


                    dgvr.Cells["ProblemColumn"].Value = NumProblems.ToString();
                    if (bOK)
                    {
                        //dgvr.Visible = false;
                    }
                }
                else
                {
                    dgvr.Cells["ProblemColumn"].Value = "X";
                }
                percent = ((double)i / (double)dataGridView1.Rows.Count);
                worker.ReportProgress((int)(percent*100));
            }
            worker.ReportProgress(100);
        }



        /*
/----------------------------------------------------------------------------\
| MainForm::CompareField()                                                   |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method compares PowerDB and SMS fields                             |
|    Logs problem columns                                                    |
|  Parameters:                                                               |
|    indexdg, indexpdb, lastrunndx, FieldName                                |
|                                                                            |
|  Return Value:                                                             |
|    Integer value based on comparison                                       |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        public int CompareField(int indexdg, int indexpdb, int lastrunndx,  string FieldName)
        {

            int RetVal = 0;
            SMSData.Position = indexpdb;
            LastRunSMSData.Position = lastrunndx;

            string SmsVal = ((DataRowView)SMSData.Current)[FieldName].ToString().Trim();
            string PdbVal = dataGridView1.Rows[indexdg].Cells[FieldName].Value.ToString().Trim();
            string LastSmsVal = "";
            if (lastrunndx > 0)
            {
                LastSmsVal = ((DataRowView)LastRunSMSData.Current)[FieldName].ToString().Trim();
            }

            if (SmsVal == PdbVal || (SmsVal == "" && PdbVal == ",") || (SmsVal == "" && PdbVal == "<None>"))
            {
                RetVal = 0;
            }
            else
            {
                if (SmsVal != LastSmsVal && PdbVal == LastSmsVal)
                {
                    RetVal = 2;
                }
                else if (SmsVal == "")
                {
                    RetVal = 1;
                }
                else if (PdbVal == "" || PdbVal == "," || PdbVal == "<None>")
                {
                    RetVal = 2;
                }
                else
                {
                    RetVal = 3;
                }
            }
            return RetVal;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::dataGridView1_CellDoubleClick()                                  |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method opens the editing box for values to be entered into dataGrid|
|    pending appropriate hash                                                |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int pdbindex = -1;
            //
            // Do something on double click, except when on the header.
            //
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || e.ColumnIndex == 0)
            {
                return;
            }
            string cname = dataGridView1.Columns[e.ColumnIndex].Name;
            EditValueForm ev = new EditValueForm();
            string tag = dataGridView1.Rows[e.RowIndex].Cells["TAG"].Value.ToString();
            ev.groupBox1.Text = cname + "(" + tag + ")";
            ev.radioButton1.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            ev.radioButton1.Checked = true;
            string setVal = "";
            if (SMSData.Count > 0)
            {
                int smsindex = SMSData.Find("TAG", tag);
                if (smsindex > -1)
                {
                    SMSData.Position = smsindex;
                    ev.radioButton2.Text = ((DataRowView)SMSData.Current)[cname].ToString().Trim();
                }
            }
            if (LastRunSMSData.Count > 0)
            {
                int lastrunsmsindex = LastRunSMSData.Find("TAG", tag);
                if (lastrunsmsindex > -1)
                {
                    LastRunSMSData.Position = lastrunsmsindex;
                    ev.lblLastRunVal.Text = ((DataRowView)LastRunSMSData.Current)[cname].ToString().Trim();
                }
            }
            ev.ShowDialog(this);
            if (ev.bReturn)
            {
                if (pdbhash.ContainsKey(tag))
                {
                    pdbindex = (int)pdbhash[tag];
                    if (ev.radioButton1.Checked)
                    {
                        setVal = ev.radioButton1.Text;
                        dataGridView1.Rows[pdbindex].Cells[cname].Value = setVal;
                        dataGridView1.Rows[pdbindex].Cells[cname].Style.BackColor = Color.LightGreen;
                    }
                    else if (ev.radioButton2.Checked)
                    {
                        setVal = ev.radioButton2.Text;
                        dataGridView1.Rows[pdbindex].Cells[cname].Style.BackColor = Color.Yellow;
                    }
          
                }
                
                switch (cname)
                {
                    case "NAME":
                        break;
                    case "ADDRESS1":
                        break;
                    case "ADDRESS2":
                        break;
                    case "CITY":
                        break;
                    case "STATE":
                        break;
                    case "ZIP":
                        break;
                    case "PHONENUM":
                        break;
                    case "EMAIL":
                        break;
                    default:
                        break;
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void statusPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        /*
/----------------------------------------------------------------------------\
| MainForm::selectAllToolStripMenuItem_Click()                               |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method selects all cells in dataGrid                               |
|    pending appropriate hash                                                |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            foreach(DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells["checkBox"].Value = true;
            }
            dataGridView1.Rows[0].Cells[0].Value = true;
            dataGridView1.RefreshEdit();
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::unSelectAllToolStripMenuItem_Click()                             |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method deselects all cells in dataGrid                             |
|    pending appropriate hash                                                |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void unSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                dgvr.Cells["checkBox"].Value = false;
            }
            dataGridView1.Rows[0].Cells[0].Value = false;
            dataGridView1.RefreshEdit();
        }


        /*
/----------------------------------------------------------------------------\
| MainForm::AddNewTag()                                                      |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method creates new tag and its asset values                        |
|                                                                            |
|  Parameters:                                                               |
|    many                                                                    |
|                                                                            |
|  Return Value:                                                             |
|    RetVal - flags succesful addition                                       |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private int AddNewTag(IPdbIntegrator pdbapi, bool bSkip, string siteId, string siteName, string district, string tagId, string address1, string address2, string city,
                                string state, string zip, string serialnum, string contact, string phone, string email, string subtype, string model, string manufacturer, string systemNumber, string systemName, string systemDescr, string parent, string children)
        {
            int RetVal = -1;
            string formguid = "";
            string siteAddr = "";
            string zeroes = "";

            if (subtypehash.Contains(subtype))
            {
                formguid = subtypehash[subtype].ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(InvalidSubType.Find(delegate(string n) { return n == subtype; })))
                {
                    InvalidSubType.Add(subtype);
                }
                bSkip = true;
            }
            if (!bSkip )
            {
                if (pdbapi.TestConnection())
                {
                    bool baddComp = true;
                    string CompanyGuid = "";
                    PdbAPI.PowerDB.Region destRegion = null;
                    try
                    {
                        destRegion = pdbapi.GetRegionWithRegionName(district);
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(InvalidRegions.Find(delegate(string m) { return m == district; })))
                        {
                            InvalidRegions.Add(district);
                        }
                        RetVal = -1;
                    }
                    if (destRegion != null)
                    {
                        if (PowerDBData.Find("TAG", tagId) < 0)
                        {
                            List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                            MatchingAddresses = pdbapi.GetAddressesWithImportID(siteId + "-2");
                            if (MatchingAddresses.Count <= 0)
                            {
                                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteId);
                            }
                            if (MatchingAddresses.Count <= 0)
                            {
                                List<PdbAPI.PowerDB.Company> MatchingCompanies = new List<PdbAPI.PowerDB.Company>();
                                MatchingCompanies = pdbapi.GetCompaniesWithName(siteName);
                                foreach (PdbAPI.PowerDB.Company ndxComp in MatchingCompanies)
                                {
                                    if (ndxComp.Name.Trim().ToLower() == siteName.Trim().ToLower())
                                    {
                                        CompanyGuid = ndxComp.CompanyGUID;
                                        baddComp = false;
                                        break;
                                    }
                                }
                                PdbAPI.PowerDB.Address newTag;
                                PdbAPI.PowerDB.Company pdbCompany;
                                //
                                //If company exist then use it else create new company
                                //
                                if (!baddComp)
                                {
                                    pdbCompany = pdbapi.GetCompanyWithGuid(CompanyGuid);
                                }
                                else
                                {
                                    pdbCompany = pdbapi.CreateCompany();
                                    pdbCompany.Name = siteName;
                                    pdbapi.Update();
                                }
                                newTag = pdbapi.CreateAddressForCompany(pdbCompany);
                                newTag.Region = destRegion;
                                newTag.AddressLine1 = address1;
                                newTag.AddressLine2 = address2;
                                newTag.City = city;
                                newTag.State = state;
                                newTag.Zip = zip;
                                newTag.Phone = phone;
                                newTag.LastModBy = "PDBAPI";
                                string[] nameArray = contact.Split(',');
                                if (nameArray.Length > 1)
                                {
                                    newTag.FirstName = nameArray[1];
                                    newTag.LastName = nameArray[0];
                                }
                                newTag.Email = email;
                                newTag.Type = PdbAPI.PowerDB.AddressType.Plant;

                                if (siteId.Length < 6)
                                {
                                    for (int len = siteId.Length; len < 6; len++)
                                    {
                                        zeroes = zeroes + "0";
                                    }
                                    siteId = zeroes + siteId;
                                }
                                
                                newTag.ImportID = siteId.Trim() + "-2";
                                pdbapi.Update();
                            }
                            // CREATE ASSET
                            if (string.IsNullOrEmpty(systemName))
                            {
                                systemName = "<None>";
                            }

                            siteAddr = siteId.Trim() + "-" + address1.Trim();
                            if (tagId.Length < 7)
                            {
                                for (int len = tagId.Length; len < 7; len++)
                                {
                                    zeroes = zeroes + "0";
                                }
                                tagId = zeroes + tagId;
                            }
                            PdbAPI.PowerDB.Asset newAsset = pdbapi.CreateAsset();
                            newAsset.SetDefaultValues();
                            newAsset.Form = pdbapi.GetFormWithFormID(formguid);

                            newAsset.AssetName = newAsset.Form.FormName;
                            newAsset.Region = destRegion;
                            newAsset.FolderLevel1 = siteName;
                            newAsset.FolderLevel2 = siteAddr;
                            newAsset.FolderLevel3 = systemName;
                            newAsset.FolderLevel4 = tagId;
                            newAsset.AssetID = tagId;
                            newAsset.BarcodeId = serialnum;
                            
                            // CREATE AssetAttributes
                            PdbAPI.PowerDB.AssetAttribute newAttribute;
                            if (!string.IsNullOrEmpty(tagId))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "TagId");
                                newAttribute.StrVal = (tagId == null) ? "" : tagId;
                            }
                            if (!string.IsNullOrEmpty(subtype))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "SubType");
                                newAttribute.StrVal = (subtype == null) ? "" : subtype;
                            }
                            if (!string.IsNullOrEmpty(systemNumber))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "SystemNumber");
                                newAttribute.StrVal = (systemNumber == null) ? "" : systemNumber;
                            }
                            if (!string.IsNullOrEmpty(systemName))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "SystemName");
                                newAttribute.StrVal = (systemName == null) ? "" : systemName;
                            }
                            if (!string.IsNullOrEmpty(systemDescr))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "SystemDescr");
                                newAttribute.StrVal = (systemDescr == null) ? "" : systemDescr;
                            }
                            if (!string.IsNullOrEmpty(parent))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "Parent");
                                newAttribute.StrVal = (parent == null) ? "" : parent;
                            }
                            if (!string.IsNullOrEmpty(children))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "Children");
                                newAttribute.StrVal = (children == null) ? "" : children;
                            }
                            if (!string.IsNullOrEmpty(serialnum))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "SerialNumber");
                                newAttribute.StrVal = (serialnum == null) ? "" : serialnum;
                            }
                            if (!string.IsNullOrEmpty(model))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "Model");
                                newAttribute.StrVal = (model == null) ? "" : model;
                            }
                            if (!string.IsNullOrEmpty(manufacturer))
                            {
                                newAttribute = pdbapi.AddNewAssetAttributeToAsset(newAsset, "Manufacturer");
                                newAttribute.StrVal = (manufacturer == null) ? "" : manufacturer;
                            }
                            pdbapi.Update();
                            RetVal = 1;
                        }

                    }
                }

                else
                {
                    RetVal = 2;
                }
            }
            return RetVal;
        }



        private void buttonAddNew_Click(object sender, EventArgs e)
        {

        }

        /*
/----------------------------------------------------------------------------\
| MainForm::LoadRegionMap()                                                  |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads region data into data table pending on corresponding  |
|    hash                                                                    |
|  Parameters:                                                               |
|    none                                                                    |
|                                                                            |
|  Return Value:                                                             |
|    RetVal - flags succesful load                                           |
|                                                                            |
\----------------------------------------------------------------------------/
*/

        //private bool LoadRegionMap()
        //{
        //    bool bRetval = false;
        //    string startupPath = Environment.CurrentDirectory + @"\RegionMap.csv";
        //    if (File.Exists(startupPath))
        //    {
        //        DataTable dt = GetRegionDataTable(startupPath);
        //        regionhash.Clear();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            regionhash.Add(dr["District"].ToString(), dr["Database"].ToString());
        //            bRetval = true;
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Unable to load Region Map.");
        //    }
        //    return bRetval;
        //}

        /*
/----------------------------------------------------------------------------\
| MainForm::LoadSubTypeMap()                                                 |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads subtype data into data table pending on corresponding |
|    hash                                                                    |
|  Parameters:                                                               |
|    none                                                                    |
|                                                                            |
|  Return Value:                                                             |
|    RetVal - flags succesful load                                           |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private bool LoadSubTypeMap()
        {
            bool bRetval = false;
            string startupPath = Environment.CurrentDirectory + @"\SubTypeMap.csv";
            if (File.Exists(startupPath))
            {
                DataTable dt = GetSubTypeDataTable(startupPath);
                subtypehash.Clear();
                dt = RemoveDuplicateRows(dt, "SubType");
                foreach (DataRow dr in dt.Rows)
                {
                    subtypehash.Add(dr["SubType"].ToString(), dr["FormGUID"].ToString());
                    bRetval = true;
                }
            }
            else
            {
                MessageBox.Show("Unable to load Region Map.");
            }
            return bRetval;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::LoadViewerAccounts()                                                 |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads user account data into an array                       |
|                                                                            |
|  Parameters:                                                               |
|    none                                                                    |
|                                                                            |
|  Return Value:                                                             |
|    RetVal - flags successful load                                          |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private bool LoadViewerAccounts()
        {
            bool bRetVal = false;
            string startupPath = Environment.CurrentDirectory + @"\UserAccounts.txt";

            if (File.Exists(startupPath))
            {
                accounts = File.ReadAllLines(startupPath);
                bRetVal = true;
            }

            return bRetVal;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::loadPowerDBDataToolStripMenuItem_Click()                         |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads powerdb data into dataGrid for viewing                |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void loadPowerDBDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadPowerDBData(false) && LoadSubTypeMap())
            {
                PowerDBLoaded = true;
                NewTagsLoaded = false;
                dataGridView1.DataSource = PowerDBData.DataSource;
                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].ReadOnly = true;
                }
                labelCount.Text = dataGridView1.Rows.Count.ToString();
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::addNewTagsToolStripMenuItem_Click()                              |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method adds new tags to Datagrid view from existing file           |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void addNewTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = Properties.Settings1.Default.newdir;
            fdlg.Filter = "Comma Delimited (*.csv)|*.csv|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;

            if (fdlg.ShowDialog() == DialogResult.OK && fdlg.FileName.Trim() != string.Empty)
            {
                try
                {
                    DataTable dt = GetDataTable(fdlg.FileName);
                    NewTags.DataSource = dt.DefaultView;
                    dataGridView1.DataSource = NewTags.DataSource;
                    for (int i = 1; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView1.Columns[i].ReadOnly = true;
                    }
                    NewTagsLoaded = true;
                    selectAll.PerformClick();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                Properties.Settings1.Default.newdir = fdlg.FileName;
                Properties.Settings1.Default.Save();
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::loadSMSDataToolStripMenuItem_Click()                             |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads sms data from csv file into dataGrid for viewing      |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void loadSMSDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = Properties.Settings1.Default.smsdir;
            fdlg.Filter = "Comma Delimited (*.csv)|*.csv|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK && fdlg.FileName.Trim() != string.Empty)
            {
                try
                {
                    Import(fdlg.FileName);
                    SMSLoaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                Properties.Settings1.Default.smsdir = fdlg.FileName;
                Properties.Settings1.Default.Save();
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::compareDataToolStripMenuItem1_Click()                            |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method updates the progress bar                                    |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void compareDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Maximum = dataGridView1.Rows.Count;
            backgroundWorker1.RunWorkerAsync();
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::operationsToolStripMenuItem_Click()                              |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method enables menu item buttons for access once clicked           |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void operationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareDataToolStripMenuItem1.Enabled = (PowerDBLoaded && SMSLoaded);
            addNewTagsToolStripMenuItem1.Enabled =  NewTagsLoaded;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::addNewTagsToolStripMenuItem1_Click()                             |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method adds new powerdb data into dataGrid manually                |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void addNewTagsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            if (!PowerDBLoaded)
            {
                //LoadRegionMap();
                LoadSubTypeMap();
                LoadPowerDBData(false);
            }
            try
            {
                int TagsAdded = 0;
                int AddResult = 0;
                string OldDistrict = "";
                string dbname = "";
                IPdbIntegrator pdbapi = null;
                bool bSkip = false;
                foreach(DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    if ((bool)dgvr.Cells[0].Value)
                    {
                        if (OldDistrict != dgvr.Cells["DISTRICT"].Value.ToString())
                        {
                            OldDistrict = dgvr.Cells["DISTRICT"].Value.ToString();
                            bSkip = false;
                            if (regionhash.Contains(OldDistrict))
                            {
                                dbname = regionhash[OldDistrict].ToString();
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(InvalidRegions.Find(delegate(string m) { return m == OldDistrict; })))
                                {
                                    InvalidRegions.Add(OldDistrict);
                                }
                                bSkip = true;
                            }
                            if (!bSkip)
                            {
                                pdbapi = PdbAPI.IntegrationFactory.CreateSqlServerInterface(serverName, dbname, pdbname, pdbpswd);
                            }
                        }
                        AddResult = AddNewTag(pdbapi, bSkip, dgvr.Cells["SITE"].Value.ToString(), dgvr.Cells["NAME"].Value.ToString(), dgvr.Cells["DISTRICT"].Value.ToString(), dgvr.Cells["TAG"].Value.ToString(),
                                            dgvr.Cells["ADDRESS1"].Value.ToString(), dgvr.Cells["ADDRESS2"].Value.ToString(), dgvr.Cells["CITY"].Value.ToString(), dgvr.Cells["STATE"].Value.ToString(),
                                            dgvr.Cells["ZIP"].Value.ToString(), dgvr.Cells["SERIAL"].Value.ToString(), dgvr.Cells["CONTACT"].Value.ToString(), dgvr.Cells["PHONENUM"].Value.ToString(), dgvr.Cells["EMAIL"].Value.ToString(), dgvr.Cells["SUBTYPE"].Value.ToString(), dgvr.Cells["MODEL"].Value.ToString(), dgvr.Cells["MANUFACTURER"].Value.ToString(),dgvr.Cells["SYSTEMNUMBER"].Value.ToString(), dgvr.Cells["SYSTEMNAME"].Value.ToString(), dgvr.Cells["SYSTEMDESCR"].Value.ToString(), dgvr.Cells["Parent"].Value.ToString(), dgvr.Cells["Children"].Value.ToString());
                        if ( AddResult == 1 )
                        {
                            dgvr.Cells[0].Value = false;
                            TagsAdded++;
                        }
                        else if ( AddResult == 2 )
                        {
                            dgvr.DefaultCellStyle.ForeColor = Color.DarkRed;
                        }
                    }
                }
                if ((InvalidRegions.Count < 1) && (InvalidSubType.Count < 1))
                {
                    MessageBox.Show("Tags Added: " + TagsAdded.ToString());                    
                }
                else
                {
                    string reglist = "";
                    string sublist = "";

                    foreach (string s in InvalidRegions)
                    {
                        reglist = reglist + s + System.Environment.NewLine;
                    }

                    foreach (string str in InvalidSubType)
                    {
                        sublist = sublist + str + System.Environment.NewLine;
                    }
                    MessageBox.Show("Tags Added: " + TagsAdded.ToString() + "\nInvalid Regions:\n" + reglist + "\nInvalid SubTypes:\n" + sublist);
                }
                LoadPowerDBData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            dataGridView1.RefreshEdit();
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::exportToCSVToolStripMenuItem_Click()                             |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method exports dataGrid values into csv file                       |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter( @"C:\output.csv");

            string strHeader = "";
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                strHeader += dataGridView1.Columns[i].HeaderText + ",";
            }
            streamWriter.WriteLine(strHeader);


            for (int m = 0; m < dataGridView1.Rows.Count; m++)
            {
                string strRowValue = "";
                for (int n = 1; n < dataGridView1.Columns.Count; n++)
                {
                    strRowValue += "\"" + dataGridView1.Rows[m].Cells[n].Value + "\",";
                }
                streamWriter.WriteLine(strRowValue);
            }

            streamWriter.Close();
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::runToolStripMenuItem_Click()                                     |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method shows tags not in sms (pdb tags)                            |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
             PowerDBNewTags = PowerDBData;
             foreach (DataRowView drv in PowerDBNewTags)
             {
                 if( smshash.Contains( drv["TAG"].ToString() ) )
                 {
                     drv.Delete();
                 }
             }
             dataGridView1.DataSource = PowerDBNewTags.DataSource;
             labelCount.Text = dataGridView1.Rows.Count.ToString();
             PowerDBLoaded = false;
                
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::showTagsNotInPdbToolStripMenuItem_Click()                        |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method shows tags not in powerdb (sms tags)                        | 
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void showTagsNotInPdbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindingSource notinpdb = SMSData;
            foreach (DataRowView drv in notinpdb)
            {
                if (pdbhash.Contains(drv["TAG"].ToString()))
                {
                    drv.Delete();
                }
            }
            dataGridView1.DataSource = notinpdb.DataSource;
            labelCount.Text = dataGridView1.Rows.Count.ToString();
            PowerDBLoaded = false;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::loadPowerDBDataWDateToolStripMenuItem_Click()                    |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads powerdb data into dataGrid for viewing based on date  |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void loadPowerDBDataWDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadPowerDBData(true) && LoadSubTypeMap())
            {
                PowerDBLoaded = true;
                NewTagsLoaded = false;
                dataGridView1.DataSource = PowerDBData.DataSource;
                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].ReadOnly = true;
                }
                labelCount.Text = dataGridView1.Rows.Count.ToString();
            }
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::loadPowerDBSMSToolStripMenuItem_Click()                          |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads powerdb and sms data into dataGrid for viewing        |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void loadPowerDBSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadPowerDBDataToolStripMenuItem.PerformClick();
            loadSMSDataToolStripMenuItem.PerformClick();
            labelCount.Text = dataGridView1.Rows.Count.ToString();
            if (File.Exists(textBoxLastSMSFile.Text))
            {
                ImportLastRun(textBoxLastSMSFile.Text);
                LastRunSMSLoaded = true;
            }
            compareDataToolStripMenuItem1.Enabled = (PowerDBLoaded && SMSLoaded);
            button3.Enabled = (PowerDBLoaded && SMSLoaded && !bViewer);
            button2.Enabled = (PowerDBLoaded && SMSLoaded);
            compareDataToolStripMenuItem1.PerformClick();
            compareDataToolStripMenuItem1.Enabled = false;
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::editRegionMapToolStripMenuItem_Click()                           |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method allows editing of regionmap directory                       |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void editRegionMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string startupPath = Environment.CurrentDirectory + @"\RegionMap.csv";
            System.Diagnostics.Process.Start(startupPath);
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::editSubTypeMapToolStripMenuItem_Click()                           |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method allows editing of subtype map directory                     |
|    hash                                                                    |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void editSubTypeMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string startupPath = Environment.CurrentDirectory + @"\SubTypeMap.csv";
            System.Diagnostics.Process.Start(startupPath);

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void showNeedsAttentionRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /*
/----------------------------------------------------------------------------\
| MainForm::checkBox1_CheckedChanged()                           |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method displays only the data that has changed b/t PDB & SMS       |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
                if (checkBox1.Checked)
                {
                    if (Row.Cells["ProblemColumn"].Value != null)
                    {
                        if ((Row.Cells["ProblemColumn"].Value == "X") || (Row.Cells["ProblemColumn"].Value.ToString() == "0"))
                        {
                            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                            currencyManager1.SuspendBinding();
                            Row.Visible = false;
                            currencyManager1.ResumeBinding();
                        
                        }
                    }
                }
                else
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                    currencyManager1.SuspendBinding();
                    Row.Visible = true;
                    currencyManager1.ResumeBinding();
                }

            }

            if (checkBox1.Checked)
            {
                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        /*
/----------------------------------------------------------------------------\
| MainForm::button2_Click()                           |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method generates s delta report and opens it in Excel              |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void button2_Click(object sender, EventArgs e)
        {
            bool bWriteToCsv = false;
            string deltaReportcsv = System.IO.Path.GetDirectoryName(Properties.Settings1.Default.smsdir) + "\\DeltaReportPDBtoSMS.csv";
            Cursor.Current = Cursors.WaitCursor;
            System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(deltaReportcsv);

            string strHeader = "";
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                strHeader += dataGridView1.Columns[i].HeaderText + ",";
            }
            streamWriter.WriteLine(strHeader);

            // Select Output file

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                bWriteToCsv = false;

                if (dgvr.Cells["ProblemColumn"].Value == "X" ) 
                {
                    bWriteToCsv = true;
                }
                else
                {
                    foreach (DataGridViewCell Cell in dgvr.Cells)
                    {
                        if (Cell.Style.BackColor == Color.LightGreen || Cell.Style.BackColor == Color.Red)
                        {
                            bWriteToCsv = true;
                            Cell.Value = "***" + Cell.Value + "***";
                            //break;
                        }
                    }
                }

                if (bWriteToCsv)
                {
                    // write csv
                    string strRowValue = "";
                    for (int n = 1; n < dataGridView1.Columns.Count; n++)
                    {
                        strRowValue += "\"" + dgvr.Cells[n].Value + "\",";
                    }
                    streamWriter.WriteLine(strRowValue);

                }
                
            }
            streamWriter.Close();

            System.Diagnostics.Process.Start(deltaReportcsv);
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::button3_Click()                                                  |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method pushes SMS changes to PDB                                   |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void button3_Click(object sender, EventArgs e)
        {
            string dbname = "";
            IPdbIntegrator pdbapi = null;
            int numChanged = 0;

            dbname = regionhash[dataGridView1.Rows[1].Cells["District"].Value].ToString();
            pdbapi = PdbAPI.IntegrationFactory.CreateSqlServerInterface(serverName, dbname, pdbname, pdbpswd);

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                
                if ((dgvr.Cells["ProblemColumn"].Value != "X") || (dgvr.Cells["ProblemColumn"].Value.ToString() == "0"))
                {
                    int smsindex = -1;
                    string tagId = dgvr.Cells["TAG"].Value.ToString();
                    string siteId = dgvr.Cells["SITE"].Value.ToString();
                    string colName = "";

                    foreach (DataGridViewCell Cell in dgvr.Cells)
                    {

                        if (Cell.Style.BackColor == Color.Yellow)
                        {
                            // Send to PowerDB

                            if (smshash.ContainsKey(tagId))
                            {
                                smsindex = (int)smshash[tagId];                    
                            }

                            if (smsindex != -1)
                            {
                                colName = Cell.OwningColumn.HeaderCell.Value.ToString();
                                SMSData.Position = smsindex;
                                string SmsVal = ((DataRowView)SMSData.Current)[colName].ToString();
                                UpdateTag(pdbapi, tagId, siteId, colName, SmsVal, Cell.Value.ToString());
                                numChanged = numChanged + 1;
                            }
                        }
                    }
                }                
            }

            MessageBox.Show("PowerDB update complete.\nUpdate count: " + numChanged);
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::UpdateTag()                           |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method updates the tag info in PDB                                 |
|                                                                            |
|  Parameters:                                                               |
|    pdbapi, tag, siteid, columnName, updateval, PdbVal                      |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/
        private void UpdateTag(IPdbIntegrator pdbapi, string tag, string siteid, string columnName, string updateval, string PdbVal)
        {
            if (columnName == "NAME")
            {
                string CompanyGuid = "";
                List<PdbAPI.PowerDB.Company> MatchingCompanies = new List<PdbAPI.PowerDB.Company>();
                MatchingCompanies = pdbapi.GetCompaniesWithName(updateval);

                if (MatchingCompanies.Count > 0)
                {

                    foreach (PdbAPI.PowerDB.Company ndxComp in MatchingCompanies)
                    {
                        if (ndxComp.Name.Trim().ToLower() == updateval.Trim().ToLower() && ndxComp.Deleted == false)
                        {
                            CompanyGuid = ndxComp.CompanyGUID;
                            break;
                        }
                    }
                }
                else
                {
                    MatchingCompanies = pdbapi.GetCompaniesWithName(PdbVal);

                    foreach (PdbAPI.PowerDB.Company ndxComp in MatchingCompanies)
                    {
                        if (ndxComp.Name.Trim().ToLower() == PdbVal.Trim().ToLower() && ndxComp.Deleted == false)
                        {
                            ndxComp.Name = updateval.Trim();
                            //break;
                        }

                    }
                }
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        asset.FolderLevel1 = updateval.Trim();
                        break;
                    }
                }

            }
            else if (columnName == "ADDRESS1")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.AddressLine1.ToLower().Trim() == PdbVal.ToLower().Trim() && address.Deleted == false)
                        {
                            address.AddressLine1 = updateval;
                            List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                            PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                            foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                            {
                                if (asset.Deleted == false)
                                {
                                    asset.FolderLevel2 = siteid + "-" + updateval.Trim();
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else if (columnName == "ADDRESS2")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.Deleted == false)
                        {
                            address.AddressLine2 = updateval;
                            break;
                        }
                    }
                }

            }
            else if (columnName == "CITY")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.Deleted == false)
                        {
                            address.City = updateval;
                            break;
                        }
                    }
                }

            }
            else if (columnName == "STATE")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.Deleted == false)
                        {
                            address.State = updateval;
                            break;
                        }
                    }
                }

            }
            else if (columnName == "ZIP")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.Deleted == false)
                        {
                            address.Zip = updateval;
                            break;
                        }
                    }
                }

            }
            else if (columnName == "SERIAL")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        asset.BarcodeId = updateval;
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "SerialNumber");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "SerialNumber");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else if (columnName == "PHONENUM")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.Deleted == false)
                        {
                            address.Phone = updateval;
                            break;
                        }
                    }
                }
            }
            else if (columnName == "CONTACT")
            {
                string[] nameArray = updateval.Split(',');
                string FirstName = "";
                string LastName = "";
                if (nameArray.Length > 1)
                {
                    FirstName = nameArray[1];
                    LastName = nameArray[0];
                }

                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {

                        if (address.Deleted == false)
                        {
                            address.FirstName = FirstName.Trim();
                            address.LastName = LastName.Trim();
                            break;
                        }
                    }
                }
            }
            else if (columnName == "EMAIL")
            {
                List<PdbAPI.PowerDB.Address> MatchingAddresses = new List<PdbAPI.PowerDB.Address>();
                MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid + "-2");
                if (MatchingAddresses.Count <= 0)
                {
                    MatchingAddresses = pdbapi.GetAddressesWithImportID(siteid);
                }
                if (MatchingAddresses.Count > 0)
                {
                    foreach (PdbAPI.PowerDB.Address address in MatchingAddresses)
                    {
                        if (address.Deleted == false)
                        {
                            address.Email = updateval;
                            break;
                        }
                    }
                }

            }
            else if (columnName == "MODEL")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "Model");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "Model");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else if (columnName == "MANUFACTURER")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "Manufacturer");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "Manufacturer");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }

            }
            else if (columnName == "SYSTEMNUMBER")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "SystemNumber");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "SystemNumber");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else if (columnName == "SYSTEMNAME")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "SystemName");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "SystemName");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else if (columnName == "SYSTEMDESCR")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "SystemDescr");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "SystemDescr");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else if (columnName == "PARENT")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "Parent");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "Parent");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else if (columnName == "CHILDREN")
            {
                List<PdbAPI.PowerDB.Asset> PdbRelays = new List<PdbAPI.PowerDB.Asset>();
                PdbAPI.PowerDB.AssetAttribute attribute = null;
                PdbRelays = pdbapi.GetAssetsWithAssetID(tag);
                foreach (PdbAPI.PowerDB.Asset asset in PdbRelays)
                {
                    if (asset.Deleted == false)
                    {
                        attribute = pdbapi.GetExistingAssetAttributeForAsset(asset, "Children");
                        if (attribute != null)
                        {
                            attribute.StrVal = updateval;
                        }
                        else
                        {
                            attribute = pdbapi.AddNewAssetAttributeToAsset(asset, "Children");
                            attribute.StrVal = (updateval == null) ? "" : updateval;
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("The following column cannot be updated with this tool: " + columnName);
            }
            pdbapi.Update();
            
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::button1_Click_1()                                                  |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method loads the SMS last run csv file                             |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = Properties.Settings1.Default.smsdir;
            fdlg.Filter = "Comma Delimited (*.csv)|*.csv|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK && fdlg.FileName.Trim() != string.Empty)
            {
                try
                {
                    ImportLastRun(fdlg.FileName);
                    LastRunSMSLoaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                Properties.Settings1.Default.lastrundir = fdlg.FileName;
                Properties.Settings1.Default.Save();
                textBoxLastSMSFile.Text = Properties.Settings1.Default.lastrundir;
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /*
/----------------------------------------------------------------------------\
| MainForm::editViewerAccountsToolStripMenuItem_Click()                      |
|----------------------------------------------------------------------------|
|  Description:                                                              |
|    This method allows the user to edit the viewer user account file        |
|                                                                            |
|  Parameters:                                                               |
|    sender, e                                                               |
|                                                                            |
|  Return Value:                                                             |
|    none                                                                    |
|                                                                            |
\----------------------------------------------------------------------------/
*/

        private void editViewerAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string startupPath = Environment.CurrentDirectory + @"\UserAccounts.txt";
            System.Diagnostics.Process.Start(startupPath);
        }

        private void textBoxLastSMSFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}