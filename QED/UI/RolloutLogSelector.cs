using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QED.Business.CodePromotion;
using QED.Business;

namespace QED.UI
{
	/// <summary>
	/// Summary description for RolloutLogSelector.
	/// </summary>
	public class RolloutLogSelector : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox cboRollerClasses;
		private System.Windows.Forms.ComboBox cboRollTypes;
		private System.Windows.Forms.Button btnList;
		private System.Windows.Forms.ListBox lstLogs;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RolloutLogSelector()
		{
			InitializeComponent();
			Type[] allTypes =  System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
			foreach(Type type in allTypes){
				if (type.BaseType == Type.GetType("QED.Business.CodePromotion.Roller")){
					this.cboRollerClasses.Items.Add(type.Name);
				}
			}
			foreach(string rollerType in Enum.GetNames(Type.GetType("QED.Business.CodePromotion.RollType"))){
				this.cboRollTypes.Items.Add(rollerType);
			}
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
			this.cboRollerClasses = new System.Windows.Forms.ComboBox();
			this.lstLogs = new System.Windows.Forms.ListBox();
			this.cboRollTypes = new System.Windows.Forms.ComboBox();
			this.btnList = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cboRollerClasses
			// 
			this.cboRollerClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRollerClasses.Location = new System.Drawing.Point(8, 16);
			this.cboRollerClasses.Name = "cboRollerClasses";
			this.cboRollerClasses.Size = new System.Drawing.Size(144, 21);
			this.cboRollerClasses.TabIndex = 0;
			// 
			// lstLogs
			// 
			this.lstLogs.Location = new System.Drawing.Point(8, 48);
			this.lstLogs.Name = "lstLogs";
			this.lstLogs.Size = new System.Drawing.Size(352, 342);
			this.lstLogs.TabIndex = 1;
			this.lstLogs.DoubleClick += new System.EventHandler(this.lstLogs_DoubleClick);
			// 
			// cboRollTypes
			// 
			this.cboRollTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRollTypes.Location = new System.Drawing.Point(152, 16);
			this.cboRollTypes.Name = "cboRollTypes";
			this.cboRollTypes.Size = new System.Drawing.Size(120, 21);
			this.cboRollTypes.TabIndex = 2;
			// 
			// btnList
			// 
			this.btnList.Location = new System.Drawing.Point(280, 16);
			this.btnList.Name = "btnList";
			this.btnList.TabIndex = 3;
			this.btnList.Text = "List";
			this.btnList.Click += new System.EventHandler(this.btnList_Click);
			// 
			// RolloutLogSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 421);
			this.Controls.Add(this.btnList);
			this.Controls.Add(this.cboRollTypes);
			this.Controls.Add(this.lstLogs);
			this.Controls.Add(this.cboRollerClasses);
			this.Name = "RolloutLogSelector";
			this.Text = "RolloutLogSelector";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnList_Click(object sender, System.EventArgs e) {
			RolloutLogs logs = new RolloutLogs(this.cboRollerClasses.Text, this.cboRollTypes.Text);
			lstLogs.DisplayMember = "DateTime";
			if (logs.Count > 0){
				lstLogs.Items.Clear();
				foreach(RolloutLog log in logs){
					lstLogs.Items.Add(log);
				}
			}else{
				MessageBox.Show(this, "No logs found matching those criteria", "QED");
			}
		}

		private void lstLogs_DoubleClick(object sender, System.EventArgs e) {
			RolloutLog log = (RolloutLog)lstLogs.SelectedItem;
			string name = "REPORT: " + log.RollClass + " " + log.RollType;
			if (log != null){
				frmMain frm = (frmMain)this.Owner;
				TabPage tp = frm.CreateLogTab(name);
				RichTextBox rch = (RichTextBox)UI.GetControlByName(tp, "rch" + name);
				rch.Text = log.Text; 
				frm.Focus(tp);
			}
		}
	}
}
