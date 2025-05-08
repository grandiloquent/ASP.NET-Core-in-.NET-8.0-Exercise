
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dir
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : Form
	{
		string _list;
		public Form1()
		{
			
			InitializeComponent();
			_list="list.txt".GetEntryPath();
			if(File.Exists(_list)){
				listBox1.Items.AddRange(File.ReadAllLines(_list).OrderBy(x=>x).ToArray());
			}
		}
		void ListBox1MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if(listBox1.SelectedIndex!=-1){
				Clipboard.SetText(listBox1.Items[listBox1.SelectedIndex].ToString());
				}
		}
		void 新建ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var v=Clipboard.GetText();
			if(File.Exists(_list)){
				var l=	File.ReadAllLines(_list).ToList();
				l.Add(v);
				File.WriteAllLines(_list,l.Distinct());
			}else{
				File.WriteAllText(_list,v);
			}
			listBox1.Items.Clear();
				listBox1.Items.AddRange(File.ReadAllLines(_list).OrderBy(x=>x).ToArray());
			
		}
		void 删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex!=-1){
				var v=listBox1.Items[listBox1.SelectedIndex].ToString();
				var l=	File.ReadAllLines(_list).ToList();
				l.Remove(v);
				File.WriteAllLines(_list,l);
				listBox1.Items.Clear();
				listBox1.Items.AddRange(File.ReadAllLines(_list).OrderBy(x=>x).ToArray());
		
			}
		}
	}
}
