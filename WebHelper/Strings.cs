using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace ShaderToy
{
	public static class Strings
	{
	
		public static string GetEntryPath(this string filename)
		{
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), filename);
		}
		public static string StripComments(this string code)
		{
			var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
	
		private static void AppendCharAsUnicodeJavaScript(StringBuilder builder, char c)
		{
			builder.AppendFormat("\\u{0:x4}", (int)c);
		}
		private static bool CharRequiresJavaScriptEncoding(char c)
		{
			return
            c < 0x20// control chars always have to be encoded
			|| c == '\"'// chars which must be encoded per JSON spec
			|| c == '\\'
			|| c == '\''// HTML-sensitive chars encoded for safety
			|| c == '<'
			|| c == '>'
			|| (c == '&')
			|| c == '\u0085'// newline chars (see Unicode 6.2, Table 5-1 [http://www.unicode.org/versions/Unicode6.2.0/ch05.pdf]) have to be encoded
			|| c == '\u2028'
			|| c == '\u2029';
		}
		private static string Concatenate(this IEnumerable<string> strings,
			Func<StringBuilder, string, StringBuilder> builderFunc)
		{
			return strings.Aggregate(new StringBuilder(), builderFunc).ToString();
		}
		// https://crates.io/crates/convert_case
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
		public static string ConcatenateLines(this IEnumerable<string> strings)
		{
			return Concatenate(strings, (StringBuilder builder, string nextValue) => builder.AppendLine(nextValue));
		}
		public static string Concatenates(this IEnumerable<string> strings, string separator)
		{
			return Concatenate(strings,
				(StringBuilder builder, string nextValue) => builder.Append(nextValue).Append(separator));
		}
		public static string Concatenates(this IEnumerable<string> strings)
		{
			return Concatenate(strings, (builder, nextValue) => builder.Append(nextValue));
		}
		public static String Decapitalize(this String s)
		{
			if (string.IsNullOrEmpty(s))
				return s;
			if (s.Length == 1)
				return s.ToUpper();
			if (char.IsLower(s[0]))
				return s;
			return char.ToLower(s[0]) + s.Substring(1);
		}
	
		public static string JavaScriptStringEncode(string value)
		{
			if (string.IsNullOrEmpty(value)) {
				return string.Empty;
			}
			StringBuilder b = null;
			int startIndex = 0;
			int count = 0;
			for (int i = 0; i < value.Length; i++) {
				char c = value[i];
				// Append the unhandled characters (that do not require special treament)
				// to the string builder when special characters are detected.
				if (CharRequiresJavaScriptEncoding(c)) {
					if (b == null)
						b = new StringBuilder(value.Length + 5);
					if (count > 0) {
						b.Append(value, startIndex, count);
					}
					startIndex = i + 1;
					count = 0;
					switch (c) {
						case '\r':
							b.Append("\\r");
							break;
						case '\t':
							b.Append("\\t");
							break;
						case '\"':
							b.Append("\\\"");
							break;
						case '\\':
							b.Append("\\\\");
							break;
						case '\n':
							b.Append("\\n");
							break;
						case '\b':
							b.Append("\\b");
							break;
						case '\f':
							b.Append("\\f");
							break;
						default:
							AppendCharAsUnicodeJavaScript(b, c);
							break;
					}
				} else {
					count++;
				}
			}
			if (b == null) {
				return value;
			}
			if (count > 0) {
				b.Append(value, startIndex, count);
			}
			return b.ToString();
		}
		public static void Log(this string message)
		{
		
			//Console.WriteLine(message);
		}
		public static string RemoveWhiteSpaceLines(this string str)
		{
			return string.Join(Environment.NewLine,
				str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where(i => !string.IsNullOrWhiteSpace(i)));
		}
		public static string SubstringAfter(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + delimiter.Length);
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBefore(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringTakeout(this string value, string startDelimiter, string endDelimiter)
		{
			var startIndex = value.IndexOf(startDelimiter);
			if (startIndex == -1)
				return value;
			startIndex+=startDelimiter.Length;
			var endIndex = value.IndexOf(endDelimiter, startIndex );
			if (endIndex == -1)
				return value;
			return value.Substring(  startIndex,endIndex-startIndex)  ;
		}
		public static string SubstringBlock(this string value, string delimiter, string s)
		{
			var startIndex = value.LastIndexOf(delimiter);
			if (startIndex == -1)
				return value;
			var count = 0;
			startIndex += delimiter.Length;
			for (int index = startIndex; index < value.Length; index++) {
	
				if (value[index] == '{') {
					count++;
				} else if (value[index] == '}') {
					count--;
					if (count == 0) {
						var s1 = value.Substring(0, startIndex);
						var s2 = s + value.Substring(index + 1);
						return value.Substring(0, startIndex) + s + value.Substring(index + 1);
					}
				}
			}
			return value;
		}
		public static IEnumerable<string> ToBlocks(this string value)
		{
			var count = 0;
			StringBuilder sb = new StringBuilder();
			List<string> ls = new List<string>();
			foreach (var t in value) {
				sb.Append(t);
				switch (t) {
					case '{':
						count++;
						continue;
					case '}':
						{
							count--;
							if (count == 0) {
								ls.Add(sb.ToString());
								sb.Clear();
							}
							continue;
						}
				}
			}
			return ls;
		}
	
		public static string UpperCamel(this string value)
		{
			return value.Camel().Capitalize();
		}
		public static String Snake(this string s)
		{
			if (s == null)
				return null;
			s = Regex.Replace(s, "[A-Z]", m => "_" + m.Value.ToLower());
			return Regex.Replace(s, "[ -]+", m => "_")
			.TrimStart('_');
		}
		public static string FormatString(this string s)
		{
			s = s.Replace("\"", "\"\"")
			.Replace("{", "{{")
			.Replace("}", "}}");
			// ClipboardShare.SetText()
			s = string.Format("string.Format(@\"{0}\")", s);
			//s = string.Format("{0}", s);
			//s = string.Format("string.Format(@\"{0}\")", s);
			return string.Format("{0}", s);
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
}

}
