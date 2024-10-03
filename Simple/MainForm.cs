using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Description of MainForm.
/// </summary>
public partial class MainForm : Form
{
	[DllImport("user32.dll", SetLastError = true)]
	static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, UInt32 uFlags);
	Dictionary<string,String> _snippets = new Dictionary<string,string>();

	
	string _fileName;
	string _fileName1;
	string _destinationFileName;
	SQLiteConnection conn;
	
	public static ushort MakeLangId(CultureInfo cultureInfo)
	{
		return (ushort)MakeLangId(PrimaryLangId(cultureInfo.LCID), SubLangId(cultureInfo.LCID));
	}

	public static int MakeLangId(int primary, int sub)
	{
		return ((ushort)sub << 10) | (ushort)primary;
	}

	private static int PrimaryLangId(int lcid)
	{
		return (ushort)lcid & 0x3ff;
	}

	private static int SubLangId(int lcid)
	{
		return (ushort)lcid >> 10;
	}
	
	
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	protected override void WndProc(ref Message m)
	{
		if (m.Msg == 0x0312) {
			/*
				  ushort id = (ushort)m.WParam;
        Keys key = (Keys)( ( (int)m.LParam >> 16 ) & 0xFFFF );
        Modifiers mods = (Modifiers)( (int)m.LParam & 0xFFFF );*/
			ushort id = (ushort)m.WParam;
			if (id == (ushort)Keys.Q) {
				Blender();
			} else if (id == 87) {
				//System.Threading.Thread.Sleep(100);
				//Utils.Press(0x45);
				//System.Threading.Thread.Sleep(100);
				//Utils.Press(0x59);
						 
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_G);
				//System.Threading.Thread.Sleep(200);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
			} else if (id == 65) {
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_G);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Y);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Y);
			} else if (id == 68) {
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Y);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Y);
		
				//new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SHIFT);
				//new WindowsInput.InputSimulator().Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT,WindowsInput.Native.VirtualKeyCode.VK_X);
				
			} else if (id == 70) {
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
				new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
				
			}
				
				
		}
		base.WndProc(ref m);
	}
	string _str = string.Empty;
	public MainForm()
	{
		
		
		
		//RegisterHotKey(this.Handle, (int)Key.KeyQ, 0, (int)Key.KeyQ);
//		RegisterHotKey(this.Handle, 87, 0, 87);
//		RegisterHotKey(this.Handle, 65, 0, 65);
//		RegisterHotKey(this.Handle, 68, 0, 68);
//		RegisterHotKey(this.Handle, 70, 0, 70);
		/*
		 
		 var sss = typeof(Color).GetProperties().Where(prop =>
                typeof(Color).IsAssignableFrom(prop.PropertyType)).Select(fv => fv.Name)
			.Select(x => {
			var c = Color.FromName(x);
			return string.Format("#define {3} vec3({0:0.000},{1:0.000},{2:0.000})", c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, Regex.Replace(x,"(?<=[a-z])[A-Z]", m => {
				return "_"+m.Value;
			                                                                                                                                }).ToUpper());
		});
		ClipboardShare.SetText(string.Join("\r\n", sss));
		
		 var jx=@"E:\归档";
		jx.CreateDirectoryIfNotExists();
		
		Path.Combine(jx,"视频").CreateDirectoryIfNotExists();
		Path.Combine(jx,"视频","教程").CreateDirectoryIfNotExists();
		Path.Combine(jx,"视频","其他").CreateDirectoryIfNotExists();
		
		Path.Combine(jx,"源代码").CreateDirectoryIfNotExists();
		Path.Combine(jx,"素材").CreateDirectoryIfNotExists();
		Path.Combine(jx,"书籍").CreateDirectoryIfNotExists();
		Path.Combine(jx,"程序").CreateDirectoryIfNotExists();
		 */
		
		
		//File.WriteAllText("1.txt".GetDesktopPath(), JsonConvert.SerializeObject(Directory.GetFiles(@"C:\Users\Administrator\Desktop\WeiXin").Select(x => Path.GetFileName(x))));
	
//		var dir = @"C:\Users\Administrator\Desktop\视频\Net\Simple";
//		dir.CreateDirectoryIfNotExists();
//		var fn = Path.Combine(dir, "Images.cs");
//		if (!File.Exists(fn)) {
//			File.Create(fn).Dispose();
//		}
		
		TopMost = true;
		TopLevel = true;
		InitializeComponent();
		
//		var pf=new PrivateFontCollection();
//		pf.AddFontFile(@"C:\Users\Administrator\Downloads\华文细黑.ttf");
//		Clipboard.SetText(pf.Families.First().GetName(MakeLangId(new CultureInfo("en-US"))));
		
		conn = new SQLiteConnection(new SQLiteConnectionStringBuilder {
			DataSource = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "notes.db"),
			JournalMode = SQLiteJournalModeEnum.Truncate
		}.ConnectionString);
		conn.Open();
		
		using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
			cmd.CommandText = @"create table if not exists Notes(Id integer not null primary key autoincrement, Title text not null unique, Content text not null,Views integer DEFAULT 0,CreateAt datetime default (datetime('now','localtime')),UpdateAt datetime default (datetime('now','localtime')));";
			cmd.ExecuteNonQuery();
		}
