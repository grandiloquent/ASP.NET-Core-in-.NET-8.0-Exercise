﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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

	
	public static void DownloadYouTubeVideo(string str)
	{
		str = str.SubstringBefore("&").SubstringAfterLast('=');
		Process.Start(new ProcessStartInfo {
			FileName = "yt-dlp_x86.exe",
			Arguments = "--proxy http://127.0.0.1:10808  -f 137 https://www.youtube.com/watch?v=" +
			str,
			WorkingDirectory = @"C:\Users\Administrator\Desktop\视频"
		});
		Process.Start(new ProcessStartInfo {
			FileName = "yt-dlp_x86.exe",
			Arguments = "--proxy http://127.0.0.1:10808  -f 299 https://www.youtube.com/watch?v=" + str,
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
	
	public static void Fetch(string dir, string uri)
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
			File.WriteAllText(Path.Combine(dir, id + ".json"), s);
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
		var dir = "scripts".GetDesktopPath();
		dir.CreateDirectoryIfNotExists();
		var i = 0;
		foreach (var element in nodes) {
			i++;
			var f = Path.Combine(dir, i + ".js");
			File.WriteAllText(f, element.InnerHtml);
		}
	}
	
	public static void ShaderToys()
	{
		var dir = @"C:\Users\Administrator\Desktop\视频\Net\WebApp\ShaderToy\Shaders";
		var files = Directory.GetFiles(dir, "*.json");
		var list = new List<string>();
		foreach (var element in files) {
			var obj = JsonConvert.DeserializeObject<JArray>(File.ReadAllText(element))[0]["info"];
			var id = obj["id"];
			var name = obj["name"];
			list.Add(string.Format("<a href=\"./Shaders/index.html?id={0}\">{1}</a>", id, name));
		}
	
		var text = File.ReadAllText("shadertoys.html".GetEntryPath())
			.Replace("{0}", string.Join(Environment.NewLine, list));
		File.WriteAllText(
			Path.Combine(Path.GetDirectoryName(dir), "index.html"), text
		);
		
	}
	
	public static void RunBlender(int start = 1, string dir = @"C:\Users\Administrator\Desktop\.Folder\099")
	{
		if (start == 0) {
			try {
				start = Directory.GetFiles(dir, "*.blend")
			.Max(x => {
					var s = Path.GetFileNameWithoutExtension(x);
					if (Regex.IsMatch(s, "^\\d+$")) {
						return int.Parse(s);
					} else {
						return 0;
					}
				});
				
				Process.Start(Path.Combine(dir, start.ToString().PadLeft(3, '0') + ".blend"));
				 
			} catch {
			
			}
			return;
		}
		 
		if (start == 1) {
			try {
				start += Directory.GetFiles(dir, "*.blend")
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
		}
		
		var f = Path.Combine(dir, start.ToString().PadLeft(3, '0') + ".blend");
		if (!File.Exists(f)) {
			var str = "1.blend".GetEntryPath();
			File.Copy(str, f);
		}
		Process.Start(f);
	}
	
	public static void QuickAndroid(string first)
	{
		var path = first.TrimStart('_');
		var fileName = Path.GetFileName(path);
		System.IO.Compression.ZipFile.ExtractToDirectory("Kuai.zip".GetEntryPath(),
			Path.GetDirectoryName(path));
		var dir = Directory.GetDirectories(Path.GetDirectoryName(path)).First();
		var src = Path.GetFileName(dir);
		Directory.Move(dir, path);
		var files = Directory.GetFiles(Path.Combine(path, "app\\src"), "*.java", SearchOption.AllDirectories);
		foreach (var element in files) {
			File.WriteAllText(element, File.ReadAllText(element).Replace(
				src.ToLower(), fileName.ToLower()
			));
		}
		var srcParent = Path.GetDirectoryName(files.First());
		Directory.Move(srcParent, Path.Combine(Path.GetDirectoryName(srcParent), fileName.ToLower()));
			               
		files = Directory.GetFiles(path, "*.kts", SearchOption.AllDirectories);
		foreach (var element in files) {
			File.WriteAllText(element, File.ReadAllText(element).Replace(
				src.ToLower(), fileName.ToLower()
			));
		}
	}
	public static void Translate(string s)
	{
		var file = @"D:\.Folder\006\Dou\app\src\main\java\psycho\euphoria\dou\TaskUtils.java";
		var l = "en";
		var req = WebRequest.Create(
			          "http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=" + l + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" +
			          s);
		var res = req.GetResponse();
		using (var reader = new StreamReader(res.GetResponseStream())) {
			var obj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd())["sentences"].ToObject<JArray>();
			var sb = new StringBuilder();
			for (int i = 0; i < obj.Count; i++) {
				sb.Append(obj[i]["trans"]).Append(' ');
			}
			
			s =	string.Format(@"public static boolean checkIf{0}(AccessibilityService accessibilityService, Bitmap bitmap) {{
Log.e(""B5aOx2"", ""{0}"");
if (Utils.checkIfColorIsRange(20,bitmap,new int[]{{420,502,0,0,0,
                415,481,255,237,237,
                429,551,255,237,237,
                455,503,0,0,0,
                471,495,255,235,237}})) {{
            Toast.makeText(accessibilityService, ""{1}"", Toast.LENGTH_SHORT).show();
            click(accessibilityService, getRandomNumber(964,1020), getRandomNumber(358,412));
            return true;
        }}
        return false;
    }}

/*
else if (TaskUtils.checkIf{0}(this, bitmap)) {{
// {1}
                            
                        }}
*/
", sb.ToString().Trim().Camel().Capitalize(), s);
			File.WriteAllText(file, File.ReadAllText
			                  (file).SubstringBeforeLast("}") + s + "}");
		}
	}
	
	public static void GenerateBlenderScript(string n)
	{
		var file = @"C:\Users\Administrator\Desktop\视频\Net\WebApp\Blender\quick_geometry_node.py";
		var s = File.ReadAllText(file);
		
		var nn=n;
		if(!n.StartsWith("ShaderNode") && !n.StartsWith("Node")&& !n.StartsWith("FunctionNode")){
			nn="GeometryNode"+n;
		}
		//(n.StartsWith("ShaderNode")?n:"GeometryNode"+n);
		var nnn=n;
		if(n.StartsWith("ShaderNode")){
			nnn=n.Substring("ShaderNode".Length);
		}else if(n.StartsWith("Node")){
			nnn=n.Substring("Node".Length);
		}else if(n.StartsWith("FunctionNode")){
			nnn=n.Substring("FunctionNode".Length);
		}
		
		s = s.Replace("#1",	string.Format(@"class {0}(Operator):
    """""" GeometryNode{0} """"""
    bl_idname = ""geometrynode.{1}""
    bl_label = """"
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('{0}')
        return {{'FINISHED'}}",nn, n.ToLower()) + "\r\n#1");
		
		s = s.Replace("#2", string.Format(@"        row = self.layout.row(align=True)
        row.operator({0}.bl_idname, text=""{1}"")",nn, nnn) + "\r\n#2");
		s = s.Replace("#3", ",\r\n"+string.Format(@"    {0}", nn) + "#3");
		var lines=Regex.Match(s,"(?<=#4)[^#]+(?=#2)").Value.Split(Environment.NewLine.ToArray(),StringSplitOptions.RemoveEmptyEntries);
		var list=new List<string>();
		foreach (var element in lines) {
			if(element.TrimStart().StartsWith("row.operator"))
				list.Add(element);
		}
		list=list.OrderBy(x=>Regex.Match(x,"(?<=text=\").+(?=\")").Value).ToList();
		var ls=new List<string>();
		var i=0;
		foreach (var element in list) {
			if(i%2==0)
				ls.Add("        row = self.layout.row(align=True)");
			ls.Add(element);
			i++;
		}
		
		s=Regex.Replace(s,"(?<=#4)[^#]+(?=#2)","\r\n"+string.Join(Environment.NewLine,ls)+"\r\n");
		File.WriteAllText(file, s);
		
	}
	
	public static void EscapeForJavaScript(string s)
	{
		var lines = s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
		var list = new List<string>();
		var vars = new List<string>();
	
		foreach (var element in lines) {
			if (Regex.IsMatch(element, "\\$\\d+")) {
				var parts = Regex.Split(element, "\\$\\d+");
				var matches = Regex.Matches(element, "\\$\\d+").Cast<Match>().Select(x => x.Value).ToArray();
				var tempList = new List<string>();
				var i = 0;
				foreach (var p in parts) {					
					tempList.Add("\"" + p.Replace("\"", "\\\"") + "\"");
					if (i < parts.Length - 1) {
						var name = matches[i].Substring(1);
						tempList.Add(string.Format("+x{0}+", name));
						vars.Add(string.Format("var x{0} = \"\";", name));
					}
					i++;
				}
				list.Add(string.Join("", tempList));
			} else {
				list.Add("\"" + element.Replace("\"", "\\\"") + "\"");
			}
		}
		ClipboardShare.SetText(string.Join(Environment.NewLine, vars) + Environment.NewLine + "var str =" +
		string.Join("+" + Environment.NewLine, list) + ";");
	}
	
	
	public static void EscapeCPlusPluse(string s)
	{
		var parts = Regex.Split(s, "\\{\\{[0-9a-zA-Z]+\\}\\}");
		var matches = Regex.Matches(s, "(?<=\\{\\{)[0-9a-zA-Z]+(?=\\}\\})").Cast<Match>().Select(x => x.Value).ToArray();
		var list = new List<string>();
		var i = 0;
		foreach (var element in parts) {
			list.Add(string.Format("R\"({0})\"", element));
			if (i < parts.Length - 1)
				list.Add(matches[i]);
			i++;
		}
		
		ClipboardShare.SetText("ss<<" + string.Join("<<\r\n", list) + ";");
		
		
	}
	public static void SplitFile(string path)
	{
				 
		var strings = File.ReadAllText(path);
		var length = strings.Length;
		var dir = Path.GetDirectoryName(path);
		var part = length / 6;
		for (int i = 0; i < 6; i++) {
			if (i < 5)
				File.WriteAllText(
					Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + (i + 1) + ".txt")
				, strings.Substring(part * i, part));
			else
				File.WriteAllText(
					Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + (i + 1) + ".txt")
				, strings.Substring(part * i));
		}
		
	 
	}
	
	public static void Delete(string dir)
	{
		if (Directory.Exists(dir)) {
			Directory.Delete(dir, true);
		} else if (File.Exists(dir)) {
			File.Delete(dir);
		} else {
			var files = ClipboardShare.GetFileNames();
			foreach (var f in files) {
				if (Directory.Exists(f)) {
					Directory.Delete(f, true);
				} else if (File.Exists(f)) {
					File.Delete(f);
				}
			}
		}
		
	}
	public static void BlenderScript(string text)
	{
		var file = @"C:\Users\Administrator\Desktop\视频\Net\WebApp\Blender\插件.py";
		var contents = File.ReadAllText(file);
		var s1 = string.Format(@"class _{0}(Operator):
    """""" {1} """"""
    bl_idname = ""{2}""
    bl_label = """"
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        
        return {{'FINISHED'}}", text.Replace(" ", "_").ToLower(), text, text.ToLower().Replace(" ", "."));
		
		contents = contents.Replace("#1", s1 + "\r\n#1");
		var s2 = string.Format("row.operator(_{0}.bl_idname, text=\"{1}\")", text.Replace(" ", "_").ToLower(), text);
		contents = contents.Replace("#2", s2 + "\r\n        #2");
		var s3 = string.Format("_{0},", text.Replace(" ", "_").ToLower());
		contents = contents.Replace("#3", s3 + "\r\n    #3");
		File.WriteAllText(file, contents);
	}
	
	public static void ToWebp(string path)
	{
		System.Drawing.Bitmap buffer = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(path);
		
		Imazen.WebP.SimpleEncoder j = new Imazen.WebP.SimpleEncoder();
		var o = new FileStream((Path.GetFileNameWithoutExtension(path) + ".webp").GetDesktopPath(), FileMode.OpenOrCreate);
		j.Encode(buffer, o, 100, true);
		var image = new Bitmap(buffer.Width, buffer.Height, buffer.PixelFormat);
			
		using (var graphics = Graphics.FromImage(image)) {
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.Clear(Color.White);
			graphics.DrawImage(buffer,0, 0,buffer.Width, buffer.Height);
			/*
			 EncoderParameters encoderParams = new EncoderParameters(1);
			encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
			ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
			// Save the image
			finalImage.Save(f, jpgEncoder, encoderParams);
			 */
			image.Save((Path.GetFileNameWithoutExtension(path) + ".jpg").GetDesktopPath(), System.Drawing.Imaging.ImageFormat.Jpeg);
			image.Dispose();	
		}
	
		buffer.Dispose();
		o.Close();
		
	}
	
	
	public static string AndroidSqlite(string s){
		var matches=Regex.Matches(s,"(?<=\\\\\")[a-zA-Z_0-9-]+(?=\\\\\")").Cast<Match>().Select(x=>x.Value);
		var list=new List<string>();
		var i=0;
		foreach (var element in matches) {
			list.Add(string.Format("values.put(\"{0}\", jsonObject.getString(\"{0}\"));",element));
			list.Add(string.Format("jsonObject.put(\"{0}\", cursor.getString({1}));",element,i++));
			
			 
		}
		return string.Join(Environment.NewLine,list.OrderBy(x=>x));
	}
	
	public static string IncreaseNumber(string s,bool isIncrease){
		return Regex.Replace(s,"(?<=[a-zA-Z])\\d+\\b",m=>{
		                     	return (int.Parse(m.Value)+(isIncrease?1:-1)).ToString();
		                     });
	}
}