using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Tesseract;
using static Screenshot;

public class Utils
{
    static int sampleSize = 5;
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
            s = Regex.Replace(s, "[\r\n]+", " ");
            s = s.Replace("- ", "");
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
            return isChinese ? $"public static String {sb.ToString().Trim().Camel().DeCapitalize()}(){{\n\n return \"\";\n\n}}" : sb.ToString();
        }
        //Clipboard.SetText(string.Format(@"{0}", TransAPI.Translate(Clipboard.GetText())));
    }
    private static Bitmap GetSampleRegion(int mouseX, int mouseY)
    {
        var bmp = new Bitmap(sampleSize, sampleSize, PixelFormat.Format32bppArgb);
        Graphics gfxScreenshot = Graphics.FromImage(bmp);
        gfxScreenshot.CopyFromScreen(mouseX - sampleSize / 2, mouseY - sampleSize / 2, 0, 0, new Size(sampleSize, sampleSize));
        gfxScreenshot.Save();
        gfxScreenshot.Dispose();
        return bmp;
    }
    public static void ColorPicker()
    {
        POINT p;
        GetCursorPos(out p);
        int mouseX = p.X;
        int mouseY = p.Y;
        var sampleBitmap = GetSampleRegion(mouseX, mouseY);
        Color sampleColor = sampleBitmap.GetPixel(sampleSize / 2, sampleSize / 2);
        sampleBitmap.Dispose();
        //			string tmpR = (sampleColor.R / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
        //			string tmpG = (sampleColor.G / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
        //			string tmpB = (sampleColor.B / 255f).ToString("0.##f", CultureInfo.GetCultureInfo("en-us"));
        //			var s = string.Format("{0}, {1}, {2}", tmpR, tmpG, tmpB);
        //var s = ColorTranslator.ToHtml(sampleColor);
        //ClipboardShare.SetText(s.Substring(1));
        int red = sampleColor.R;
        int green = sampleColor.G;
        int blue = sampleColor.B;
        double r_output = Math.Pow(red / 255.0, 2.2);
        double g_output = Math.Pow(green / 255.0, 2.2);
        double b_output = Math.Pow(blue / 255.0, 2.2);
        ClipboardShare.SetText(string.Format("{0}\r\n{1}\r\n{2}", r_output, g_output, b_output));
    }
    static TesseractEngine _engine;
    public static string Ocr(Point point1, Point point2)
    {
        var buf = ImageUtils.ScreenShoot(point1, point2);
        if (buf == null)
            return null;
        if (_engine == null)
            _engine = new TesseractEngine(@"C:\Users\Administrator\Desktop\视频\Net\TextRecognition\tessdata_best".GetEntryPath(), "eng", EngineMode.Default);

        using (var img = Pix.LoadFromMemory(buf))
        {
            using (var page = _engine.Process(img))
            {
                var text = page.GetText();
                //						Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());
                //
                //						Console.WriteLine("Text (GetText): \r\n{0}", text);
                //						Console.WriteLine("Text (iterator):");
                //						


                return text;
            }
        }
    }

    public static void MakeFile()
    {
        var dir = @"D:\Documents\Repositories\AutoClicker\app\src\main\java\psycho\euphoria\autoclicker";
        var filename = Path.Combine(dir, "Utils.java");
        if (!File.Exists(filename))
        {
            File.Create(filename).Dispose();
        }

    }
}