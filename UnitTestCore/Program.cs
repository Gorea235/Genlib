using System;
using Genlib.Strings;
using Genlib.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine(Sanitation.Sanitise("hello world <()>!", Sanitation.SanitationType.URL));
        Dictionary<string, string> dict = new Dictionary<string, string>
        {
            { "1", "one" },
            { "2", "two" },
            { "3", "three" },
            { "4", "four" }
        };
        SerializableDictionary<string, string> sdict = dict;
        string stringDict;
        XmlSerializer slz = new XmlSerializer(typeof(SerializableDictionary<string, string>));
        using (StringWriter writer = new StringWriter())
        {
            slz.Serialize(writer, sdict);
            stringDict = writer.ToString();
            Console.WriteLine(stringDict);
        }
        using (StringReader reader = new StringReader(stringDict))
        {
            SerializableDictionary<string, string> redict = (SerializableDictionary<string, string>)slz.Deserialize(reader);
            Console.WriteLine(redict);
        }

        Console.ReadLine();
    }
}