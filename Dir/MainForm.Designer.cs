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
		private System.Windows.Forms.ToolStripSplitButton toolStripButton1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripSplitButton toolStripButton2;
		private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ToolStripMenuItem 收藏夹ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 保存收藏夹ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem 运行ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 代码段ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 保存代码段ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 文件列表ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripButton5;
		private System.Windows.Forms.ToolStripMenuItem 文件名排序ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 快速删除ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 删除剪切板ToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton6;
		private System.Windows.Forms.ToolStripButton toolStripButton7;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem 剪切板ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem geometryNodeToolStripMenuItem;
		
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
			this.toolStripButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.收藏夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.保存收藏夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.快速删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.删除剪切板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripSplitButton();
			this.文件列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.文件名排序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.运行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.代码段ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.保存代码段ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripSplitButton();
			this.剪切板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.geometryNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripButton1,
			this.toolStripButton2,
			this.toolStripButton3,
			this.toolStripButton4,
			this.toolStripSplitButton1,
			this.toolStripButton5,
			this.toolStripButton6,
			this.toolStripButton7});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(334, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.删除ToolStripMenuItem,
			this.收藏夹ToolStripMenuItem,
			this.保存收藏夹ToolStripMenuItem,
			this.快速删除ToolStripMenuItem,
			this.删除剪切板ToolStripMenuItem});
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(48, 22);
			this.toolStripButton1.Text = "目录";
			this.toolStripButton1.ButtonClick += new System.EventHandler(this.ToolStripButton1ButtonClick);
			// 
			// 删除ToolStripMenuItem
			// 
			this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
			this.删除ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.删除ToolStripMenuItem.Text = "删除";
			this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItemClick);
			// 
			// 收藏夹ToolStripMenuItem
			// 
			this.收藏夹ToolStripMenuItem.Name = "收藏夹ToolStripMenuItem";
			this.收藏夹ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.收藏夹ToolStripMenuItem.Text = "收藏夹";
			this.收藏夹ToolStripMenuItem.Click += new System.EventHandler(this.收藏夹ToolStripMenuItemClick);
			// 
			// 保存收藏夹ToolStripMenuItem
			// 
			this.保存收藏夹ToolStripMenuItem.Name = "保存收藏夹ToolStripMenuItem";
			this.保存收藏夹ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.保存收藏夹ToolStripMenuItem.Text = "保存收藏夹";
			this.保存收藏夹ToolStripMenuItem.Click += new System.EventHandler(this.保存收藏夹ToolStripMenuItemClick);
			// 
			// 快速删除ToolStripMenuItem
			// 
			this.快速删除ToolStripMenuItem.Name = "快速删除ToolStripMenuItem";
			this.快速删除ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.快速删除ToolStripMenuItem.Text = "快速删除";
			this.快速删除ToolStripMenuItem.Click += new System.EventHandler(this.快速删除ToolStripMenuItemClick);
			// 
			// 删除剪切板ToolStripMenuItem
			// 
			this.删除剪切板ToolStripMenuItem.Name = "删除剪切板ToolStripMenuItem";
			this.删除剪切板ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.删除剪切板ToolStripMenuItem.Text = "删除剪切板";
			this.删除剪切板ToolStripMenuItem.Click += new System.EventHandler(this.删除剪切板ToolStripMenuItemClick);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.文件列表ToolStripMenuItem,
			this.文件名排序ToolStripMenuItem});
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(48, 22);
			this.toolStripButton2.Text = "列表";
			this.toolStripButton2.ButtonClick += new System.EventHandler(this.ToolStripButton2Click);
			// 
			// 文件列表ToolStripMenuItem
			// 
			this.文件列表ToolStripMenuItem.Name = "文件列表ToolStripMenuItem";
			this.文件列表ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.文件列表ToolStripMenuItem.Text = "文件列表";
			this.文件列表ToolStripMenuItem.Click += new System.EventHandler(this.文件列表ToolStripMenuItemClick);
			// 
			// 文件名排序ToolStripMenuItem
			// 
			this.文件名排序ToolStripMenuItem.Name = "文件名排序ToolStripMenuItem";
			this.文件名排序ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.文件名排序ToolStripMenuItem.Text = "文件名排序";
			this.文件名排序ToolStripMenuItem.Click += new System.EventHandler(this.文件名排序ToolStripMenuItemClick);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(60, 22);
			this.toolStripButton3.Text = "设置目录";
			this.toolStripButton3.Click += new System.EventHandler(this.ToolStripButton3Click);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(36, 22);
			this.toolStripButton4.Text = "退后";
			this.toolStripButton4.Click += new System.EventHandler(this.ToolStripButton4Click);
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.运行ToolStripMenuItem,
			this.代码段ToolStripMenuItem,
			this.保存代码段ToolStripMenuItem,
			this.toolStripMenuItem2,
			this.toolStripMenuItem3});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.ToolStripSplitButton1ButtonClick);
			// 
			// 运行ToolStripMenuItem
			// 
			this.运行ToolStripMenuItem.Name = "运行ToolStripMenuItem";
			this.运行ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.运行ToolStripMenuItem.Text = "运行";
			this.运行ToolStripMenuItem.Click += new System.EventHandler(this.运行ToolStripMenuItemClick);
			// 
			// 代码段ToolStripMenuItem
			// 
			this.代码段ToolStripMenuItem.Name = "代码段ToolStripMenuItem";
			this.代码段ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.代码段ToolStripMenuItem.Text = "代码段";
			this.代码段ToolStripMenuItem.Click += new System.EventHandler(this.代码段ToolStripMenuItemClick);
			// 
			// 保存代码段ToolStripMenuItem
			// 
			this.保存代码段ToolStripMenuItem.Name = "保存代码段ToolStripMenuItem";
			this.保存代码段ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.保存代码段ToolStripMenuItem.Text = "保存代码段";
			this.保存代码段ToolStripMenuItem.Click += new System.EventHandler(this.保存代码段ToolStripMenuItemClick);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(136, 22);
			this.toolStripMenuItem2.Text = "3";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(136, 22);
			this.toolStripMenuItem3.Text = "4";
			this.toolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3Click);
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
			this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton6.Text = "toolStripButton6";
			this.toolStripButton6.Click += new System.EventHandler(this.ToolStripButton6Click);
			// 
			// toolStripButton7
			// 
			this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
			this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton7.Name = "toolStripButton7";
			this.toolStripButton7.Size = new System.Drawing.Size(23, 20);
			this.toolStripButton7.Text = "toolStripButton7";
			this.toolStripButton7.Click += new System.EventHandler(this.ToolStripButton7Click);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.textBox1.Location = new System.Drawing.Point(0, 25);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(334, 97);
			this.textBox1.TabIndex = 1;
			this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox1DragDrop);
			this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextBox1DragEnter);
			this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox1KeyUp);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.剪切板ToolStripMenuItem,
			this.geometryNodeToolStripMenuItem});
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
			this.剪切板ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.剪切板ToolStripMenuItem.Text = "剪切板";
			this.剪切板ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButton5Click);
			// 
			// geometryNodeToolStripMenuItem
			// 
			this.geometryNodeToolStripMenuItem.Name = "geometryNodeToolStripMenuItem";
			this.geometryNodeToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.geometryNodeToolStripMenuItem.Text = "GeometryNode";
			this.geometryNodeToolStripMenuItem.Click += new System.EventHandler(this.GeometryNodeToolStripMenuItemClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 122);
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
