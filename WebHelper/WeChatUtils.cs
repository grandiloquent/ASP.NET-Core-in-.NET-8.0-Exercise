using System;
using System.IO;
using System.Linq;
using System.Reflection;
namespace ShaderToy
{
	public static class WeChatUtils
	{
		
		public static void CreatePage(string name)
		{
			var baseDir = @"C:\blender";
			var entry = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var dir = Path.Combine(baseDir, "pages", name);
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			var extensions = new string[]{ "js", "json", "wxml", "wxss" };
			foreach (var element in extensions) {
				var file = Path.Combine(dir, name + "." + element);
				if (!File.Exists(file)) {
					File.Copy(Path.Combine(entry, "x." + element), file);
				}
			}
			var app = Path.Combine(baseDir, "app.json");
			var contents = File.ReadAllText(app);
			var index = contents.IndexOf(']');
			contents = contents.Insert(index - 1, ",\r\n\"pages/" + name + "/" + name + "\"\r\n");
			File.WriteAllText(app, contents);
			
		}
 
		public static void CreateFile(string name)
		{
			var baseDir = @"C:\blender";
			var dir = Path.Combine(baseDir, "utils");
			var file = Path.Combine(dir, name + ".js");
			if (File.Exists(file))
				return;
			var s = string.Format(@"
module.exports = () => {{
	return new Promise((reslove, reject) => {{
		wx.request({{
			url: '',
			success: res => {{
			 
			if(res.statusCode>=200 && res.statusCode<400)
				reslove(res.data)
				else
				reject(new Error(res.statusCode));
			}},
			fail: err => {{
				reject(err)
			}}
		}})
	}})
}}
// const {1}=require(""../../utils/{0}"")", name, name.Camel());
			File.WriteAllText(file, s);
			
		}
		public static void GenerateHtmlElment(string name)
		{
			var s = string.Format(@"<view class=""{0}"">

    </view>", name);
			var file = @"C:\blender\pages\index\index.wxml";
			 
			var str = File.ReadAllText(file);
			str =str.Contains("<!---->") ?str.Replace("<!---->",s+Environment.NewLine +"<!---->"): str + Environment.NewLine + s;
			File.WriteAllText(file, str);
		}
		public static void GenerateCss(string name, string content)
		{
			var s = string.Format(@".{0}{{
			{1}
}}", name, content);
			var file = @"C:\blender\app.wxss";
			 
			var str = File.ReadAllText(file);
			str = str + Environment.NewLine + s;
			File.WriteAllText(file, str);
		}
		public static void SortCode()
		{
			var f = @"C:\blender\app.wxss";
			var contents = File.ReadAllText(f);
			var blocks =	contents.ToBlocks()
				.OrderBy(x => {
				return x.SubstringBefore('{').Trim();
			}).ToArray();
			File.WriteAllText(f, string.Join(Environment.NewLine, blocks).RemoveWhiteSpaceLines());
		}
		public static void RefactorCss(string style, string name)
		{
			
			var htmlFile = @"C:\blender\pages\index\index.wxml";
			var cssFile = @"C:\blender\app.wxss";
			var str = File.ReadAllText(cssFile);
			str = str + Environment.NewLine + string.Format(@".{0}{{

{1}
}}", name, style);
			File.WriteAllText(cssFile, str);
			ClipboardShare.SetText(string.Format(@" class=""{0}"" ", name));
			
		}
		public static void RefactorWxss(string name)
		{
			
		
			var cssFile = @"C:\blender\app.wxss";
			var str = File.ReadAllText(cssFile);
			str = str + Environment.NewLine + string.Format(@"@import ""{0}.wxss"";", name);
			File.WriteAllText(cssFile, str);
			var file=Path.Combine(Path.GetDirectoryName(cssFile),name+".wxss");
			if(!File.Exists(file)){
				File.Create(file).Dispose();
			}
			
		}
		public static void RefactorJs(string name)
		{
			
			 
			var jsFile = @"C:\blender\pages\index\index.js";
			var str = File.ReadAllText(jsFile);
			str = str.SubstringBeforeLast("}") + Environment.NewLine + string.Format(@",{0}(e){{
const value=e.detail.value;
const id = e.target.dataset.id;

this.setData({{

}});
}}
}});", name);
			File.WriteAllText(jsFile, str);
		}
	}
}