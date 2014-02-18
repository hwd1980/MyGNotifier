namespace MyGNotifier
{
    partial class MGN_SET_PASSWORD
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
            this.btnCloseAll = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPwd2 = new System.Windows.Forms.TextBox();
            this.btnAccedi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCloseAll
            // 
            this.btnCloseAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseAll.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnCloseAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.btnCloseAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnCloseAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseAll.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseAll.ForeColor = System.Drawing.Color.DarkRed;
            this.btnCloseAll.Location = new System.Drawing.Point(7, 77);
            this.btnCloseAll.Name = "btnCloseAll";
            this.btnCloseAll.Size = new System.Drawing.Size(51, 24);
            this.btnCloseAll.TabIndex = 10;
            this.btnCloseAll.Text = "ESCI";
            this.btnCloseAll.UseVisualStyleBackColor = true;
            this.btnCloseAll.Click += new System.EventHandler(this.btnCloseAll_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MyGNotifier.Properties.Resources.gmail_icon;
            this.pictureBox2.Location = new System.Drawing.Point(6, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 50);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "IMPOSTA PASSWORD DI AMMINISTRATORE";
            // 
            // txtPwd
            // 
            this.txtPwd.ForeColor = System.Drawing.Color.Green;
            this.txtPwd.Location = new System.Drawing.Point(75, 27);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(237, 23);
            this.txtPwd.TabIndex = 12;
            this.txtPwd.UseSystemPasswordChar = true;
            this.txtPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPwd_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "RIPETI PASSWORD";
            // 
            // txtPwd2
            // 
            this.txtPwd2.ForeColor = System.Drawing.Color.Green;
            this.txtPwd2.Location = new System.Drawing.Point(75, 79);
            this.txtPwd2.Name = "txtPwd2";
            this.txtPwd2.PasswordChar = '*';
            this.txtPwd2.Size = new System.Drawing.Size(237, 23);
            this.txtPwd2.TabIndex = 14;
            this.txtPwd2.UseSystemPasswordChar = true;
            this.txtPwd2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPwd2_KeyPress);
            // 
            // btnAccedi
            // 
            this.btnAccedi.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccedi.ForeColor = System.Drawing.Color.Blue;
            this.btnAccedi.Location = new System.Drawing.Point(327, 27);
            this.btnAccedi.Name = "btnAccedi";
            this.btnAccedi.Size = new System.Drawing.Size(125, 75);
            this.btnAccedi.TabIndex = 15;
            this.btnAccedi.Text = "PROCEDI";
            this.btnAccedi.UseVisualStyleBackColor = true;
            this.btnAccedi.Click += new System.EventHandler(this.btnAccedi_Click);
            // 
            // MGN_SET_PASSWORD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(464, 148);
            this.ControlBox = false;
            this.Controls.Add(this.btnAccedi);
            this.Controls.Add(this.txtPwd2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCloseAll);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 180);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 180);
            this.Name = "MGN_SET_PASSWORD";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyGNotifier - IMPOSTA PASSWORD";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseAll;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPwd2;
        private System.Windows.Forms.Button btnAccedi;
    }
}