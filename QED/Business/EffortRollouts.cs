using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
namespace QED.Business{
	public class EffortRollouts : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "effortRollouts";
		MySqlDBLayer _dbLayer;	
		BusinessBase _parent;
		#region Collection Members
		public EffortRollout Add(EffortRollout obj) {
			obj.BusinessCollection = this;
			if (this.IsChildOfRollout){
				obj.Rollout = (Rollout)this.Parent;
			}
			if (this.IsChildOfEffort){
				obj.Effort = (Effort)this.Parent;
			}
			List.Add(obj); return obj;
		}
		public bool IsChildOfEffort{ 
			get{
				return (this.Parent is Effort);
			}
		}
		public bool IsChildOfRollout{ 
			get{
				return (this.Parent is Rollout);
			}
		}
		public bool Contains(EffortRollout obj) {
			foreach(EffortRollout child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public EffortRollout this[int id] {
			get{
				return (EffortRollout) List[id];
			}
		}
		public EffortRollout item(int id) {
			foreach(EffortRollout obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		public EffortRollout item(Effort eff) {
			foreach(EffortRollout obj in List) {
				if (obj.Effort.Id == eff.Id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public EffortRollouts() {
		}
		public EffortRollouts(Effort parent) {
			EffortRollout obj;
			this.Parent = parent;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "effId", parent.Id)){
					while(dr.Read()) {
						obj = new EffortRollout(dr);
						obj.BusinessCollection = this;
						List.Add(obj);
					}
				}
			}
		}
		public EffortRollouts(Rollout parent) {
			EffortRollout obj;
			this.Parent = parent;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "rollId", parent.Id)){
					while(dr.Read()) {
						obj = new EffortRollout(dr);
						obj.BusinessCollection = this;
						List.Add(obj);
					}
				}
			}
		}

		public void Update() {
			foreach (EffortRollout obj in List) {
				obj.Update();
			}
		}
		#endregion
		#region Business Members
		public Rollouts Rollouts{
			get{
				Rollouts rolls = new Rollouts();
				foreach(EffortRollout effRoll in this){
					rolls.Add(effRoll.Rollout);
				}
				return rolls;
			}
		}
		public Efforts Efforts{
			get{
				Efforts effs = new Efforts();
				foreach(EffortRollout effRoll in this){
					effs.Add(effRoll.Effort);
				}
				return effs;
			}
		}
		public BusinessBase Parent{
			get{
				return _parent;
			}
			set{
				_parent = value;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class EffortRollout : BusinessBase  {
		#region Instance Data
		string _table = "effortRollouts";
		int _id;
		MySqlDBLayer _dbLayer;	
		BusinessCollectionBase _businessCollection;
		Effort _effort = null;
		Rollout _rollout = null;
		int _effortId = -1;
		int _rolloutId = -1;
		string _text = "";
		bool _rolled = false;
		string _reasonForRollBack = "";
		string _reasonForCodeFix  = "";
		int _codeFixed = -1;
		string _finalComments = "";
		Group _departmentResponsibleForError = null;
		int _departmentResponsibleForErrorId = -1;
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
		public EffortRollout() {
			Setup();
			base.MarkNew();
		}
		public EffortRollout(int id) {
			Setup();
			this.Load(id);
		}
		public EffortRollout(MySqlDataReader dr) {
			Setup();
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			using(MySqlConnection conn = (MySqlConnection) this.Conn){
				conn.Open();
				MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", conn);
				cmd.Parameters.Add("@Id", this.Id);
				using(MySqlDataReader dr =  cmd.ExecuteReader()){
					if (dr.HasRows) {
						dr.Read();
						this.Load(dr);
					}else{
						throw new Exception("EffortRollout " + id + " doesn't exist.");
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			SetId(Convert.ToInt32(dr["Id"]));
			this._effortId = Convert.ToInt32(dr["effId"]);
			this._rolloutId = Convert.ToInt32(dr["rollId"]);
			this._rolled = Convert.ToBoolean(dr["rolled"]);
			this._text = Convert.ToString(dr["text"]);
			this._reasonForCodeFix = Convert.ToString(dr["reasonForCodeFix"]);
			this._reasonForRollBack = Convert.ToString(dr["reasonForRollBack"]);
			if (dr["wasCodeFixed"] == DBNull.Value)
				this._codeFixed = -1;
			else
				this._codeFixed = Convert.ToInt32(dr["wasCodeFixed"]);

			this._departmentResponsibleForErrorId = Convert.ToInt32(dr["departmentResponsibleForErrorId"]);
			this._finalComments= Convert.ToString(dr["finalComment"]);
			MarkOld();
		}
		
		public override void Update(){
			_dbLayer.Update();
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@Id", this.Id);
				paramHash.Add("@effId", this._effortId);
				paramHash.Add("@rollId", this._rolloutId);
				paramHash.Add("@rolled", this.Rolled);
				paramHash.Add("@text", this.Text);
				paramHash.Add("@reasonForCodeFix", this.ReasonForCodeFix);
				paramHash.Add("@reasonForRollBack", this.ReasonForRollBack);
				paramHash.Add("@wasCodeFixed", this.CodeFixed);
				paramHash.Add("@departmentResponsibleForErrorId", _departmentResponsibleForErrorId);
				paramHash.Add("@finalComment", this.FinalComments);
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
				if (_effort == null && _effortId != -1)
					_effort = new Effort(_effortId, true);
				return _effort;
			}
			set{
				BusinessBase bb = (BusinessBase)_effort;
				SetValue(ref bb, value, ref _effortId);
			}
		}
		public Rollout Rollout{
			get{
				if (_rollout == null && _rolloutId != -1)
					_rollout = new Rollout(_rolloutId);
				return _rollout;
			}
			set{
				BusinessBase bb = (BusinessBase)_rollout;
				SetValue(ref bb, value, ref _rolloutId);
			}
		}
		public int RolloutId{
			get{
				return _rolloutId;
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
		public bool Rolled{
			get{
					return _rolled;
			}
			set{
				SetValue(ref _rolled, value);
			}
		}
		public string ReasonForRollBack{
			get{
				return _reasonForRollBack;
			}
			set{
				SetValue(ref _reasonForRollBack, value);
			}
		}
		public string ReasonForCodeFix{
			get{
				return _reasonForCodeFix;
			}
			set{
				SetValue(ref _reasonForCodeFix, value);
			}
		}
		public int CodeFixed{
			get{
				return _codeFixed;
			}
			set{
				if (value < -1 || value >1) throw new Exception("EffortRollouts.CodeFixed is a tristate property. The only allowable values are -1, 0 and 1");
				SetValue(ref _codeFixed, value);
			}
		}
		public string CodeFixedYesNo{
			get{
				if (this.CodeFixed == 1) return "Yes";
				if (this.CodeFixed == 0) return "No";
				return "";
			}
			set{
				switch(value.ToUpper()){
					case "YES":
						this.CodeFixed = 1;
						break;
					case "NO":
						this.CodeFixed = 0;
						break;
					default:
						this.CodeFixed = -1;
						break;
				}
			}
		}
		public Group DepartmentResponsibleForError{
			get{
				if (_departmentResponsibleForError == null && _departmentResponsibleForErrorId != -1)
					_departmentResponsibleForError = Groups.Inst.item(_departmentResponsibleForErrorId );
				return _departmentResponsibleForError;
			}
			set{
				_departmentResponsibleForErrorId = value.Id;
				_departmentResponsibleForError = (Group)SetValue(value);
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

		#endregion
		#region Validation Management
		public override bool IsValid {
			get {
				return (this.BrokenRules.Count == 0);
			}
		}
		public override BrokenRules BrokenRules {
			get {
				BrokenRules br = new BrokenRules();
				br.Assert("Effort ID == -1", _effortId == -1);
				br.Assert("Rollout ID = -1", _rolloutId == -1);
				if (this.Rolled){
					foreach(EffortRollout effRoll in this.Effort.EffortRollouts){
						if (effRoll.Id != this.Id && effRoll.Rolled){
							br.Add("Effort: " + effRoll.Effort.ConventionalId + " has already been rolled. See rollout: " + effRoll.Rollout.ToString());
						}
					}
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


