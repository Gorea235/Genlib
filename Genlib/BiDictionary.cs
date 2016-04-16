using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genlib
{
    /// <summary>
    /// A 2-way dictionary, naming scheme as if links between 2 single-field tables. Cannot remove values.
    /// </summary>
    /// <typeparam name="TLeft">The variable type on the left.</typeparam>
    /// <typeparam name="TRight">The variable type on the right</typeparam>
    public class BiDictionary<TLeft, TRight>
    {
        private Dictionary<TLeft, List<TRight>> LeftDictionary;
        private Dictionary<TRight, List<TLeft>> RightDictionary;

        public BiDictionary()
        {
            LeftDictionary = new Dictionary<TLeft, List<TRight>>();
            RightDictionary = new Dictionary<TRight, List<TLeft>>();
        }

        public BiDictionary(BiDictionary<TLeft, TRight> biDictionary)
        {
            LeftDictionary = new Dictionary<TLeft, List<TRight>>(biDictionary.LeftDictionary);
            RightDictionary = new Dictionary<TRight, List<TLeft>>(biDictionary.RightDictionary);
        }

        public BiDictionary(Dictionary<TLeft, List<TRight>> leftDictionary, Dictionary<TRight, List<TLeft>> rightDictionary)
        {
            LeftDictionary = leftDictionary;
            RightDictionary = rightDictionary;
        }

        public void Add(TLeft left, TRight right)
        {
            List<TRight> toRight;
            if (!LeftDictionary.TryGetValue(left, out toRight))
            {
                toRight = new List<TRight>();
                LeftDictionary[left] = toRight;
            }
            List<TLeft> toLeft;
            if (!RightDictionary.TryGetValue(right, out toLeft))
            {
                toLeft = new List<TLeft>();
                RightDictionary[right] = toLeft;
            }
            toLeft.Add(left);
            toRight.Add(right);
        }

        public List<TRight> this[TLeft i] { get { return GetLeft(i); } }
        public List<TLeft> this[TRight i] { get { return GetRight(i); } }

        public List<TRight> GetLeft(TLeft left)
        {
            List<TRight> toRight;
            if (!LeftDictionary.TryGetValue(left, out toRight))
                return new List<TRight>();
            return toRight;
        }

        public List<TLeft> GetRight(TRight right)
        {
            List<TLeft> toLeft;
            if (!RightDictionary.TryGetValue(right, out toLeft))
                return new List<TLeft>();
            return toLeft;
        }

        //public void Remove(TLeft left, TRight right)
        //{
        //    List<TRight> toRight;
        //    if (!LeftDictionary.TryGetValue(left, out toRight))
        //        throw new IndexOutOfRangeException();
        //    List<TLeft> toLeft;
        //    if (!RightDictionary.TryGetValue(right, out toLeft))
        //        throw new IndexOutOfRangeException();

        //}
    }
}
