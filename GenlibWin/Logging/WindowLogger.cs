using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genlib.Logging
{
    internal partial class WindowLoggerForm : Form
    {
        public WindowLoggerForm()
        {
            InitializeComponent();
            CreateHandle();
        }
    }

    /// <summary>
    /// A logger which outputs to a window
    /// </summary>
    public class WindowLogger : Logger
    {
        private WindowLoggerForm Form;
        private dynamic CheckBoxLink = null;
        private enum CheckBoxType { WinForms, WPF }
        private CheckBoxType CurrentCheckBoxType;

        /// <summary>
        /// The pixels from the top of the screen
        /// </summary>
        public int Top { get { return Form.Top; } set { Form.Top = value; } }
        /// <summary>
        /// The pixels from the left of the screen
        /// </summary>
        public int Left { get { return Form.Left; } set { Form.Left = value; } }
        /// <summary>
        /// The width of the form
        /// </summary>
        public int Width { get { return Form.Width; } set { Form.Width = value; } }
        /// <summary>
        /// The height of the form
        /// </summary>
        public int Height { get { return Form.Height; } set { Form.Height = value; } }
        private bool shown = true;
        /// <summary>
        /// Whether the form is hidden or shown
        /// </summary>
        public bool Shown { get { return shown; } set { shown = value; if (shown) Form.Show(); else Form.Hide(); } }
        /// <summary>
        /// Hides the form
        /// </summary>
        public void Hide() { Shown = false; }
        /// <summary>
        /// Shows the form
        /// </summary>
        public void Show() { Shown = true; }
        /// <summary>
        /// Whether to call Application.DoEvents on a Flush
        /// </summary>
        public bool DoEventsOnFlush { get; set; } = false;
        private bool showCommandInput = false;
        /// <summary>
        /// Whether to show the command input textbox.
        /// </summary>
        public bool ShowCommandInput { get { return showCommandInput; } set { showCommandInput = value; ShowCommandInputChanged?.Invoke(this, value); } }
        private event EventHandler<bool> ShowCommandInputChanged;

        /// <summary>
        /// Raised when a command is sent.
        /// </summary>
        public event EventHandler<string> CommandSent;

        /// <summary>
        /// Creates a new WindowLogger.
        /// </summary>
        public WindowLogger()
        {
            Form = new WindowLoggerForm();
            OnFlush += WindowLogger_OnFlush;
            Form.FormClosing += Form_FormClosing;
            Form.TxtCmd.PreviewKeyDown += TxtCmd_PreviewKeyDown;
            ShowCommandInputChanged += WindowLogger_ShowCommandInputChanged;
            ShowCommandInput = false;
            Show();
        }

        private void WindowLogger_ShowCommandInputChanged(object sender, bool e)
        {
            if (e)
                Form.TxtCmd.Dock = DockStyle.Bottom;
            else
                Form.TxtCmd.Dock = DockStyle.None;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void TxtCmd_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Form.TxtCmd.Text != "")
                    CommandSent?.Invoke(this, Form.TxtCmd.Text);
                Form.TxtCmd.Text = "";
            }
        }

        private delegate void ProcessFlushDelegate(string fullstring);
        private void WindowLogger_OnFlush(object sender, OnFlushEventArgs e) { Form.Invoke(new ProcessFlushDelegate(ProcessFlush), e.FullString); }
        private void ProcessFlush(string fullstring)
        {
            Form.RtbxLog.AppendText(fullstring);
            Form.RtbxLog.SelectionStart = Form.RtbxLog.TextLength;
            Form.RtbxLog.ScrollToCaret();
            if (DoEventsOnFlush)
                Application.DoEvents();
        }

        /// <summary>
        /// Binds a CheckBox to the form, which will allow the window to be hidden or shown automatically,
        /// if a CheckBox has already been bound, it will be unbound
        /// </summary>
        /// <param name="checkbox">The CheckBox to bind</param>
        public void BindCheckBox(CheckBox checkbox)
        {
            if (CheckBoxLink != null)
                UnbindCheckBox();
            CheckBoxLink = checkbox;
            CurrentCheckBoxType = CheckBoxType.WinForms;
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            checkbox.Disposed += Checkbox_Disposed;
        }

        /// <summary>
        /// Binds a CheckBox to the form, which will allow the window to be hidden or shown automatically,
        /// if a CheckBox has already been bound, it will be unbound
        /// </summary>
        /// <param name="checkbox">The CheckBox to bind</param>
        public void BindCheckBox(System.Windows.Controls.CheckBox checkbox)
        {
            if (CheckBoxLink != null)
                UnbindCheckBox();
            CheckBoxLink = checkbox;
            CurrentCheckBoxType = CheckBoxType.WPF;
            checkbox.Checked += Checkbox_CheckedChanged;
            checkbox.Unchecked += Checkbox_CheckedChanged;
            checkbox.Unloaded += Checkbox_Disposed;
        }

        /// <summary>
        /// Unbinds the currently bound CheckBox
        /// </summary>
        public void UnbindCheckBox()
        {
            if (CheckBoxLink == null)
                throw new NullReferenceException("No CheckBox has been bound");
            if (CurrentCheckBoxType == CheckBoxType.WinForms)
            {
                (CheckBoxLink as CheckBox).CheckedChanged += Checkbox_CheckedChanged;
                (CheckBoxLink as CheckBox).Disposed += Checkbox_Disposed;
            }
            else
            {
                (CheckBoxLink as System.Windows.Controls.CheckBox).Checked += Checkbox_CheckedChanged;
                (CheckBoxLink as System.Windows.Controls.CheckBox).Unchecked += Checkbox_CheckedChanged;
                (CheckBoxLink as System.Windows.Controls.CheckBox).Unloaded += Checkbox_Disposed;
            }
            CheckBoxLink = null;
        }

        private void Checkbox_Disposed(object sender, EventArgs e)
        {
            UnbindCheckBox();
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            bool ischecked;
            if (CurrentCheckBoxType == CheckBoxType.WinForms)
                ischecked = CheckBoxLink.Checked;
            else
                ischecked = CheckBoxLink.IsChecked;
            if (ischecked)
                Show();
            else
                Hide();
        }

        /// <summary>
        /// Closes the logger.
        /// </summary>
        public override void Close()
        {
            Flush();
            if (CheckBoxLink != null)
                UnbindCheckBox();
            Form.Close();
        }

        /// <summary>
        /// Disposes the logger.
        /// </summary>
        public override void Dispose()
        {
            Close();
            Form.Dispose();
        }
    }
}
