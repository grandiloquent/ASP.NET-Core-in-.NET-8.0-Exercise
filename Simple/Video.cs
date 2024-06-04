using System.IO;
using System.Windows.Forms;
public class Video
{
	public static void HandleKeyDown(TextBox textBox, KeyEventArgs arg)
	{
		if (arg.KeyCode == Keys.F2) {
			
		}
		if (arg.KeyCode == Keys.F3) {
		
		}
		 
		if (arg.KeyCode == Keys.F4) {
		
		}
		if (arg.KeyCode == Keys.F5) {
			CreateSvg();
		}
		if (arg.KeyCode == Keys.F12) {
			CreateSvgDirectory();
		}
	}
	private static void CreateSvg(){
		var dir=@"C:\Users\Administrator\Desktop\svg2movie";
		dir.CreateDirectoryIfNotExists();
		var s=Clipboard.GetText();
		s=s.Replace("<svg ","<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" ");
		s=s.SubstringBefore(">")+">"+"\n<style>\n\n</style>\n"+s.SubstringAfter(">");
		var i=1;
		var f=Path.Combine(dir,i.ToString().PadLeft(3,'0')+".svg");
		while(File.Exists(f)){
			i++;
			f=Path.Combine(dir,i.ToString().PadLeft(3,'0')+".svg");
		}
		File.WriteAllText(f,s);
		
	}
	private static void CreateSvgDirectory(){
		var i=1;
		var dir=i.ToString().PadLeft(3,'0').GetDesktopPath();
		while(Directory.Exists(dir)){
			i++;
			dir=i.ToString().PadLeft(3,'0').GetDesktopPath();
		}
		Directory.CreateDirectory(dir);
	}
}