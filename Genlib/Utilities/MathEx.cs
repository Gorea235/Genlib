using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib.Utilities
{
    /// <summary>
    /// Includes extra functions for dealing with numbers.
    /// </summary>
    public class MathEx
    {
        /// <summary>
        /// Ensures that a value is within the given range, if not then it is resized to fit.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum the value can be (inclusive).</param>
        /// <param name="max">The maximum the value can be (inclusive).</param>
        /// <returns>Clamped value.</returns>
        public static decimal Clamp(decimal value, decimal min, decimal max) => (value < min) ? min : ((value > max) ? max : value);

        /// <summary>
        /// Performs the modulus (i.e. %) of the numbers in a more cyclic way, so that minus numbers
        /// will count down from the mod rather than produce minus numbers.
        /// </summary>
        /// <param name="left">The left-hand operand.</param>
        /// <param name="right">The right-hand operand.</param>
        /// <returns>The modulus of the left number by the right number.</returns>
        public static int Mod(int left, int right)
        {
            int r = left % right;
            return r < 0 ? r + right : r;
        }
    }
}
