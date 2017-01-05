using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Genlib.Serialization
{
    /// <summary>
    /// A class that enables dictionary serialization.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [XmlRoot("Dictionary")]
#if !NET_CORE
    [Serializable]
#endif
    public class SerializableDictionary<TKey, TValue>
    {
        /// <summary>
        /// Creates a new serializable dictionary.
        /// </summary>
        public SerializableDictionary() { Dictionary = new Dictionary<TKey, TValue>(); }
        /// <summary>
        /// Creates a new serializable dictionary using the given dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to load from.</param>
        public SerializableDictionary(Dictionary<TKey, TValue> dictionary) { Dictionary = dictionary; }

        /// <summary>
        /// The dictionary that is being used.
        /// </summary>
        [XmlIgnore]
        public Dictionary<TKey, TValue> Dictionary { get; set; }

        /// <summary>
        /// Converts the dictionary into an array of KeyValuePairs to
        /// enable serialzation.
        /// </summary>
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public DictionaryItem<TKey, TValue>[] DictionaryItems
        {
            get
            {
                return DictionaryItem<TKey, TValue>.ConvertDictionaryToArray(Dictionary);
            }
            set
            {
                Dictionary.Clear();
                DictionaryItem<TKey, TValue>.AddArrayToDictionary(value, Dictionary);
            }
        }

        /// <summary>
        /// Converts the serializable dictionary to a normal dictionary.
        /// </summary>
        /// <param name="serialDictionary">The serializable dictionary to convert from.</param>
        public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> serialDictionary)
        {
            return new Dictionary<TKey, TValue>(serialDictionary.Dictionary);
        }

        /// <summary>
        /// Converts the normal dictionary to a serializable dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to convert from.</param>
        public static implicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return new SerializableDictionary<TKey, TValue>(dictionary);
        }
    }
}
