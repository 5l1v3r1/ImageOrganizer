using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ImageOrganizer
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				if (args.Length >= 2)
				{
					DirectoryInfo diSource = new DirectoryInfo(args[0]);
					DirectoryInfo diTarget = new DirectoryInfo(args[1]);

					bool move = false;
					bool creationTime = false;
					string filter = "jpg";
					for (int i = 2; i < args.Length; i++)
					{
						if (args[i] == "/M")
							move = true;
						else if (args[i] == "/C")
							creationTime = true;
						else if (args[i].StartsWith("/E:"))
							filter = args[i].Substring(3);
					}
					HashSet<string> extensions = new HashSet<string>(filter.Split('/'));
					FileInfo[] sources = diSource.GetFiles("*", SearchOption.AllDirectories);
					if (!diTarget.Exists)
						Directory.CreateDirectory(diTarget.FullName);
					string targetDirNormalized = diTarget.FullName.TrimEnd('\\', '/') + "/";
					foreach (FileInfo fi in sources)
					{
						if (filter.Contains(fi.Extension.StartsWith(".") ? fi.Extension.Substring(1) : (string.IsNullOrEmpty(fi.Extension) ? fi.Name : fi.Extension)))
						{
							string timestamp = (creationTime ? fi.CreationTime : fi.LastWriteTime).ToString("yyyyMMddHHmmssfff");
							int counter = 1;
							string fileName = GetFileName(targetDirNormalized, timestamp, fi.Extension, counter);
							while (File.Exists(fileName))
								fileName = GetFileName(targetDirNormalized, timestamp, fi.Extension, ++counter);
							if (move)
								fi.MoveTo(fileName);
							else
								fi.CopyTo(fileName, false);
						}
					}
				}
				else
				{
					OutputUsage();
				}

			}
			catch (Exception ex)
			{
				ReportError(ex);
			}
		}

		private static string GetFileName(string targetDirNormalized, string timestamp, string extension, int counter)
		{
			if (counter < 2)
				return targetDirNormalized + timestamp + extension;
			return targetDirNormalized + timestamp + " " + counter + extension;
		}

		private static void ReportError(Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(ex.ToString());
			Console.ResetColor();
		}

		private static void OutputUsage()
		{
			FileInfo fi = new FileInfo(Environment.GetCommandLineArgs()[0]);
			Console.WriteLine();
			Console.WriteLine("Copies files from the source directory and child directories, ");
			Console.WriteLine("consolidating them in the target directory and renaming the ");
			Console.WriteLine("files to the format yyyyMMddHHmmssfff + .extension where the ");
			Console.WriteLine("timestamp is the last modified date's year, month, day, hour, ");
			Console.WriteLine("minute, second, millisecond.");
			Console.WriteLine();
			Console.WriteLine("\tNote: Files will not be overwritten.  If a file ");
			Console.WriteLine("\talready exists, the new file will have a space and an ");
			Console.WriteLine("\tidentifying number will be appended to the timestamp ");
			Console.WriteLine("\tin the file name (e.g. 20140408010101000.jpg and ");
			Console.WriteLine("\t20140408010101000 2.jpg and 20140408010101000 3.jpg)");
			Console.WriteLine();
			Console.WriteLine("Usage: " + fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + " source target [/C] [/M] [/E:Extension1[/Extension2][/Extension3]...]");
			Console.WriteLine();
			Console.WriteLine("\t/C\tNames the files based on creation time instead of modified time.");
			Console.WriteLine("\t/M\tMoves the files instead of only copying them.");
			Console.WriteLine("\t/E:\tSpecify the extension(s) of the files to move or copy.");
			Console.WriteLine("\t\tIf unspecified, only jpg files will be affected.");
			Console.WriteLine();
			Console.WriteLine("\t\tExample: /E:jpg/jpeg/bmp/png/gif/webp/raw");
		}
	}
}
