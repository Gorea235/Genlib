namespace Genlib.Logging
{
    partial class WindowLoggerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RtbxLog = new System.Windows.Forms.RichTextBox();
            this.TxtCmd = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RtbxLog
            // 
            this.RtbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RtbxLog.Location = new System.Drawing.Point(0, 0);
            this.RtbxLog.Margin = new System.Windows.Forms.Padding(0);
            this.RtbxLog.Name = "RtbxLog";
            this.RtbxLog.ReadOnly = true;
            this.RtbxLog.Size = new System.Drawing.Size(729, 531);
            this.RtbxLog.TabIndex = 0;
            this.RtbxLog.Text = "";
            // 
            // TxtCmd
            // 
            this.TxtCmd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TxtCmd.Location = new System.Drawing.Point(0, 531);
            this.TxtCmd.Name = "TxtCmd";
            this.TxtCmd.Size = new System.Drawing.Size(729, 26);
            this.TxtCmd.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RtbxLog);
            this.panel1.Controls.Add(this.TxtCmd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(729, 557);
            this.panel1.TabIndex = 2;
            // 
            // WindowLoggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 557);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WindowLoggerForm";
            this.Text = "WindowLoggerForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RichTextBox RtbxLog;
        internal System.Windows.Forms.TextBox TxtCmd;
        private System.Windows.Forms.Panel panel1;
    }
}