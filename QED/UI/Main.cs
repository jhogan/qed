using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using QED.Business;
using System.Threading;
using JCSLA;
using System.Security.Principal;
using QED.SEC;
using QED.Net;
using QED.Business.CodePromotion;
using JCSLA.Reports;
using System.Drawing.Printing;
using Microsoft.Office.Core;
using QED.UI.Reports;
namespace QED.UI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		
		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabRollout;
		private System.Windows.Forms.TabPage tablList;
		private System.Windows.Forms.TreeView tvwHier;
		private System.Windows.Forms.ComboBox cboCatLists;
		private System.Windows.Forms.ContextMenu mnuListHier;
		private System.Windows.Forms.MenuItem mnuItemListHierDelete;
		private System.Windows.Forms.MenuItem mnuItemListHierRefresh;
		private System.Windows.Forms.MenuItem mnuItemListHierRename;
		public System.Windows.Forms.ListView lvKVP;
		private System.Windows.Forms.ColumnHeader colheadKey;
		private System.Windows.Forms.ColumnHeader colHeadVal;
		private System.Windows.Forms.MenuItem mnuItemListHierCatAdd;
		private System.Windows.Forms.MenuItem mnuItemListHierEntAdd;
		private System.Windows.Forms.TabPage tabTesting;
		private System.Windows.Forms.Button btnSearchTestingEffort;
		private System.Windows.Forms.TextBox txtTestROEffort;
		private System.Windows.Forms.Button btnRollBackAllEffs;
		private System.Windows.Forms.Button btnRollBackEff;
		private System.Windows.Forms.Button btnRollAllEffs;
		private System.Windows.Forms.Button btnRollEff;
		private System.Windows.Forms.ListBox lstRolled;
		private System.Windows.Forms.ListBox lstUnrolled;
		private System.ComponentModel.IContainer components;
		
		private System.Windows.Forms.ListView lvTestDefectNotes;
		private System.Windows.Forms.ListView lvTestNotes;
		private System.Windows.Forms.ListView lvRollNotes;
		private System.Windows.Forms.CheckBox chkApproved;
		private System.Windows.Forms.ContextMenu mnuNotes;
		private System.Windows.Forms.MenuItem mnuItemNotesAdd;
		private System.Windows.Forms.MenuItem mnuItemNotesDelete;
		private System.Windows.Forms.ListView lvRollDefectNotes;

		private System.Windows.Forms.ColumnHeader col;
		private System.Windows.Forms.ColumnHeader Defects;
		private System.Windows.Forms.ColumnHeader notes;
		private System.Windows.Forms.ColumnHeader colRollDefects;
		private System.Windows.Forms.TextBox txtEffortId;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.DateTimePicker dtpRoll;
		private System.Windows.Forms.ComboBox cboClients;
		private System.Windows.Forms.ContextMenu mnuGenListBox;
		private System.Windows.Forms.MenuItem mnuItemGenListBoxAdd;
		private System.Windows.Forms.MenuItem mnuItemGenListBoxRemove;
		private System.Windows.Forms.Label lblRolled;
		private System.Windows.Forms.Button btnCompleteRoll;
		private System.Windows.Forms.MenuItem mnuItemAbout;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.TextBox txtListPath;
		private System.Windows.Forms.MenuItem mnuConnections;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem mnuItemIBM_PROD_PP_OS;
		private System.Windows.Forms.MenuItem mnuItemIBM_PROD_MAX;
		private System.Windows.Forms.TextBox txtEffDesc;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnEffortData;
		private System.Windows.Forms.Button btnFindRollByRollDate;
		private System.Windows.Forms.Button btnFindRollByScheduledRollDate;
		private System.Windows.Forms.MenuItem mnuItemIBM_UAT_REP;
		public System.Windows.Forms.Button btnTestTimer;
		private System.Windows.Forms.Button btnRollTimer;
		private System.Windows.Forms.MenuItem mnuItemTimeRollout;
		private System.Windows.Forms.MenuItem mnuItemTimeEffort;
		
		private System.Windows.Forms.Button bntAddKVP;
		private System.Windows.Forms.Button btnDeleteKVP;
		private System.Windows.Forms.MenuItem mnuItemIBM_UAT_PP_OS;
		private System.Windows.Forms.MenuItem mnuItemLenovo_UAT_PP_OS;
		private System.Windows.Forms.MenuItem mnuItemLenovo_UAT_REP;
		private System.Windows.Forms.MenuItem mnuItemLenovo_Prod_PP_OS;
		private System.Windows.Forms.MenuItem mnuItemLenovo_Prod_Max;
		private System.Windows.Forms.MenuItem mnuItemIBM_PROD_Reptool;
		private System.Windows.Forms.MenuItem mnuItemLenovo_Prod_Reptool;
		private System.Windows.Forms.MenuItem mnuItemIBM_UAT_MAX;
		private System.Windows.Forms.MenuItem mnuItemLenovo_UAT_MAX;
		bool _tvwHierWasLoaded;
		CatList _pubList;
		public CatList _systemList;
		Effort _testingEff =null;
		Rollout _rollout;
		DateTime _timeTesting = DateTime.MinValue;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem mnuItemRolloutLogs;
		int _rollerCount = 0;
		private System.Windows.Forms.MenuItem mnuItemTimersCurrentlyRunning;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem mnuItemIBM_Merge_Max;
		private System.Windows.Forms.MenuItem mnuItemTimeReport;
		private System.Windows.Forms.MenuItem mnuItemQAPostRollReportCard;
		private System.Windows.Forms.MenuItem mnuItemStartEffort;
		private System.Windows.Forms.MenuItem mnuItemUnapprovedEfforts;
		private System.Windows.Forms.MenuItem mnuItemUnrolledEfforts;
		private System.Windows.Forms.MenuItem mnuItemEmployeeTime;
		private System.Windows.Forms.MenuItem mnuItemScheduledRollouts;
		private System.Windows.Forms.MenuItem mnuItemManagerReports;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.MenuItem mnuItemCodePromotion;
		private System.Windows.Forms.MenuItem mnuItemGenCodeRoller;
		Times _times = new Times();
		private System.Windows.Forms.MenuItem mnuItemRolloutsAndBranches;
		private System.Windows.Forms.MenuItem mnuItemsDevRollouts;
		GenRoller  _genRoller;
		private System.Windows.Forms.Label lblUnrolledEfforts;
		private System.Windows.Forms.Label lblRolledEfforts;
		private System.Windows.Forms.MenuItem mnuItemRolloutTicketBreakdown;
		private System.Windows.Forms.MenuItem mnuItemsTruncatedRolloutTicketBreakdown;
		bool ignoreChkApprovedSave = false;
		public frmMain() {
			InitializeComponent();
		}

		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mnuItemStartEffort = new System.Windows.Forms.MenuItem();
			this.mnuItemCodePromotion = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_PROD_PP_OS = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_PROD_MAX = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_PROD_Reptool = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.mnuItemLenovo_Prod_PP_OS = new System.Windows.Forms.MenuItem();
			this.mnuItemLenovo_Prod_Max = new System.Windows.Forms.MenuItem();
			this.mnuItemLenovo_Prod_Reptool = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_UAT_PP_OS = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_UAT_REP = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_UAT_MAX = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.mnuItemLenovo_UAT_PP_OS = new System.Windows.Forms.MenuItem();
			this.mnuItemLenovo_UAT_REP = new System.Windows.Forms.MenuItem();
			this.mnuItemLenovo_UAT_MAX = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.mnuItemIBM_Merge_Max = new System.Windows.Forms.MenuItem();
			this.mnuItemGenCodeRoller = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.mnuItemRolloutLogs = new System.Windows.Forms.MenuItem();
			this.mnuItemTimeReport = new System.Windows.Forms.MenuItem();
			this.mnuItemQAPostRollReportCard = new System.Windows.Forms.MenuItem();
			this.mnuItemUnrolledEfforts = new System.Windows.Forms.MenuItem();
			this.mnuItemUnapprovedEfforts = new System.Windows.Forms.MenuItem();
			this.mnuItemScheduledRollouts = new System.Windows.Forms.MenuItem();
			this.mnuItemManagerReports = new System.Windows.Forms.MenuItem();
			this.mnuItemEmployeeTime = new System.Windows.Forms.MenuItem();
			this.mnuItemRolloutsAndBranches = new System.Windows.Forms.MenuItem();
			this.mnuItemsDevRollouts = new System.Windows.Forms.MenuItem();
			this.mnuItemRolloutTicketBreakdown = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuItemTimeRollout = new System.Windows.Forms.MenuItem();
			this.mnuItemTimeEffort = new System.Windows.Forms.MenuItem();
			this.mnuItemTimersCurrentlyRunning = new System.Windows.Forms.MenuItem();
			this.mnuConnections = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mnuItemAbout = new System.Windows.Forms.MenuItem();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabTesting = new System.Windows.Forms.TabPage();
			this.btnEffortData = new System.Windows.Forms.Button();
			this.txtEffDesc = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lvTestDefectNotes = new System.Windows.Forms.ListView();
			this.Defects = new System.Windows.Forms.ColumnHeader();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.lvTestNotes = new System.Windows.Forms.ListView();
			this.col = new System.Windows.Forms.ColumnHeader();
			this.chkApproved = new System.Windows.Forms.CheckBox();
			this.btnTestTimer = new System.Windows.Forms.Button();
			this.txtTestROEffort = new System.Windows.Forms.TextBox();
			this.txtEffortId = new System.Windows.Forms.TextBox();
			this.btnSearchTestingEffort = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.tabRollout = new System.Windows.Forms.TabPage();
			this.btnFindRollByScheduledRollDate = new System.Windows.Forms.Button();
			this.btnCompleteRoll = new System.Windows.Forms.Button();
			this.lblRolled = new System.Windows.Forms.Label();
			this.btnFindRollByRollDate = new System.Windows.Forms.Button();
			this.dtpRoll = new System.Windows.Forms.DateTimePicker();
			this.cboClients = new System.Windows.Forms.ComboBox();
			this.lvRollDefectNotes = new System.Windows.Forms.ListView();
			this.colRollDefects = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.lvRollNotes = new System.Windows.Forms.ListView();
			this.notes = new System.Windows.Forms.ColumnHeader();
			this.btnRollBackAllEffs = new System.Windows.Forms.Button();
			this.btnRollBackEff = new System.Windows.Forms.Button();
			this.btnRollAllEffs = new System.Windows.Forms.Button();
			this.btnRollEff = new System.Windows.Forms.Button();
			this.lblUnrolledEfforts = new System.Windows.Forms.Label();
			this.lblRolledEfforts = new System.Windows.Forms.Label();
			this.lstRolled = new System.Windows.Forms.ListBox();
			this.lstUnrolled = new System.Windows.Forms.ListBox();
			this.btnRollTimer = new System.Windows.Forms.Button();
			this.tablList = new System.Windows.Forms.TabPage();
			this.txtListPath = new System.Windows.Forms.TextBox();
			this.bntAddKVP = new System.Windows.Forms.Button();
			this.btnDeleteKVP = new System.Windows.Forms.Button();
			this.lvKVP = new System.Windows.Forms.ListView();
			this.colheadKey = new System.Windows.Forms.ColumnHeader();
			this.colHeadVal = new System.Windows.Forms.ColumnHeader();
			this.cboCatLists = new System.Windows.Forms.ComboBox();
			this.tvwHier = new System.Windows.Forms.TreeView();
			this.mnuListHier = new System.Windows.Forms.ContextMenu();
			this.mnuItemListHierCatAdd = new System.Windows.Forms.MenuItem();
			this.mnuItemListHierEntAdd = new System.Windows.Forms.MenuItem();
			this.mnuItemListHierDelete = new System.Windows.Forms.MenuItem();
			this.mnuItemListHierRename = new System.Windows.Forms.MenuItem();
			this.mnuItemListHierRefresh = new System.Windows.Forms.MenuItem();
			this.mnuNotes = new System.Windows.Forms.ContextMenu();
			this.mnuItemNotesAdd = new System.Windows.Forms.MenuItem();
			this.mnuItemNotesDelete = new System.Windows.Forms.MenuItem();
			this.mnuGenListBox = new System.Windows.Forms.ContextMenu();
			this.mnuItemGenListBoxAdd = new System.Windows.Forms.MenuItem();
			this.mnuItemGenListBoxRemove = new System.Windows.Forms.MenuItem();
			this.mnuItemsTruncatedRolloutTicketBreakdown = new System.Windows.Forms.MenuItem();
			this.tabMain.SuspendLayout();
			this.tabTesting.SuspendLayout();
			this.tabRollout.SuspendLayout();
			this.tablList.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.menuItem1,
																					this.menuItem2,
																					this.menuItem3,
																					this.menuItem4,
																					this.mnuItemCodePromotion,
																					this.menuItem15,
																					this.menuItem5,
																					this.mnuConnections,
																					this.menuItem7});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "&File";
			this.menuItem1.Visible = false;
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "&Edit";
			this.menuItem2.Visible = false;
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "E&Mail";
			this.menuItem3.Visible = false;
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem8,
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.mnuItemStartEffort});
			this.menuItem4.Text = "&Start";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 0;
			this.menuItem8.Text = "&Notepad";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 1;
			this.menuItem9.Text = "&Calculator";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 2;
			this.menuItem10.Text = "&Training Docs";
			this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
			// 
			// mnuItemStartEffort
			// 
			this.mnuItemStartEffort.Enabled = false;
			this.mnuItemStartEffort.Index = 3;
			this.mnuItemStartEffort.Text = "&Load effort in browser";
			this.mnuItemStartEffort.Click += new System.EventHandler(this.mnuItemStartEffort_Click);
			// 
			// mnuItemCodePromotion
			// 
			this.mnuItemCodePromotion.Enabled = false;
			this.mnuItemCodePromotion.Index = 4;
			this.mnuItemCodePromotion.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuItem12,
																								 this.menuItem17,
																								 this.menuItem18,
																								 this.mnuItemGenCodeRoller});
			this.mnuItemCodePromotion.Text = "&Code Promotion";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 0;
			this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem14,
																					   this.menuItem16});
			this.menuItem12.Text = "&Production Roll";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 0;
			this.menuItem14.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuItemIBM_PROD_PP_OS,
																					   this.mnuItemIBM_PROD_MAX,
																					   this.mnuItemIBM_PROD_Reptool});
			this.menuItem14.Text = "&IBM";
			// 
			// mnuItemIBM_PROD_PP_OS
			// 
			this.mnuItemIBM_PROD_PP_OS.Index = 0;
			this.mnuItemIBM_PROD_PP_OS.Text = "&Personal Pages && Order Status";
			this.mnuItemIBM_PROD_PP_OS.Click += new System.EventHandler(this.mnuItemIBM_PROD_PP_OS_Click);
			// 
			// mnuItemIBM_PROD_MAX
			// 
			this.mnuItemIBM_PROD_MAX.Index = 1;
			this.mnuItemIBM_PROD_MAX.Text = "&Max";
			// 
			// mnuItemIBM_PROD_Reptool
			// 
			this.mnuItemIBM_PROD_Reptool.Index = 2;
			this.mnuItemIBM_PROD_Reptool.Text = "&RepTool";
			this.mnuItemIBM_PROD_Reptool.Click += new System.EventHandler(this.mnuItemIBM_PROD_Reptool_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 1;
			this.menuItem16.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuItemLenovo_Prod_PP_OS,
																					   this.mnuItemLenovo_Prod_Max,
																					   this.mnuItemLenovo_Prod_Reptool});
			this.menuItem16.Text = "&Lenovo";
			// 
			// mnuItemLenovo_Prod_PP_OS
			// 
			this.mnuItemLenovo_Prod_PP_OS.Index = 0;
			this.mnuItemLenovo_Prod_PP_OS.Text = "&Personal Pages && Order Status";
			this.mnuItemLenovo_Prod_PP_OS.Click += new System.EventHandler(this.mnuItemLenovo_Prod_PP_OS_Click);
			// 
			// mnuItemLenovo_Prod_Max
			// 
			this.mnuItemLenovo_Prod_Max.Index = 1;
			this.mnuItemLenovo_Prod_Max.Text = "&Max";
			// 
			// mnuItemLenovo_Prod_Reptool
			// 
			this.mnuItemLenovo_Prod_Reptool.Index = 2;
			this.mnuItemLenovo_Prod_Reptool.Text = "&Reptool";
			this.mnuItemLenovo_Prod_Reptool.Click += new System.EventHandler(this.mnuItemLenovo_Prod_Reptool_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem11,
																					   this.menuItem13});
			this.menuItem17.Text = "&UAT Roll";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 0;
			this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuItemIBM_UAT_PP_OS,
																					   this.mnuItemIBM_UAT_REP,
																					   this.mnuItemIBM_UAT_MAX});
			this.menuItem11.Text = "&IBM";
			// 
			// mnuItemIBM_UAT_PP_OS
			// 
			this.mnuItemIBM_UAT_PP_OS.Index = 0;
			this.mnuItemIBM_UAT_PP_OS.Text = "&Personal Pages && Order Status";
			this.mnuItemIBM_UAT_PP_OS.Click += new System.EventHandler(this.mnuItemIBM_UAT_PP_OS_Click);
			// 
			// mnuItemIBM_UAT_REP
			// 
			this.mnuItemIBM_UAT_REP.Index = 1;
			this.mnuItemIBM_UAT_REP.Text = "&Reptool";
			this.mnuItemIBM_UAT_REP.Click += new System.EventHandler(this.mnuItemIBM_UAT_REP_Click);
			// 
			// mnuItemIBM_UAT_MAX
			// 
			this.mnuItemIBM_UAT_MAX.Index = 2;
			this.mnuItemIBM_UAT_MAX.Text = "&Max";
			this.mnuItemIBM_UAT_MAX.Click += new System.EventHandler(this.mnuItemIBM_UAT_MAX_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 1;
			this.menuItem13.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuItemLenovo_UAT_PP_OS,
																					   this.mnuItemLenovo_UAT_REP,
																					   this.mnuItemLenovo_UAT_MAX});
			this.menuItem13.Text = "&Lenovo";
			// 
			// mnuItemLenovo_UAT_PP_OS
			// 
			this.mnuItemLenovo_UAT_PP_OS.Index = 0;
			this.mnuItemLenovo_UAT_PP_OS.Text = "&Personal Pages && Order Status";
			this.mnuItemLenovo_UAT_PP_OS.Click += new System.EventHandler(this.mnuItemLenovo_UAT_PP_OS_Click);
			// 
			// mnuItemLenovo_UAT_REP
			// 
			this.mnuItemLenovo_UAT_REP.Index = 1;
			this.mnuItemLenovo_UAT_REP.Text = "&Reptool";
			this.mnuItemLenovo_UAT_REP.Click += new System.EventHandler(this.mnuItemLenovo_UAT_REP_Click);
			// 
			// mnuItemLenovo_UAT_MAX
			// 
			this.mnuItemLenovo_UAT_MAX.Index = 2;
			this.mnuItemLenovo_UAT_MAX.Text = "&Max";
			this.mnuItemLenovo_UAT_MAX.Click += new System.EventHandler(this.mnuItemLenovo_UAT_MAX_Click);
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 2;
			this.menuItem18.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem19});
			this.menuItem18.Text = "&Merge";
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 0;
			this.menuItem19.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuItemIBM_Merge_Max});
			this.menuItem19.Text = "&IBM";
			// 
			// mnuItemIBM_Merge_Max
			// 
			this.mnuItemIBM_Merge_Max.Index = 0;
			this.mnuItemIBM_Merge_Max.Text = "&Max";
			this.mnuItemIBM_Merge_Max.Click += new System.EventHandler(this.mnuItemIBM_Merge_Max_Click);
			// 
			// mnuItemGenCodeRoller
			// 
			this.mnuItemGenCodeRoller.Index = 3;
			this.mnuItemGenCodeRoller.Text = "&General Code Roller";
			this.mnuItemGenCodeRoller.Click += new System.EventHandler(this.mnuItemGenCodeRoller_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 5;
			this.menuItem15.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuItemRolloutLogs,
																					   this.mnuItemTimeReport,
																					   this.mnuItemQAPostRollReportCard,
																					   this.mnuItemUnrolledEfforts,
																					   this.mnuItemUnapprovedEfforts,
																					   this.mnuItemScheduledRollouts,
																					   this.mnuItemManagerReports,
																					   this.mnuItemRolloutsAndBranches,
																					   this.mnuItemsDevRollouts,
																					   this.mnuItemRolloutTicketBreakdown,
																					   this.mnuItemsTruncatedRolloutTicketBreakdown});
			this.menuItem15.Text = "&Reports";
			// 
			// mnuItemRolloutLogs
			// 
			this.mnuItemRolloutLogs.Index = 0;
			this.mnuItemRolloutLogs.Text = "&Rollout Logs";
			this.mnuItemRolloutLogs.Click += new System.EventHandler(this.mnuItemRolloutLogs_Click);
			// 
			// mnuItemTimeReport
			// 
			this.mnuItemTimeReport.Index = 1;
			this.mnuItemTimeReport.Text = "&Time Report";
			this.mnuItemTimeReport.Click += new System.EventHandler(this.mnuItemTimeReport_Click);
			// 
			// mnuItemQAPostRollReportCard
			// 
			this.mnuItemQAPostRollReportCard.Enabled = false;
			this.mnuItemQAPostRollReportCard.Index = 2;
			this.mnuItemQAPostRollReportCard.Text = "&QA Post Roll Report Card";
			this.mnuItemQAPostRollReportCard.Click += new System.EventHandler(this.mnuItemQAPostRollReportCard_Click);
			// 
			// mnuItemUnrolledEfforts
			// 
			this.mnuItemUnrolledEfforts.Index = 3;
			this.mnuItemUnrolledEfforts.Text = "&Unrolled Efforts";
			this.mnuItemUnrolledEfforts.Click += new System.EventHandler(this.mnuItemUnrolledEfforts_Click);
			// 
			// mnuItemUnapprovedEfforts
			// 
			this.mnuItemUnapprovedEfforts.Index = 4;
			this.mnuItemUnapprovedEfforts.Text = "Un&approved Efforts";
			this.mnuItemUnapprovedEfforts.Click += new System.EventHandler(this.mnuItemUnapprovedEfforts_Click);
			// 
			// mnuItemScheduledRollouts
			// 
			this.mnuItemScheduledRollouts.Index = 5;
			this.mnuItemScheduledRollouts.Text = "&Scheduled Rollouts";
			this.mnuItemScheduledRollouts.Click += new System.EventHandler(this.mnuItemScheduledRollouts_Click);
			// 
			// mnuItemManagerReports
			// 
			this.mnuItemManagerReports.Index = 6;
			this.mnuItemManagerReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								  this.mnuItemEmployeeTime});
			this.mnuItemManagerReports.Text = "&Manager Reports";
			this.mnuItemManagerReports.Visible = false;
			// 
			// mnuItemEmployeeTime
			// 
			this.mnuItemEmployeeTime.Index = 0;
			this.mnuItemEmployeeTime.Text = "&Employee Time";
			this.mnuItemEmployeeTime.Click += new System.EventHandler(this.mnuItemEmployeeTime_Click);
			// 
			// mnuItemRolloutsAndBranches
			// 
			this.mnuItemRolloutsAndBranches.Index = 7;
			this.mnuItemRolloutsAndBranches.Text = "Rollouts And Branches";
			this.mnuItemRolloutsAndBranches.Click += new System.EventHandler(this.mnuItemRolloutsAndBranches_Click);
			// 
			// mnuItemsDevRollouts
			// 
			this.mnuItemsDevRollouts.Index = 8;
			this.mnuItemsDevRollouts.Text = "Developer Rollouts";
			this.mnuItemsDevRollouts.Click += new System.EventHandler(this.mnuItemsDevRollouts_Click);
			// 
			// mnuItemRolloutTicketBreakdown
			// 
			this.mnuItemRolloutTicketBreakdown.Index = 9;
			this.mnuItemRolloutTicketBreakdown.Text = "Rollout Ticket Breakdown";
			this.mnuItemRolloutTicketBreakdown.Click += new System.EventHandler(this.mnuItemRolloutTicketBreakdown_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 6;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuItemTimeRollout,
																					  this.mnuItemTimeEffort,
																					  this.mnuItemTimersCurrentlyRunning});
			this.menuItem5.Text = "&Timing";
			// 
			// mnuItemTimeRollout
			// 
			this.mnuItemTimeRollout.Enabled = false;
			this.mnuItemTimeRollout.Index = 0;
			this.mnuItemTimeRollout.Text = "Edit time for current rollout";
			this.mnuItemTimeRollout.Click += new System.EventHandler(this.mnuItemTimeRollout_Click);
			// 
			// mnuItemTimeEffort
			// 
			this.mnuItemTimeEffort.Enabled = false;
			this.mnuItemTimeEffort.Index = 1;
			this.mnuItemTimeEffort.Text = "Edit time for current effort";
			this.mnuItemTimeEffort.Click += new System.EventHandler(this.mnuItemTimeEffort_Click);
			// 
			// mnuItemTimersCurrentlyRunning
			// 
			this.mnuItemTimersCurrentlyRunning.Index = 2;
			this.mnuItemTimersCurrentlyRunning.Text = "&Timers currently running";
			this.mnuItemTimersCurrentlyRunning.Click += new System.EventHandler(this.mnuItemTimersCurrentlyRunning_Click);
			// 
			// mnuConnections
			// 
			this.mnuConnections.Index = 7;
			this.mnuConnections.Text = "&Connections";
			this.mnuConnections.Visible = false;
			this.mnuConnections.Click += new System.EventHandler(this.mnuConnections_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 8;
			this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuItemAbout});
			this.menuItem7.Text = "&Help";
			// 
			// mnuItemAbout
			// 
			this.mnuItemAbout.Index = 0;
			this.mnuItemAbout.Text = "&About";
			this.mnuItemAbout.Click += new System.EventHandler(this.mnuItemAbout_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabTesting);
			this.tabMain.Controls.Add(this.tabRollout);
			this.tabMain.Controls.Add(this.tablList);
			this.tabMain.Location = new System.Drawing.Point(16, 16);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(992, 696);
			this.tabMain.TabIndex = 3;
			this.tabMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabMain_MouseUp);
			// 
			// tabTesting
			// 
			this.tabTesting.Controls.Add(this.btnEffortData);
			this.tabTesting.Controls.Add(this.txtEffDesc);
			this.tabTesting.Controls.Add(this.label1);
			this.tabTesting.Controls.Add(this.lvTestDefectNotes);
			this.tabTesting.Controls.Add(this.lvTestNotes);
			this.tabTesting.Controls.Add(this.chkApproved);
			this.tabTesting.Controls.Add(this.btnTestTimer);
			this.tabTesting.Controls.Add(this.txtTestROEffort);
			this.tabTesting.Controls.Add(this.txtEffortId);
			this.tabTesting.Controls.Add(this.btnSearchTestingEffort);
			this.tabTesting.Controls.Add(this.label6);
			this.tabTesting.Location = new System.Drawing.Point(4, 22);
			this.tabTesting.Name = "tabTesting";
			this.tabTesting.Size = new System.Drawing.Size(984, 670);
			this.tabTesting.TabIndex = 0;
			this.tabTesting.Text = "Testing";
			// 
			// btnEffortData
			// 
			this.btnEffortData.Enabled = false;
			this.btnEffortData.Location = new System.Drawing.Point(688, 16);
			this.btnEffortData.Name = "btnEffortData";
			this.btnEffortData.Size = new System.Drawing.Size(75, 24);
			this.btnEffortData.TabIndex = 3;
			this.btnEffortData.Text = "&Effort Data";
			this.btnEffortData.Click += new System.EventHandler(this.btnEffortData_Click);
			// 
			// txtEffDesc
			// 
			this.txtEffDesc.Enabled = false;
			this.txtEffDesc.Location = new System.Drawing.Point(344, 16);
			this.txtEffDesc.Name = "txtEffDesc";
			this.txtEffDesc.Size = new System.Drawing.Size(328, 20);
			this.txtEffDesc.TabIndex = 2;
			this.txtEffDesc.Text = "";
			this.txtEffDesc.Leave += new System.EventHandler(this.txtEffDesc_Leave);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(280, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 24);
			this.label1.TabIndex = 17;
			this.label1.Text = "Description";
			// 
			// lvTestDefectNotes
			// 
			this.lvTestDefectNotes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.Defects,
																								this.columnHeader1});
			this.lvTestDefectNotes.GridLines = true;
			this.lvTestDefectNotes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvTestDefectNotes.Location = new System.Drawing.Point(280, 368);
			this.lvTestDefectNotes.Name = "lvTestDefectNotes";
			this.lvTestDefectNotes.Size = new System.Drawing.Size(688, 272);
			this.lvTestDefectNotes.TabIndex = 7;
			this.lvTestDefectNotes.View = System.Windows.Forms.View.Details;
			this.lvTestDefectNotes.DoubleClick += new System.EventHandler(this.lvTestDefectNotes_DoubleClick);
			this.lvTestDefectNotes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvTestDefectNotes_MouseUp);
			// 
			// Defects
			// 
			this.Defects.Text = "Defects";
			this.Defects.Width = 688;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Effort";
			// 
			// lvTestNotes
			// 
			this.lvTestNotes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.col});
			this.lvTestNotes.GridLines = true;
			this.lvTestNotes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvTestNotes.Location = new System.Drawing.Point(280, 56);
			this.lvTestNotes.Name = "lvTestNotes";
			this.lvTestNotes.Size = new System.Drawing.Size(688, 288);
			this.lvTestNotes.TabIndex = 6;
			this.lvTestNotes.View = System.Windows.Forms.View.Details;
			this.lvTestNotes.DoubleClick += new System.EventHandler(this.lvTestNotes_DoubleClick);
			this.lvTestNotes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvTestNotes_MouseUp);
			// 
			// col
			// 
			this.col.Text = "Notes";
			this.col.Width = 688;
			// 
			// chkApproved
			// 
			this.chkApproved.Enabled = false;
			this.chkApproved.Location = new System.Drawing.Point(776, 16);
			this.chkApproved.Name = "chkApproved";
			this.chkApproved.Size = new System.Drawing.Size(80, 24);
			this.chkApproved.TabIndex = 4;
			this.chkApproved.Text = "&Approved";
			this.chkApproved.CheckedChanged += new System.EventHandler(this.chkApproved_CheckedChanged);
			// 
			// btnTestTimer
			// 
			this.btnTestTimer.Enabled = false;
			this.btnTestTimer.Image = ((System.Drawing.Image)(resources.GetObject("btnTestTimer.Image")));
			this.btnTestTimer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnTestTimer.Location = new System.Drawing.Point(864, 16);
			this.btnTestTimer.Name = "btnTestTimer";
			this.btnTestTimer.Size = new System.Drawing.Size(104, 24);
			this.btnTestTimer.TabIndex = 5;
			this.btnTestTimer.Text = "Start &Timer";
			this.btnTestTimer.Click += new System.EventHandler(this.btnTestTimer_Click);
			// 
			// txtTestROEffort
			// 
			this.txtTestROEffort.BackColor = System.Drawing.SystemColors.Control;
			this.txtTestROEffort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTestROEffort.Location = new System.Drawing.Point(24, 56);
			this.txtTestROEffort.Multiline = true;
			this.txtTestROEffort.Name = "txtTestROEffort";
			this.txtTestROEffort.ReadOnly = true;
			this.txtTestROEffort.Size = new System.Drawing.Size(240, 576);
			this.txtTestROEffort.TabIndex = 9;
			this.txtTestROEffort.Text = "";
			// 
			// txtEffortId
			// 
			this.txtEffortId.Location = new System.Drawing.Point(72, 16);
			this.txtEffortId.Name = "txtEffortId";
			this.txtEffortId.TabIndex = 0;
			this.txtEffortId.Text = "";
			this.txtEffortId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEffortId_KeyDown);
			// 
			// btnSearchTestingEffort
			// 
			this.btnSearchTestingEffort.Location = new System.Drawing.Point(184, 16);
			this.btnSearchTestingEffort.Name = "btnSearchTestingEffort";
			this.btnSearchTestingEffort.Size = new System.Drawing.Size(75, 24);
			this.btnSearchTestingEffort.TabIndex = 1;
			this.btnSearchTestingEffort.Text = "Search";
			this.btnSearchTestingEffort.Click += new System.EventHandler(this.btnSearchTestingEffort_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 24);
			this.label6.TabIndex = 7;
			this.label6.Text = "Enter Effort";
			// 
			// tabRollout
			// 
			this.tabRollout.Controls.Add(this.btnFindRollByScheduledRollDate);
			this.tabRollout.Controls.Add(this.btnCompleteRoll);
			this.tabRollout.Controls.Add(this.lblRolled);
			this.tabRollout.Controls.Add(this.btnFindRollByRollDate);
			this.tabRollout.Controls.Add(this.dtpRoll);
			this.tabRollout.Controls.Add(this.cboClients);
			this.tabRollout.Controls.Add(this.lvRollDefectNotes);
			this.tabRollout.Controls.Add(this.lvRollNotes);
			this.tabRollout.Controls.Add(this.btnRollBackAllEffs);
			this.tabRollout.Controls.Add(this.btnRollBackEff);
			this.tabRollout.Controls.Add(this.btnRollAllEffs);
			this.tabRollout.Controls.Add(this.btnRollEff);
			this.tabRollout.Controls.Add(this.lblUnrolledEfforts);
			this.tabRollout.Controls.Add(this.lblRolledEfforts);
			this.tabRollout.Controls.Add(this.lstRolled);
			this.tabRollout.Controls.Add(this.lstUnrolled);
			this.tabRollout.Controls.Add(this.btnRollTimer);
			this.tabRollout.Location = new System.Drawing.Point(4, 22);
			this.tabRollout.Name = "tabRollout";
			this.tabRollout.Size = new System.Drawing.Size(984, 670);
			this.tabRollout.TabIndex = 1;
			this.tabRollout.Text = "Rollout";
			// 
			// btnFindRollByScheduledRollDate
			// 
			this.btnFindRollByScheduledRollDate.Enabled = false;
			this.btnFindRollByScheduledRollDate.Location = new System.Drawing.Point(272, 16);
			this.btnFindRollByScheduledRollDate.Name = "btnFindRollByScheduledRollDate";
			this.btnFindRollByScheduledRollDate.Size = new System.Drawing.Size(152, 23);
			this.btnFindRollByScheduledRollDate.TabIndex = 2;
			this.btnFindRollByScheduledRollDate.Text = "S&earch by Scheduled Data";
			this.btnFindRollByScheduledRollDate.Click += new System.EventHandler(this.btnFindRollByScheduledRollDate_Click);
			// 
			// btnCompleteRoll
			// 
			this.btnCompleteRoll.Enabled = false;
			this.btnCompleteRoll.Location = new System.Drawing.Point(720, 16);
			this.btnCompleteRoll.Name = "btnCompleteRoll";
			this.btnCompleteRoll.Size = new System.Drawing.Size(120, 23);
			this.btnCompleteRoll.TabIndex = 4;
			this.btnCompleteRoll.Text = "C&omplete Roll Form";
			this.btnCompleteRoll.Click += new System.EventHandler(this.btnCompleteRoll_Click);
			// 
			// lblRolled
			// 
			this.lblRolled.Location = new System.Drawing.Point(576, 16);
			this.lblRolled.Name = "lblRolled";
			this.lblRolled.Size = new System.Drawing.Size(128, 23);
			this.lblRolled.TabIndex = 43;
			// 
			// btnFindRollByRollDate
			// 
			this.btnFindRollByRollDate.Enabled = false;
			this.btnFindRollByRollDate.Location = new System.Drawing.Point(432, 16);
			this.btnFindRollByRollDate.Name = "btnFindRollByRollDate";
			this.btnFindRollByRollDate.Size = new System.Drawing.Size(120, 23);
			this.btnFindRollByRollDate.TabIndex = 3;
			this.btnFindRollByRollDate.Text = "Search By Roll &Date";
			this.btnFindRollByRollDate.Click += new System.EventHandler(this.btnFindRollByRollDate_Click);
			// 
			// dtpRoll
			// 
			this.dtpRoll.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpRoll.Location = new System.Drawing.Point(184, 16);
			this.dtpRoll.Name = "dtpRoll";
			this.dtpRoll.Size = new System.Drawing.Size(88, 20);
			this.dtpRoll.TabIndex = 1;
			// 
			// cboClients
			// 
			this.cboClients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboClients.Location = new System.Drawing.Point(8, 16);
			this.cboClients.Name = "cboClients";
			this.cboClients.Size = new System.Drawing.Size(168, 21);
			this.cboClients.TabIndex = 0;
			this.cboClients.SelectedIndexChanged += new System.EventHandler(this.cboClients_SelectedIndexChanged);
			// 
			// lvRollDefectNotes
			// 
			this.lvRollDefectNotes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.colRollDefects,
																								this.columnHeader2});
			this.lvRollDefectNotes.GridLines = true;
			this.lvRollDefectNotes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvRollDefectNotes.Location = new System.Drawing.Point(8, 256);
			this.lvRollDefectNotes.Name = "lvRollDefectNotes";
			this.lvRollDefectNotes.Size = new System.Drawing.Size(952, 168);
			this.lvRollDefectNotes.TabIndex = 7;
			this.lvRollDefectNotes.View = System.Windows.Forms.View.Details;
			this.lvRollDefectNotes.DoubleClick += new System.EventHandler(this.lvRollDefectNotes_DoubleClick);
			this.lvRollDefectNotes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvRollDefectNotes_MouseUp);
			// 
			// colRollDefects
			// 
			this.colRollDefects.Text = "Defects";
			this.colRollDefects.Width = 792;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Effort";
			this.columnHeader2.Width = 155;
			// 
			// lvRollNotes
			// 
			this.lvRollNotes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.notes});
			this.lvRollNotes.GridLines = true;
			this.lvRollNotes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvRollNotes.Location = new System.Drawing.Point(8, 48);
			this.lvRollNotes.Name = "lvRollNotes";
			this.lvRollNotes.Size = new System.Drawing.Size(952, 176);
			this.lvRollNotes.TabIndex = 6;
			this.lvRollNotes.View = System.Windows.Forms.View.Details;
			this.lvRollNotes.DoubleClick += new System.EventHandler(this.lvRollNotes_DoubleClick);
			this.lvRollNotes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvRollNotes_MouseUp);
			// 
			// notes
			// 
			this.notes.Text = "Notes";
			this.notes.Width = 948;
			// 
			// btnRollBackAllEffs
			// 
			this.btnRollBackAllEffs.Location = new System.Drawing.Point(448, 560);
			this.btnRollBackAllEffs.Name = "btnRollBackAllEffs";
			this.btnRollBackAllEffs.TabIndex = 12;
			this.btnRollBackAllEffs.Text = "<<";
			this.btnRollBackAllEffs.Click += new System.EventHandler(this.btnRollBackAllEffs_Click);
			// 
			// btnRollBackEff
			// 
			this.btnRollBackEff.Location = new System.Drawing.Point(448, 528);
			this.btnRollBackEff.Name = "btnRollBackEff";
			this.btnRollBackEff.TabIndex = 11;
			this.btnRollBackEff.Text = "<";
			this.btnRollBackEff.Click += new System.EventHandler(this.btnRollBackEff_Click);
			// 
			// btnRollAllEffs
			// 
			this.btnRollAllEffs.Location = new System.Drawing.Point(448, 496);
			this.btnRollAllEffs.Name = "btnRollAllEffs";
			this.btnRollAllEffs.TabIndex = 10;
			this.btnRollAllEffs.Text = ">>";
			this.btnRollAllEffs.Click += new System.EventHandler(this.btnRollAllEffs_Click);
			// 
			// btnRollEff
			// 
			this.btnRollEff.Location = new System.Drawing.Point(448, 464);
			this.btnRollEff.Name = "btnRollEff";
			this.btnRollEff.TabIndex = 9;
			this.btnRollEff.Text = ">";
			this.btnRollEff.Click += new System.EventHandler(this.btnRollEff_Click);
			// 
			// lblUnrolledEfforts
			// 
			this.lblUnrolledEfforts.Location = new System.Drawing.Point(16, 440);
			this.lblUnrolledEfforts.Name = "lblUnrolledEfforts";
			this.lblUnrolledEfforts.Size = new System.Drawing.Size(408, 24);
			this.lblUnrolledEfforts.TabIndex = 33;
			this.lblUnrolledEfforts.Text = "Unrolled";
			// 
			// lblRolledEfforts
			// 
			this.lblRolledEfforts.Location = new System.Drawing.Point(544, 440);
			this.lblRolledEfforts.Name = "lblRolledEfforts";
			this.lblRolledEfforts.Size = new System.Drawing.Size(408, 24);
			this.lblRolledEfforts.TabIndex = 32;
			this.lblRolledEfforts.Text = "Rolled";
			// 
			// lstRolled
			// 
			this.lstRolled.DisplayMember = "ExternalId_Desc";
			this.lstRolled.Location = new System.Drawing.Point(544, 464);
			this.lstRolled.Name = "lstRolled";
			this.lstRolled.Size = new System.Drawing.Size(416, 160);
			this.lstRolled.Sorted = true;
			this.lstRolled.TabIndex = 13;
			this.lstRolled.DoubleClick += new System.EventHandler(this.lstRolled_DoubleClick);
			// 
			// lstUnrolled
			// 
			this.lstUnrolled.DisplayMember = "ExternalId_Desc";
			this.lstUnrolled.Location = new System.Drawing.Point(8, 464);
			this.lstUnrolled.Name = "lstUnrolled";
			this.lstUnrolled.Size = new System.Drawing.Size(424, 160);
			this.lstUnrolled.Sorted = true;
			this.lstUnrolled.TabIndex = 8;
			this.lstUnrolled.DoubleClick += new System.EventHandler(this.lstUnrolled_DoubleClick);
			this.lstUnrolled.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstUnrolled_MouseUp);
			// 
			// btnRollTimer
			// 
			this.btnRollTimer.Enabled = false;
			this.btnRollTimer.Image = ((System.Drawing.Image)(resources.GetObject("btnRollTimer.Image")));
			this.btnRollTimer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRollTimer.Location = new System.Drawing.Point(856, 16);
			this.btnRollTimer.Name = "btnRollTimer";
			this.btnRollTimer.Size = new System.Drawing.Size(104, 23);
			this.btnRollTimer.TabIndex = 5;
			this.btnRollTimer.Text = "Start &Timer";
			this.btnRollTimer.Click += new System.EventHandler(this.btnRollTimer_Click);
			// 
			// tablList
			// 
			this.tablList.Controls.Add(this.txtListPath);
			this.tablList.Controls.Add(this.bntAddKVP);
			this.tablList.Controls.Add(this.btnDeleteKVP);
			this.tablList.Controls.Add(this.lvKVP);
			this.tablList.Controls.Add(this.cboCatLists);
			this.tablList.Controls.Add(this.tvwHier);
			this.tablList.Location = new System.Drawing.Point(4, 22);
			this.tablList.Name = "tablList";
			this.tablList.Size = new System.Drawing.Size(984, 670);
			this.tablList.TabIndex = 3;
			this.tablList.Text = "List";
			// 
			// txtListPath
			// 
			this.txtListPath.BackColor = System.Drawing.SystemColors.InactiveBorder;
			this.txtListPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtListPath.Location = new System.Drawing.Point(312, 40);
			this.txtListPath.Name = "txtListPath";
			this.txtListPath.Size = new System.Drawing.Size(648, 13);
			this.txtListPath.TabIndex = 6;
			this.txtListPath.Text = "";
			// 
			// bntAddKVP
			// 
			this.bntAddKVP.Location = new System.Drawing.Point(816, 624);
			this.bntAddKVP.Name = "bntAddKVP";
			this.bntAddKVP.TabIndex = 5;
			this.bntAddKVP.Text = "&Add";
			// 
			// btnDeleteKVP
			// 
			this.btnDeleteKVP.Location = new System.Drawing.Point(896, 624);
			this.btnDeleteKVP.Name = "btnDeleteKVP";
			this.btnDeleteKVP.TabIndex = 4;
			this.btnDeleteKVP.Text = "&Delete";
			this.btnDeleteKVP.Click += new System.EventHandler(this.btnDeleteKVP_Click);
			// 
			// lvKVP
			// 
			this.lvKVP.AllowColumnReorder = true;
			this.lvKVP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					this.colheadKey,
																					this.colHeadVal});
			this.lvKVP.FullRowSelect = true;
			this.lvKVP.GridLines = true;
			this.lvKVP.Location = new System.Drawing.Point(312, 72);
			this.lvKVP.MultiSelect = false;
			this.lvKVP.Name = "lvKVP";
			this.lvKVP.Size = new System.Drawing.Size(656, 536);
			this.lvKVP.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvKVP.TabIndex = 3;
			this.lvKVP.View = System.Windows.Forms.View.Details;
			this.lvKVP.Click += new System.EventHandler(this.lvKVP_Click);
			this.lvKVP.DoubleClick += new System.EventHandler(this.lvKVP_DoubleClick);
			// 
			// colheadKey
			// 
			this.colheadKey.Text = "Key";
			this.colheadKey.Width = 221;
			// 
			// colHeadVal
			// 
			this.colHeadVal.Text = "Value";
			this.colHeadVal.Width = 594;
			// 
			// cboCatLists
			// 
			this.cboCatLists.DisplayMember = "ListName";
			this.cboCatLists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboCatLists.Location = new System.Drawing.Point(8, 32);
			this.cboCatLists.Name = "cboCatLists";
			this.cboCatLists.Size = new System.Drawing.Size(288, 21);
			this.cboCatLists.TabIndex = 2;
			this.cboCatLists.SelectedIndexChanged += new System.EventHandler(this.cboCatLists_SelectedIndexChanged);
			// 
			// tvwHier
			// 
			this.tvwHier.ImageIndex = -1;
			this.tvwHier.Location = new System.Drawing.Point(8, 72);
			this.tvwHier.Name = "tvwHier";
			this.tvwHier.SelectedImageIndex = -1;
			this.tvwHier.Size = new System.Drawing.Size(288, 576);
			this.tvwHier.TabIndex = 1;
			this.tvwHier.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvwHier_MouseUp);
			this.tvwHier.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwHier_AfterSelect);
			// 
			// mnuListHier
			// 
			this.mnuListHier.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuItemListHierCatAdd,
																						this.mnuItemListHierEntAdd,
																						this.mnuItemListHierDelete,
																						this.mnuItemListHierRename,
																						this.mnuItemListHierRefresh});
			// 
			// mnuItemListHierCatAdd
			// 
			this.mnuItemListHierCatAdd.Index = 0;
			this.mnuItemListHierCatAdd.Text = "&Add Category";
			this.mnuItemListHierCatAdd.Click += new System.EventHandler(this.mnuItemListHierCatAdd_Click);
			// 
			// mnuItemListHierEntAdd
			// 
			this.mnuItemListHierEntAdd.Index = 1;
			this.mnuItemListHierEntAdd.Text = "Add &Entry";
			this.mnuItemListHierEntAdd.Click += new System.EventHandler(this.mnuItemListHierEntAdd_Click);
			// 
			// mnuItemListHierDelete
			// 
			this.mnuItemListHierDelete.Index = 2;
			this.mnuItemListHierDelete.Text = "&Delete";
			this.mnuItemListHierDelete.Click += new System.EventHandler(this.mnuItemListHierDelete_Click);
			// 
			// mnuItemListHierRename
			// 
			this.mnuItemListHierRename.Index = 3;
			this.mnuItemListHierRename.Text = "Re&name";
			this.mnuItemListHierRename.Click += new System.EventHandler(this.mnuItemListHierRename_Click);
			// 
			// mnuItemListHierRefresh
			// 
			this.mnuItemListHierRefresh.Index = 4;
			this.mnuItemListHierRefresh.Text = "&Refresh List";
			this.mnuItemListHierRefresh.Click += new System.EventHandler(this.mnuItemListHierRefresh_Click);
			// 
			// mnuNotes
			// 
			this.mnuNotes.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuItemNotesAdd,
																					 this.mnuItemNotesDelete});
			// 
			// mnuItemNotesAdd
			// 
			this.mnuItemNotesAdd.Index = 0;
			this.mnuItemNotesAdd.Text = "&Add";
			this.mnuItemNotesAdd.Click += new System.EventHandler(this.mnuItemNotesAdd_Click);
			// 
			// mnuItemNotesDelete
			// 
			this.mnuItemNotesDelete.Index = 1;
			this.mnuItemNotesDelete.Text = "&Delete";
			this.mnuItemNotesDelete.Click += new System.EventHandler(this.mnuItemNotesDelete_Click);
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
			// mnuItemsTruncatedRolloutTicketBreakdown
			// 
			this.mnuItemsTruncatedRolloutTicketBreakdown.Index = 10;
			this.mnuItemsTruncatedRolloutTicketBreakdown.Text = "&Truncated Rollout Ticket Breakdown";
			this.mnuItemsTruncatedRolloutTicketBreakdown.Click += new System.EventHandler(this.mnuItemsTruncatedRolloutTicketBreakdown_Click);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 701);
			this.Controls.Add(this.tabMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mnuMain;
			this.Name = "frmMain";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.tabMain.ResumeLayout(false);
			this.tabTesting.ResumeLayout(false);
			this.tabRollout.ResumeLayout(false);
			this.tablList.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new frmMain());
		}
		
		#region Login
		private void frmMain_Load(object sender, System.EventArgs e) {
			try{
				Login dlg = new Login();
				DialogResult res;
				res = dlg.ShowDialog(this);
				if (res == DialogResult.Cancel) {this.Close(); return;}
				if (UI.DemandClientVerMatchesDBVer(this) == false){
					this.Close(); return;
				}
				this.Text = "QED " + UI.QED_CLIENT_ID;
				this.cboClients.DisplayMember = "Name";
				foreach(Client cl in Clients.Inst) {
					if (!cl.Retired)
						this.cboClients.Items.Add(cl);
				}
				if (!_tvwHierWasLoaded){
					this.Load_tvwHier();
				}
				if (Thread.CurrentPrincipal.IsInRole("Admin")){
					this.mnuConnections.Visible = true;
					this.mnuItemCodePromotion.Enabled = true;
				}
				if (Thread.CurrentPrincipal.IsInRole("Manager")){
					mnuItemManagerReports.Visible = true;
				}

			}
			catch (ThreadAbortException ex){
				System.Diagnostics.Debug.Write(ex.Message);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}
		private void DoLogout() {
			Thread.CurrentPrincipal = 
				new GenericPrincipal(	new GenericIdentity(""), new string[] {});
		}
		#endregion

		#region List
		private void Load_tvwHier() {
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			CatList pubList;
			if (!cls.Contains("Public") )
				pubList = cls.AddNew("Public");
			else
				pubList = cls.item("Public");
			
			if (Thread.CurrentPrincipal.IsInRole("admin"))
			{
				if (!cls.Contains("System") )
					cls.AddNew("System");
			}
			_pubList = pubList;
			_systemList = cls.item("System");
			
			this.cboCatLists.Items.Clear();
			foreach(CatList cl in cls){
					this.cboCatLists.Items.Add(cl);
			}
			if (!Thread.CurrentPrincipal.IsInRole("admin")){
				this.cboCatLists.Items.Remove(cls.item("System"));
			}
			this.cboCatLists.SelectedItem = pubList;
			this.UpdateList();
		}
		private void UpdateList() {
			this.tvwHier.Nodes.Clear();
			this.lvKVP.Items.Clear();
			foreach (Entry ent in _pubList.RootEntries) {
				this.LoadEntry(ent, null);
			}
			_tvwHierWasLoaded = true;
			this.tvwHier.ExpandAll();
		}
		private void LoadEntry(Entry ent, TreeNode lastTn){
			TreeNode tn = new TreeNode();
			if ( !ent.IsLeaf){
				tn = new TreeNode(ent.Key);
				tn.Tag = ent;
				if (ent.IsRoot){
					tvwHier.Nodes.Add(tn);
				}else{
					lastTn.Nodes.Add(tn);
				}
				foreach(Entry childEnt in ent.Entries){
					this.LoadEntry(childEnt, tn);
				}
				tn = tn.Parent;
			}
		}

		private void cboCatLists_SelectedIndexChanged(object sender, System.EventArgs e) {
			_pubList = (CatList)cboCatLists.SelectedItem;
			this.UpdateList();
		}	
		private void tvwHier_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (e.Button == System.Windows.Forms.MouseButtons.Right) {
				Point selectedPoint = new Point(e.X, e.Y);
				TreeNode tn = tvwHier.GetNodeAt(selectedPoint);
				if (tn != null) {
					tvwHier.SelectedNode = tn;
					mnuItemListHierDelete.Enabled = true;
					mnuItemListHierRefresh.Enabled = true;
					mnuItemListHierRename.Enabled = true;
					mnuItemListHierCatAdd.Enabled = true;
					mnuItemListHierEntAdd.Enabled = true;
				}else{
					tvwHier.SelectedNode = null;
					mnuItemListHierDelete.Enabled = false;
					mnuItemListHierCatAdd.Enabled = true;
					mnuItemListHierRefresh.Enabled = true;
					mnuItemListHierRename.Enabled = false;
					mnuItemListHierEntAdd.Enabled = false;
				}
				mnuListHier.Show(tvwHier, selectedPoint);
			}
		}

		private void mnuItemListHierCatAdd_Click(object sender, System.EventArgs e) {
			const string CATEGORY_NAME = "Enter Category Name";
			TreeNode tn;
			Entry ent;
			string key;
			if (tvwHier.SelectedNode != null) {
				ent = (JCSLA.Entry)tvwHier.SelectedNode.Tag;
				InputModal im = new InputModal("", CATEGORY_NAME );
				if (im.ShowDialog(this) == DialogResult.OK){
					key = im.Answer(CATEGORY_NAME);
					if (key != ""){
						Entry newEnt = ent.Entries.Add(key);
						if (newEnt.IsValid){
							newEnt.Update();
							tn = new TreeNode(newEnt.Key); tn.Tag = newEnt;
							tvwHier.SelectedNode.Nodes.Add(tn);
						}else{ 
							MessageBox.Show(this, "New entry could not be added because is was invalid:\r\n" + newEnt.BrokenRules.ToString());
						}
					}
				}
			}else {
				InputModal im = new InputModal("", CATEGORY_NAME);
				if (im.ShowDialog(this) == DialogResult.OK){
					key = im.Answer(CATEGORY_NAME);
					if (key.Trim() != ""){
						ent = _pubList.AddRoot(key);
						if (ent.IsValid){
							ent.Update();
							tn = new TreeNode(ent.Key); tn.Tag = ent;
							tvwHier.Nodes.Add(tn);
						}else{
							MessageBox.Show(this, "Entry is invalid\r\n" + ent.BrokenRules);
						}
					}
				}
			}
		}
		private void lvKVP_Click(object sender, System.EventArgs e) {
			if (lvKVP.SelectedItems[0] != null){
			   this.txtListPath.Text = ((Entry)lvKVP.SelectedItems[0].Tag).Path;
			}
		}

		private void mnuItemListHierDelete_Click(object sender, System.EventArgs e) {
			JCSLA.Entry ent = (JCSLA.Entry)tvwHier.SelectedNode.Tag;
			if (MessageBox.Show(this, "Confirm Delete", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes){
				ent.Delete();
				tvwHier.SelectedNode.Remove();
				lvKVP.Items.Clear();
			}
		}

		private void mnuItemListHierRefresh_Click(object sender, System.EventArgs e) {
			this.Load_tvwHier();
		}

		private void mnuItemListHierRename_Click(object sender, System.EventArgs e) {
			const string ENTER_NEW_NAME = "Enter New Name";
			InputModal im = new InputModal("", ENTER_NEW_NAME);
			
			Entry ent = (Entry)tvwHier.SelectedNode.Tag;
			((TextBox)im.AnswerTable[ENTER_NEW_NAME]).Text = ent.Key;
			if (im.ShowDialog(this) == DialogResult.OK){
				string newKey = im.Answer(ENTER_NEW_NAME);
				if (newKey != "") {
					ent.Key = newKey; 
					if (ent.IsValid){
						ent.Update();
						tvwHier.SelectedNode.Text = newKey;
					}else{
						MessageBox.Show(this, "New entry could not be added because is was invalid:\r\n" + ent.BrokenRules.ToString());
					}
				}
			}
		}

		private void tvwHier_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
			lvKVP.Items.Clear();
			Entry ent = (Entry)e.Node.Tag;
			foreach (Entry childEnt in ent.Leafs){
				this.AddEntToKVP(childEnt);
			}
			this.txtListPath.Text = ent.Path;
		}
		private void btnDeleteKVP_Click(object sender, System.EventArgs e) {
			if (MessageBox.Show(this, "Confirm Delete", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
				Entry ent = (Entry)lvKVP.SelectedItems[0].Tag;
				ent.Delete();
				lvKVP.SelectedItems[0].Remove();
			}
		}

		private void mnuItemListHierEntAdd_Click(object sender, System.EventArgs e) {
			AddEntToKVP();
		}
		private void AddEntToKVP(){
			const string KEY= "Enter Key";
			const string VAL = "Enter Value";
			bool beenHere = false;
			Entry ent = (Entry)tvwHier.SelectedNode.Tag;
			DialogResult res;
			InputModal im = new InputModal("", KEY, VAL);			
			do	{
				if (beenHere) {
					MessageBox.Show(this, "All fields required", "Error");
				}else{
				}
				res =  im.ShowDialog(this);
				beenHere = true;
			}
			while (res  == DialogResult.OK && (im.Answer(KEY) == "" || im.Answer(VAL) == ""));
			if (res  == DialogResult.OK){
				Entry newEnt = ent.Entries.Add(im.Answer(KEY), im.Answer(VAL));
				if (newEnt.IsValid){
					newEnt.Update();
					this.AddEntToKVP(newEnt);
				}else{
					MessageBox.Show("Entry can not be saved:\r\n" + newEnt.BrokenRules);
				}
			}
		}
		private void AddEntToKVP(Entry ent) {
			ListViewItem item = new ListViewItem(new string[]{ent.Key, ent.Value});
			item.Tag = ent;
			lvKVP.Items.Add(item);
		}

		private void lvKVP_DoubleClick(object sender, System.EventArgs e) {
			const string KEY= "Enter Key";
			const string VAL = "Enter Value";
			Entry ent = (Entry)lvKVP.SelectedItems[0].Tag;
			InputModal im = new InputModal("Edit Entry", KEY, VAL);
			((TextBox)im.AnswerTable[KEY]).Text = ent.Key;
			((TextBox)im.AnswerTable[VAL]).Text = ent.Value;
			if (im.ShowDialog(this) == DialogResult.OK){ 
				ent.Key = im.Answer(KEY);
				ent.Value = im.Answer(VAL);
				if (ent.IsValid){
					ent.Update();
					lvKVP.SelectedItems[0].SubItems[0].Text = ent.Key;
					lvKVP.SelectedItems[0].SubItems[1].Text = ent.Value;
				}else{
					MessageBox.Show("Entry could not be saved because it was invalid:\r\n" + ent.BrokenRules);
				}
			}
		}
		private void btnSearchTestingEffort_Click(object sender, System.EventArgs e) {
			try{
				bool cancel = false;
				Business.Time time;
				if (_testingEff != null){
					time = _times.item(_testingEff);
					if (time != null){
						PromptTimerStop(time, ref cancel);
					}
				}
				if (!cancel){
					_testingEff = new Effort(this.txtEffortId.Text, true);
					if (_testingEff.IsNew){
						if (_testingEff.IsValid){
							_testingEff.Update();
						}else{
							MessageBox.Show(this, "Can't create local record for effort:\r\n" + _testingEff.BrokenRules);
						}
					}
					UpdateTestingTab();
					time = _times.item(_testingEff);
					if (time == null){
						time = new QED.Business.Time(_testingEff);
						PromptTimerStart(time);
					}else{
						HookUpTimer(time);
					}

				}
			}
			catch(Exception ex) {
				this.chkApproved.Enabled = false;
				txtTestROEffort.Text = "";
				this.lvTestNotes.Items.Clear();
				this.lvTestDefectNotes.Items.Clear();
				this.txtEffDesc.Text = "";
				MessageBox.Show(this, "There was a problem loading effort:\r\n" + ex.Message, "QED");
			}
		}
		#endregion

		#region Effort Testing
		private void UpdateTestingTab() {
			ListViewItem lvi;
			txtTestROEffort.Text = "";
			this.lvTestNotes.Items.Clear();
			this.lvTestDefectNotes.Items.Clear();
			this.txtEffDesc.Text = _testingEff.Desc;
			this.btnEffortData.Enabled =true;
			this.chkApproved.Enabled = true;
			this.txtEffDesc.Enabled = true;
			this.btnTestTimer.Enabled = true;
			this.mnuItemTimeEffort.Enabled = true;
			if (_testingEff.EffortType == EffortType.Ticket){
				txtTestROEffort.Text += "Ticket: " + _testingEff.ConventionalId + "\r\n";
				txtTestROEffort.Text += "Overdue: " +_testingEff.Overdue.ToString() + "\r\n";
				txtTestROEffort.Text += "Creation Date: " + ((_testingEff.CreationDate == DateTime.MinValue) ? ("") : (_testingEff.CreationDate.ToShortDateString() + "\r\n"));
				txtTestROEffort.Text += "Close Date: " + ((_testingEff.CloseDate == DateTime.MinValue) ? "" : _testingEff.CloseDate.ToShortDateString() + "\r\n");
				txtTestROEffort.Text += "Client: " + ((_testingEff.Client == null) ? "" : _testingEff.Client.Name + "\r\n");
			}else{
				txtTestROEffort.Text = "Project: " + _testingEff.ConventionalId + "\r\n";
				txtTestROEffort.Text += "<PMO ACCESS UNAVAILABLE>";
			}
			mnuItemStartEffort.Enabled = true;
			mnuItemStartEffort.Text = "Load " + _testingEff.ConventionalId + " in browser";

			foreach(QED.Business.Message msg in _testingEff.Messages) {
				lvi = new ListViewItem(msg.Text);
				lvi.Tag = msg;
				lvTestNotes.Items.Add(lvi);
			}
			foreach(Defect def in _testingEff.Defects) {
				lvi = new ListViewItem(def.Desc);
				lvi.Tag = def;
				lvTestDefectNotes.Items.Add(lvi);
			}
			this.chkApproved.Checked = _testingEff.Approved;
		}
		private void txtEffortId_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e.KeyCode == Keys.Return) this.btnSearchTestingEffort_Click(new object(), new EventArgs());
		}
		private void txtEffDesc_Leave(object sender, System.EventArgs e) {
			_testingEff.Desc = this.txtEffDesc.Text;
			if (_testingEff.IsValid){
				_testingEff.Update();
			}else{
				MessageBox.Show(this, _testingEff.BrokenRules.ToString(), "Effort Invalid");
			}		
		}
		private void btnEffortData_Click(object sender, System.EventArgs e) {
			EffortData effData = new EffortData(_testingEff);
			effData.ShowDialog(this);
		}

		private void show_mnuItemNotesAdd(ListView lv, Point p, MouseButtons mb) {
			if ((_testingEff != null && (lv == lvTestDefectNotes || lv == lvTestNotes)) ||
				(_rollout != null && (lv == lvRollDefectNotes|| lv == lvRollNotes)))	{
				if (mb == MouseButtons.Right){
					if (lv.SelectedItems.Count > 0){
						mnuItemNotesAdd.Enabled = true;
						mnuItemNotesDelete.Enabled = true;
					}else{
						mnuItemNotesAdd.Enabled = true;
						mnuItemNotesDelete.Enabled = false;
					}
					mnuNotes.Show(lv, p);
				}
			}
		}
		private void lvTestNotes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			this.show_mnuItemNotesAdd(lvTestNotes, new Point(e.X, e.Y), e.Button);
		}
		private void lvTestDefectNotes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			this.show_mnuItemNotesAdd(lvTestDefectNotes, new Point(e.X, e.Y), e.Button);
		}
		private void lvRollNotes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			this.show_mnuItemNotesAdd(lvRollNotes, new Point(e.X, e.Y), e.Button);
		}
		
		private void lvRollDefectNotes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			this.show_mnuItemNotesAdd(lvRollDefectNotes, new Point(e.X, e.Y), e.Button);
		}
		private void mnuItemNotesAdd_Click(object sender, System.EventArgs e) {
			ListView lv = (ListView)this.ActiveControl;
			ListViewItem item = new ListViewItem();
			QED.Business.Message  msg = null;
			InputModal imRollDef = null;
			InputModelLarge im = null;
			const string DEFECT = "Description";
			const string EFFORT = "Enter Effort: ";
			Defect def = null;
			if (lv.Name != "lvRollDefectNotes"){
				im = new InputModelLarge();
			}else{
				if (_rollout.Efforts.Count > 0){
					imRollDef = new InputModal("Enter Defect Info", DEFECT);
					ComboBox cb = new ComboBox();
					cb.Name = EFFORT;
					cb.DropDownStyle = ComboBoxStyle.DropDownList;
					cb.DisplayMember = "ExternalId_Desc";
					foreach(Effort eff in _rollout.Efforts){
						cb.Items.Add(eff);
					}
					imRollDef.AddToPanel(cb);
				}else{
					MessageBox.Show("No efforts have been assigned to this rollout.");
				}
			}

			if( (imRollDef != null && imRollDef.ShowDialog() == DialogResult.OK) || 
				(im != null && im.ShowDialog(this) == DialogResult.OK) ){
				switch (lv.Name) {
					case "lvTestNotes":
						msg = _testingEff.Messages.Add(new QED.Business.Message(im.Answer));
						if (msg.IsValid)
							msg.Update(); 
						else
							MessageBox.Show(this, msg.BrokenRules.ToString());
						item.Text = msg.Text; item.Tag = msg;
						
						break;
					case "lvTestDefectNotes" : 
						def = _testingEff.Defects.Add(new QED.Business.Defect(im.Answer));
						def.ForRoll = false;
						if (def.IsValid)
							def.Update(); 
						else
							MessageBox.Show(this, def.BrokenRules.ToString());
						item.Text = def.Desc; item.Tag = def;
						break;
					case "lvRollNotes" : 
						msg = _rollout.Messages.Add(new QED.Business.Message(im.Answer));
						if (msg.IsValid)
							msg.Update(); 
						else
							MessageBox.Show(this, msg.BrokenRules.ToString());
						item.Text = msg.Text; item.Tag = msg;
						break;
					case "lvRollDefectNotes" : 
						string defectMsg = ((TextBox)imRollDef.AnswerTable[DEFECT]).Text;
						Effort eff = null;
						if (((ComboBox)imRollDef.AnswerTable[EFFORT]).SelectedItem != null)
							eff = ((Effort)((ComboBox)imRollDef.AnswerTable[EFFORT]).SelectedItem);
						if (eff == null || defectMsg.Trim() == ""){
							MessageBox.Show(this, "All values required", "QED");
						}else{
							def = _rollout.Defects.Add(new QED.Business.Defect(defectMsg));
							def.ForRoll = true;
							def.Effort = eff;
							if (def.IsValid)
								def.Update(); 
							else
								MessageBox.Show(this, def.BrokenRules.ToString());
							item.Tag = def;
							item.Text = def.Desc;
							item.SubItems.Add(eff.ExternalId_Desc);
						}
						break;
				}
				if ( (def != null && def.IsValid) || (msg != null && msg.IsValid) )
					lv.Items.Add(item);
			}
		}
		private void lvTestDefectNotes_DoubleClick(object sender, System.EventArgs e) {
			UpdateNotes(lvTestDefectNotes);			
		}

		private void lvRollNotes_DoubleClick(object sender, System.EventArgs e) {
			UpdateNotes(lvRollNotes);			
		}

		private void lvRollDefectNotes_DoubleClick(object sender, System.EventArgs e) {
			ListViewItem item =  lvRollDefectNotes.SelectedItems[0];
			Defect def =  (Defect)item.Tag;
			InputModal im = new InputModal("", "Description");
			ComboBox cb = new ComboBox();
			cb.Name = "Effort";
			cb.DropDownStyle = ComboBoxStyle.DropDownList;
			cb.DisplayMember = "ExternalId_Desc"; 
			im.AddToPanel(cb);
			((TextBox)im.AnswerTable["Description"]).Text = def.Desc;
			foreach (Effort eff in _rollout.Efforts){
				cb.Items.Add(eff);
				if (eff.Id == def.Effort.Id){
					((ComboBox)im.AnswerTable["Effort"]).SelectedItem = eff;
				}
			}
			if (im.ShowDialog() == DialogResult.OK){
				def.Desc = ((TextBox)im.AnswerTable["Description"]).Text;
				def.Effort = ((Effort)((ComboBox)im.AnswerTable["Effort"]).SelectedItem);
			}
			if (UI.UpdateIfValid(this, def)){
				item.SubItems[0].Text = def.Desc;
				item.SubItems[1].Text = def.Effort.ExternalId_Desc;
			}
		}		
		private void lvTestNotes_DoubleClick(object sender, System.EventArgs e) {
				UpdateNotes(lvTestNotes);						
		}
		private void UpdateNotes(ListView lv){
			BusinessBase note =  (BusinessBase)lv.SelectedItems[0].Tag;
			string text;
			if(note != null) {
				if(note is Defect)
					text = ((Defect)note).Desc;
				else
					text = ((Business.Message)note).Text;
				InputModelLarge im = new InputModelLarge(text);
				if (im.ShowDialog(this) == DialogResult.OK){
					if(note is Defect)
						text = ((Defect)note).Desc = im.Answer;
					else
						text = ((Business.Message)note).Text = im.Answer;
					if (note.IsValid){
						note.Update();
						lv.SelectedItems[0].Text = text;
					}else{
						MessageBox.Show(this, note.BrokenRules.ToString(), "QED");
					}
				}
			}
		}
		private void mnuItemNotesDelete_Click(object sender, System.EventArgs e) {
			ListView lv = (ListView)this.ActiveControl;
			ListViewItem item = lv.SelectedItems[0];
			BusinessBase note = (BusinessBase)item.Tag;
			if(MessageBox.Show(this, "Confirm Delete", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes){
				note.Delete();
				lv.Items.Remove(item);
			}
		}

		private void chkApproved_CheckedChanged(object sender, System.EventArgs e) {
			_testingEff.Approved = this.chkApproved.Checked;
			if (!ignoreChkApprovedSave){
				if (_testingEff.IsValid){
					_testingEff.Update();
				}else{
					MessageBox.Show(this, "Can't change approval:\r\n" + _testingEff.BrokenRules, "Effort Invalid");
					ignoreChkApprovedSave = true;
					this.chkApproved.Checked = !this.chkApproved.Checked;
					ignoreChkApprovedSave = false;
				}
			}
		}
		#endregion

		#region Rollout

		private void UpdateRolloutTab() {
			ListViewItem lvi;
			this.btnRollTimer.Enabled = this.btnCompleteRoll.Enabled = true;
			this.mnuItemTimeRollout.Enabled = true;
			this.mnuItemQAPostRollReportCard.Enabled = true;
			Business.Time time = _times.item(_rollout);

			if (_rollout.Rolled){
				lblRolled.ForeColor = Color.Green;
				lblRolled.Text = "ROLLED";
			}else	if (_rollout.RolledBack){
				lblRolled.ForeColor = Color.Red;
				lblRolled.Text = "ROLLED BACK";
			}else{
				lblRolled.ForeColor = Color.Blue;
				lblRolled.Text = "NOT ROLLED";
			}
			lvRollDefectNotes.Items.Clear();
			lvRollNotes.Items.Clear();
			lstRolled.Items.Clear();
			lstUnrolled.Items.Clear();
			
			foreach(QED.Business.Message msg in _rollout.Messages) {
				lvi = new ListViewItem(msg.Text);
				lvi.Tag = msg;
				lvRollNotes.Items.Add(lvi);
			}
			foreach(Defect def in _rollout.Defects) {
				lvi = new ListViewItem(new string[] {def.Desc, def.Effort.ExternalId_Desc});
				lvi.Tag = def;
				lvRollDefectNotes.Items.Add(lvi);
			}
			foreach(Effort eff in _rollout.UnrolledEfforts) {
				lstUnrolled.Items.Add(eff);
			}
			foreach(Effort eff in _rollout.RolledEfforts) {
				lstRolled.Items.Add(eff);
			}
			this.lblUnrolledEfforts.Text = "Unrolled Efforts (count: " + _rollout.UnrolledEfforts.Count.ToString() + ")";
			this.lblRolledEfforts.Text = "Rolled Efforts (count: " + _rollout.RolledEfforts.Count.ToString() + ")";
		}

		private void lstUnrolled_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (_rollout != null) {
				if (e.Button == MouseButtons.Right){
					if (lstUnrolled.SelectedItem != null) {
						mnuItemGenListBoxAdd.Enabled = true;
						mnuItemGenListBoxRemove.Enabled = true;
					}else{
						mnuItemGenListBoxAdd.Enabled = true;
						
						mnuItemGenListBoxRemove.Enabled = false;
					}
					mnuGenListBox.Show(lstUnrolled, new Point(e.X, e.Y));
				}
			}
		}

		private void mnuItemGenListBoxAdd_Click(object sender, System.EventArgs e) {
			try{
				ComboBox cb = new ComboBox();
				cb.Name = "Select Effort";
				cb.DisplayMember = "ExternalId_Desc";
				cb.DropDownStyle = ComboBoxStyle.DropDown;
				Efforts rolloutEfforts = _rollout.Efforts;
				foreach (Effort eff in Efforts.Unrolled()){
					if ( !rolloutEfforts.Contains(eff.Id) &&  eff.Approved && (eff.EffortType == EffortType.Project || (eff.Client.Id == _rollout.Client.Id)))
						cb.Items.Add(eff);
				}
				InputModal im = new InputModal("", cb);
				if (im.ShowDialog(this) == DialogResult.OK) {
					if (cb.Text.Trim() != ""){
						Effort eff = (Effort)cb.SelectedItem;
						if (eff == null) {
							eff = new Effort(cb.Text, true);
							if (rolloutEfforts.Contains(eff.Id)){
								MessageBox.Show(this, "This effort has already been assigned. Can't re-assign", "QED");
								return;
							}
							if (!eff.Approved){
								MessageBox.Show(this, "This effort has not been marked approved yet.", "QED");
								return;
							}
							if (eff.Rolled){
								MessageBox.Show(this, "This effort has been rolled. Can't add.", "QED");
								return;
							}
						}
						_rollout.AddUnrolled(eff);
						if (_rollout.IsValid) {
							_rollout.Update();
							lstUnrolled.Items.Add(eff);
						}else{
							MessageBox.Show(this, _rollout.BrokenRules.ToString(), "Error");
						}
					}
				}
				UpdateRolloutTab();
			}
			catch(Exception ex){
				UI.ShowException(this, ex);
			}
		}
		private void mnuItemGenListBoxRemove_Click(object sender, System.EventArgs e) {
			Effort eff = (Effort)this.lstUnrolled.SelectedItem;
			if (eff != null){
				_rollout.Delete(eff);
				this.lstUnrolled.Items.Remove(this.lstUnrolled.SelectedItem);
			}
			UpdateRolloutTab();
		}

		private void btnRollEff_Click(object sender, System.EventArgs e) {
			MoveEffs(true, false);
		}
		private void btnRollAllEffs_Click(object sender, System.EventArgs e) {
			MoveEffs(true, true);
		}

		private void btnRollBackEff_Click(object sender, System.EventArgs e) {
			MoveEffs(false, false);
		}

		private void btnRollBackAllEffs_Click(object sender, System.EventArgs e) {
			MoveEffs(false, true);
		}
		private void MoveEffs(bool roll, bool all) {
			try{
				ListBox lstFrom, lstTo;
				Effort eff;
				if (roll) {
					lstFrom = lstUnrolled;
					lstTo= lstRolled;
				}else{
					lstFrom = lstRolled;
					lstTo= lstUnrolled;
				}
				if (all) {
					if (lstFrom.Items.Count == 0){
						MessageBox.Show(this, "Nothing to move", "Error");
					}else{
						for (int i=lstFrom.Items.Count-1; i>=0; i--){
							eff = (Effort)lstFrom.Items[i];
							if (roll)
								_rollout.Roll(eff);
							else
								_rollout.Unroll(eff);
							MoveEff(eff, lstFrom, lstTo);
						}
					}
				}else{
					if (lstFrom.SelectedItem != null){
						eff = (Effort) lstFrom.SelectedItem;
						if (roll)
							_rollout.Roll(eff);
						else
							_rollout.Unroll(eff);
						MoveEff(eff, lstFrom, lstTo);
					}else{
						MessageBox.Show(this, "Nothing to move. Please select an effort.", "Error");
					}
				}
				UpdateRolloutTab();
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void MoveEff(Effort eff, ListBox lstFrom, ListBox lstTo) {								   
			if (UI.UpdateIfValid(this, _rollout)){
				lstFrom.Items.Remove(eff);
				lstTo.Items.Add(eff);
			}else{
				_rollout.ToggleRolledState(eff);
				throw new Exception("Aborted move");
			}
		}

		private void lstUnrolled_DoubleClick(object sender, System.EventArgs e) {
			Effort eff = (Effort)this.lstUnrolled.SelectedItem;
			if (eff != null){
				ShowTestingTab(eff);
			}
		}

		private void lstRolled_DoubleClick(object sender, System.EventArgs e) {
			Effort eff = (Effort)this.lstRolled.SelectedItem;
			if (eff != null){
				ShowTestingTab(eff);
			}
		}
		private void ShowTestingTab(Effort eff) {
			this._testingEff = eff;
			UpdateTestingTab();
			this.tabMain.SelectedTab = this.tabTesting;
		}

		private void btnCompleteRoll_Click(object sender, System.EventArgs e) {
			RolloutComplete rc = new RolloutComplete(this._rollout);
			rc.ShowDialog(this);
			this.UpdateRolloutTab();
		}
		
		private void cboClients_SelectedIndexChanged(object sender, System.EventArgs e) {
			Client c = (Client)cboClients.SelectedItem;
			this.btnFindRollByScheduledRollDate.Enabled =
				this.btnFindRollByRollDate.Enabled = 	(c != null);
		}

		private void btnFindRollByScheduledRollDate_Click(object sender, System.EventArgs e) {
			try{
				bool cancel = false;
				Business.Time time = _times.item(_rollout);
				if (time != null){
					PromptTimerStop(time, ref cancel);
				}
				if (!cancel){
					Client client = (Client)this.cboClients.SelectedItem;
					Rollouts rolls = new Rollouts(client, this.dtpRoll.Value, SearchBy.ScheduledDate);
					Rollout roll = rolls[0];
					if (roll.IsNew) {
						if (MessageBox.Show(this, 
							"Rollout for " +  client.Name + " on " + dtpRoll.Value.ToLongDateString() + " doesn't exist. Create?", "Create", MessageBoxButtons.YesNo) == DialogResult.Yes){
							roll.Update();
							_rollout = roll;
							UpdateRolloutTab();
						}
					}else{
						_rollout = roll;
						UpdateRolloutTab();
					}
					if (_rollout != null){
						time = _times.item(_rollout);
						if (time == null){
							time = new QED.Business.Time(_rollout);
							PromptTimerStart(time);
						}else{
							HookUpTimer(time);
						}
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(this, ex.Message, "Exception");
			}
		}
		private void btnFindRollByRollDate_Click(object sender, System.EventArgs e) {
			try{
				bool cancel = false;
				Business.Time time = _times.item(_rollout);
				if (time != null){
					PromptTimerStop(time, ref cancel);
				}
				if (!cancel){
					Client client = (Client)this.cboClients.SelectedItem;
					Rollouts rolls = new Rollouts(client, this.dtpRoll.Value, SearchBy.RollDate); // Client can have 1 roll scheduled for a day.
					bool found = false;
					switch (rolls.Count){
						case 0:
							MessageBox.Show(this, "Can't find rollout", "QED");
							break;
						case 1:
							_rollout = rolls[0];
							UpdateRolloutTab();
							found = true;
							break;
						default: // Multiple
							ComboBox cbo = new ComboBox();
							cbo.Name = "Select Rollout by Id"; cbo.DisplayMember = "Id";
							foreach(Rollout roll in rolls){
								cbo.Items.Add(roll);
							}
							InputModal im = new InputModal("Multiple entries returned", cbo);
							if (im.ShowDialog(this) == DialogResult.OK){
								Rollout roll = (Rollout)cbo.SelectedItem;
								if (roll != null){
									_rollout = roll;
									UpdateRolloutTab();
									found = true;
								}
							}
							break;
					}
					if (found){
						time = _times.item(_rollout);
						if (time == null){
							time = new Business.Time(_rollout);
							PromptTimerStart(time);
						}else{
							HookUpTimer(time);
						}
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(this, ex.Message, "Exception");
			}
		}
		#endregion

		#region About
		private void mnuItemAbout_Click(object sender, System.EventArgs e) {
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			CatList system = cls.item("System");
			About about = new About();
			TextBox text =  about.txtAbout;
			text.Text += "Program: QED\r\n";
			text.Text += "Original Author: Jesse Hogan\r\n";
			text.Text += "Client Version: " + UI.QED_CLIENT_ID + "\r\n";
			text.Text += "Database version: " + system.Entry("/Versioning/dbVersion").Value + "\r\n";
			text.Text += "Maximum client version supported: " + system.Entry("/Versioning/MaxClientVersion").Value + "\r\n";
			text.Text += "Minimum client version supported: " + system.Entry("/Versioning/MinClientVersion").Value + "\r\n";
			about.ShowDialog(this);
		}
		#endregion

		#region Start
		/* START NOTEPAD*/
		private void menuItem8_Click(object sender, System.EventArgs e) {
			System.Diagnostics.Process.Start("notepad");
		}
		/* START CALCULATOR*/
		private void menuItem9_Click(object sender, System.EventArgs e) {
			System.Diagnostics.Process.Start("calc");
		}
	
		/* START DOCUMENTATION*/
		private void menuItem10_Click(object sender, System.EventArgs e) {
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			System.Diagnostics.Process.Start(cls.item("System").Entry("/Directories/Max Documentation/Location").Value);
		
		}
		/* Load URL */
		private void mnuItemStartEffort_Click(object sender, System.EventArgs e) {
			if (_testingEff.URLReference != ""){
				System.Diagnostics.Process.Start(_testingEff.URLReference);
			}
		}
		#endregion

		#region ConnectionManager
		private void mnuConnections_Click(object sender, System.EventArgs e) {
			ConMan con = new ConMan();
			con.Show();
		}
		#endregion

		#region Timer
		private void mnuItemTimeRollout_Click(object sender, System.EventArgs e) {
			QED.UI.Time timeDia = new QED.UI.Time(_rollout);
			timeDia.ShowDialog(this);
		}

		private void mnuItemTimeEffort_Click(object sender, System.EventArgs e) {
			QED.UI.Time timeDia = new QED.UI.Time(_testingEff);
			timeDia.ShowDialog(this);
		}

		public void Time_OnMinuteChange(Business.Time time, System.EventArgs e) {
			if (time.ForEffort){
				UpdateTimerButton(time, btnTestTimer);
			}
			if (time.ForRollout){
				UpdateTimerButton(time, btnRollTimer);
			}
		}
		private void btnRollTimer_Click(object sender, System.EventArgs e) {
			Business.Time time = _times.item(_rollout);
			bool cancel = false;
			if (time == null){
				time = new Business.Time(_rollout);
				_times.Add(time);
				time.StartTimer();
				HookUpTimer(time);
			}else{
				time.StopTimer();
				this.SubmitTime(time, ref cancel);
				if (!cancel){
					_times.Remove(time);
					UpdateTimerButton(time, btnRollTimer);
				}else{
					time.StartTimer();
				}
			}
		}
		private void btnTestTimer_Click(object sender, System.EventArgs e) {
			Business.Time time = _times.item(_testingEff);
			bool cancel = false;
			if (time == null){
				time = new Business.Time(_testingEff);
				_times.Add(time);
				time.StartTimer();
				HookUpTimer(time);
			}else{
				time.StopTimer();
				this.SubmitTime(time, ref cancel);
				if (!cancel){
					_times.Remove(time);
					UpdateTimerButton(null, btnTestTimer);
				}else{
					time.StartTimer();
				}
			}
		}
		public void SubmitTime(Business.Time time, ref bool cancel){
			/* This function is public because it is used by CurrentTimerManager */
			string minutes = time.Minutes.ToString();
			InputModal im = new InputModal("Submit this timing information", "Minutes", "Date", "Comment");
			TextBox txtMin = (TextBox)im.AnswerTable["Minutes"];
			TextBox txtDate = (TextBox)im.AnswerTable["Date"];
			txtMin.Text = minutes;
			txtDate.Text = DateTime.Now.ToString();
			if (im.ShowDialog(this) == DialogResult.OK){				
				time.Minutes = Int32.Parse(txtMin.Text);
				time.Date = DateTime.Parse(txtDate.Text);
				time.User = Thread.CurrentPrincipal.Identity.Name;
				time.Text =((TextBox)im.AnswerTable["Comment"]).Text;
				if (time.IsValid)
					time.Update();
				else
					MessageBox.Show(this, time.BrokenRules.ToString());
				cancel = false;
			}else{
				cancel = true;
			}
		}
		private void HookUpTimer(Business.Time time){
			time.OnMinuteChange += new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
			Button btn = (time.ForEffort) ? this.btnTestTimer : this.btnRollTimer;
			UpdateTimerButton(time, btn);
		}
		public void UpdateTimerButton(Business.Time time, Button btn){
			/* This function is public because it is used by CurrentTimerManager */
			if (time != null){
				if (time.IsTimerRunning){
					btn.Text = time.Minutes.ToString() + " mins";
				}else{
					btn.Text = "Start Time";
				}
				/* The code below will make sure that the two static timer buttons (btnRollTimer and btnTestTimer) are updated when CurrentTimerManager.cs updates
					its own dynamically created tiemr buttons */
				if (btn != btnTestTimer && btn != btnRollTimer){
					if (time.ForEffort){
						if (time.Effort.Id == _testingEff.Id){
							UpdateTimerButton(time, btnTestTimer);
						}
					}
					if (time.ForRollout){
						if (time.Rollout.Id == _rollout.Id){
							UpdateTimerButton(time, btnRollTimer);
						}
					}
				}
			}else{
				btn.Text = "Start Time";
			}
		}
		private void PromptTimerStop(Business.Time time, ref bool cancel){
			DialogResult res;
			res = MessageBox.Show(this,
				"A timer is running for " + ((time.ForEffort) ? "effort: " + time.Effort.ConventionalId : "rollout: " + time.Rollout.ToString()) + "\r\n" + 
				"Click Yes to stop this timer and submit time to the database\r\n" + 
				"Click No to allow the timer to continue running in the background\r\n" + 
				"Click Cancel to return to abort search",
				"QED",
				MessageBoxButtons.YesNoCancel,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1);
			if (res == DialogResult.Yes){
				time.StopTimer();
				SubmitTime(time, ref cancel);
				if (!cancel){
					_times.Remove(time);
					if (time.ForEffort){
						UpdateTimerButton(time, btnTestTimer);
					}
					if (time.ForRollout){
						UpdateTimerButton(time, btnRollTimer);
					}
				}else{
					time.StartTimer();
					return;
				}
			}
			if (res == DialogResult.No){
				cancel = false;
			}
			if (res == DialogResult.Cancel){
				cancel = true;
			}else{ // Yes or No
				time.OnMinuteChange -= new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
			}
		}
		private  void PromptTimerStart(Business.Time time){
			if (UI.AskYesNoDefNo(this, "Do you want to begin the timer for " + ((time.ForEffort) ? "effort: " + time.Effort.ConventionalId : "rollout: " + time.Rollout.ToString())) == DialogResult.Yes){
				_times.Add(time);
				time.StartTimer();
				Button btn = (time.ForEffort) ? btnTestTimer : btnRollTimer;
				UpdateTimerButton(time, btn);
				time.OnMinuteChange += new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
			}
		}
		private void mnuItemTimersCurrentlyRunning_Click(object sender, System.EventArgs e) {
			CurrentTimerManager frm = new CurrentTimerManager(_times, this);
			frm.ShowDialog(this);
		}

		#endregion

		#region Code Promotion
		#region IBM UAT
		private void mnuItemIBM_UAT_PP_OS_Click(object sender, System.EventArgs e) {
			try{
				string PPBranch = "";
				string OSBranch = "";
				RollType rt = RollType.Local_UAT; 
				bool cancel = false;
				if (this.ConfirmRoll()){
					GetPPAndOSRollData(ref PPBranch, ref OSBranch, ref rt, ref cancel);
					if (!cancel){
						IBM_PP_OS_Roller roller = new IBM_PP_OS_Roller(rt, PPBranch, OSBranch);
						SetupRoller(roller);
						roller.Roll();
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemIBM_UAT_REP_Click(object sender, System.EventArgs e) {
			try{
				RollType rt = RollType.Local_UAT;
				string branch = "";
				if (this.ConfirmRoll()){
					if(GetBranch(ref branch)){
						IBM_REP_Roller roller = new IBM_REP_Roller(rt, branch);
						this.SetupRoller(roller);
						roller.Roll();
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemIBM_UAT_MAX_Click(object sender, System.EventArgs e) {
			try{
				const string SERVER = "Server: ";
				const string BRANCH = "Branch: ";
				string server = ""; string branch = ""; 
				InputModal im = new InputModal("Enter information for production roll", SERVER, BRANCH);
				if (im.ShowDialog(this) == DialogResult.OK){
					server = im.Answer(SERVER);
					branch = im.Answer(BRANCH);
					IBM_MAX_Roller roller = new IBM_MAX_Roller(RollType.Prod, server, branch);
					this.SetupRoller(roller);
					roller.Roll();
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemIBM_PROD_PP_OS_Click(object sender, System.EventArgs e) {
			try{
				bool cancel = false;
				RollType rt = this.AskForRollType(ref cancel, RollType.Prod_Prep, RollType.Prod);
				if ( ! cancel){
					IBM_PP_OS_Roller roller = new IBM_PP_OS_Roller(rt, "HEAD", "HEAD");
					SetupRoller(roller);
					roller.Roll();
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemIBM_PROD_Reptool_Click(object sender, System.EventArgs e) {
			bool cancel = false;
			RollType rt = this.AskForRollType(ref cancel, RollType.Prod_Prep, RollType.Prod);
			if ( !cancel){
				IBM_REP_Roller roller = new IBM_REP_Roller(rt, "HEAD");
				this.SetupRoller(roller);
				roller.Roll();
			}else{
				MessageBox.Show(this, "Roll canceled", "QED");
			}
		}

		private void mnuItemIBM_Merge_Max_Click(object sender, System.EventArgs e) {
			try{
				string server = ""; string user = ""; string passwd = ""; string branch = ""; string comment = ""; bool cancel = false;
				this.GetMaxMergeInfo(ref server, ref user, ref passwd, ref branch, ref comment, ref cancel);
				if (!cancel){
					IBM_MAX_Roller roller = new IBM_MAX_Roller(RollType.Merge, server, user, passwd, branch, comment);
					this.SetupRoller(roller);
					roller.Roll();
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		#endregion
		#region Lenovo UAT
		private void mnuItemLenovo_UAT_PP_OS_Click(object sender, System.EventArgs e) {
			try{
				string PPBranch = "";
				string OSBranch = "";
				RollType rt = RollType.Local_UAT; 
				bool cancel = false;
				if (this.ConfirmRoll()){
					GetPPAndOSRollData(ref PPBranch, ref OSBranch, ref rt, ref cancel);
					if (!cancel){
						LEN_PP_OS_Roller roller = new LEN_PP_OS_Roller(rt, PPBranch, OSBranch);
						this.SetupRoller(roller);
						roller.Roll();
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemLenovo_UAT_REP_Click(object sender, System.EventArgs e) {
			try{
				RollType rt = RollType.Local_UAT;
				string branch = "";
				if (this.ConfirmRoll()){
					if(GetBranch(ref branch)){
						LEN_REP_Roller roller = new LEN_REP_Roller(rt, branch);
						this.SetupRoller(roller);
						roller.Roll();
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemLenovo_UAT_MAX_Click(object sender, System.EventArgs e) {
			try{
				string server = ""; string user = ""; string passwd = ""; string branch = ""; string comment = ""; bool cancel = false;
				this.GetMaxMergeInfo(ref server, ref user, ref passwd, ref branch, ref comment, ref cancel);
				if (!cancel){
					LEN_MAX_Roller roller = new LEN_MAX_Roller(RollType.Local_UAT, server, user, passwd, branch, comment);
					roller.Roll();
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		
		private void mnuItemLenovo_Prod_PP_OS_Click(object sender, System.EventArgs e) {
			try{
				bool cancel = false;
				RollType rt = this.AskForRollType(ref cancel, RollType.Prod_Prep, RollType.Prod);
				if ( ! cancel){
					LEN_PP_OS_Roller roller = new LEN_PP_OS_Roller(rt, "HEAD", "HEAD");
					SetupRoller(roller);
					roller.Roll();
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemLenovo_Prod_Reptool_Click(object sender, System.EventArgs e) {
			bool cancel = false;
			RollType rt = this.AskForRollType(ref cancel, RollType.Prod_Prep, RollType.Prod);
			if ( !cancel){
				LEN_REP_Roller roller = new LEN_REP_Roller(rt, "HEAD");
				this.SetupRoller(roller);
				roller.Roll();
			}else{
				MessageBox.Show(this, "Roll canceled", "QED");
			}
		}

		#endregion
		#region General Code Promotion
		private void mnuItemGenCodeRoller_Click(object sender, System.EventArgs e) {
			if (_genRoller == null)
				_genRoller = new GenRoller();
			_genRoller.ShowDialog(this);
		}
		#endregion


		#region CoPro Functions
		private bool ConfirmRoll(){
			string msg = "This will cause latest code to be rolled. Continue?";
			return (UI.AskYesNoDefNo(this, msg) == DialogResult.Yes);
		}
		
		private void GetMaxMergeInfo(ref string server, ref string user, ref string passwd, ref string branch, ref string comment, ref bool cancel){
			const string MAX_SERVER = "Max Server";
			const string MAX_USER = "Max User";
			const string MAX_PW = "Max Password";
			const string CVS_BRANCH = "CVS Branch";
			const string CVS_COMMENT = "CVS Comment";
			InputModal im = new InputModal("Enter Merge Information", MAX_SERVER, MAX_USER, MAX_PW, CVS_BRANCH, CVS_COMMENT);
			((TextBox)im.AnswerTable[MAX_PW]).PasswordChar = '*';
			((TextBox)im.AnswerTable[MAX_USER]).Text = ((BusinessIdentity)System.Threading.Thread.CurrentPrincipal.Identity).UserName;
			if (im.ShowDialog(this) == DialogResult.OK){
				server  = im.Answer(MAX_SERVER);
				user  = im.Answer(MAX_USER);
				passwd = im.Answer(MAX_PW);
				branch = im.Answer(CVS_BRANCH);
				comment  = im.Answer(CVS_COMMENT);
				cancel = false;
			}else{
				cancel = true;
			}
		}
		private void tabMain_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			string msg = "";
			if (e.Button == MouseButtons.Right){
				TabPage tp = tabMain.SelectedTab;
				if ( !(tp.Text == "Testing" || tp.Text == "Rollout" || tp.Text == "List")){
					if (tp.Tag is Roller){
						Roller roller = (Roller)tp.Tag;
						msg = "Are you sure you want to delete this TabPage\r\n" + tp.Text.ToUpper() + "?\r\n";
						if (roller.IsRolling)
							msg += "The operations is still in progress.";
					}else{
						msg = "Are you sure you want to delete this TabPage?";
					}
					if (UI.AskYesNoDefNo(this, msg) == DialogResult.Yes){
						tabMain.TabPages.Remove(tp);
					}
				}
			}
		}

		private void HookUpNewTab(Roller roller){
			roller.OnReport += new Roller.ReportHandler(Roller_Report);			
			roller.OnCompleted += new Roller.CompletedHandler(Roller_Complete);
			roller.OnPrompt += new Roller.PromptHandler(Roller_Prompt);
			TabPage tp = this.CreateLogTab(roller.ToString());
			if (tp.Tag == null)
				tp.Tag = roller;
		}
		public TabPage CreateLogTab(string name){
			TabPage tp;
			tp =  UI.GetTabByName(this.tabMain, "tab" + name);
			if (tp == null){
				RichTextBox rch = new RichTextBox();
				rch.BackColor = System.Drawing.Color.Black;
				rch.ForeColor = System.Drawing.Color.Lime;
				rch.Location = new System.Drawing.Point(8, 16);
				rch.Name = "rch" + name;
				rch.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
				rch.Size = new System.Drawing.Size(960, 632);
				rch.TabIndex = 0;
				rch.Text = "";
				rch.ReadOnly = true;
				rch.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

				tp = new TabPage(name);
				tp.Controls.Add(rch);
				tp.Location = new System.Drawing.Point(4, 22);
				tp.Name = "tab" + name;
				tp.Size = new System.Drawing.Size(984, 670);
				this.tabMain.TabPages.Add(tp);
			}
			return tp;
		}
		public void Roller_Report(Roller roller, QED.Business.CodePromotion.ReportEventArgs  args){
			TabPage tp = (TabPage)UI.GetTabByName(this.tabMain, "tab"  + roller.ToString());
			RichTextBox rch = (RichTextBox)UI.GetControlByName(tp, "rch"  + roller.ToString());
			if (rch != null){
				rch.Text += args.Message + "\r\n";
				System.Diagnostics.Debug.WriteLine(args.Message);
			}else{
				throw new Exception ("RichTextBox not found to report message to for roller " + roller.ToString());	
			}
		}
		public void Roller_Complete(Roller roller, QED.Business.CodePromotion.ReportEventArgs args){
			_rollerCount--;
			DialogResult res = MessageBox.Show(this, "Roll: " + roller.ToString() + " is complete. \r\n" + args.Message + "\r\nClose output tab?", 
				"QED", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (res == DialogResult.Yes){
				TabPage tp = (TabPage)UI.GetTabByName(this.tabMain, "tab"  + roller.ToString());
				tabMain.TabPages.Remove(tp);
			}
		}
		public void Roller_Prompt(Roller roller, EventArgs args){
			if (args is DialogEventArgs){
				DialogEventArgs diaArgs = (DialogEventArgs) args;
				diaArgs.DialogResults = 
					MessageBox.Show(this, diaArgs.Message, "QED", diaArgs.MessageBoxButtons, diaArgs.MessageBoxIcon, diaArgs.MessageBoxDefaultButton);
			}
			if (args is AskEventArgs){
				AskEventArgs askArgs = (AskEventArgs) args;
				InputModal im = new InputModal(askArgs.Prompt, askArgs.Questions);
				askArgs.DialogResult = im.ShowDialog(this);
				askArgs.AnswerTable = im.AnswerTable;
			}
		}
		public void UnhookTime(Business.Time time){
			if (time.ForEffort){
				if (_testingEff != null){
					if (time.Effort.Id == _testingEff.Id){
						time.OnMinuteChange -= new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
					}
				}
			}else if (_rollout != null){
				if (time.Rollout.Id == _rollout.Id){
					time.OnMinuteChange -= new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
				}
			}
		}

		public void SetupRoller(Roller roller){
			_rollerCount++;
			HookUpNewTab(roller);
		}
		private void mnuItemRolloutLogs_Click(object sender, System.EventArgs e) {
			RolloutLogSelector frm = new RolloutLogSelector();
			frm.ShowDialog(this);
		}
		private RollType AskForRollType(ref bool cancel, params RollType[] rollTypes){
			RollType rt = RollType.Local_UAT;
			object selectedItem;
			ComboBox cb = new ComboBox();
			cb.DropDownStyle = ComboBoxStyle.DropDownList;
			cb.Name = "Select Roll Type";
			foreach(RollType rollType in rollTypes){
				cb.Items.Add(rollType);
			}
			InputModal im = new InputModal("Select Roll Type", cb);
			if (im.ShowDialog() == DialogResult.OK){
				selectedItem = ((ComboBox)im.AnswerTable["Select Roll Type"]).SelectedItem;
				if (selectedItem == null){cancel = true; return rt;};
				rt = (RollType) selectedItem;
				if (rt == RollType.Prod){
					if (MessageBox.Show(this, 
						"This will roll code to the production environment. Before doing this you must have already done a PROD_PREP roll. If you have done the PROD_PREP roll and are ready to roll code to the production server then click OK.", 
						"QED", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Cancel){
						cancel = true;
						return rt;
					}
				}
				cancel = false;
			}else{
				cancel = true;
			}
			return rt;
		}
		private void GetPPAndOSRollData(ref string PPBranch, ref string OSBranch, ref RollType rt, ref bool cancel){
			const string PP_CVS_BRANCH = "PP CVS Branch";
			const string OS_CVS_BRANCH = "OS CVS Branch";
			const string ROLL_CODE_TO = "Roll code to:";
			const string REMOTE_UAT_ROLL = "Remote UAT Site";
			const string LOCAL_UAT_ROLL = "Local UAT Site";
			string rollType = "";
			rt = RollType.Local_UAT; // Compiler wants a default
			ComboBox cbo = new ComboBox();
			cbo.Name = ROLL_CODE_TO;
			cbo.DropDownStyle = ComboBoxStyle.DropDownList;
			cbo.Items.Add(LOCAL_UAT_ROLL); cbo.Items.Add(REMOTE_UAT_ROLL);
			InputModal im = new InputModal("Enter Branches for this roll", PP_CVS_BRANCH, OS_CVS_BRANCH);
			im.AddToPanel(cbo);
			((TextBox)im.AnswerTable[PP_CVS_BRANCH]).Text = "HEAD";
			((TextBox)im.AnswerTable[OS_CVS_BRANCH]).Text = "HEAD";
			if (im.ShowDialog(this) == DialogResult.OK){
				PPBranch = im.Answer(PP_CVS_BRANCH);
				OSBranch = im.Answer(OS_CVS_BRANCH);
				rollType = im.Answer(ROLL_CODE_TO);
				if (rollType == REMOTE_UAT_ROLL)
					rt = RollType.Remote_UAT;
				else if (rollType == LOCAL_UAT_ROLL)
					rt = RollType.Local_UAT;
				cancel = false;
			}else{
				cancel = true;
			}
		}
		private bool GetBranch(ref string branch){
			const string CVS_BRANCH = "Enter Branch";
			InputModal im = new InputModal("Enter Branches for this roll", CVS_BRANCH);
			((TextBox)im.AnswerTable[CVS_BRANCH]).Text = "HEAD";
			if (im.ShowDialog(this) == DialogResult.OK){
				branch = im.Answer(CVS_BRANCH);
				return (branch.Trim() != "");
			}
			return false;
		}
		#endregion
		#endregion
		
		#region Exit
		private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (_rollerCount != 0){
				if (UI.AskYesNoDefNo(this, "There are still rolls in progress. It is recommended that you allow the rolls to complete. Do you want to quit and abort these rolls?") == DialogResult.No){
					e.Cancel = true;
				}
			}
			if (_times.Count > 0){
				string msg = "There " + ((_times.Count == 1) ? "is still a timer" : "are still timers") + " running:\r\n";
				foreach(Business.Time time in _times){
					msg += ((time.ForEffort) ? "Effort: " + time.Effort.ConventionalId : "Rollout: " + time.Rollout.ToString()) + " Mins:" + time.Minutes.ToString();
					msg +=  "\r\n";
				}
				msg += "\r\n";
				msg += 
					"It is recommended that you stop " 
						+ ((_times.Count == 1) ? "this timer" : "these timers") + 
						" before closing QED. Do you want to quit and abort " + 
						((_times.Count == 1) ? "this timer" : "these timers") + "?";
				if (UI.AskYesNoDefNo(this, msg) == DialogResult.No){
					e.Cancel = true;
				}
			}
		}
		#endregion

		#region Misc
		public void Focus(TabPage tp){
			this.tabMain.SelectedTab = tp;
		}
		protected void rch_LinkClick(object sender, LinkClickedEventArgs e){
			System.Diagnostics.Process.Start(e.LinkText);
		}
		#endregion

		#region Reports
		public TabPage CreateReportTab(Report report){
			TabPage tp;
			string name = report.ToString();
			tp =  UI.GetTabByName(this.tabMain, "tab" + name);
			if (tp == null){
				RichTextBox rch = new RichTextBox();
				rch.BackColor = System.Drawing.Color.White;
				rch.ForeColor = System.Drawing.Color.Black;
				rch.Location = new System.Drawing.Point(8, 16);
				rch.Name = "rch" + name;
				rch.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
				rch.Size = new System.Drawing.Size(960, 600);
				rch.TabIndex = 0;
				rch.Text = "";
				rch.ReadOnly = true;
				rch.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				rch.WordWrap = false;
				rch.LinkClicked += new LinkClickedEventHandler(rch_LinkClick);

				Button btnPrint = new	Button();
				btnPrint.Name = "btnPrint" + name;
				btnPrint.Text = "&Print";
				btnPrint.Location = new Point(816, 624);
				btnPrint.Size = new Size(75, 23);
				btnPrint.Click += new System.EventHandler(Report_Print);
				btnPrint.Tag = report;

				Button btnExcel = new Button();
				btnExcel.Name = "btnExcel" + name;
				btnExcel.Text = "&Excel";
				btnExcel.Location = new Point(896, 624);
				btnExcel.Size = new Size(75, 23);
				btnExcel.Click += new System.EventHandler(Report_ExportToExcel);
				btnExcel.Tag = report;

				tp = new TabPage(name);
				tp.Location = new System.Drawing.Point(4, 22);
				tp.Name = "tab" + name;
				tp.Size = new System.Drawing.Size(984, 670);
				this.tabMain.TabPages.Add(tp);
				tp.Controls.AddRange(new Control[]{rch, btnPrint, btnExcel});
				tp.Tag = report;
				report.Tag = tp;
			}
			return tp;
		}
		private void Report_Complete(Report report, JCSLA.Reports.ReportEventArgs args){
			((TabPage)report.Tag).Text = report.ToString();
			UpdateReportRichTextBox(report, args.ReportText);
		}
		private void Report_AddRow(Report report, JCSLA.Reports.ReportEventArgs args){
			TabPage tp = (TabPage)report.Tag;
			UpdateReportRichTextBox(report, args.ReportText);
		}
		
		private void UpdateReportRichTextBox(Report report, string text){
			RichTextBox rch = null;
			TabPage tp = (TabPage)report.Tag;
			foreach(Control ctrl in tp.Controls){
				if (ctrl is RichTextBox){
					rch = (RichTextBox)ctrl;
				}
			}
			rch.Text = text;
		}

		private void Report_Print(object sender, EventArgs args){
			Report report = ((Report) ((Button)sender).Tag);
			if ( !report.IsInProgress){
				PrintDialog dia = new PrintDialog();
				PrintDocument pd = new PrintDocument();
				dia.Document = pd;
				if (dia.ShowDialog(this) == DialogResult.OK){
					report.Print(pd);
				}
			}else{
				MessageBox.Show(this, "Report is not yet complete.");
			}
		}
		private void Report_ExportToExcel(object sender, EventArgs args){
			Report report = ((Report) ((Button)sender).Tag);
			Excel.Application app =  report.ExcelApp;
			app.Visible = true;
			System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
			GC.Collect();
			GC.WaitForPendingFinalizers(); 
		}
		
		private TabPage HookUpNewTab(Report report){
			report.OnAddRow += new Report.AddRowHandler(Report_AddRow);
			report.OnCompleted += new Report.CompletedHandler(Report_Complete);
			return this.CreateReportTab(report);
		}
	
		private void mnuItemRolloutsAndBranches_Click(object sender, System.EventArgs e) {
			DateTimePicker dtpFrom = new DateTimePicker();
			DateTimePicker dtpTo = new DateTimePicker();
			if (		GetFromTo(ref dtpFrom,	ref dtpTo) ){
				object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName, dtpFrom.Value, dtpTo.Value};
				RunReport("RolloutsBranches", ctorParams);
			}
		}

		private void mnuItemRolloutTicketBreakdown_Click(object sender, System.EventArgs e) {
			DateTimePicker dtpFrom = new DateTimePicker();
			DateTimePicker dtpTo = new DateTimePicker();
			if (		GetFromTo(ref dtpFrom,	ref dtpTo) ){
				object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName, dtpFrom.Value, dtpTo.Value};
				RunReport("TicketBreakdownForRollouts", ctorParams);
			}
		}
		private void mnuItemsTruncatedRolloutTicketBreakdown_Click(object sender, System.EventArgs e) {
			DateTimePicker dtpFrom = new DateTimePicker();
			DateTimePicker dtpTo = new DateTimePicker();
			if (		GetFromTo(ref dtpFrom,	ref dtpTo) ){
				object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName, dtpFrom.Value, dtpTo.Value};
				RunReport("TruncatedTicketBreakdownForRollouts", ctorParams);
			}
		}
		private void mnuItemsDevRollouts_Click(object sender, System.EventArgs e) {
			DateTimePicker dtpFrom = new DateTimePicker();
			DateTimePicker dtpTo = new DateTimePicker();
			if (		GetFromTo(ref dtpFrom,	ref dtpTo) ){
				object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName, dtpFrom.Value, dtpTo.Value};
				RunReport("DevRollouts", ctorParams);
			}
		}
		
		private void mnuItemTimeReport_Click(object sender, System.EventArgs e) {
			object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName};
			RunReport("TimeReport", ctorParams);
		}
		private void mnuItemQAPostRollReportCard_Click(object sender, System.EventArgs e) {
			object[] ctorParams = {_rollout, ((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName};
			RunReport("QAPostRollReportCard", ctorParams);
	
		}
		private void mnuItemUnrolledEfforts_Click(object sender, System.EventArgs e) {
			object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName};
			RunReport("UnrolledEfforts", ctorParams);
		}
		private void mnuItemUnapprovedEfforts_Click(object sender, System.EventArgs e) {
			object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName};
			RunReport("UnapprovedEfforts", ctorParams);
		}

		private void mnuItemEmployeeTime_Click(object sender, System.EventArgs e) {
			object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName};
			RunReport("EmployeeTime", ctorParams);
		}

		public void RunReport(string reportClassName, object[] ctorParams){
			Type reportType = Type.GetType("QED.UI.Reports." + reportClassName);
			Report report = GetCurrentReportIfExists(reportType);
			if (report == null){
				report = (Report)System.Activator.CreateInstance(reportType, ctorParams);
				RunReport(report, true);
			}else{
				//RunReport(report, false);
				tabMain.TabPages.Remove((TabPage)report.Tag);
				report = (Report)System.Activator.CreateInstance(reportType, ctorParams);
				this.Focus(((TabPage)report.Tag));	
				RunReport(report, true);
			}
		}
	
		private void mnuItemScheduledRollouts_Click(object sender, System.EventArgs e) {
			object[] ctorParams = {((BusinessIdentity)Thread.CurrentPrincipal.Identity).FullName};
			RunReport("ScheduledRollouts", ctorParams);
		}

		public Report GetCurrentReportIfExists(Type reportType){
			Report report = null; 
			foreach (TabPage tabPage in this.tabMain.TabPages){
				if (tabPage.Tag != null && ((Report)tabPage.Tag).GetType() == reportType){
					report = (Report)tabPage.Tag; 
					report.Clear();
				}
			}
			return report;
		}
		private void RunReport(Report report, bool newTab){
			TabPage tp = null;
			try{
				if (newTab){
					tp = HookUpNewTab(report);
				}else{
					tp = (TabPage)report.Tag;
				}
				tp.Text = tp.Text + "[RUNNING]";
				report.Run(true);
			}
			catch(Exception ex){
				tp.Text = report.ToString() + "[ERROR]";
				MessageBox.Show(this, ex.Message, "QED");
			}			
		}

		private bool GetFromTo(ref DateTimePicker dtpFrom, ref DateTimePicker dtpTo){
			InputModal im = new InputModal("Select from and to date");
			im.AddToPanel("From : ", dtpFrom);
			im.AddToPanel("To : ", dtpTo);
			DialogResult res = im.ShowDialog(this);
			dtpFrom.Value = dtpFrom.Value.Date;
			dtpTo.Value = dtpTo.Value.Date;
			return res == DialogResult.OK;
		}


	}
	#endregion
		
}


