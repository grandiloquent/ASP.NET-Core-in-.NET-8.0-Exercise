using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class Utils
{
	public static void FormatNumber(bool b)
	{
		
		
		
		var ss = ClipboardShare.GetText();
		if (b && Regex.IsMatch(ss, "^[\\d.-]+$")) {
			
			if (Regex.IsMatch(ss, "^\\d")) {
				if (ss.StartsWith("0"))
					ss = "." + ss.Substring(1);
				else
					ss = ss.Substring(0, 1) + "." + ss.Substring(1);
			} else
				ss = "." + Regex.Replace(ss, "^[0.-]", "").Trim('.');
			
			ClipboardShare.SetText((float.Parse(ss)).ToString());
		} else {
		
			if (Regex.IsMatch(ss, "^[\\d.-]+$")) {
				if (Regex.IsMatch(ss, "^\\d")) {
					if (ss.StartsWith("0"))
						ss = "-." + ss.Substring(1).TrimStart(new []{ '.' });
					else
						ss = "-" + ss.Substring(0, 1) + "." + ss.Substring(1).TrimStart(new []{ '.' });
				} else
					ss = "-." + Regex.Replace(ss, "^[0.-]", "").Trim('.');
				//  / 1000f
				ClipboardShare.SetText((float.Parse(ss)).ToString());
			}
			
		}
	}

	public static void FormatStyle(string text)
	{
		var f = @"D:\Documents\Files\CSharp\Uploader\blazor-web\Shared\TopBar.razor";
		var f1 = f + ".css";
		var pieces = text.Trim().Split(new char[]{ '\n' }, 2);
		
		var s = File.ReadAllText(f);
		File.WriteAllText(f, string.Format("{1}\n<div class=\"{0}\">\n</div>", pieces[0].Trim(), s));
		File.AppendAllText(f1, string.Format(".{0}{{\n{1}}}", pieces[0], FormatStyleLines(pieces[1])));
	}

	
	public static string GetCurrentLine(TextBox textBox)
	{
		var s = textBox.Text;
		var i = textBox.SelectionStart;
		var j = textBox.SelectionStart + textBox.SelectionLength;
		while (i > 0 && s[i] != '\n') {
			i--;
		}
		while (j < s.Length - 1 && s[j] != '\n') {
			j++;
		}
		return s.Substring(i, j - i);
	}
	public static double[] MatchDoubles(string s)
	{
		var matches = Regex.Matches(s, "[0-9.-]+").Cast<Match>().Select(x => double.Parse(x.Value));
		return matches.ToArray();
	}
	public static void BlenderResize(TextBox textBox)
	{
		var values = MatchDoubles(GetCurrentLine(textBox));
		if (values.Length >= 3) {
			
			var v = string.Join(",", values);
			Clipboard.SetText(string.Format(@"bpy.ops.transform.resize(value=({0}))", v));
			
		} else if (values.Length == 1) {
			
			Clipboard.SetText(string.Format(@"bpy.ops.transform.resize(value=({0},{0},{0}))", values[0]));
		}
	}
	public static void BlenderTranslate(TextBox textBox)
	{
		var values = MatchDoubles(GetCurrentLine(textBox));
		if (values.Length >= 3) {
			
			var v = string.Join(",", values);
			Clipboard.SetText(string.Format(@"bpy.ops.transform.translate(value=({0}))", v));
			
		} else if (values.Length == 1) {
			
			Clipboard.SetText(string.Format(@"bpy.ops.transform.translate(value=(0,0,{0}))", values[0]));
		}
	}
	public static void BlenderExtrudeRegionMove(TextBox textBox)
	{
		var values = MatchDoubles(GetCurrentLine(textBox));
		if (values.Length >= 3) {
			
			var v = string.Join(",", values);
			Clipboard.SetText(string.Format(@"bpy.ops.mesh.extrude_region_move(TRANSFORM_OT_translate={{""value"":({0})}})", v));
			
		} else if (values.Length == 1) {
			
			Clipboard.SetText(string.Format(@"bpy.ops.mesh.extrude_region_move(TRANSFORM_OT_translate={{""value"":(0,0,{0})}})", values[0]));
		}
	}
	private const uint WS_MINIMIZE = 0x20000000;

	private const int SW_SHOW = 0x05;
	private const uint SW_MINIMIZE = 0x06;
	private const int SW_RESTORE = 0x09;
	[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
	static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
	[DllImport("user32.dll", SetLastError = true)]
	static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
	[DllImport("coredll.dll", SetLastError = true)]
	static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();
	[DllImport("kernel32.dll")]
	static extern IntPtr GetCurrentThread();
	[DllImport("kernel32.dll")]
	static extern uint GetCurrentThreadId();
	[DllImport("user32.dll")]
	static extern bool AttachThreadInput(uint idAttach, uint idAttachTo,
		bool fAttach);
	[DllImport("user32.dll", SetLastError = true)]
	static extern bool BringWindowToTop(IntPtr hWnd);
	[DllImport("user32.dll")]
	static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

	public static void FocusWindow(IntPtr focusOnWindowHandle)
	{
		int style = GetWindowLongPtr(focusOnWindowHandle, -16).ToInt32();

		// Minimize and restore to be able to make it active.
		if ((style & WS_MINIMIZE) == WS_MINIMIZE) {
			ShowWindow(focusOnWindowHandle, SW_RESTORE);
		}

		uint currentlyFocusedWindowProcessId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
		uint appThread = GetCurrentThreadId();

		if (currentlyFocusedWindowProcessId != appThread) {
			AttachThreadInput(currentlyFocusedWindowProcessId, appThread, true);
			BringWindowToTop(focusOnWindowHandle);
			ShowWindow(focusOnWindowHandle, SW_SHOW);
			AttachThreadInput(currentlyFocusedWindowProcessId, appThread, false);
		} else {
			BringWindowToTop(focusOnWindowHandle);
			ShowWindow(focusOnWindowHandle, SW_SHOW);
		}
	}
	public static void ExtractVideoFrames()
	{
		try {
			var vf = Clipboard.GetFileDropList()[0];
			Process.Start(new ProcessStartInfo {
				FileName = "ffmpeg",
				Arguments = string.Format("-i \"{0}\" -q:v 2 img_%07d.png", vf),
				WorkingDirectory = @"C:\Users\Administrator\Desktop\Documents\Video Frames"
			});
		} catch {
			
		}
	}
	public static void OpenChrome(TextBox textBox1)
	{
		var s = textBox1.Text;
		var i = textBox1.SelectionStart;
		var j = textBox1.SelectionStart + textBox1.SelectionLength;
		while (i > 0 && !char.IsWhiteSpace(s[i]) && s[i] != '(') {
			i--;
		}
		while (j < s.Length - 1 && !char.IsWhiteSpace(s[j]) && s[i] != ')') {
			j++;
		}
		var line = s.Substring(i, j - i).Trim().Trim("()".ToArray());
		
		Process.Start(new ProcessStartInfo {
			FileName = "chrome",
			Arguments = line
		});
	}
	public static string Translate(string s = "")
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
			return isChinese ? string.Format("public static void {0}(){{\r\n}}", sb.ToString().Trim().Camel().Decapitalize()) : sb.ToString();
		}
		//Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
	}
	public static void NewCollection()
	{
		var s = string.Format(@"o = bpy.data.collections.new(""{0}"")
bpy.context.scene.collection.children.link(o)
bpy.context.view_layer.active_layer_collection = bpy.context.view_layer.layer_collection.children[o.name]
bpy.ops.mesh.primitive_plane_add()
bpy.context.object.rotation_euler[0] = 1.5708", Translate());
		Clipboard.SetText(s);
	}
	public static void SortCode(string str)
	{
		var s = str.ToBlocks();
		Clipboard.SetText(string.Join("\n\n", s.OrderBy(x => x.SubstringAfter("\"").SubstringBefore("\""))
		                              .Select(x => x.Trim().Trim(';') + ";")).RemoveWhiteSpaceLines()
		);
	}
	public static void FormatGetCode(string s)
	{
		if (s.Contains("?")) {
			var s1 = s.Split('?')[0];
			var s2 = s.Split('?')[1];
			s = string.Format(@"app.MapGet(""{0}"",
    async (HttpResponse response,IDataService dataService, {1}) =>
    {{
    var s = await dataService.QueryString(""select * from (@arg1,@arg2,@arg3)"",
            (cmd) =>
            {{
                cmd.Parameters.AddWithValue(""@arg1"",NpgsqlDbType.Integer, openId);
                cmd.Parameters.AddWithValue(""@arg2"", NpgsqlDbType.Integer, class_type);
            }});
    if (s == string.Empty)
            response.StatusCode = 404;
            
        return s;
    }});", s1, string.Join(",\n", Regex.Matches(s2, "(?<=<)[0-9a-zA-Z_]+(?=>)").Cast<Match>().Select(x => string.Format("[FromQuery(Name = \"{0}\")] int {0}", x.Value))));
		}
		Clipboard.SetText(s);
	}
	
	public static string FormatStyleLines(string s)
	{
		

		
		var lines = s.Split('\n').Where(i => !string.IsNullOrWhiteSpace(i))
			.Select(x => x.Trim());
		var das = lines.Where(i => i.StartsWith("--"));
		var values = lines.Where(i => !i.StartsWith("--"));
		var ls = new List<string>();
		foreach (var item in values) {
			var m = Regex.Match(item, "(?!var\\()--[^)]+\\)*?(?=\\))");
			if (m.Success) {
				ls.Add(Regex.Replace(item, "var\\((--[^)]+\\)*?)\\)", vvv => {
					var v1 = das.FirstOrDefault(i => i.SubstringBefore(':') == vvv.Groups[1].Value.SubstringBefore(','));
					var value = (v1 ?? m.Groups[1].Value.SubstringAfter(',')).SubstringAfter(':')
				                     		.TrimEnd(';') ?? m.Groups[1].Value.SubstringAfter(',');
					while (Regex.IsMatch(value, "(?!var\\()--[^)]+\\)*?(?=\\))")) {
						Console.WriteLine(value);
						value = Regex.Replace(value, "var\\((--[^)]+)\\)+",
							vv => das.First(i => i.SubstringBefore(':') == vv.Groups[1].Value.SubstringBefore(',')).SubstringAfter(':')
				                     		                      .TrimEnd(';'));
					}
				                     	
					return value;
				}));
			} else {
				ls.Add(item);
			}
		}
		
		
		return	string.Join("\n", ls);
		
	}
	public static   void CreateFile(string s)
	{
		
//		if (s.EndsWith(".md")) {
//			var dir = @"D:\Documents\Files\CSharp\web_app\Notes";
//			var f = Path.Combine(dir, s);
//			if (!File.Exists(f)) {
//				File.Create(f).Dispose();
//			}
//		}else{
//			var dir = @"D:\Documents\Files\CSharp\web_app";
//			var f = Path.Combine(dir, s+".cs");
//			if (!File.Exists(f)) {
//				File.Create(f).Dispose();
//			}
//		}
		
//		var dir = @"D:\Documents\Files\CSharp\Uploader\blazor-web\Shared";
//
//		var f = Path.Combine(dir, s + ".razor");
//		if (!File.Exists(f)) {
//			File.WriteAllText(f, "@using Microsoft.AspNetCore.Components");
//		}
//
//		f = Path.Combine(dir, s + ".razor.css");
//		if (!File.Exists(f)) {
//			File.WriteAllText(f, string.Empty);
//		}


		var dir = @"D:\Documents\Files\CSharp\Uploader\WebApplication\static\editor";
		dir.CreateDirectoryIfNotExists();
		var f = Path.Combine(dir, s + ".js");
		if (!File.Exists(f)) {
			File.WriteAllText(f, string.Empty);
		}
	}
	public static void TranslateBlenderPythonClass()
	{
		var ss = Clipboard.GetText().Trim();
		var s = Translate(ss);
		ClipboardShare.SetText(string.Format(@"class {0}(Operator):
    """""" tooltip goes here """"""
    bl_idname = ""{1}""
    bl_label = ""I'm a Skeleton Operator""
    bl_options = {{""REGISTER"", ""UNDO""}}

    @classmethod
    def poll(cls, context):
        return context.mode == ""OBJECT""

    def execute(self, context):
        return {{'FINISHED'}}

#prop = col.operator({0}.bl_idname, text=""{2}"")
", s.Snake(), s.Snake().Replace("_", "."), ss));
	}
	[DllImport("user32.dll", SetLastError = true)]
	public extern static void SendInput(int nInputs, Input[] pInputs, int cbsize);

	[DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
	public extern static int MapVirtualKey(int wCode, int wMapType);

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int x;
		public int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MouseInput
	{
		public int X;
		public int Y;
		public int Data;
		public int Flags;
		public int Time;
		public IntPtr ExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct KeyboardInput
	{
		public short VirtualKey;
		public short ScanCode;
		public int Flags;
		public int Time;
		public IntPtr ExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct HardwareInput
	{
		public int uMsg;
		public short wParamL;
		public short wParamH;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Input
	{
		public int Type;
		public InputUnion ui;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct InputUnion
	{
		[FieldOffset(0)]
		public MouseInput Mouse;
		[FieldOffset(0)]
		public KeyboardInput Keyboard;
		[FieldOffset(0)]
		public HardwareInput Hardware;
	}

	public const int INPUT_MOUSE = 0;

	public const int INPUT_KEYBOARD = 1;

	public const int INPUT_HARDWARE = 2;

	public const int KEYEVENTF_KEYDOWN = 0x0;

	public const int KEYEVENTF_KEYUP = 0x2;

	public const int KEYEVENTF_EXTENDEDKEY = 0x1;

	public const int VK_SNAPSHOT = 0x2C;

	public const int VK_LWIN = 0x5B;

	public static void Press(short key)
	{
		Utils.Input[] inputs = new Utils.Input[4];
		inputs[0] = new Input();
		inputs[0].Type = 1;
		inputs[0].ui.Keyboard.VirtualKey = key;
		inputs[0].ui.Keyboard.ScanCode = (short)MapVirtualKey(key, 0);
		inputs[0].ui.Keyboard.Flags = KEYEVENTF_KEYDOWN;
		inputs[0].ui.Keyboard.Time = 0;
		inputs[0].ui.Keyboard.ExtraInfo = IntPtr.Zero;

		inputs[1] = new Input();
		inputs[1].Type = 1;
		inputs[1].ui.Keyboard.VirtualKey = key;
		inputs[1].ui.Keyboard.ScanCode = (short)MapVirtualKey(key, 0);
		inputs[1].ui.Keyboard.Flags = KEYEVENTF_KEYUP;
		inputs[1].ui.Keyboard.Time = 0;
		inputs[1].ui.Keyboard.ExtraInfo = IntPtr.Zero;
            
		Utils.SendInput(inputs.Length, inputs, Marshal.SizeOf(inputs[0]));
	}
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
	
	public static void CreateDirectories(string dir, string[] names)
	{
		//new []{ "Materials", "Scenes", "Shaders","Textures","Scripts","Models"}
		dir.CreateDirectoryIfNotExists();
		names.ToList().ForEach(x => {
			(Path.Combine(dir, x)).CreateFileIfNotExists();//.CreateDirectoryIfNotExists();
		});
//		var dir=".Folder".GetDesktopPath();
//		dir.CreateDirectoryIfNotExists();
//		for (int i = 1; i < 100; i++) {
//			Directory.CreateDirectory(Path.Combine(dir,i.ToString().PadLeft(3,'0')));
//		}
		
	}
	
	public static string GetLine(TextBox textBox)
	{
		var start = textBox.SelectionStart;
		var end = start;
		while (start - 1 > -1) {
			var founded = false;
			if (textBox.Text[start] == '\n') {
				var p = start - 1;
				while (p - 1 > -1) {
					if (textBox.Text[p] == '\n') {
						if (string.IsNullOrWhiteSpace(textBox.Text.Substring(p, start - p))) {
							founded = true;
							start++;
							break;
						}
					}
					p--;
				}
			}
			if (founded)
				break;
			start--;
		}
		while (end + 1 < textBox.Text.Length) {
			var founded = false;
			if (textBox.Text[end] == '\n') {
				var p = end + 1;
				while (p + 1 < textBox.Text.Length) {
					if (textBox.Text[p] == '\n') {
						if (string.IsNullOrWhiteSpace(textBox.Text.Substring(end, p - end))) {
							founded = true;
							end++;
							break;
						}
					}
					p++;
				}
			}
			if (founded)
				break;
			end++;
		}
		return textBox.Text.Substring(start, end - start + 1).Trim();
	}
}

