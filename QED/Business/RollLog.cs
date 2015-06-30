using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
using QED.Business.CodePromotion;
namespace QED.Business{
	public class RolloutLogs : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "rollLogs";
		MySqlDBLayer _dbLayer;	
		#region Collection Members
		public RolloutLog Add(RolloutLog obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		
		public bool Contains(RolloutLog obj) {
			foreach(RolloutLog child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public RolloutLog this[int id] {
			get{
				return (RolloutLog) List[id];
			}
		}
		public RolloutLog item(int id) {
			foreach(RolloutLog obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public RolloutLogs(string rollerClass, string rollType) {
			RolloutLog log;
			using (MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open();
				MySqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = "SELECT " + AllColumnsExceptText + " FROM " + _table + " WHERE rollClass = @RollClass and rollType = @RollType ORDER BY ts, rollClass, rollType";
				cmd.Parameters.Add("@RollClass", rollerClass);
				cmd.Parameters.Add("@RollType", rollType);
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					while(dr.Read()){
						log = new RolloutLog(dr);
						this.Add(log);
					}
				}
			}
		}
		public void Update() {
			foreach (RolloutLog obj in List) {
				obj.Update();
			}
		}
		#endregion
		#region Business Members
		public string AllColumnsExceptText{
			get{
				return "id, userEmail, rollClass, rollType, ts";
				}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class RolloutLog : BusinessBase  {
		#region Instance Data
		string _table = "rollLogs";
		int _id = -1;
		string _text = "";
		string _userEmail = "";
		string _rollClass = "";
		RollType _rollType;
		MySqlDBLayer _dbLayer;	
		DateTime _dateTime;
		BusinessCollectionBase _businessCollection;
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
		public RolloutLog() {
			Setup();
			base.MarkNew();
		}
		public RolloutLog(int id) {
			Setup();
			this.Load(id);
		}
		
		public RolloutLog(MySqlDataReader dr) {
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			MySqlConnection conn = (MySqlConnection) this.Conn;
			conn.Open();
			using(MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", conn)){
				cmd.Parameters.Add("@Id", this.Id);
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					if (dr.HasRows) {
						dr.Read();
						this.Load(dr);
					}else{
						throw new Exception("RolloutLog " + id + " doesn't exist.");
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			if (dr.GetSchemaTable().Select("ColumnName = 'text'").Length == 1)  // Check the schema to determine if the "text" column is in it.
				this._text = Convert.ToString(dr["text"]);
			this._userEmail = Convert.ToString(dr["userEmail"]);
			this._rollClass = Convert.ToString(dr["rollClass"]);
			this._rollType = (RollType)Enum.Parse(Type.GetType("QED.Business.CodePromotion.RollType"), Convert.ToString(dr["rollType"]));
			if (dr["ts"] != DBNull.Value)
				this._dateTime = Convert.ToDateTime(dr["ts"]);
			MarkOld();
		}
		
		public override void Update(){
			_dbLayer.Update();
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@text", this.Text);
				paramHash.Add("@userEmail", this.UserEmail);
				paramHash.Add("@rollClass", this.RollClass);
				paramHash.Add("@rollType", this.RollType.ToString());
				paramHash.Add("@ts", DBNull.Value);
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
		public string Text{
			get{
				if (_text == "" && !this.IsNew)
					this.Load(this.Id); // Reload. The object might not have been loaded with "text" data for performance purposes.
				return _text;
			}
			set{
				SetValue(ref _text, value);
			}
		}
		public string UserEmail{
			get{
				return _userEmail;
			}
			set{
				SetValue(ref _userEmail, value);
			}
		}
		public void Append(string msg){
			if (this.Text != "")
				this.Text += "\r\n" + msg;
			else
				this.Text += msg;
		}
		public string RollClass{
			get{
				return _rollClass;
			}
			set{
				SetValue(ref _rollClass, value);
			}
		}
		public RollType RollType{
			get{
				return _rollType;
			}
			set{
				if (_rollType != value){
					_rollType = value;
					MarkDirty();
				}
			}
		}
		public DateTime DateTime{
			get{
				return _dateTime;
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
				return br;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.RollClass + " " + this.RollType.ToString() + " " + this.DateTime.ToShortDateString() + " " + this.DateTime.ToShortTimeString();
		}
			#endregion
	}
}


