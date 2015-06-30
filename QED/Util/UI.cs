using System;
using System.Windows.Forms;
using JCSLA;
using QED.Business;
namespace QED.UI
{
	/// <summary>
	/// Summary description for UI.
	/// </summary>
	public class UI
	{
		public const string QED_CLIENT_ID = "1.4.0";
		public static string n = System.Environment.NewLine;
		public UI()
		{
		}
		public static DialogResult AskYesNoDefNo(IWin32Window owner, string msg){
			return MessageBox.Show(owner, msg, "QED", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
		}
		public static DialogResult AskYesNoDefYes(IWin32Window owner, string msg){
			return MessageBox.Show(owner, msg, "QED", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		}
		public static bool ClientVerMatchesDBVer(){
			CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
			CatList system =  cls.item("System");
			bool matches = true;
			int majorMax =0;
			int minorMax =0;
			int revMax = 0;

			int majorMin =0;
			int minorMin =0;
			int revMin = 0;

			int majorClient =0;
			int minorClient =0;
			int revClient = 0;

			string dbVersion = system.Entry("/Versioning/dbVersion").Value;
			string maxClientVer = system.Entry("/Versioning/MaxClientVersion").Value;
			string minClientVer = system.Entry("/Versioning/MinClientVersion").Value;
			
			// Parse Maximum Client Version Number
			majorMax = int.Parse(maxClientVer.Split('.')[0]);
			if (maxClientVer.Split('.').Length > 1) minorMax = int.Parse(maxClientVer.Split('.')[1]);
			if (maxClientVer.Split('.').Length > 2) revMax = int.Parse(maxClientVer.Split('.')[2]);

			// Parse Minimum Client Version Number
			majorMin = int.Parse(minClientVer.Split('.')[0]);
			if (minClientVer.Split('.').Length > 1) minorMin = int.Parse(minClientVer.Split('.')[1]);
			if (minClientVer.Split('.').Length > 2) revMin = int.Parse(minClientVer.Split('.')[2]);

			// Parse Actual Client Version Number
			majorClient = int.Parse(QED_CLIENT_ID.Split('.')[0]);
			if (QED_CLIENT_ID.Split('.').Length > 1) minorClient = int.Parse(QED_CLIENT_ID.Split('.')[1]);
			if (QED_CLIENT_ID.Split('.').Length > 2) revClient = int.Parse(QED_CLIENT_ID.Split('.')[2]);
			

			if (majorClient > majorMax || majorClient < majorMin){
				matches = false;
			}else{
				if (majorClient == majorMax){
					if (minorClient > minorMax){
						matches = false;
					}
					if (minorClient == minorMax){
						if (revClient > revMax){
							matches = false;
						}
					}

				}
				if (majorClient == majorMin){
					if (minorClient < minorMin){
						matches = false;
					}
					if (minorClient == minorMin){
						if (revClient < revMin){
							matches = false;
						}
					}
				}
			}
			return matches;
		}
		public static bool DemandClientVerMatchesDBVer(IWin32Window owner){
			if (!UI.ClientVerMatchesDBVer()){
				CatLists cls = new CatLists(Connections.Inst.item("QED_DB").MySqlConnection);
				CatList system =  cls.item("System");
				string dbVersion = system.Entry("/Versioning/dbVersion").Value;
				string maxClientVer = system.Entry("/Versioning/MaxClientVersion").Value;
				string minClientVer = system.Entry("/Versioning/MinClientVersion").Value;
				string latestVerLoc =  system.Entry("/Versioning/LatestVersion").Value;
				string verMsg ="Client Version: " + QED_CLIENT_ID + "\r\n";
				verMsg += "Database version: " + dbVersion + "\r\n";
				verMsg += "Maximum client version supported: " + maxClientVer + "\r\n";
				verMsg += "Minimum client version supported: " + minClientVer;

				MessageBox.Show(owner, "This version of QED does not match the database version.\r\n"	+
														"You need to get the latest version of the client at: " + latestVerLoc + "\r\n" + verMsg + "\r\n"+
														"Operation will abort.",
														"QED");
				return false;
			}
			return true;
		}
		public static Control GetControlByName(Form form, string name){
			foreach(Control con in form.Controls){
				if (con.Name == name){
					return con;
				}
			}
			return null;
		}
		public static TabPage GetTabByName(TabControl tabCtrl, string name){
			foreach(TabPage tp in tabCtrl.TabPages){
				if (tp.Name == name){
					return tp;
				}
			}
			return null;
		}
		public static Control GetControlByName(TabPage tp, string name){
			foreach(Control con in tp.Controls){
				if (con.Name == name){
					return con;
				}
			}
			return null;
		}
		public static  bool UpdateIfValid(IWin32Window owner, BusinessBase bb){
			if (bb.IsValid){
				bb.Update();
				return true;
			}else{
				MessageBox.Show(owner, "Could not save " + bb.GetType().Name + " because it was invalid. Reason:\r\n" + bb.BrokenRules.ToString(), "Error Saving", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
				return false;
			}
		}
		public static void ShowException(IWin32Window owner, Exception ex){
			MessageBox.Show(owner, "An exception was thrown " + n + ex.Message, "QED");
		}
	}
}
