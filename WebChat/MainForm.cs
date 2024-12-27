
using System;
using System.Collections.Generic;
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
		void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Control) {
				if (e.KeyCode == Keys.Q) {
					var name= GetCurrentLine(textBox1).Trim();
				
					var contents=ClipboardShare.GetText();
					
					CreateFile(name,contents);
					
					
				} else if (e.KeyCode == Keys.G) {
					var line = GetCurrentLine(textBox1);
					textBox1.Text += Environment.NewLine +	Translate(line);
					
				} else if (e.KeyCode == Keys.C) {
					if (textBox1.SelectionLength == 0) {
						ClipboardShare.SetText(GetCurrentLine(textBox1));
					} else {
						textBox1.Copy();
					}
				} else if (e.KeyCode == Keys.F) {
					var s = textBox1.Text.Trim();
					var first = s.SubstringBefore('\n').Trim();
					var array = first.Split('|');
					var ss = ClipboardShare.GetText();
					var list=new List<string>();
				
					for (int i = 1; i < array.Length; i++) {
					var	str = ss.Replace(
							array[0], array[i]
			
						).Replace(
							array[0].Capitalize(), array[i].Capitalize()
			
						);
						str = Regex.Replace(str, "(?<=== )\\d+", m => {
						                   	return (int.Parse(m.Value) + i).ToString();
						});
						str = Regex.Replace(str, "(?<=, )\\d+(?=\\))", m => {
						                   	return (int.Parse(m.Value) + i).ToString();
						});
						list.Add(str);
					}
					ClipboardShare.SetText(string.Join(Environment.NewLine,list));
				}else if(e.KeyCode==Keys.D){
					CreatePage(ClipboardShare.GetText().Trim());
				} else if (e.KeyCode == Keys.P) {
					var name = GetCurrentLine(textBox1).Trim();
					if (name.StartsWith("1"))
						VsCode(name.Substring(1));
					else
						VsCode();
				}
			}
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
	
			public static void CreatePage(string name)
		{
			var baseDir = @"C:\blender";
			var entry = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var dir = Path.Combine(baseDir, "pages", name);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			var extensions = new string[]{ "js", "json", "wxml", "wxss" };
			foreach (var element in extensions) {
				var file = Path.Combine(dir, name + "." + element);
				if (!File.Exists(file)) {
					File.Copy(Path.Combine(entry, "x." + element), file);
				}
			}
			var app = Path.Combine(baseDir, "app.json");
			var contents = File.ReadAllText(app);
			var index = contents.IndexOf(']');
			contents = contents.Insert(index - 1, ",\r\n\"pages/" + name + "/" + name + "\"\r\n");
			File.WriteAllText(app, contents);
			
		}
			public static void CreateFile(string name,string contents)
		{
			var baseDir = @"C:\blender";
			var dir = Path.Combine(baseDir, "utils");
			var file = Path.Combine(dir, name + ".js");
			if (File.Exists(file))
				return;
			var s = string.Format(@"
module.exports = () => {{
{2}
	}}
	
// const {1}=require(""../../utils/{0}"")", name, name.Camel(),contents);
			File.WriteAllText(file, s);
			
		}
			/*return new Promise((reslove, reject) => {{
		wx.request({{
			url: '',
			success: res => {{
			 
			if(res.statusCode>=200 && res.statusCode<400)
				reslove(res.data)
				else
				reject(new Error(res.statusCode));
			}},
			fail: err => {{
				reject(err)
			}}
		}})
	)
			 }}
			 */

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
		void TextBox1KeyDown(object sender, KeyEventArgs e)
		{
	
		}
	}
}
