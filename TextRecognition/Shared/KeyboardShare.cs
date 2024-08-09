

using System.Runtime.InteropServices;

public class KeyboardShare
{
    public delegate IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);

    public delegate void KeyEvent(object sender, KeyEventArg e);

    private event KeyboardHookProc keyhookevent;
    public event KeyEvent KeyDown;
    public event KeyEvent KeyUp;
    private IntPtr hookPtr;

    public KeyboardShare()
    {
        this.keyhookevent += KeyboardHook_keyhookevent;
    }

    [DllImport("User32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hook, int code, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern short GetKeyState(int keyCode);

    [DllImport("User32.dll")]
    private static extern IntPtr SetWindowsHookExA(HookID hookID, KeyboardHookProc lpfn, IntPtr hmod,
        int dwThreadId);

    [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    public static extern int DispatchMessage([In] ref MSG msg);

    [DllImport("user32.dll")]
    public static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
        uint wMsgFilterMax);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool TranslateMessage([In, Out] ref MSG msg);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    private IntPtr KeyboardHook_keyhookevent(int code, IntPtr wParam, IntPtr lParam)
    {
        KeyStaus ks = (KeyStaus)wParam.ToInt32();
        KeyboardHookStruct khs = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
        KeyEvent ke = ks == KeyStaus.KeyDown || ks == KeyStaus.SysKeyDown ? KeyDown : KeyUp;
        if (ke != null)
        {
            ke.Invoke(this, new KeyEventArg()
            {
                Key = khs.Key,
                KeyStaus = ks
            });
        }

        return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
    }

    public void ConfigHook()
    {
        hookPtr = SetWindowsHookExA(HookID.Keyboard_LL, keyhookevent, IntPtr.Zero, 0);
        if (hookPtr == null)
            throw new Exception();
    }

    public static bool isKeyPressed(int keyCode)
    {
        return (GetKeyState(keyCode) & 0x8000) != 0;
    }
}
