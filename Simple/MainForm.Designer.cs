
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 替换ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 替换内容ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
		
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.替换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.替换内容ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listBox1);
			this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox1);
			this.splitContainer1.Size = new System.Drawing.Size(605, 242);
			this.splitContainer1.SplitterDistance = 173;
			this.splitContainer1.TabIndex = 0;
			// 
			// listBox1
			// 
			this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 12;
			this.listBox1.Location = new System.Drawing.Point(0, 20);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBox1.Size = new System.Drawing.Size(173, 222);
			this.listBox1.TabIndex = 2;
			this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListBox1MouseDoubleClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.新建ToolStripMenuItem,
			this.替换ToolStripMenuItem,
			this.添加ToolStripMenuItem,
			this.toolStripSeparator1,
			this.替换内容ToolStripMenuItem,
			this.导出ToolStripMenuItem,
			this.删除ToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(125, 142);
			// 
			// 新建ToolStripMenuItem
			// 
			this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
			this.新建ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.新建ToolStripMenuItem.Text = "新建";
			this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItemClick);
			// 
			// 替换ToolStripMenuItem
			// 
			this.替换ToolStripMenuItem.Name = "替换ToolStripMenuItem";
			this.替换ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.替换ToolStripMenuItem.Text = "替换";
			this.替换ToolStripMenuItem.Click += new System.EventHandler(this.替换ToolStripMenuItemClick);
			// 
			// 添加ToolStripMenuItem
			// 
			this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
			this.添加ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.添加ToolStripMenuItem.Text = "添加";
			this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItemClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
			// 
			// 替换内容ToolStripMenuItem
			// 
			this.替换内容ToolStripMenuItem.Name = "替换内容ToolStripMenuItem";
			this.替换内容ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.替换内容ToolStripMenuItem.Text = "替换内容";
			this.替换内容ToolStripMenuItem.Click += new System.EventHandler(this.替换内容ToolStripMenuItemClick);
			// 
			// 导出ToolStripMenuItem
			// 
			this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
			this.导出ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.导出ToolStripMenuItem.Text = "导出";
			this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItemClick);
			// 
			// 删除ToolStripMenuItem
			// 
			this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
			this.删除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.删除ToolStripMenuItem.Text = "删除";
			this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItemClick);
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(0, 0);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(173, 20);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ComboBox1KeyUp);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.MaxLength = 32767000;
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(428, 242);
			this.textBox1.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(605, 242);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Text = "Simple";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			//this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
