namespace PowerCreator.LiveClient.Desktop.WinFormControl
{
    partial class MsPlayerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsPlayerControl));
            this.MsPlayer = new AxMSPLAYERLib.AxMSPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.MsPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // MsPlayer
            // 
            this.MsPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MsPlayer.Enabled = true;
            this.MsPlayer.Location = new System.Drawing.Point(0, 0);
            this.MsPlayer.Name = "MsPlayer";
            this.MsPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MsPlayer.OcxState")));
            this.MsPlayer.Size = new System.Drawing.Size(150, 150);
            this.MsPlayer.TabIndex = 0;
            // 
            // MsPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MsPlayer);
            this.Name = "MsPlayerControl";
            ((System.ComponentModel.ISupportInitialize)(this.MsPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSPLAYERLib.AxMSPlayer MsPlayer;
    }
}
