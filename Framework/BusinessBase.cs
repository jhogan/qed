using System;
using System.Data;
using System.Collections;

namespace JCSLA {
	abstract public class BusinessBase {
		#region Persistance state management
		bool _isNew = true;
		bool _isMarkedForDeletion = false;
		bool _isDirty = true;
		public bool IsNew {
			get {
				return _isNew;
			}
		}
		public bool IsMarkedForDeletion {
			get {
				return _isMarkedForDeletion;
			}
		}
		virtual public bool IsDirty {
			get {
				return _isDirty;
			}
		}
		public void MarkNew() {
			_isNew = true;
			_isMarkedForDeletion = false;
			MarkDirty();
		}

		public void MarkOld() {
			_isNew = false;
			MarkClean();
		}

		public void MarkDeleted() {
			_isMarkedForDeletion = true;
			MarkDirty();
		}
		public void MarkUndeleted() {
			_isMarkedForDeletion = false;
			MarkDirty();
		}

		protected void MarkDirty() {
			_isDirty = true;
		}

		protected void MarkClean() {
			_isDirty = false;
		}

		#endregion
		#region Data Access
		abstract public Hashtable ParamHash {
			 get;
		}
		abstract public int Id {
			get;
		}
		abstract public void SetId(int id) ;

		abstract public object Conn {
			get;
		}
		abstract public BusinessCollectionBase BusinessCollection{
			get;
			set;
		}
		abstract public string Table	{
			get;
		}
		abstract public void Update();
		#endregion
		#region Delete

		public void Delete() {
			MarkDeleted();
			this.Update();
		}
		public void Undelete(){
			MarkUndeleted();
			this.Update();
		}
		#endregion
		#region Business Help
		protected void SetValue(ref string prop, string value) {
			if (prop != value) this.MarkDirty();
			if (value == null){
				prop = "";
			}else{
				prop = value.Trim();
			}
		}
		protected void SetValue(ref int prop, int value) {
			if (prop != value){
				prop = value;
				this.MarkDirty();
			}
		}
		protected void SetValue(ref bool prop, bool value) {
			if (prop != value){
				prop = value;
				this.MarkDirty();
			}
		}
		protected object SetValue(object value) {
			MarkDirty();
			return value;
		}
		protected void SetValue(ref BusinessBase bb, BusinessBase value, ref int id) {
			if (value == null || bb == null || value.Id != bb.Id){
					MarkDirty();
					bb = value;
					if (value != null)
						id = value.Id;
					else
						id = -1;
			}
		}
		protected void SetValue(ref DateTime prop, DateTime value) {
			prop = value;
			this.MarkDirty();
		}
				
		#endregion
		#region BrokenRules, IsValid

		abstract public bool IsValid {
			get;
		}

		abstract public BrokenRules BrokenRules {
			get;
		}

		#endregion

	}
}
