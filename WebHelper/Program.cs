
using System;
using System.Linq;
using System.Text.RegularExpressions;
using EventHook;
using System.IO;
namespace ShaderToy
{
	
	class Program
	{
		static string _lastName;
		private static void Web(string ss)
		{
			
			if (Regex.IsMatch(ss, "^0")) {
				WebUtils.GenerateFile(Regex.Replace(ss, "^\\d+", ""));
			} else if (Regex.IsMatch(ss, "^[a-z][a-z0-9-]+$")) {
				_lastName = ss;
				WebUtils.GenerateHtmlElment(ss);
			} else if (Regex.IsMatch(ss, "[a-z]: +[a-z0-9]")) {
				if (string.IsNullOrWhiteSpace(_lastName))
					_lastName = "div";
				WebUtils.GenerateCss(_lastName, ss);
			} else if (Regex.IsMatch(ss, "^class=\"[a-z0-9-]")) {
							 
				WebUtils.GenerateJavaScript(Regex.Match(ss, "(?<=class=\")[^\"]+").Value);
			}
		}
		private static void WeChat(string ss)
		{
			if (Regex.IsMatch(ss, "^0[a-z]")) {
				WeChatUtils.CreatePage(Regex.Replace(ss, "^\\d+", "").Trim());
			} else if (Regex.IsMatch(ss, "^1[a-z]")) {
				WeChatUtils.CreateFile(Regex.Replace(ss, "^\\d+", "").Trim());
			} else if (Regex.IsMatch(ss, "^2[a-z]")) {
				WeChatUtils.RefactorWxss(Regex.Replace(ss, "^\\d+", "").Trim());
			} else if (Regex.IsMatch(ss, "^style=\"[^\"]+\"[a-z0-9_-]+$")) {
				WeChatUtils.RefactorCss(ss.SubstringTakeout("\"", "\""), ss.SubstringAfterLast('\"'));
			} else if (Regex.IsMatch(ss, "^[a-z:]+=\"[^\"]+\"$")) {
				WeChatUtils.RefactorJs(ss.SubstringTakeout("\"", "\""));
			} else if (Regex.IsMatch(ss, "^\\.[a-z][a-z0-9-]+$")) {
				_lastName = ss.Substring(1);
				WeChatUtils.GenerateHtmlElment(_lastName);
			} else if (Regex.IsMatch(ss, "[a-z]: +[a-z0-9]")) {
				if (string.IsNullOrWhiteSpace(_lastName))
					_lastName = "view";
				WeChatUtils.GenerateCss(_lastName, ss);
			} else if (Regex.IsMatch(ss, "^sss")) {
				if (string.IsNullOrWhiteSpace(_lastName))
					WeChatUtils.SortCode();
			} 
		}
		public static void Main(string[] args)
		{
		
			var dir = @"C:\blender\resources";
			foreach (var path in Directory.GetFiles(dir,"*.*")) {
				if (!Regex.IsMatch(path, "\\.(?:png|jfif)$"))
					continue;
				System.Drawing.Bitmap buffer = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(path);
		
				Imazen.WebP.SimpleEncoder j = new Imazen.WebP.SimpleEncoder();
				var o = new FileStream(Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + ".webp"), FileMode.OpenOrCreate);
				j.Encode(buffer, o, 80, true);
				buffer.Dispose();
				o.Close();
				File.Delete(path);
			}
			
			using (var eventHookFactory = new EventHookFactory()) {
				
				var clipboardWatcher = eventHookFactory.GetClipboardWatcher();
				clipboardWatcher.Start();
				clipboardWatcher.OnClipboardModified += (s, e) => {
					try {
						var ss = e.Data.ToString();
						WeChat(ss);
						///Web(ss);
//						var ss = e.Data.ToString();

					} catch (Exception ex) {
						Console.WriteLine(ex);
						Console.WriteLine(e.Data.ToString());
					}
				};
			}
			Console.Read();
		}
	}
	
}