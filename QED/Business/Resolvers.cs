using System;
using System.Collections;
namespace QED.Business {
	/// <summary>
	/// Summary description for Resolvers.
	/// </summary>
	public class People {
		public People(){}
	}
	public class Resolvers {

		ArrayList _resolvers = new ArrayList();
		public Resolvers() {
		}
	}
	public class Resolver {
		int _id;
		string _resource;  
		public Resolver(int id) {
			_id = id;
		}
		/*
		public string Resource {
			get {
			}
		}
		*/
	}


}
