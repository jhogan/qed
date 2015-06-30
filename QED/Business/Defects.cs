using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
namespace QED.Business{
	/// <summary>
	/// Summary description for defects.
	/// </summary>
	public enum DefectStatus  { 
		//TODO Get status codes
		a, b
	}
	public class Defects : BusinessCollectionBase {
		const string  _table = "defects";
		Effort _eff;
		BusinessBase _parent;
		#region Collection Members
		public Defect Add(Defect def) {
			def.BusinessCollection = this;
			def.Parent = this._parent;
			List.Add(def); return def;
		}
		
		public bool Contains(Defect def) {
			foreach(Defect obj in List) {
				if (obj.Equals(def)){
					return true;
				}
			}
			return false;
		}
		public Defect this[int id] {
			get{
				return (Defect) List[id];
			}
		}
		public Defect item(int id) {
			foreach(Defect obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public Defects() {
		}
		public Defects(BusinessBase parent) {
			Defect def;
			string SQL;
			_parent = parent;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				using(MySqlCommand cmd = conn.CreateCommand()){
					if (parent is Effort){
						SQL = "SELECT * FROM " + _table + " WHERE effId = @effId and forRoll = 0";
						cmd.Parameters.Add("@effId", parent.Id);
					}else{
						SQL = "SELECT * FROM " + _table + " WHERE rollId = @rollId and forRoll = 1";
						cmd.Parameters.Add("@rollId", parent.Id);
					}
					cmd.CommandText = SQL;
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						while(dr.Read()) {
							def = new Defect(dr);
							def.BusinessCollection = this;
							List.Add(def);
						}
						conn.Close();
					}
				}
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
		public void Update() {
			foreach (Defect def in List) {
				def.Update();
			}
		}
		#endregion
		#region Validation Management

		#endregion
	}
	public class Defect : BusinessBase  {
		#region Instance Data
		int _id = -1;
		string _desc = "";
		int _effId = -1;
		int _rollId = -1;
		DefectStatus _status;
		const string  _table = "defects";
		MySqlDBLayer _dbLayer;
		BusinessCollectionBase _businessCollection;
		string _createdBy = "";
		string _resolver = "";
		Effort _effort;
		Rollout _rollout;
		BusinessBase _parent;
		bool _forRoll = false;
		#endregion
		#region DB Access / ctors
        public Defect() {
			Setup();
			base.MarkNew();
		}
		public Defect(string desc) {
			Setup();
			_desc = desc;
			base.MarkNew();
		}
		public Defect(int id) {
			Setup();
			this.Load(id);
		}
		public override void SetId(int id) {
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
		public override string Table {
			get {
				return _table;
			}
		}
		private void Setup() {
			if (_dbLayer == null) {
				 _dbLayer = new MySqlDBLayer(this);
			}
		}
		public Defect(MySqlDataReader dr) {
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
						throw new Exception("Defect " + id + " doesn't exist.");
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["id"]));
			this._desc = dr["desc_"].ToString();
			this._effId = Convert.ToInt32(dr["effId"]);
			this._rollId =  Convert.ToInt32(dr["rollId"]);
			this._resolver = dr["resolver"].ToString();
			this._createdBy = dr["createdBy"].ToString();
			//this._status = dr["Status"].ToString();
			this._forRoll = Convert.ToBoolean(dr["forRoll"]);
			MarkOld();
		}
		
		public  override void Update(){
			_dbLayer.Update();
		}
		public override object Conn {
			get {
				return Connections.Inst.item("QED_DB").MySqlConnection;
			}
		}
		public override Hashtable ParamHash {
			get {
					Hashtable paramHash = new Hashtable();
					paramHash.Add("@Desc_", this.Desc);
					paramHash.Add("@RollId", this._rollId);
					paramHash.Add("@EffId", this._effId);
					paramHash.Add("@CreatedBy", this.CreatedBy);
					paramHash.Add("@Resolver", this.Resolver);
					paramHash.Add("@forRoll", this.ForRoll);
					return paramHash;
				}
		}
		#endregion
		#region Business Properties
		
		public override int Id {
			get {
				return _id;
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
		public DefectStatus Status {
			get {
				return DefectStatus.a;
			}
		}
		public Effort Effort{
			get{
				if (_effort == null){
					if (_effId != -1){
						_effort = new Effort(_effId, false);
						return _effort;
					}else{
						return null;
					}
				}else{
					return _effort;
				}
			}
			set{
				_effort = (Effort) SetValue(value);
				_effId = _effort.Id;
			}
		}
		public Rollout Rollout{
			get{
				if (_rollout == null){
					if (_rollId != -1){
						_rollout = new Rollout(_rollId);
						return _rollout;
					}else{
						return null;
					}
				}else{
					return _rollout;
				}
			}
			set{
				_rollout = (Rollout) SetValue(value);
				_rollId = _rollout.Id;
			}
		}
		public string CreatedBy{
			get{
				return _createdBy;
			}
			set{
				SetValue(ref _createdBy, value);
			}
		}
		public string Resolver{
			get{
				return _resolver;
			}
			set{
				SetValue(ref _resolver, value);
			}
		}
		public BusinessBase Parent{
			get{
				return _parent;
			}
			set{
				if (value is Effort) {
					this.Effort = (Effort)value;
				}else{
					this.Rollout = (Rollout)value;
				}
			}
		}

		public bool ForRoll{
			get{
				return _forRoll;
			}
			set{
				SetValue(ref _forRoll, value);
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
				br.Assert("NO_DESC", "Must provide a description", this.Desc == "");
				br.Assert("NO_PARENT", "No parent specified", this._effId== -1 && this._rollId == -1);
				br.Assert("NO_CREATOR", "No creator specified", this.CreatedBy == "");
				return br;
			}
		}
		#endregion
	}
}

