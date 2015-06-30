using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
using QED.DataValidation;
using QED.SEC;
namespace QED.Business{
	/// <summary>
	/// Summary description for efforts.
	/// </summary>
	public enum EffortType {
		Project, Ticket, Undetermined, InValid
	}
	public enum ContactTypes :int{
		Reporter = 1,
		Helpdesk = 2,
		Resolver = 3,
		Other = 4,
	}
	public class EffortID {
		string _id;
		public EffortID(string id) {
			_id = id.Trim().ToUpper();
		}
		public EffortType Type {		
			get {
				Double d;
				string numericPart = _id.Substring(1);
				switch (_id.Substring(0,1)) {
					case "P":
						if (! Double.TryParse(numericPart, System.Globalization.NumberStyles.Integer, null,  out d))
							return EffortType.InValid;
						return EffortType.Project;
					case "T":
						if (! Double.TryParse(numericPart, System.Globalization.NumberStyles.Integer, null,  out d))
							return EffortType.InValid;
						return EffortType.Ticket;
					default:
						if (Double.TryParse(_id, System.Globalization.NumberStyles.Integer, null,  out d)) { // read: if (id.IsNumeric())
							return EffortType.Undetermined;
						}else {
							return EffortType.InValid;
						}
				}
			}
		}
		public int NumericPart {
			get {
				if (this.Type != EffortType.InValid || this.Type != EffortType.Undetermined) {
					return Convert.ToInt32(_id.Substring(1));
				}else {
					throw new Exception("Error parsing effort type.");
				}
			}
		}
		public char TypeChar {
			get {
				if (this.Type != EffortType.InValid || this.Type != EffortType.Undetermined) {
					return Convert.ToChar(_id.Substring(0, 1));
				}else {
					throw new Exception("Error parsing effort type.");
				}
			}
		}
		public override string ToString() {
			return _id;
		}

	}
	public class Efforts : BusinessCollectionBase {
		#region Instance Data
		const string _table = "efforts";
		Rollout _rollout;
		MySqlDBLayer _dbLayer;	
		string n = System.Environment.NewLine;
		#endregion
		#region Collection Members
		public Effort Add(Effort obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		
		public bool Contains(Effort obj) {
			foreach(Effort child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public bool Contains(int id) {
			foreach(Effort child in List) {
				if (child.Id == id){
					return true;
				}
			}
			return false;
		}
		public Effort this[int id] {
			get{
				return (Effort) List[id];
			}
		}
		public Effort item(int id) {
			foreach(Effort obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public Efforts() {
		}
		public void Update() {
			foreach (Effort obj in List) {
				obj.Update();
			}
		}
		public Efforts(Rollout rollout) {
			Effort obj;
			_rollout= rollout;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "rolloutId", rollout.Id)){
					while(dr.Read()) {
						obj = new Effort(dr, true);
						obj.BusinessCollection = this;
						List.Add(obj);
					}
				}
			}
		}
		public static Efforts Unrolled(){ 
			Efforts effs = new Efforts();
			Effort eff;
			bool beenHere = false;
			System.Text.StringBuilder inList = new System.Text.StringBuilder();
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				MySqlCommand cmd = conn.CreateCommand();
				cmd.CommandText =	 "SELECT effId FROM effortRollouts WHERE rolled = 1";
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					while(dr.Read()){
						if (beenHere) inList.Append(","); else beenHere = true;
						inList.Append(Convert.ToString(dr[0]));
					}
				}
				if (inList.Length > 0){
					cmd.CommandText =	 "SELECT * FROM efforts WHERE id not in (" + inList.ToString() + ")";
				}else{
					cmd.CommandText =	 "SELECT * FROM efforts";
				}
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					while(dr.Read()) {
						eff = new Effort(dr, true);
						eff.BusinessCollection = effs;
						effs.Add(eff);
					}
				}
		
			}
			return effs;
		}
		public static Efforts AllEfforts(){ 
			Efforts effs = new Efforts();
			Effort eff;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				MySqlCommand cmd = conn.CreateCommand();
				cmd.CommandText =	 "SELECT * FROM efforts";
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					while(dr.Read()) {
						eff = new Effort(dr, true);
						eff.BusinessCollection = effs;
						effs.Add(eff);
					}
				}
			}
			return effs;
		}
		#endregion
		#region Business Members
		public Efforts Projects{
			get{
				Efforts ret= new Efforts();
				foreach (Effort eff in this){
					if (eff.EffortType == EffortType.Project)
						ret.Add(eff);
				}
				return ret;
			}
		}
		public Efforts Tickets{
			get{
				Efforts ret= new Efforts();
				foreach (Effort eff in this){
					if (eff.EffortType == EffortType.Ticket)
						ret.Add(eff);
				}
				return ret;
			}
		}
		public Times GetTimes(string userEmail){
			Times times = new Times();
			foreach(Effort eff in this){
				foreach(Time time in eff.Times){
					if (time.User == userEmail){
						times.Add(time);
					}
				}
			}
			return times;
		}
		public Efforts Sort(){
			ArrayList al = new ArrayList();
			Efforts newEfforts = new Efforts();
			foreach(Effort eff in this){
				al.Add(eff.ConventionalId);
			}
			al.Sort();
			foreach(string conventionalID in al){
				foreach(Effort eff in this){
					if (eff.ConventionalId == conventionalID){
						newEfforts.Add(eff);
					}
				}
			}
			this.Clear();
			foreach(Effort eff in newEfforts){
				this.Add(eff);
			}
			return this;
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Effort : BusinessBase  {
		#region Instance Data
		string _table = "efforts";
		MySqlDBLayer _dbLayer;	
		EffortType  _effType;
		int _rolloutId = -1;
		int _id = -1;
		int _clientId = -1;
		string _pmResource = "";
		string _webResource = "";
		string _dbResource = "";
		bool _uatApproved = false;
		string _submitedBy = "";
		string _projectManager = "";
		string _maxResource = "";
		string _uatApprovedBy = "";
		string _branchFileHierarchy = "";
		string _environment = "";
		Client _client;
		Times _times;
		int _priorityID;
		EffortRollouts _cachedEffortRollouts = null; 
		string n = System.Environment.NewLine;
		#region Internal Data
		int _extId = -1;
		char _type = '\0';
		string _testedBy = "" ;
		bool _approved = false;
		bool _overdue = false;
		#endregion
		#region External Data
		DateTime _creationDate;
		DateTime _closeDate;
		string _requester = "";
		string _desc = "";
		int qaBudgetedHours;
		int qaActualHours;
		string qaTester;
		string _urlReference;
		DateTime _startDate;
		DateTime _endDate;
		string _reviewer;
		string _submitBy;
		string _status;
		string _title;
		EffortID _effId;		
		BusinessCollectionBase _businessCollection;
		#endregion
		Defects _defects;
		Messages _messages;
		#endregion
		#region DB Access / ctors
		public override string Table {
			get {
				return _table;
			}
		}
		public override object Conn {
			get {
				return Connections.Inst.item("QED_DB").MySqlConnection;
			}
		}
		private void Setup() {
			if (_dbLayer == null) {
				_dbLayer = new MySqlDBLayer(this);
			}
		}
		public override void SetId(int id) {
			/* This function is public for technical reasons. It is intended to be used only by the db
				 * layer*/
			_id = id;
		}
		public override BusinessCollectionBase BusinessCollection {
			get{
				return _businessCollection;
			}
			set {
				_businessCollection = value;
			}
		}
		public Effort() {
			Setup();
			base.MarkNew();
		}	
		public Effort(string id, bool loadExternal) {
			_effId = new EffortID(id);
			if (_effId.Type == EffortType.Undetermined || _effId.Type == EffortType.InValid) {
				throw new Exception("Effort Id is in the wrong format. Correct format: P05431 or T10210");
			}else{
				Setup();
				this.Load(_effId.ToString(), loadExternal);
			}
		}
		public Effort(int id, bool loadExternal) {
			SetId(id);
			Setup();
			this.Load(id, loadExternal);
		}
		public Effort(MySqlDataReader dr) {
			this.Load(dr);
		}
		public Effort(MySqlDataReader dr, bool loadExternal) {
			this.Load(dr);
			if (loadExternal)
				this.LoadExternal();
		}
		private void Load(string id, bool loadExternal) {
			_effId = new EffortID(id);
			using (MySqlConnection conn = (MySqlConnection)this.Conn){
				MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + _table + " WHERE extId = @extId AND type = @type ORDER BY 1", conn);
				cmd.Parameters.Add("@extID", _effId.NumericPart);
				cmd.Parameters.Add("@type", _effId.TypeChar.ToString());
				conn.Open();
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					if (dr.HasRows) {
						dr.Read();
						this.Load(dr);
						SetId(_id); // Load internal id after load since we are using external id for load.
					}else{
						// No data in private DB so create entry corresponding to external db
						this._extId = _effId.NumericPart;
						MarkNew();
					}
				}
			}
			if (loadExternal){
				LoadExternal();
			}
		}
		private void Load(int id, bool loadExternal) {
			using (MySqlConnection conn = (MySqlConnection)this.Conn){
				MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + _table + " WHERE id = @id", conn);
				cmd.Parameters.Add("@id", id);
				conn.Open();
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					dr.Read();
					this.Load(dr);
				}
			}
			if (loadExternal){
				LoadExternal();
			}
		}
		private void LoadExternal(){
			if (this.EffortType == EffortType.Ticket) {
				using(MySqlConnection conn = (MySqlConnection)Connections.Inst.item("HTS").MySqlConnection){
					using (MySqlDataReader dr = MySqlDBLayer.Load(conn, "Tickets", "ID", _effId.NumericPart)){
						if (!dr.HasRows) throw new Exception ("Could not find ticket. Check ticket id.");
						dr.Read();
						this.LoadTicket(dr);
					}
				}
			}else{
				//Still wating for PMO access
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			_extId = Convert.ToInt32(dr["extId"]);
			this._effId = new EffortID(dr["type"].ToString() + _extId);
			_testedBy = Convert.ToString(dr["testedBy"]);
			_approved = Convert.ToBoolean(dr["approved"]);
			_pmResource = Convert.ToString(dr["pmResource"]);
			_webResource = Convert.ToString(dr["webResource"]);
			_dbResource = Convert.ToString(dr["dbResource"]);
			_uatApproved = Convert.ToBoolean(dr["uatApproved"]);
			_projectManager = Convert.ToString(dr["pmResource"]);
			_maxResource = Convert.ToString(dr["maxResource"]);
			_uatApprovedBy = Convert.ToString(dr["uatApprovedBy"]);
			_branchFileHierarchy = Convert.ToString(dr["branchFileHierarchy"]);
			_environment = Convert.ToString(dr["environment"]);
			if (Convert.ToChar(dr["type"]) == 'P')
				_effType = EffortType.Project;
			if (Convert.ToChar(dr["type"]) == 'T')
				_effType = EffortType.Ticket;
			this._desc = Convert.ToString(dr["desc_"]);
			_requester = Convert.ToString(dr["requestor"]);
			MarkOld();
		}
		public void LoadTicket(MySqlDataReader dr) {
			/*TODO: There are more ticket data we could get here but other table hits would be necessary */
			this._extId = Convert.ToInt32(dr["ID"]);
			this._overdue = (dr["overdue"] == DBNull.Value) ? false : Convert.ToBoolean(dr["overdue"]);
			if (dr["CreationDate"] != DBNull.Value)
				this._creationDate = Convert.ToDateTime(dr["CreationDate"]);
			if (dr["CloseDate"] != DBNull.Value)
				this._closeDate = Convert.ToDateTime(dr["CloseDate"]);
			if (dr["ClientId"] != DBNull.Value)
				this._clientId = Convert.ToInt32(dr["ClientId"]);
			if (dr["PriorityID"] != DBNull.Value)
				this._priorityID = Convert.ToInt32(dr["PriorityID"]);
		}
		public override void Update(){
			_dbLayer.Update();
			if (this._defects != null)
				this._messages.Update();
			if (this._messages != null)
				this._messages.Update();
		}
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@extId", this.ExtId);
				paramHash.Add("@type", this._effId.TypeChar.ToString());
				paramHash.Add("@testedBy", this.TestedBy);
				paramHash.Add("@approved", this.Approved);
				paramHash.Add("@pmResource", this._pmResource);
				paramHash.Add("@webResource", this._webResource);
				paramHash.Add("@dbResource", this._dbResource);
				paramHash.Add("@uatApproved", this._uatApproved);
				paramHash.Add("@maxResource", this._maxResource);
				paramHash.Add("@desc_", this.Desc);
				paramHash.Add("@uatApprovedBy", this.UATApprovedBy);
				paramHash.Add("@branchFileHierarchy", this.BranchFileHierarchy);
				paramHash.Add("@environment", this.Environment);
				if (this.IsNew && this.IsTicket)
					paramHash.Add("@requestor", this.RequesterFromTicketServer);
				else
					paramHash.Add("@requestor", this.Requester);
				return paramHash;
			}
		}
		public EffortRollout GetEffortRollout(Rollout rollout){
			foreach(EffortRollout er in this.EffortRollouts){
				if (er.RolloutId == rollout.Id){
					return er;
				}
			}
			return null;
		}

		#endregion
		#region Business Properties
		#region DB Props
		public override int Id{
			get {
				return _id;
			}
		}
		public EffortType EffortType{
			get{
				return this._effId.Type;
			}
		}
		
		public int ExtId{
			get{
				return _extId;
			}
			set{
				SetValue(ref _extId, value);
			}
		}
		public string TestedBy{
			get{
				return _testedBy;
			}
			set{
				SetValue(ref _testedBy, value);
			}
		}
		public bool Approved{
			get{
				return _approved;
			}
			set{
				SetValue(ref _approved, value);
			}
		}
		public bool Rolled{
			get{
				EffortRollouts effRolls = this.EffortRollouts;
				foreach(EffortRollout effRoll in effRolls){
					if (effRoll.Rolled){
						return true;
					}
				}
				return false;
			}
		}
		public bool Overdue{
			get{
				return _overdue;
			}
		}
		public DateTime CreationDate{
			get{
				return _creationDate;
			}
		}
		public DateTime CloseDate{
			get{
				return _closeDate;
			}
		}
		public string Desc{
			get{
				return _desc;
			}
			set{
				SetValue(ref _desc, value);
			}
		}
		public int QABudgetedHours{
			get{
				//Derived from PMO
				return qaBudgetedHours;
			}
		}
		public int QAActualHours{
			get{
				// Derived from time structure
				return qaActualHours;
			}
		}
		public string URLReference{
			get{
				if (this.EffortType == EffortType.Ticket)
					return "http://ticket/viewticket.php?ticket=" + this.ExtId.ToString();
				else
					return "http://matrix/pmo/pmo_office/glb_base/pub_proj/dsp_proj3_edit.cfm?pid=" + this.ExtId + "&navid=35&sectionid=40&action=view";
				
			}
		}
		public DateTime StartDate{
			get{
				return _startDate;
			}
		}
		public DateTime EndDate{
			get{
				return _endDate;
			}
		}
		public string SubmitBy{
			get{
				return _submitBy;
			}
			set{
				SetValue(ref _submitBy, value);
			}
		}
		public string Title{
			get{
				return _title;
			}
		}
		#endregion
		public string MaxResource{
			get{
				return _maxResource.Trim().ToLower();
			}
			set{
				SetValue(ref _maxResource, value);
			}
		}

		public string ConventionalId{
			get{
				return _effId.ToString();
			}
		}
		public string ExternalId_Desc{
			get{
				return this.ConventionalId + " - " + this.Desc;
			}
		}
		public Messages Messages{
			get {
				if (_messages == null)
					_messages = new Messages((BusinessBase)this);
				return _messages;
			}
		}
		public Defects Defects{
			get {
				if (_defects == null)
					_defects = new Defects(this);
				return _defects;
			}
		}
		public EffortRollouts EffortRollouts{
			get{
				return new EffortRollouts(this);
			}
		}
		public EffortRollouts GetCachedEffortRollouts(bool reload){
			if (reload || _cachedEffortRollouts == null)
				_cachedEffortRollouts = new EffortRollouts(this);
			return _cachedEffortRollouts;

		}
		public Rollouts Rollouts{
			get{
				EffortRollouts effRolls = this.EffortRollouts;
				return effRolls.Rollouts;
			}
		}
		public Rollout RolloutWhereEffortIsRolled{
			get{
				foreach (EffortRollout effRoll in this.EffortRollouts){
					if (effRoll.Rolled){
						return effRoll.Rollout;
					}
				}
				return null;
			}
		}
		public Rollouts RolloutsWhereEffortIsUnrolled{
			get{
				Rollouts ret = new Rollouts();
				foreach (EffortRollout effRoll in this.EffortRollouts){
					if (!effRoll.Rolled){
						ret.Add(effRoll.Rollout);
					}
				}
				return ret;
			}
		}
		public Client Client{
			get{
				if (_client == null && _clientId != -1)
					_client = new Client(_clientId);
				return _client;
			}
			set{
				_client = value;
			}
		}
		public string PMResource{
			get{
				return _pmResource.Trim();
			}
			set{
				SetValue(ref _pmResource, value);
			}
		}
		public string WebResource{
			get{
				return _webResource.Trim().ToLower();
			}
			set{
				SetValue(ref _webResource, value);
			}
		}
		public string DBResource{
			get{
				return _dbResource.Trim().ToLower();
			}
			set{
				SetValue(ref _dbResource, value);
			}
		}
		public string PMResourceUserName{
			get{
				return EmailValidator.EmailToUser(this.PMResource);
			}
		}
		public string WebResourceUserName{
			get{
				return EmailValidator.EmailToUser(this.WebResource);
			}
		}
		public string DBResourceUserName{
			get{
				return EmailValidator.EmailToUser(this.DBResource);
			}
		}
		public string MaxResourceUserName{
			get{
				return EmailValidator.EmailToUser(this.MaxResource);
			}
		}

		public bool UATApproved{
			get{
				return _uatApproved;
			}
			set{
				SetValue(ref _uatApproved, value);
			}
		}
		public string SubmitedBy{
			get{
				return _submitedBy.Trim();
			}
			set{
				SetValue(ref _submitedBy, value);
			}
		}
		public string ProjectManager{
			get{
				return _projectManager.Trim();
			}
			set{
				SetValue(ref _projectManager, value);
			}
		}
		public Times Times{
			get{
				_times = new Times(this);
				return _times;
			}
		}
		public string UATApprovedBy{
			get{
				return _uatApprovedBy;
			}
			set{
				SetValue(ref _uatApprovedBy, value);
			}
		}
		public string BranchFileHierarchy{
			get{
				return _branchFileHierarchy.Trim();
			}
			set{
				SetValue(ref _branchFileHierarchy, value);
			}
		}
		public string Environment{
			get{
				return _environment.ToLower().Trim();
			}
			set{
				SetValue(ref _environment, value);
			}
		}
		public DateTime[] FutureScheduledRollDates{
			get {
				ArrayList dates = new ArrayList();
				DateTime[] ret;
				foreach(EffortRollout er in this.EffortRollouts){
					dates.Add(er.Rollout.ScheduledDate);
				}
				ret = new DateTime[dates.Count];
				for(int i=0; i< dates.Count; i++){
					ret[i] = (DateTime)dates[i];
				}
				return ret;
			}
		}
		public DateTime RollDate{
			get{
				foreach(EffortRollout er in this.GetCachedEffortRollouts(false)){
					if (er.Rolled){
						return er.Rollout.RolledDate;
					}
				}
				return DateTime.MinValue;
			}
		}
		public int PriorityID{
			get{
				return _priorityID;
			}
		}

		public string DevelopersUserNames_CommaSeperated{
			get{
				string ret = "";
				if (this.DBResource != "")
					ret = this.DBResourceUserName;
				if (this.MaxResource != ""){
					if (ret != "")
						ret += ", " + this.MaxResourceUserName;
					else
						ret = this.MaxResourceUserName;
				}
				if (this.WebResource != ""){
					if (ret != "")
						ret += ", " + this.WebResourceUserName;
					else
						ret = this.WebResourceUserName;
				}
				return ret;
			}
		}
		public string Branches_CommaSeperated{
			get{
				string ret = "";
				foreach(string line in this.BranchFileHierarchy.Split(new char[2]{'\r', '\n'})){
					if (line.Trim() == "") continue;
					if (line.StartsWith("\t")) continue;
					if (ret == "")
						ret = line.Trim();
					else
						ret += ", " + line.Trim();
				}
				return ret;
			}
		}
		public string Requester{
			get{
				if (_requester == "" && this.IsTicket)
					_requester = this.RequesterFromTicketServer;
				return _requester;
			}
			set{
				SetValue(ref _requester, value);
			}
		}
		public string RequesterFromTicketServer{
			get{
				if (this.IsTicket){
					using(MySqlConnection conn = Connections.Inst.item("HTS").MySqlConnection){
						conn.Open();
						MySqlCommand cmd = conn.CreateCommand();
						cmd.CommandText =	 @"SELECT Users.Name
															FROM Tickets
															INNER JOIN TicketContacts
																ON Tickets.ID = TicketContacts.TicketID
															INNER JOIN Users
																ON Users.ID = TicketContacts.UserID
															WHERE Tickets.ID =  @TICKETNUMBER and TicketContacts.ContactTypeID = " + (int) ContactTypes.Reporter;
						cmd.Parameters.Add("@TICKETNUMBER", this.ExtId);
						return (string)cmd.ExecuteScalar() + "@directalliance.com";
					}
				}else{
					throw new Exception("Property not available when effort is ticket");
				}
			}
		}
		public string RequestorUserName{
			get{
				return EmailValidator.EmailToUser(this.Requester);
			}
		}
		public bool IsTicket{
			get{
				return this._effId.Type == QED.Business.EffortType.Ticket;
			}
		}
		public bool IsProject{
			get{
				return this._effId.Type == QED.Business.EffortType.Project;
			}
		}
		#endregion
		#region Validation Management
		public override bool IsValid {
			get {
				return (this.BrokenRules.Count == 0);
			}
		}
		public override BrokenRules BrokenRules {
			get {
				EmailValidator eValid = new EmailValidator();
				BrokenRules br = new BrokenRules();
				bool foundLineThatContainsData = true;
				int tabedLineCount = 0;
				string line;
				if (this._messages != null)
					br.Add(this._messages.BrokenRules);
				if (this._defects != null)
					br.Add(this._defects.BrokenRules);
				br.Assert("Tested By is not a valid email address", this.TestedBy != "" && !eValid.Parse(this.TestedBy));
				br.Assert("DB Resource is not a valid email address", this.DBResource != "" && !eValid.Parse(this.DBResource));
				br.Assert("Web Resource is not a valid email address", this.WebResource != "" && !eValid.Parse(this.WebResource));
				br.Assert("PM Resource is not a valid email address", this.PMResource != "" && !eValid.Parse(this.PMResource));
				br.Assert("Max Resource is not a valid email address", this.MaxResource != "" && !eValid.Parse(this.MaxResource));
				br.Assert("Requester is not a valid email address", this.Requester != "" && !eValid.Parse(this.Requester));
				foreach(string rawLine in this.BranchFileHierarchy.Split(new char[2]{'\r', '\n'})){
					line = rawLine.Trim();
					if (line.StartsWith(" ")){
						br.Add("Branch File Hierarchy is in incorrect format. Can't start lines with spaces. Only use tabs. Found at " + line);
					}
					foundLineThatContainsData = (line != "");
					if (rawLine.StartsWith("\t")) tabedLineCount++;
				}
				if (this.BranchFileHierarchy.Length != 0 &&  foundLineThatContainsData && tabedLineCount == 0){
					br.Add("In the Branch File Hierarchy a line was found (maybe a brach name) but no indented lines (file paths) were found");
				}
				return br;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return base.ToString();
		}
		#endregion
	}
}