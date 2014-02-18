using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.IO;
using System.Collections;

using System.Diagnostics;

using System.Net.Mail;
using ActiveUp.Net.Mail;
using ActiveUp.Net.Security;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using Telerik.WinControls;
using Telerik.WinControls.UI;

using System.Resources;


namespace MyGNotifier
{
    public partial class MGN_IMPOSTAZIONI : Form
    {
        public MGN_IMPOSTAZIONI()
        {
            InitializeComponent();
        }

        # region VARIBILI CONNESSIONE

        public string SERVER;
        public int PORTA;
        public string USER;
        public string PWD;
        public string STATUS_OK;
        public string STATUS_NO;

        # endregion

        # region VARIABILI APPLICAZIONE

        public bool bClose;
        public bool bIMAP_ERROR;
        public string MGN_ERROR;
        public bool bNewMsgICON;

        public bool bSETTINGS;
        public bool NEW_MSGS;
        public string MSGS_DETAILS;
        public bool bAlreadyConnect;

        public List<string> MSGS_ID;

        public Imap4Client GMAIL_IMAP;

        public int iCountIDS;
        public int iLastCOUNT;

        # endregion

        # region EVENTI FORM

        private void MGN_IMPOSTAZIONI_Load(object sender, EventArgs e)
        {
            //TRANSLATE_ALL();

            nIcon.Text = "INIZIALIZZAZIONE CONTROLLO POSTA IN CORSO...";

            if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME != "")
            {
                radGNotifier.ThemeName = MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME;
            }
            if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
            {
                RAD_INIT();
            }

            bNewMsgICON = false;
            bAlreadyConnect = false;
            Hide();
            FILL_SETTINGS();
            StartStopTimer();
            MSGS_ID = new List<string>();

            //IMAP CONNECTION
            if (bSETTINGS)
            {
                if (MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE)
                {
                    CONNECT_IMAP();
                }
            }
        }

