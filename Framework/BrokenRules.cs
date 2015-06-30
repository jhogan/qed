using System;
using System.Collections;

namespace JCSLA {
	public class BrokenRules :CollectionBase{
		public BrokenRules() {
		}
		public void Assert(string name, string desc, bool isBroken) {
			if (isBroken) {
				BrokenRule rule = new BrokenRule();
				rule.Name = name;
				rule.Desc = desc;
				this.Add(rule);	
			}
		}
		public void Assert(string desc, bool isBroken) {
			if (isBroken) {
				BrokenRule rule = new BrokenRule();
				rule.Name ="";
				rule.Desc = desc;
				this.Add(rule);	
			}
		}
		public void Add(BrokenRule rule) {
			List.Add(rule);
		}
		public void Add(string desc){
			BrokenRule rule = new BrokenRule();
			rule.Name=""; rule.Desc = desc;
			this.Add(rule);
		}
		public void Add(BusinessBase bb) {
			if (bb != null)
				this.Add(bb.BrokenRules);
		}
		public void Add(BusinessCollectionBase bcb) {
			if (bcb != null)
				this.Add(bcb.BrokenRules);
		}
		/*public void Add(object obj){
			if (obj is BusinessCollectionBase)
				this.Add((BusinessCollectionBase)obj);
			else if (obj is BusinessBase)
				this.Add((BusinessBase)obj);
			else
				throw new Exception("Can't add type:" + obj.GetType().Name + " to broken rules collection.");
		}*/
		public void Add(BrokenRules rules) {
			foreach(BrokenRule rule in rules) {
				List.Add(rule);
			}
		}
		public override string ToString() {
			string brokenRules = "";
			foreach (BrokenRule rule in List) {
				brokenRules += rule.Desc + "\r\n";
			}
			return brokenRules;
		}
		public BrokenRules GetWithName(string name){
			BrokenRules ret = new BrokenRules();
			foreach(BrokenRule br in this){
				if (br.Name == name){
					ret.Add(br);
				}
			}
			return ret;
		}
	}
	public struct BrokenRule {
		public string Name;
		public string Desc;
	}
}
