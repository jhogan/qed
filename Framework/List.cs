using System;
using System.Data;
using System.Data.Odbc;
using ByteFX.Data.MySqlClient;
using ByteFX.Data;
using System.Collections;
namespace JCSLA{
	public class CatLists : BusinessCollectionBase {
		#region Instant Data
		const string _table = "lists";
		MySqlConnection _conn;
		#endregion
		#region Collection Members
		public CatList AddNew(string listName) {
			CatList cl= new CatList(listName, true, _conn);
			List.Add(cl); return cl;
		}
		public bool Contains(CatList cl) {
			foreach(CatList obj in List) {
				if (cl.Equals(obj)){
					return true;
				}
			}
			return false;
		}
		public bool Contains(string listName) {
			foreach(CatList obj in List) {
				if (obj.ListName.ToUpper() == listName.ToUpper().Trim()){
					return true;
				}
			}
			return false;
		}
		public CatList this[int id] {
			get{
				return (CatList) List[id];
			}
		}
		public CatList item(string listName) {
			foreach(CatList cl in List) {
				if (cl.ListName == listName)
					return cl;
			}
			return null;
		}
		#endregion
		#region DB Access and ctors
		public CatLists(MySqlConnection conn) {
			_conn = conn;
			conn.Open();
			ArrayList listNames = new ArrayList();
			MySqlCommand cmd = conn.CreateCommand();
			cmd.CommandText ="SELECT DISTINCT ListName FROM " + _table;
			MySqlDataReader dr = cmd.ExecuteReader();
			while(dr.Read()) {
				listNames.Add(Convert.ToString(dr["ListName"]));
			}
			dr.Close();
			conn.Close();
			foreach(string listName in listNames){
				List.Add(new CatList(listName, false, conn));
			}
			
		}
		public void Update() {
			foreach (CatList obj in List) {
				obj.Update();
			}
		}
		#endregion
		#region Business Members
		#endregion
	}
	public class CatList : BusinessBase  {
		#region Instant Data
		const string _table = "lists";
		Entries _ents;
		MySqlDBLayer _dbLayer;
		string _listName = "";
		MySqlConnection _conn;
		BusinessCollectionBase _businessCollection;
		#endregion
		#region DB Access / ctors
		public override void SetId(int id) {
			/* This function is required by the base class but is not needed here */
		}
		public override BusinessCollectionBase BusinessCollection {
			get{
				return _businessCollection;
			}
			set {
				_businessCollection = value;
			}
		}
		public override object Conn {
			get {
				return _conn;
			}
		}
		public override string Table {
			get {
				return _table;
			}
		}
		private void Setup(MySqlConnection conn, string listName) {
			_conn = conn; _listName = listName;
			_ents = new Entries(this);
			if (_dbLayer == null) {
				_dbLayer = new MySqlDBLayer(this);
			}
		}
		public CatList(string listName, bool create, MySqlConnection conn) {
			Setup(conn, listName);
			if (create){
				base.MarkNew();
			}else{
				this.Load(listName);
			}
		}
		public CatList(string listName, MySqlConnection conn) {
			Setup(conn, listName);
			this.Load(listName);
		}
		public void Load(string listName) {
			_listName = listName;
			MySqlConnection conn = (MySqlConnection)this.Conn;
			MySqlDataReader dr = MySqlDBLayer.LoadWhereColumnIs(conn, _table, "listName",listName);
			while(dr.Read()) {
				_ents.Add(new Entry(dr, this));
			}
			conn.Close();
		}
		public void Reload() {
			_ents.Clear();
			Load(this.ListName);
		}
		public override void Update(){

			foreach(Entry ent in _ents) {
				ent.Update();
			}
		}
		public string ListName{
			get{
				return _listName;
			}
		}
		#endregion
		#region Business Properties
		public override int Id {
			get {
				/* This is required by base class but not needed here */
				return 0;
			}
		}
		public Entries Entries{
			get{
				return _ents;
			}
		}
		public Entries RootEntries{
			get{
				Entries ents =new Entries(this);
				foreach(Entry ent in _ents) {
					if (ent.Parent == null) {
						ents.Add(ent);
					}
				}
				return ents;
			}
		}
		public Entry AddRoot(string key) {
			Entry ent = new Entry(key, this);
			ent.IsLeaf = false; 
			ent.Value = "";
			ent.BusinessCollection = this.Entries;
			this.Entries.Add(ent);
			return ent;
		}
		public Entry Entry(string key) {
			Entry ent;
			Entry root = new Entry();
			key = key.ToUpper();
			string[] path = key.Split("/".ToCharArray());
			foreach(Entry rootEnt in this.RootEntries) {
				if (rootEnt.Key.ToUpper() == path[1]){
					root = rootEnt; break;
				}
			}
			if (root != null){
				ent = root;
				for(int i = 2; i<path.Length; i++){
					foreach (Entry child in ent.Entries) {
						if (child.Key.ToUpper() == path[i]){
							ent = child; break;
						}
					}
				}

				/* I don't know why I did this but it didn't work when I wanted to get a category entry with this method
				if (ent.IsLeaf)
					return ent;
				else
					return null;*/
				return ent;
			}
			return null;
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
		public override Hashtable ParamHash{
			get {
				/* This is required by base class but not needed here */
				return new Hashtable();
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.ListName;
		}
		#endregion
	}
	public class Entries : BusinessCollectionBase {
		#region Instant Data
		const string _table = "lists";
		CatList _catList;
		Entry _parEnt;
		#endregion
		#region Collection Members
		public Entry Add(Entry ent) {
			/* Used by CatList.Entries. See Entries.Add(string key)*/
			ent.BusinessCollection = this;
			List.Add(ent); return ent;
		}
		
		public Entry Add(string key) {
			/* Used to add a new leaf entry to an existing entry: ent.Entries.Add("MyNewEntry"); */
			Entry ent = new Entry(key, this._catList);
			ent.ParentId = _parEnt.Id;
			ent.IsLeaf = false;
			this._catList.Entries.Add(ent);
			ent.BusinessCollection = this._catList.Entries;
			List.Add(ent); return ent;
		}
		public Entry Add(string key, string val) {
			/* Used to add a new category entry to an existing entry: ent.Entries.Add("myKey", "myVal"); */
			Entry ent = new Entry(key, this._catList);
			ent.Value = val;
			ent.IsLeaf = true;
			ent.ParentId = _parEnt.Id;
			this._catList.Entries.Add(ent);
			ent.BusinessCollection = this._catList.Entries;
			List.Add(ent); return ent;
		}
		
		public bool Contains(Entry obj) {
			foreach(Entry ent in List) {
				if (ent.Equals(obj)){
					return true;
				}
			}
			return false;
		}
		public bool Contains(string key) {
			foreach(Entry ent in List) {
				if (ent.Key.ToUpper() == key.ToUpper()){
					return true;
				}
			}
			return false;
		}
		public Entry this[int id] {
			get{
				return (Entry) List[id];
			}
		}
		public Entry item(int id) {
			foreach(Entry obj in List) {
				if (obj.Id == id)
					return obj;
			}
			return null;
		}
		#endregion
		#region Business Members
		#endregion
		#region DB Access and ctors
		public Entries(CatList cl) {
			_catList = cl;
		}
		public Entries(Entry ent, CatList cl) {
			_parEnt = ent;
			_catList = cl;
		}

		public void Update() {
			foreach (Entry obj in List) {
				obj.Update();
			}
		}
		#endregion
	}
	public class Entry : BusinessBase  {
		#region Instant Data
		const string _table = "lists";
		MySqlDBLayer _dbLayer;
		int _id;
		CatList _catList;
		string _listName = "";
		string _key = "";
		string _value = "";
		bool _leaf = false;
		int _parId = -1;
		BusinessCollectionBase _businessCollection;
		#endregion
		#region DB Access / ctors
		public override object Conn {
			get {
				return this.CatList.Conn;
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
		public Entry() {
			Setup();
			base.MarkNew();
		}
		public Entry(string key, CatList catList) {
			Setup();
			_key = key;
			_catList = catList;
			base.MarkNew();
		}

		public Entry(MySqlDataReader dr, CatList catList) {
			Setup();
			_catList = catList;
			this.Load(dr);
		}
		private void Load(MySqlDataReader dr) {
			SetId(Convert.ToInt32(dr["Id"]));
			this._key = Convert.ToString(dr["key_"]);
			this._value = Convert.ToString(dr["value"]);
			this._leaf = Convert.ToBoolean(dr["leaf"]);
			this._listName = Convert.ToString(dr["listName"]);
			this._parId = Convert.ToInt32(dr["parId"]);
			MarkOld();
		}
		public override void Update(){
			if (this.IsMarkedForDeletion){
				Entries allDesc = this.AllDescendents;
				int allDescCount = allDesc.Count;
				Entry ent;
				for (int i=allDescCount-1; i>-1; i--){
					ent = allDesc[i];
					ent.MarkDeleted();
					System.Diagnostics.Debug.WriteLine(ent.Id);
					ent.DirectUpdate();
					//ent.CatList.Entries.Remove(ent);
				}
				this.DirectUpdate();
				//this.CatList.Entries.Remove(this);
			}else{
				_dbLayer.Update();
			}
		}
		internal void DirectUpdate(){
			
			_dbLayer.Update();
		}

		public Entries AllDescendents{
			get{
				Entries ents = new Entries(this.CatList);
				foreach (Entry ent in this.Entries) {
					_allDescendents(ent, ents);
				}
				return ents;
			}
		}
		private void _allDescendents(Entry ent, Entries ents) {
			ents.Add(ent);
			foreach (Entry child in ent.Entries) {
				_allDescendents(child, ents);
			}
		}
		
		public override Hashtable ParamHash {
			get {
				Hashtable paramHash = new Hashtable();
				paramHash.Add("@Id", this.Id);
				paramHash.Add("@ListName", this.ListName);
				paramHash.Add("@leaf", this.IsLeaf);
				paramHash.Add("@Key_", this.Key);
				paramHash.Add("@Value", this.Value);
				paramHash.Add("@ParId", this._parId);
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
		public Entries Entries{
			get {
				Entries cl = new Entries(this, this.CatList);
				foreach(Entry ent in this.CatList.Entries) {
					if (ent.ParentId == this.Id){
						cl.Add(ent);
					}
				}
				return cl;
			}
		}
		public Entry SubEntry(string key) {
			key = this.Path + "/" + key;
			return this.CatList.Entry(key);
		}
		public Entries Leafs{
			get {
				Entries cl = new Entries(this, this.CatList);
				foreach(Entry ent in this.CatList.Entries) {
					if (ent.ParentId == this.Id && ent.IsLeaf){
						cl.Add(ent);
					}
				}
				return cl;
			}
		}
		public Entries Categories{
			get {
				Entries cl = new Entries(this, this.CatList);
				foreach(Entry ent in this.CatList.Entries) {
					if (ent.ParentId == this.Id && !ent.IsLeaf){
						cl.Add(ent);
					}
				}
				return cl;
			}
		}
		public Entry Parent {
			get{
				Entries cl = new Entries(this.CatList);
				foreach(Entry ent in this.CatList.Entries) {
					if (ent.Id == this.ParentId){
						return ent;
					}
				}
				return null;
			}
		}
		
		public int ParentId {
			get{
				return _parId;
			}
			set{
				base.SetValue(ref _parId, value);
			}
		}
		
		public string ListName{
			get{
				return this.CatList.ListName;
			}

		}
		public string Key{
			get{
				return _key;
			}
			set{
				base.SetValue(ref _key, value);
			}
		}
		public string Value{
			get{
				return _value;
			}
			set{
				base.SetValue(ref _value, value);
			}
		}
		public bool IsLeaf
		{
			get{
				return _leaf;
			}
			set{
				base.SetValue(ref _leaf, value);
			}
		}
		public bool IsRoot{
			get{
				return (_parId == -1);
			}
		}
		public Entries Siblings{
			get{
				Entries ret = new Entries(this.CatList);
				Entries sybEnts;
				
				if (this.IsRoot)
					sybEnts = this.CatList.RootEntries;
				else
					sybEnts = this.Parent.Entries;

				foreach(Entry sybEnt in sybEnts) {
					if (this != sybEnt) {
						ret.Add(sybEnt);
					}
				}
				return ret;
			}
		}

		public CatList CatList {
			get{
				return _catList;
			}
		}
		public string Path{
			get {
				string ret;
				Entry par;
				ret = this.Key;
				par = this.Parent;
				while (par != null) {
					ret = par.Key + "/" + ret;
					par = par.Parent;
				}
				ret = "/" + ret;
				return ret;
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
				br.Assert("LIST_NAME_EMPTY", "List name must be specified", this.ListName == "");
				br.Assert("KEY_NAME_EMPTY", "Category or key name must be specified", this.Key == "");
				br.Assert("NO_CATLIST", "Entry is not part of a CatList", this.CatList == null);
				br.Assert("LEAF_HASNT_VAL", "Leafs must have values", this.Value == "" && this.IsLeaf);
				br.Assert("CAT_HAS_VAL", "Categories can't have values", this.Value != "" && !this.IsLeaf);
				br.Assert("LEAF_HAS_CHILDREN", "Leafs can't have child entries.", this.Entries.Count != 0 && this.IsLeaf);
				br.Assert("DUP_NAME", "There is aleady a category or key with this name.", this.Siblings.Contains(this.Key));
				return br;
			}
		}
		#endregion
		#region System.Object overrides
		public override string ToString(){	
			return this.Key;
		}
		#endregion
	}
}