//		var f = @"C:\Users\Administrator\Desktop\Documents\Codes\App\Simple\Utils.cs";
//		if (!File.Exists(f)) {
//			File.WriteAllText(f, string.Empty);
//		}
		_fileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "contents.txt");
		if (File.Exists(_fileName)) {
			textBox1.Text = File.ReadAllText(_fileName);
		}
		_fileName1 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "contents1.json");
		if (!File.Exists(_fileName1)) {
			File.Create(_fileName1).Dispose();
		} else {
			LoadData();
		}
		SetWindowPos(this.Handle, new IntPtr(-1), this.Left, this.Top, this.Width, this.Height, 0x0040);
//		var files = ClipboardShare.GetFileNames();
//		_destinationFileName = (files != null && files.Any()) ? files.First(File.Exists) : null;
//		if (_destinationFileName != null) {
//			textBox1.Text = File.ReadAllText(_destinationFileName).Replace("\n", "\r\n");
//		}
		textBox1.KeyDown +=	(sender, args) => {
			Android.HandleKeyDown(textBox1, args);
			//Video.HandleKeyDown(textBox1,args);
		};
		
		using (var eventHookFactory = new EventHook.EventHookFactory()) {
			var x = Keys.M;
			var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
			keyboardWatcher.Start();
			keyboardWatcher.OnKeyInput += (s, e) => {
				 
				/*
				 Invoke(new Action(() => {
				                  	textBox1.Text+=Environment.NewLine+e.KeyData.Keyname;
					}));
				 */
				if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F8") {
//					try {
//						Images.Ocr(this, textBox1, 0, 120, 120);
//					
//					} catch {
//						
//					}
					try {
						Images.Ocr(this, textBox1, 110);
						
					} catch {
						
					}
				} else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F9") {
					try {
						Images.Ocr(this, textBox1, 106);
						
					} catch {
						
					}
				} else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F10") {
					try {
						Images.Ocr(this, textBox1, 120);
						
					} catch {
						
					}

				} 
