
using System;
using System.Linq;
using System.Text.RegularExpressions;
using EventHook;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ShaderToy
{
	
	public class WebUtils
	{
			static string _file=@"D:\.Folder\003\Yun\app\src\main\assets\index.js";
		public static void GenerateHtmlElment(string name)
		{
			var s = string.Format(@"<div class=""{0}"">

    </div>", name);
			var file =Path.ChangeExtension(_file,".html");
			var p = "</body>";
			var str = File.ReadAllText(file);
			str = str.Replace(p, s + Environment.NewLine + p);
			File.WriteAllText(file, str);
		}
		public static void GenerateCss(string name,string content)
		{
			var s = string.Format(@".{0}{{
			{1}
}}",name, content);
			var file =Path.ChangeExtension(_file,".css");
			 
			var str = File.ReadAllText(file);
			str = str + Environment.NewLine + s;
			File.WriteAllText(file, str);
		}

		
		public static void GenerateJavaScript(string name)
		{
			var s = string.Format(@"const {1} = document.querySelector('.{0}');
{1}.addEventListener('click', evt => {{
    evt.stopPropagation();
    evt.preventDefault();
    evt.stopImmediatePropagation();
    
}})

document.querySelectorAll('.{0}')
    .forEach(element => {{
        element.addEventListener('click', evt => {{
            evt.stopPropagation();
        }})
    }});

",name,name.Camel());
			var file =_file;
			 
			var str = File.ReadAllText(file);
			str = str + Environment.NewLine + s;
			File.WriteAllText(file, str);
		}
		
		public static void GenerateFile(string name){
			
			var s = string.Format(@"<script src=""js/{0}.js""></script>", name);
			var file =_file;
			var dir=Path.Combine(Path.GetDirectoryName(file),"js");
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			var j=Path.Combine(dir,name+".js");
			if(!File.Exists(j))
				File.WriteAllText(j,string.Empty);
			var p = "</html>";
			var str = File.ReadAllText(file);
			str = str.Replace(p, s + Environment.NewLine + p);
			File.WriteAllText(file, str);
		}
	}
}