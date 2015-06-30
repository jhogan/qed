using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using JCSLA;
using QED.Business;
using QED.SEC;
using System.IO;
using QED.Business.CodePromotion;
namespace QED.UI {
	/// <summary>
	/// Summary description for GenRoller.
	/// </summary>
	public class GenRoller : System.Windows.Forms.Form {
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtPlan;
		private System.Windows.Forms.TextBox txtFSOList;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtCVSSvr;
		private System.Windows.Forms.RadioButton rdoCVSSrc;
		private System.Windows.Forms.ComboBox cboCVSRepos;
		private System.Windows.Forms.ComboBox cboCVSMod;
		private System.Windows.Forms.TextBox txtExportRoot;
		private System.Windows.Forms.RadioButton rdoDirSrc;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtCVSBranch;
		private System.Windows.Forms.TextBox txtDest;
		private System.Windows.Forms.ComboBox cboSrcDir;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ComboBox cboDest;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		string n = System.Environment.NewLine;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.Button btnRefreshPlan;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtCVSUser;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cboScenarios;
		private System.Windows.Forms.Button btnLockParamControls;
		General_Roller _genRoller;
		public GenRoller() {
			InitializeComponent();
			_genRoller = new General_Roller();
			rdoCVSSrc.Checked = true;
			LoadDefaults();
			this.txtCVSBranch.Text = "HEAD";
			this.txtCVSSvr.Text = "CVS";
			this.txtCVSUser.Text = ((BusinessIdentity)System.Threading.Thread.CurrentPrincipal.Identity).UserName;
		}
		private void LoadDefaults(){
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			CatList pubList = cls.item("Public");
			Entry CVSPaths = pubList.Entry("/System/List/CVS Repositories");
			
			cboCVSRepos.Items.Clear();
			foreach(Entry ent in CVSPaths.Categories){
				cboCVSRepos.Items.Add(ent.SubEntry("Repos").Value);
			}
			
			cboSrcDir.Items.Clear();
			Entry srcDirs = pubList.Entry("/System/List/Source Directories");
			foreach(Entry ent in srcDirs.Categories){
				cboSrcDir.Items.Add(ent.SubEntry("PATH").Value);
			}

			cboCVSMod.Items.Clear();
			Entry CVSMod = pubList.Entry("/System/List/CVS Modules");
			foreach(Entry ent in CVSMod.Categories){
				cboCVSMod.Items.Add(ent.SubEntry("PATH").Value);
			}
			 
			cboDest.Items.Clear();
			cboDest.DisplayMember = "Path";
			cboDest.DropDownStyle = ComboBoxStyle.DropDownList;

			Entry rollDestEnts = pubList.Entry("/System/List/Generic Roll Destinations");
			RolloutDestination rollDest;
			foreach(Entry ent in rollDestEnts.Categories){
				rollDest  = new RolloutDestination();
				rollDest.Path = ent.SubEntry("Destination").Value;
				rollDest.BKPath = ent.SubEntry("Backup").Value;
				if (rollDest.Path.Trim() != null && rollDest.BKPath.Trim() != "")
					cboDest.Items.Add(rollDest);
			}

			cboScenarios.Items.Clear();
			Entry rollScenarios = pubList.Entry("/System/General Roller/Roll Scenarios");
			cboScenarios.DisplayMember = "Key";
			foreach(Entry scenarioEnt in rollScenarios.Categories){
					cboScenarios.Items.Add(scenarioEnt);
			}

			if (txtExportRoot.Text.Trim() == "")
				txtExportRoot.Text = pubList.Entry("/System/Temp CVS Export Root/PATH").Value;

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
			this.txtCVSSvr = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rdoDirSrc = new System.Windows.Forms.RadioButton();
			this.rdoCVSSrc = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.cboCVSRepos = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cboCVSMod = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtExportRoot = new System.Windows.Forms.TextBox();
			this.txtPlan = new System.Windows.Forms.TextBox();
			this.btnRun = new System.Windows.Forms.Button();
			this.txtFSOList = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtCVSBranch = new System.Windows.Forms.TextBox();
			this.txtDest = new System.Windows.Forms.TextBox();
			this.cboSrcDir = new System.Windows.Forms.ComboBox();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.cboDest = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.btnRefreshPlan = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.txtCVSUser = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.cboScenarios = new System.Windows.Forms.ComboBox();
			this.btnLockParamControls = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCVSSvr
			// 
			this.txtCVSSvr.Location = new System.Drawing.Point(152, 160);
			this.txtCVSSvr.Name = "txtCVSSvr";
			this.txtCVSSvr.Size = new System.Drawing.Size(560, 20);
			this.txtCVSSvr.TabIndex = 0;
			this.txtCVSSvr.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rdoDirSrc);
			this.groupBox1.Controls.Add(this.rdoCVSSrc);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(240, 64);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Roll Source";
			// 
			// rdoDirSrc
			// 
			this.rdoDirSrc.Enabled = false;
			this.rdoDirSrc.Location = new System.Drawing.Point(128, 24);
			this.rdoDirSrc.Name = "rdoDirSrc";
			this.rdoDirSrc.TabIndex = 1;
			this.rdoDirSrc.Text = "Directory";
			this.rdoDirSrc.CheckedChanged += new System.EventHandler(this.rdoDirSrc_CheckedChanged);
			// 
			// rdoCVSSrc
			// 
			this.rdoCVSSrc.Enabled = false;
			this.rdoCVSSrc.Location = new System.Drawing.Point(16, 24);
			this.rdoCVSSrc.Name = "rdoCVSSrc";
			this.rdoCVSSrc.TabIndex = 0;
			this.rdoCVSSrc.Text = "CVS";
			this.rdoCVSSrc.CheckedChanged += new System.EventHandler(this.rdoCVSSrc_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 160);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "CVS Server";
			// 
			// cboCVSRepos
			// 
			this.cboCVSRepos.Enabled = false;
			this.cboCVSRepos.Location = new System.Drawing.Point(152, 192);
			this.cboCVSRepos.Name = "cboCVSRepos";
			this.cboCVSRepos.Size = new System.Drawing.Size(560, 21);
			this.cboCVSRepos.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 192);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 23);
			this.label2.TabIndex = 5;
			this.label2.Text = "CVS Repos";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 224);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 23);
			this.label3.TabIndex = 7;
			this.label3.Text = "CVS Mod";
			// 
			// cboCVSMod
			// 
			this.cboCVSMod.Enabled = false;
			this.cboCVSMod.Location = new System.Drawing.Point(152, 224);
			this.cboCVSMod.Name = "cboCVSMod";
			this.cboCVSMod.Size = new System.Drawing.Size(560, 21);
			this.cboCVSMod.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 256);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 23);
			this.label4.TabIndex = 9;
			this.label4.Text = "CVS Export Root";
			// 
			// txtExportRoot
			// 
			this.txtExportRoot.Location = new System.Drawing.Point(152, 256);
			this.txtExportRoot.Name = "txtExportRoot";
			this.txtExportRoot.Size = new System.Drawing.Size(560, 20);
			this.txtExportRoot.TabIndex = 8;
			this.txtExportRoot.Text = "";
			// 
			// txtPlan
			// 
			this.txtPlan.Location = new System.Drawing.Point(8, 576);
			this.txtPlan.Multiline = true;
			this.txtPlan.Name = "txtPlan";
			this.txtPlan.ReadOnly = true;
			this.txtPlan.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtPlan.Size = new System.Drawing.Size(704, 96);
			this.txtPlan.TabIndex = 11;
			this.txtPlan.Text = "";
			this.txtPlan.WordWrap = false;
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(640, 688);
			this.btnRun.Name = "btnRun";
			this.btnRun.TabIndex = 12;
			this.btnRun.Text = "&Run";
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// txtFSOList
			// 
			this.txtFSOList.Location = new System.Drawing.Point(8, 352);
			this.txtFSOList.Multiline = true;
			this.txtFSOList.Name = "txtFSOList";
			this.txtFSOList.Size = new System.Drawing.Size(320, 176);
			this.txtFSOList.TabIndex = 14;
			this.txtFSOList.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(272, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 23);
			this.label5.TabIndex = 16;
			this.label5.Text = "Source Directory";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 288);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(112, 23);
			this.label6.TabIndex = 18;
			this.label6.Text = "CVS Branch";
			// 
			// txtCVSBranch
			// 
			this.txtCVSBranch.Location = new System.Drawing.Point(152, 288);
			this.txtCVSBranch.Name = "txtCVSBranch";
			this.txtCVSBranch.Size = new System.Drawing.Size(560, 20);
			this.txtCVSBranch.TabIndex = 17;
			this.txtCVSBranch.Text = "";
			// 
			// txtDest
			// 
			this.txtDest.Enabled = false;
			this.txtDest.Location = new System.Drawing.Point(344, 384);
			this.txtDest.Multiline = true;
			this.txtDest.Name = "txtDest";
			this.txtDest.Size = new System.Drawing.Size(368, 144);
			this.txtDest.TabIndex = 19;
			this.txtDest.Text = "";
			// 
			// cboSrcDir
			// 
			this.cboSrcDir.Enabled = false;
			this.cboSrcDir.Location = new System.Drawing.Point(368, 40);
			this.cboSrcDir.Name = "cboSrcDir";
			this.cboSrcDir.Size = new System.Drawing.Size(344, 21);
			this.cboSrcDir.TabIndex = 20;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(632, 8);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.TabIndex = 21;
			this.btnRefresh.Text = "&Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(632, 352);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.TabIndex = 22;
			this.btnAdd.Text = "&Add";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// cboDest
			// 
			this.cboDest.Enabled = false;
			this.cboDest.Location = new System.Drawing.Point(344, 352);
			this.cboDest.Name = "cboDest";
			this.cboDest.Size = new System.Drawing.Size(280, 21);
			this.cboDest.TabIndex = 23;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 320);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(320, 23);
			this.label7.TabIndex = 24;
			this.label7.Text = "Source Files and Folders";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(344, 320);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(352, 23);
			this.label8.TabIndex = 25;
			this.label8.Text = "Destinations";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 544);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(320, 23);
			this.label9.TabIndex = 26;
			this.label9.Text = "Plan";
			// 
			// btnRefreshPlan
			// 
			this.btnRefreshPlan.Location = new System.Drawing.Point(632, 536);
			this.btnRefreshPlan.Name = "btnRefreshPlan";
			this.btnRefreshPlan.Size = new System.Drawing.Size(80, 23);
			this.btnRefreshPlan.TabIndex = 27;
			this.btnRefreshPlan.Text = "Refresh &Plan";
			this.btnRefreshPlan.Click += new System.EventHandler(this.btnRefreshPlan_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 128);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(112, 23);
			this.label10.TabIndex = 31;
			this.label10.Text = "CVS User";
			// 
			// txtCVSUser
			// 
			this.txtCVSUser.Location = new System.Drawing.Point(152, 128);
			this.txtCVSUser.Name = "txtCVSUser";
			this.txtCVSUser.Size = new System.Drawing.Size(560, 20);
			this.txtCVSUser.TabIndex = 30;
			this.txtCVSUser.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(16, 96);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(112, 23);
			this.label11.TabIndex = 33;
			this.label11.Text = "Scenarios";
			// 
			// cboScenarios
			// 
			this.cboScenarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboScenarios.Location = new System.Drawing.Point(152, 96);
			this.cboScenarios.Name = "cboScenarios";
			this.cboScenarios.Size = new System.Drawing.Size(472, 21);
			this.cboScenarios.TabIndex = 32;
			this.cboScenarios.SelectedIndexChanged += new System.EventHandler(this.cboScenarios_SelectedIndexChanged);
			// 
			// btnLockParamControls
			// 
			this.btnLockParamControls.Location = new System.Drawing.Point(632, 96);
			this.btnLockParamControls.Name = "btnLockParamControls";
			this.btnLockParamControls.TabIndex = 34;
			this.btnLockParamControls.Text = "&Unlock";
			this.btnLockParamControls.Visible = false;
			this.btnLockParamControls.Click += new System.EventHandler(this.btnLockParamControls_Click);
			// 
			// GenRoller
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(728, 717);
			this.Controls.Add(this.btnLockParamControls);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.cboScenarios);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtCVSUser);
			this.Controls.Add(this.btnRefreshPlan);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.cboDest);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.cboSrcDir);
			this.Controls.Add(this.txtDest);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtCVSBranch);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtFSOList);
			this.Controls.Add(this.btnRun);
			this.Controls.Add(this.txtPlan);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtExportRoot);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cboCVSMod);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cboCVSRepos);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtCVSSvr);
			this.Name = "GenRoller";
			this.Text = "GenRoller";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void rdoCVSSrc_CheckedChanged(object sender, System.EventArgs e) {
			if (rdoCVSSrc.Checked){
				this.cboSrcDir.Enabled = false;
				this.txtCVSBranch.Enabled = true;
				this.txtCVSSvr.Enabled = true;
				this.txtExportRoot.Enabled = true;
				this.txtCVSUser.Enabled = true;
			}else{
				this.txtCVSBranch.Enabled = false;
				this.txtCVSSvr.Enabled = false;
				this.txtExportRoot.Enabled = false;
				this.cboCVSMod.Enabled = false;
				this.cboCVSRepos.Enabled = false;
				this.txtCVSUser.Enabled = false;
			}
		}
		private void rdoDirSrc_CheckedChanged(object sender, System.EventArgs e) {
			rdoCVSSrc_CheckedChanged(sender, e);
		}

		private void btnRefresh_Click(object sender, System.EventArgs e) {
			this.LoadDefaults();
		}

		private void btnAdd_Click(object sender, System.EventArgs e) {
			RolloutDestination rollDest = (RolloutDestination)cboDest.SelectedItem;
			if (rollDest != null){
				this.txtDest.Text += "Path: " + rollDest.Path + n;
				this.txtDest.Text += "BK: " + rollDest.BKPath + n; 
			}
		}
		private void btnRefreshPlan_Click(object sender, System.EventArgs e) {
			ReFillRoller();
			try{
				EnsureAllCVSFileExist();  //This will update plan
			}
			catch(Exception ex){
				txtPlan.Text = "";
				MessageBox.Show(this, "An error was encounter."  + n + ex.Message, "QED");
			}
		}
		private bool EnsureAllCVSFileExist(){
			bool returnFalse;
			if (_genRoller.GeneralRollSource.SourceType == SourceType.CVS){
				BrokenRules noSourceFileBrs = _genRoller.BrokenRules.GetWithName("NO_SOURCE_FILE");
				if (noSourceFileBrs.Count >0){
					if (_genRoller.BrokenRules.Count - noSourceFileBrs.Count == 0){ //The only broken rules are NO_SOURCE_FILE's
						string ask = "The following issues were found:" 
							+ n + noSourceFileBrs.ToString() + n + "You will need to update you local copy of CVS to continue. Would you like me to update you working copy and try again?";
						if (DialogResult.Yes == UI.AskYesNoDefNo(this, ask)){
							this.txtPlan.Text = "UPDATING LOCAL CVS WORKING DIRECTORY. PLEASE WAIT"; 	this.txtPlan.Refresh();
							_genRoller.UpdateCVSWorkingCopy();
							this.txtPlan.Text = "DONE UPDATING LOCAL CVS WORKING DIRECTORY";
							noSourceFileBrs = _genRoller.BrokenRules.GetWithName("NO_SOURCE_FILE");
							if (noSourceFileBrs.Count == 0){
								MessageBox.Show(this, "CVS checkout has fixed the problem. All source files are found in local working copy.", "QED");
								returnFalse = false;
							}else{
								MessageBox.Show(this, "CVS checkout has NOT fixed the problem. You must correct the source file/directory paths before continuing.", "QED");
								returnFalse = true;
							}
						}else{
							returnFalse = true;
						}
					}else{
						returnFalse = true;
					}
				}else{
					returnFalse = false;
				}
			}else{
				returnFalse = false;
			}
			txtPlan.Text = _genRoller.Plan;
			if (returnFalse){
				return false;
			}else{
				return true;
			}
		}

		public void ReFillRoller(){
			try{
				bool gettingTag = false; bool waitingfForPath = false; string tag = ""; string path = "";int i;
				string l;
				_genRoller = new General_Roller();
				//_genRoller.Clear();
				GeneralRollSrc genRollerSrc = _genRoller.GeneralRollSource;
				// Source
				if(rdoCVSSrc.Checked) {
					genRollerSrc.SourceType =  SourceType.CVS; 
					genRollerSrc.Branch = this.txtCVSBranch.Text;
					genRollerSrc.CVSExportRoot = new DirectoryInfo(this.txtExportRoot.Text);
					genRollerSrc.CVSModule = this.cboCVSMod.Text;
					genRollerSrc.CVSPath = this.cboCVSRepos.Text;
					genRollerSrc.CVSServer = this.txtCVSSvr.Text;
					genRollerSrc.CVSUser = this.txtCVSUser.Text;

				}else{
					genRollerSrc.SourceType =  SourceType.Directory;
					genRollerSrc.BaseDir = new DirectoryInfo(this.cboSrcDir.Text);
				}
				foreach(string line in txtFSOList.Lines){
					l = line.Trim();
					if (l == "") continue;
					genRollerSrc.AddSourceFileSystemInfo(l);
				}
				// Dest
				GeneralRollDest rollDest = null;
				bool lineWaitForBK = true;	bool lineWaitForDest = false; //These will be immediatly negated in loop
				foreach(string line in txtDest.Lines){
					l = line.Trim();
					char[] chs = l.ToCharArray();
					if (l == "") continue;
					lineWaitForBK = !lineWaitForBK; lineWaitForDest = !lineWaitForDest;
					i=0; gettingTag = false; waitingfForPath = false; tag = ""; path = "";
					foreach(char ch in chs){
						if (i++ == 0) gettingTag = true;
						if (gettingTag && ch == ' '){
							gettingTag = false;
							waitingfForPath = true;
						}
						if (gettingTag){
							tag += ch.ToString();
						}
						if (waitingfForPath && ch != ' ')
							waitingfForPath = false;

						if (!gettingTag &&  !waitingfForPath){
							path += ch.ToString();
						}
					}
					switch (tag.ToUpper()){
						case "BK:":
							if (rollDest.BaseDir != null && lineWaitForBK)
								rollDest.BackupDir = new DirectoryInfo(path);
							else
								throw new Exception("Destination text box is in the wrong format. Found at " + line);
							break;
						case "PATH:":
							if (lineWaitForDest){
								rollDest = _genRoller.GeneralRollDestinations.CreateNew();
								rollDest.BaseDir = new DirectoryInfo(path);
							}else{
								throw new Exception("Destination text box is in the wrong format. Found at " + line);
							 }
							break;
						default:
							throw new Exception("In the \"Destination\" textbox, the tag \""+ tag +"\" is invalid. Did you forget the \":\".");
					}
				}
				if (!lineWaitForBK){
					throw new Exception("Destination text box is in the wrong format. Must end with BK directory");
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, "An error was encounter. See the \"Plan\" text box for more information." + n + ex.Message, "QED");
			}
		}

		private void btnRun_Click(object sender, System.EventArgs e) {
			try{
				frmMain main = (frmMain)this.Owner;
				this.ReFillRoller();
				if (EnsureAllCVSFileExist()){
					ShowRollerWarnings();
					if (_genRoller.IsValid){
						if (UI.AskYesNoDefNo(this, "Have you checked the plan and are you sure you want to execute this plan?") == DialogResult.Yes){
							main.SetupRoller(_genRoller); 
							_genRoller.Roll();
							this.Clear();
						}
					}else{
						MessageBox.Show(this, "Parameters seem to be invalid for this roll. Check the plan and try again." +n + _genRoller.BrokenRules.ToString(), "QED");
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void ShowRollerWarnings(){
			BrokenRules rules = _genRoller.BrokenRulesWarnings;
			if (rules.Count>0){
				string msg = "Potential problems were found with the configuration of this roll. Please analyze the below warning to make sure there aren't problems." + n;
				msg += rules.ToString();
				MessageBox.Show(this, msg, "QED");
			}
		}

		private void btnLockParamControls_Click(object sender, System.EventArgs e) {
			if (btnLockParamControls.Text == "Unlock"){
				if (this.rdoCVSSrc.Checked){
					this.cboCVSMod.Enabled = true;
					this.cboCVSRepos.Enabled = true;
				}else{
					this.cboSrcDir.Enabled = true;
				}
				this.txtDest.Enabled = true;
				this.btnAdd.Enabled = true;
				this.cboDest.Enabled = true;
				btnLockParamControls.Text = "Lock";
			}else{
				this.cboCVSMod.Enabled = false;
				this.cboCVSRepos.Enabled = false;
				this.cboSrcDir.Enabled = false;
				this.txtDest.Enabled = false;
				this.btnAdd.Enabled = false;
				this.cboDest.Enabled = false;
				btnLockParamControls.Text = "Unlock";
			}
		}
		
		private void Clear(){
			this.cboCVSRepos.Text = "";
			this.cboCVSMod.Text = "";
			this.cboSrcDir.Text = "";
			this.cboDest.Text = "";
			this.txtDest.Text = "";
			this.cboCVSRepos.Text = "";
		}
		private void cboScenarios_SelectedIndexChanged(object sender, System.EventArgs e) {
			Entry scenarioEnt = (Entry)this.cboScenarios.SelectedItem;
			string listPath = "";
			CatList catList;
			SourceType sourceType = SourceType.CVS;
			if (scenarioEnt != null){
				txtDest.Text = "";
				catList = scenarioEnt.CatList;					
				switch (scenarioEnt.SubEntry("SourceType").Value.ToUpper()){
					case "DIRECTORY":
						sourceType = SourceType.Directory;
						this.cboCVSMod.Enabled = false;
						this.cboCVSRepos.Enabled = false;
						break;
					case "CVS":
						sourceType = SourceType.CVS;
						this.cboSrcDir.Enabled = false;
						break;
					default:
						MessageBox.Show(this, "The SourceType at " + scenarioEnt.Path + " is in valid. Reconfigure the list, click refresh and try again.", "QED");
						return;
				}
				
				if (sourceType == SourceType.CVS) this.rdoCVSSrc.Checked = true;
				else this.rdoDirSrc.Checked = true;

				foreach(Entry ent in scenarioEnt.Categories){
					switch(ent.Key.ToUpper()){
						case "DESTINATIONS" :
							foreach (Entry dest in ent.Categories){
								listPath = dest.SubEntry("ListPath").Value;
								txtDest.Text += "Path: " + catList.Entry(listPath).SubEntry("Destination").Value + n;
								txtDest.Text += "BK: " + catList.Entry(listPath).SubEntry("Backup").Value + n;
							}
							break;
						case "CVS MODULES": 
							listPath = ent.SubEntry("ListPath").Value;
							this.cboCVSMod.Text = catList.Entry(listPath).SubEntry("PATH").Value;
							break;
						case "CVS REPOSITORY":
							listPath = ent.SubEntry("ListPath").Value;
							this.cboCVSRepos.Text = catList.Entry(listPath).SubEntry("Repos").Value;
							break;
						case "SOURCE DIRECTORY":
							listPath = ent.SubEntry("ListPath").Value;
							this.cboSrcDir.Text = catList.Entry(listPath).SubEntry("Path").Value;
							break;
						default:
							MessageBox.Show(this, "The catagory name " + ent.Path + " is in valid. Reconfigure the list, click refresh and try again.", "QED");
							return;
					}
				}
			}
		}
	}
	public class RolloutDestination{
		public string  _path;
		public string BKPath;
		public string Path{
			get{
				return _path;
			}
			set{
				_path = value;
			}
		}
	}
}


