namespace s3Geometry
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iGeometryCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGeometryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGeometryCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertGeometryCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setGeometriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iSegmentCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSegmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.querySegmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSegmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPointCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPointCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatePointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinateSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文档ToolStripMenuItem,
            this.iGeometryCollectionToolStripMenuItem,
            this.iSegmentCollectionToolStripMenuItem,
            this.iPointCollectionToolStripMenuItem,
            this.coordinateSystemToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(557, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文档ToolStripMenuItem
            // 
            this.文档ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载ToolStripMenuItem});
            this.文档ToolStripMenuItem.Name = "文档ToolStripMenuItem";
            this.文档ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文档ToolStripMenuItem.Text = "文档";
            // 
            // 加载ToolStripMenuItem
            // 
            this.加载ToolStripMenuItem.Name = "加载ToolStripMenuItem";
            this.加载ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.加载ToolStripMenuItem.Text = "加载";
            this.加载ToolStripMenuItem.Click += new System.EventHandler(this.加载ToolStripMenuItem_Click);
            // 
            // iGeometryCollectionToolStripMenuItem
            // 
            this.iGeometryCollectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGeometryToolStripMenuItem,
            this.addGeometryCollectionToolStripMenuItem,
            this.insertGeometryCollectionToolStripMenuItem,
            this.setGeometriesToolStripMenuItem});
            this.iGeometryCollectionToolStripMenuItem.Name = "iGeometryCollectionToolStripMenuItem";
            this.iGeometryCollectionToolStripMenuItem.Size = new System.Drawing.Size(138, 21);
            this.iGeometryCollectionToolStripMenuItem.Text = "IGeometryCollection";
            // 
            // addGeometryToolStripMenuItem
            // 
            this.addGeometryToolStripMenuItem.Name = "addGeometryToolStripMenuItem";
            this.addGeometryToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.addGeometryToolStripMenuItem.Text = "AddGeometry";
            this.addGeometryToolStripMenuItem.Click += new System.EventHandler(this.addGeometryToolStripMenuItem_Click);
            // 
            // addGeometryCollectionToolStripMenuItem
            // 
            this.addGeometryCollectionToolStripMenuItem.Name = "addGeometryCollectionToolStripMenuItem";
            this.addGeometryCollectionToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.addGeometryCollectionToolStripMenuItem.Text = "AddGeometryCollection";
            this.addGeometryCollectionToolStripMenuItem.Click += new System.EventHandler(this.addGeometryCollectionToolStripMenuItem_Click);
            // 
            // insertGeometryCollectionToolStripMenuItem
            // 
            this.insertGeometryCollectionToolStripMenuItem.Name = "insertGeometryCollectionToolStripMenuItem";
            this.insertGeometryCollectionToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.insertGeometryCollectionToolStripMenuItem.Text = "InsertGeometryCollection";
            this.insertGeometryCollectionToolStripMenuItem.Click += new System.EventHandler(this.insertGeometryCollectionToolStripMenuItem_Click);
            // 
            // setGeometriesToolStripMenuItem
            // 
            this.setGeometriesToolStripMenuItem.Name = "setGeometriesToolStripMenuItem";
            this.setGeometriesToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.setGeometriesToolStripMenuItem.Text = "SetGeometries";
            this.setGeometriesToolStripMenuItem.Click += new System.EventHandler(this.setGeometriesToolStripMenuItem_Click);
            // 
            // iSegmentCollectionToolStripMenuItem
            // 
            this.iSegmentCollectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSegmentToolStripMenuItem,
            this.querySegmentToolStripMenuItem,
            this.setSegmentToolStripMenuItem});
            this.iSegmentCollectionToolStripMenuItem.Name = "iSegmentCollectionToolStripMenuItem";
            this.iSegmentCollectionToolStripMenuItem.Size = new System.Drawing.Size(132, 21);
            this.iSegmentCollectionToolStripMenuItem.Text = "ISegmentCollection";
            // 
            // addSegmentToolStripMenuItem
            // 
            this.addSegmentToolStripMenuItem.Name = "addSegmentToolStripMenuItem";
            this.addSegmentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.addSegmentToolStripMenuItem.Text = "AddSegment";
            this.addSegmentToolStripMenuItem.Click += new System.EventHandler(this.addSegmentToolStripMenuItem_Click);
            // 
            // querySegmentToolStripMenuItem
            // 
            this.querySegmentToolStripMenuItem.Name = "querySegmentToolStripMenuItem";
            this.querySegmentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.querySegmentToolStripMenuItem.Text = "QuerySegment";
            this.querySegmentToolStripMenuItem.Click += new System.EventHandler(this.querySegmentToolStripMenuItem_Click);
            // 
            // setSegmentToolStripMenuItem
            // 
            this.setSegmentToolStripMenuItem.Name = "setSegmentToolStripMenuItem";
            this.setSegmentToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.setSegmentToolStripMenuItem.Text = "SetSegment";
            this.setSegmentToolStripMenuItem.Click += new System.EventHandler(this.setSegmentToolStripMenuItem_Click);
            // 
            // iPointCollectionToolStripMenuItem
            // 
            this.iPointCollectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPointCollectionToolStripMenuItem,
            this.queryPointsToolStripMenuItem,
            this.updatePointToolStripMenuItem});
            this.iPointCollectionToolStripMenuItem.Name = "iPointCollectionToolStripMenuItem";
            this.iPointCollectionToolStripMenuItem.Size = new System.Drawing.Size(110, 21);
            this.iPointCollectionToolStripMenuItem.Text = "IPointCollection";
            // 
            // addPointCollectionToolStripMenuItem
            // 
            this.addPointCollectionToolStripMenuItem.Name = "addPointCollectionToolStripMenuItem";
            this.addPointCollectionToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.addPointCollectionToolStripMenuItem.Text = "AddPointCollection";
            this.addPointCollectionToolStripMenuItem.Click += new System.EventHandler(this.addPointCollectionToolStripMenuItem_Click);
            // 
            // queryPointsToolStripMenuItem
            // 
            this.queryPointsToolStripMenuItem.Name = "queryPointsToolStripMenuItem";
            this.queryPointsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.queryPointsToolStripMenuItem.Text = "QueryPoints";
            this.queryPointsToolStripMenuItem.Click += new System.EventHandler(this.queryPointsToolStripMenuItem_Click);
            // 
            // updatePointToolStripMenuItem
            // 
            this.updatePointToolStripMenuItem.Name = "updatePointToolStripMenuItem";
            this.updatePointToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.updatePointToolStripMenuItem.Text = "UpdatePoint";
            this.updatePointToolStripMenuItem.Click += new System.EventHandler(this.updatePointToolStripMenuItem_Click);
            // 
            // coordinateSystemToolStripMenuItem
            // 
            this.coordinateSystemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alterToolStripMenuItem,
            this.getToolStripMenuItem,
            this.setToolStripMenuItem});
            this.coordinateSystemToolStripMenuItem.Name = "coordinateSystemToolStripMenuItem";
            this.coordinateSystemToolStripMenuItem.Size = new System.Drawing.Size(126, 21);
            this.coordinateSystemToolStripMenuItem.Text = "CoordinateSystem";
            // 
            // alterToolStripMenuItem
            // 
            this.alterToolStripMenuItem.Name = "alterToolStripMenuItem";
            this.alterToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.alterToolStripMenuItem.Text = "Alter";
            this.alterToolStripMenuItem.Click += new System.EventHandler(this.alterToolStripMenuItem_Click);
            // 
            // getToolStripMenuItem
            // 
            this.getToolStripMenuItem.Name = "getToolStripMenuItem";
            this.getToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.getToolStripMenuItem.Text = "Get";
            this.getToolStripMenuItem.Click += new System.EventHandler(this.getToolStripMenuItem_Click);
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.setToolStripMenuItem.Text = "Set";
            this.setToolStripMenuItem.Click += new System.EventHandler(this.setToolStripMenuItem_Click);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(3, 50);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(554, 297);
            this.axMapControl1.TabIndex = 2;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 23);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(557, 28);
            this.axToolbarControl1.TabIndex = 1;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(234, 273);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 347);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "s3";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem iGeometryCollectionToolStripMenuItem;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.ToolStripMenuItem addGeometryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addGeometryCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertGeometryCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setGeometriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文档ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iSegmentCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSegmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem querySegmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSegmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iPointCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPointCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updatePointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinateSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
    }
}