//				else if (e.KeyData.EventType == EventHook.KeyEvent.up && KeyboardShare.isKeyPressed(18)) {
//
//					if (e.KeyData.Keyname == "D") {
//						var arg = string.Format("--proxy http://127.0.0.1:10809  -f 137 " + ClipboardShare.GetText());
//						Process.Start(new ProcessStartInfo {
//							FileName = "yt-dlp_x86.exe",
//							Arguments = arg,
//							WorkingDirectory = @"C:\Users\Administrator\Desktop\视频"
//						});
//					} else if (e.KeyData.Keyname == "Q") {
//						try {
//							Images.Ocr(this, textBox1, 0, 300, 20, false);
//							
//						} catch {
//							
//						}
//					} else if (e.KeyData.Keyname == "E") {
//						//string q
//						// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=%s&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
//						// en
//						// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
//						
//						var ss = ClipboardShare.GetText().Trim();
//						var req = WebRequest.Create(
//							           "http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" + ss);
//						//req.Proxy = new WebProxy("127.0.0.1", 10809);
//						var res = req.GetResponse();
//						using (var reader = new StreamReader(res.GetResponseStream())) {
//							//var obj =
//							//  (JsonElement)JsonSerializer.Deserialize<Dictionary<String, dynamic>>(reader.ReadToEnd())["sentences"];
//								 
//							var obj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd())["sentences"].ToObject<JArray>();
//							var sb = new StringBuilder();
//							for (int i = 0; i < obj.Count; i++) {
//								sb.Append(obj[i]["trans"]).Append(' ');
//							}
//							// Regex.Replace(sb.ToString().Trim(), "[ ](?=[a-zA-Z0-9])", m => "_").ToLower();
//							// std::string {0}(){{\n}}
//							//return string.Format("{0}", Regex.Replace(sb.ToString().Trim(), " ([a-zA-Z0-9])", m => m.Groups[1].Value.ToUpper()).Decapitalize());
//							//return  sb.ToString().Trim();
//							/*
//			 sb.ToString().Trim();
//							 */
//							ClipboardShare.SetText(sb.ToString().Trim().Camel().Capitalize());
//							
//						}
//						//Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
//					}
//					
//				}
					
					
				if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "OemMinus") {
					
					Utils.FormatNumber(false);
				} else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "OemPlus") {
					Utils.FormatNumber(true);
				} 
//				else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "P") {
//					Invoke(new Action(() => {
//						
//					}));
//				} 
				if (e.KeyData.EventType == EventHook.KeyEvent.up) {
					
					if (e.KeyData.Keyname == "0") {
						_str += "0";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D1") {
						_str += "1";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D2") {
						_str += "2";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D3") {
						_str += "3";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D4") {
						_str += "4";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D5") {
						_str += "5";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D6") {
						_str += "6";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D7") {
						_str += "7";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D8") {
						_str += "8";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "D9") {
						_str += "9";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad1") {
						_str += "1";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad2") {
						_str += "2";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad3") {
						_str += "3";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad4") {
						_str += "4";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad5") {
						_str += "5";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad6") {
						_str += "6";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad7") {
						_str += "7";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad8") {
						_str += "8";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad9") {
						_str += "9";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "NumPad0") {
						_str += "0";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "OemMinus") {
						_str = "-" + _str.TrimStart('-');
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "Subtract") {
						_str = "-" + _str.TrimStart('-');
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "Add") {
						_str = "-." + _str.TrimStart("-.".ToArray());
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "OemPeriod") {
						_str += ".";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "Decimal") {
						_str += ".";
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "Back") {
						if (_str.Length > 0)
							_str = _str.Substring(0, _str.Length - 1);
						Invoke(new Action(() => {
							Text = _str;
						}));
					} else if (e.KeyData.Keyname == "Return") {
						ClipboardShare.SetText(_str);
						_str = string.Empty;
						Invoke(new Action(() => {
							Text = _str;
						}));
					}


				}
