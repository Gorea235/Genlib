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
            this.rtbx_log = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbx_log
            // 
            this.rtbx_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbx_log.Location = new System.Drawing.Point(0, 0);
            this.rtbx_log.Name = "rtbx_log";
            this.rtbx_log.Size = new System.Drawing.Size(284, 261);
            this.rtbx_log.TabIndex = 0;
            this.rtbx_log.Text = "";
            // 
            // WindowLoggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.rtbx_log);
            this.Name = "WindowLoggerForm";
            this.Text = "WindowLoggerForm";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RichTextBox rtbx_log;
    }
}