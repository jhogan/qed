using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
using QED.DataValidation;
namespace QED.Business{
	public enum SearchBy {
		RollDate, ScheduledDate
	}
	public class Rollouts : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "roll";
		MySqlDBLayer _dbLayer;	
		#region Collection Members
		public Rollout Add(Rollout obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		public bool Contains(Rollout obj) {
			foreach(Rollout child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Rollout this[int id] {
			get{
				return (Rollout) List[id];
			}
		}
		public Rollout item(int id) {
			foreach(Rollout obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public object Conn {
			get {
				return Connections.Inst.item("QED_DB").MySqlConnection;
			}
		}
		public Rollouts() {
		}
		public Rollouts(Client client, DateTime date, SearchBy searchBy) {
			MySqlCommand cmd;
			Rollout roll;
			string dateField = (searchBy == SearchBy.RollDate) ? "rolledDate" : "scheduledDate";
			date = date.Date; // Store date part, time part not needed.
			using(MySqlConnection conn = (MySqlConnection)this.Conn){
				conn.Open();
				cmd = conn.CreateCommand();
				cmd.CommandText = "SELECT * FROM " + _table + " WHERE clientId = @clientId AND " + dateField + " = @" + dateField;
				cmd.Parameters.Add("@clientId", client.Id);
				cmd.Parameters.Add("@" + dateField, date);
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					if(!dr.HasRows && dateField == "scheduledDate"){
						roll = new Rollout();
						roll.Client = client;
						roll.ScheduledDate = date;
						roll.BusinessCollection = this;
						List.Add(roll);
					}else{
						while (dr.Read()){
							roll = new Rollout(dr);
							roll.BusinessCollection = this;
							List.Add(roll);
						}
					}
				}
			}
		}
		public void Update() {
			foreach (Rollout obj in List) {
				obj.Update();
			}
		}
		public static Rollouts GetAllUnrolled(string ORDER_BY){
			Rollouts rollouts = new Rollouts();
			Rollout rollout;
			using (MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				using(MySqlCommand cmd = conn.CreateCommand()){
					cmd.CommandText = "SELECT * FROM " + _table + " WHERE rolled = 0" + ((ORDER_BY.Trim().Length == 0) ? "" : " ORDER BY " + ORDER_BY);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						while(dr.Read()){
							rollout = new Rollout(dr);
							rollouts.Add(rollout);
						}
					}
				}
			}
			return rollouts;
		}
		public static Rollouts Get(DateTime from, DateTime to, string orderBy){
			Rollouts rollouts = new Rollouts();
			Rollout rollout;
			using (MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				using(MySqlCommand cmd = conn.CreateCommand()){
					cmd.CommandText = "SELECT * FROM " + _table + " WHERE scheduledDate BETWEEN @FROM AND @TO ORDER BY " + orderBy; 
					cmd.Parameters.Add("@FROM", from);
					cmd.Parameters.Add("@TO", to);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						while(dr.Read()){
							rollout = new Rollout(dr);
							rollouts.Add(rollout);
						}
					}
				}
			}
			return rollouts;
		}
		public static Rollouts Get(DateTime from, DateTime to, bool rolled, string orderBy){
			Rollouts rollouts = new Rollouts();
			Rollout rollout;
			using (MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				using(MySqlCommand cmd = conn.CreateCommand()){
					cmd.CommandText = "SELECT * FROM " + _table + " WHERE scheduledDate BETWEEN @FROM AND @TO AND rolled = @ROLLED ORDER BY " + orderBy; 
					cmd.Parameters.Add("@FROM", from);
					cmd.Parameters.Add("@TO", to);
					cmd.Parameters.Add("@ROLLED", rolled);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						while(dr.Read()){
							rollout = new Rollout(dr);
							rollouts.Add(rollout);
						}
					}
				}
			}
			return rollouts;
		}
		#endregion
		#region Business Members
		public Times GetTimes(string userEmail){
			Times times = new Times();
			foreach(Rollout roll in this){
				foreach(Time time in roll.Times){
					if (time.User == userEmail){
						times.Add(time);
					}
				}
			}
			return times;
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Rollout : BusinessBase  {
		#region Instance Data
		string _table = "roll";
		int _clientId = -1;
		DateTime _scheduledDate = DateTime.MinValue;
		DateTime _rolledDate  = DateTime.MinValue;
		bool _rolled = false;
		bool _rolledBack= false;
		string _finalComments = "";
		Defects _defects;
		Messages _messages;
		MySqlDBLayer _dbLayer;	
		int _id = -1;
		Client _client;
		BusinessCollectionBase _businessCollection;
		string _rolledBy = "";
		Times _times;
		EffortRollouts _effortRollouts;
		Efforts _cachedEfforts = null;
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
		public Rollout() {
			Setup();
			base.MarkNew();
		}

		public Rollout(int id) {
			Setup();
			this.Load(id);
		}
		public Rollout(MySqlDataReader dr) {
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			using(MySqlConnection conn = (MySqlConnection) this.Conn){
				conn.Open();
				MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", conn);
				cmd.Parameters.Add("@Id", this.Id);
				using(MySqlDataReader dr = cmd.ExecuteReader()){

					if (dr.HasRows) {
						dr.Read();
						this.Load(dr);
					}else{
						throw new Exception("Rollout " + id + " doesn't exist.");
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			this._clientId = Convert.ToInt32(dr["clientId"]);
			this._finalComments= Convert.ToString(dr["finalComments"]);
			this._rolledBack = Convert.ToBoolean(dr["rolledBack"]);
			this._rolledDate= Convert.ToDateTime(dr["rolledDate"]);
			this._scheduledDate = Convert.ToDateTime(dr["scheduledDate"]);
			this._rolledBy = Convert.ToString(dr["rolledBy"]);
			this.Rolled = Convert.ToBoolean(dr["rolled"]);
			MarkOld();
		}
		
		public override  void Update(){
			_dbLayer.Update();
			if (_effortRollouts != null)
				_effortRollouts.Update();
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@ClientId", this._clientId);
				paramHash.Add("@FinalComments", this.FinalComments);
				paramHash.Add("@RolledBack", this.RolledBack);
				paramHash.Add("@RolledDate", this.RolledDate);
				paramHash.Add("@ScheduledDate", this.ScheduledDate);
				paramHash.Add("@rolledBy", this.RolledBy);
				paramHash.Add("@rolled", this.Rolled);
				return paramHash;
			}
		}
		#endregion
		#region Business Properties
		public override int Id{
			get {
				return _id;
			}
		}
		public int ClientId{
			get{
				return _clientId;
			}
			set{
				SetValue(ref _clientId, value);
			}
		}
		public DateTime ScheduledDate{
			get{
				return _scheduledDate;
			}
			set{
				SetValue(ref _scheduledDate, value);
			}
		}
		public DateTime RolledDate{
			get{
				return _rolledDate;
			}
			set{
				SetValue(ref _rolledDate, value);
			}
		}
		public bool Rolled{
			get{
				return _rolled;
			}
			set{
				SetValue(ref _rolled, value);
			}
		}
		public bool RolledBack{
			get{
				return _rolledBack;
			}
			set{
				SetValue(ref _rolledBack, value);
			}
		}

		public string FinalComments{
			get{
				return _finalComments;
			}
			set{
				SetValue(ref _finalComments, value);
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
		public Client Client{
			get{
				if (_client == null)
					_client = new Client(_clientId);
				return _client;
			}
			set{
				_client = (Client)SetValue(value);
				_clientId = _client.Id;
			}
		}
		public EffortRollouts EffortRollouts{
			get{
				if (_effortRollouts == null)
					_effortRollouts = new EffortRollouts(this);
				return _effortRollouts;
			}
		}
		public EffortRollout GetEffortRollout(Effort eff){
			foreach (EffortRollout er in this.EffortRollouts){
				if (er.Effort.Id == eff.Id){
					return er;
				}
			}
			return null;
		}
		public Efforts Efforts{
			get{
				return this.EffortRollouts.Efforts;
			}
		}
		public Efforts GetCachedEfforts(bool reload){
			if (reload || _cachedEfforts == null)
				_cachedEfforts = this.Efforts;
			return _cachedEfforts;
		}
		public Efforts RolledEfforts{
			get{
				Efforts effs = new Efforts();
				foreach (EffortRollout effRoll in this.EffortRollouts){
					if (effRoll.Rolled)
						effs.Add(effRoll.Effort);
				}
				return effs;
			}
		}
		public Efforts UnrolledEfforts{
			get{
				Efforts effs = new Efforts();
				foreach (EffortRollout effRoll in this.EffortRollouts){
					if (!effRoll.Rolled)
						effs.Add(effRoll.Effort);
				}
				return effs;
			}			
		}
		public void AddUnrolled(Effort eff){
			EffortRollout effRoll = new EffortRollout();
			effRoll.Effort = eff;
			_effortRollouts.Add(effRoll);
		}
		public void Delete(Effort eff){
			EffortRollout effRoll = _effortRollouts.item(eff);
			effRoll.Delete();
		}
		public void Roll(Effort eff){
			_effortRollouts.item(eff).Rolled = true;
		}
		public void Unroll(Effort eff){
			_effortRollouts.item(eff).Rolled = false;
		}
		public void ToggleRolledState(Effort eff){
			EffortRollout effRoll =_effortRollouts.item(eff);
			effRoll.Rolled = !effRoll.Rolled;
		}
		public string RolledBy{
			get{
				return _rolledBy;
			}
			set{
				SetValue(ref _rolledBy, value);
			}
		}
		public Times Times{
			get{
				_times = new Times(this);
				return _times;
			}
		}
		public Defects GetDefects(Effort eff){
			Defects ret = new Defects();
			ret.Parent = this;
			foreach(Defect def in this.Defects){
				if (def.Effort.Id == eff.Id){
					ret.Add(def);
				}
			}
			return ret;
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
				br.Assert("A rollout can't be rolled and rolledback at the same time", this.Rolled && this.RolledBack);
				br.Assert("\"Rolled By\" is not a valid email address", this.RolledBy != "" && !eValid.Parse(this.RolledBy));
				br.Add(this._effortRollouts);
				return br;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return "Client:\"" + this.Client.Name + "\"  Scheduled:" + this.ScheduledDate.ToShortDateString();
		}
		#endregion
	}
}


