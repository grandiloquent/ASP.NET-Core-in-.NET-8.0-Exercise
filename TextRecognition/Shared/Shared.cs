using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

public static class Shared
{

    public static string GetUniqueFileName(this string dir, string extension)
    {
        string path;
        var i = 0;
        do
        {
            i++;
            path = Path.Combine(dir, i.ToString().PadLeft(3, '0') + extension);
        } while (File.Exists(path));
        return path;
    }
    public static string GetEntryPath(this string filename)
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), filename);
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
    public static String DeCapitalize(this String s)
    {
        if (string.IsNullOrEmpty(s))
            return s;
        if (s.Length == 1)
            return s.ToLower();
        if (char.IsLower(s[0]))
            return s;
        return char.ToLower(s[0]) + s.Substring(1);
    }
    public static string GetDesktopPath(this string f)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
    }
    public static void CreateDirectoryIfNotExists(this string path)
    {
        if (Directory.Exists(path))
            return;
        Directory.CreateDirectory(path);
    }
    public static void CreateFileIfNotExists(this string path)
    {
        if (File.Exists(path))
            return;
        File.Create(path).Dispose();
    }
    public static string FormatString(this string s)
    {
        s = s.Replace("\"", "\"\"")
            .Replace("{", "{{")
            .Replace("}", "}}");
        // ClipboardShare.SetText()
        //s = string.Format("var s1 = string.Format(@\"{0}\")", s);
        //s = string.Format("{0}", s);
        //s = string.Format("string.Format(@\"{0}\")", s);
        s = string.Format("var s1 = string.Join(Environment.NewLine,array.Select((x,i)=>string.Format(@\"{0}\")));", s);

        return string.Format("{0}", s);
    }
    public static IEnumerable<string> ToBlocks(this string value)
    {
        var count = 0;
        StringBuilder sb = new StringBuilder();
        List<string> ls = new List<string>();
        foreach (var t in value)
        {
            sb.Append(t);
            switch (t)
            {
                case '(':
                    count++;
                    continue;
                case ')':
                    {
                        count--;
                        if (count == 0)
                        {
                            ls.Add(sb.ToString());
                            sb.Clear();
                        }
                        continue;
                    }
            }
        }
        return ls;
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
        for (int index = startIndex; index < value.Length; index++)
        {

            if (value[index] == '{')
            {
                count++;
            }
            else if (value[index] == '}')
            {
                count--;
                if (count == 0)
                {
                    var s1 = value.Substring(0, startIndex);
                    var s2 = s + value.Substring(index + 1);
                    return value.Substring(0, startIndex) + s + value.Substring(index + 1);
                }
            }
        }
        return value;
    }

}