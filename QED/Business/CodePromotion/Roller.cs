using System;
using System.IO;
using QED.Business;
using QED.Util;
using QED.Net;
using QED.SEC;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using JCSLA;
namespace QED.Business.CodePromotion {
	public enum RollType{
		Remote_UAT, Local_UAT, Prod_Prep, Prod, Merge, General
	}
	public abstract class Roller {
		RollType _rollType;
		Thread _rollThread;
		DateTime _dateTimeStarted ;
		RolloutLog _log; // = new RolloutLog();
		protected string ps = "\\";
		protected string n = System.Environment.NewLine;
		public delegate void ReportHandler(Roller roller, ReportEventArgs reportEventArgs);
		public ReportHandler OnReport;

		public delegate void PromptHandler(Roller roller, EventArgs EventArgs);
		public PromptHandler OnPrompt;

		public delegate void CompletedHandler(Roller roller, ReportEventArgs reportEventArgs);
		public CompletedHandler OnCompleted;
		
		private void LogRollEntry(string msg){
			_log.Append(msg);
			_log.UserEmail = System.Threading.Thread.CurrentPrincipal.Identity.Name;
			_log.RollClass = this.GetType().Name;
			_log.RollType = this.RollType;
			_log.Update();
		}
		#region Report Helper Methods
		protected void Report(object obj){
			string msg = obj.ToString();
			LogRollEntry(msg);
			OnReport(this, new ReportEventArgs(msg));
		}
		protected void Report(string msg){
			LogRollEntry(msg);
			OnReport(this, new ReportEventArgs(msg));
		}
		protected void ReportCopy(FileSystemInfo from, FileSystemInfo to){
			Report("Copying " + from.FullName + " to " + to.FullName);
		}
		protected void ReportCopy(DirectoryInfo dir){
			Report("Deleting " + dir.FullName);
		}
		protected void ReportAndCopy(DirectoryInfo from, DirectoryInfo to, bool force){
			ReportCopy(from, to);
			IO.Copy(from, to, force);
		}	
		protected void ReportAndCopy(FileInfo from, FileInfo to, bool force){
			ReportCopy(from, to);
			from.CopyTo(to.FullName, force);
		}
		protected void ReportAndZip(DirectoryInfo from, FileInfo  to, bool force){
			Report("Zipping " + from.FullName + " to " + to.FullName);
			IO.Zip(from, to, force);
		}
		
		protected void ReportAndDelete(FileSystemInfo fsi){
			Report("Delete " + fsi.FullName);
			this.DeleteFileIfExists(fsi);
		}
		protected void ReportAndEmptyDir(DirectoryInfo dir){
			Report("Empty Working Directory: " + dir.FullName);
			this.EmptyWorkingDir(dir);
		}
		protected void ReportAndCheckoutCVS(string CVSUser, 
			string CVSServer, string CVSPath, 
			string CVSModule, DirectoryInfo CVSExportRoot, string branch, bool report){
			ShellCmd cmd;
			if (report) Report("Exporting CVS files"); 
			cmd = new ShellCmd(CVS, 
				" -d :pserver:" + CVSUser + "@cvs.:" + CVSPath + " checkout  -r " + branch + " " + CVSModule , CVSExportRoot, false);
			if (report) Report(cmd.ToString() + " --(This may take a minute.)");
			cmd.Run();
			cmd.WaitForCommandToFinish();
		}
		protected bool ReportAndLoginToCVS(string CVSUser, string CVSPass, string CVSServer, string CVSPath){
			ShellCmd cmd;
			Report("Login to CVS");
			cmd = new ShellCmd(CVS, 
				" -d :pserver:" + CVSUser + "@cvs.:" + CVSPath + " login", true);
			cmd.Stdin.WriteLine(CVSPass);
			cmd.WaitForCommandToFinish();
			Report(cmd.ToString());
			cmd.WaitForCommandToFinish();
			return true;
		}
		#endregion