//				Invoke(new Action(() => {
//					//textBox1.Text+="\r\n"+e.KeyData.Keyname;
//					//Text = e.KeyData.Keyname;
//				}));
			};
		}
	}
	
	void ListBox1MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (listBox1.SelectedIndex == -1) {
			//LoadData();
			
		} else {
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"select Content from Notes where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				using (var reader = cmd.ExecuteReader()) {
					if (reader.Read())
						Clipboard.SetText(reader.GetString(0));
				}
			}
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"update Notes set Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				using (var reader = cmd.ExecuteReader()) {
					if (reader.Read())
						Clipboard.SetText(reader.GetString(0));
				}
			}
			//LoadData();
		}
		
	}
	public static void TranslateChinese()
	{
		//string q
		// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=%s&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
		// en
		var req = WebRequest.Create(
			          "http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" +
			          ClipboardShare.GetText());
		req.Proxy = new WebProxy("127.0.0.1", 10809);
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
			 */
			Clipboard.SetText(sb.ToString().Trim().Camel().Decapitalize());
		}
		//Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
	}
	
	void MainFormFormClosing(object sender, FormClosingEventArgs e)
	{
		File.WriteAllText(_fileName, textBox1.Text);
	}
	void TextBox1MouseDoubleClick(object sender, MouseEventArgs e)
	{
		textBox1.Text = Regex.Replace(textBox1.Text, "[\r\n]+", "\r\n\r\n");
	}
	
	void LoadData()
	{
//		var s = File.ReadAllText(_fileName1);
//		if (s.Length > 0) {
//			_snippets = JsonConvert.DeserializeObject<Dictionary<string,string>>(s);
//
//			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
//				cmd.CommandText = @"insert into Notes(Title,Content) values(@Title,@Content)";
//				foreach (var element in _snippets.Keys) {
//					cmd.Reset();
//					cmd.Parameters.Add("Title", DbType.String).Value = element;
//					cmd.Parameters.Add("Content", DbType.String).Value = _snippets[element];
//					cmd.ExecuteNonQuery();
//				}
//			}
//		}
		if (comboBox1.Text.Length > 0) {
			listBox1.Items.Clear();
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"select Title,Content from Notes Order By Views DESC";
				//cmd.Parameters.Add("q", DbType.String).Value = comboBox1.Text.Trim();
				var regex = new Regex(comboBox1.Text.Trim());
				using (var reader = cmd.ExecuteReader()) {
					while (reader.Read()) {
						if (regex.IsMatch(reader.GetString(0)) || regex.IsMatch(reader.GetString(1))) {
							listBox1.Items.Add(reader.GetString(0));
						}
					}
				}
			}
			return;
		}
		listBox1.Items.Clear();
		using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
			cmd.CommandText = @"select Title from Notes Order By Views DESC";
			
			using (var reader = cmd.ExecuteReader()) {
				while (reader.Read())
					listBox1.Items.Add(reader.GetString(0));
			}
		}
		//
