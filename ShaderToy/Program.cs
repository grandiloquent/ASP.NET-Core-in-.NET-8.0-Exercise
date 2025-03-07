
using System;
using System.Linq;
using System.Text.RegularExpressions;
using EventHook;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace ShaderToy
{
	
	public class ShaderToy
	{
	
		public static void Fetch(string dir, string uri)
		{
			if (!uri.Contains("shadertoy.com")) {
				return;
			}
			var id = uri.SubstringAfter("/view/").SubstringBefore("/");
			var req = (HttpWebRequest)WebRequest.Create("https://www.shadertoy.com/shadertoy");
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
			//req.Headers.Add("Cookie", File.ReadAllText("cookie.txt".GetEntryPath()));
			req.Referer = uri;
			var bytes = new UTF8Encoding(false).GetBytes("s=%7B%20%22shaders%22%20%3A%20%5B%22" + id + "%22%5D%20%7D&nt=1&nl=1&np=1");
			req.GetRequestStream().Write(bytes, 0, bytes.Length);
			using (var reader = new StreamReader(req.GetResponse().GetResponseStream())) {
				var s = reader.ReadToEnd();
//				s=File.ReadAllText(
//					Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),"1.txt")
//				).Replace("{{0}}",s);
//				File.WriteAllText(Path.Combine(dir,id + ".txt"),s);
			
			
				ClipboardShare.SetText(string.Format("ShaderToy\n\n{0}", s));
				
			}
		
		}
		private static void PostData(string s)
		{
			
			var req = (HttpWebRequest)WebRequest.Create("http://192.168.35.56:8100/svg");
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

			var bytes = new UTF8Encoding(false).GetBytes("s=%7B%20%22shaders%22%20%3A%20%5B%22" + id + "%22%5D%20%7D&nt=1&nl=1&np=1");
			req.GetRequestStream().Write(bytes, 0, bytes.Length);
			using (var reader = new StreamReader(req.GetResponse().GetResponseStream())) {
				var s = reader.ReadToEnd();
//				s=File.ReadAllText(
//					Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),"1.txt")
//				).Replace("{{0}}",s);
//				File.WriteAllText(Path.Combine(dir,id + ".txt"),s);
			
			
			}
		}
	}
	class Program
	{
		public static void Main(string[] args)
		{
	
			
			using (var eventHookFactory = new EventHookFactory()) {
				
				var clipboardWatcher = eventHookFactory.GetClipboardWatcher();
				clipboardWatcher.Start();
				clipboardWatcher.OnClipboardModified += (s, e) => {
					try {
						ShaderToy.Fetch(@"C:\Users\Administrator\Desktop", e.Data.ToString());
					
						Console.WriteLine(e.Data.ToString());
					} catch (Exception ex) {
						Console.WriteLine(ex);
						Console.WriteLine(e.Data.ToString());
					}
				};
			}
			Console.Read();
		}
	}
	
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
			var startIndex = value.LastIndexOf(startDelimiter);
			if (startIndex == -1)
				return value;
			var endIndex = value.LastIndexOf(endDelimiter, startIndex + startDelimiter.Length);
			if (endIndex == -1)
				return value;
			return value.Substring(0, startIndex) + value.Substring(endIndex + endDelimiter.Length);
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
					case '(':
						count++;
						continue;
					case ')':
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
}