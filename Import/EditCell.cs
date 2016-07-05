using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Import
{
    public partial class EditValueForm : Form
    {
        public EditValueForm()
        {
            InitializeComponent();

            bReturn = false;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bReturn = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            bReturn = false;
            Close();
        }

        private void EditValueForm_Load(object sender, EventArgs e)
        {
            if (radioButton1.Text == string.Empty)
            {
                radioButton1.Visible = false;
            }
            if (radioButton2.Text == string.Empty)
            {
                radioButton2.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
