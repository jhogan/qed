using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
namespace QED.Business{
	public class Groups : BusinessCollectionBase {
		#region Instance Data
		private static object syncRoot = new Object();
		static Groups _instance;
		#endregion
		const string _table = "Groups";
		MySqlDBLayer _dbLayer;	

		#region Collection Members
		public Group Add(Group obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		
		public bool Contains(Group obj) {
			foreach(Group child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Group this[int id] {
			get{
				return (Group) List[id];
			}
		}
		public Group item(int id) {
			foreach(Group obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public static Groups Inst {
			get {
				if (_instance == null) {
					lock (syncRoot) {
						if (_instance == null) 
							_instance = new Groups();
					}
				}
				return _instance;
			}
		}
		private Groups() {
			Group obj;
			using(MySqlConnection conn = Connections.Inst.item("HTS").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadAll(conn, _table)){
					while(dr.Read()) {
						obj = new Group(dr);
						obj.BusinessCollection = this;
						List.Add(new Group(dr));
					}
				}
			}
		}
		#endregion
		#region Business Members
		public Groups Active{
			get{
				Groups gs = new Groups();
				foreach(Group obj in List) {
					if ( !obj.Retired)
						gs.Add(obj);
				}
				return gs;
			}
		}

		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Group : BusinessBase{ 
		#region Instance Data
		string _table = "Groups";
		int _id;
		string _name;
		string _description;
		string _email;
		MySqlDBLayer _dbLayer;	
		BusinessCollectionBase _businessCollection;
		bool _retired;
		#endregion
		#region DB Access / ctors
		public override string Table {
			get {
				return _table;
			}
		}
		public override object Conn {
			get {
				return Connections.Inst.item("HTS").MySqlConnection;
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
		public Group() {
			Setup();
			base.MarkNew();
		}
		public Group(int id) {
			Setup();
			this.Load(id);
		}
		
		public Group(MySqlDataReader dr) {
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			using(MySqlConnection conn = (MySqlConnection) this.Conn){
				conn.Open();
				using(MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", conn)){
					cmd.Parameters.Add("@Id", this.Id);
					using (MySqlDataReader dr = cmd.ExecuteReader()){
						if (dr.HasRows) {
							dr.Read();
							this.Load(dr);
						}else{
							throw new Exception("Groups " + id + " doesn't exist.");
						}
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			string name = Convert.ToString(dr["Name"]);
			string[] splitName = System.Text.RegularExpressions.Regex.Split(name, " - ");
			if (splitName.Length == 1) {
				this._name = splitName[0].Trim();
				this._retired = false;
			}else{
				if (splitName[0] == "RETIRED") {
					this._retired = true;
					this._name = splitName[1].Trim();
				}else{
					this._retired = false;
					this._name = splitName[0].Trim();
				}
			}
			this._description = Convert.ToString(dr["Description"]);
			this._email = Convert.ToString(dr["Email"]);
			MarkOld();
		}
		
		public override void Update(){
			throw new NotSupportedException("Update not available. Groups are read only.");
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@Id", this.Id);
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
		public string Name{
			get{
				return _name;
			}
		}
		public string Description{
			get{
				return _description;
			}
		}
		public string Email{
			get{
				return _email;
			}
		}
		public bool Retired{
			get{
				return _retired;
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
			return base.ToString();
		}
		#endregion
	}
}


