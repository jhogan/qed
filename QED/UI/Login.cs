using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using QED.SEC;
using QED.Business;
using JCSLA;
using ByteFX.Data.MySqlClient;
namespace QED.UI
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnOK;
		public System.Windows.Forms.TextBox txtUserName;
		public  System.Windows.Forms.TextBox txtPasswd;
		public  System.Windows.Forms.TextBox txtKey;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Login()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Text = "QED " + UI.QED_CLIENT_ID;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPasswd = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtUserName
			// 
			this.txtUserName.Location = new System.Drawing.Point(120, 8);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(168, 20);
			this.txtUserName.TabIndex = 0;
			this.txtUserName.Tag = "";
			this.txtUserName.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(128, 104);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "User Name";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(216, 104);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Password";
			// 
			// txtPasswd
			// 
			this.txtPasswd.Location = new System.Drawing.Point(120, 40);
			this.txtPasswd.Name = "txtPasswd";
			this.txtPasswd.PasswordChar = '*';
			this.txtPasswd.Size = new System.Drawing.Size(168, 20);
			this.txtPasswd.TabIndex = 1;
			this.txtPasswd.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Key";
			// 
			// txtKey
			// 
			this.txtKey.Location = new System.Drawing.Point(120, 72);
			this.txtKey.Name = "txtKey";
			this.txtKey.PasswordChar = '*';
			this.txtKey.Size = new System.Drawing.Size(168, 20);
			this.txtKey.TabIndex = 2;
			this.txtKey.Text = "";
			// 
			// Login
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(296, 133);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtKey);
			this.Controls.Add(this.txtPasswd);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Login";
			this.Text = "Login";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e) {
			try{
				Connections conns = Connections.Inst;
				conns.Load(this.txtKey.Text);
				QEDPrincipal.Login(this.txtUserName.Text, this.txtPasswd.Text);
				if(!Thread.CurrentPrincipal.Identity.IsAuthenticated){
					MessageBox.Show("The username or password were not valid",
						"Failed login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					this.DialogResult = DialogResult.Cancel;
				}else{
					this.DialogResult =  DialogResult.OK;
				}
			}
			catch(MySqlException ex) {
				MessageBox.Show("Failed to log in. Check encryption key.\r\n" + ex.Message,
					"Failed login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.DialogResult = DialogResult.Cancel;
			}
			catch(System.Security.Cryptography.CryptographicException ex) {
				MessageBox.Show("Failed to log in. The encryption key may be incorrect. \r\n" + ex.Message,
					"Failed login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.DialogResult = DialogResult.Cancel;
			}
			catch(Exception ex) {
				MessageBox.Show("Failed to log in.\r\n" + ex.Message,
					"Failed login", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.DialogResult = DialogResult.Cancel;
			}
			this.Close();
		}
		private void btnCancel_Click(object sender, System.EventArgs e) {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
