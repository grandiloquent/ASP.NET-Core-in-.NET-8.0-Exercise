
using System;
using System.Runtime.InteropServices;

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
			D = 0x44,
			G = 0x47,
			V = 0x56,
			X = 0x58,
			Y = 0x59,
			Z = 0x5A,
		}
		const uint KEYEVENTF_KEYUP = 0x0002;
		public const int INPUT_KEYBOARD = 1;
		public static void Main(string[] args)
		{
			
			RegisterHotKey(IntPtr.Zero, 81, 0, 81);//Q
			RegisterHotKey(IntPtr.Zero, 87, 0, 87);//W 
			RegisterHotKey(IntPtr.Zero, 65, 0, 65);//A
			RegisterHotKey(IntPtr.Zero, 68, 0, 68);//D
			RegisterHotKey(IntPtr.Zero, 70, 0, 70);//F
			
			MSG msg;
			int ret;
			while ((ret = GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0) {
				if (ret == 1 && msg.message == 0x0312) {
					ushort id = (ushort)msg.wParam;
					Console.WriteLine(id);
					if (id == 81) {
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
					
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
				
//						new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_E);
//						new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
//						new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
						
						//new WindowsInput.InputSimulator().Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, WindowsInput.Native.VirtualKeyCode.VK_D);
						//	new WindowsInput.InputSimulator().Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, WindowsInput.Native.VirtualKeyCode.VK_D);
						//	new WindowsInput.InputSimulator().Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.SHIFT, WindowsInput.Native.VirtualKeyCode.VK_D);
					} else if (id == 87) {
					
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.G, (byte)MapVirtualKey((uint)VK.G, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
				
						
						//System.Threading.Thread.Sleep(100);
						//Utils.Press(0x45);
						//System.Threading.Thread.Sleep(100);
						//Utils.Press(0x59);
						 
						/*	new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_G);
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
				*/
					} else if (id == 65) {
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.D, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.D, (byte)MapVirtualKey((uint)VK.D, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release    	
						
//						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
//						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
//						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
//						keybd_event((int)VK.SHIFT, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0); // Alt Release 
					
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.Z, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						
						keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), 0, 0); //Alt Press  
						keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.V, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						keybd_event((int)VK.CTRL, (byte)MapVirtualKey((uint)VK.SHIFT, 0), KEYEVENTF_KEYUP, 0);
						
						keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), 0, 0); // N1 Press  
						keybd_event((int)VK.ENTER, (byte)MapVirtualKey((uint)VK.Z, 0), KEYEVENTF_KEYUP, 0); // N1 Release  
						
						
					}
				} else {
					
					TranslateMessage(ref msg);
					DispatchMessage(ref msg);
				}
			}
		}
	}
}