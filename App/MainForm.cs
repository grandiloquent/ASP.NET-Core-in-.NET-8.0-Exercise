
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
using HtmlAgilityPack;
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
			CreateJavaScript();
		}
		private void KeyD2Pressed()
		{
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
				static void HandleNode(StringBuilder sb, HtmlNode element, string name)
		{
			if (element.NodeType == HtmlNodeType.Text) {
				if (!string.IsNullOrWhiteSpace(element.InnerText))
					sb.AppendFormat("{0}.textContent=`{1}`;\n", name, element.InnerText.Trim());
				return;
			}
			var n = element.GetAttributeValue("id", "");
			if (string.IsNullOrWhiteSpace(n)) {
				n = element.GetAttributeValue("class", "");
			} else {
				n = n.CamelCase();
				
			}
			if (string.IsNullOrWhiteSpace(n)) {
				n = element.Name;
			} else {
				n = n.CamelCase();
			}
			while (_indexs.Contains(n)) {
				n = n.CamelCase() + (_index++);
			}
			_indexs.Add(n);
			if (element.Name == "svg" || element.Name == "path") {
				sb.AppendFormat("const {0}=document.createElementNS('http://www.w3.org/2000/svg','{1}');\n", n, element.Name);
				
			} else
				sb.AppendFormat("const {0}=document.createElement('{1}');\n", n, element.Name);
			foreach (var atr in element.GetAttributes()) {
				if (atr.Name == "style") {
					var lines = atr.Value.Split(';').Where(x => !string.IsNullOrWhiteSpace(x));
					foreach (var v in lines) {
						
						sb.AppendFormat("{0}.style.{1}=\"{2}\";\n", n, v.Split(':')[0].Trim().CamelCase(), v.Split(':')[1].Trim());
					}
				} else
					sb.AppendFormat("{0}.setAttribute(\"{1}\",\"{2}\");\n", n, atr.Name == "viewbox" ? "viewBox" : atr.Name, atr.Value);
			}
			sb.AppendFormat("{0}.appendChild({1});\n", name, n);
			
			foreach (var x in element.ChildNodes) {
				
				HandleNode(sb, x, n);
			}
		}
		public	static void CreateJavaScript()
		{
			_indexs.Clear();
			_index = 1;
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(ClipboardShare.GetText());
			
			var sb = new StringBuilder();
			foreach (var element in hd.DocumentNode.ChildNodes) {
				HandleNode(sb, element, "this.root");
			}
			ClipboardShare.SetText(sb.ToString());
		}
		static int _index = 1;
		static List<string> _indexs = new List<string>();
		
		void ReplaceStrig()
		{
			var line = GetCurrentLine(textBox1);
			var parts = line.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries);
			var s = ClipboardShare.GetText();
			for (int i = 0; i < parts.Length; i += 2) {
				if (i + 1 < parts.Length) {
					s = s.Replace(
						parts[i], parts[i + 1]
					).Replace(
						parts[i].Camel().Decapitalize(), parts[i + 1].Camel().Decapitalize()
					);
				}
			}
			ClipboardShare.SetText(s);
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
	public static class StringUtils
	{
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringAfter(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + delimiter.Length);
		}
		public static string CamelCase(this string value)
		{
			return
                Regex.Replace(
				Regex.Replace(value, "[\\-_ ]+([a-zA-Z])", m => m.Groups[1].Value.ToUpper()),
				"\\s+",
				""
			);
		}
		private static string Concatenate(this IEnumerable<string> strings,
			Func<StringBuilder, string, StringBuilder> builderFunc)
		{
			return strings.Aggregate(new StringBuilder(), builderFunc).ToString();
		}
		public static string ConcatenateLines(this IEnumerable<string> strings)
		{
			return Concatenate(strings, (StringBuilder builder, string nextValue) => builder.AppendLine(nextValue));
		}
		public static string Concatenates(this IEnumerable<string> strings, string separator)
		{
			return Concatenate(strings,
				(StringBuilder builder, string nextValue) => builder.Append(nextValue).Append(separator));
		}
		public static string Concatenates(this IEnumerable<string> strings)
		{
			return Concatenate(strings, (builder, nextValue) => builder.Append(nextValue));
		}
	}
}
