using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QED.Business;
using JCSLA;
using System.Diagnostics;
namespace QED.UI
{
	/// <summary>
	/// Summary description for RolloutComplete.
	/// </summary>
	public class RolloutComplete : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		Rollout _rollout;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtRolledBy;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtFinalComments;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dtpRolledDate;
		private System.Windows.Forms.GroupBox gbRolledStatus;
		private System.Windows.Forms.RadioButton rdoUnrolled;
		private System.Windows.Forms.RadioButton rdoRolledBack;
		private System.Windows.Forms.RadioButton rdoRolled;
		private System.Windows.Forms.TextBox txtScheduledRollDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView lvwEfforts;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		string _defaultUserDomain;
		string n = System.Environment.NewLine;
		bool _suppressRollCompletionProcess = false;
		
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.txtRolledBy = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtFinalComments = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.dtpRolledDate = new System.Windows.Forms.DateTimePicker();
			this.gbRolledStatus = new System.Windows.Forms.GroupBox();
			this.rdoUnrolled = new System.Windows.Forms.RadioButton();
			this.rdoRolledBack = new System.Windows.Forms.RadioButton();
			this.rdoRolled = new System.Windows.Forms.RadioButton();
			this.txtScheduledRollDate = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lvwEfforts = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.gbRolledStatus.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(752, 488);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 11;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(832, 488);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(112, 23);
			this.label8.TabIndex = 32;
			this.label8.Text = "Rolled By";
			// 
			// txtRolledBy
			// 
			this.txtRolledBy.Location = new System.Drawing.Point(224, 24);
			this.txtRolledBy.Name = "txtRolledBy";
			this.txtRolledBy.Size = new System.Drawing.Size(216, 20);
			this.txtRolledBy.TabIndex = 0;
			this.txtRolledBy.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 136);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(200, 23);
			this.label7.TabIndex = 30;
			this.label7.Text = "Final Comments";
			// 
			// txtFinalComments
			// 
			this.txtFinalComments.Location = new System.Drawing.Point(224, 128);
			this.txtFinalComments.Multiline = true;
			this.txtFinalComments.Name = "txtFinalComments";
			this.txtFinalComments.Size = new System.Drawing.Size(664, 80);
			this.txtFinalComments.TabIndex = 10;
			this.txtFinalComments.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 23);
			this.label2.TabIndex = 20;
			this.label2.Text = "Scheduled Roll Date";
			// 
			// dtpRolledDate
			// 
			this.dtpRolledDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpRolledDate.Location = new System.Drawing.Point(224, 88);
			this.dtpRolledDate.Name = "dtpRolledDate";
			this.dtpRolledDate.TabIndex = 5;
			this.dtpRolledDate.Value = new System.DateTime(2005, 11, 14, 10, 58, 41, 395);
			// 
			// gbRolledStatus
			// 
			this.gbRolledStatus.Controls.Add(this.rdoUnrolled);
			this.gbRolledStatus.Controls.Add(this.rdoRolledBack);
			this.gbRolledStatus.Controls.Add(this.rdoRolled);
			this.gbRolledStatus.Location = new System.Drawing.Point(584, 24);
			this.gbRolledStatus.Name = "gbRolledStatus";
			this.gbRolledStatus.Size = new System.Drawing.Size(304, 64);
			this.gbRolledStatus.TabIndex = 3;
			this.gbRolledStatus.TabStop = false;
			// 
			// rdoUnrolled
			// 
			this.rdoUnrolled.Location = new System.Drawing.Point(16, 24);
			this.rdoUnrolled.Name = "rdoUnrolled";
			this.rdoUnrolled.Size = new System.Drawing.Size(88, 24);
			this.rdoUnrolled.TabIndex = 0;
			this.rdoUnrolled.Text = "Unrolled";
			// 
			// rdoRolledBack
			// 
			this.rdoRolledBack.Location = new System.Drawing.Point(208, 24);
			this.rdoRolledBack.Name = "rdoRolledBack";
			this.rdoRolledBack.Size = new System.Drawing.Size(88, 24);
			this.rdoRolledBack.TabIndex = 2;
			this.rdoRolledBack.Text = "Rolled Back";
			// 
			// rdoRolled
			// 
			this.rdoRolled.Location = new System.Drawing.Point(112, 24);
			this.rdoRolled.Name = "rdoRolled";
			this.rdoRolled.Size = new System.Drawing.Size(64, 24);
			this.rdoRolled.TabIndex = 1;
			this.rdoRolled.Text = "Rolled";
			this.rdoRolled.CheckedChanged += new System.EventHandler(this.rdoRolled_CheckedChanged);
			// 
			// txtScheduledRollDate
			// 
			this.txtScheduledRollDate.Location = new System.Drawing.Point(224, 56);
			this.txtScheduledRollDate.Name = "txtScheduledRollDate";
			this.txtScheduledRollDate.ReadOnly = true;
			this.txtScheduledRollDate.Size = new System.Drawing.Size(216, 20);
			this.txtScheduledRollDate.TabIndex = 4;
			this.txtScheduledRollDate.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 88);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 23);
			this.label1.TabIndex = 16;
			this.label1.Text = "Rolled Date";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lvwEfforts);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.txtRolledBy);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.txtFinalComments);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.dtpRolledDate);
			this.groupBox1.Controls.Add(this.gbRolledStatus);
			this.groupBox1.Controls.Add(this.txtScheduledRollDate);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(896, 464);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			// 
			// lvwEfforts
			// 
			this.lvwEfforts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						 this.columnHeader1,
																						 this.columnHeader2,
																						 this.columnHeader3,
																						 this.columnHeader4,
																						 this.columnHeader5,
																						 this.columnHeader6});
			this.lvwEfforts.GridLines = true;
			this.lvwEfforts.Location = new System.Drawing.Point(16, 224);
			this.lvwEfforts.Name = "lvwEfforts";
			this.lvwEfforts.Size = new System.Drawing.Size(872, 224);
			this.lvwEfforts.TabIndex = 33;
			this.lvwEfforts.View = System.Windows.Forms.View.Details;
			this.lvwEfforts.DoubleClick += new System.EventHandler(this.lvwEfforts_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Effort";
			this.columnHeader1.Width = 88;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Final Comments";
			this.columnHeader2.Width = 197;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Reason for Rollback";
			this.columnHeader3.Width = 180;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Reason for Code Fix";
			this.columnHeader4.Width = 123;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Was Code Fixed";
			this.columnHeader5.Width = 110;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Responsible Department for Error";
			this.columnHeader6.Width = 168;
			// 
			// RolloutComplete
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(920, 525);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RolloutComplete";
			this.Text = "Complete Roll";
			this.gbRolledStatus.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public RolloutComplete(Rollout rollout) {
			InitializeComponent();
			ListViewItem item;
			_rollout = rollout;
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			CatList system = cls.item("System");
			_defaultUserDomain = system.Entry("/Defaults/UserDomain").Value;
			string deptName = "";
			this.rdoUnrolled.Checked = (!_rollout.RolledBack && !_rollout.Rolled);
			this.rdoRolledBack.Checked = _rollout.RolledBack;
			_suppressRollCompletionProcess = true; this.rdoRolled.Checked = _rollout.Rolled; _suppressRollCompletionProcess = false; 
			this.txtScheduledRollDate.Text = rollout.ScheduledDate.ToLongDateString();
			this.txtFinalComments.Text = rollout.FinalComments;
			this.txtRolledBy.Text = (_rollout.RolledBy == "") ? _defaultUserDomain : _rollout.RolledBy;
			if (rollout.RolledDate != DateTime.MinValue)
				this.dtpRolledDate.Value = rollout.RolledDate;
			EffortRollout er; 
			foreach(Effort eff in rollout.Efforts){
				er = rollout.GetEffortRollout(eff);
				//er = eff.GetEffortRollout(rollout);
				if (er.DepartmentResponsibleForError != null){
					deptName = er.DepartmentResponsibleForError.Name;
				}
				item = new ListViewItem(new string[]{eff.ConventionalId, er.FinalComments, er.ReasonForRollBack, 
														er.ReasonForCodeFix, er.CodeFixedYesNo, deptName});
				item.Tag = er;
				lvwEfforts.Items.Add(item);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void btnOK_Click(object sender, System.EventArgs e) {
			try{
				if ( !rdoUnrolled.Checked ||
					(rdoUnrolled.Checked	&&
						MessageBox.Show(this, "You marked the roll as \"Unrolled\". Is this correct", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)){
						Rollout r  = _rollout;
						r.Rolled = this.rdoRolled.Checked;
						r.RolledBack = this.rdoRolledBack.Checked;
						r.RolledDate = DateTime.Parse(this.dtpRolledDate.Text);
						r.ScheduledDate = DateTime.Parse(this.txtScheduledRollDate.Text);
						r.FinalComments = this.txtFinalComments.Text;
						r.RolledBy = (this.txtRolledBy.Text== _defaultUserDomain) ? "" : this.txtRolledBy.Text;
						if (r.IsValid) {
							r.Update();
							this.Close();
						}else{
							MessageBox.Show(this, "Problem saving rollout completion information to DB. No information was saved.\r\n" + r.BrokenRules.ToString());
						}
					}
			}
			catch(Exception ex) {
				MessageBox.Show(this, ex.Message);
			}
		}

		private void lvwEfforts_DoubleClick(object sender, System.EventArgs e) {
			try{
				const string COMMENT = "Comment";
				const string REASON_FOR_CODE_FIX = "Reason for Code Fix";
				const string REASON_FOR_ROLL_BACK = "Reason for Rollback";
				const string WAS_CODE_FIXED = "Was Code Fixed";
				const string RESPONSIBLE_DEPARTMENT_FOR_ERROR = "Dept Responsible";
				ComboBox cb;
				if (lvwEfforts.SelectedItems[0] != null){
					ListViewItem item = lvwEfforts.SelectedItems[0];
					EffortRollout er = (EffortRollout)item.Tag;
					InputModal im = new InputModal("", COMMENT, REASON_FOR_CODE_FIX, REASON_FOR_ROLL_BACK);
					cb = new ComboBox();
					cb.Name = WAS_CODE_FIXED;
					cb.Items.AddRange(new string[]{"", "Yes", "No"});
					cb.DropDownStyle = ComboBoxStyle.DropDownList;
					cb.Text = er.CodeFixedYesNo;
					im.AddToPanel(cb);
					cb = new ComboBox();
					cb.Name = RESPONSIBLE_DEPARTMENT_FOR_ERROR;
					cb.DisplayMember = "Name";
					cb.DropDownStyle = ComboBoxStyle.DropDownList;
					foreach(Group gr in Groups.Inst.Active){
					    cb.Items.Add(gr);
						if (er.DepartmentResponsibleForError != null && er.DepartmentResponsibleForError.Id == gr.Id)
							cb.SelectedItem = gr;
					}
					im.AddToPanel(cb);
					((TextBox)im.AnswerTable[COMMENT]).Text = er.FinalComments;
					((TextBox)im.AnswerTable[REASON_FOR_CODE_FIX]).Text = er.ReasonForCodeFix;
					((TextBox)im.AnswerTable[REASON_FOR_ROLL_BACK]).Text = er.ReasonForRollBack;
					if (im.ShowDialog(this) == DialogResult.OK){
						er.FinalComments = ((TextBox)im.AnswerTable[COMMENT]).Text;
						er.ReasonForCodeFix = ((TextBox)im.AnswerTable[REASON_FOR_CODE_FIX]).Text;
						er.ReasonForRollBack = ((TextBox)im.AnswerTable[REASON_FOR_ROLL_BACK]).Text;
						string wasCodeFixed  = (string)((ComboBox)im.AnswerTable[WAS_CODE_FIXED]).SelectedItem;
						er.CodeFixedYesNo = wasCodeFixed;

						if (((ComboBox)im.AnswerTable[RESPONSIBLE_DEPARTMENT_FOR_ERROR]).SelectedItem != null)
							er.DepartmentResponsibleForError = ((Group)((ComboBox)im.AnswerTable[RESPONSIBLE_DEPARTMENT_FOR_ERROR]).SelectedItem);
						if (er.IsValid){
							er.Update();
							item.SubItems[0].Text = er.Effort.ConventionalId;
							item.SubItems[1].Text = er.FinalComments;
							item.SubItems[2].Text = er.ReasonForRollBack;
							item.SubItems[3].Text = er.ReasonForCodeFix;
							item.SubItems[4].Text = er.CodeFixedYesNo;
							if (er.DepartmentResponsibleForError != null)
								item.SubItems[5].Text = er.DepartmentResponsibleForError.Name;
						}else{
							MessageBox.Show(this, er.BrokenRules.ToString(), "QED");
						}
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}

		private void rdoRolled_CheckedChanged(object sender, System.EventArgs e) {
			bool cancelTicketClose = false;
			string projects = ""; string from = ""; 	string subject = ""; string body = ""; 	string to = "";
			string smtp = "";	DialogResult res; string effortReport = ""; string commonEmail=""; 
			ArrayList distinct = new ArrayList();
			if (!_suppressRollCompletionProcess  && rdoRolled.Checked){
				CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
				smtp = cls.item("System").Entry("/Email/SMTP").Value.Trim();
				CatList pubList = cls.item("Public");
				Entry emails = pubList.Entry("/System/Email/Rollout Release Emails");
				from = emails.SubEntry("From").Value.Trim();
				subject = emails.SubEntry("Subject").Value.Trim();
				body = emails.SubEntry("Body").Value.Trim();
				effortReport = "";
				to = "";
				foreach(Entry ent in emails.Categories){
					if (_rollout.ClientId == Convert.ToInt16(ent.SubEntry("HTS Client ID").Value.Trim())){
						to = ent.SubEntry("To").Value;
						commonEmail = ent.SubEntry("CommonRelease").Value;
						break;
					}
				}
				if (UI.AskYesNoDefYes(this, "Would you like to compose and send an email to the release group") == DialogResult.Yes){
					foreach(Effort eff in _rollout.Efforts){
						if (eff.Rolled){
							effortReport += "\t" + eff.ExternalId_Desc + n;
						}
					}
					body = body.Replace("\\n", n);
					body = body.Replace("@@EFFORTS",  effortReport);
					SendMail sendMail = new SendMail(from, to, subject, body, smtp);
					sendMail.ShowDialog(this);
				}
				if (commonEmail != ""){
					subject = emails.SubEntry("CommonSubject").Value.Trim();
					body = emails.SubEntry("CommonEmailBody").Value.Trim();
					body = body.Replace("@@ROLLED_CLIENT", _rollout.Client.Name);
					SendMail sendMail = new SendMail(from, commonEmail, subject, body, smtp, true);
					sendMail.ShowDialog(this);
				}

				if (UI.AskYesNoDefNo(this, "Would you like to open the rolled efforts in your browser to close them?") == DialogResult.Yes){
					foreach (Effort eff in _rollout.RolledEfforts){
						if (!cancelTicketClose && eff.EffortType == EffortType.Ticket){
							res = MessageBox.Show(this, "Open " + eff.ExternalId_Desc + "?", "QED", MessageBoxButtons.YesNoCancel);
							if (res == DialogResult.Yes)
								Process.Start(eff.URLReference);
							else if (res == DialogResult.Cancel)
								cancelTicketClose = true;
						}else if (eff.EffortType == EffortType.Project){
							projects += "\t" + eff.ExternalId_Desc + n;
						}
					}
					if (projects != ""){
						if (MessageBox.Show(this, "The below projects were rolled. Do you want to open PMO to close them?" + n + projects, "QED", MessageBoxButtons.YesNo) == DialogResult.Yes){
							Process.Start("http://matrix/pmo/pmo_office/glb_base/pub_wf/index.cfm?view=W");
						}
					}
				}
			}
		}
	}
}