		protected void DeleteFileIfExists(FileSystemInfo file) {
			if (file.Exists){
				Report("Deleting " + file.FullName);
				if (file is DirectoryInfo)
					((DirectoryInfo)file).Delete(true);
				else
					file.Delete();
			}
		}
		protected void ReportAndCopy(FileSystemInfo src, FileSystemInfo dest, bool force){
			if (src is FileInfo){
				FileInfo srcFile = (FileInfo) src;
				if (dest is DirectoryInfo){
					DirectoryInfo destDir = (DirectoryInfo) dest;
					ReportAndCopy(srcFile, destDir, true);
				}else if (dest is FileInfo){
					FileInfo destFile = (FileInfo) dest;
					ReportAndCopy(srcFile, destFile, true);
				}
			}else if (src is DirectoryInfo){
				DirectoryInfo srcDir = (DirectoryInfo) src;
				if (dest is DirectoryInfo){
					DirectoryInfo destDir = (DirectoryInfo) dest;
					ReportAndCopy(srcDir, destDir, true);
				}else if (dest is FileInfo){
					throw new Exception("Can't copy a directory to a file");
				}
			}
		}
		protected DialogResult AskYesNotDefNo(string msg){
			DialogEventArgs args = new DialogEventArgs(msg, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			OnPrompt(this, (EventArgs)args);
			return args.DialogResults;
		}
		protected Hashtable Ask(string prompt, params string[] questions){
			AskEventArgs args = new AskEventArgs(prompt, questions);
			OnPrompt(this, (EventArgs)args);
			return args.AnswerTable;
		}
		public Roller(RollType rollType){
			_rollType = rollType;
		}
		protected abstract void BK();
		protected abstract void GET();
		protected abstract void PREP();
		protected abstract void PUT();
		public abstract void Roll();
		public abstract bool IsRolling{
			get;
			set;
		}
		protected void roll() {
			if (!this.IsRolling){
				_rollThread = new Thread(new ThreadStart(_roll));
				_rollThread.Name = this.ToString();
				_rollThread.Start();
			}else{
				throw new Exception("Operations is currently in progress.");
			}
		}
		private  void _roll() {
			try{
				if (UI.UI.ClientVerMatchesDBVer()){
					this.IsRolling = true;
					this._dateTimeStarted = DateTime.Now;
					_log = new RolloutLog();
					Report("-".PadLeft(87, '-'));
					Report("Begin rollout at " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
					Report("Roller class: " + this.GetType().Name + "\r\nRollType = " + this.RollType.ToString());
					Report("-".PadLeft(87, '-'));
					this.BK();
					this.GET();
					this.PREP();
					this.PUT();
					this.IsRolling = false;
					Report("---Rollout has finished---");
					OnCompleted(this, new ReportEventArgs("Rollout completed successfully"));
				}else{
					throw new Exception("Client version is out of synch with DB version. Please update QED client before performing this roll.");
				}
			}
			catch(Exception ex){
				this.IsRolling = false;
				Report(ex.Message + "\r\n" + ex.StackTrace);
				throw new Exception(ex.Message, ex);
			}
		 }

		public static string DirTimeStamp{
			get{
				string ret;
				DateTime now =DateTime.Now;
				ret = now.Year.ToString().PadLeft(4, '0');
				ret += now.Month.ToString().PadLeft(2, '0');
				ret += now.Day.ToString().PadLeft(2, '0');
				return ret;
			}
		}
		public RollType RollType{
			get{
				return _rollType;
			}
			set{
				_rollType = value;
			}
		}
		public DirectoryInfo BinDir{
			get{
				return new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\bin");
			 }
		}
		public string CVS{
			get{
				return this.BinDir.GetFiles("cvs.exe")[0].FullName;
			}
		}
		public string Plink{
			get{
				return this.BinDir.GetFiles("plink.exe")[0].FullName;
			}
		}
		public string CVSUser{
			get{
				return ((BusinessIdentity)System.Threading.Thread.CurrentPrincipal.Identity).UserName;
			}
		}
		public void EmptyWorkingDir(DirectoryInfo WorkingDir){
			FileSystemInfo[] fsis = IO.GetFileSystemInfosRecursively(WorkingDir);
			FileInfo file;
			DirectoryInfo dir;
			foreach(FileSystemInfo fsi in fsis){
				if (fsi.Exists){
					if (fsi is DirectoryInfo){
						if (fsi.Name == "CVS"){
							dir = (DirectoryInfo)fsi;
							dir.Attributes = 0;
							dir.Delete(true);
						}
					}else{
						file = ((FileInfo)fsi);
						file.Attributes = 0;
						file.Delete();
					}
				}
			}
		}
		public override string ToString() {
			return this.GetType().Name + " " + this.RollType.ToString();
		}
		public string InstanceId{
			get{
				return this.GetType().Name + "_" + this.RollType.ToString() + "_" + this.DateTimeStarted.Ticks;
			}
		}
		public DateTime DateTimeStarted{
			get{
				return _dateTimeStarted;
			}
		}

	}

	#endregion
	#region General Roller
	public enum SourceType{
		CVS, Directory
	}
	public class General_Roller : Roller {
		bool _dry = false;
		static bool _isRolling;
		string _plan = "";
		GeneralRollSrc _genSrc;
		GeneralRollDests _genDests;
		public bool _cvsPullComplete;
		public General_Roller() : base(RollType.General){
			_genSrc = new GeneralRollSrc(this);
			_genDests = new GeneralRollDests(this);
		}
		protected override void BK() {
			if ( ! this.IsValid) throw new Exception("Can't run " + this.GetType().ToString() + " because it is in an invalid state");
			_plan += "BACKUP:" + n;
			FileSystemInfo qualifiedDestBK;
			FileSystemInfo fsi;
			foreach(GeneralRollDest dest in this.GeneralRollDestinations){
				foreach(string path in dest.PathsToRoll){
					fsi = IO.PathToFSI(path);
					if (fsi != null){ // fsi represents an existing FileInfo or DirectoryInfo
						qualifiedDestBK = dest.GetQualifiedBackup(fsi);
						if ( fsi.Exists){
							if ( !_dry){
								EnsureDirectoryExists(qualifiedDestBK);
								ReportAndCopy(fsi, qualifiedDestBK, false);
							}else{
								_plan += "\tCopy " + fsi.FullName + " " + qualifiedDestBK.FullName + n;
							}
						}
					}
				}
			}
		}
		
		protected override void GET() {
		}
		protected override void PREP() {}
		protected override void PUT() {
			_plan += "DEPLOY:" + n;
			FileSystemInfo destFSI;
			foreach(FileSystemInfo srcFSI in _genSrc.FileSystemInfosToRoll){
				foreach(GeneralRollDest genDest in this.GeneralRollDestinations){
					destFSI = genDest.GetDestFSI(srcFSI);
					if (!_dry){
						if (!srcFSI.Exists) throw new Exception(srcFSI.FullName + " was scheduled to roll but isn't available.");
						this.EnsureDirectoryExists(destFSI);
						ReportAndCopy(srcFSI, destFSI, true);
					}else{
						_plan += "\tCopy " + srcFSI.FullName + " " + destFSI.FullName + n;
					}
				}
			}
		}

		public void UpdateCVSWorkingCopy(){
			if (_genSrc.SourceType == SourceType.CVS){
				if ( ! _genSrc.CVSExportRoot.Exists) _genSrc.CVSExportRoot.Create();
				ReportAndCheckoutCVS(_genSrc.CVSUser,  _genSrc.CVSServer, _genSrc.CVSPath, _genSrc.CVSModule, _genSrc.CVSExportRoot, _genSrc.Branch, false);
			}else{
				throw new Exception("Can't update CVS on roller that isn't of CVS source type.");
			}
		}
		public override void Roll() {
			base.roll();
		}
		public override bool IsRolling {
			get {
				return _isRolling;
			}
			set {
				_isRolling = value;
			}
		}
		public string Plan{
			get{
				_plan = "";
				BrokenRules rules = this.BrokenRulesWarnings;
				if (rules.Count > 0){
					foreach (BrokenRule rule in rules){
						_plan += "WARNING: " + rule.Desc + n;
					}

				}
				if (this.IsValid){
					_dry = true;
					this.BK(); this.GET(); this.PREP(); this.PUT();
					_dry = false;
				}else{
					_plan += "Plan can not be created yet because the information supplied is either invalid or incomplete. See below:" + n + this.BrokenRules;
				}
				return _plan;
			}
		}
		public GeneralRollSrc GeneralRollSource{
			get{
				return _genSrc;
			}
			set{
				_genSrc = value;
			}
		}
		public GeneralRollDests GeneralRollDestinations{
			get{
				return _genDests;
			}
			set{
				_genDests = value;
			}
		}
			
		public void Clear(){
			this.GeneralRollDestinations = new GeneralRollDests(this);
			this.GeneralRollSource = new GeneralRollSrc(this);
			this.GeneralRollSource.RelativeFileSystemInfosToRoll.Clear();
			this.GeneralRollDestinations.Clear();
		}
		public void EnsureDirectoryExists(FileSystemInfo fsi){
			DirectoryInfo dir;
			if (fsi is DirectoryInfo)
				dir =(DirectoryInfo)fsi;
			else
				dir = ((FileInfo)fsi).Directory;
			if (!dir.Exists) dir.Create();
		}
		public bool IsValid{
			get{
				return (this.BrokenRules.Count == 0);
			}
		}
		public BrokenRules BrokenRulesWarnings{
			get{
				BrokenRules ret = new BrokenRules();
				FileSystemInfo destFSI;
				foreach(FileSystemInfo srcFSI in _genSrc.FileSystemInfosToRoll){
					foreach(GeneralRollDest genDest in this.GeneralRollDestinations){
						destFSI = genDest.GetDestFSI(srcFSI);
						ret.Assert("FYI: The destination file or directory " + destFSI.FullName + " doesn't exist yet. It will be created after you roll", !destFSI.Exists);
					}
				}
				return ret;
			}
		}
		public BrokenRules BrokenRules{
			get{
				BrokenRules ret = new BrokenRules();
				if (this.GeneralRollSource.SourceType == SourceType.CVS){
					ret.Assert("CVS Branch required", this.GeneralRollSource.Branch == "");
					if (this.GeneralRollSource.CVSExportRoot == null)
						ret.Add("CVS Export Root required");

					ret.Assert("CVS Module required", this.GeneralRollSource.CVSModule == "");
					ret.Assert("CVS Path required", this.GeneralRollSource.CVSPath == "");
					ret.Assert("CVS Server required", this.GeneralRollSource.CVSServer == "");
					ret.Assert("CVS User required", this.GeneralRollSource.CVSUser == "");
				}else{
					if (this.GeneralRollSource.BaseDir == null)
						ret.Add("Source Directory required");
					else
						ret.Assert("Source directory doesn't exists", !this.GeneralRollSource.BaseDir.Exists);
				}
				ret.Assert("At lease one source file or directory needs to be provided",
					this.GeneralRollSource.RelativeFileSystemInfosToRoll.Count == 0);

				ret.Assert("At lease one destination is required", this.GeneralRollDestinations.Count == 0);
				foreach(GeneralRollDest dest in this.GeneralRollDestinations){
					if (dest.BaseDir != null){
						ret.Assert("Destination directory doesn't exist: " + dest.BaseDir.FullName, !dest.BaseDir.Exists);
					}else{
						ret.Add("Destination directory missing");
					}
					if (dest.BackupDir != null){
						ret.Assert("Backup directory doesn't exist: " + dest.BackupDir.FullName, !dest.BackupDir.Exists);
					}else{
						ret.Add("Backup directory missing for destination for " + dest.BaseDir.FullName);
					}
				}
				foreach(FileSystemInfo srcFSI in _genSrc.FileSystemInfosToRoll){
					ret.Assert("NO_SOURCE_FILE", "Source file or directory doesn't exist: " + srcFSI.FullName + "." , !srcFSI.Exists);
				}
				return ret;
			}
		}
	}
	public class GeneralRollSrc{
		General_Roller _genRoller;
		protected string ps = "\\";
		string n = System.Environment.NewLine;
		public GeneralRollSrc(General_Roller genRoller){
			_genRoller = genRoller;
		}
		DirectoryInfo _cvsExportRoot;
		DirectoryInfo _baseDir;
		string _cvsUser = "";
		string _cvsServer = "";
		string _cvsPath = "";
		string _cvsModule = "";
		string _branch = "";
		SourceType _sourceType;
		ArrayList _relativefileSystemInfosToRoll = new ArrayList();
		public void AddSourceFileSystemInfo(string fsi){
			this.RelativeFileSystemInfosToRoll.Add(fsi);
		}
		public ArrayList RelativeFileSystemInfosToRoll{
			get{
				return _relativefileSystemInfosToRoll;
			}
		}
		public FileSystemInfo[] FileSystemInfosToRoll{
			get{
				if (this.BaseDir == null) return new FileSystemInfo[0];
				FileSystemInfo[] ret = new FileSystemInfo[this.RelativeFileSystemInfosToRoll.Count];
				int i=0;
				FileInfo file;
				DirectoryInfo dir;
				foreach (string relative in this.RelativeFileSystemInfosToRoll){
					file = new FileInfo(this.BaseDir.FullName + ps + relative.Replace("/", @"\" ));
					if (file.Exists){
						ret[i++] = file;
					}else{
						dir = new DirectoryInfo(this.BaseDir.FullName + ps + relative.Replace("/", @"\" ));
						ret[i++] = dir;
					}
				}
				return ret;
			}
		}
		public string CVSUser{
			get{
				return _cvsUser.Trim();
			}
			set{
				 _cvsUser= value;
			}
		}
		public string CVSServer{
			get{
				return _cvsServer.Trim();
			}
			set{
				 _cvsServer= value;
			}
		}
		public string CVSPath{
			get{
				return _cvsPath.Trim();
			}
			set{
				 _cvsPath= value;
			}
		}
		public string CVSModule{
			get{
				return _cvsModule.Trim();
			}
			set{
				 _cvsModule= value;
			}
		}
  
		public DirectoryInfo BaseDir{
			get{
				if (this.SourceType == SourceType.CVS)
					return new DirectoryInfo(this.CVSExportRoot.FullName + ps + this.CVSModule.Replace("/", "\\"));
				else
					return _baseDir;
			}
			set{
				if (this.SourceType == SourceType.CVS) throw new Exception("Can't set BaseDir for CVS SourceType");
				_baseDir = value;
			}
		}
		public DirectoryInfo CVSExportRoot{
			get{
				return _cvsExportRoot;
			}
			set{
				if (this.SourceType == SourceType.Directory) throw new Exception("Can't set CVSExportRoot for Directory SourceType");
				_cvsExportRoot = value;
			}
		}
		public SourceType SourceType{
			get{
				return _sourceType;
			}
			set{
				_sourceType = value;
			}
		}
		public string Branch{
			get{
				return _branch.Trim();
			}
			set{
				_branch = value;
			}
		}

	}
	public class GeneralRollDests : CollectionBase{
		General_Roller _genRoller;
		public GeneralRollDests(General_Roller genRoller){
			_genRoller = genRoller;
		}
		public GeneralRollDest CreateNew(){
			GeneralRollDest ret = new GeneralRollDest(_genRoller);
			List.Add(ret);
			return ret;
		}
	}
	public class GeneralRollDest{
		GeneralRollSrc _src;
		DirectoryInfo _backupDir;
		DirectoryInfo _baseDir;
		General_Roller _genRoller;
		string n = System.Environment.NewLine;
		string ps = "\\";
		public DirectoryInfo BaseDir{
			get{
				return _baseDir; 
			}
			set{
				_baseDir = value;
			}
		}
		public GeneralRollDest(General_Roller genRoller){
			_genRoller = genRoller;
			_src = _genRoller.GeneralRollSource;
		}
		public FileSystemInfo[] FileSystemInfosToRoll{
			get{
				string ex_relative = "";
				try{
					FileSystemInfo[] ret = new FileSystemInfo[_src.RelativeFileSystemInfosToRoll.Count];
					int i = 0;
					foreach(string relative in _src.RelativeFileSystemInfosToRoll){
						ex_relative = relative;
						ret[i++] = new FileInfo(this._baseDir.FullName + @"\" + relative);
					}
					return ret;
				}
				catch(ArgumentException ex){
					throw new Exception("ArgumentException was encountered. " + 
													"There may be an error in the syntax of the relative source path." + n + 
													"Relative source path: " + ex_relative + n +
												    "Base directory: " + this._baseDir.FullName + n + ex.Message) ;
				}
			}
		}
		public string[] PathsToRoll{
			get{
				string ex_relative = "";
				string [] ret = new string[_src.RelativeFileSystemInfosToRoll.Count];
				int i = 0;
				foreach(string relative in _src.RelativeFileSystemInfosToRoll){
					ex_relative = relative;
					ret[i++] = this._baseDir.FullName + @"\" + relative;
				}
				return ret;
			}
		}
		public FileSystemInfo GetDestFSI(FileSystemInfo srcFSI){
			string relativePath = srcFSI.FullName.Substring(this._genRoller.GeneralRollSource.BaseDir.FullName.Length);
			if (srcFSI is FileInfo)
				return new FileInfo(this.BaseDir.FullName + ps + relativePath);
			else
				return new DirectoryInfo(this.BaseDir.FullName + ps + relativePath);
		}
		public DirectoryInfo BackupDir{
			get{
				return _backupDir;
			}
			set{
				_backupDir = value;
			}
		}
		public FileSystemInfo GetQualifiedBackup(FileSystemInfo fsi){
			DirectoryInfo dir = null;
			if (fsi is FileInfo)
				dir = ((FileInfo)fsi).Directory;
			else
				dir = (DirectoryInfo)fsi;
			string relative = dir.FullName.Substring(this.BaseDir.FullName.Length);
			string progName = this.BaseDir.Name;
			if (fsi is FileInfo)
				return new FileInfo( this.BackupDir.FullName + ps + progName + "_" + Roller.DirTimeStamp + relative + ps + fsi.Name);
			else
				return new DirectoryInfo( this.BackupDir.FullName + ps + progName + "_" + Roller.DirTimeStamp + relative);
		 }
	}
	#endregion
	#region EventArg Derivitives
	public class ReportEventArgs : EventArgs{
		public readonly string Message = "";
		public ReportEventArgs(string msg){
			Message = msg;
		}
	}
	public class DialogEventArgs : EventArgs{
		public readonly string Message = "";
		public readonly MessageBoxButtons MessageBoxButtons;
		public readonly MessageBoxIcon  MessageBoxIcon;
		public readonly MessageBoxDefaultButton MessageBoxDefaultButton;
		private DialogResult _dialogResult;
		public DialogEventArgs(string msg, MessageBoxButtons mbb, MessageBoxIcon ico, MessageBoxDefaultButton defBtn){
			Message = msg;
			this.MessageBoxButtons = mbb;
			this.MessageBoxIcon = ico;
			this.MessageBoxDefaultButton = defBtn;
		}
		public DialogResult DialogResults{
			get{
				return _dialogResult;
			}
			set{
				_dialogResult = value;
			}
		}
	}
	public class AskEventArgs : EventArgs{
		public readonly string Prompt = "";
		public string[] Questions;
		public Hashtable AnswerTable;
		public DialogResult DialogResult;
		public AskEventArgs(string prompt, params string[] questions){
			Prompt = prompt;
			Questions = questions;
		}
	}
	#endregion
	
	#region Template
	/*public class TEMPLATE_Roller :roller {
		static bool _isRolling;
		public TEMPLATE_Roller(RollType rollType) : base(rollType){
		}
		protected override void BK() {
		}
		protected override void GET() {
		}
		protected override void PREP() {
		
		}
		protected override void PUT() {
		
		}
		public override void Roll() {
			base.roll();
		}
		public override bool IsRolling {
			get {
				return _isRolling;
			}
			set {
				_isRolling = value;
			}
		}
	}
	*/
	#endregion

}

