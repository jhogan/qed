using System;
using ByteFX.Data;
using ByteFX.Data.MySqlClient;
using System.Collections;
namespace JCSLA
{
	/// <summary>
	/// Summary description for MySQlDBLayer.
	/// </summary>
	public class MySqlDBLayer
	{
		BusinessBase _bb;
		public MySqlDBLayer(BusinessBase bb)
		{
			_bb = bb;
		}
		/*public MySqlDataReader Load() {
			MySqlConnection conn = (MySqlConnection) _bb.Conn;
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + _bb.Table + " WHERE ID = @ID", conn);
			cmd.Parameters.Add("@Id", _bb.Id);
			return cmd.ExecuteReader();
		}*/
		
		public static MySqlDataReader LoadAll(MySqlConnection conn, string table) {
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + table, conn);
			MySqlDataReader dr = cmd.ExecuteReader();
			return dr;
		}

		public static MySqlDataReader LoadWhereColumnIs(MySqlConnection conn, string table, string key, int value) {
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + table + " WHERE " + key + " = @Id " + 
				"ORDER BY 1", conn);
			cmd.Parameters.Add("@Id", value);
			MySqlDataReader dr = cmd.ExecuteReader();
			return dr;
		}
		public static MySqlDataReader LoadWhereColumnIs(MySqlConnection conn, string table, string key, string value) {
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + table + " WHERE " + key + " = @Id " +
					"ORDER BY 1", conn);
			cmd.Parameters.Add("@Id", value);
			MySqlDataReader dr = cmd.ExecuteReader();
			return dr;
		}

		
		public static MySqlDataReader Load(MySqlConnection conn, string table, string pk, int value) {
			conn.Open();
			MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + table + " WHERE " + pk + " = @PK", conn);
			cmd.Parameters.Add("@PK", value);
			return cmd.ExecuteReader();
		}
		
		public void Update(){
			using (MySqlConnection conn = (MySqlConnection) _bb.Conn) {
				conn.Open();
				using(MySqlCommand cmd = conn.CreateCommand()) {
					if (_bb.IsMarkedForDeletion){
						if ( !_bb.IsNew){
							cmd.CommandText = "DELETE FROM " + _bb.Table + " WHERE ID = @ID";
							cmd.Parameters.Add("@Id", _bb.Id);
							if (cmd.ExecuteNonQuery() != 1) throw new Exception("Delete resulted in incorrected number of deletions. Expected 1 delete.");
						}
						_bb.BusinessCollection.Remove(_bb);
					}else{
						if (_bb.IsValid){
							if (_bb.IsNew) {
								this.SetToInsert(cmd);
								object obj = cmd.ExecuteScalar();
								_bb.SetId(Convert.ToInt32(obj));
							}else if (_bb.IsDirty){
								this.SetToUpdate(cmd);
								cmd.Parameters.Add("@Id", _bb.Id);
								if (cmd.ExecuteNonQuery() != 1) throw new Exception("Update statement resulted in 0 updates. Expected 1 update.");
							}
						}else {
							throw new InvalidOperationException("Objects can't be saved because its state is invalid");
						}
						_bb.MarkOld();
					}
				}
			}
		}
		private void SetToInsert(MySqlCommand cmd) {
			cmd.CommandText = "INSERT INTO " + _bb.Table + 
				this.FIELDS + " " + 
				this.VALUES + "; " + 
				"SELECT LAST_INSERT_ID()";
				SetParams(cmd);
		}

		private void SetToUpdate(MySqlCommand cmd){
			cmd.CommandText = "UPDATE " + _bb.Table + " " + this.SET + " WHERE Id = @ID";
			cmd.Parameters.Add("@Id", _bb.Id);
			SetParams(cmd);
		}
		private void SetParams(MySqlCommand cmd){
			Hashtable paramHash = _bb.ParamHash;
			IDictionaryEnumerator paramEnum = paramHash.GetEnumerator();
			object val;
			while(paramEnum.MoveNext()) {
				val = paramEnum.Value;
				if (val is String){
					val = Replacements(val.ToString());
				}
				cmd.Parameters.Add((string)paramEnum.Key, val);
			}
		}
		private string SET {
			get {
				string set = "SET ";
				string field, param;
				bool beenHere = false;
				IDictionaryEnumerator paramEnum = _bb.ParamHash.GetEnumerator();
				while(paramEnum.MoveNext()) {
					if (beenHere) set += ", ";
					beenHere = true;
					field = Convert.ToString(paramEnum.Key);
					field = field.Remove(0,1);
					param = Convert.ToString(paramEnum.Key);
					set += field + " = " + param; 
				}
				return set;
			}
		}

		private string VALUES{
			get {
				string value = "VALUES (";
				bool beenHere = false;
				IDictionaryEnumerator paramEnum = _bb.ParamHash.GetEnumerator();
				while(paramEnum.MoveNext()) {
					if (beenHere) value += ", ";
					beenHere = true;
					value += Convert.ToString(paramEnum.Key);
				}
				value += ")";
				return value;
			} 
		}
		private string FIELDS{
			get {
				string fields = "(";
				string field;
				bool beenHere = false;
				IDictionaryEnumerator paramEnum = _bb.ParamHash.GetEnumerator();
				while(paramEnum.MoveNext()) {
					if (beenHere) fields += ", ";
					beenHere = true;
					field = Convert.ToString(paramEnum.Key);
					field = field.Remove(0,1);
					fields += field;
				}
				fields += ")";
				return fields;
			}
		}
		public static string Replacements(string str) {
			str = str.Replace(@"\", @"\\");

			return str;
		}
	}
}
