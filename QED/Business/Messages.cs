using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
namespace QED.Business{
	public class Messages : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "msgs";
		MySqlDBLayer _dbLayer;	
		BusinessBase _parent;
		#region Collection Members
		public Message Add(Message obj) {
			obj.BusinessCollection = this;
			obj.Parent = this._parent;
			List.Add(obj); return obj;
		}
		public bool Contains(Message obj) {
			foreach(Message child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Message this[int id] {
			get{
				return (Message) List[id];
			}
		}
		public Message item(int id) {
			foreach(Message obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public Messages() {
		}
		public Messages(BusinessBase parent) {
			Message msg;
			int fkVal; string fk;
			_parent = parent;
			if (parent is Effort){
				fk = "effId";
				fkVal = parent.Id;
			}else{
				fkVal = parent.Id;
				fk = "rollId";
			}
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, fk, fkVal)){
					while(dr.Read()) {
						msg = new Message(dr);
						msg.BusinessCollection = this;
						List.Add(msg);
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
			foreach (Message obj in List) {
				obj.Update();
			}
		}
		#endregion
		#region Business Members
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Message : BusinessBase  {
		#region Instance Data
		string _table = "msgs";
		MySqlDBLayer _dbLayer;	
		string _text = "";
		int _effId = -1;
		int _rollId = -1;
		string _createdBy = "";
		int _id = -1;
		BusinessCollectionBase _businessCollection;
		Rollout _rollout;
		Effort _effort;
		BusinessBase _parent;
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
		public Message(string text) {
			this.Text = text;
			Setup();
			base.MarkNew();
		}
		public Message(int id) {
			Setup();
			this.Load(id);
		}
		public Message(MySqlDataReader dr) {
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			using(MySqlConnection conn = (MySqlConnection) this.Conn){
				conn.Open();
				using(MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", conn)){
					cmd.Parameters.Add("@Id", this.Id);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						if (dr.HasRows) {
							dr.Read();
							this.Load(dr);
						}else{
							throw new Exception("Message " + id + " doesn't exist.");
						}
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			this._text = Convert.ToString(dr["text"]);
			this._effId = Convert.ToInt32(dr["effId"]);
			this._rollId = Convert.ToInt32(dr["rollId"]);
			this._createdBy = Convert.ToString(dr["createdBy"]);
			MarkOld();
		}
		
		public override void Update(){
			_dbLayer.Update();
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@text", this.Text);
				paramHash.Add("@effId", this._effId);
				paramHash.Add("@rollId", this._rollId);
				paramHash.Add("@createdBy", this.CreatedBy);
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
		public string Text{
			get{
				return _text;
			}
			set{
				SetValue(ref _text, value);
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
				br.Assert("NO_TEXT", "Text property empty", this.Text == "");
				br.Assert("NO_PARENT", "No parent specified", this._effId== -1 && this._rollId == -1);
				br.Assert("NO_CREATOR", "No creator specified", this.CreatedBy == "");
				return br;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
}


