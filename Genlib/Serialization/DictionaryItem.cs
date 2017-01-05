using System;
using System.Collections.Generic;
using System.Text;

namespace Genlib.Serialization
{
    /// <summary>
    /// Defines a dictionary item of a key and value that can be get or set.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
#if !NET_CORE
    [Serializable]
#endif
    public struct DictionaryItem<TKey, TValue>
    {
        /// <summary>
        /// Gets or sets the key of the dictionary item.
        /// </summary>
        public TKey Key { get; set; }
        /// <summary>
        /// Gets or sets the value of the dictionary item.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Initialises a new <c>DictionaryItem&lt;TKey, TValue&gt;</c> with the specified key and value.
        /// </summary>
        /// <param name="key">The key of the dictionary item.</param>
        /// <param name="value">The value of the dictionary item.</param>
        public DictionaryItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Implicitly converts <c>DictionaryItem&lt;TKey, TValue&gt;</c> to <c>KeyValuePair&lt;TKey, TValue&gt;</c>.
        /// </summary>
        /// <param name="item">The item to convert.</param>
        public static implicit operator KeyValuePair<TKey, TValue>(DictionaryItem<TKey, TValue> item)
        {
            return new KeyValuePair<TKey, TValue>(item.Key, item.Value);
        }

        /// <summary>
        /// Implicitly converts <c>KeyValuePair&lt;TKey, TValue&gt;</c> to <c>DictionaryItem&lt;TKey, TValue&gt;</c>.
        /// </summary>
        /// <param name="item">The item to convert.</param>
        public static implicit operator DictionaryItem<TKey, TValue>(KeyValuePair<TKey, TValue> item)
        {
            return new DictionaryItem<TKey, TValue>(item.Key, item.Value);
        }

        /// <summary>
        /// Converts a list of <c>DictionaryItem&lt;TKey, TValue&gt;</c> to a <c>Dictionary&lt;TKey, TValue&gt;</c>
        /// </summary>
        /// <param name="items">The list of <c>DictionaryItem&lt;TKey, TValue&gt;</c> to convert.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ConvertArrayToDictionary(DictionaryItem<TKey, TValue>[] items)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            AddArrayToDictionary(items, dict);
            return dict;
        }

        /// <summary>
        /// Converts a list of <c>DictionaryItem&lt;TKey, TValue&gt;</c> to a <c>Dictionary&lt;TKey, TValue&gt;</c>
        /// </summary>
        /// <param name="items">The list of <c>DictionaryItem&lt;TKey, TValue&gt;</c> to convert.</param>
        /// <param name="dict">The dictionary to add the list to.</param>
        /// <returns></returns>
        public static void AddArrayToDictionary(DictionaryItem<TKey, TValue>[] items, Dictionary<TKey, TValue> dict)
        {
            foreach (var item in items)
                dict.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Converts a <c>Dictionary&lt;TKey, TValue&gt;</c> to a list of <c>DictionaryItem&lt;TKey, TValue&gt;</c>
        /// </summary>
        /// <param name="dict">The <c>Dictionary&lt;TKey, TValue&gt;</c> to convert.</param>
        /// <returns></returns>
        public static DictionaryItem<TKey, TValue>[] ConvertDictionaryToArray(Dictionary<TKey, TValue> dict)
        {
            DictionaryItem<TKey, TValue>[] items = new DictionaryItem<TKey, TValue>[dict.Count]; // use this instead of list for fast processing
            int i = 0;
            foreach (var kv in dict)
            {
                items[i] = kv;
                i++;
            }
            return items;
        }
    }
}
