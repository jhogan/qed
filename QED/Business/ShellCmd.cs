using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
namespace QED.Business
{
	/// <summary>
	/// Wrap System.Diagnostic.Process to make it work better for shelling out.
	/// </summary>
	public class ShellCmd
	{
		Process _proc = new Process();
		
		/*public ShellCmd(DirectoryInfo curDir){
			Environment.CurrentDirectory = curDir.FullName;
		}*/

		public ShellCmd(string cmd, string args, bool runNow) {
			this.SetupStartInfo(cmd, args, new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
			if (runNow)
				_proc.Start();
		}

		public ShellCmd(string cmd, string args, DirectoryInfo curDir, bool runNow)
		{
			this.SetupStartInfo(cmd, args, curDir);
			if (runNow)
				_proc.Start();
		}

		private void SetupStartInfo(string cmd, string args, DirectoryInfo curDir)
		{
			Environment.CurrentDirectory = curDir.FullName;
			_proc.StartInfo.UseShellExecute = false;
			_proc.StartInfo.RedirectStandardOutput = true;
			_proc.StartInfo.RedirectStandardInput = true;
			_proc.StartInfo.FileName = cmd;
			_proc.StartInfo.Arguments = args;
			_proc.StartInfo.CreateNoWindow = true;
		}
		public void Run(){
			_proc.Start();
		}
		public string Stdout
		{
			get
			{
				/* Need to put this on a new thread if we implement this.Strerr */
				string ret = _proc.StandardOutput.ReadToEnd();
				_proc.WaitForExit();
				return ret;
			}
		}
		public void WaitForCommandToFinish(){
			_proc.StandardOutput.ReadToEnd(); // This needs to be called first because of a pipe buffering issue. Consult MSDN's article on the Process class for info.
			_proc.WaitForExit();
		}
		public override string ToString() {
			FileInfo cmd = new FileInfo(_proc.StartInfo.FileName);
			string args = _proc.StartInfo.Arguments;
			return cmd.Name + " " + args;
		}
		public StreamWriter Stdin{
			get{
				return _proc.StandardInput;
			}
		}

		
	}
}
