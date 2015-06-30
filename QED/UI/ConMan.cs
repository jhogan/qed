using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QED.Business;
using QED.Business.Sec;
namespace QED.UI
{
	/// <summary>
	/// Summary description for Connections.
	/// </summary>
	public class ConMan : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lstConns;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboServer;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtPasswd;
		private System.Windows.Forms.TextBox txtReenterPasswd;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.ComboBox cboProtocol;
		private System.Windows.Forms.TextBox txtDec;
		private System.Windows.Forms.TextBox txtEnc;
		private System.Windows.Forms.CheckBox chkSSPI;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtSystemName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		Hashtable _defaultPorts = new Hashtable();
		private System.Windows.Forms.ContextMenu mnuGenListBox;
		private System.Windows.Forms.MenuItem mnuItemGenListBoxAdd;
		private System.Windows.Forms.MenuItem mnuItemGenListBoxRemove;
		private System.ComponentModel.Container components = null;
		Connection _conn;
		Connections _conns;
		private System.Windows.Forms.Label lblCatalog;
		private System.Windows.Forms.TextBox txtCatalog;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnUnhide;
		Servers _svrs;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtHashOut;
		private System.Windows.Forms.TextBox txtMD5PasswdReeneter;
		private System.Windows.Forms.TextBox txtMD5Passwd;
		private System.Windows.Forms.Button btnGenHash;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		bool _passwdHidden = true;

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
			this.lstConns = new System.Windows.Forms.ListBox();
			this.cboServer = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnUnhide = new System.Windows.Forms.Button();
			this.lblCatalog = new System.Windows.Forms.Label();
			this.txtCatalog = new System.Windows.Forms.TextBox();
			this.chkSSPI = new System.Windows.Forms.CheckBox();
			this.cboProtocol = new System.Windows.Forms.ComboBox();
			this.txtPasswd = new System.Windows.Forms.TextBox();
			this.txtReenterPasswd = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSystemName = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtEnc = new System.Windows.Forms.TextBox();
			this.txtDec = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.mnuGenListBox = new System.Windows.Forms.ContextMenu();
			this.mnuItemGenListBoxAdd = new System.Windows.Forms.MenuItem();
			this.mnuItemGenListBoxRemove = new System.Windows.Forms.MenuItem();
			this.btnApply = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.txtHashOut = new System.Windows.Forms.TextBox();
			this.txtMD5PasswdReeneter = new System.Windows.Forms.TextBox();
			this.txtMD5Passwd = new System.Windows.Forms.TextBox();
			this.btnGenHash = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstConns
			// 
			this.lstConns.DisplayMember = "SystemName";
			this.lstConns.Location = new System.Drawing.Point(8, 16);
			this.lstConns.Name = "lstConns";
			this.lstConns.Size = new System.Drawing.Size(160, 277);
			this.lstConns.TabIndex = 0;
			this.lstConns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstConns_MouseUp);
			this.lstConns.SelectedIndexChanged += new System.EventHandler(this.lstConns_SelectedIndexChanged);
			// 
			// cboServer
			// 
			this.cboServer.Location = new System.Drawing.Point(128, 56);
			this.cboServer.Name = "cboServer";
			this.cboServer.Size = new System.Drawing.Size(352, 21);
			this.cboServer.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnUnhide);
			this.groupBox1.Controls.Add(this.lblCatalog);
			this.groupBox1.Controls.Add(this.txtCatalog);
			this.groupBox1.Controls.Add(this.chkSSPI);
			this.groupBox1.Controls.Add(this.cboProtocol);
			this.groupBox1.Controls.Add(this.txtPasswd);
			this.groupBox1.Controls.Add(this.txtReenterPasswd);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtPort);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtUser);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtSystemName);
			this.groupBox1.Controls.Add(this.cboServer);
			this.groupBox1.Location = new System.Drawing.Point(176, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(496, 280);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Conneciton Info";
			// 
			// btnUnhide
			// 
			this.btnUnhide.Location = new System.Drawing.Point(408, 216);
			this.btnUnhide.Name = "btnUnhide";
			this.btnUnhide.TabIndex = 38;
			this.btnUnhide.Text = "Unhide";
			this.btnUnhide.Click += new System.EventHandler(this.btnUnhide_Click);
			// 
			// lblCatalog
			// 
			this.lblCatalog.Enabled = false;
			this.lblCatalog.Location = new System.Drawing.Point(16, 120);
			this.lblCatalog.Name = "lblCatalog";
			this.lblCatalog.TabIndex = 37;
			this.lblCatalog.Text = "Default Catalog";
			// 
			// txtCatalog
			// 
			this.txtCatalog.Enabled = false;
			this.txtCatalog.Location = new System.Drawing.Point(128, 120);
			this.txtCatalog.Name = "txtCatalog";
			this.txtCatalog.Size = new System.Drawing.Size(352, 20);
			this.txtCatalog.TabIndex = 5;
			this.txtCatalog.Text = "";
			// 
			// chkSSPI
			// 
			this.chkSSPI.Enabled = false;
			this.chkSSPI.Location = new System.Drawing.Point(400, 88);
			this.chkSSPI.Name = "chkSSPI";
			this.chkSSPI.Size = new System.Drawing.Size(80, 24);
			this.chkSSPI.TabIndex = 4;
			this.chkSSPI.Text = "SSPI";
			// 
			// cboProtocol
			// 
			this.cboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboProtocol.Location = new System.Drawing.Point(128, 88);
			this.cboProtocol.Name = "cboProtocol";
			this.cboProtocol.Size = new System.Drawing.Size(256, 21);
			this.cboProtocol.TabIndex = 3;
			this.cboProtocol.SelectedIndexChanged += new System.EventHandler(this.cboProtocol_SelectedIndexChanged);
			// 
			// txtPasswd
			// 
			this.txtPasswd.Location = new System.Drawing.Point(128, 216);
			this.txtPasswd.Name = "txtPasswd";
			this.txtPasswd.PasswordChar = '*';
			this.txtPasswd.Size = new System.Drawing.Size(272, 20);
			this.txtPasswd.TabIndex = 8;
			this.txtPasswd.Text = "";
			// 
			// txtReenterPasswd
			// 
			this.txtReenterPasswd.Location = new System.Drawing.Point(128, 248);
			this.txtReenterPasswd.Name = "txtReenterPasswd";
			this.txtReenterPasswd.PasswordChar = '*';
			this.txtReenterPasswd.Size = new System.Drawing.Size(272, 20);
			this.txtReenterPasswd.TabIndex = 9;
			this.txtReenterPasswd.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 216);
			this.label6.Name = "label6";
			this.label6.TabIndex = 31;
			this.label6.Text = "Password";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 248);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 23);
			this.label7.TabIndex = 30;
			this.label7.Text = "Re-enter Password";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 56);
			this.label5.Name = "label5";
			this.label5.TabIndex = 29;
			this.label5.Text = "Server";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 88);
			this.label4.Name = "label4";
			this.label4.TabIndex = 27;
			this.label4.Text = "Protocol";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 152);
			this.label3.Name = "label3";
			this.label3.TabIndex = 25;
			this.label3.Text = "Port";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(128, 152);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(48, 20);
			this.txtPort.TabIndex = 6;
			this.txtPort.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 184);
			this.label2.Name = "label2";
			this.label2.TabIndex = 23;
			this.label2.Text = "User";
			// 
			// txtUser
			// 
			this.txtUser.Location = new System.Drawing.Point(128, 184);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(352, 20);
			this.txtUser.TabIndex = 7;
			this.txtUser.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.TabIndex = 21;
			this.label1.Text = "Name";
			// 
			// txtSystemName
			// 
			this.txtSystemName.Location = new System.Drawing.Point(128, 24);
			this.txtSystemName.Name = "txtSystemName";
			this.txtSystemName.Size = new System.Drawing.Size(352, 20);
			this.txtSystemName.TabIndex = 1;
			this.txtSystemName.Text = "";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.txtEnc);
			this.groupBox2.Controls.Add(this.txtDec);
			this.groupBox2.Location = new System.Drawing.Point(16, 344);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(496, 144);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Encrypted data with system key";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 38;
			this.label9.Text = "Encrypted";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 32);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 37;
			this.label8.Text = "Decrypted";
			// 
			// txtEnc
			// 
			this.txtEnc.Location = new System.Drawing.Point(16, 112);
			this.txtEnc.Name = "txtEnc";
			this.txtEnc.Size = new System.Drawing.Size(464, 20);
			this.txtEnc.TabIndex = 11;
			this.txtEnc.Text = "";
			// 
			// txtDec
			// 
			this.txtDec.Location = new System.Drawing.Point(16, 56);
			this.txtDec.Name = "txtDec";
			this.txtDec.Size = new System.Drawing.Size(464, 20);
			this.txtDec.TabIndex = 10;
			this.txtDec.Text = "";
			this.txtDec.TextChanged += new System.EventHandler(this.txtDec_TextChanged);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(512, 304);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 13;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(600, 304);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 14;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// mnuGenListBox
			// 
			this.mnuGenListBox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuItemGenListBoxAdd,
																						  this.mnuItemGenListBoxRemove});
			// 
			// mnuItemGenListBoxAdd
			// 
			this.mnuItemGenListBoxAdd.Index = 0;
			this.mnuItemGenListBoxAdd.Text = "&Add";
			this.mnuItemGenListBoxAdd.Click += new System.EventHandler(this.mnuItemGenListBoxAdd_Click);
			// 
			// mnuItemGenListBoxRemove
			// 
			this.mnuItemGenListBoxRemove.Index = 1;
			this.mnuItemGenListBoxRemove.Text = "&Remove";
			this.mnuItemGenListBoxRemove.Click += new System.EventHandler(this.mnuItemGenListBoxRemove_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(432, 304);
			this.btnApply.Name = "btnApply";
			this.btnApply.TabIndex = 12;
			this.btnApply.Text = "&Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.txtHashOut);
			this.groupBox3.Controls.Add(this.txtMD5PasswdReeneter);
			this.groupBox3.Controls.Add(this.txtMD5Passwd);
			this.groupBox3.Controls.Add(this.btnGenHash);
			this.groupBox3.Location = new System.Drawing.Point(16, 504);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(496, 160);
			this.groupBox3.TabIndex = 40;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Generate MD5 Hash. Use this output to store as the password in the user table.";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(16, 88);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(56, 16);
			this.label14.TabIndex = 63;
			this.label14.Text = "Hash";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(16, 56);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(96, 16);
			this.label12.TabIndex = 62;
			this.label12.Text = "Re-eneter password";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(16, 24);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 16);
			this.label13.TabIndex = 61;
			this.label13.Text = "Password";
			// 
			// txtHashOut
			// 
			this.txtHashOut.Location = new System.Drawing.Point(128, 96);
			this.txtHashOut.Name = "txtHashOut";
			this.txtHashOut.Size = new System.Drawing.Size(344, 20);
			this.txtHashOut.TabIndex = 60;
			this.txtHashOut.Text = "";
			// 
			// txtMD5PasswdReeneter
			// 
			this.txtMD5PasswdReeneter.Location = new System.Drawing.Point(128, 64);
			this.txtMD5PasswdReeneter.Name = "txtMD5PasswdReeneter";
			this.txtMD5PasswdReeneter.PasswordChar = '*';
			this.txtMD5PasswdReeneter.Size = new System.Drawing.Size(344, 20);
			this.txtMD5PasswdReeneter.TabIndex = 59;
			this.txtMD5PasswdReeneter.Text = "";
			this.txtMD5PasswdReeneter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMD5PasswdReeneter_KeyDown);
			// 
			// txtMD5Passwd
			// 
			this.txtMD5Passwd.Location = new System.Drawing.Point(128, 32);
			this.txtMD5Passwd.Name = "txtMD5Passwd";
			this.txtMD5Passwd.PasswordChar = '*';
			this.txtMD5Passwd.Size = new System.Drawing.Size(344, 20);
			this.txtMD5Passwd.TabIndex = 57;
			this.txtMD5Passwd.Text = "";
			this.txtMD5Passwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMD5Passwd_KeyDown);
			// 
			// btnGenHash
			// 
			this.btnGenHash.Location = new System.Drawing.Point(400, 128);
			this.btnGenHash.Name = "btnGenHash";
			this.btnGenHash.TabIndex = 56;
			this.btnGenHash.Text = "&Generate Hash";
			this.btnGenHash.Click += new System.EventHandler(this.btnGenHash_Click);
			// 
			// ConMan
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(704, 701);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lstConns);
			this.Controls.Add(this.groupBox3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConMan";
			this.Text = "Connection Manager";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		public ConMan() {
			InitializeComponent();
			_defaultPorts.Add("FTP", 21);
			_defaultPorts.Add("SSH", 22);
			_defaultPorts.Add("SFTP", 22);
			_defaultPorts.Add("TELNET", 23);
			_defaultPorts.Add("HTTP", 80);
			_defaultPorts.Add("MYSQL", 3306);
			_defaultPorts.Add("MSSQL", 1433);
			_conns = Connections.Inst;
			foreach(Connection conn in _conns) {
				lstConns.Items.Add(conn);
			}
			UpdateScreen();
		}
		private void UpdateScreen() {
			cboServer.Items.Clear(); cboServer.Text = "";
			cboProtocol.Items.Clear();
			string[] prots;

			_svrs = new Servers();
			_svrs.LoadAll();
			foreach(Server svr in _svrs) {
				cboServer.Items.Add(svr.DNSName);
			}

			prots = Enum.GetNames(Type.GetType("QED.Business.Protocol"));
			foreach(string prot in prots) {
				cboProtocol.Items.Add(prot);
			}
			
			if (_conn != null){
				this.txtSystemName.Text = _conn.SystemName;
				if (_conn.Server != null)
					this.cboServer.Text = _conn.Server.DNSName;
				this.cboProtocol.SelectedItem = _conn.Protocol.ToString();
				this.chkSSPI.Checked = _conn.SSPI;
				this.txtCatalog.Text = _conn.Catalog;
				this.txtPort.Text = _conn.Port.ToString();
				this.txtUser.Text = _conn.User;
				this.txtPasswd.Text = _conn.DecPasswd;
				this.txtReenterPasswd.Text =  _conn.DecPasswd;
				this.chkSSPI.Enabled = 
					this.lblCatalog.Enabled = 
						this.txtCatalog.Enabled =
							_conn.IsDBProtocol;
			}
		}

		private void lstConns_SelectedIndexChanged(object sender, System.EventArgs e) {
			Connection conn = (Connection)this.lstConns.SelectedItem;
			if (conn != null){
				_conn = conn;
				this.UpdateScreen();
			}
		}

		private void cboProtocol_SelectedIndexChanged(object sender, System.EventArgs e) {
			this.txtPort.Text = _defaultPorts[cboProtocol.SelectedItem.ToString()].ToString();
			if (_conn != null && this.cboProtocol.Text.Trim() != ""){
				_conn.Protocol = (Protocol)Enum.Parse(Type.GetType("QED.Business.Protocol"), this.cboProtocol.Text);
				this.chkSSPI.Enabled = 
					this.lblCatalog.Enabled = 
					this.txtCatalog.Enabled =
					_conn.IsDBProtocol;
			}
		}

		private void lstConns_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (e.Button == MouseButtons.Right){
				if (lstConns.SelectedItem != null) {
					mnuItemGenListBoxAdd.Enabled = true;
					mnuItemGenListBoxRemove.Enabled = true;
				}else{
					mnuItemGenListBoxAdd.Enabled = true;
					mnuItemGenListBoxRemove.Enabled = false;
				}
				mnuGenListBox.Show(lstConns, new Point(e.X, e.Y));
			}
		}

		private void mnuItemGenListBoxAdd_Click(object sender, System.EventArgs e) {
			InputModal im = new InputModal("", "Connection Name");
			if (im.ShowDialog(this) == DialogResult.OK) {
				_conn = new Connection();
				_conn.SystemName = im.Answer("Connection Name");
				_conns.Add(_conn);
				this.lstConns.Items.Add(_conn);
				this.UpdateScreen();
				this.lstConns.SelectedItem = _conn;
			}
		}
		private void mnuItemGenListBoxRemove_Click(object sender, System.EventArgs e) {
			if (_conn != null){
				if (MessageBox.Show(this, "The system may depend on this connections. Are you sure you want to delete it?", 
												"Confirm Delete",
												MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
												MessageBoxDefaultButton.Button2) ==
												DialogResult.Yes){
					_conn.Delete();
					this.lstConns.Items.Remove(_conn);
				}
			}
		}
		private void UpdateConn(){
			if (_conn != null){
				if (this.txtReenterPasswd.Text == this.txtPasswd.Text){
					_conn.SystemName = this.txtSystemName.Text;
					foreach(Server svr in _svrs) {
						if (svr.DNSName == this.cboServer.Text.Trim()){
							_conn.Server = svr;
							break;
						}
					}
					if (this.cboProtocol.Text.Trim() != "")
						_conn.Protocol = (Protocol)Enum.Parse(Type.GetType("QED.Business.Protocol"), this.cboProtocol.Text);
					_conn.SSPI = this.chkSSPI.Checked;
					_conn.Catalog = this.txtCatalog.Text;
					if (this.txtPort.Text.Trim() != "")
						_conn.Port  = Convert.ToInt32(this.txtPort.Text); 
					_conn.User = this.txtUser.Text;
					_conn.DecPasswd = this.txtPasswd.Text;
					if (_conn.IsValid)
						_conn.Update();
					else
						MessageBox.Show(this, _conn.BrokenRules.ToString(), "Error Saving");
				}else{
					MessageBox.Show(this, "Passwords don't match", "QED");
				}
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e) {
			this.UpdateConn();
			if (_conn != null && !_conn.IsDirty){
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e) {
			if (_conn != null && _conn.IsDirty){
				if(
					MessageBox.Show(this, "\"" + _conn.SystemName + "\" hasn't been saved. Close anyway?", "Close", MessageBoxButtons.YesNo)
					== DialogResult.Yes){
					this.Close();
				}
			}else{
				this.Close();
			}
		}

		private void btnApply_Click(object sender, System.EventArgs e) {
			this.UpdateConn();
		}

		private void txtDec_TextChanged(object sender, System.EventArgs e) {
			txtEnc.Text = Encryptor.EncryptString(txtDec.Text, Connections.Inst.Key);
		}

		private void btnUnhide_Click(object sender, System.EventArgs e) {
			_passwdHidden = !_passwdHidden;
			if (_passwdHidden){
				this.btnUnhide.Text = "Unhide";
				this.txtPasswd.PasswordChar = '*';
			}else{
				this.btnUnhide.Text = "Hide";
				this.txtPasswd.PasswordChar = '\0';
			}
		}

		private void btnGenHash_Click(object sender, System.EventArgs e) {
			if (this.txtMD5Passwd.Text == this.txtMD5PasswdReeneter.Text) {
				this.txtHashOut.Text = SimpleHash.ComputeHash(this.txtMD5Passwd.Text, "MD5", null);
			}else{
				MessageBox.Show(this, "Passwords don't match", "QED");
			}
		}

		private void txtMD5Passwd_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Return) this.btnGenHash_Click(new object(), new EventArgs());
		}

		private void txtMD5PasswdReeneter_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Return) this.btnGenHash_Click(new object(), new EventArgs());
		}


	}
}
