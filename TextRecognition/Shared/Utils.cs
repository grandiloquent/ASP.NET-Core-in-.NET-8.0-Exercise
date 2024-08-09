using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

public class Utils
{
    public static string Translate(string s = "")
    {
        //string q
        // http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=%s&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
        // en
        // http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=en&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=
        var l = "en";
        s = s == "" ? ClipboardShare.GetText() : s;

        var isChinese = Regex.IsMatch(s, "[\u4e00-\u9fa5]");
        if (!isChinese)
        {
            l = "zh";
        }
        var req = WebRequest.Create(
                      "http://translate.google.com/translate_a/single?client=gtx&sl=auto&tl=" + l + "&dt=t&dt=bd&ie=UTF-8&oe=UTF-8&dj=1&source=icon&q=" +
                      s);
        //req.Proxy = new WebProxy("127.0.0.1", 10809);
        var res = req.GetResponse();
        using (var reader = new StreamReader(res.GetResponseStream()))
        {
            var obj =
          (JsonElement)JsonSerializer.Deserialize<Dictionary<String, dynamic>>(reader.ReadToEnd())["sentences"];
            //var obj = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd())["sentences"].ToObject<JArray>();
            var sb = new StringBuilder();
            for (int i = 0; i < obj.GetArrayLength(); i++)
            {
                sb.Append(obj[i].GetProperty("trans").GetString()).Append(' ');
            }
            // Regex.Replace(sb.ToString().Trim(), "[ ](?=[a-zA-Z0-9])", m => "_").ToLower();
            // std::string {0}(){{\n}}
            //return string.Format("{0}", Regex.Replace(sb.ToString().Trim(), " ([a-zA-Z0-9])", m => m.Groups[1].Value.ToUpper()).Decapitalize());
            //return  sb.ToString().Trim();
            /*
			 sb.ToString().Trim();
			 .Trim().Camel().Capitalize()
			 */
            return isChinese ? sb.ToString() : sb.ToString();
        }
        //Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
    }
}