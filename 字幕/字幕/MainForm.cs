
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace 字幕
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			/*
			 console.error([...document.querySelectorAll('a.orm-Link-root')].map(x=>x.href.match(/9781837026418-[a-z0-9_]+/)&&x.href.match(/9781837026418-[a-z0-9_]+/)[0]).join('\n'))
			 */
			
			foreach (var element in Clipboard.GetText().Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries)) {
				Download(element);
			}
		}
		void Download(string id){
			var prefix=". ";
			var files=Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),"*.mp4")
				;
			prefix=((files==null?0:files.Length)+1).ToString().PadLeft(3,'0')+prefix;
			var cookie = File.ReadAllText(
				             Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "1.txt")
			             );
			var body = File.ReadAllText(
				           Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "5.txt")
			           );
			body = body.Replace("xxx",id);
			var res = Post("https://www.kaltura.com/api_v3/service/media/action/list", cookie, body);
			var obj = JsonConvert.DeserializeObject<JObject>(res)["objects"][0]
				.ToObject<JObject>();
			var entryid = obj["id"].ToString();
			var name = prefix+obj["name"].ToString().Replace(":"," ");
			DownloadSrt(entryid,name,cookie);
			DownloadVideo(entryid,name, cookie);
		}
		void DownloadSrt(string entryid, string name, string cookie)
		{
			var body = File.ReadAllText(
				           Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "2.txt")
			           );
			body = body.Replace("xxx", entryid);
			var res = Post("https://www.kaltura.com/api_v3/service/caption_captionasset/action/list", cookie, body);
			var id = JsonConvert.DeserializeObject<JObject>(res)["objects"][0]["id"]
				.ToObject<string>();
			body = File.ReadAllText(
				Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "3.txt")
			);
			body = body.Replace("xxx", id);
			res = Post("https://www.kaltura.com/api_v3/service/caption_captionasset/action/getUrl", cookie, body);
			DownFile(res.Trim('"').Replace("\\", ""),
				Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), name + ".srt")
			);
		}
		void DownloadVideo(string entryid, string name, string cookie)
		{
			var body = File.ReadAllText(
				           Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "6.txt")
			           );
			body = body.Replace("xxx", entryid);
			var res = Post("https://www.kaltura.com/api_v3/service/flavorasset/action/list", cookie, body);
			var id = JsonConvert.DeserializeObject<JObject>(res)["objects"][0]["id"]
				.ToObject<string>();
			body = File.ReadAllText(
				Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "3.txt")
			);
			body = body.Replace("xxx", id);
			res = Post("https://www.kaltura.com/api_v3/service/flavorasset/action/getUrl", cookie, body);
			// Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), name+".mp4")
			// 
			var url = res.Trim('"').Replace("\\", "");
			File.WriteAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), name + ".txt"), url);
			
			var p=Process.Start(new ProcessStartInfo() {
				FileName = "aria2c",
				Arguments = url + " --out=\"" + name + ".mp4\"",
				WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
			}
			
			        
			);
			p.WaitForExit();
		}
		string Post(string url, string cookie, string body)
		{
			ServicePointManager.Expect100Continue = true;

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			var req = (HttpWebRequest)WebRequest.Create(url);
			req.Headers.Add("cookie", cookie);
			req.ContentType = "application/json";
			req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36";
			//req.Referer = "https://learning.oreilly.com/videos/game-math-foundations/9781837026418/9781837026418-video2_1/";
			req.Method = "POST";
			var os =	req.GetRequestStream();
			var buf = Encoding.UTF8.GetBytes(body);
			os.Write(buf, 0, buf.Length);
			os.Close();
			var res = (HttpWebResponse)req.GetResponse();
			using (var reader = new StreamReader(res.GetResponseStream())) {
				return reader.ReadToEnd();
			}
		}
		void DownFile(string url, string fileName)
		{
			ServicePointManager.Expect100Continue = true;

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			var req = (HttpWebRequest)WebRequest.Create(url);
		
			req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36";
			req.Method = "GET";
		
			var res = (HttpWebResponse)req.GetResponse();
			;
			
			var os = new FileStream(fileName, FileMode.OpenOrCreate);
			res.GetResponseStream().CopyTo(os);
			os.Close();
		}
	}
}
