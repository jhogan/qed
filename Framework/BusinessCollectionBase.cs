using System;
using System.Collections;

namespace JCSLA{
	abstract public class BusinessCollectionBase : CollectionBase{
		public bool Contains(BusinessBase item) {
			return List.Contains(item);
		}
		public bool IsDirty {
			get {
				foreach(BusinessBase child in List)
					if(child.IsDirty)
						return true;
				return false;
			}
		}
		public virtual bool IsValid {
			get {
				foreach(BusinessBase child in List)
					if(!child.IsValid)
						return false;
				return true;
			}
		}
		public void Remove(BusinessBase child) {
			List.Remove(child);
		}
		public BrokenRules BrokenRules {
			get {
				BrokenRules colBR = new BrokenRules();
				foreach(BusinessBase bb  in List)
					foreach(BrokenRule bbBR in bb.BrokenRules) 
						colBR.Add(bbBR);
				return colBR;
			}
		}


	}
}
