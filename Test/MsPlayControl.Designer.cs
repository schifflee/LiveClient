namespace Test
{
    partial class MsPlayControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsPlayControl));
            this.axMSPlayer1 = new AxMSPLAYERLib.AxMSPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.axMSPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // axMSPlayer1
            // 
            this.axMSPlayer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axMSPlayer1.Enabled = true;
            this.axMSPlayer1.Location = new System.Drawing.Point(0, 0);
            this.axMSPlayer1.Name = "axMSPlayer1";
            this.axMSPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMSPlayer1.OcxState")));
            this.axMSPlayer1.Size = new System.Drawing.Size(414, 150);
            this.axMSPlayer1.TabIndex = 0;
            // 
            // MsPlayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axMSPlayer1);
            this.Name = "MsPlayControl";
            this.Size = new System.Drawing.Size(414, 150);
            ((System.ComponentModel.ISupportInitialize)(this.axMSPlayer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSPLAYERLib.AxMSPlayer axMSPlayer1;
    }
}
