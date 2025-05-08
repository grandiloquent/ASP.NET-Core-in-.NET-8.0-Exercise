using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class Extensions
{
	public static string GetEntryPath(this string filename)
	{
		return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), filename);
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
	public static string GetCurrentLine(TextBox textBox)
	{
		var s = textBox.Text;
		var i = textBox.SelectionStart;
		var j = textBox.SelectionStart + textBox.SelectionLength;
			
		while (i > 0 && s[i - 1] != '\n') {
			i--;
		}
		while (j < s.Length && s[j] != '\n') {
			j++;
		}
		return s.Substring(i, j - i).Trim();
	}
	public static long GetDirectorySize(string folderPath)
	{
		DirectoryInfo di = new DirectoryInfo(folderPath);
		return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
	}
	// Returns the human-readable file size for an arbitrary, 64-bit file size
	// The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
	public static string GetBytesReadable(long i)
	{
		// Get absolute value
		long absolute_i = (i < 0 ? -i : i);
		// Determine the suffix and readable value
		string suffix;
		double readable;
		if (absolute_i >= 0x1000000000000000) { // Exabyte
			suffix = "EB";
			readable = (i >> 50);
		} else if (absolute_i >= 0x4000000000000) { // Petabyte
			suffix = "PB";
			readable = (i >> 40);
		} else if (absolute_i >= 0x10000000000) { // Terabyte
			suffix = "TB";
			readable = (i >> 30);
		} else if (absolute_i >= 0x40000000) { // Gigabyte
			suffix = "GB";
			readable = (i >> 20);
		} else if (absolute_i >= 0x100000) { // Megabyte
			suffix = "MB";
			readable = (i >> 10);
		} else if (absolute_i >= 0x400) { // Kilobyte
			suffix = "KB";
			readable = i;
		} else {
			return i.ToString("0 B"); // Byte
		}
		// Divide by 1024 to get fractional value
		readable = (readable / 1024);
		// Return formatted number with suffix
		return readable.ToString("0.### ") + suffix;
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
	public static String Snake(this string s)
	{
		if (s == null)
			return null;
		s = Regex.Replace(s, "[A-Z]", m => "_" + m.Value.ToLower());
		return Regex.Replace(s, "[ -]+", m => "_")
			.TrimStart('_');
	}
}