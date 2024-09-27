using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class Handlers
{
	public static void Download(string dir, string str)
	{
		var req = (HttpWebRequest)WebRequest.Create(str);
		req.Method = "GET";
		req.ContentType = "application/x-www-form-urlencoded";
		req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
		//req.Headers.Add("Cookie", File.ReadAllText("cookie.txt".GetEntryPath()));
	
		var stream = req.GetResponse().GetResponseStream();
		var f = new FileStream(Path.Combine(dir, str.SubstringAfterLast('/')), FileMode.OpenOrCreate);
		stream.CopyTo(f);
		f.Close();
	}

	
	public static void DownloadYouTubeVideo(TextBox textBox)
	{
		var str = textBox.Text.TrimStart('d').SubstringAfterLast('=').SubstringBefore("&");
		Process.Start(new ProcessStartInfo {
			FileName = "yt-dlp_x86.exe",
			Arguments = "--proxy http://127.0.0.1:10809  -f 137 https://www.youtube.com/watch?v=" +
			str,
			WorkingDirectory = @"C:\Users\Administrator\Desktop\视频"
		});
		Process.Start(new ProcessStartInfo {
			FileName = "yt-dlp_x86.exe",
			Arguments = "--proxy http://127.0.0.1:10809  -f 299 https://www.youtube.com/watch?v=" + str,
			WorkingDirectory = @"C:\Users\Administrator\Desktop\视频"
		});
	}
	
	public static void CreateDirectories(TextBox textBox)
	{
		var list = textBox.Text.TrimStart('1').Trim()
				.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
		foreach (var element in list) {
			var array = element.Split('|');
			if (array.Length > 1) {
				for (int i = 1; i < array.Length; i++) {
					var path = Path.Combine(array[0], array[i]);
					Path.GetDirectoryName(path).CreateDirectoryIfNotExists();
					if (path.SubstringAfterLast('\\').Contains("."))
						path.CreateFileIfNotExists();
					else
						path.CreateDirectoryIfNotExists();
				}
			}
		}
	}
	
	public static void CopyResourceFiles(string first)
	{
		var parts = first.TrimStart('2').Trim().Split(new char[]{ '|' }, StringSplitOptions.RemoveEmptyEntries);
			
		var files = Directory.GetFiles(parts[0], "*", SearchOption.AllDirectories)
				.Where(x => Path.GetFileName(x) == parts[1]);
			
		foreach (var element in files) {
			var f = Path.Combine(parts[2], element.SubstringAfter(parts[0] + "\\"));
			Path.GetDirectoryName(f).CreateDirectoryIfNotExists();
			if (!File.Exists(f))
				File.Copy(element, f);
		}
	}
	
	public static void DeleteDirectory(string first)
	{
		foreach (var element in Directory.GetFileSystemEntries(first.TrimStart('x').Trim())) {
			if (File.Exists(element)) {
					
				File.Delete(element);
			} else {
				Directory.Delete(element, true);
			}
		}
	}
	public static void MoveAllTypeFiles(string first)
	{
		var parts = first.TrimStart('9').Split('|');
		var dir1 = parts[0];
		var p = parts[1];
		var dir2 = parts[2];
		var files = Directory.GetFiles(dir1, "*." + p, SearchOption.AllDirectories);
			
		foreach (var element in files) {
			var f = Path.Combine(dir2, Path.GetFileName(element));
			var i = 0;
			while (File.Exists(f)) {
				i++;
				f = Path.Combine(dir2, Path.GetFileNameWithoutExtension(element) + "_" + i + "." + Path.GetExtension(element));
			}
			try {
				File.Move(element, f);
			} catch {
					
			}
		}
	}
	
	public static void CopyBlendFiles(string first)
	{
		var parts = first.TrimStart('9').Split('|');
		var dir1 = parts[0];
		var count = int.Parse(parts[1]);
		var start = 1;
		try {
			start += Directory.GetFiles(dir1, "*.blend")
			.Max(x => {
				var s = Path.GetFileNameWithoutExtension(x);
				if (Regex.IsMatch(s, "^\\d+$")) {
					return int.Parse(s);
				} else {
					return 0;
				}
			});
		} catch {
			
		}
		var str = "1.blend".GetEntryPath();
		for (int i = start; i < start + count; i++) {
			var f = Path.Combine(dir1, i.ToString().PadLeft(3, '0') + ".blend");
			File.Copy(str, f);
		}
	}
	
	public static void Fetch(string uri)
	{
		//if (!uri.Contains("shadertoy.com")) {
		//	return;
		//}
		var id = uri.SubstringAfter("/view/").SubstringBefore("/");
		var req = (HttpWebRequest)WebRequest.Create("https://www.shadertoy.com/shadertoy");
		req.Method = "POST";
		req.ContentType = "application/x-www-form-urlencoded";
		req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
		//req.Headers.Add("Cookie", File.ReadAllText("cookie.txt".GetEntryPath()));
		req.Referer = uri;
		var bytes = new UTF8Encoding(false).GetBytes("s=%7B%20%22shaders%22%20%3A%20%5B%22" + id + "%22%5D%20%7D&nt=1&nl=1&np=1");
		req.GetRequestStream().Write(bytes, 0, bytes.Length);
		using (var reader = new StreamReader(req.GetResponse().GetResponseStream())) {
			var s = reader.ReadToEnd();
			File.WriteAllText((id + ".json").GetDesktopPath(), s);
			//FormatJSON(s, uri);
		}
		
	}
	public static void FormatJSON(string s, string uri)
	{
		var renderpass =	JsonConvert.DeserializeObject<JArray>(s)[0]["renderpass"].ToObject<JArray>();
		string common = string.Empty;
		string image = string.Empty;
		var list = new List<string>();
		foreach (var element in renderpass) {
			if (element["type"].ToString() == "sound") {
				continue;
			} else if (element["type"].ToString() == "common") {
				common = "\r\n\r\n// ==================================\r\n// " + element["name"] + "\r\n// ==================================\r\n" + element["code"] + "\r\n// ==================================\r\n// ==================================\r\n\r\n";
				continue;
			} else if (element["type"].ToString() == "image") {
				image = "\r\n\r\n// ==================================\r\n// " + element["name"] + "\r\n// ==================================\r\n" + element["code"] + "\r\n// ==================================\r\n// ==================================\r\n\r\n";
				continue;
			} else {
				var c = "\r\n\r\n// ==================================\r\n// " + element["name"] + "\r\n// ==================================\r\n" + element["code"] + "\r\n// ==================================\r\n// ==================================\r\n\r\n";
				list.Add(c);
			}
		}
		var b = string.Empty;
		if (list.Count == 0) {
			b = File.ReadAllText("01.html".GetEntryPath());
			b = b.Replace("{{1}}", common + image);
			b = "<!--" + uri + "\n-->\r\n" + b;
		} else {
			b = File.ReadAllText("02.html".GetEntryPath());
			b = b.Replace("{{1}}", common + image);
			var sb = new StringBuilder();
			if (list.Count > 0) {
				
				var bb = File.ReadAllText("03.html".GetEntryPath());
				for (int i = 0; i < list.Count; i++) {
					sb.AppendLine(bb.Replace("{{1}}", common + list[i])
					              .Replace("{{2}}", "b" + list[i].SubstringAfter("// Buffer ")
					                       .SubstringBefore("\n").Trim().ToLower()));
				}
			} 
			b = b.Replace("{{2}}", sb.ToString());
		
			b = "<!--" + uri + "\n-->\r\n" + b;
			
			
		}
		/*
		var req = (HttpWebRequest)WebRequest.Create("http://192.168.8.55:8500/code");
		req.Method = "POST";
		var d = new Dictionary<string,string>();
		d.Add("title", "WebGL");
		d.Add("content", b);
		
		var bytes = new UTF8Encoding(false).GetBytes(JsonConvert.SerializeObject(d));
		req.GetRequestStream().Write(bytes, 0, bytes.Length);
		using (var reader = new StreamReader(req.GetResponse().GetResponseStream())) {
			Console.WriteLine("Success");
		}*/
		
		File.WriteAllText("1.txt".GetDesktopPath(), b);
	}
	
	public static void SaveScripts(string filePath)
	{
		var hd = new HtmlAgilityPack.HtmlDocument();
		hd.LoadHtml(File.ReadAllText(filePath));
		var nodes =	hd.DocumentNode.SelectNodes("//script[not(@src)]");
		var dir="scripts".GetDesktopPath();
		dir.CreateDirectoryIfNotExists();
		var i=0;
		foreach (var element in nodes) {
			i++;
			var f=Path.Combine(dir,i+".js");
			File.WriteAllText(f,element.InnerHtml);
		}
	}
	
}