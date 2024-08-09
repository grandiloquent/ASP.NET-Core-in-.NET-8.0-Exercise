using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

public static class ClipboardShare
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
        if (!IsClipboardFormatAvailable(15))
        {
            var n = GetText();
            if (Directory.Exists(n) || File.Exists(n))
            {
                return new string[] { n };
            }
            return null;
        }
        IntPtr handle = IntPtr.Zero;
        try
        {
            OpenClipboard();
            handle = GetClipboardData(15);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            var count = DragQueryFile(handle, unchecked((int)0xFFFFFFFF), null, 0);
            if (count == 0)
            {
                return Enumerable.Empty<string>();
            }
            var sb = new StringBuilder(260);
            var files = new string[count];
            for (var i = 0; i < count; i++)
            {
                var charlen = DragQueryFile(handle, i, sb, sb.Capacity);
                var s = sb.ToString();
                if (s.Length > charlen)
                {
                    s = s.Substring(0, charlen);
                }
                files[i] = s;
            }
            return files;
        }
        finally
        {
            CloseClipboard();
        }
    }

    public static string GetText()
    {
        if (!IsClipboardFormatAvailable(cfUnicodeText))
        {
            return null;
        }
        IntPtr handle = IntPtr.Zero;
        IntPtr pointer = IntPtr.Zero;
        try
        {
            OpenClipboard();
            handle = GetClipboardData(cfUnicodeText);
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            pointer = GlobalLock(handle);
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            var size = GlobalSize(handle);
            var buff = new byte[size];
            Marshal.Copy(pointer, buff, 0, size);
            return Encoding.Unicode.GetString(buff).TrimEnd('\0');
        }
        finally
        {
            if (pointer != IntPtr.Zero)
            {
                GlobalUnlock(handle);
            }
            CloseClipboard();
        }
    }
    public static void OpenClipboard()
    {
        var num = 10;
        while (true)
        {
            if (OpenClipboard(IntPtr.Zero))
            {
                break;
            }
            if (--num == 0)
            {
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
        try
        {
            var bytes = (text.Length + 1) * 2;
            hGlobal = Marshal.AllocHGlobal(bytes);
            if (hGlobal == IntPtr.Zero)
            {
                ThrowWin32();
            }
            var target = GlobalLock(hGlobal);
            if (target == IntPtr.Zero)
            {
                ThrowWin32();
            }
            try
            {
                Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
            }
            finally
            {
                GlobalUnlock(target);
            }
            // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclipboarddata
            if (SetClipboardData(cfUnicodeText, hGlobal) == IntPtr.Zero)
            {
                ThrowWin32();
            }
            hGlobal = IntPtr.Zero;
        }
        finally
        {
            if (hGlobal != IntPtr.Zero)
            {
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
