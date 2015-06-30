using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using System.Threading;
using JCSLA;
using System.Timers;
namespace QED.Business{
	public class Times : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "times";
		MySqlDBLayer _dbLayer;	
		BusinessBase _parent;
		#region Collection Members
		public Time Add(Time obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		
		public bool Contains(Time obj) {
			foreach(Time child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Time this[int id] {
			get{
				return (Time) List[id];
			}
		}
		public Time item(int id) {
			foreach(Time obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		public Time item(Rollout rollout) {
			foreach(Time obj in List) {
				if (obj.ForRollout){
					if (rollout.Id == obj.Rollout.Id){
						return obj;
					}
				}
			}
			return null;
		}
		public Time item(Effort eff) {
			foreach(Time obj in List) {
				if (obj.ForEffort){
					if (eff.Id == obj.Effort.Id){
						return obj;
					}
				}
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public Times() {
		}
		public Times(Effort parent) {
			Time obj;
			_parent = parent;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "effId", parent.Id)){
					while(dr.Read()) {
						obj = new Time(dr);
						obj.BusinessCollection = this;
						List.Add(obj);
					}
				}
			}
		}
		public Times(Rollout parent) {
			Time obj;
			_parent = parent;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "rollId", parent.Id)){
					while(dr.Read()) {
						obj = new Time(dr);
						obj.BusinessCollection = this;
						List.Add(obj);
					}
				}
			}
		}
		public BusinessBase Parent{
			get{
				return _parent;
			}
		}
		public void Update() {
			foreach (Time obj in List) {
				obj.Update();
			}
		}
		#endregion
		#region Business Members
		public int Total{
			get{
				int total = 0;
				foreach(Time time in this)
					total += time.Minutes;
				return total;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Time : BusinessBase  {
		#region Instance Data
		string _table = "times";
		int _id;
		MySqlDBLayer _dbLayer;	
		BusinessCollectionBase _businessCollection;
		Effort _effort; int _effId = -1;
		Rollout _roll; int _rollId = -1;
		string _text = "";
		string _user = "";
		int _minutes = 0;
		System.Timers.Timer _timer;
		DateTime _date = DateTime.MinValue;
		DateTime _timerStarted = DateTime.MinValue;
		public delegate void OnMinuteChangeHandler(Time time, EventArgs e);
		public OnMinuteChangeHandler OnMinuteChange;
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
		public Time() {
			Setup();
			base.MarkNew();
		}
		public Time(Rollout roll) {
			Setup();
			this.Rollout = roll;
			base.MarkNew();
		}
		public Time(Effort eff) {
			this.Effort = eff;
			Setup();
			base.MarkNew();
		}
		public Time(int id) {
			Setup();
			this.Load(id);
		}
		
		public Time(MySqlDataReader dr) {
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			using(MySqlConnection conn = (MySqlConnection) this.Conn)
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", (MySqlConnection) this.Conn);
			cmd.Parameters.Add("@Id", this.Id);
			using(MySqlDataReader dr = cmd.ExecuteReader()){
				if (dr.HasRows) {
					dr.Read();
					this.Load(dr);
				}else{
					throw new Exception("Time " + id + " doesn't exist.");
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			this._effId = Convert.ToInt32(dr["effId"]);
			this._rollId = Convert.ToInt32(dr["rollId"]);
			this._minutes = Convert.ToInt32(dr["minutes"]);
			this._date = Convert.ToDateTime(dr["date"]);
			this._user = Convert.ToString(dr["user"]);
			this._text = Convert.ToString(dr["text"]);
			MarkOld();
		}
		
		public override void Update(){
			_dbLayer.Update();
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@Id", this.Id);
				paramHash.Add("@effId", this._effId);
				paramHash.Add("@rollId", this._rollId);
				paramHash.Add("@minutes", this.Minutes);
				paramHash.Add("@date", this.Date);
				paramHash.Add("@user", this.User);
				paramHash.Add("@text", this.Text);
				return paramHash;
			}
		}
		#endregion
		#region Business Properties
		public override int Id{
			get{
				return _id;
			}
		}

		public Effort Effort{
			get{
				return _effort;
			}
			set{
				BusinessBase bb = (BusinessBase)_effort;
				SetValue(ref bb, (BusinessBase)value, ref _effId);
				_effort = (Effort)bb;
			}
		}
		public bool ForEffort{
			get{
				return (_effort != null && _roll == null);
			}
		}
		public Rollout Rollout{
			get{
				return _roll;
			}
			set{
				BusinessBase bb = (BusinessBase)_roll;
				SetValue(ref bb, (BusinessBase)value, ref _rollId);
				_roll = (Rollout)bb;
			}
		}
		public bool ForRollout{
			get{
				return (_effort == null && _roll != null);
			}
		}
		public string Text{
			get{
				return _text;
			}
			set{
				SetValue(ref _text, value);
			}
		}

		public int Minutes{
			get{
				return _minutes;
			}
			set{
				SetValue(ref _minutes, value);
			}
		}
		public DateTime Date{
			get{
				return _date;
			}
			set{
				SetValue(ref _date, value);
			}
		}
		public string User{
			get{
				return _user;
			}
			set{
				SetValue(ref _user, value);
			}
		}
		#endregion
		#region Timer Members
		public void StartTimer(){
			if (_timer == null){
				_timer = new System.Timers.Timer(1000);
				_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
			}
			_timer.Enabled = true;
			if (_timerStarted == DateTime.MinValue){
				_timerStarted = DateTime.Now;
			}
		}
		public void RestartTimer(){
			_timer.Enabled = true;
			this.Minutes = 0;
			_timerStarted = DateTime.Now;
		}
		public void StopTimer(){
			if (_timer != null){
				_timer.Enabled = false;
			}else{
				throw new Exception ("Can't stop timer because timer hasn't been instantiated");
			}
		}
		public void OnTimedEvent(object source, ElapsedEventArgs e){
			TimeSpan ts = e.SignalTime.Subtract(_timerStarted);
			int minutes = int.Parse(ts.TotalMinutes.ToString().Split('.')[0]);
			if (minutes != this.Minutes){
				this.Minutes = minutes;
				OnMinuteChange(this, new EventArgs());
			}
		}
		public bool IsTimerRunning{
			get{
				return (_timer != null && _timer.Enabled);
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
				System.Security.Principal.IPrincipal p = Thread.CurrentPrincipal;
				bool man = p.IsInRole("manager");
				bool admin = p.IsInRole("admin");
				BrokenRules br = new BrokenRules();
				br.Assert("Only managers or admins can edit time entries for other users.", this.User != p.Identity.Name && (!man && !admin));
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


