/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2025/4/11
 * 时间: 15:15
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace Dir
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem 运行ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripButton5;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem 剪切板ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem socketToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 翻译ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
		
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.运行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripSplitButton();
			this.剪切板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.socketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.翻译ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripSplitButton1,
			this.toolStripButton5,
			this.toolStripSplitButton2});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(381, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.运行ToolStripMenuItem,
			this.toolStripMenuItem2,
			this.toolStripMenuItem3});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			// 
			// 运行ToolStripMenuItem
			// 
			this.运行ToolStripMenuItem.Name = "运行ToolStripMenuItem";
			this.运行ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.运行ToolStripMenuItem.Text = "运行";
			this.运行ToolStripMenuItem.Click += new System.EventHandler(this.运行ToolStripMenuItemClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem2.Text = "3";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
			this.toolStripMenuItem3.Text = "4";
			this.toolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3Click);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.剪切板ToolStripMenuItem,
			this.socketToolStripMenuItem,
			this.翻译ToolStripMenuItem});
			this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
			this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Size = new System.Drawing.Size(60, 22);
			this.toolStripButton5.Text = "剪切板";
			this.toolStripButton5.ButtonClick += new System.EventHandler(this.ToolStripButton5Click);
			// 
			// 剪切板ToolStripMenuItem
			// 
			this.剪切板ToolStripMenuItem.Name = "剪切板ToolStripMenuItem";
			this.剪切板ToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
			this.剪切板ToolStripMenuItem.Text = "剪切板";
			this.剪切板ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButton5Click);
			// 
			// socketToolStripMenuItem
			// 
			this.socketToolStripMenuItem.Name = "socketToolStripMenuItem";
			this.socketToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
			this.socketToolStripMenuItem.Text = "Socket";
			this.socketToolStripMenuItem.Click += new System.EventHandler(this.SocketToolStripMenuItemClick);
			// 
			// 翻译ToolStripMenuItem
			// 
			this.翻译ToolStripMenuItem.Name = "翻译ToolStripMenuItem";
			this.翻译ToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
			this.翻译ToolStripMenuItem.Text = "翻译";
			this.翻译ToolStripMenuItem.Click += new System.EventHandler(this.翻译ToolStripMenuItemClick);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.textBox1.Location = new System.Drawing.Point(0, 25);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(381, 269);
			this.textBox1.TabIndex = 1;
			this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox1KeyUp);
			// 
			// toolStripSplitButton2
			// 
			this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
			this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton2.Name = "toolStripSplitButton2";
			this.toolStripSplitButton2.Size = new System.Drawing.Size(32, 22);
			this.toolStripSplitButton2.Text = "toolStripSplitButton2";
			this.toolStripSplitButton2.ButtonClick += new System.EventHandler(this.ToolStripSplitButton2ButtonClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(381, 294);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MainForm";
			this.Text = "Dir";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
