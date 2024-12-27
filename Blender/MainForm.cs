
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Android
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			var f = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "1.txt");
			if (File.Exists(f))
				textBox1.Text = File.ReadAllText(f);
		 
		}
		 
		 
		private void KeyAPressed()
		{
		}
		private void KeyBPressed()
		{
		}
		private void KeyCPressed()
		{
		}
		private void KeyDPressed()
		{
			DownloadYouTubeVideo(ClipboardShare.GetText());
		}
		private void KeyEPressed()
		{
		}
		private void KeyFPressed()
		{
		}
		private void KeyGPressed()
		{
		}
		private void KeyHPressed()
		{
		}
		private void KeyIPressed()
		{
		}
		private void KeyJPressed()
		{
		}
		private void KeyKPressed()
		{
		}
		private void KeyLPressed()
		{
		}
		private void KeyMPressed()
		{
		}
		private void KeyNPressed()
		{
		}
		private void KeyOPressed()
		{
			GenerateBlenderShaderScript(ClipboardShare.GetText().Trim());
		}
		
		private void KeyPPressed()
		{
			var name = GetCurrentLine(textBox1).Trim();
			if (name.StartsWith("1"))
				VsCode(name.Substring(1));
			else
				VsCode();
		}
		private void KeyQPressed()
		{
			var line = GetCurrentLine(textBox1);
			
			if (line.StartsWith("dot")) {
				Process.Start(new ProcessStartInfo {
					FileName = "cmd",
					Arguments = "/C dotnet run",
					WorkingDirectory = @"C:\Users\Administrator\Desktop\视频\Net\WebApp",
					WindowStyle = ProcessWindowStyle.Hidden
				});
			} else if (line.StartsWith("b")) {
				RunBlender(
					Regex.IsMatch(line, "\\d+") ? int.Parse(Regex.Match(line, "\\d+").Value) : 1
				);//EscapeCPlusPluse(textBox.Text.TrimStart('b'));
			}
		}
		
		private void KeyRPressed()
		{
		}
		private void KeySPressed()
		{
		}
		private void KeyTPressed()
		{
		}
		private void KeyUPressed()
		{
		}
		private void KeyVPressed()
		{
		}
		private void KeyWPressed()
		{
		}
		private void KeyXPressed()
		{
		}
		private void KeyYPressed()
		{
		}
		private void KeyZPressed()
		{
		}
		private void KeyD0Pressed()
		{
		}
		private void KeyD1Pressed()
		{
			Screenshot.GetCursorPos(out _p3);
		}
		private void KeyD2Pressed()
		{
			Screenshot.GetCursorPos(out _p4);
			using (Bitmap bitmap = new Bitmap(_p4.X - _p3.X, _p4.Y - _p3.Y)) {
				// Draw the screenshot into our bitmap.
				using (Graphics g = Graphics.FromImage(bitmap)) {
					g.CopyFromScreen(_p3.X, _p3.Y, 0, 0, bitmap.Size);
				}
				
				var i = 1;
				var f = (i + ".png").GetDesktopPath();
				while (File.Exists(f)) {
					i++;
					f = (i + ".png").GetDesktopPath();
				}
				var ms = new FileStream(f, FileMode.OpenOrCreate);
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				bitmap.Dispose();
				
			}
		}
		private void KeyD3Pressed()
		{
		}
		private void KeyD4Pressed()
		{
		}
		private void KeyD5Pressed()
		{
		}
		private void KeyD6Pressed()
		{
		}
		private void KeyD7Pressed()
		{
		}
		private void KeyD8Pressed()
		{
		}
		private void KeyD9Pressed()
		{
		}
		private void KeyF1Pressed()
		{
		}
		private void KeyF2Pressed()
		{
		}
		private void KeyF3Pressed()
		{
		}
		private void KeyF4Pressed()
		{
		}
		private void KeyF5Pressed()
		{
		}
		private void KeyF6Pressed()
		{
		}
		private void KeyF7Pressed()
		{
		}
		private void KeyF8Pressed()
		{
		}
		private void KeyF9Pressed()
		{
			try {
				Images.Ocr(this, textBox1, 124, 160, 60);
						
			} catch {
						
			}
		}
		private void KeyF10Pressed()
		{
		}
		private void KeyF11Pressed()
		{
		}
		private void KeyF12Pressed()
		{
		}
		Screenshot.POINT _p3 = new Screenshot.POINT();
		Screenshot.POINT _p4 = new Screenshot.POINT();
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
	
		public static string GetCurrentLine(TextBox textBox)
		{
			var s = textBox.Text;
			var i = textBox.SelectionStart;
			var j = textBox.SelectionStart + textBox.SelectionLength;
		
			while (i > 0 && s[i - 1] != '\n') {
				i--;
			}
			while (j < s.Length && s[j] != '\n') {
				j++;
			}
			return s.Substring(i, j - i).Trim();
		}
		public static void GenerateBlenderScript(string n)
		{
			var file = @"C:\Users\Administrator\Desktop\视频\Net\WebApp\Blender\quick_geometry_node.py";
			var s = File.ReadAllText(file);
		
			var nn = n;
			if (!n.StartsWith("ShaderNode") && !n.StartsWith("Node") && !n.StartsWith("FunctionNode")) {
				nn = "GeometryNode" + n;
			}
			//(n.StartsWith("ShaderNode")?n:"GeometryNode"+n);
			var nnn = n;
			if (n.StartsWith("ShaderNode")) {
				nnn = n.Substring("ShaderNode".Length);
			} else if (n.StartsWith("Node")) {
				nnn = n.Substring("Node".Length);
			} else if (n.StartsWith("FunctionNode")) {
				nnn = n.Substring("FunctionNode".Length);
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
        return {{'FINISHED'}}", nn, n.ToLower()) + "\r\n#1");
		
			s = s.Replace("#2", string.Format(@"        row = self.layout.row(align=True)
        row.operator({0}.bl_idname, text=""{1}"")", nn, nnn) + "\r\n#2");
			s = s.Replace("#3", ",\r\n" + string.Format(@"    {0}", nn) + "#3");
			var lines = Regex.Match(s, "(?<=#4)[^#]+(?=#2)").Value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			var list = new List<string>();
			foreach (var element in lines) {
				if (element.TrimStart().StartsWith("row.operator"))
					list.Add(element);
			}
			list = list.OrderBy(x => Regex.Match(x, "(?<=text=\").+(?=\")").Value).ToList();
			var ls = new List<string>();
			var i = 0;
			foreach (var element in list) {
				if (i % 2 == 0)
					ls.Add("        row = self.layout.row(align=True)");
				ls.Add(element);
				i++;
			}
		
			s = Regex.Replace(s, "(?<=#4)[^#]+(?=#2)", "\r\n" + string.Join(Environment.NewLine, ls) + "\r\n");
			File.WriteAllText(file, s);
		
		}
		public static void GenerateBlenderShaderScript(string n)
		{
			var file = @"C:\Users\Administrator\Desktop\视频\Net\WebApp\Blender\quick_shader_node.py";
			var s = File.ReadAllText(file);
		
			var nn = n;
			if (!n.StartsWith("ShaderNode") && !n.StartsWith("Node") && !n.StartsWith("FunctionNode")) {
				nn = "" + n;
			}
			//(n.StartsWith("ShaderNode")?n:"GeometryNode"+n);
			var nnn = n;
			if (n.StartsWith("ShaderNode")) {
				nnn = n.Substring("ShaderNode".Length);
			} else if (n.StartsWith("Node")) {
				nnn = n.Substring("Node".Length);
			} else if (n.StartsWith("FunctionNode")) {
				nnn = n.Substring("FunctionNode".Length);
			}
		
			s = s.Replace("#1",	string.Format(@"class {0}(Operator):
    """""" ShaderNode{0} """"""
    bl_idname = ""shadernode.{1}""
    bl_label = """"
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return True

    def execute(self, context):
        shader('{0}')
        return {{'FINISHED'}}", nn, n.ToLower()) + "\r\n#1");
		
			s = s.Replace("#2", string.Format(@"        row = self.layout.row(align=True)
        row.operator({0}.bl_idname, text=""{1}"")", nn, nnn) + "\r\n#2");
			s = s.Replace("#3", ",\r\n" + string.Format(@"    {0}", nn) + "#3");
			var lines = Regex.Match(s, "(?<=#4)[^#]+(?=#2)").Value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			var list = new List<string>();
			foreach (var element in lines) {
				if (element.TrimStart().StartsWith("row.operator"))
					list.Add(element);
			}
			list = list.OrderBy(x => Regex.Match(x, "(?<=text=\").+(?=\")").Value).ToList();
			var ls = new List<string>();
			var i = 0;
			foreach (var element in list) {
				if (i % 2 == 0)
					ls.Add("        row = self.layout.row(align=True)");
				ls.Add(element);
				i++;
			}
		
			s = Regex.Replace(s, "(?<=#4)[^#]+(?=#2)", "\r\n" + string.Join(Environment.NewLine, ls) + "\r\n");
			File.WriteAllText(file, s);
		
		}
	
		public static void VsCode(string fileName = "javascript")
		{
			var file = @"C:\Users\Administrator\AppData\Roaming\Code\User\snippets\" + fileName + ".json";
			
			var res = File.Exists(file) ? JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(file))
				: JsonConvert.DeserializeObject<Dictionary<string, dynamic>>("{}");
			var str = ClipboardShare.GetText().Trim();
			var obj = new Dictionary<string, dynamic>();
			var body = new Dictionary<string, List<string>>();
			var name = str.SubstringBefore("\n").Trim();
			obj.Add("prefix", name);
			obj.Add("body", str.SubstringAfter("\n").Trim().Split(new char[] { '\n' }).Select(i => i.TrimEnd()).ToList());
 
			if (res.ContainsKey(name)) {
				res[name] = obj;
			} else
				res.Add(name, obj);
			var text = JsonConvert.SerializeObject(res, Formatting.Indented);
			
			File.WriteAllText(file, text);
		}
		public static void RefactorJavaScript(string name, string contents)
		{
			var file = @"C:\blender\resources\web\index.html";
			var s = File.ReadAllText(file);
			var fileName = Path.GetFileNameWithoutExtension(file);
			if (!s.Contains(fileName + "-utils.js")) {
				s = s.Replace("</html>", string.Format(@"
<script src=""{0}""></script>
</script>
</html>", fileName + "-utils.js"));
				File.WriteAllText(file, s);
			}
			file = Path.Combine(Path.GetDirectoryName(file), fileName + "-utils.js");
			if (!File.Exists(file)) {
				File.WriteAllText(file, string.Empty);
			}
			s = File.ReadAllText(file);
			
			File.WriteAllText(file, string.Format(@"{0}
async function {1}(){{
{2}
}}
", s, name, contents));
			ClipboardShare.SetText(name + "();");
		}
		public static void GenerateJavaScript(string name)
		{
			var s = string.Format(@"

const {1} = document.querySelector('.{0}');
{1}.addEventListener('click', evt => {{
    evt.stopPropagation();
    
}})

document.querySelectorAll('.{0}')
    .forEach(element => {{
        element.addEventListener('click', evt => {{
            evt.stopPropagation();
        }})
    }});
", name, name.Camel());
			ClipboardShare.SetText(s);
			//var file =_file;
			 
			//var str = File.ReadAllText(file);
			//str = str + Environment.NewLine + s;
			//File.WriteAllText(file, str);
		}
		
		public static string Translate(string s = "", int mode = 1)
		{
			//string q
			// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=%s&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
			// en
			// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
			var l = "en";
			s = s == "" ? ClipboardShare.GetText() : s;
		
			var isChinese = Regex.IsMatch(s, "[\u4e00-\u9fa5]");
			if (!isChinese) {
				l = "zh";
				s = Regex.Replace(Regex.Replace(s, "[\r\n]+", " "), "- ", "");
			}
			var req = WebRequest.Create(
				          "http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=" + l + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" +
				          s);
			//req.Proxy = new WebProxy("127.0.0.1", 10809);
			var res = req.GetResponse();
			using (var reader = new StreamReader(res.GetResponseStream())) {
				//var obj =
				//  (JsonElement)JsonSerializer.Deserialize<Dictionary<String, dynamic>>(reader.ReadToEnd())["sentences"];
				var obj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd())["sentences"].ToObject<JArray>();
				var sb = new StringBuilder();
				for (int i = 0; i < obj.Count; i++) {
					sb.Append(obj[i]["trans"]).Append(' ');
				}
				// Regex.Replace(sb.ToString().Trim(), "[ ](?=[a-zA-Z0-9])", m => "_").ToLower();
				// std::string {0}(){{\n}}
				//return string.Format("{0}", Regex.Replace(sb.ToString().Trim(), " ([a-zA-Z0-9])", m => m.Groups[1].Value.ToUpper()).Decapitalize());
				//return  sb.ToString().Trim();
				/*
			 sb.ToString().Trim();
			 .Trim().Camel().Capitalize()
			 */
				return isChinese ? (mode == 0 ? sb.ToString().Trim().Camel().Capitalize() : sb.ToString().Trim().Camel().Decapitalize()) : sb.ToString();
			}
			//Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
	
			var f = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "1.txt");
			 
			File.WriteAllText(f, textBox1.Text);
		}
		
		private static void DuplicateFiles()
		{
			var dir = @"C:\blender\resources\web";
			var find = "book";
			var replace = "article";
			var baseFileName = "books";
			var extensions = new string[]{ "css", "js", "html" };
			
			foreach (var element in extensions) {
				var f = Path.Combine(dir,
					        baseFileName.Replace(find, replace) + "." + element
				        );
				if (File.Exists(f)) {
					continue;
				}
				if (element == "css") {
					File.Copy(Path.Combine(dir, baseFileName + "." + element), f);
				 
				} else {
					var contents = File.ReadAllText(Path.Combine(dir, baseFileName + "." + element));
					contents = contents.Replace(find, replace)
						.Replace(find.Camel().Capitalize(), replace.Camel().Capitalize());
					File.WriteAllText(f, contents);
				}
			}
		}
		void TextBox1KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control) {
				if (e.KeyCode == Keys.A) {
					KeyAPressed();
				} else if (e.KeyCode == Keys.B) {
					KeyBPressed();
				} else if (e.KeyCode == Keys.C) {
					KeyCPressed();
				} else if (e.KeyCode == Keys.D) {
					KeyDPressed();
				} else if (e.KeyCode == Keys.E) {
					KeyEPressed();
				} else if (e.KeyCode == Keys.F) {
					KeyFPressed();
				} else if (e.KeyCode == Keys.G) {
					KeyGPressed();
				} else if (e.KeyCode == Keys.H) {
					KeyHPressed();
				} else if (e.KeyCode == Keys.I) {
					KeyIPressed();
				} else if (e.KeyCode == Keys.J) {
					KeyJPressed();
				} else if (e.KeyCode == Keys.K) {
					KeyKPressed();
				} else if (e.KeyCode == Keys.L) {
					KeyLPressed();
				} else if (e.KeyCode == Keys.M) {
					KeyMPressed();
				} else if (e.KeyCode == Keys.N) {
					KeyNPressed();
				} else if (e.KeyCode == Keys.O) {
					KeyOPressed();
				} else if (e.KeyCode == Keys.P) {
					KeyPPressed();
				} else if (e.KeyCode == Keys.Q) {
					KeyQPressed();
				} else if (e.KeyCode == Keys.R) {
					KeyRPressed();
				} else if (e.KeyCode == Keys.S) {
					KeySPressed();
				} else if (e.KeyCode == Keys.T) {
					KeyTPressed();
				} else if (e.KeyCode == Keys.U) {
					KeyUPressed();
				} else if (e.KeyCode == Keys.V) {
					KeyVPressed();
				} else if (e.KeyCode == Keys.W) {
					KeyWPressed();
				} else if (e.KeyCode == Keys.X) {
					KeyXPressed();
				} else if (e.KeyCode == Keys.Y) {
					KeyYPressed();
				} else if (e.KeyCode == Keys.Z) {
					KeyZPressed();
				} else if (e.KeyCode == Keys.D0) {
					KeyD0Pressed();
				} else if (e.KeyCode == Keys.D1) {
					KeyD1Pressed();
				} else if (e.KeyCode == Keys.D2) {
					KeyD2Pressed();
				} else if (e.KeyCode == Keys.D3) {
					KeyD3Pressed();
				} else if (e.KeyCode == Keys.D4) {
					KeyD4Pressed();
				} else if (e.KeyCode == Keys.D5) {
					KeyD5Pressed();
				} else if (e.KeyCode == Keys.D6) {
					KeyD6Pressed();
				} else if (e.KeyCode == Keys.D7) {
					KeyD7Pressed();
				} else if (e.KeyCode == Keys.D8) {
					KeyD8Pressed();
				} else if (e.KeyCode == Keys.D9) {
					KeyD9Pressed();
				}
			} else if (e.KeyCode == Keys.F1) {
				KeyF1Pressed();
			} else if (e.KeyCode == Keys.F2) {
				KeyF2Pressed();
			} else if (e.KeyCode == Keys.F3) {
				KeyF3Pressed();
			} else if (e.KeyCode == Keys.F4) {
				KeyF4Pressed();
			} else if (e.KeyCode == Keys.F5) {
				KeyF5Pressed();
			} else if (e.KeyCode == Keys.F6) {
				KeyF6Pressed();
			} else if (e.KeyCode == Keys.F7) {
				KeyF7Pressed();
			} else if (e.KeyCode == Keys.F8) {
				KeyF8Pressed();
			} else if (e.KeyCode == Keys.F9) {
				KeyF9Pressed();
			} else if (e.KeyCode == Keys.F10) {
				KeyF10Pressed();
			} else if (e.KeyCode == Keys.F11) {
				KeyF11Pressed();
			} else if (e.KeyCode == Keys.F12) {
				KeyF12Pressed();
			}
		}
	}
	
}
