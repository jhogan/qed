using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
namespace QED.Business{
	public class Clients : BusinessCollectionBase {
		#region Instance Data
		private static object syncRoot = new Object();
		const string _table = "Clients";
		MySqlDBLayer _dbLayer;	
		static Clients _instance;
		#endregion
		#region Collection Members
		public Client Add(Client obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		public bool Contains(Client obj) {
			foreach(Client child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Client this[int id] {
			get{
				return (Client) List[id];
			}
		}
		public Client item(int id) {
			foreach(Client obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		public Client item(string id) {
			foreach(Client obj in List) {
				if (obj.Name == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public static Clients Inst {
			get {
				if (_instance == null) {
					lock (syncRoot) {
						if (_instance == null) 
							_instance = new Clients();
					}
				}
				return _instance;
			}
		}
		private Clients() {
			Client obj;
			using(MySqlConnection conn = Connections.Inst.item("HTS").MySqlConnection){
				using (MySqlDataReader dr = MySqlDBLayer.LoadAll(conn, _table)){
					while(dr.Read()) {
						obj = new Client(dr);
						obj.BusinessCollection = this;
						List.Add(new Client(dr));
					}
				}
			}
		}

		public void Update() {
			throw new NotSupportedException("Update not available. Clients are read only.");
		}
		#endregion
		#region Business Members
		public Clients Active() {
			Clients clients = new Clients();
			foreach(Client c in List) {
				if (!c.Retired) clients.Add(c);
			}
			return clients;
		}
		public Clients Retired() {
			Clients clients = new Clients();
			foreach(Client c in List) {
				if (c.Retired) clients.Add(c);
			}
			return clients;
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Client : BusinessBase  {
		#region Instance Data
		string _table = "Clients";
		int _id;
		string _name;
		bool _retired;
		MySqlDBLayer _dbLayer;	
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
		public Client() {
			Setup();
			base.MarkNew();
		}
		public Client(int id) {
			Setup();
			this.Load(id);
		}
		
		public Client(MySqlDataReader dr) {
			this.Load(dr);
		}
		public void Load(int id) {
			SetId(id);
			using(MySqlConnection conn = (MySqlConnection)this.Conn){
				conn.Open();
				using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + this.Table + " WHERE ID = @ID", conn)){
					cmd.Parameters.Add("@Id", this.Id);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						if (dr.HasRows) {
							dr.Read();
							this.Load(dr);
						}else{
							throw new Exception("Client " + id + " doesn't exist.");
						}
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			string name;
			string[] splitName;
			Setup();
			SetId(Convert.ToInt32(dr["ID"]));
			name = Convert.ToString(dr["Name"]);
			splitName = System.Text.RegularExpressions.Regex.Split(name, " - ");
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
			MarkOld();
		}
		
		public override void Update(){
			throw new NotSupportedException("Update not available. Clients are read only.");
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
			return this.ToString();
		}
		#endregion
	}
}


