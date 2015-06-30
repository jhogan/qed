using System;
using System.Security.Principal;
using System.Threading;
using System.Collections;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using QED.Business;
using JCSLA;
using QED.DataValidation;
using QED.Business.Sec;

namespace QED.SEC {
	public class QEDPrincipal : IPrincipal {
		BusinessIdentity _identity;
		#region IPrincipal
		/// <summary>
		/// Implements the Identity property defined by IPrincipal.
		/// </summary>
		IIdentity IPrincipal.Identity {
			get {
				return _identity;
			}
		}
		/// <summary>
		/// Implements the IsInRole property defined by IPrincipal.
		/// </summary>
		bool IPrincipal.IsInRole(string role) {
			return _identity.IsInRole(role);
		}
		#endregion

		#region Login process
		public static void Login(string username, string password) {
			new QEDPrincipal(username, password);
		}
		private QEDPrincipal(string username, string password) {
			AppDomain currentdomain = Thread.GetDomain();
			currentdomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);

			IPrincipal oldPrincipal = Thread.CurrentPrincipal;
			Thread.CurrentPrincipal = this;

			try {
				if(!(oldPrincipal.GetType() == typeof(QEDPrincipal)))
					currentdomain.SetThreadPrincipal(this);
			}                                                                                        
			catch {
				// failed, but we don't care because there's nothing
				// we can do in this case
			}

			// load the underlying identity object that tells whether
			// we are really logged in, and if so will contain the 
			// list of roles we belong to
			_identity = new BusinessIdentity().LoadIdentity(username, password);
		}
		#endregion
	}

	public class BusinessIdentities : BusinessCollectionBase {
		#region Instance Data
		#endregion
		const string _table = "users";
		MySqlDBLayer _dbLayer;	
		#region Collection Members
		public BusinessIdentity Add(BusinessIdentity obj) {
			obj.BusinessCollection = this;
			List.Add(obj); return obj;
		}
		
		public bool Contains(BusinessIdentity obj) {
			foreach(BusinessIdentity child in List) {
				if (obj.Equals(child)){
					return true;
				}
			}
			return false;
		}
		public BusinessIdentity this[int id] {
			get{
				return (BusinessIdentity) List[id];
			}
		}
		#endregion
		#region DB Access and ctors
		public BusinessIdentities() {
		}
		/*
		public void Update() {
			foreach (BusinessIdentity obj in List) {
				obj.Update();
			}
		}
		*/
		public static BusinessIdentities AllBusinessIdentities(){
			BusinessIdentities bids = new BusinessIdentities();
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				conn.Open(); MySqlCommand cmd =conn.CreateCommand();
				cmd.CommandText = "SELECT * FROM users";
				BusinessIdentity bid;
				using(MySqlDataReader dr = cmd.ExecuteReader()){
					while (dr.Read()){
						bid = new BusinessIdentity();
						bid.LoadIdentity(dr);
						bids.Add(bid);
					}
				}
			}
			return bids;
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
	public class BusinessIdentity : IIdentity {
		string _userName = string.Empty;
		bool _isAuthenticated = false;
		bool _qa = false;
		bool _manager = false;
		bool _admin = false;
		string _firstName;
		string _lastName;
		#region IIdentity
		/// <summary>
		/// Implements the IsAuthenticated property defined by IIdentity.
		/// </summary>
		bool IIdentity.IsAuthenticated {
			get {
				return _isAuthenticated;
			}
		}
		string IIdentity.AuthenticationType {
			get {
				return "JCSLA";
			}
		}
		string IIdentity.Name {
			get {
				// This is the email address which identifies the users
				return _userName;
			}
		}
		public string UserName{
			get{
				// This is the username part (local part) of the user's email address (IIdentity.Name)
				EmailValidator ev = new EmailValidator(_userName);
				return ev.LocalPart;
			}
		}
		public string Domain{
			get{
				EmailValidator ev = new EmailValidator(_userName);
				return ev.Domain;
			}
		}
		public string Email{
			get{
				return _userName;
			}
		}
		public string FirstName{
			get{
				return _firstName;
			}
		}
		public string LastName{
			get{
				return _lastName;
			}
		}

		public string FullName{
			get{
				return this.FirstName + " " + this.LastName;
			}
		}

		#endregion
		internal bool IsInRole(string role) {
			switch (role.ToUpper()){
				case "QA":
					return (_qa || _manager || _admin);
				case "MANAGER":
					return (_manager);
				case "ADMIN":
					return (_admin);
			}
			return false;
		}
		#region Instance Data
		string _table = "users";
		int _id;
		MySqlDBLayer _dbLayer;	
		BusinessCollectionBase _businessCollection;
		#endregion
		#region DB Access / ctors
		public string Table {
			get {
				return _table;
			}
		}
		public object Conn {
			get {
				return Connections.Inst.item("QED_DB").MySqlConnection;
			}
		}
		private void Setup() {

		}
		public void SetId(int id) {
			/* This function is public for technical reasons. It is intended to be used only by the db
			 * layer*/
			_id = id;
		}
		public BusinessCollectionBase BusinessCollection {
			get{
				return _businessCollection;
			}
			set {
				_businessCollection = value;
			}
		}
		public BusinessIdentity() {} 
		internal BusinessIdentity LoadIdentity(string email, string password) {
			_qa = _manager = _admin = _isAuthenticated = false;
			_userName = email;
			using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
				using(MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "email", email)){
					if (dr.Read()){
						if (SimpleHash.VerifyHash(password, "MD5", Convert.ToString(dr["passwd"]))){
							this.LoadIdentity(dr);
							_isAuthenticated =true;
						}
					}
				}
			}
			return this;
		}
		public void LoadIdentity(MySqlDataReader dr){
			_firstName = Convert.ToString(dr["fname"]);
			_lastName = Convert.ToString(dr["lname"]);
			_qa = Convert.ToBoolean((dr["qa"]));
			_manager = Convert.ToBoolean((dr["manager"]));
			_admin = Convert.ToBoolean((dr["admin"]));
			_userName = Convert.ToString(dr["email"]);
		}

		/* Persistence can be implemented later. Just don't need it right now.
		public override void Update(){
			_dbLayer.Update();
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@Id", this.Id);
				return paramHash;
			}
		}*/
		#endregion
		#region Business Properties
		/*
		public override int Id{
			get{
				return _id;
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
		}*/
		public Efforts EffortsWorked{
			get{
				Efforts effs = new Efforts();
				Effort eff;
				using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
					conn.Open();
					MySqlCommand cmd = conn.CreateCommand();
					cmd.CommandText = "SELECT DISTINCT effId FROM times WHERE user = @user and effId != -1";
					cmd.Parameters.Add("@user", this.Email);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						while (dr.Read()){
							eff = new Effort(Convert.ToInt32(dr["effId"]), false);
							effs.Add(eff);
						}
					}
				}
				return effs;
			}
		}
		public Rollouts RolloutsWorked{
			get{
				Rollouts rolls = new Rollouts();
				Rollout roll;
				using(MySqlConnection conn = Connections.Inst.item("QED_DB").MySqlConnection){
					conn.Open();
					MySqlCommand cmd = conn.CreateCommand();
					cmd.CommandText = "SELECT DISTINCT rollId FROM times WHERE user = @user and rollId != -1";
					cmd.Parameters.Add("@user", this.Email);
					using(MySqlDataReader dr = cmd.ExecuteReader()){
						while (dr.Read()){
							roll = new Rollout(Convert.ToInt32(dr["rollId"]));
							rolls.Add(roll);
						}
					}
				}
				return rolls;
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

