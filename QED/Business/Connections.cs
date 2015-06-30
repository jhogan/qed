using System;
using System.Data;
using ByteFX.Data;
using System.Data.SqlClient;
using ByteFX.Data.MySqlClient;
using System.Configuration;
using System.Collections;
using JCSLA;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.IO;
namespace QED.Business {
	/// <summary>
	/// Summary description for Connections.
	/// </summary>
	public enum Transport {
		TCP, UDP
	}
	public enum Protocol{
		TELNET, SSH, SFTP, FTP, HTTP, MYSQL, MSSQL
	}
	public class Connections : BusinessCollectionBase {
		#region Instance Data
		const string _table = "connections";
		MySqlDBLayer _dbLayer;	
		private static object syncRoot = new Object();
		static Connections _instance;
		Connection _qed;
		string _key = "";
		bool _isLoaded = false;
		#endregion
		#region Collection Members
		public static Connections Inst {
			get {
					if (_instance == null) {
						lock (syncRoot) {
							if (_instance == null) 
								_instance = new Connections();
						}
					}
					return _instance;
				}
		}
		public Connection Add(Connection obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		
		public bool Contains(Connection obj) {
			foreach(Connection child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public Connection this[int id] {
			get{
				return (Connection) List[id];
			}
		}
		public Connection item(int id) {
			foreach(Connection obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		public Connection item(string SystemName) {
			if (SystemName == "QED_DB") return _qed;
			foreach (Connection conn in List) {
				if (conn.SystemName == SystemName) {
					return conn;
				}
			}
			return null;
		}

		#endregion
		#region DB Access and ctors
		public void Update() {
			foreach (Connection obj in List) {
				obj.Update();
			}
		}
		private Connections(){
		}
		public void Load(string key){
			if (! _isLoaded){
				_key = key;
				Connection connection;
				_qed = new Connection(new ConnectionString(ConfigurationSettings.AppSettings["QED_DB"].Trim()));
				_qed.BusinessCollection = this;
				_qed.Protocol = Protocol.MYSQL;	_qed.Port = 3306; _qed.Transport = Transport.TCP; _qed.SystemName = "QED_DB";
				_qed.PrivateDB = true;

				using(MySqlConnection conn = _qed.MySqlConnection){
					using(MySqlDataReader dr = MySqlDBLayer.LoadAll(conn, _table)){
						while(dr.Read()) {
							connection = new Connection();
							connection.BusinessCollection = this;
							connection	.Load(dr);
							List.Add(connection);
						}
						conn.Close();
					}
				}
				_isLoaded = true;
			}
		}
		
		#endregion
		#region Business Members
		public string Key{
			get{
				return _key;
			}
			set{
				_key = value;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ToString();
		}
		#endregion
	}
	public class Connection : BusinessBase  {
		#region Instance Data
		string _table = "connections";
		int _id = -1;
		string _user = "";
		string _encPasswd = "";
		string _decPasswd = "";
		Server _server;
		int _serverId = -1;
		string _connectionString = "";
		string _userAtHost = "";
		int _port = 0;
		Transport _transport;
		Protocol _protocol;
		bool _SSPI;
		string _systemName;
		string _catalog;
		MySqlDBLayer _dbLayer;	
		BusinessCollectionBase _businessCollection;
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
				return (object) new MySqlConnection(ConfigurationSettings.AppSettings["QED_DB"].Trim());
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
		public Connection() {
			Setup();
			base.MarkNew();
		}
		public Connection(int id) {
			Setup();
			this.Load(id);
		}
		public Connection(ConnectionString connStr) {
			Setup();
			this._catalog = 	connStr.InitialCatalog;
			this._SSPI = (connStr.IntegratedSecurity.ToUpper() == "SSPI");
			this._encPasswd = connStr.Password;
			this._user =  connStr.UserID;
			this._server = new Server(); this._server.DNSName = connStr.DataSource;
		}
		public Connection(MySqlDataReader dr) {
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
						throw new Exception("Connection " + id + " doesn't exist.");
					}
				}
			}
		}
		public void Load(MySqlDataReader dr) {
			Setup();
			SetId(Convert.ToInt32(dr["Id"]));
			this._catalog = Convert.ToString(dr["catalog"]);
			this._encPasswd = Convert.ToString(dr["passwd"]);
			this._port = Convert.ToInt32(dr["port"]);
			this._protocol = (QED.Business.Protocol) Enum.Parse(typeof(QED.Business.Protocol), Convert.ToString(dr["protocol"]));
			this._serverId = Convert.ToInt32(dr["serverId"]);
			this._SSPI = Convert.ToBoolean(dr["sspi"]);
			this._systemName = Convert.ToString(dr["systemName"]);
			this._transport = (QED.Business.Transport) Enum.Parse(typeof(QED.Business.Transport), Convert.ToString(dr["transport"]));
			this._user = Convert.ToString(dr["user"]);
			MarkOld();
		}
		public override void Update(){
			if (!_privateDB){
				_dbLayer.Update();
				if (_server != null)
					_server.Update();
			}else{
				throw new Exception("Connection is marked not to be saved");
			}
		}
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@passwd", this.EncPasswd);
				paramHash.Add("@port", this.Port);
				paramHash.Add("@protocol", this.Protocol.ToString());
				paramHash.Add("@serverId", this._serverId);
				paramHash.Add("@sspi", this.SSPI);
				paramHash.Add("@systemName", this.SystemName);
				paramHash.Add("@catalog", this.Catalog);
				paramHash.Add("@transport", this.Transport.ToString());
				paramHash.Add("@user", this.User);
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
		
		public string User{
			get{
				return _user;
			}
			set{
				SetValue(ref _user, value);
			}
		}
		public string EncPasswd{
			get{
				return _encPasswd;
			}
		}
		public string DecPasswd{
			get{
				string key = ((Connections) this.BusinessCollection).Key;
				if (key == "") throw new Exception("Key must be provided to decrypt password");
				return Encryptor.DecryptString(_encPasswd, key);
			}
			set{
				string key = ((Connections) this.BusinessCollection).Key;
				if (key == "") throw new Exception("Key must be provided to encrypt password");
				_encPasswd = Encryptor.EncryptString(value, key);
				MarkDirty();
			}
		}
		public Server Server{
			get{
				if (_server == null && _serverId != -1) 
					_server = new Server(_serverId);
				return _server;
			}
			set{
				BusinessBase bb = (BusinessBase) _server;
				SetValue(ref bb, (BusinessBase)value, ref _serverId);
			}
		}
		public SqlConnection SqlConnection{
			get{
				if (this.Protocol == Protocol.MYSQL) {
					return new SqlConnection(this.ConnectionString);
				}
				else {
					throw new InvalidOperationException("Protocol is not MSSQL");
				}
			}
		}
		public MySqlConnection MySqlConnection{
			get{
				if (this.Protocol == Protocol.MYSQL) {
					return new MySqlConnection (this.ConnectionString);
				}
				else {
					throw new InvalidOperationException("Protocol is not MySQL");
				}
			}
		}
		public string ConnectionString{
			get{
				if (this.IsDBProtocol){
					return "Server=" + this.Server.DNSName + "; Uid=" + this.User + 
						"; Password=" + this.DecPasswd + "; Database=" + this.Catalog + ((this.SSPI) ? ";Integrated Security=SSPI" : "");
				}else{ 
					return "";
				}
			}
		}
		public string UserAtHost{
			get{
				return _userAtHost;
			}
		}
		public int Port{
			get{
				return _port;
			}
			set{
				SetValue(ref _port, value);
			}
		}
		public Transport Transport{
			get{
				return _transport;
			}
			set{
				if (_transport != value){
					_transport = value; MarkDirty();
				}
			}
		}
		public Protocol Protocol{
			get{
				return _protocol;
			}
			set{
				if (_protocol != value){
					_protocol = value; MarkDirty();
				}
			}
		}
		public bool SSPI{
			get{
				return _SSPI;
			}
			set{
				SetValue(ref _SSPI, value);
			}
		}
		public string Catalog{
			get{
				return _catalog;
			}
			set{
				SetValue(ref _catalog, value);
			}
		}
		public string SystemName{
			get{
				return _systemName;
			}
			set{
				SetValue(ref _systemName, value);
			}
		}
		public bool IsDBProtocol {
			get {
				return (this.Protocol == Protocol	.MSSQL || this.Protocol == Protocol.MYSQL);
			}
		}
		public bool PrivateDB{
			get {
				return _privateDB;
			}
			set {
				_privateDB = value;
				if (_server != null) {
					_server.PrivateDB = value;
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
	public class ConnectionString {
		/* This small class takes a connection string as a param to the ctor and then makes each of
		 * the connection string's elements a public field. It could be extended to take into account
		 * more connection string elements but the ones supported are likely to be all the program needs. */
		public string IntegratedSecurity = "";
		public string DataSource = "";
		public string InitialCatalog = "";
		public string Password = "";
		public string UserID = "";
		public ConnectionString(string connStr) {
			string[] KVPs = connStr.Split(';');
			string key, val;
			int firstEq;
			foreach (string KVP in KVPs) {
				firstEq = KVP.IndexOf("=");
				key = KVP.Substring(0, firstEq).ToUpper();
				val = KVP.Substring(firstEq+1, (KVP.Length - firstEq-1));
				switch (key) {
					case "INTEGRATED SECURITY": 
						IntegratedSecurity = val;
						break;
					case "DATA SOURCE": case "SERVER":
						DataSource = val;
						break;
					case "INITIAL CATALOG": case "DATABASE":
						InitialCatalog = val;
						break;
					case "PASSWORD": case "PWD":
						Password = val;
						break;
					case "USER ID": case "UID":
						UserID = val;
						break;
				}
			}
		}
		public override string ToString() {
			return "Integrated Security = " + this.IntegratedSecurity + "; " + 
				"Server = " + this.DataSource + "; " +
				"Database = " + this.InitialCatalog + "; " + 
				"User Id = " + this.UserID + "; " +
				"Password = " + ((this.Password.Length == 0) ? "": "".PadRight(this.Password.Length, 'X'));
				
		}
	}
	public class Encryptor{
		// Author: Deon Spengler
		// See http://dotnet.org.za/deon/articles/2998.aspx for commentary 
		public static string EncryptString(string InputText, string Password) {
			RijndaelManaged RijndaelCipher = new RijndaelManaged();
			byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
			byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
			ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(PlainText, 0, PlainText.Length);
			cryptoStream.FlushFinalBlock();
			byte[] CipherBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			string EncryptedData = Convert.ToBase64String(CipherBytes);
			return EncryptedData;
		}
		public static string DecryptString(string InputText, string Password) {
			RijndaelManaged  RijndaelCipher = new RijndaelManaged();
			byte[] EncryptedData = Convert.FromBase64String(InputText);
			byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
			ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
			MemoryStream  memoryStream = new MemoryStream(EncryptedData);
			CryptoStream  cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
			byte[] PlainText = new byte[EncryptedData.Length];
			int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
			memoryStream.Close();
			cryptoStream.Close();
			string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
			return DecryptedData;
		}
	}
}

