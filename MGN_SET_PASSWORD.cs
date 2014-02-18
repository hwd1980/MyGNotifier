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
    public partial class MGN_SET_PASSWORD : Form
    {
        public MGN_SET_PASSWORD()
        {
            InitializeComponent();
        }

        public bool bOK = false;

        private void btnAccedi_Click(object sender, EventArgs e)
        {
            if (txtPwd.Text != "")
            {
                if (txtPwd2.Text != "")
                {
                    if (txtPwd.Text == txtPwd2.Text)
                    {
                        try
                        {
                            MyGNotifier.Properties.Settings.Default.MAIN_PASSWORD = txtPwd2.Text;
                            MyGNotifier.Properties.Settings.Default.Save();
                            DialogResult r = MessageBox.Show("PASSWORD DI AMMINISTRATORE IMPOSTATA CON SUCCESSO!", "MyGNotifier", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (r == DialogResult.OK)
                            {
                                bOK = true;
                                this.Hide();
                                var MyG = new MGN_IMPOSTAZIONI();
                                MyG.Show();
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("E' AVVENUTO UN ERRORE DURANTE IL TENTATIVO DI SALVARE LA PASSWORD:\n" + err.Message, "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("LE PASSWORD IMMESSE SONO DIVERSE!\nRICONTROLLARE!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("RIPETERE LA PASSWORD IMMESSA!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("IMPOSTARE UNA PASSWORD!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                txtPwd2.Focus();
            }
        }

        private void txtPwd2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.Invoke(new EventHandler(btnAccedi_Click));
            }
        }

    }
}
