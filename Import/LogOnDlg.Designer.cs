namespace Import
{
    partial class LoginDlg
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
            this.ServerName = new System.Windows.Forms.TextBox();
            this.pdbUserName = new System.Windows.Forms.TextBox();
            this.pdbPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dbslist = new System.Windows.Forms.ComboBox();
            this.regionslist = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ServerName
            // 
            this.ServerName.Location = new System.Drawing.Point(113, 25);
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(226, 20);
            this.ServerName.TabIndex = 0;
            this.ServerName.Leave += new System.EventHandler(this.ServerName_Leave);
            // 
            // pdbUserName
            // 
            this.pdbUserName.Location = new System.Drawing.Point(115, 440);
            this.pdbUserName.Name = "pdbUserName";
            this.pdbUserName.Size = new System.Drawing.Size(224, 20);
            this.pdbUserName.TabIndex = 1;
            // 
            // pdbPassword
            // 
            this.pdbPassword.Location = new System.Drawing.Point(115, 486);
            this.pdbPassword.Name = "pdbPassword";
            this.pdbPassword.PasswordChar = '*';
            this.pdbPassword.Size = new System.Drawing.Size(224, 20);
            this.pdbPassword.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server Name:";
            this.label1.UseWaitCursor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 447);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "User Name:";
            this.label2.UseWaitCursor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 490);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password:";
            this.label3.UseWaitCursor = true;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(242, 526);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(94, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(131, 526);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 23);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "CANCEL";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Database:";
            this.label4.UseWaitCursor = true;
            // 
            // dbslist
            // 
            this.dbslist.FormattingEnabled = true;
            this.dbslist.Location = new System.Drawing.Point(113, 62);
            this.dbslist.Name = "dbslist";
            this.dbslist.Size = new System.Drawing.Size(223, 21);
            this.dbslist.TabIndex = 10;
            this.dbslist.SelectedIndexChanged += new System.EventHandler(this.dbslist_SelectedIndexChanged);
            // 
            // regionslist
            // 
            this.regionslist.FormattingEnabled = true;
            this.regionslist.Location = new System.Drawing.Point(113, 101);
            this.regionslist.Name = "regionslist";
            this.regionslist.Size = new System.Drawing.Size(223, 319);
            this.regionslist.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Regions:";
            this.label5.UseWaitCursor = true;
            // 
            // LoginDlg
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 600);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.regionslist);
            this.Controls.Add(this.dbslist);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pdbPassword);
            this.Controls.Add(this.pdbUserName);
            this.Controls.Add(this.ServerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Logon";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox pdbPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox ServerName;
        public System.Windows.Forms.TextBox pdbUserName;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        public bool bRun;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox dbslist;
        public System.Windows.Forms.CheckedListBox regionslist;
        private System.Windows.Forms.Label label5;
    }
}