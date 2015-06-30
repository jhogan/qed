using System;
using QED.Business;
using QED.SEC;
using JCSLA.Reports;
using System.Collections;

namespace QED.UI.Reports {
	public class TimeReport : JCSLA.Reports.Report{
		bool _isInProgress = false;
		public TimeReport(string ranBy) : base(ranBy){
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		protected override void run() {
			string[] row = { 
							   "Effort", "Description", "Time", "Rolled"};
			Table table = base.CreateDefaultTable("Effort Times",row);
			Efforts allEffs = Efforts.AllEfforts();
			foreach(Effort eff in allEffs){
				row = new string[]{eff.ConventionalId, base.Truncate(eff.Desc, 30, true), eff.Times.Total.ToString(), ((eff.Rolled) ? "Y" : "N")};
				Row objRow = table.AddRow(row);
				base.RaiseOnAddRow(objRow);
			}
			base.RaiseOnComplete();
		}
		public override string Title{
			get{
				return "Time Report";
			}
		}
	}
	public class QAPostRollReportCard: JCSLA.Reports.Report{
		bool _isInProgress = false;
		Rollout _rollout;
		public QAPostRollReportCard(Rollout rollout, string ranBy) : base(ranBy){
			_rollout = rollout;
		}
		protected override void run() {
			bool beenHere;
			string rollDate;
			Sheet sheet = base.CreateSheet("Report Card");
			Table tabBanner = sheet.Add(new Table(sheet, 2));
			tabBanner.AddRow("Client: ", _rollout.Client.Name);
			if (_rollout.RolledDate == DateTime.MinValue)
				tabBanner.AddRow("Roll Date: ", "");
			else
				tabBanner.AddRow("Roll Date: ", _rollout.RolledDate.ToShortDateString());

			tabBanner.AddRow("Number of Tickets: ", _rollout.Efforts.Tickets.Count.ToString());
			tabBanner.AddRow("Number of Projects: ", _rollout.Efforts.Projects.Count.ToString());
			
			string[] rowReportCard ={
										"Roll Status (Pass / Fail)", "Prj # / Tkt #", "Title / Description", "Link to Ticket system", 
										"Planned Roll Dates",  "Actual Roll Date", "QA Budgeted Hours", "QA Actual Hours - prior to roll",	
										"QA tester", 	"UAT Approved (Name)",  "# of Defects found in QA",	"# of Defects found post roll", 
										"Was code fixed during roll?", "Reason for code fix (see reason codes)", "Reason For Roll Back (see reason codes)",  "Comments / Updates"
									};
			string[] rowContacts = {
									   "P#/T#", "Issue Description", "PM", "DB", "MAX", "QA", "Roll Date"
								   };
			Table tabReportCart = sheet.Add(new Table(sheet, "", rowReportCard));

			Sheet sheetContacts = base.CreateSheet("Resources");
			
			Table tabContacts = sheetContacts.Add(new Table(sheetContacts, "", rowContacts));
			EffortRollout er;
			Defects defsPostRoll;
			foreach(Effort eff in _rollout.Efforts){
				for(int i = 0; i< rowReportCard.Length; i++) rowReportCard[i] = "";
				//er = eff.GetEffortRollout(_rollout);
				er = _rollout.GetEffortRollout(eff);
				defsPostRoll = _rollout.GetDefects(eff);
				rowReportCard[0] = (er.Rolled) ? "Pass" : "Fail";
				rowReportCard[1] = eff.ConventionalId;
				rowReportCard[2] = eff.Desc;
				rowReportCard[3] = eff.URLReference;
				beenHere = false;
				foreach(DateTime dt in eff.FutureScheduledRollDates){
					if (beenHere){
						rowReportCard[4] += " and ";
					}
					rowReportCard[4] += dt.ToShortDateString();
					beenHere = true;
				}
				rollDate = (eff.RollDate == DateTime.MinValue) ? "" : eff.RollDate.ToShortDateString();
				rowReportCard[5] = rollDate;
				rowReportCard[6] = eff.QABudgetedHours.ToString();
				rowReportCard[7] = eff.QAActualHours.ToString();
				rowReportCard[8] = eff.TestedBy;
				rowReportCard[9] = eff.UATApprovedBy;
				rowReportCard[10] = eff.Defects.Count.ToString();
				rowReportCard[11] = defsPostRoll.Count.ToString();
				rowReportCard[12] = er.CodeFixedYesNo;
				rowReportCard[13] = er.ReasonForCodeFix;
				rowReportCard[14] = er.ReasonForRollBack;
				rowReportCard[15] = er.FinalComments;
				tabReportCart.AddRow(true, rowReportCard);
				rowContacts[0] = eff.ConventionalId;
				rowContacts[1] = eff.Desc;
				rowContacts[2] = eff.PMResource;
				rowContacts[3] = eff.DBResource;
				rowContacts[4] = eff.MaxResource;
				rowContacts[5] = eff.TestedBy;
				rowContacts[6] = rollDate;
				tabContacts.AddRow(true, rowContacts);
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class UnrolledEfforts : JCSLA.Reports.Report{
		bool _isInProgress = false;
		public UnrolledEfforts(string ranBy) : base(ranBy){
		}
		protected override void run() {
			Table table = base.CreateDefaultTable("", "Effort", "Description", "Scheduled Date(s)");
			bool beenHere;
			string scheduledDate = "";
			string[] row = new string[3];
			foreach(Effort eff in Efforts.Unrolled()){
				row[0] = base.Truncate(eff.ConventionalId, 15, true);
				row[1] = base.Truncate(eff.Desc, 20, true);
				row[2] = "";
				beenHere = false;
				if (eff.Rollouts.Count > 0){
					foreach(Rollout rollout in eff.Rollouts){
						scheduledDate = (rollout.ScheduledDate != DateTime.MinValue) ? rollout.ScheduledDate.ToShortDateString() : "";
						if (!beenHere){
							row[2] = scheduledDate;
							table.AddRow(row);
							beenHere = true;
						}
						else{
							table.AddRow(true, "", "", scheduledDate);
						}
					}
				}else{
					table.AddRow(row);
				}
				
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class UnapprovedEfforts : JCSLA.Reports.Report{
		bool _isInProgress = false;
		public UnapprovedEfforts(string ranBy) : base(ranBy){
		}
		protected override void run() {
			Table table = base.CreateDefaultTable("", "Effort", "Description", "QA Approved", "UAT Approved");
			string uatApproved = "";  string qaApproved = "";
			foreach(Effort eff in Efforts.AllEfforts()){
				if (eff.UATApproved && eff.Approved) continue;
				uatApproved = (eff.UATApproved) ? "Y" : "N";
				qaApproved = (eff.Approved) ? "Y" : "N";
				table.AddRow(true, eff.ConventionalId, base.Truncate(eff.Desc, 30, true), qaApproved, uatApproved);
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class EmployeeTime: JCSLA.Reports.Report{
		bool _isInProgress = false;
		public EmployeeTime(string ranBy) : base(ranBy){
		}
		protected override void run() {
			Times tckTimes = null;
			Times prjTimes = null;
			Times rollTimes = null;
			Efforts effsWorked = null;
			Rollouts rollsWorked = null;
			Efforts prjWorked = null;
			Efforts tckWorked = null;
			Table table = base.CreateDefaultTable("Employee Time", "Employee", "Time on Tickets", "Time on Projects", "Time on Rollouts");
			BusinessIdentities employees =  BusinessIdentities.AllBusinessIdentities();
			foreach(BusinessIdentity emp in employees){
				effsWorked = emp.EffortsWorked;
				rollsWorked = emp.RolloutsWorked;
				prjWorked = effsWorked.Projects;
				tckWorked = effsWorked.Tickets;
				tckTimes = tckWorked.GetTimes(emp.Email);
				prjTimes = prjWorked.GetTimes(emp.Email);
				rollTimes = rollsWorked.GetTimes(emp.Email);
				table.AddRow(true, emp.FullName, tckTimes.Total.ToString(),
					prjTimes.Total.ToString(),
					rollTimes.Total.ToString());
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class ScheduledRollouts : JCSLA.Reports.Report{
		bool _isInProgress = false;
		public ScheduledRollouts(string ranBy) : base(ranBy){
		}
		protected override void run() {
			string clientName;
			Table table = base.CreateDefaultTable("Scheduled Rollouts", "Client", "Scheduled Date", "Efforts", "Description", "Was Effort Rolled");
			foreach(Rollout rollout in Rollouts.GetAllUnrolled("clientId")){
				clientName = Truncate(rollout.Client.Name, 20, true);
				if (rollout.Efforts.Count > 0){
					foreach(Effort eff in rollout.Efforts){
						table.AddRow(clientName, rollout.ScheduledDate.ToShortDateString(), eff.ConventionalId, base.Truncate(eff.Desc, 30, true) , ((eff.Rolled)?"Y":"N"));
					}
				}else{
					table.AddRow(clientName, rollout.ScheduledDate.ToShortDateString());
				}
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class RolloutsBranches : JCSLA.Reports.Report{
		bool _isInProgress = false;
		DateTime _from; DateTime _to;
		public RolloutsBranches(string ranBy, DateTime from, DateTime to) : base(ranBy){
			_from = from; _to = to;
		}
		protected override void run() {
			string clientName;
			string effDesc;
			string maxDev;
			string dbaDev;
			string webDev;
			string[] lines;
			DateTime scheduledDate;
			bool rolled;
			DateTime rolledDate;
			Table table = base.CreateDefaultTable("Rollouts", "Scheduled Date", "Client", "Rolled Date", "Efforts", "Max Dev", "DBA Dev", "Web Dev", "Rolled", "Branchs", "Files");
			foreach(Rollout rollout in Rollouts.Get(_from, _to, "scheduledDate")){
				scheduledDate = rollout.ScheduledDate;
				rolledDate = rollout.RolledDate;
				clientName = Truncate(rollout.Client.Name, 20, true);
				table.AddRow(true, scheduledDate.ToShortDateString(), clientName, rolledDate.ToShortDateString());
				foreach(Effort eff in rollout.Efforts){
					effDesc = eff.ExternalId_Desc;
					maxDev = eff.MaxResource;
					dbaDev = eff.DBResource;
					webDev = eff.WebResource;
					rolled = eff.Rolled;
			 		table.AddRow(false, "", "", "", Truncate(effDesc, 30, true), maxDev, dbaDev, webDev, ((rolled) ? "Y" : "N"));
					if (eff.BranchFileHierarchy != ""){
						lines = eff.BranchFileHierarchy.Split(new char[2]{'\r', '\n'});
						foreach(string line in lines){
							if (line.Trim() != ""){
								if (line.StartsWith("\t")){
									table.AddRow(false, "", "", "", "", "", "", "", "", "", line.Trim());
								}else{
									table.AddRow(false, "", "", "", "", "", "", "", "", line.Trim());
								}
							}
						}
					}
				}
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class DevRollouts : JCSLA.Reports.Report{
		bool _isInProgress = false;
		DateTime _from; DateTime _to;
		public DevRollouts(string ranBy, DateTime from, DateTime to) : base(ranBy){
			_from = from; _to = to;
		}
		protected override void run() {
			string devType = "";
			ArrayList devs = new ArrayList();
			string[] lines;
			Table table = base.CreateDefaultTable("Dev Rollouts", "Developer", "Rolled Date", "Effort", "Branch", "Files");
			Rollouts rollouts = Rollouts.Get(_from, _to, true, "rolledDate");
			foreach(Rollout rollout in rollouts){
				foreach (Effort eff in rollout.Efforts){
					if (eff.Rolled){
						if (eff.MaxResource != "" && !devs.Contains(eff.MaxResource))
								devs.Add(eff.MaxResource );
						if (eff.DBResource != "" && !devs.Contains(eff.DBResource))
								devs.Add(eff.DBResource );
						if (eff.WebResource != "" && !devs.Contains(eff.WebResource))
								devs.Add(eff.WebResource );
					}
				}
			}
			devs.Sort();
			foreach (string dev in devs){
				table.AddRow(true, dev);
				foreach(Rollout rollout in rollouts){
					foreach (Effort eff in rollout.GetCachedEfforts(false)){
						if (dev == eff.MaxResource || dev == eff.DBResource || dev == eff.WebResource){
							devType = (dev == eff.MaxResource) ? "MAX" : devType;
							devType = (dev == eff.DBResource) ? "DBA" : devType;
							devType = (dev == eff.WebResource) ? "WEB" : devType;
							table.AddRow(false, "", eff.RollDate.ToShortDateString(), Truncate(eff.ExternalId_Desc, 30, true));
							if (eff.BranchFileHierarchy != ""){
								lines = eff.BranchFileHierarchy.Split(new char[2]{'\r', '\n'});
								foreach(string line in lines){
									if (line.Trim() != ""){
										if (line.StartsWith("\t")){
											table.AddRow(false, "", "", "", "", line.Trim());
										}else{
											table.AddRow(false, "", "", "",  line.Trim());
										}
									}
								}
							}
						}
					}
				}
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class TicketBreakdownForRollouts: JCSLA.Reports.Report{
		bool _isInProgress = false;
		DateTime _from; DateTime _to;
		public TicketBreakdownForRollouts(string ranBy, DateTime from, DateTime to) : base(ranBy){
			_from = from; _to = to;
		}
		protected override void run() {
			Rollouts rollouts = Rollouts.Get(_from, _to, "scheduledDate");
			Table table = base.CreateDefaultTable("Rollouts", "Rollout For", "Requestor", "Dev", "Env", "Branch", "Description");
			foreach(Rollout rollout in rollouts){
				table.AddRow(true , rollout.Client.Name + " on " + rollout.ScheduledDate.ToShortDateString());
				Efforts effs = rollout.Efforts;
				effs.Sort();
				foreach(Effort eff in effs){
					table.AddRow(true, eff.ConventionalId, eff.RequestorUserName, eff.DevelopersUserNames_CommaSeperated,  eff.Environment, eff.Branches_CommaSeperated, base.Truncate(eff.Desc, 30, true));
				}
				table.AddRow(false, "");
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	public class TruncatedTicketBreakdownForRollouts: JCSLA.Reports.Report{
		bool _isInProgress = false;
		DateTime _from; DateTime _to;
		public TruncatedTicketBreakdownForRollouts(string ranBy, DateTime from, DateTime to) : base(ranBy){
			_from = from; _to = to;
		}
		protected override void run() {
			Rollouts rollouts = Rollouts.Get(_from, _to, "scheduledDate");
			Table table = base.CreateDefaultTable("Rollouts", "Rollout For", "Env");
			foreach(Rollout rollout in rollouts){
				table.AddRow(true , rollout.Client.Name + " on " + rollout.ScheduledDate.ToShortDateString());
				Efforts effs = rollout.Efforts;
				effs.Sort();
				foreach(Effort eff in effs){
					table.AddRow(true, eff.ExternalId_Desc, eff.Environment);
				}
				table.AddRow(false, "");
			}
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}

	/*
	public class Template : JCSLA.Reports.Report{
		bool _isInProgress = false;
		public Template(string ranBy) : base(ranBy){
		}
		protected override void run() {
			// Put report code here
			base.RaiseOnComplete();
		}
		public override bool IsInProgress {
			get {
				return _isInProgress;
			}
			set{
				_isInProgress = value;
			}
		}
		public override string Title{
			get{
				return this.GetType().Name;
			}
		}
	}
	*/
}
