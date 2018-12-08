namespace PowerCreator.LiveClient.Modules.MsPlayer.Player
{
    partial class PlayerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            this.MsPlayerControl = new AxMSPLAYERLib.AxMSPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.MsPlayerControl)).BeginInit();
            this.SuspendLayout();
            // 
            // MsPlayerControl
            // 
            this.MsPlayerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MsPlayerControl.Enabled = true;
            this.MsPlayerControl.Location = new System.Drawing.Point(0, 0);
            this.MsPlayerControl.Name = "MsPlayerControl";
            this.MsPlayerControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MsPlayerControl.OcxState")));
            this.MsPlayerControl.Size = new System.Drawing.Size(150, 150);
            this.MsPlayerControl.TabIndex = 0;
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MsPlayerControl);
            this.Name = "PlayerControl";
            ((System.ComponentModel.ISupportInitialize)(this.MsPlayerControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMSPLAYERLib.AxMSPlayer MsPlayerControl;
    }
}
