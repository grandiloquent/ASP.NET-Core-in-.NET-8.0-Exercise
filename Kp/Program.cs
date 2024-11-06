
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kb
{
	
	

	class Program
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

		static void BlenderMoveY()
		{
			keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press
			keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Y, 0), 0, 0); // N1 Press
			keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Y, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			
		}
		static void BlenderMoveZ()
		{
			keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press
			keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Y, 0), 0, 0); // N1 Press
			keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Y, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			
		}
		static void BlenderLoopCut()
		{
			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.CTRL, 0), 0, 0); // N1 Press
			keybd_event((int)VK.R, (byte)MapVirtualKey((uint)VK.R, 0), 0, 0); // N1 Press
			keybd_event((int)VK.R, (byte)MapVirtualKey((uint)VK.R, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.CTRL, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		}
		public static void Main(string[] args)
		{
			//Directory.Delete(@"C:\Users\Administrator\AppData\Local\JetBrains\Fleet",true);
			//GeneratorTri();
			//RegisterHotKey(IntPtr.Zero, 81, 0, 81);//Q
			
			
			//RegisterHotKey(IntPtr.Zero, 68, 0, 68);//D
			//RegisterHotKey(IntPtr.Zero, 70, 0, 70);//F
			RegisterHotKey(IntPtr.Zero, (int)Keys.W, 0, (int)Keys.W);
			RegisterHotKey(IntPtr.Zero, (int)Keys.Q, 0, (int)Keys.Q);
			RegisterHotKey(IntPtr.Zero, (int)Keys.E, 0, (int)Keys.E);
			
			RegisterHotKey(IntPtr.Zero, (int)Keys.F1, 0, (int)Keys.F1);
			RegisterHotKey(IntPtr.Zero, (int)Keys.F2, 0, (int)Keys.F2);
			RegisterHotKey(IntPtr.Zero, (int)Keys.F3, 0, (int)Keys.F3);
			
			//RegisterHotKey(IntPtr.Zero, (int)Keys.W, 0, (int)Keys.W);
			//RegisterHotKey(IntPtr.Zero, (int)Keys.E, 0, (int)Keys.E);
			
//			RegisterHotKey(IntPtr.Zero, 34, 0, 34);//P
//			RegisterHotKey(IntPtr.Zero, 33, 0, 33);// PageUp
			// document.addEventListener('keydown',evt=>console.log(evt));
			
//			RegisterHotKey(IntPtr.Zero, 65, 0, 65);//A
//			RegisterHotKey(IntPtr.Zero, 0x53, 0, 0x53);//S
			//	RegisterHotKey(IntPtr.Zero, 49, 0, 49);
			MSG msg;
			int ret;
			while ((ret = GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0) {
				if (ret == 1 && msg.message == 0x0312) {
					ushort id = (ushort)msg.wParam;
					
					if (id == (int)Keys.Q) { // Q
						keybd_event((int)VK.L, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.L, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
							
						keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			
					} else if (id == (int)Keys.W) {
						keybd_event((int)VK.L, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.L, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release
									
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
					} else if (id == (int)Keys.E) {
						keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
					} else if (id == (int)Keys.F1) {
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
					} else if (id == (int)Keys.F2) {
						keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
					} else if (id == (int)Keys.F3) {
						keybd_event((int)VK.R, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.R, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					} else if (id == 34) {//P
						TakeScreenShot();
					} else if (id == 65) {//a
						//BlenderMoveXY();
						
						PhotoshopBrushDecrease();
					} else if (id == 0x53) {
						PhotoshopBrushIncrement();
					} else if (id == 68) {
						PhotoshopCurve();
						//GenerateClickCode();
					} else if (id == 33) {
						HsvToHex();
					}
				} else {
					
					TranslateMessage(ref msg);
					DispatchMessage(ref msg);
				}
			}
		}
		static int GetInt(int min, int max)
		{
			 
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
				byte[] randomNumber = new byte[4];//4 for int32
				rng.GetBytes(randomNumber);
				int value = BitConverter.ToInt32(randomNumber, 0);
				return	new Random(value).Next(min, max);
			 
				 
			}
		}
		static void BlenderGeometryNodes()
		{
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.A, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
			keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.S, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		}
		static void BlenderDuplicate()
		{
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
		}
		static void BlenderDuplicate1()
		{
			
		}
		
		static void PhotoshoBrush()
		{
			keybd_event((int)VK.I, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.I, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
			Thread.Sleep(100);
			mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
			keybd_event((int)VK.B, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.B, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		}
		static void PhotoshopCurve()
		{
//			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
//			keybd_event((int)VK.M, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//			keybd_event((int)VK.M, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
//			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
			//SetCursorPos();
		

			SetCursorPos(1252, 715);
			mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
			Thread.Sleep(100);
			mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

			SetCursorPos(1292, 461);
			mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
			Thread.Sleep(100);
			mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
		}
		static void GenerateClickCode()
		{
			
			POINT p;
			GetCursorPos(out p);
			ClipboardShare.SetText(string.Format(@"
SetCursorPos({0},{1});
mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
			Thread.Sleep(100);
			mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);", p.X, p.Y));
		}
		static void PhotoshopBrushDecrease()
		{
			//keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			//keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event(219, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event(219, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		}
		
		static void PhotoshopBrushIncrement()
		{
			//keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			//keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((byte)VK.OemCloseBrackets, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((byte)VK.OemCloseBrackets, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
		}
		static void GeneratorTri()
		{
			var oy = 96;
			var ox = 0;
			var min = 30;
			var max = 60;
			ox = GetInt(min, max);
			var x = new int[]{ 0, 0 };
			var y = new int[]{ 0, oy };
			var z = new int []{ ox, 0 };
			var toggle = true;
			var length = 800;
			var list = new List<string>();
			
			list.Add("<svg viewBox=\"0 0 200 200\" xmlns=\"http://www.w3.org/2000/svg\">");
			for (int i = 0; i < 20; i++) {
				list.Add(string.Format("<path d=\"M{0},{1}L{2} {3}L{4} {5}\" fill=\"{6}\" stoke=\"none\"></path>", x[0], x[1], y[0], y[1], z[0], z[1], toggle ? "#46B9CC" : "#43B3BF"));
				if (toggle) {
					var xz = z[0];
					y[0] = x[0];
					x[0] = z[0];
					x[1] = z[1];
					y[1] = 96;
					ox = GetInt(min, max);
					z[0] += ox;
					z[1] = 96;
				} else {
					var xz = z[0];
					y[0] = x[0];
					x[0] = z[0];
					x[1] = z[1];
					y[1] = 0;
					ox = GetInt(min, max);
					z[0] += ox;
					z[1] = 0;
				}
				toggle = !toggle;
			}
			list.Add("</svg>");
			File.WriteAllLines(@"C:\Users\Administrator\Desktop\12.svg", list);
		}
		static void BlenderMoveXY()
		{
		
						
			keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
				
		}
	  
		static void BlenderDuplicateZ()
		{
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.D, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.D, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release    	
			keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
							
		}
		
		static void TakeScreenShot()
		{
			var bitmap =	Screenshot.GetScreenshot();
			var i = 0;
			var f = Screenshot.GetDesktopPath(i.ToString().PadLeft(3, '0') + ".png");
			while (File.Exists(f)) {
				i++;
				f = Screenshot.GetDesktopPath(i.ToString().PadLeft(3, '0') + ".png");
			}
			bitmap.Save(f, System.Drawing.Imaging.ImageFormat.Png);
					
			
		}
		static	int sampleSize = 5;
		private static Bitmap GetSampleRegion(Screen screen, int mouseX, int mouseY)
		{
			var bmp = new Bitmap(sampleSize, sampleSize, PixelFormat.Format32bppArgb);
			Graphics gfxScreenshot = Graphics.FromImage(bmp);
			gfxScreenshot.CopyFromScreen(mouseX - sampleSize / 2, mouseY - sampleSize / 2, 0, 0, new Size(sampleSize, sampleSize));
			gfxScreenshot.Save();
			gfxScreenshot.Dispose();
			return bmp;
		}
		static void ColorPicker()
		{
			POINT p = new POINT();
			GetCursorPos(out p);
			int mouseX = p.X;
			int mouseY = p.Y;
			var screen = Screen.FromRectangle(new Rectangle(p.X, p.Y, 1, 1));
			var	sampleBitmap = GetSampleRegion(screen, mouseX, mouseY);
			Color sampleColor = sampleBitmap.GetPixel(sampleSize / 2, sampleSize / 2);
			sampleBitmap.Dispose();
//			string tmpR = (sampleColor.R / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
//			string tmpG = (sampleColor.G / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
//			string tmpB = (sampleColor.B / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
//			var s = string.Format("{0}, {1}, {2}", tmpR, tmpG, tmpB);
			//var s = ColorTranslator.ToHtml(sampleColor);
			//ClipboardShare.SetText(s.Substring(1));
			int red = sampleColor.R;
			int green = sampleColor.G;
			int blue = sampleColor.B;
			double r_output = Math.Pow(red / 255.0, 2.2);
			double g_output = Math.Pow(green / 255.0, 2.2);
			double b_output = Math.Pow(blue / 255.0, 2.2);
			ClipboardShare.SetText(string.Format("{0}\r\n{1}\r\n{2}", r_output, g_output, b_output));
		}
		public static Color ConvertHexToColor(string hex)
		{
			hex = hex.Remove(0, 1);
			byte a = hex.Length == 8 ? Byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) : (byte)255; 
			byte r = Byte.Parse(hex.Substring(hex.Length - 6, 2), NumberStyles.HexNumber);
			byte g = Byte.Parse(hex.Substring(hex.Length - 4, 2), NumberStyles.HexNumber);
			byte b = Byte.Parse(hex.Substring(hex.Length - 2), NumberStyles.HexNumber);
			return Color.FromArgb(a, r, g, b);
		}
		public static Tuple<double, double, double> RgbToHsv(double r, double g, double b)
		{
			double[] hsv = new double[3]; 
			r = r / 255.0;
			g = g / 255.0;
			b = b / 255.0;
			double max = new[] { r, g, b }.Max();
			double min = new[] { r, g, b }.Min(); 
			double delta = max - min;
			hsv[1] = max != 0 ? delta / max : 0;
			hsv[2] = max;
			if (hsv[1] == 0) {
				return new Tuple<double, double, double>(hsv[0], hsv[1], hsv[2]);
			}
			if (r == max) {
				hsv[0] = ((g - b) / delta);
			} else if (g == max) {
				hsv[0] = ((b - r) / delta) + 2.0;
			} else if (b == max) {
				hsv[0] = ((r - g) / delta) + 4.0;
			}
			hsv[0] *= 60.0;
			if (hsv[0] < 0) {
				hsv[0] += 360.0;
			}
			return new Tuple<double, double, double>(hsv[0], hsv[1], hsv[2]);
		}
		public static Color HSVtoRGB(float hue, float saturation, float value)
		{
			int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
			float f = hue / 60 - (float)Math.Floor(hue / 60);
			value = value * 255;
			int v = Convert.ToInt32(value);
			int p = Convert.ToInt32(value * (1 - saturation));
			int q = Convert.ToInt32(value * (1 - f * saturation));
			int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
			if (hi == 0) {
				return Color.FromArgb(255, v, t, p);
			} else if (hi == 1) {
				return Color.FromArgb(255, q, v, p);
			} else if (hi == 2) {
				return Color.FromArgb(255, p, v, t);
			} else if (hi == 3) {
				return Color.FromArgb(255, p, q, v);
			} else if (hi == 4) {
				return Color.FromArgb(255, t, p, v);
			} else {
				return Color.FromArgb(255, v, p, q);
			}
		}
		public static void HsvToHex()
		{
			var matches = Regex.Matches(ClipboardShare.GetText(), "[0-9.-]+")
				.Cast<Match>().Select(x => float.Parse(x.Value)).ToArray();
			var color = HSVtoRGB(matches[0], matches[1], matches[2]);
			int red = color.R;
			int green = color.G;
			int blue = color.B;
			double r_output = (red > 0.04045) ? 
                  System.Math.Pow((red + 0.055) / (1.0 + 0.055), 2.4) : 
                  (red / 12.92);
			double g_output = (green > 0.04045) ? 
                  System.Math.Pow((green + 0.055) / (1.0 + 0.055), 2.4) : 
                  (green / 12.92);
			double b_output = (blue > 0.04045) ? 
                  System.Math.Pow((blue + 0.055) / (1.0 + 0.055), 2.4) : 
                  (blue / 12.92); 
			ClipboardShare.SetText(string.Format("{0}{1}{2}", r_output, g_output, b_output));
			//ClipboardShare.SetText(string.Format("{0}{1}{2}",color.R.ToString("X2"),color.G.ToString("X2"),color.B.ToString("X2")));
						
		}
		
		
		public static void ShaderToy1()
		{
			var n = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "003.html");
			
			var str = File.ReadAllText(n);
			var s = ClipboardShare.GetText();
			var dir = @"C:\Users\Administrator\Desktop\视频\Net\WebApp\ShaderToy";
			var d = Directory.GetFiles(dir, "*.html")
				.Select(x => {
				var m = Regex.Match(Path.GetFileName(x), "[0-9]+");
				return m.Success ? int.Parse(m.Value) : 0;
			}).Max() + 1;
			var dd = Path.Combine(dir, d.ToString().PadLeft(3, '0') + ".html");
			File.WriteAllText(dd, Regex.Replace(str, "\\{\\{[0-9]+}}", s));
		}
		public static void Translate(string s = "", bool zh = false)
		{
			//string q
			// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=%s&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
			// en
			// http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
			var l = zh ? "zh" : "en";
			s = s == "" ? ClipboardShare.GetText() : s;
		
//			var isChinese = Regex.IsMatch(s, "[\u4e00-\u9fa5]");
//			if (!isChinese) {
//				l = "zh";
//			}
			if (zh) {
				s = Regex.Replace(s, "[\r\n]+", " ");
				s = s.Replace("- ", "");
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
			Clipboard.SetText( sb.ToString().Trim();
			 .Trim().Camel().Capitalize())
			 */
				//return isChinese ? sb.ToString() : sb.ToString();
				if (zh)
					ClipboardShare.SetText(sb.ToString());
				else
					ClipboardShare.SetText(sb.ToString().Trim().Camel().Capitalize());
	
			}
			//Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
		}
	}
	public static    class ClipboardShare
	{
		const uint cfUnicodeText = 13;
		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern int DragQueryFile(IntPtr hDrop, int iFile, StringBuilder lpszFile, int cch);
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CloseClipboard();
		[DllImport("gdi32.dll")]
		static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, string lpszFile);
		[DllImport("gdi32.dll")]
		static extern bool DeleteEnhMetaFile(IntPtr hemf);
		[DllImport("user32.dll")]
		static extern bool EmptyClipboard();
		[DllImport("User32.dll", SetLastError = true)]
		static extern IntPtr GetClipboardData(uint uFormat);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GlobalLock(IntPtr hMem);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int GlobalSize(IntPtr hMem);
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GlobalUnlock(IntPtr hMem);
		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool IsClipboardFormatAvailable(uint format);
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);
		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);
		public static IEnumerable<string> GetFileNames()
		{
			if (!IsClipboardFormatAvailable(15)) {
				var n = GetText();
				if (Directory.Exists(n) || File.Exists(n)) {
					return new string[] { n };
				}
				return null;
			}
			IntPtr handle = IntPtr.Zero;
			try {
				OpenClipboard();
				handle = GetClipboardData(15);
				if (handle == IntPtr.Zero) {
					return null;
				}
				var count = DragQueryFile(handle, unchecked((int)0xFFFFFFFF), null, 0);
				if (count == 0) {
					return Enumerable.Empty<string>();
				}
				var sb = new StringBuilder(260);
				var files = new string[count];
				for (var i = 0; i < count; i++) {
					var charlen = DragQueryFile(handle, i, sb, sb.Capacity);
					var s = sb.ToString();
					if (s.Length > charlen) {
						s = s.Substring(0, charlen);
					}
					files[i] = s;
				}
				return files;
			} finally {
				CloseClipboard();
			}
		}
	
		public static string GetText()
		{
			if (!IsClipboardFormatAvailable(cfUnicodeText)) {
				return null;
			}
			IntPtr handle = IntPtr.Zero;
			IntPtr pointer = IntPtr.Zero;
			try {
				OpenClipboard();
				handle = GetClipboardData(cfUnicodeText);
				if (handle == IntPtr.Zero) {
					return null;
				}
				pointer = GlobalLock(handle);
				if (pointer == IntPtr.Zero) {
					return null;
				}
				var size = GlobalSize(handle);
				var buff = new byte[size];
				Marshal.Copy(pointer, buff, 0, size);
				return Encoding.Unicode.GetString(buff).TrimEnd('\0');
			} finally {
				if (pointer != IntPtr.Zero) {
					GlobalUnlock(handle);
				}
				CloseClipboard();
			}
		}
		public static void OpenClipboard()
		{
			var num = 10;
			while (true) {
				if (OpenClipboard(IntPtr.Zero)) {
					break;
				}
				if (--num == 0) {
					ThrowWin32();
				}
				System.Threading.Thread.Sleep(100);
			}
		}
		public static void SetText(string text)
		{
			OpenClipboard();
			EmptyClipboard();
			IntPtr hGlobal = IntPtr.Zero;
			try {
				var bytes = (text.Length + 1) * 2;
				hGlobal = Marshal.AllocHGlobal(bytes);
				if (hGlobal == IntPtr.Zero) {
					ThrowWin32();
				}
				var target = GlobalLock(hGlobal);
				if (target == IntPtr.Zero) {
					ThrowWin32();
				}
				try {
					Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
				} finally {
					GlobalUnlock(target);
				}
				// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclipboarddata
				if (SetClipboardData(cfUnicodeText, hGlobal) == IntPtr.Zero) {
					ThrowWin32();
				}
				hGlobal = IntPtr.Zero;
			} finally {
				if (hGlobal != IntPtr.Zero) {
					Marshal.FreeHGlobal(hGlobal);
				}
				CloseClipboard();
			}
		}
		// https://github.com/nanoant/ChromeSVG2Clipboard/blob/e135818eb25be5f5f1076a3746b675e9228657d1/ChromeClipboardHost/Program.cs
		static void ThrowWin32()
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		public static string Camel(this string value)
		{
			return
            Regex.Replace(
				Regex.Replace(value, "[\\-_ ]+([a-zA-Z])", m => m.Groups[1].Value.ToUpper()),
				"\\s+",
				""
			);
		}
		public static String Capitalize(this String s)
		{
			if (string.IsNullOrEmpty(s))
				return s;
			if (s.Length == 1)
				return s.ToUpper();
			if (char.IsUpper(s[0]))
				return s;
			return char.ToUpper(s[0]) + s.Substring(1);
		}

	}
	public static  class Screenshot
	{
		public	struct POINT
		{
			public int X;
			public int Y;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
			public int X {
				get {
					return Left;
				}
				set {
					Right -= Left - value;
					Left = value;
				}
			}
			public int Y {
				get {
					return Top;
				}
				set {
					Bottom -= Top - value;
					Top = value;
				}
			}
			public int Width {
				get {
					return Right - Left;
				}
				set {
					Right = value + Left;
				}
			}
			public int Height {
				get {
					return Bottom - Top;
				}
				set {
					Bottom = value + Top;
				}
			}
			public Point Location {
				get {
					return new Point(Left, Top);
				}
				set {
					X = value.X;
					Y = value.Y;
				}
			}
			public Size Size {
				get {
					return new Size(Width, Height);
				}
				set {
					Width = value.Width;
					Height = value.Height;
				}
			}
			public RECT(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}
			public RECT(Rectangle r)
				: this(r.Left, r.Top, r.Right, r.Bottom)
			{
			}
			public static implicit operator Rectangle(RECT r)
			{
				return new Rectangle(r.Left, r.Top, r.Width, r.Height);
			}
			public static implicit operator RECT(Rectangle r)
			{
				return new RECT(r);
			}
			public static bool operator ==(RECT r1, RECT r2)
			{
				return r1.Equals(r2);
			}
			public static bool operator !=(RECT r1, RECT r2)
			{
				return !r1.Equals(r2);
			}
			public bool Equals(RECT r)
			{
				return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
			}
			public override bool Equals(object obj)
			{
//            if (obj is RECT rect)
//            {
//                return Equals(rect);
//            }
//
//            if (obj is Rectangle rectangle)
//            {
//                return Equals(new RECT(rectangle));
//            }
				return false;
			}
			public override int GetHashCode()
			{
				return ((Rectangle)this).GetHashCode();
			}
			public override string ToString()
			{
				return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
			}
		}
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);
		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nReghtRect, int nBottomRect);
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nReghtRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDC);
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public	static extern bool GetCursorPos(out POINT lpPoint);
		[DllImport("user32.dll")]
		static extern IntPtr WindowFromPoint(POINT p);
		private static Bitmap CaptureRectangleNative(Rectangle rect, bool captureCursor = false)
		{
			IntPtr handle = GetDesktopWindow();
			return CaptureRectangleNative(handle, rect, captureCursor);
		}
		private static Bitmap CaptureRectangleNative(IntPtr handle, Rectangle rect, bool captureCursor = false)
		{
			if (rect.Width == 0 || rect.Height == 0) {
				return null;
			}
			IntPtr hdcSrc = GetWindowDC(handle);
			IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, rect.Width, rect.Height);
			IntPtr hOld = SelectObject(hdcDest, hBitmap);
			BitBlt(hdcDest, 0, 0, rect.Width, rect.Height, hdcSrc, rect.X, rect.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);
			if (captureCursor) {
//				try {
//					CursorData cursorData = new CursorData();
//					cursorData.DrawCursor(hdcDest, rect.Location);
//				} catch (Exception e) {
//					DebugHelper.WriteException(e, "Cursor capture failed.");
//				}
			}
			SelectObject(hdcDest, hOld);
			DeleteDC(hdcDest);
			ReleaseDC(handle, hdcSrc);
			Bitmap bmp = Image.FromHbitmap(hBitmap);
			DeleteObject(hBitmap);
			return bmp;
		}
		public static Bitmap CaptureRectangle(Rectangle rect)
		{
//            if (RemoveOutsideScreenArea)
//            {
//                Rectangle bounds = CaptureHelpers.GetScreenBounds();
//                rect = Rectangle.Intersect(bounds, rect);
//            }
			return CaptureRectangleNative(rect);
		}
		public static Bitmap CaptureWindow(IntPtr handle)
		{
			if (handle.ToInt32() > 0) {
				Rectangle rect;
				//if (CaptureClientArea) {
				rect = GetClientRect(handle);
//				} else {
//					rect = CaptureHelpers.GetWindowRectangle(handle);
//				}
				//bool isTaskbarHide = false;
//                try
//                {
//                    if (AutoHideTaskbar)
//                    {
//                        isTaskbarHide = NativeMethods.SetTaskbarVisibilityIfIntersect(false, rect);
//                    }
//
				return CaptureRectangle(rect);
//                }
//                finally
//                {
//                    if (isTaskbarHide)
//                    {
//                        NativeMethods.SetTaskbarVisibility(true);
//                    }
//                }
			}
			return null;
		}
		public static string GenerateRandomString(int length)
		{
			var coupon = new StringBuilder();
			var rng = new RNGCryptoServiceProvider();
			var rnd = new byte[1];
			var n = 0;
			while (n < length) {
				rng.GetBytes(rnd);
				var c = (char)rnd[0];
				if (c <= 122 && c >= 97) {
					++n;
					coupon.Append(c);
				}
			}
			return coupon.ToString();
		}
		public static Rectangle GetClientRect(IntPtr handle)
		{
			RECT rect;
			GetClientRect(handle, out  rect);
			Point position = rect.Location;
			ClientToScreen(handle, ref position);
			return new Rectangle(position, rect.Size);
		}
		public static string GetDesktopPath(string f)
		{
			var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			dir = Path.Combine(dir, "图片");
			if (!Directory.Exists(dir)) {
				Directory.CreateDirectory(dir);
			}
			return Path.Combine(dir, f);
		}
		public static void SaveScreenshot()
		{
			POINT p;
			GetCursorPos(out p);
			IntPtr hwnd = WindowFromPoint(p);//handle here
			var bitmap = CaptureWindow(hwnd);
			//string.Format("{0}-{1}.jpg", DateTime.Now.ToString("yyyy-MM-dd"), GenerateRandomString(6))
			var src = GetDesktopPath(string.Format("{0}.jpg", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
			
			WriteBitmapToFile(src, bitmap);
		}
		public static Bitmap GetScreenshot()
		{
			POINT p;
			GetCursorPos(out p);
			IntPtr hwnd = WindowFromPoint(p);//handle here
			return CaptureWindow(hwnd);
			
		}
		public static void WriteBitmapToFile(string filename, Bitmap bitmap)
		{
			bitmap.Save(filename, ImageFormat.Jpeg);
		}
	}


}