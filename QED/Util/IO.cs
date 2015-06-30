using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;

namespace QED.Util
{
	/// <summary>
	/// Summary description for IO.
	/// </summary>
	public class IO {
		public IO() {
		}
		public static bool Copy(DirectoryInfo src, DirectoryInfo dest, bool force){
			FileInfo[] files; DirectoryInfo[] subDirs; FileInfo destFile; FileAttributes attr;
			char ps = Path.DirectorySeparatorChar;
			files = src.GetFiles();
			subDirs = src.GetDirectories();
			if (dest.Exists && !force) return false;
			if ( !dest.Exists) dest.Create();
			foreach(FileInfo file in files) {
				destFile = new FileInfo(dest.FullName + ps + file.Name);
				if (destFile.Exists){
					attr = destFile.Attributes;
					if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)	{
						File.SetAttributes(destFile.FullName, (attr &(~System.IO.FileAttributes.ReadOnly)));
					}
				}
				file.CopyTo(destFile.FullName, true);
			}
			foreach(DirectoryInfo dir in subDirs) {
				Copy(dir, new DirectoryInfo(dest.FullName + ps + dir.Name), true);
			}
			return true;
		}
		public static string ReadFile(FileInfo file) {
			StreamReader sr = file.OpenText();
			string ret = sr.ReadToEnd();
			sr.Close();
			return ret;
		}
		public static void OverwriteFile(FileInfo file, string with){
			FileInfo tmp = new FileInfo(file.FullName + ".tmp");
			StreamWriter sw = new StreamWriter(tmp.FullName);
			sw.Write(with);
			sw.Close();
			file.Delete();
			tmp.MoveTo(file.FullName);
		}
		public static void Zip(DirectoryInfo src, FileInfo destZip, bool force) {
			FileInfo[] files;
			ZipEntry entry;
			char ps = Path.DirectorySeparatorChar;
			if (force){
				if (destZip.Exists){
					destZip.Delete();
					destZip.Refresh();
				}
			}
			if (!destZip.Exists){
				files = GetFilesRecursively(src);
				ZipOutputStream zos = new ZipOutputStream(destZip.Open(FileMode.Create, FileAccess.ReadWrite));
				zos.SetLevel(9); 
				int fileOffset = src.FullName.Length + ((src.FullName.EndsWith(@"\")) ? 0 : 1);
				foreach (FileInfo file in files) {
					FileStream fs = file.OpenRead();
					byte[] buffer = new byte[fs.Length];
					fs.Read(buffer, 0, buffer.Length);
					entry = new ZipEntry(file.FullName.Substring(fileOffset));
					zos.PutNextEntry(entry);	
					zos.Write(buffer, 0, buffer.Length);
				}
				zos.Finish();
				zos.Close();
			}
		}
		
		public static FileInfo[] GetFilesRecursively(DirectoryInfo forDir){
			int i = 0;
			ArrayList files = new ArrayList();
			GetFilesRecursively(forDir, files);
			FileInfo[] ret = new FileInfo[files.Count];
			foreach (FileInfo file in files){
				ret[i++] = (FileInfo)file;
			}
			return ret;
		}

		private static void GetFilesRecursively(DirectoryInfo forDir, ArrayList files){
			foreach (FileInfo file in forDir.GetFiles()) {
				files.Add(file);
			}
			foreach (DirectoryInfo dir in forDir.GetDirectories()){
				GetFilesRecursively(dir, files);
			}
		}
		public static FileSystemInfo[] GetFileSystemInfosRecursively(DirectoryInfo forDir){
			int i = 0;
			ArrayList FileSystemInfos = new ArrayList();
			GetFileSystemInfosRecursively(forDir, FileSystemInfos);
			FileSystemInfo[] ret = new FileSystemInfo[FileSystemInfos.Count];
			foreach (FileSystemInfo fsi in FileSystemInfos){
				ret[i++] = fsi;
			}
			return ret;
		}

		private static void GetFileSystemInfosRecursively(DirectoryInfo forDir, ArrayList FileSystemInfos){
			foreach (FileSystemInfo fsi in forDir.GetFileSystemInfos()) {
				FileSystemInfos.Add(fsi);
			}
			foreach (DirectoryInfo dir in forDir.GetDirectories()){
				GetFileSystemInfosRecursively(dir, FileSystemInfos);
			}
		}
		public static void UnsetReadOnly(DirectoryInfo dir){
			FileInfo[] files = GetFilesRecursively(dir);
			foreach(FileInfo file in files){
				FileAttributes attr = file.Attributes;
				if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)	{
					File.SetAttributes(file.FullName, (attr &(~System.IO.FileAttributes.ReadOnly)));
				}
			}
		}
		public static FileSystemInfo PathToFSI(string path){
			FileInfo file = new FileInfo(path);
			if (file.Exists) return file;
			DirectoryInfo dir = new DirectoryInfo(path);
			if (dir.Exists) return dir;
			return null;
		}
	}
}