//			listBox1.Items.AddRange(_snippets.Keys.OrderBy(x => x).ToArray());
	}
	
	void 新建ToolStripMenuItemClick(object sender, EventArgs e)
	{
		var s = Clipboard.GetText().Trim();
		if (s.Length > 0) {
			var pieces = s.Split(new char[]{ '\n' }, 2);
			
			if (pieces.Length > 1) {
				//var key = pieces[0].Trim();
//				if (_snippets.ContainsKey(key))
//					_snippets[key] = pieces[1].Trim();
//				else
//					_snippets.Add(key, pieces[1].Trim());
//				File.WriteAllText(_fileName1, JsonConvert.SerializeObject(_snippets));
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"insert into Notes(Title,Content) values(@Title,@Content)";
					
					cmd.Parameters.Add("Title", DbType.String).Value = pieces[0].Trim();
					cmd.Parameters.Add("Content", DbType.String).Value = pieces[1].Trim();
					cmd.ExecuteNonQuery();
				}
				LoadData();
			}
			
		}
	}
	void 替换ToolStripMenuItemClick(object sender, EventArgs e)
	{
//		var s = Clipboard.GetText().Trim();
//		if (s.Length > 0 && listBox1.SelectedIndex != -1) {
//			_snippets.Remove(listBox1.SelectedItem.ToString());
//
//			var pieces = s.Split(new char[]{ '\n' }, 2);
//
//			if (pieces.Length > 1) {
//				var key = pieces[0].Trim();
//				if (_snippets.ContainsKey(key))
//					_snippets[key] = pieces[1].Trim();
//				else
//					_snippets.Add(key, pieces[1].Trim());
//				File.WriteAllText(_fileName1, JsonConvert.SerializeObject(_snippets));
//				LoadData();
//			}
//
//		}
		var s = Clipboard.GetText().Trim();
		if (s.Length > 0 && listBox1.SelectedIndex != -1) {
			var pieces = s.Split(new char[]{ '\n' }, 2);
			
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"update Notes set Title = @NewTitle,Content = @Content,Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				cmd.Parameters.Add("NewTitle", DbType.String).Value = pieces[0].Trim();
				cmd.Parameters.Add("Content", DbType.String).Value = pieces[1].Trim();
				cmd.ExecuteNonQuery();
			}
			LoadData();
		}
		
	}
	public static void FormatString()
	{
		var s = Clipboard.GetText();
		s = s.Replace("\"", "\"\"")
			.Replace("{", "{{")
			.Replace("}", "}}");
		// ClipboardShare.SetText()
		s = string.Format("string.Format(@\"{0}\")", s);
		//s = string.Format("{0}", s);
		//s = string.Format("string.Format(@\"{0}\")", s);
		Clipboard.SetText(string.Format("{0}", s));
	}
	void 添加ToolStripMenuItemClick(object sender, EventArgs e)
	{
//		var s = Clipboard.GetText().Trim();
//		if (s.Length > 0 && listBox1.SelectedIndex != -1) {
//			var key = listBox1.SelectedItem.ToString();
//			_snippets[key] = _snippets[key] + Environment.NewLine + Environment.NewLine + s;
//			File.WriteAllText(_fileName1, JsonConvert.SerializeObject(_snippets));
//
//		}
		
		var s = Clipboard.GetText().Trim();
		if (s.Length > 0 && listBox1.SelectedIndex != -1) {
			var text = string.Empty;
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"select Content from Notes where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				using (var reader = cmd.ExecuteReader()) {
					if (reader.Read())
						text = reader.GetString(0);
				}
			}
			
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"update Notes set Content = @Content,Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				cmd.Parameters.Add("Content", DbType.String).Value = text + Environment.NewLine + Environment.NewLine + s.Trim();
				cmd.ExecuteNonQuery();
			}
			//LoadData();
		}
		
	}
	void 替换内容ToolStripMenuItemClick(object sender, EventArgs e)
	{
//		var s = Clipboard.GetText().Trim();
//		if (s.Length > 0 && listBox1.SelectedIndex != -1) {
//			var key = listBox1.SelectedItem.ToString();
//			_snippets[key] = s;
//			File.WriteAllText(_fileName1, JsonConvert.SerializeObject(_snippets));
//
//		}
		var s = Clipboard.GetText().Trim();
		if (s.Length > 0 && listBox1.SelectedIndex != -1) {
			//var pieces = s.Split(new char[]{ '\n' }, 2);
			
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"update Notes set Content = @Content,Views = Views + 1,UpdateAt = (datetime('now','localtime')) where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				cmd.Parameters.Add("Content", DbType.String).Value = s.Trim();
				cmd.ExecuteNonQuery();
			}
			//LoadData();
		}
	}
	void 导出ToolStripMenuItemClick(object sender, EventArgs e)
	{
		if (listBox1.SelectedIndices.Count > 0) {
			var sb = new StringBuilder();
			for (int i = 0; i < listBox1.SelectedItems.Count; i++) {
				var key = listBox1.SelectedItems[i];
				sb.AppendLine(key.ToString())
					.AppendLine();
				using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
					cmd.CommandText = @"select Content from Notes where Title = @Title";
					cmd.Parameters.Add("Title", DbType.String).Value = key;
					using (var reader = cmd.ExecuteReader()) {
						if (reader.Read())
							sb.AppendLine(reader.GetString(0)).AppendLine().AppendLine();
					}
				}
				// 
				
			}
			var f = Path.Combine(
				        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				        "contents.txt"
			        );
			File.WriteAllText(f, sb.ToString());
			
		}
	}
	void ComboBox1KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter) {
			listBox1.Items.Clear();
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"select Title,Content from Notes Order By Views DESC";
				//cmd.Parameters.Add("q", DbType.String).Value = comboBox1.Text.Trim();
				var regex = new Regex(comboBox1.Text.Trim());
				using (var reader = cmd.ExecuteReader()) {
					while (reader.Read()) {
						if (regex.IsMatch(reader.GetString(0)) || regex.IsMatch(reader.GetString(1))) {
							listBox1.Items.Add(reader.GetString(0));
						}
					}
				}
			}
		}
	}
	void 删除ToolStripMenuItemClick(object sender, EventArgs e)
	{

		if (listBox1.SelectedIndex != -1) {
			//var pieces = s.Split(new char[]{ '\n' }, 2);
			
			using (var cmd = (SQLiteCommand)conn.CreateCommand()) {
				cmd.CommandText = @"delete from Notes where Title = @Title";
				cmd.Parameters.Add("Title", DbType.String).Value = listBox1.SelectedItem.ToString();
				cmd.ExecuteNonQuery();
			}
			LoadData();
		}
	}
	void MainFormKeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.D1) {
			Screenshot.GetCursorPos(out _p3);
		} else if (e.Control && e.KeyCode == Keys.D2) {
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
	}
	
	Screenshot.POINT _p3 = new Screenshot.POINT();
	Screenshot.POINT _p4 = new Screenshot.POINT();
	
	Point _p1;
	Point _p2;
	
	static void Blender()
	{
		keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
		keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.A, 0), 0, 0); // N1 Press  
		keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.A, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
		keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.S, 0), 0, 0); // N1 Press  
		keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.S, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		/*
			 keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
			keybd_event((int)VK.R, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.R, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
			keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			 */
	}
	#pragma warning disable 649
	internal struct INPUT
	{
		public UInt32 Type;
		public KEYBOARDMOUSEHARDWARE Data;
	}
	[StructLayout(LayoutKind.Explicit)]
	//This is KEYBOARD-MOUSE-HARDWARE union INPUT won't work if you remove MOUSE or HARDWARE
        internal struct KEYBOARDMOUSEHARDWARE
	{
		[FieldOffset(0)]
		public KEYBDINPUT Keyboard;
		[FieldOffset(0)]
		public HARDWAREINPUT Hardware;
		[FieldOffset(0)]
		public MOUSEINPUT Mouse;
	}
	internal struct KEYBDINPUT
	{
		public UInt16 Vk;
		public UInt16 Scan;
		public UInt32 Flags;
		public UInt32 Time;
		public IntPtr ExtraInfo;
	}
	internal struct MOUSEINPUT
	{
		public Int32 X;
		public Int32 Y;
		public UInt32 MouseData;
		public UInt32 Flags;
		public UInt32 Time;
		public IntPtr ExtraInfo;
	}
	internal struct HARDWAREINPUT
	{
		public UInt32 Msg;
		public UInt16 ParamL;
		public UInt16 ParamH;
	}
	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
	static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint extraInfo);
	const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
	const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
	const uint MOUSEEVENTF_LEFTUP = 0x0004;
	const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
	const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
	const uint MOUSEEVENTF_MOVE = 0x0001;
	const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
	const uint MOUSEEVENTF_RIGHTUP = 0x0010;
	const uint MOUSEEVENTF_XDOWN = 0x0080;
	const uint MOUSEEVENTF_XUP = 0x0100;
	const uint MOUSEEVENTF_WHEEL = 0x0800;
	const uint MOUSEEVENTF_HWHEEL = 0x01000;
	public enum MouseEventDataXButtons : uint
	{
		XBUTTON1 = 0x00000001,
		XBUTTON2 = 0x00000002
	}
	[DllImport("user32.dll")]
	static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData,
		int dwExtraInfo);
	[DllImport("user32.dll", SetLastError = true)]
	static extern int MapVirtualKey(uint uCode, uint uMapType);
	[DllImport("user32.dll", SetLastError = true)]
	static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
	enum VK
	{
		ENTER = 0x0D,
		SHIFT = 0x10,
		CTRL = 0x11,
		MENU = 0x12,
		NUMPAD0 = 0x60,
		NUMPAD1 = 0x61,
		NUMPAD2 = 0x62,
		NUMPAD3 = 0x63,
		NUMPAD4 = 0x64,
		NUMPAD5 = 0x65,
		NUMPAD6 = 0x66,
		NUMPAD7 = 0x67,
		NUMPAD8 = 0x68,
		NUMPAD9 = 0x69,
		PageUp = 33,
		PageDown = 34,
		A = 65,
		B = 66,
		C = 67,
		D = 68,
		E = 69,
		F = 70,
		G = 71,
		H = 72,
		I = 73,
		J = 74,
		K = 75,
		L = 76,
		M = 77,
		N = 78,
		O = 79,
		P = 80,
		Q = 81,
		R = 82,
		S = 83,
		T = 84,
		U = 85,
		V = 86,
		W = 87,
		X = 88,
		Y = 89,
		Z = 90,
		OemOpenBrackets = 219,
		Backslash = 220,
		OemCloseBrackets = 221,
	}
	const uint KEYEVENTF_KEYUP = 0x0002;
	public const int INPUT_KEYBOARD = 1;

	
}
