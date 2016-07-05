namespace Import
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.checkContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.unSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProblemColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPowerDBSMSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSMSDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPowerDBDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPowerDBDataWDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewTagsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.compareDataToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTagsNotInPdbToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRegionMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSubTypeMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editViewerAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PowerDBData = new System.Windows.Forms.BindingSource(this.components);
            this.dataPanel = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxLastSMSFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SMSData = new System.Windows.Forms.BindingSource(this.components);
            this.LastRunSMSData = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.checkContext.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerDBData)).BeginInit();
            this.dataPanel.SuspendLayout();
            this.statusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SMSData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunSMSData)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkBox,
            this.ProblemColumn});
            this.dataGridView1.Location = new System.Drawing.Point(0, 56);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(787, 236);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // checkBox
            // 
            this.checkBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.checkBox.ContextMenuStrip = this.checkContext;
            this.checkBox.HeaderText = "Add";
            this.checkBox.Name = "checkBox";
            this.checkBox.Width = 32;
            // 
            // checkContext
            // 
            this.checkContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAll,
            this.unSelectAllToolStripMenuItem});
            this.checkContext.Name = "contextMenuStrip1";
            this.checkContext.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAll
            // 
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(137, 22);
            this.selectAll.Text = "Select All";
            this.selectAll.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // unSelectAllToolStripMenuItem
            // 
            this.unSelectAllToolStripMenuItem.Name = "unSelectAllToolStripMenuItem";
            this.unSelectAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.unSelectAllToolStripMenuItem.Text = "UnSelect All";
            this.unSelectAllToolStripMenuItem.Click += new System.EventHandler(this.unSelectAllToolStripMenuItem_Click);
            // 
            // ProblemColumn
            // 
            this.ProblemColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ProblemColumn.HeaderText = "P";
            this.ProblemColumn.MaxInputLength = 10;
            this.ProblemColumn.Name = "ProblemColumn";
            this.ProblemColumn.ReadOnly = true;
            this.ProblemColumn.Width = 39;
            // 
            // ControlPanel
            // 
            this.ControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlPanel.Controls.Add(this.label2);
            this.ControlPanel.Controls.Add(this.dateTimePicker1);
            this.ControlPanel.Controls.Add(this.menuStrip1);
            this.ControlPanel.Location = new System.Drawing.Point(0, 1);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(787, 25);
            this.ControlPanel.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Date Modified:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(408, 3);
            this.dateTimePicker1.MaxDate = new System.DateTime(2040, 12, 31, 0, 0, 0, 0);
            this.dateTimePicker1.MinDate = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(108, 20);
            this.dateTimePicker1.TabIndex = 34;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.operationsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(787, 24);
            this.menuStrip1.TabIndex = 33;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewTagsToolStripMenuItem,
            this.loadPowerDBSMSToolStripMenuItem,
            this.loadSMSDataToolStripMenuItem,
            this.loadPowerDBDataToolStripMenuItem,
            this.loadPowerDBDataWDateToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // addNewTagsToolStripMenuItem
            // 
            this.addNewTagsToolStripMenuItem.Name = "addNewTagsToolStripMenuItem";
            this.addNewTagsToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.addNewTagsToolStripMenuItem.Text = "Load New Data";
            this.addNewTagsToolStripMenuItem.Click += new System.EventHandler(this.addNewTagsToolStripMenuItem_Click);
            // 
            // loadPowerDBSMSToolStripMenuItem
            // 
            this.loadPowerDBSMSToolStripMenuItem.Name = "loadPowerDBSMSToolStripMenuItem";
            this.loadPowerDBSMSToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.loadPowerDBSMSToolStripMenuItem.Text = "Load PowerDB/Oracle";
            this.loadPowerDBSMSToolStripMenuItem.Click += new System.EventHandler(this.loadPowerDBSMSToolStripMenuItem_Click);
            // 
            // loadSMSDataToolStripMenuItem
            // 
            this.loadSMSDataToolStripMenuItem.Name = "loadSMSDataToolStripMenuItem";
            this.loadSMSDataToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.loadSMSDataToolStripMenuItem.Text = "Load Oracle Data";
            this.loadSMSDataToolStripMenuItem.Click += new System.EventHandler(this.loadSMSDataToolStripMenuItem_Click);
            // 
            // loadPowerDBDataToolStripMenuItem
            // 
            this.loadPowerDBDataToolStripMenuItem.Name = "loadPowerDBDataToolStripMenuItem";
            this.loadPowerDBDataToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.loadPowerDBDataToolStripMenuItem.Text = "Load PowerDB Data";
            this.loadPowerDBDataToolStripMenuItem.Click += new System.EventHandler(this.loadPowerDBDataToolStripMenuItem_Click);
            // 
            // loadPowerDBDataWDateToolStripMenuItem
            // 
            this.loadPowerDBDataWDateToolStripMenuItem.Name = "loadPowerDBDataWDateToolStripMenuItem";
            this.loadPowerDBDataWDateToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.loadPowerDBDataWDateToolStripMenuItem.Text = "Load Modified PowerDB Data";
            this.loadPowerDBDataWDateToolStripMenuItem.Click += new System.EventHandler(this.loadPowerDBDataWDateToolStripMenuItem_Click);
            // 
            // operationsToolStripMenuItem
            // 
            this.operationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToCSVToolStripMenuItem,
            this.addNewTagsToolStripMenuItem1,
            this.compareDataToolStripMenuItem1,
            this.runToolStripMenuItem,
            this.showTagsNotInPdbToolStripMenuItem});
            this.operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
            this.operationsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.operationsToolStripMenuItem.Text = "Operations";
            this.operationsToolStripMenuItem.Click += new System.EventHandler(this.operationsToolStripMenuItem_Click);
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exportToCSVToolStripMenuItem.Text = "Export to CSV";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // addNewTagsToolStripMenuItem1
            // 
            this.addNewTagsToolStripMenuItem1.Enabled = false;
            this.addNewTagsToolStripMenuItem1.Name = "addNewTagsToolStripMenuItem1";
            this.addNewTagsToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.addNewTagsToolStripMenuItem1.Text = "Add New Tags";
            this.addNewTagsToolStripMenuItem1.Click += new System.EventHandler(this.addNewTagsToolStripMenuItem1_Click);
            // 
            // compareDataToolStripMenuItem1
            // 
            this.compareDataToolStripMenuItem1.Enabled = false;
            this.compareDataToolStripMenuItem1.Name = "compareDataToolStripMenuItem1";
            this.compareDataToolStripMenuItem1.Size = new System.Drawing.Size(204, 22);
            this.compareDataToolStripMenuItem1.Text = "Compare All Data";
            this.compareDataToolStripMenuItem1.Click += new System.EventHandler(this.compareDataToolStripMenuItem1_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.runToolStripMenuItem.Text = "Show Tags Not in Oracle";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // showTagsNotInPdbToolStripMenuItem
            // 
            this.showTagsNotInPdbToolStripMenuItem.Name = "showTagsNotInPdbToolStripMenuItem";
            this.showTagsNotInPdbToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showTagsNotInPdbToolStripMenuItem.Text = "Show Tags Not in Pdb";
            this.showTagsNotInPdbToolStripMenuItem.Click += new System.EventHandler(this.showTagsNotInPdbToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editRegionMapToolStripMenuItem,
            this.editSubTypeMapToolStripMenuItem,
            this.editViewerAccountsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // editRegionMapToolStripMenuItem
            // 
            this.editRegionMapToolStripMenuItem.Name = "editRegionMapToolStripMenuItem";
            this.editRegionMapToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editRegionMapToolStripMenuItem.Text = "Edit RegionMap";
            this.editRegionMapToolStripMenuItem.Click += new System.EventHandler(this.editRegionMapToolStripMenuItem_Click);
            // 
            // editSubTypeMapToolStripMenuItem
            // 
            this.editSubTypeMapToolStripMenuItem.Name = "editSubTypeMapToolStripMenuItem";
            this.editSubTypeMapToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editSubTypeMapToolStripMenuItem.Text = "Edit SubTypeMap";
            this.editSubTypeMapToolStripMenuItem.Click += new System.EventHandler(this.editSubTypeMapToolStripMenuItem_Click);
            // 
            // editViewerAccountsToolStripMenuItem
            // 
            this.editViewerAccountsToolStripMenuItem.Name = "editViewerAccountsToolStripMenuItem";
            this.editViewerAccountsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editViewerAccountsToolStripMenuItem.Text = "Edit Viewer Accounts";
            this.editViewerAccountsToolStripMenuItem.Click += new System.EventHandler(this.editViewerAccountsToolStripMenuItem_Click);
            // 
            // dataPanel
            // 
            this.dataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataPanel.Controls.Add(this.button3);
            this.dataPanel.Controls.Add(this.button2);
            this.dataPanel.Controls.Add(this.button1);
            this.dataPanel.Controls.Add(this.textBoxLastSMSFile);
            this.dataPanel.Controls.Add(this.label5);
            this.dataPanel.Controls.Add(this.label4);
            this.dataPanel.Controls.Add(this.checkBox1);
            this.dataPanel.Controls.Add(this.dataGridView1);
            this.dataPanel.Location = new System.Drawing.Point(0, 28);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size(787, 316);
            this.dataPanel.TabIndex = 30;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(571, 294);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(216, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Push Oracle Data to PowerDB";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(0, 293);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(216, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Generate Delta Report";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(737, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBoxLastSMSFile
            // 
            this.textBoxLastSMSFile.Location = new System.Drawing.Point(86, 1);
            this.textBoxLastSMSFile.Name = "textBoxLastSMSFile";
            this.textBoxLastSMSFile.ReadOnly = true;
            this.textBoxLastSMSFile.Size = new System.Drawing.Size(645, 20);
            this.textBoxLastSMSFile.TabIndex = 6;
            this.textBoxLastSMSFile.TextChanged += new System.EventHandler(this.textBoxLastSMSFile_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Last Oracle File:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(441, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(342, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Red = Needs Attention. Green = Push to Oracle; Yellow = Push to PDB";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(3, 33);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Only Show Different";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // statusPanel
            // 
            this.statusPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusPanel.Controls.Add(this.label1);
            this.statusPanel.Controls.Add(this.labelCount);
            this.statusPanel.Controls.Add(this.progressBar1);
            this.statusPanel.Location = new System.Drawing.Point(0, 350);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(787, 25);
            this.statusPanel.TabIndex = 3;
            this.statusPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.statusPanel_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(380, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "*";
            // 
            // labelCount
            // 
            this.labelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(744, 6);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(0, 13);
            this.labelCount.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(0, 1);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(293, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 1;
            this.progressBar1.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 375);
            this.Controls.Add(this.dataPanel);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.statusPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Data Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.checkContext.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PowerDBData)).EndInit();
            this.dataPanel.ResumeLayout(false);
            this.dataPanel.PerformLayout();
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SMSData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastRunSMSData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.BindingSource PowerDBData;
        private System.Windows.Forms.BindingSource NewTags;
        private System.Windows.Forms.BindingSource SMSMissingTags;
        private System.Windows.Forms.BindingSource PowerDBNewTags;
        private System.Collections.Hashtable pdbhash;
        private System.Collections.Hashtable smshash;
        private System.Collections.Hashtable lastrunsmshash;
        private System.Collections.Hashtable regionhash;
        private System.Collections.Hashtable subtypehash;
        private System.Windows.Forms.Panel dataPanel;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.Label labelCount;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip checkContext;
        private System.Windows.Forms.ToolStripMenuItem selectAll;
        private PdbAPI.IPdbIntegrator m_PdbAPI;
        public LoginDlg Logdlg;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewTagsToolStripMenuItem;
        private bool PowerDBLoaded, NewTagsLoaded, SMSLoaded, LastRunSMSLoaded, bViewer;
        private System.Windows.Forms.BindingSource SMSData;
        private string serverName,pdbname,pdbpswd, database, regions;
        private string[] accounts;
        private System.Windows.Forms.ToolStripMenuItem operationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewTagsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem compareDataToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem unSelectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn checkBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProblemColumn;
        private System.Windows.Forms.ToolStripMenuItem showTagsNotInPdbToolStripMenuItem;
        public System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ToolStripMenuItem loadPowerDBDataWDateToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem loadPowerDBSMSToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem loadSMSDataToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem loadPowerDBDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editRegionMapToolStripMenuItem;
        private System.Collections.Generic.List<string> InvalidRegions;
        private System.Collections.Generic.List<string> InvalidSubType;
        private System.Windows.Forms.ToolStripMenuItem editSubTypeMapToolStripMenuItem;
        private System.Windows.Forms.BindingSource LastRunSMSData;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxLastSMSFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem editViewerAccountsToolStripMenuItem;
    }
}

