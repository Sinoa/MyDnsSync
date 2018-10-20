namespace MyDnsSync.Components
{
    partial class MainNotifyIcon
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ContextMenuStrip contextMenuStrip;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            this.immediateSyncMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.removeAccountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            contextMenuStrip.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.immediateSyncMenuItem,
            this.removeAccountMenuItem,
            toolStripSeparator1,
            this.exitMenuItem});
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(187, 76);
            // 
            // immediateSyncMenuItem
            // 
            this.immediateSyncMenuItem.Name = "immediateSyncMenuItem";
            this.immediateSyncMenuItem.Size = new System.Drawing.Size(186, 22);
            this.immediateSyncMenuItem.Text = "直ちに同期する";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitMenuItem.Text = "終了";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = contextMenuStrip;
            this.notifyIcon.Text = "MyDnsSync";
            // 
            // removeAccountMenuItem
            // 
            this.removeAccountMenuItem.Name = "removeAccountMenuItem";
            this.removeAccountMenuItem.Size = new System.Drawing.Size(186, 22);
            this.removeAccountMenuItem.Text = "ログイン情報を削除する";
            contextMenuStrip.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem immediateSyncMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAccountMenuItem;
    }
}
