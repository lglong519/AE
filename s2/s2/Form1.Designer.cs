namespace s2
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.地图加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载地图文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载特定地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存地图文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存地图文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图层管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加SHP文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图形绘制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.线条ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.矩形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圆形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.空间查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.点选查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.矩形框选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图形查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.多边形查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.名称查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axMapControl2 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(501, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "查询名称：";
            this.label1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(561, 64);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Visible = false;
            this.textBox1.WordWrap = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.地图加载ToolStripMenuItem,
            this.图层管理ToolStripMenuItem,
            this.图形绘制ToolStripMenuItem,
            this.空间查询ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 25);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 地图加载ToolStripMenuItem
            // 
            this.地图加载ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载地图文档ToolStripMenuItem,
            this.加载特定地图ToolStripMenuItem,
            this.保存地图文档ToolStripMenuItem,
            this.另存地图文档ToolStripMenuItem});
            this.地图加载ToolStripMenuItem.Name = "地图加载ToolStripMenuItem";
            this.地图加载ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.地图加载ToolStripMenuItem.Text = "地图加载";
            // 
            // 加载地图文档ToolStripMenuItem
            // 
            this.加载地图文档ToolStripMenuItem.Name = "加载地图文档ToolStripMenuItem";
            this.加载地图文档ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.加载地图文档ToolStripMenuItem.Text = "加载地图文档";
            this.加载地图文档ToolStripMenuItem.Click += new System.EventHandler(this.加载地图文档ToolStripMenuItem_Click);
            // 
            // 加载特定地图ToolStripMenuItem
            // 
            this.加载特定地图ToolStripMenuItem.Name = "加载特定地图ToolStripMenuItem";
            this.加载特定地图ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.加载特定地图ToolStripMenuItem.Text = "加载特定地图";
            this.加载特定地图ToolStripMenuItem.Click += new System.EventHandler(this.加载特定地图ToolStripMenuItem_Click);
            // 
            // 保存地图文档ToolStripMenuItem
            // 
            this.保存地图文档ToolStripMenuItem.Name = "保存地图文档ToolStripMenuItem";
            this.保存地图文档ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.保存地图文档ToolStripMenuItem.Text = "保存地图文档";
            this.保存地图文档ToolStripMenuItem.Click += new System.EventHandler(this.保存地图文档ToolStripMenuItem_Click);
            // 
            // 另存地图文档ToolStripMenuItem
            // 
            this.另存地图文档ToolStripMenuItem.Name = "另存地图文档ToolStripMenuItem";
            this.另存地图文档ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.另存地图文档ToolStripMenuItem.Text = "另存地图文档";
            this.另存地图文档ToolStripMenuItem.Click += new System.EventHandler(this.另存地图文档ToolStripMenuItem_Click);
            // 
            // 图层管理ToolStripMenuItem
            // 
            this.图层管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加SHP文件ToolStripMenuItem,
            this.添加图层ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.移动ToolStripMenuItem});
            this.图层管理ToolStripMenuItem.Name = "图层管理ToolStripMenuItem";
            this.图层管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.图层管理ToolStripMenuItem.Text = "图层管理";
            // 
            // 添加SHP文件ToolStripMenuItem
            // 
            this.添加SHP文件ToolStripMenuItem.Name = "添加SHP文件ToolStripMenuItem";
            this.添加SHP文件ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加SHP文件ToolStripMenuItem.Text = "添加SHP";
            this.添加SHP文件ToolStripMenuItem.Click += new System.EventHandler(this.添加SHP文件ToolStripMenuItem_Click);
            // 
            // 添加图层ToolStripMenuItem
            // 
            this.添加图层ToolStripMenuItem.Name = "添加图层ToolStripMenuItem";
            this.添加图层ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.添加图层ToolStripMenuItem.Text = "添加图层";
            this.添加图层ToolStripMenuItem.Click += new System.EventHandler(this.添加图层ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除ToolStripMenuItem.Text = "删除图层";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 移动ToolStripMenuItem
            // 
            this.移动ToolStripMenuItem.Name = "移动ToolStripMenuItem";
            this.移动ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.移动ToolStripMenuItem.Text = "移动图层";
            this.移动ToolStripMenuItem.Click += new System.EventHandler(this.移动ToolStripMenuItem_Click);
            // 
            // 图形绘制ToolStripMenuItem
            // 
            this.图形绘制ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.线条ToolStripMenuItem,
            this.矩形ToolStripMenuItem,
            this.文本ToolStripMenuItem,
            this.圆形ToolStripMenuItem});
            this.图形绘制ToolStripMenuItem.Name = "图形绘制ToolStripMenuItem";
            this.图形绘制ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.图形绘制ToolStripMenuItem.Text = "图形绘制";
            // 
            // 线条ToolStripMenuItem
            // 
            this.线条ToolStripMenuItem.Name = "线条ToolStripMenuItem";
            this.线条ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.线条ToolStripMenuItem.Text = "线条";
            this.线条ToolStripMenuItem.Click += new System.EventHandler(this.线条ToolStripMenuItem_Click);
            // 
            // 矩形ToolStripMenuItem
            // 
            this.矩形ToolStripMenuItem.Name = "矩形ToolStripMenuItem";
            this.矩形ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.矩形ToolStripMenuItem.Text = "矩形";
            this.矩形ToolStripMenuItem.Click += new System.EventHandler(this.矩形ToolStripMenuItem_Click);
            // 
            // 文本ToolStripMenuItem
            // 
            this.文本ToolStripMenuItem.Name = "文本ToolStripMenuItem";
            this.文本ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.文本ToolStripMenuItem.Text = "文本";
            this.文本ToolStripMenuItem.Click += new System.EventHandler(this.文本ToolStripMenuItem_Click);
            // 
            // 圆形ToolStripMenuItem
            // 
            this.圆形ToolStripMenuItem.Name = "圆形ToolStripMenuItem";
            this.圆形ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.圆形ToolStripMenuItem.Text = "圆形";
            this.圆形ToolStripMenuItem.Click += new System.EventHandler(this.圆形ToolStripMenuItem_Click);
            // 
            // 空间查询ToolStripMenuItem
            // 
            this.空间查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.点选查询ToolStripMenuItem,
            this.矩形框选ToolStripMenuItem,
            this.图形查询ToolStripMenuItem,
            this.多边形查询ToolStripMenuItem,
            this.名称查询ToolStripMenuItem,
            this.清除选择ToolStripMenuItem});
            this.空间查询ToolStripMenuItem.Name = "空间查询ToolStripMenuItem";
            this.空间查询ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.空间查询ToolStripMenuItem.Text = "空间查询";
            // 
            // 点选查询ToolStripMenuItem
            // 
            this.点选查询ToolStripMenuItem.Name = "点选查询ToolStripMenuItem";
            this.点选查询ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.点选查询ToolStripMenuItem.Text = "点选查询";
            // 
            // 矩形框选ToolStripMenuItem
            // 
            this.矩形框选ToolStripMenuItem.Name = "矩形框选ToolStripMenuItem";
            this.矩形框选ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.矩形框选ToolStripMenuItem.Text = "矩形框选";
            // 
            // 图形查询ToolStripMenuItem
            // 
            this.图形查询ToolStripMenuItem.Name = "图形查询ToolStripMenuItem";
            this.图形查询ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.图形查询ToolStripMenuItem.Text = "图形查询";
            // 
            // 多边形查询ToolStripMenuItem
            // 
            this.多边形查询ToolStripMenuItem.Name = "多边形查询ToolStripMenuItem";
            this.多边形查询ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.多边形查询ToolStripMenuItem.Text = "多边形查询";
            // 
            // 名称查询ToolStripMenuItem
            // 
            this.名称查询ToolStripMenuItem.Name = "名称查询ToolStripMenuItem";
            this.名称查询ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.名称查询ToolStripMenuItem.Text = "名称查询";
            this.名称查询ToolStripMenuItem.Click += new System.EventHandler(this.名称查询ToolStripMenuItem_Click);
            // 
            // 清除选择ToolStripMenuItem
            // 
            this.清除选择ToolStripMenuItem.Name = "清除选择ToolStripMenuItem";
            this.清除选择ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.清除选择ToolStripMenuItem.Text = "清除选择";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(624, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 28);
            this.button1.TabIndex = 7;
            this.button1.Text = "add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Location = new System.Drawing.Point(3, 338);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(76, 73);
            this.axPageLayoutControl1.TabIndex = 6;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(3, 28);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(681, 28);
            this.axToolbarControl1.TabIndex = 2;
            // 
            // axMapControl2
            // 
            this.axMapControl2.Location = new System.Drawing.Point(477, 307);
            this.axMapControl2.Name = "axMapControl2";
            this.axMapControl2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl2.OcxState")));
            this.axMapControl2.Size = new System.Drawing.Size(207, 104);
            this.axMapControl2.TabIndex = 1;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(4, 62);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(680, 349);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnAfterScreenDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterScreenDrawEventHandler(this.axMapControl1_OnAfterScreenDraw);
            this.axMapControl1.OnExtentUpdated += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(this.axMapControl1_OnExtentUpdated);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 410);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.axPageLayoutControl1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axMapControl2);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "s2";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl2;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 地图加载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载地图文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载特定地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存地图文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存地图文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图层管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加SHP文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加图层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图形绘制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 线条ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 矩形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 圆形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 空间查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 点选查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 矩形框选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图形查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 多边形查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 名称查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除选择ToolStripMenuItem;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
        private System.Windows.Forms.Button button1;
    }
}

