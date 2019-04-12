namespace p_statistics
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(168, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(97, 20);
            this.comboBox1.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(271, 33);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(97, 20);
            this.comboBox2.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(165, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "耕地等别";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(58, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(65, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "标记角度";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(128, 313);
            this.progressBar1.MarqueeAnimationSpeed = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(409, 16);
            this.progressBar1.Step = 5;
            this.progressBar1.TabIndex = 9;
            this.progressBar1.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(124, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(41, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "清除";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "<";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "90",
            "60",
            "45",
            "30",
            "15",
            "5"});
            this.comboBox3.Location = new System.Drawing.Point(12, 4);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(45, 20);
            this.comboBox3.TabIndex = 12;
            this.comboBox3.Text = "60";
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(131, 28);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(28, 292);
            this.axToolbarControl1.TabIndex = 2;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(160, 28);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(380, 301);
            this.axMapControl1.TabIndex = 1;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(-1, 28);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(130, 302);
            this.axTOCControl1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(232, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "清空图层";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 330);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "statistics";
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button button1;

    }
}

