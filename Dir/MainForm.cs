
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

		string _file;

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
			
			_file = "1.txt".GetEntryPath();
			
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
//			using (var eventHookFactory = new EventHook.EventHookFactory()) {
//				 
//				var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();
//				keyboardWatcher.Start();
//				keyboardWatcher.OnKeyInput += (s, e) => {
//					 
//					 
//					if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F9") {
//					 
//						
//						//  124, 160, 60\
//						// 88, 160, 60
//						
//					  
//					} else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "F10") {
//						try {
//							
//							//  124, 160, 60\
//							// 88, 160, 60
//						
//						} catch {
//						
//						}
//					} else if (e.KeyData.EventType == EventHook.KeyEvent.up && e.KeyData.Keyname == "D1") {
//						if (_shortkey) {
//							//ShiftA();
////							Invoke(new Action(() => {
////								ShiftA();
////							}));
//						}
//					}
//				};
//			}
			RegisterHotKey(this.Handle, (int)Keys.F5, 0, (int)Keys.F5);
			RegisterHotKey(this.Handle, (int)Keys.F6, 0, (int)Keys.F6);
			RegisterHotKey(this.Handle, (int)Keys.F7, 0, (int)Keys.F7);
			RegisterHotKey(this.Handle, (int)Keys.F4, 0, (int)Keys.F4);
//			RegisterHotKey(this.Handle, (int)Keys.D4, 0, (int)Keys.D4);
//			RegisterHotKey(this.Handle, (int)Keys.D5, 0, (int)Keys.D5);
//			RegisterHotKey(this.Handle, (int)Keys.D6, 0, (int)Keys.D6);
//			RegisterHotKey(this.Handle, (int)Keys.D7, 0, (int)Keys.D7);
//			RegisterHotKey(this.Handle, (int)Keys.D8, 0, (int)Keys.D8);
//			RegisterHotKey(this.Handle, (int)Keys.D9, 0, (int)Keys.D9);
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0312) {
				/*
				  ushort id = (ushort)m.WParam;
        Keys key = (Keys)( ( (int)m.LParam >> 16 ) & 0xFFFF );
        Modifiers mods = (Modifiers)( (int)m.LParam & 0xFFFF );*/
				ushort id = (ushort)m.WParam;
				if (id == (ushort)Keys.F5) {
					//ShiftA();
					Images.Ocr(this, textBox1, 126, 110, 30);
				} else if (id == (ushort)Keys.F6) {
					//ShiftA();
					Images.Ocr(this, textBox1, 128, 110, 30);
				} else if (id == (ushort)Keys.F7) {
					//ShiftA();
					Images.Ocr(this, textBox1, 124, 110, 30);
				} else if (id == (ushort)Keys.F4) {
					//ShiftA();
					//Images.Ocr(this, textBox1, 80, 220, 60);
					//Images.Ocr(this, textBox1, 96  ,110, 30);
					Images.Ocr(this, textBox1, 124, 110, 30);
				} else if (id == (ushort)Keys.D1) {
					
//					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//					Thread.Sleep(20);
//					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
//					Thread.Sleep(20);
//					keybd_event((int)VK.X, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//					Thread.Sleep(20);
//					keybd_event((int)VK.X, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
//					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				} else if (id == (ushort)Keys.D4) {
					
//					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//					Thread.Sleep(20);
//					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
//					Thread.Sleep(20);
//					keybd_event((int)VK.X, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//					Thread.Sleep(20);
//					keybd_event((int)VK.X, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
//					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				}else if (id == (ushort)Keys.D5) {
					
					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				}else if (id == (ushort)Keys.D6) {
					
					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				}
				else if (id == (ushort)Keys.D7) {
					
					keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.X, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.X, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				}else if (id == (ushort)Keys.D8) {
					
					keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				}else if (id == (ushort)Keys.D9) {
					
					keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
					Thread.Sleep(20);
					keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
					Thread.Sleep(20);
					keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
					Thread.Sleep(20);
					keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release
					Thread.Sleep(20);
				}
			}
			base.WndProc(ref m);
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			
			File.WriteAllText("1.txt".GetEntryPath(), textBox1.Text);
		}

		
		void CopyLine()
		{
			if (textBox1.SelectedText.Length == 0) {
				var line = Extensions.GetCurrentLine(textBox1);
			
				Clipboard.SetText(line);
			}
			
		}

	
		void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode) {
				case Keys.F1:
				//	OpenPath();
					Clipboard.SetText(
						"." + Clipboard.GetText()
					);
					break;
				case Keys.F2:
					var str = Clipboard.GetText();
					Clipboard.SetText(
						str.Substring(0, 1) +	"." + str.Substring(1)
					);
					break;
				case Keys.F3:
					Clipboard.SetText(
						"-" + Clipboard.GetText()
					);
					break;
				case Keys.F4:
					//CopyLine();
					break;
				case Keys.F6:
					//SetDirectory();
					//LoadDirectories();
					break;
			
				case Keys.F5:
					//QuickDelete();
					break;
				case Keys.C:
					if (e.Control) {
						CopyLine();
					}
					break;
					
			}
			
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
		
		
		
		void ToolStripButton5Click(object sender, EventArgs e)
		{
			var f = new Form1();
			f.TopMost = true;
			f.Show();
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
		void GeometryNodeToolStripMenuItemClick(object sender, EventArgs e)
		{
			var f = new Form2();
			f.TopMost = true;
			f.Show();
		}
		void SocketToolStripMenuItemClick(object sender, EventArgs e)
		{
			var s = Clipboard.GetText();
			var v = File.ReadAllText("socket.txt".GetEntryPath());
			Clipboard.SetText(string.Format(v, 
				"\"" + string.Join("\",\"", s.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)) + "\""
			));
		}
		
		void 翻译ToolStripMenuItemClick(object sender, EventArgs e)
		{
			textBox1.Text += Environment.NewLine + Translate();
		}
		public static string Translate(string s = "", int mode = 1)
		{
			//string q
			// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=%s&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
			// en
			// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
			var l = "en";
			s = s == "" ? Clipboard.GetText() : s;
		
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
		public static string ProcessTextWithRegex(string inputText)
    {
        if (string.IsNullOrEmpty(inputText))
        {
            return string.Empty;
        }

        string[] lines = inputText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        if (lines.Length < 2)
        {
            return inputText; // Or handle this case differently
        }

        string firstLine = lines[0];
        string[] firstLineParts = firstLine.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (firstLineParts.Length < 2)
        {
            return inputText; // Or handle this case differently
        }

        string firstPiece = Regex.Escape(firstLineParts[0]); // Escape for regex
        string secondPiece = firstLineParts[1];

        string[] remainingLines = lines.Skip(1).ToArray();
        string joinedRemainingLines = string.Join(Environment.NewLine, remainingLines); // Join with original newlines

        // Use Regex.Replace for whole word match
        string pattern = @"\b"+firstPiece+@"\b";
        string result = Regex.Replace(joinedRemainingLines, pattern, secondPiece);

        return result;
    }
		void ToolStripSplitButton2ButtonClick(object sender, EventArgs e)
		{
			textBox1.Text= ProcessTextWithRegex(textBox1.Text.Trim());
			
			
		}
		
	
	 
	}
	
	
}
