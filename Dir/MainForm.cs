
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dir
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		 
		
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	
		[DllImport("user32.dll")]
		static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
			uint wMsgFilterMax);
		[DllImport("user32.dll")]
		static extern bool TranslateMessage([In] ref MSG lpMsg);
		[DllImport("user32.dll")]
		static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
       
		}
		[StructLayout(LayoutKind.Sequential)]

		public struct MSG
		{
			IntPtr hwnd;
			public	uint message;
			public	UIntPtr wParam;
			IntPtr lParam;
			int time;
			POINT pt;
			int lPrivate;
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
		#pragma warning restore 649
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
		
		[DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out POINT point);

		string _dir;
		string _file;
		string _fav;
		string _snippet;
		bool _shortkey=true;

		void ShiftA()
		{
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			Thread.Sleep(20);
			keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
			Thread.Sleep(20);
			keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
			Thread.Sleep(20);
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
			Thread.Sleep(20);
			keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			Thread.Sleep(20);
			keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release

		}

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			textBox1.AllowDrop = true; // Crucial step!
			var config = "config".GetEntryPath();
			if (File.Exists(config)) {
				_dir = File.ReadAllText(config);
			}
			_file = "1.txt".GetEntryPath();
			_fav = "fav.txt".GetEntryPath();
			_snippet = "snippet".GetEntryPath();
			if (File.Exists(_file)) {
				textBox1.Text = File.ReadAllText(_file);
			}
			
			/*var text = Clipboard.GetText().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			if (text == null || text.Length == 0) {
				return;
			}
			var dir = text[0];
			if (Directory.Exists(dir)) {
				for (int i = 1; i < text.Length; i++) {
					var n = Path.Combine(dir, text[i]);
					if (text[i].Contains(".") && !File.Exists(n)) {
						File.WriteAllText(n, string.Empty);
					} else if (!Directory.Exists(n)) {
						Directory.CreateDirectory(n);
					}
				}
			}*/
//			var dir=@"C:\Users\Administrator\Desktop\新建文件夹\13\新建文件夹";
//			var files=Directory.GetFiles(dir);
//			foreach (var element in files) {
//				var contents=File.ReadAllText(element,System.Text.Encoding.GetEncoding("gb2312"));
//				File.WriteAllText(Path.Combine(Path.GetDirectoryName(dir),Path.GetFileName(element)),contents,System.Text.Encoding.UTF8);
//			}
//			Directory.GetDirectories(@"C:\Users\Administrator\Desktop\文件夹")
//				.ToList()
//				.ForEach(x => {
//				var files = Directory.GetFiles(x);
//				if (files.Length == 0) {
//					Directory.Delete(x);
//				}
//			});
			using (var eventHookFactory = new EventHook.EventHookFactory()) {
				 
				var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
				keyboardWatcher.Start();
				keyboardWatcher.OnKeyInput += (s, e) => {
					 
					 
					if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F9") {
					 
							Images.Ocr(this, textBox1, 110, 120, 30);
							//  124, 160, 60\
							// 88, 160, 60
						
					  
					} else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F10") {
						try {
							Images.Ocr(this, textBox1, 120, 110, 30);
							//  124, 160, 60\
							// 88, 160, 60
						
						} catch {
						
						}
					} else  if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "D1") {
						if (_shortkey) {
							//ShiftA();
//							Invoke(new Action(() => {
//								ShiftA();
//							}));
						}
					}
				};
			}
		}
		void ToolStripButton1ButtonClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText();
			if (Directory.Exists(dir)) {
				_dir = dir;
				File.WriteAllText("config".GetEntryPath(), _dir);
			}
		}

		void LoadDirectories()
		{
			var list = Directory.GetDirectories(_dir);
			var buf = new List<KeyValuePair<string,long>>();
			foreach (var element in list) {
				long length = 0;
				try {
					length = Extensions.GetDirectorySize(element);
				} catch (Exception) {
					
				}
				buf.Add(new KeyValuePair<string, long>(Path.GetFileName(element),
					length));
			}
			var s = buf.OrderByDescending(x => x.Value).Select(x => x.Key + " | " + Extensions.GetBytesReadable(x.Value));
			textBox1.Text = string.Join(Environment.NewLine, s);
		}

		void ToolStripButton2Click(object sender, EventArgs e)
		{
			LoadDirectories();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			
			File.WriteAllText("1.txt".GetEntryPath(), textBox1.Text);
		}

		void OpenPath()
		{
			var line = Extensions.GetCurrentLine(textBox1);
			if (line.Contains("|")) {
				line = line.SubstringBefore('|').Trim();
			}
			var p = line.Contains("\\") ? line : Path.Combine(_dir, line);
			if (Directory.Exists(p) || File.Exists(p)) {
				Process.Start(p);
			} else if (p.StartsWith("http://") || p.StartsWith("https://")) {
				Process.Start("chrome", "\"" + p + "\"");
			}
		}

		void CreateDirectory()
		{
			var line = Extensions.GetCurrentLine(textBox1);
			if (line.Contains("|")) {
				line = line.SubstringBefore('|').Trim();
			}
			var p = Path.Combine(_dir, line);
			if (!Directory.Exists(p)) {
				Directory.CreateDirectory(p);
			}
		}

		void CreateFile()
		{
			var line = Extensions.GetCurrentLine(textBox1);
			if (line.Contains("|")) {
				line = line.SubstringBefore('|').Trim();
			}
			var p = Path.Combine(_dir, line);
			if (!File.Exists(p)) {
				File.WriteAllText(p, string.Empty);
			}
		}

		void CopyLine()
		{
			if (textBox1.SelectedText.Length == 0) {
				var line = Extensions.GetCurrentLine(textBox1);
			
				Clipboard.SetText(line);
			}
			
		}

		void QuickDelete()
		{
			if (isQuickDelete) {
				var line = Extensions.GetCurrentLine(textBox1);
				if (line.Contains("|")) {
					line = line.SubstringBefore('|').Trim();
				}
				var p = Path.Combine(_dir, line);
				if (Directory.Exists(p)) {
					try {
						Directory.Delete(p, true);
					} catch {
					}
				}
			}
		}

		void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) {
				case Keys.F1:
					OpenPath();
					break;
				case Keys.F2:
					CreateDirectory();
					break;
				case Keys.F3:
					CreateFile();
					break;
				case Keys.F4:
					CopyLine();
					break;
				case Keys.F6:
					SetDirectory();
					LoadDirectories();
					break;
			
				case Keys.F5:
					QuickDelete();
					break;
				case Keys.C:
					if (e.Control) {
						CopyLine();
					}
					break;
					
			}
			
		}
		void 删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var line = Extensions.GetCurrentLine(textBox1);
			if (line.Contains("|")) {
				line = line.SubstringBefore('|').Trim();
			}
			var p = Path.Combine(_dir, line);
			if (Directory.Exists(p)) {
				try {
					Directory.Delete(p, true);
				} catch {
				}
			}
		}

		void SetDirectory()
		{
			var line = Extensions.GetCurrentLine(textBox1);
			if (line.Contains("|")) {
				line = line.SubstringBefore('|').Trim();
			}
			var p = Path.Combine(_dir, line);
			if (Directory.Exists(p)) {
				_dir = p;
				File.WriteAllText("config".GetEntryPath(), _dir);
			}
		}

		void ToolStripButton3Click(object sender, EventArgs e)
		{
			SetDirectory();
			LoadDirectories();
		}
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			var p = Path.GetDirectoryName(_dir);
			if (Directory.Exists(p)) {
				_dir = p;
				File.WriteAllText("config".GetEntryPath(), _dir);
			}
		}
		void 收藏夹ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (File.Exists(_fav)) {
				textBox1.Text = File.ReadAllText(_fav);
			}
			
		}
		void 保存收藏夹ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			File.WriteAllText(_fav, textBox1.Text);
			
		}
		void 运行ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Process.Start(new ProcessStartInfo {
				FileName = "cmd",
				Arguments = "/C dotnet run",
				WorkingDirectory = @"C:\Users\Administrator\Desktop\视频\Net\WebApp",
				WindowStyle = ProcessWindowStyle.Hidden
			});
		}
		void 代码段ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
			if (File.Exists(_snippet)) {
				textBox1.Text = File.ReadAllText(_snippet);
			}
		}
		void 保存代码段ToolStripMenuItemClick(object sender, EventArgs e)
		{
			File.WriteAllText(_snippet, textBox1.Text);
		}
		
		void 文件列表ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dir = Clipboard.GetText();
			if (Directory.Exists(dir)) {
				_dir = dir;
				File.WriteAllText("config".GetEntryPath(), _dir);
			}
			
			var files = Directory.GetFiles(_dir).Select(x => Path.GetFileName(x));
			textBox1.Text = string.Join(Environment.NewLine, files);
			
		}
		void ToolStripButton5Click(object sender, EventArgs e)
		{
			var f = new Form1();
			f.TopMost = true;
			f.Show();
		}
		void 文件名排序ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var list = Directory.GetDirectories(_dir);
			var buf = new List<KeyValuePair<string,long>>();
			foreach (var element in list) {
				long length = 0;
				try {
					length = Extensions.GetDirectorySize(element);
				} catch (Exception) {
					
				}
				buf.Add(new KeyValuePair<string, long>(Path.GetFileName(element),
					length));
			}
			var s = buf.OrderBy(x => x.Key).Select(x => x.Key + " | " + Extensions.GetBytesReadable(x.Value));
			textBox1.Text = string.Join(Environment.NewLine, s);
		}
		bool isQuickDelete;
		void 快速删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			isQuickDelete = true;
		}
		void 删除剪切板ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var files = Clipboard.GetFileDropList();
			if (files != null && files.Count > 0) {
				foreach (var element in files) {
					try {
						if (Directory.Exists(element)) {
							Directory.Delete(element, true);
						} else {
							File.Delete(element);
						}
					} catch (Exception) {
						
						 
					}
				}
			}
		}
		void ToolStripButton6Click(object sender, EventArgs e)
		{
			if (File.Exists(_fav)) {
				textBox1.Text = File.ReadAllText(_fav);
			}
		}
		void ToolStripButton7Click(object sender, EventArgs e)
		{
			if (File.Exists(_snippet)) {
				textBox1.Text = File.ReadAllText(_snippet);
			}
		}
		void TextBox1DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy; // Show a "copy" cursor
			}
		}
		void TextBox1DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files != null && files.Length > 0) {
				// For simplicity, let's just display the first file path in the TextBox

				// You can process multiple files here if needed
				foreach (string file in files) {
					var n = Path.Combine(
						        @"D:\Knife", Path.GetFileName(file)
					        );
				 	
					if (File.Exists(file) && !File.Exists(n)) {
						File.Move(file, n);
					} else if (Directory.Exists(file) && !Directory.Exists(n)) {
						Directory.Move(file, n);
					}
				}
			}
		}
		void ToolStripSplitButton1ButtonClick(object sender, EventArgs e)
		{
			_shortkey = !_shortkey;
			
		}
		void ToolStripMenuItem3Click(object sender, EventArgs e)
		{
			var dir = @"C:\blender\resources\Resource";

			var n = Clipboard.GetText().Trim();
			var f = Path.Combine(dir, n + ".blend");
			if (!File.Exists(f)) {
				File.Copy(Path.Combine(dir, "模板4.blend"), f);
			}
			Process.Start(new ProcessStartInfo {
				FileName = @"C:\Users\Administrator\Desktop\blender-4.4.0-windows-x64\blender.exe",
				Arguments = "\"" + f + "\"",
				WorkingDirectory = dir
			});
		}
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			var dir = @"C:\blender\resources\Resource";

			var n = Clipboard.GetText().Trim();
			var f = Path.Combine(dir, n + ".blend");
			if (!File.Exists(f)) {
				File.Copy(Path.Combine(dir, "模板3.blend"), f);
			}
			Process.Start(new ProcessStartInfo {
				FileName = @"C:\Users\Administrator\Desktop\blender-3.6.5-windows-x64\blender.exe",
				Arguments = "\"" + f + "\"",
				WorkingDirectory = dir
			});
		}
	 
	}
	
	
}
