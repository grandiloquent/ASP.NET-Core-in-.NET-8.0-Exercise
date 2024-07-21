
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;

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
			A = 0x41,
			B = 0x42,
			D = 0x44,
			G = 0x47,
			I = 0x49,
			S = 0x53,
			V = 0x56,
			X = 0x58,
			Y = 0x59,
			Z = 0x5A,
		}
		const uint KEYEVENTF_KEYUP = 0x0002;
		public const int INPUT_KEYBOARD = 1;
		public static void Main(string[] args)
		{
			//Directory.Delete(@"C:\Users\Administrator\AppData\Local\JetBrains\Fleet",true);
			//GeneratorTri();
			//RegisterHotKey(IntPtr.Zero, 81, 0, 81);//Q
			//RegisterHotKey(IntPtr.Zero, 87, 0, 87);//W 
			
			//RegisterHotKey(IntPtr.Zero, 68, 0, 68);//D
			//RegisterHotKey(IntPtr.Zero, 70, 0, 70);//F
			RegisterHotKey(IntPtr.Zero, 0x51, 0, 0x51);//Q
			RegisterHotKey(IntPtr.Zero, 65, 0, 65);//A
			RegisterHotKey(IntPtr.Zero, 0x53, 0, 0x53);//S
			
			MSG msg;
			int ret;
			while ((ret = GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0) {
				if (ret == 1 && msg.message == 0x0312) {
					ushort id = (ushort)msg.wParam;
					
					if (id == 0x51) {
						//BlenderGeometryNodes();
						PhotoshoBrush();
						/*
						BlenderDuplicate();
					*/
				
					} else if (id == 65) {
						
						keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event(219, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event(219, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					} else if (id == 0x53) {
						keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event(0x42, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event(221, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press
						keybd_event(221, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
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
			keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.Y, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
			keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
			keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
			keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
			keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
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
	}
}