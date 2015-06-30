using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QED.Business;
using JCSLA;
namespace QED.UI
{
	/// <summary>
	/// Summary description for EffortData.
	/// </summary>
	public class EffortData : System.Windows.Forms.Form {
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPM;
		private System.Windows.Forms.TextBox txtWebResx;
		private System.Windows.Forms.TextBox txtTestedBy;
		private System.Windows.Forms.TextBox txtDBResx;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkUATApproved;
		string _defaultUserDomain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox txtMaxResx;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtUATApprovedBy;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtBranchFileHierarchy;
		private System.Windows.Forms.TextBox txtEnv;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtRequester;
		Effort _eff;
		public EffortData(Effort eff) {
			InitializeComponent();
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			CatList system = cls.item("System");
			 _defaultUserDomain = system.Entry("/Defaults/UserDomain").Value;
			_eff = eff; this.Text = eff.ConventionalId;
			this.txtDBResx.Text = eff.DBResource;
			this.txtPM.Text = (eff.PMResource == "") ? _defaultUserDomain : eff.PMResource;
			this.txtTestedBy.Text = (eff.TestedBy == "") ? _defaultUserDomain : eff.TestedBy;
			this.txtWebResx.Text = (eff.WebResource == "") ? _defaultUserDomain : eff.WebResource;
			this.txtDBResx.Text = (eff.DBResource == "") ? _defaultUserDomain : eff.DBResource;
			this.txtMaxResx.Text = (eff.MaxResource == "") ? _defaultUserDomain : eff.MaxResource;
			this.txtUATApprovedBy.Text = (eff.UATApprovedBy == "") ? _defaultUserDomain : _eff.UATApprovedBy;
			// if (eff.IsTicket) this.txtRequester.Enabled = false;
			this.txtRequester.Text = (eff.Requester == "") ? _defaultUserDomain : _eff.Requester;
			this.chkUATApproved.Checked = eff.UATApproved;
			this.txtEnv.Text = eff.Environment;
			this.txtBranchFileHierarchy.Text = eff.BranchFileHierarchy;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
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
		private void InitializeComponent() {
			this.txtPM = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txtWebResx = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.txtTestedBy = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDBResx = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkUATApproved = new System.Windows.Forms.CheckBox();
			this.txtMaxResx = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtUATApprovedBy = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtBranchFileHierarchy = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtEnv = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtRequester = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtPM
			// 
			this.txtPM.Location = new System.Drawing.Point(200, 72);
			this.txtPM.Name = "txtPM";
			this.txtPM.Size = new System.Drawing.Size(304, 20);
			this.txtPM.TabIndex = 2;
			this.txtPM.Text = "";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 16);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(186, 23);
			this.label12.TabIndex = 47;
			this.label12.Text = "UAT Approved";
			// 
			// txtWebResx
			// 
			this.txtWebResx.Location = new System.Drawing.Point(200, 144);
			this.txtWebResx.Name = "txtWebResx";
			this.txtWebResx.Size = new System.Drawing.Size(304, 20);
			this.txtWebResx.TabIndex = 4;
			this.txtWebResx.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 144);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(184, 23);
			this.label11.TabIndex = 45;
			this.label11.Text = "Web Resource";
			// 
			// txtTestedBy
			// 
			this.txtTestedBy.Location = new System.Drawing.Point(200, 208);
			this.txtTestedBy.Name = "txtTestedBy";
			this.txtTestedBy.Size = new System.Drawing.Size(304, 20);
			this.txtTestedBy.TabIndex = 6;
			this.txtTestedBy.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 208);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(184, 23);
			this.label3.TabIndex = 37;
			this.label3.Text = "Tested By";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(186, 23);
			this.label2.TabIndex = 35;
			this.label2.Text = "PM";
			// 
			// txtDBResx
			// 
			this.txtDBResx.Location = new System.Drawing.Point(200, 112);
			this.txtDBResx.Name = "txtDBResx";
			this.txtDBResx.Size = new System.Drawing.Size(304, 20);
			this.txtDBResx.TabIndex = 3;
			this.txtDBResx.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 112);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(184, 24);
			this.label1.TabIndex = 33;
			this.label1.Text = "DB Resource";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(336, 624);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(424, 624);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkUATApproved
			// 
			this.chkUATApproved.Location = new System.Drawing.Point(200, 16);
			this.chkUATApproved.Name = "chkUATApproved";
			this.chkUATApproved.TabIndex = 0;
			// 
			// txtMaxResx
			// 
			this.txtMaxResx.Location = new System.Drawing.Point(200, 176);
			this.txtMaxResx.Name = "txtMaxResx";
			this.txtMaxResx.Size = new System.Drawing.Size(304, 20);
			this.txtMaxResx.TabIndex = 5;
			this.txtMaxResx.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 176);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(184, 23);
			this.label4.TabIndex = 53;
			this.label4.Text = "Max Resource";
			// 
			// txtUATApprovedBy
			// 
			this.txtUATApprovedBy.Location = new System.Drawing.Point(200, 40);
			this.txtUATApprovedBy.Name = "txtUATApprovedBy";
			this.txtUATApprovedBy.Size = new System.Drawing.Size(304, 20);
			this.txtUATApprovedBy.TabIndex = 1;
			this.txtUATApprovedBy.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(186, 23);
			this.label5.TabIndex = 55;
			this.label5.Text = "UAT Approved By";
			// 
			// txtBranchFileHierarchy
			// 
			this.txtBranchFileHierarchy.AcceptsTab = true;
			this.txtBranchFileHierarchy.Location = new System.Drawing.Point(8, 336);
			this.txtBranchFileHierarchy.Multiline = true;
			this.txtBranchFileHierarchy.Name = "txtBranchFileHierarchy";
			this.txtBranchFileHierarchy.Size = new System.Drawing.Size(496, 280);
			this.txtBranchFileHierarchy.TabIndex = 8;
			this.txtBranchFileHierarchy.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 304);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(184, 23);
			this.label6.TabIndex = 57;
			this.label6.Text = "Branch-File Hierarchy";
			// 
			// txtEnv
			// 
			this.txtEnv.Location = new System.Drawing.Point(200, 272);
			this.txtEnv.Name = "txtEnv";
			this.txtEnv.Size = new System.Drawing.Size(304, 20);
			this.txtEnv.TabIndex = 7;
			this.txtEnv.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 272);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(184, 23);
			this.label7.TabIndex = 59;
			this.label7.Text = "Environments";
			// 
			// txtRequester
			// 
			this.txtRequester.Location = new System.Drawing.Point(200, 240);
			this.txtRequester.Name = "txtRequester";
			this.txtRequester.Size = new System.Drawing.Size(304, 20);
			this.txtRequester.TabIndex = 60;
			this.txtRequester.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 240);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(184, 23);
			this.label8.TabIndex = 61;
			this.label8.Text = "Requestor";
			// 
			// EffortData
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 653);
			this.Controls.Add(this.txtRequester);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.txtEnv);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtBranchFileHierarchy);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtUATApprovedBy);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtMaxResx);
			this.Controls.Add(this.txtPM);
			this.Controls.Add(this.txtWebResx);
			this.Controls.Add(this.txtTestedBy);
			this.Controls.Add(this.txtDBResx);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.chkUATApproved);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "EffortData";
			this.Text = "1";
			this.Load += new System.EventHandler(this.EffortData_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e) {
			_eff.PMResource = (this.txtPM.Text == _defaultUserDomain) ? "" : this.txtPM.Text;
			_eff.TestedBy = (this.txtTestedBy.Text == _defaultUserDomain) ? "" : this.txtTestedBy.Text;
			_eff.WebResource = (this.txtWebResx.Text == _defaultUserDomain) ? "" : this.txtWebResx.Text;
			_eff.DBResource = (this.txtDBResx.Text == _defaultUserDomain) ? "" : this.txtDBResx.Text;
			_eff.MaxResource = (this.txtMaxResx.Text == _defaultUserDomain) ? "" : this.txtMaxResx.Text;
			_eff.UATApproved = this.chkUATApproved.Checked;
			_eff.UATApprovedBy = (this.txtUATApprovedBy.Text == _defaultUserDomain) ? "" : this.txtUATApprovedBy.Text;
			_eff.Requester = (this.txtRequester.Text == _defaultUserDomain) ? "" : this.txtRequester.Text;
			_eff.Environment = this.txtEnv.Text;
			_eff.BranchFileHierarchy = this.txtBranchFileHierarchy.Text;
			if (_eff.IsValid){
				_eff.Update();
				this.DialogResult = DialogResult.OK;
				this.Close();
			}else{
				MessageBox.Show(this, _eff.BrokenRules.ToString(), "QED");
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void EffortData_Load(object sender, System.EventArgs e) {
		
		}
	}
}

