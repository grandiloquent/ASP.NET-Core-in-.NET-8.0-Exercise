
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
					
					var file = @"D:\.Folder\003\Yun\app\src\main\java\psycho\euphoria\yun\WebServerUtils.java";
					if (!File.Exists(file)) {
						File.WriteAllText(file, string.Empty);
					} else {
						var line = ClipboardShare.GetText().Trim();
						var contents = File.ReadAllText(file);
						if (line.Contains("{")) {
			
							contents = contents.Replace("//1", string.Format(@"
public static {0}
//1", line.SubstringAfter(" ")));				
						} else {
							
							contents = contents.Replace("//1", string.Format(@"
public static void {0}(){{
}}
//1", line));
						}
						File.WriteAllText(file, contents);
					}
					
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
				}
			}
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
	}
}
