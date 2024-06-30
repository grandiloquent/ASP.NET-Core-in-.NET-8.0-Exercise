
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
		public static void Main(string[] args)
		{
			RegisterHotKey(IntPtr.Zero, 81, 0, 81);
			RegisterHotKey(IntPtr.Zero, 87, 0, 87);
			RegisterHotKey(IntPtr.Zero, 65, 0, 65);
			RegisterHotKey(IntPtr.Zero, 68, 0, 68);
			RegisterHotKey(IntPtr.Zero, 70, 0, 70);
			
			MSG msg;
			int ret;
			while ((ret = GetMessage(out msg, IntPtr.Zero, 0, 0)) != 0) {
				if (ret == 1 && msg.message == 0x0312) {
					ushort id = (ushort)msg.wParam;
					if (id == 81) {
						new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_E);
						//System.Threading.Thread.Sleep(200);
						new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
						new WindowsInput.InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Z);
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
				} else {
					
					TranslateMessage(ref msg);
					DispatchMessage(ref msg);
				}
			}
		}
	}
}