using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
using JCSLA;
using System.Net;

namespace QED.Business {
	public class Servers : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "servers";
		MySqlDBLayer _dbLayer;	
		#region Collection Members
		public Server Add(Server obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		public bool Contains(Server obj) {
			foreach(Server child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Server this[int id] {
			get{
				return (Server) List[id];
			}
		}
		public Server item(int id) {
			foreach(Server obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public Servers() {
		}
		public void Update() {
			foreach (Server obj in List) {
				obj.Update();
			}
		}
		public void LoadAll() {
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadAll(conn, _table)){
					Server server;
					while(dr.Read()) {
						server = new Server(dr);
						server.BusinessCollection = this;
						List.Add(server);
					}
				}
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
	public class Server : BusinessBase  {
		#region Instance Data
		string _table = "servers";
		int _id = -1;
		MySqlDBLayer _dbLayer;	
		BusinessCollectionBase _businessCollection;
		string _dnsName = "";
		string _desc = "";
		bool _privateDB = false;
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
		public Server() {
			Setup();
			base.MarkNew();
		}
		public Server(int id) {
			Setup();
			this.Load(id);
		}
		
		public Server(MySqlDataReader dr) {
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
						throw new Exception("Server " + id + " doesn't exist.");
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			this._desc = Convert.ToString(dr["desc_"]);
			this._dnsName = Convert.ToString(dr["dnsName"]);
			MarkOld();
		}
		public override void Update(){
			_dbLayer.Update();
		}
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@Desc_", this.Desc);
				paramHash.Add("@DNSName", this.DNSName);
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

		public string DNSName{
			get{
				return _dnsName;
			}
			set{
				_dnsName = value;
			}
		}
		public string Desc{
			get{
				return _desc;
			}
			set{
				_desc = value;
			}
		}
		public bool PrivateDB{
			get {
				return _privateDB;
			}
			set {
				_privateDB = value;
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




