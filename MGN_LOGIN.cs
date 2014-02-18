using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyGNotifier
{
    public partial class MGN_LOGIN : Form
    {
        public MGN_LOGIN()
        {
            InitializeComponent();
        }

        private void MGN_LOGIN_Shown(object sender, EventArgs e)
        {
            txtPwd.Focus();
        }

        private void btnAccedi_Click(object sender, EventArgs e)
        {
            if (txtPwd.Text != "")
            {
                string sP = txtPwd.Text;
                if (sP == MyGNotifier.Properties.Settings.Default.MAIN_PASSWORD)
                {
                    this.Hide();
                    var MyG = new MGN_IMPOSTAZIONI();
                    MyG.Show();
                }
                else
                {
                    MessageBox.Show("PASSWORD ERRATA!\nRIPROVARE!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("E' NECESSARIO SCRIVERE UNA PASSWORD", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            { 
                this.Invoke(new EventHandler(btnAccedi_Click));
            }
        }

        
    }
}