        public void CONNECT_IMAP()
        {
            try
            {
                GMAIL_IMAP = new Imap4Client();
                GMAIL_IMAP.ConnectSsl(SERVER, PORTA);
                GMAIL_IMAP.Login(USER, PWD);
                bIMAP_ERROR = false;
                btnRECONNECT.Visible = false;
            }
            catch (Exception errCONN)
            {
                nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Error;
                btnRECONNECT.Visible = true;
                TimerGNotifier.Enabled = false;
                bIMAP_ERROR = true;
                rtError.Text += DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP:\n" + errCONN.Message + "\n------------------\n";
                MGN_ERROR = DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP: " + errCONN.Message;
                if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR)
                {
                    NOTIFY_ERROR(MGN_ERROR);
                }
            }
        }

        private void btnRECONNECT_Click(object sender, EventArgs e)
        {
            CONNECT_IMAP();
        }

        private void MGN_IMPOSTAZIONI_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
                nIcon.Visible = true;
            }
            else if (FormWindowState.Normal == WindowState)
            {
                Show();
                nIcon.Visible = false;
            }
        }

        private void MGN_IMPOSTAZIONI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bClose)
            {
                DialogResult oConfirm = MessageBox.Show("Intendi davvero chiudere l'applicazione?", "CONFERMA CHIUSURA APPLICAZIONE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (oConfirm == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE)
                    {
                        try
                        {
                            GMAIL_IMAP.Disconnect();
                        }
                        catch { }
                    }
                    Process o = Process.GetCurrentProcess();
                    try
                    {
                        o.Kill();
                    }
                    catch (Exception errProcess)
                    {
                        MessageBox.Show("Errore durante il tentativo di CHIUDERE l'Applicazione.\nIMPOSSIBILE TERMINARE IL PROCESSO ATTIVO:\n" + errProcess.Message, "ERRORE CHIUSURA APPLICAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        # endregion

        # region IMPOSTAZIONI

        public void StartStopTimer()
        {
            btnTimer.Text = TimerGNotifier.Enabled ? "STOP TIMER" : "START TIMER";
            btnTimer.ForeColor = TimerGNotifier.Enabled ? Color.Red : Color.Green;
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            if (TimerGNotifier.Enabled)
            {
                TimerGNotifier.Enabled = false;
            }
            else
            {
                TimerGNotifier.Enabled = true;
            }

            StartStopTimer();
        }

        public void FILL_SETTINGS()
        {
            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
            bSETTINGS = false;
            SERVER = MyGNotifier.Properties.Settings.Default.IMAP_SERVER;
            PORTA = MyGNotifier.Properties.Settings.Default.IMAP_PORT;
            USER = MyGNotifier.Properties.Settings.Default.IMAP_USERNAME;
            PWD = MyGNotifier.Properties.Settings.Default.IMAP_PASSWORD;
            bSETTINGS = SERVER != "" && PORTA != 0 && USER != "" && PWD != "" ? true : false;

            if (bSETTINGS)
            {
                lblSettingStatus.Text = "IMPOSTAZIONI DI CONNESSIONE OK!";
                lblSettingStatus.ForeColor = Color.Blue;
                txtIMAP_SERVER.Text = SERVER;
                txtPORTA_IMAP.Text = PORTA.ToString();
                txtUSERNAME.Text = USER;
                txtPASSWORD.Text = PWD;

                txtMainPWD.Text = MyGNotifier.Properties.Settings.Default.MAIN_PASSWORD;
                chkMainPwd.Checked = MyGNotifier.Properties.Settings.Default.MAIN_PASS_SHOW;
                chkDOPPIO_CLICK.Checked = MyGNotifier.Properties.Settings.Default.MAIN_DOUBLE_CLICK_GMAIL;

                chkMOSTRA_NOTIFICA_ERRORE.Checked = MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR;
                chkMOSTRA_NOTIFICHE_MESSAGGI_AUTO.Checked = MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_MSGS;

                nudTIMER.Value = MyGNotifier.Properties.Settings.Default.MAIN_TIMER_REFRESH;

                //chkKEEP_CONNECTION_ALIVE.Checked = MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE;
                chkCHECK_MONITOR.Checked = MyGNotifier.Properties.Settings.Default.MAIN_CHECK_MONITOR;

                chkRAD_NOTIFIER.Checked = MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER;

                nudAltezzaNotifica.Value = MyGNotifier.Properties.Settings.Default.MAIN_NOTIFIER_HEIGHT;

                chkCLEAN_LOGS.Checked = MyGNotifier.Properties.Settings.Default.MAIN_CLEAN_LOGS;
                nudCleanLogsInterval.Value = MyGNotifier.Properties.Settings.Default.MAIN_CLEAN_LOGS_INTERVAL;

                if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME != "")
                {
                    cbRadTheme_DEFAULT.SelectedIndex = cbRadTheme_DEFAULT.Items.IndexOf(MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME);
                }

                if (nudTIMER.Value > 0)
                {
                    TimerGNotifier.Interval = Convert.ToInt32(nudTIMER.Value);
                }

                TimerGNotifier.Enabled = true;

                if (chkCLEAN_LOGS.Checked)
                {
                    TimerCleanLOGS.Interval = Convert.ToInt32(nudCleanLogsInterval.Value);
                    TimerCleanLOGS.Enabled = true;
                }
                else
                {
                    TimerCleanLOGS.Enabled = false;
                }
            }
            else
            {
                Show();
                lblSettingStatus.ForeColor = Color.Red;
                lblSettingStatus.Text = "IMPOSTAZIONI DI CONNESSIONE ASSENTI! PROCEDERE ALLA MEMORIZZAZIONE!";
                TimerGNotifier.Enabled = false;

                txtMainPWD.Text = MyGNotifier.Properties.Settings.Default.MAIN_PASSWORD;
                chkMainPwd.Checked = MyGNotifier.Properties.Settings.Default.MAIN_PASS_SHOW;
                chkDOPPIO_CLICK.Checked = MyGNotifier.Properties.Settings.Default.MAIN_DOUBLE_CLICK_GMAIL;

                chkMOSTRA_NOTIFICA_ERRORE.Checked = MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR;
                chkMOSTRA_NOTIFICHE_MESSAGGI_AUTO.Checked = MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_MSGS;

                nudTIMER.Value = MyGNotifier.Properties.Settings.Default.MAIN_TIMER_REFRESH;
                //chkKEEP_CONNECTION_ALIVE.Checked = MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE;
                chkCHECK_MONITOR.Checked = MyGNotifier.Properties.Settings.Default.MAIN_CHECK_MONITOR;

                chkRAD_NOTIFIER.Checked = MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER;

                nudAltezzaNotifica.Value = MyGNotifier.Properties.Settings.Default.MAIN_NOTIFIER_HEIGHT;

                if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME != "")
                {
                    cbRadTheme_DEFAULT.SelectedIndex = cbRadTheme_DEFAULT.Items.IndexOf(MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME);
                }

                if (nudTIMER.Value > 0)
                {
                    TimerGNotifier.Interval = Convert.ToInt32(nudTIMER.Value);
                }

                if (chkCLEAN_LOGS.Checked)
                {
                    TimerCleanLOGS.Interval = Convert.ToInt32(nudCleanLogsInterval.Value);
                    TimerCleanLOGS.Enabled = true;
                }
                else
                {
                    TimerCleanLOGS.Enabled = false;
                }
            }
        }

        private void btnSalvaImpostazioni_Click(object sender, EventArgs e)
        {
            if (TimerGNotifier.Enabled)
            {
                TimerGNotifier.Enabled = false;
                StartStopTimer();
            }
            if (txtIMAP_SERVER.Text != "" && txtPORTA_IMAP.Text != "" && txtPORTA_IMAP.Text != "0" && txtUSERNAME.Text != "" && txtPASSWORD.Text != "" && txtMainPWD.Text != "")
            {
                try
                {
                    int iPorta = Convert.ToInt32(txtPORTA_IMAP.Text);
                    MyGNotifier.Properties.Settings.Default.IMAP_SERVER = txtIMAP_SERVER.Text;
                    MyGNotifier.Properties.Settings.Default.IMAP_PORT = iPorta;
                    MyGNotifier.Properties.Settings.Default.IMAP_USERNAME = txtUSERNAME.Text;
                    MyGNotifier.Properties.Settings.Default.IMAP_PASSWORD = txtPASSWORD.Text;

                    MyGNotifier.Properties.Settings.Default.MAIN_PASSWORD = txtMainPWD.Text;
                    MyGNotifier.Properties.Settings.Default.MAIN_PASS_SHOW = chkMainPwd.Checked;
                    MyGNotifier.Properties.Settings.Default.MAIN_DOUBLE_CLICK_GMAIL = chkDOPPIO_CLICK.Checked;

                    MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR = chkMOSTRA_NOTIFICA_ERRORE.Checked;
                    MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_MSGS = chkMOSTRA_NOTIFICHE_MESSAGGI_AUTO.Checked;

                    MyGNotifier.Properties.Settings.Default.MAIN_TIMER_REFRESH = Convert.ToInt32(nudTIMER.Value);
                    //MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE = chkKEEP_CONNECTION_ALIVE.Checked;
                    MyGNotifier.Properties.Settings.Default.MAIN_CHECK_MONITOR = chkCHECK_MONITOR.Checked;

                    MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER = chkRAD_NOTIFIER.Checked;

                    MyGNotifier.Properties.Settings.Default.MAIN_NOTIFIER_HEIGHT = Convert.ToInt32(nudAltezzaNotifica.Value);

                    MyGNotifier.Properties.Settings.Default.MAIN_CLEAN_LOGS = chkCLEAN_LOGS.Checked;
                    MyGNotifier.Properties.Settings.Default.MAIN_CLEAN_LOGS_INTERVAL = Convert.ToInt32(nudCleanLogsInterval.Value);

                    if (cbRadTheme_DEFAULT.SelectedIndex != -1)
                    {
                        MyGNotifier.Properties.Settings.Default.MAIN_RAD_THEME = cbRadTheme_DEFAULT.SelectedItem.ToString();
                    }

                    if (chkCLEAN_LOGS.Checked)
                    {
                        TimerCleanLOGS.Interval = Convert.ToInt32(nudCleanLogsInterval.Value);
                        TimerCleanLOGS.Enabled = true;
                    }
                    else
                    {
                        TimerCleanLOGS.Enabled = false;
                    }

                    try
                    {
                        MyGNotifier.Properties.Settings.Default.Save();
                        DialogResult R = MessageBox.Show("IMPOSTAZIONI MEMORIZZATE CON SUCCESSO!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (R == DialogResult.OK)
                        {
                            bClose = true;
                            Application.Restart();
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("ERRORE DURANTE IL TENTATIVO DI MEMORIZZARE LE IMPOSTAZIONI:\n" + err.Message, "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    MessageBox.Show("IMMETTERE UN VALORE PORTA NUMERICO VALIDO!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("CONTROLLARE CHE TUTTE LE IMPOSTAZIONI SIANO PRESENTI!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConnectionTest_Click(object sender, EventArgs e)
        {
            if (TimerGNotifier.Enabled)
            {
                TimerGNotifier.Enabled = false;
                StartStopTimer();
            }
            
            bool bOK = true;
            string RESULT = "";

            # region CONNECTION ATTEMPT
            Imap4Client imap = new Imap4Client();
            try
            {
                imap.ConnectSsl(txtIMAP_SERVER.Text, Convert.ToInt16(txtPORTA_IMAP.Text));
            }
            catch
            {
                bOK = false;
                RESULT += "IMPOSSIBILE STABILIRE CONNESSINE SSL!\nVERIFICARE SERVER e PORTA!\n";
            }
            try
            {
                imap.Login(txtUSERNAME.Text, txtPASSWORD.Text);
            }
            catch
            {
                bOK = false;
                RESULT += "NOME UTENTE O PASSWORD NON CORRETTI!\n";
            }
            try
            {
                imap.Disconnect();
            } catch { }
            
            # endregion

            if (bOK)
            {
                MessageBox.Show("LE IMPOSTAZIONI DI CONNESSIONE SONO CORRETTE!", "CONNESSIONE OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("LE IMPOSTAZIONI DI CONNESSIONE NON SONO CORRETTE!\nPREGO VERIFICARE QUANTO SEGUE:\n" + RESULT, "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (bOK)
            {
                if (!TimerGNotifier.Enabled)
                {
                    TimerGNotifier.Enabled = true;
                    StartStopTimer();
                }
            }
        }

        # endregion

        # region FUNZIONI MyGNotifier

        public void MYG_IMAP_READ_AND_MARK(Imap4Client imap)
        {
            if (!bIMAP_ERROR)
            {
                nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Checking;
                try
                {
                    imap.Command("capability");

                    Mailbox inbox = imap.SelectMailbox("inbox");
                    int[] ids = inbox.Search("UNSEEN");
                    if (ids.Length > 0)
                    {
                        iCountIDS = 0;
                        for (var i = 0; i < ids.Length; i++)
                        {
                            if (!MSGS_ID.Contains(ids[i].ToString()))
                            {
                                iCountIDS++;
                                MSGS_ID.Add(ids[i].ToString());
                            }
                        }

                        if (iCountIDS > 0)
                        {
                            iLastCOUNT = iCountIDS;
                            MSGS_DETAILS = "";
                            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Check;
                            bNewMsgICON = true;
                            NEW_MSGS = true;

                            //ActiveUp.Net.Mail.Message msg_first = inbox.Fetch.MessageObject(ids[0]);

                            //XElement xmail = new XElement("gmail",
                            //    new XAttribute("count", ids.Length.ToString()),
                            //    new XAttribute("modified", msg_first.Date.ToString())
                            //);

                            //string name = "", address = "", from = "";
                            //Regex reg_name = new Regex("\"[^\"]+");
                            //Regex reg_address = new Regex("<[^>]+");

                            ActiveUp.Net.Mail.Message msg = null;

                            for (var i = 0; i < ids.Length; i++)
                            {
                                msg = inbox.Fetch.MessageObject(ids[i]);

                                //from = msg.HeaderFields["from"];
                                //name = reg_name.Match(from).Value.Replace("\"", "");
                                //address = reg_address.Match(from).Value.Replace("<", "");

                                rtLog.Text += DateTime.Now.ToString() + "\n";
                                rtLog.Text += "DATA: " + msg.Date + "\n";
                                rtLog.Text += "MITTENTE: " + msg.From + "\n";
                                rtLog.Text += "OGGETTO: " + msg.Subject + "\n";
                                rtLog.Text += "----------------------------\n";

                                MSGS_DETAILS += "DATA: " + msg.Date + "\n";
                                MSGS_DETAILS += "MITTENTE: " + msg.From + "\n";
                                MSGS_DETAILS += "OGGETTO: " + msg.Subject + "\n";
                                MSGS_DETAILS += "---------------------------\n";

                                //xmail.Add(new XElement("entry",
                                //    new XAttribute("id", msg.MessageId),
                                //    new XAttribute("modified", msg.Date.ToString()),
                                //    new XAttribute("name", name),
                                //    new XAttribute("address", address),
                                //    new XElement("subject", msg.Subject),
                                //    new XElement("body-text", msg.BodyText.TextStripped),
                                //    new XElement("body-html", msg.BodyHtml.Text)
                                //));
                                //mark as unread
                                var flags = new FlagCollection();
                                flags.Add("Seen");
                                inbox.RemoveFlags(ids[i], flags);
                            }

                            //File.WriteAllText("gmail.xml", xmail.ToString());
                        }
                        else
                        {
                            NEW_MSGS = false;
                            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
                        }
                    }
                    else
                    {
                        NEW_MSGS = false;
                        nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
                    }
                }
                catch (Exception err)
                {
                    nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Error;
                    rtError.Text += DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP:\n" + err.Message + "\n------------------\n"; ;
                    MGN_ERROR = DateTime.Now.ToString() + "-> ERRORE ELABORAZIONE IMAP: " + err.Message;
                    if (MGN_ERROR.ToLower().Contains("connessione interrotta"))
                    {
                        CONNECT_IMAP();
                    }
                    if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR)
                    {
                        NOTIFY_ERROR(MGN_ERROR);
                    }
                }
            }
            else
            {
                rtLog.Text += "\n---------------\n" + DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP!\n---------------\n";
            }

            if (bNewMsgICON) { nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Check; }
        }

        public void MYG_IMAP_READ_AND_MARK_ALWAYS_CONNECT()
        {
            DateTime dtInizio = DateTime.Now;
            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Checking;
            nIcon.Text = "STO CONTROLLANDO LA CASELLA DI POSTA...";

            Imap4Client imap = new Imap4Client();
            btnRECONNECT.Visible = false;

            # region CONNECT
            try
            {
                imap = new Imap4Client();
                imap.ConnectSsl(SERVER, PORTA);
                imap.Login(USER, PWD);
                bIMAP_ERROR = false;
            }
            catch (Exception errCONN)
            {
                nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Error;
                nIcon.Text = "ERRORE CONNESSIONE IMAP:\n" + errCONN.Message;

                bIMAP_ERROR = true;
                rtError.Text += DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP:\n" + errCONN.Message + "\n------------------\n";
                MGN_ERROR = DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP: " + errCONN.Message;
                if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR)
                {
                    if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                    {
                        RAD_NOTIFY_ERROR(MGN_ERROR);
                    }
                    else
                    {
                        NOTIFY_ERROR(MGN_ERROR);
                    }
                }
            }
            # endregion

            # region ELABORA MESSAGGI
            if (!bIMAP_ERROR)
            {
                try
                {
                    imap.Command("capability");
                    Mailbox inbox = imap.SelectMailbox("inbox");
                    int[] ids = inbox.Search("UNSEEN");
                    if (ids.Length > 0)
                    {
                        iCountIDS = 0;
                        for (var i = 0; i < ids.Length; i++)
                        {
                            if (!MSGS_ID.Contains(ids[i].ToString()))
                            {
                                iCountIDS++;
                                MSGS_ID.Add(ids[i].ToString());
                            }
                        }

                        if (iCountIDS > 0)
                        {
                            iLastCOUNT = iCountIDS;
                            MSGS_DETAILS = "";
                            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Check;
                            nIcon.Text = iCountIDS > 1 ? "SONO PRESENTI " + iCountIDS + " NUOVI MESSAGGI" : "E' PRESENTE 1 NUOVO MESSAGGIO!";

                            bNewMsgICON = true;
                            NEW_MSGS = true;
                            ActiveUp.Net.Mail.Message msg = null;

                            for (var i = 0; i < ids.Length; i++)
                            {
                                msg = inbox.Fetch.MessageObject(ids[i]);

                                rtLog.Text += DateTime.Now.ToString() + "\n";
                                rtLog.Text += "DATA: " + msg.Date + "\n";
                                rtLog.Text += "MITTENTE: " + msg.From + "\n";
                                rtLog.Text += "OGGETTO: " + msg.Subject + "\n";
                                rtLog.Text += "----------------------------\n";

                                MSGS_DETAILS += "DATA: " + msg.Date + "\n";
                                MSGS_DETAILS += "MITTENTE: " + msg.From + "\n";
                                MSGS_DETAILS += "OGGETTO: " + msg.Subject + "\n";
                                MSGS_DETAILS += "---------------------------\n";
                                
                                //mark as unread
                                var flags = new FlagCollection();
                                flags.Add("Seen");
                                inbox.RemoveFlags(ids[i], flags);
                            }
                        }
                        else
                        {
                            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
                            NEW_MSGS = false;
                        }
                    }
                    else
                    {
                        nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
                        NEW_MSGS = false;
                    }
                    
                }
                catch (Exception err)
                {
                    nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Error;
                    rtError.Text += DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP:\n" + err.Message + "\n------------------\n"; ;
                    MGN_ERROR = DateTime.Now.ToString() + "-> ERRORE ELABORAZIONE IMAP: " + err.Message;
                    if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR)
                    {
                        if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                        {
                            RAD_NOTIFY_ERROR(MGN_ERROR);
                        }
                        else
                        {
                            NOTIFY_ERROR(MGN_ERROR);
                        }
                    }
                }
            }
            else
            {
                rtLog.Text += "\n---------------\n" + DateTime.Now.ToString() + "-> ERRORE CONNESSIONE IMAP!\n---------------\n";
            }
            # endregion

            # region DISCONNECT
            try
            {
                imap.Disconnect();
            }
            catch (Exception err)
            {
                nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Error;
                nIcon.Text = "ERRORE DISCONNESSIONE IMAP:\n" + err.Message;

                rtError.Text += DateTime.Now.ToString() + "-> ERRORE DISCONNESSIONE IMAP:\n" + err.Message + "\n------------------\n"; ;
                MGN_ERROR = DateTime.Now.ToString() + "-> ERRORE DISCONNESSIONE IMAP: " + err.Message;
                if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_ERROR)
                {
                    if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                    {
                        RAD_NOTIFY_ERROR(MGN_ERROR);
                    }
                    else
                    {
                        NOTIFY_ERROR(MGN_ERROR);
                    }
                }
            }
            # endregion

            if (bNewMsgICON) { nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Check; }
            else
            {
                if (!NEW_MSGS)
                {
                    nIcon.Text = "NESSUN NUOVO MESSAGGIO!";
                }
            }

            if (MyGNotifier.Properties.Settings.Default.MAIN_CHECK_MONITOR)
            {
                DateTime dtFine = DateTime.Now;
                TimeSpan ts = dtFine - dtInizio;
                rtLog.Text += DateTime.Now.ToString() + "-> FINE CONTROLLO CASELLA POSTA: DURATA CONTROLLO " + ts.Seconds.ToString() + " secondi.\n";
            }
        }

        private void TimerGNotifier_Tick(object sender, EventArgs e)
        {
            if (NEW_MSGS)
            {
                NEW_MSGS = false;
                if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_MSGS)
                {
                    if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                    {
                        RAD_NOTIFY();
                    }
                    else
                    {
                        NOTIFY();
                    }
                }
            }
            else
            {
                if (MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE)
                {
                    MYG_IMAP_READ_AND_MARK(GMAIL_IMAP);
                }
                else
                {
                    MYG_IMAP_READ_AND_MARK_ALWAYS_CONNECT();
                }
            }
        }

        public void NOTIFY()
        {
            GNotifier.TitleText = "NUOVI MESSAGGI IN ARRIVO!";
            GNotifier.TitleColor = Color.Blue;
            GNotifier.ContentText = MSGS_DETAILS;
            GNotifier.ShowCloseButton = true;
            GNotifier.ShowOptionsButton = false;
            GNotifier.ShowGrip = true;
            GNotifier.Delay = 5000;
            GNotifier.AnimationInterval = 10;
            GNotifier.AnimationDuration = 1000;
            GNotifier.TitlePadding = new Padding(0);
            GNotifier.ContentPadding = new Padding(0);
            GNotifier.ImagePadding = new Padding(0);
            GNotifier.Scroll = true;

            if (iCountIDS > 0)
            {
                GNotifier.Size = new System.Drawing.Size(450, (iCountIDS * 100));
            }

            GNotifier.Image = Properties.Resources.gmail_icon;
            GNotifier.Popup();
        }

        public void LAST_NOTIFY()
        {
            GNotifier.TitleText = "ULTIMI MESSAGGI NOTIFICATI";
            GNotifier.TitleColor = Color.Blue;
            GNotifier.ContentText = MSGS_DETAILS;
            GNotifier.ShowCloseButton = true;
            GNotifier.ShowOptionsButton = false;
            GNotifier.ShowGrip = true;
            GNotifier.Delay = 5000;
            GNotifier.AnimationInterval = 10;
            GNotifier.AnimationDuration = 1000;
            GNotifier.TitlePadding = new Padding(0);
            GNotifier.ContentPadding = new Padding(0);
            GNotifier.ImagePadding = new Padding(0);
            GNotifier.Scroll = true;

            if (iLastCOUNT > 0)
            {
                GNotifier.Size = new System.Drawing.Size(450, (iLastCOUNT * 100));
            }

            GNotifier.Image = Properties.Resources.gmail_icon;
            GNotifier.Popup();
        }

        public void NOTIFY_ERROR(string ERR_MSG)
        {
            GNotifier.TitleText = "ERRORE DI CONNESSIONE!";
            GNotifier.TitleColor = Color.Red;
            GNotifier.ContentText = "E' AVVENUTO UN ERRORE DURANTE L'ELABORAZIONE (controllare il LOG):\n" +  ERR_MSG;
            GNotifier.ShowCloseButton = true;
            GNotifier.ShowOptionsButton = false;
            GNotifier.ShowGrip = true;
            GNotifier.Delay = 5000;
            GNotifier.AnimationInterval = 10;
            GNotifier.AnimationDuration = 1000;
            GNotifier.TitlePadding = new Padding(0);
            GNotifier.ContentPadding = new Padding(0);
            GNotifier.ImagePadding = new Padding(0);
            GNotifier.Scroll = true;

            GNotifier.Image = Properties.Resources.MyGNotifier_ERROR;
            GNotifier.Size = new System.Drawing.Size(400, 350);
            GNotifier.Popup();
        }

        public void NOTIFY_CUSTOM(string TITLE, string CONTENT)
        {
            GNotifier.TitleText = TITLE;
            GNotifier.TitleColor = Color.Blue;
            GNotifier.ContentText = CONTENT;
            GNotifier.ShowCloseButton = true;
            GNotifier.ShowOptionsButton = false;
            GNotifier.ShowGrip = true;
            GNotifier.Delay = 5000;
            GNotifier.AnimationInterval = 10;
            GNotifier.AnimationDuration = 1000;
            GNotifier.TitlePadding = new Padding(0);
            GNotifier.ContentPadding = new Padding(0);
            GNotifier.ImagePadding = new Padding(0);
            GNotifier.Scroll = true;

            GNotifier.Size = new System.Drawing.Size(400, 250);

            GNotifier.Image = Properties.Resources.gmail_icon;
            GNotifier.Popup();
        }

        private void GNotifier_Click(object sender, EventArgs e)
        {
            Process.Start("http://gmail.google.com");
            bNewMsgICON = false;
            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
            nIcon.Text = "NESSUN NUOVO MESSAGGIO PRESENTE!";
        }

        # region RAD NOTIFIER

        public void RAD_INIT()
        {
            radBtnGMAIL.Click += new EventHandler(RadNotifier_Click);
            radCHANGE_THEME.ComboBoxElement.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(Change_Rad_THEME);
        }

        public void RAD_SIZE(string TEXT)
        {
            int iCONST_HEIGHT = MyGNotifier.Properties.Settings.Default.MAIN_NOTIFIER_HEIGHT;
            MeasurementGraphics graphics = MeasurementGraphics.CreateMeasurementGraphics();
            SizeF sizeF = graphics.Graphics.MeasureString(TEXT, this.Font, 450);
            this.radGNotifier.FixedSize = new Size(450, (int)sizeF.Height + iCONST_HEIGHT);
        }

        private void RadNotifier_Click(object sender, EventArgs e)
        {
            Process.Start("http://gmail.google.com");
            bNewMsgICON = false;
            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
            nIcon.Text = "NESSUN NUOVO MESSAGGIO PRESENTE!";
            radGNotifier.Popup.ClosePopup(Telerik.WinControls.UI.RadPopupCloseReason.CloseCalled);
        }

        private void Change_Rad_THEME(object sender, EventArgs e)
        {
            if (radCHANGE_THEME.ComboBoxElement.SelectedIndex != -1)
            {
                switch (radCHANGE_THEME.ComboBoxElement.SelectedItem.ToString())
                { 
                    case "Office2007Black":
                        radGNotifier.ThemeName = "Office2007Black";
                        break;
                    case "Office2007Silver":
                        radGNotifier.ThemeName = "Office2007Silver";
                        break;
                    case "Office2010Black":
                        radGNotifier.ThemeName = "Office2010Black";
                        break;
                    case "Office2010Blue":
                        radGNotifier.ThemeName = "Office2010Blue";
                        break;
                    case "Office2010Silver":
                        radGNotifier.ThemeName = "Office2010Silver";
                        break;
                    case "Metro":
                        radGNotifier.ThemeName = "TelerikMetro";
                        break;
                    case "MetroBlue":
                        radGNotifier.ThemeName = "TelerikMetroBlue";
                        break;
                    case "MetroTouch":
                        radGNotifier.ThemeName = "TelerikMetroTouch";
                        break;
                    case "Windows7":
                        radGNotifier.ThemeName = "Windows7";
                        break;
                    case "VisualStudio2012":
                        radGNotifier.ThemeName = "VisualStudio2012Light";
                        break;
                }
            }
        }

        public void RAD_NOTIFY()
        {
            radGNotifier.CaptionText = "NUOVI MESSAGGI IN ARRIVO!";
            radGNotifier.ContentText = MSGS_DETAILS;
            
            radGNotifier.Popup.LoadElementTree();
            RAD_SIZE(MSGS_DETAILS);
            
            
            radGNotifier.Show();
        }

        public void RAD_LAST_NOTIFY()
        {
            radGNotifier.CaptionText = "ULTIMI MESSAGGI NOTIFICATI";
            radGNotifier.ContentText = MSGS_DETAILS;
            RAD_SIZE(MSGS_DETAILS);

            radGNotifier.Show();
        }

        public void RAD_NOTIFY_ERROR(string ERR_MSG)
        {
            string ERR_CONTENT = "E' AVVENUTO UN ERRORE DURANTE L'ELABORAZIONE (controllare il LOG):\n" + ERR_MSG;
            radGNotifier.CaptionText = "ATTENZIONE: ERRORE ELABORAZIONE!";
            radGNotifier.ContentText = ERR_CONTENT;
            RAD_SIZE(ERR_CONTENT);
            
            radGNotifier.Show();
        }

        public void RAD_NOTIFY_CUSTOM(string TITLE, string CONTENT)
        {
            radGNotifier.CaptionText = TITLE;
            radGNotifier.ContentText = CONTENT;
            RAD_SIZE(CONTENT);

            radGNotifier.Show();
        }

        # endregion

        private void TimerCleanLOGS_Tick(object sender, EventArgs e)
        {
            DateTime dtNow = DateTime.Now;

            rtError.Text = dtNow.ToString() + " -> AZZERAMENTO LOG AUTOMATICO";
            rtLog.Text = dtNow.ToString() + " -> AZZERAMENTO LOG AUTOMATICO";
        }

        # endregion

        # region MENU DX

        private void aPRIIMPOSTAZIONIToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void vAIAGMAILToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("http://gmail.google.com");
            bNewMsgICON = false;
            nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
            nIcon.Text = "NESSUN NUOVO MESSAGGIO PRESENTE!";
        }

        private void vISUALIZZANOTIFICAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MSGS_DETAILS != null && MSGS_DETAILS != "")
            {
                if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                {
                    RAD_LAST_NOTIFY();
                }
                else
                {
                    LAST_NOTIFY();
                }
            }
            else
            {
                MessageBox.Show("NESSUN NUOVO MESSAGGIO DA NOTIFICARE!", "INFORMAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cHIUDIAPPLICAZIONEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vISUALIZZAULTIMOERROREToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MGN_ERROR != null && MGN_ERROR != "")
            {
                if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                {
                    RAD_NOTIFY_ERROR(MGN_ERROR);
                }
                else
                {
                    NOTIFY_ERROR(MGN_ERROR);
                }
            }
            else
            {
                MessageBox.Show("NESSUN ERRORE DA NOTIFICARE!", "INFORMAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cONTROLLAADESSOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TimerGNotifier.Enabled)
            {
                TimerGNotifier.Enabled = false;
            }
            if (MyGNotifier.Properties.Settings.Default.MAIN_KEEP_CONNECTION_ALIVE)
            {
                MYG_IMAP_READ_AND_MARK(GMAIL_IMAP);
            }
            else
            {
                MYG_IMAP_READ_AND_MARK_ALWAYS_CONNECT();
            }
            if (NEW_MSGS)
            {
                NEW_MSGS = false;
                if (MyGNotifier.Properties.Settings.Default.MAIN_AUTO_NOTIFY_MSGS)
                {
                    if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                    {
                        RAD_NOTIFY();
                    }
                    else
                    {
                        NOTIFY();
                    }
                }
            }
            else
            {
                nIcon.Text = "NESSUN NUOVO MESSAGGIO PRESENTE!";
                if (MyGNotifier.Properties.Settings.Default.MAIN_RAD_NOTIFIER)
                {
                    RAD_NOTIFY_CUSTOM("NESSUN NUOVO MESSAGGIO", "DAL CONTROLLO EFFETTUATO IL " + DateTime.Now.ToString() + ", NON RISULTANO NUOVI MESSAGGI!");
                }
                else
                {
                    NOTIFY_CUSTOM("NESSUN NUOVO MESSAGGIO", "DAL CONTROLLO EFFETTUATO IL " + DateTime.Now.ToString() + ", NON RISULTANO NUOVI MESSAGGI!");
                }
            }

            TimerGNotifier.Enabled = true;
        }

        # endregion

        # region EVENTI ICONA NOTIFICHE

        private void nIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MyGNotifier.Properties.Settings.Default.MAIN_DOUBLE_CLICK_GMAIL)
            {
                nIcon.Icon = MyGNotifier.Properties.Resources.MyGNotify_Empty_Gray;
                bNewMsgICON = false;
                Process.Start("http://gmail.google.com");
            }
            else
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
        }

        # endregion

        # region TEST FUNCTIONS

        public void TRANSLATE_ALL()
        {
            ResourceManager LocRM = new ResourceManager("MyGNotifier.MyGNotifier_Local", typeof(MGN_IMPOSTAZIONI).Assembly);
            tabIMPOSTAZIONI.Text = LocRM.GetString("TAB_IMPOSTAZIONI");
            tabIMPOSTAZIONI_CONNESSIONE.Text = LocRM.GetString("TAB_IMPOSTAZIONI_CONNESSIONE");
            STATUS_OK = LocRM.GetString("lblSettingStatusOK");
            STATUS_NO = LocRM.GetString("lblSettingStatusNO");
            lblServerImap.Text = LocRM.GetString("lblServerImap");
            lblPortaImap.Text = LocRM.GetString("lblPortaImap");
            tabIMPOSTAZIONI_SISTEMA.Text = LocRM.GetString("TAB_IMPOSTAZIONI_SISTEMA");
            lblPasswordSistema.Text = LocRM.GetString("lblPasswordSistema");
            chkMainPwd.Text = LocRM.GetString("chkMainPwd");
            chkDOPPIO_CLICK.Text = LocRM.GetString("chkDOPPPIO_CLICK");
            chkMOSTRA_NOTIFICA_ERRORE.Text = LocRM.GetString("chkMOSTRA_NOTIFICA_ERRORE");
            chkMOSTRA_NOTIFICHE_MESSAGGI_AUTO.Text = LocRM.GetString("chkMOSTRA_NOTIFICHE_MESSAGGI_AUTO");
            lblFrequenzaAggiornamento.Text = LocRM.GetString("lblFrequenzaAggiornamento");
            chkCHECK_MONITOR.Text = LocRM.GetString("chkCHECK_MONITOR");
            tabLOG.Text = LocRM.GetString("TAB_LOG");
            grLogScanMessaggiImap.Text = LocRM.GetString("grLogScanMessaggiImap");
            grLogErroriMessaggiImap.Text = LocRM.GetString("grLogErroriMessaggiImap");
            btnSalvaImpostazioni.Text = LocRM.GetString("btnSalvaImpostazioni");
            chkRAD_NOTIFIER.Text = LocRM.GetString("chkRAD_NOTIFIER");
            lblTemaDefaultStileOutlook.Text = LocRM.GetString("lblTemaDefaultStileOutlook");
            lblDeltaAltezzaNotifica.Text = LocRM.GetString("lblDeltaAltezzaNotifica");
            rtError.Text = "";
            rtLog.Text = "";
        }

        # endregion

    }
}
