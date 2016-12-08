using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Genlib.Utilities;

namespace Genlib.WpfEx
{
    /// <summary>
    /// A TextBox that only accepts numeric values.
    /// </summary>
    public class NumericTextBox : TextBox
    {
        private static readonly Regex ValidChars = new Regex(@"[0-9\.-]");

        private decimal value = 0;
        /// <summary>
        /// The value of the NumericUpDown.
        /// </summary>
        /// <remarks>Uses decimal because the System.Windows.Forms.NumericUpDown does.</remarks>
        public decimal Value
        {
            get { return value; }
            set
            {
                UpdatedPropertyEventArgs<decimal> e = new UpdatedPropertyEventArgs<decimal>(this.value, value);
                this.value = value;
                ValueChanged?.Invoke(this, e);
                Text = Value.ToString();
            }
        }
        /// <summary>
        /// Raised whenever the property ValueChanged is changed.
        /// </summary>
        public event EventHandler<UpdatedPropertyEventArgs<decimal>> ValueChanged;

        /// <summary>
        /// The maximum value the NumericTextBox can go to.
        /// </summary>
        public decimal Max { get; set; } = 100;
        /// <summary>
        /// The minimum value the NumericTextBox can go to.
        /// </summary>
        public decimal Min { get; set; } = 0;
        /// <summary>
        /// Whether to force the user to input an integer value (is still stored in a decimal).
        /// </summary>
        public bool ForceInt { get; set; } = false;

        /// <summary>
        /// Creates a new NumericTextBox.
        /// </summary>
        public NumericTextBox() : base()
        {
            PreviewTextInput += NumericTextBox_PreviewTextInput;
            DataObject.AddPastingHandler(this, OnPaste);
            LostFocus += NumericTextBox_LostFocus;
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            decimal newVal;
            if (decimal.TryParse(Text, out newVal))
                Value = MathEx.Clamp(ForceInt ? Math.Round(newVal) : newVal, Min, Max);
            else
                Text = Value.ToString();
        }

        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(e.Text);
        }

        private static bool IsValid(string txt) => ValidChars.IsMatch(txt);

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsValid(text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }
    }
}
