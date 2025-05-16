/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2025/5/11
 * 时间: 17:45
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace GeometryNode
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 更新ToolStripMenuItem;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(214, 28);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ComboBox1KeyUp);
			// 
			// listBox1
			// 
			this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.listBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 17;
			this.listBox1.Location = new System.Drawing.Point(0, 28);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(214, 72);
			this.listBox1.TabIndex = 1;
			this.listBox1.DoubleClick += new System.EventHandler(this.ListBox1DoubleClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.更新ToolStripMenuItem,
			this.新建ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
			// 
			// 更新ToolStripMenuItem
			// 
			this.更新ToolStripMenuItem.Name = "更新ToolStripMenuItem";
			this.更新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.更新ToolStripMenuItem.Text = "更新";
			this.更新ToolStripMenuItem.Click += new System.EventHandler(this.更新ToolStripMenuItemClick);
			// 
			// listBox2
			// 
			this.listBox2.ContextMenuStrip = this.contextMenuStrip1;
			this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.listBox2.FormattingEnabled = true;
			this.listBox2.ItemHeight = 17;
			this.listBox2.Location = new System.Drawing.Point(0, 100);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(214, 284);
			this.listBox2.TabIndex = 2;
			this.listBox2.SelectedIndexChanged += new System.EventHandler(this.ListBox2SelectedIndexChanged);
			this.listBox2.DoubleClick += new System.EventHandler(this.ListBox2DoubleClick);
			// 
			// 新建ToolStripMenuItem
			// 
			this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
			this.新建ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.新建ToolStripMenuItem.Text = "新建";
			this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(214, 384);
			this.Controls.Add(this.listBox2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.comboBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "MainForm";
			this.Text = "GeometryNode";
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
