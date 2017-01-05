using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Genlib.Logging;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Genlib;
using System.Threading;
using Genlib.Utilities;
using Genlib.Strings;
using Genlib.Cryptography;

namespace UnitTest
{
    sealed class ChangeType : EnhancedEnum<ChangeType, string>
    {
        public static readonly ChangeType New = new ChangeType("new");
        public static readonly ChangeType Update = new ChangeType("update");
        public static readonly ChangeType Delete = new ChangeType("delete");
        public static readonly ChangeType Failed = new ChangeType("failed");

        private ChangeType(string value) : base(value) { }
    }
    sealed class LolType : EnhancedEnum<LolType, string>
    {
        public static readonly LolType New = new LolType("lolnew");
        public static readonly LolType Update = new LolType("lolupdate");
        public static readonly LolType Delete = new LolType("loldelete");
        public static readonly LolType Failed = new LolType("lolfailed");

        private LolType(string value) : base(value) { }
    }
    public class EventedDictionary : Dictionary<string, string>
    {
        public event EventHandler<UpdatedPropertyEventArgs<KeyValuePair<string, string>?>> Changed;

        public new string this[string key]
        {
            get { return base[key]; }
            set
            {
                UpdatedPropertyEventArgs<KeyValuePair<string, string>?> e =
                    new UpdatedPropertyEventArgs<KeyValuePair<string, string>?>(null, new KeyValuePair<string, string>(key, value));
                base[key] = value;
                Changed?.Invoke(this, e);
            }
        }

        public new void Add(string key, string value)
        {
            base.Add(key, value);
            Changed?.Invoke(this, new UpdatedPropertyEventArgs<KeyValuePair<string, string>?>(null, new KeyValuePair<string, string>(key + "add", value)));
        }
    }
    public enum TestEnum
    {
        lol = 1,
        bla = 3,
        foo = 5,
        [System.ComponentModel.Description("another one")]
        another = 7,
        [System.ComponentModel.Description("too far m8")]
        one = 9
    }

    class Program
    {
        static byte[] key; //= { 23, 54, 234, 54, 87, 72, 43, 164, 24, 65, 43 };
        static byte[] iv;

        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            WebRequest req = WebRequest.Create("https://localhost:25567");
            req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequired;
            req.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Console.WriteLine("Auth: {0}, Code: {1}, Data: {2}", resp.IsMutuallyAuthenticated, resp.StatusCode, new StreamReader(resp.GetResponseStream()).ReadToEnd());
            }
            catch (WebException ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yo yo yo its HH:mm:ss {0}"));
                Console.WriteLine("Log [{0:yyyy-MM-dd}] [{0:HH:mm:ss}] [Level: {1}] ", DateTime.Now, "the best level");
            }
            catch (Exception ex)
            {
                Console.WriteLine("fuck {0}", ex.Message);
            }

            FileLogger logger = new FileLogger(Path.Combine(Directory.GetCurrentDirectory(), "test.log"));
            logger.Prefix = "Test log [{0:yyyy-MM-dd}] [{0:HH:mm:ss}] [level: {1}] ";
            logger.AutoFlush = true;
            logger.WriteInfo("lol");
            logger.Switch.Level = TraceLevel.Error;
            logger.Switch.TraceLevelStrings[TraceLevel.Error] = "fail";
            logger.WriteError("test err");
            WindowLogger winlog = new WindowLogger();
            winlog.DoEventsOnFlush = true;
            winlog.AutoFlush = true;
            winlog.Prefix = "[{1}] windows omg : ";
            winlog.Switch.Level = TraceLevel.Info;
            winlog.Height = 500;
            DebugLogger dlog = new DebugLogger();
            dlog.AutoFlush = true;
            dlog.Prefix = "[{1}] lol it's the debug: ";
            dlog.Switch.TraceLevelStrings[TraceLevel.Error] = "fail";
            LoggerWrapper logwrap = new LoggerWrapper(new KeyValuePair<string, Logger>("file", logger), new KeyValuePair<string, Logger>("window", winlog),
                new KeyValuePair<string, Logger>("debug", dlog));
            logwrap.Switch.Level = TraceLevel.Info;
            logwrap.PrefixEnabled = false;
            logwrap.AutoFlush = true;
            for (int i = 0; i < 2; i++)
                logwrap.WriteInfo("log number {0}", i);
            logwrap.Prefix = "[{1}] das log: ";
            logwrap.PrefixEnabled = true;
            for (int i = 0; i < 2; i++)
                logwrap.WriteInfo("logging-a {0} lol", i);
            logwrap.Switch.Level = TraceLevel.Warning;
            for (int i = 0; i < 2; i++)
                logwrap.WriteInfo("logging-b {0} lol", i);
            logwrap.Flush();

            Console.WriteLine(Encoding.Unicode.GetString(new byte[] { 76, 56, 78 }));
            string teststr = "i'm a cool testing <string>. I do lots* of %cool th`ings £££\"(money money) ^^ ~~ \\/ look ma&pa'/=annnnn";
            string teststrXML = Sanitation.Sanitise(teststr, Sanitation.SanitationType.XML);
            string teststrURL = Sanitation.Sanitise(teststr, Sanitation.SanitationType.URL);
            Console.WriteLine(teststr);
            Console.WriteLine(teststrXML);
            Console.WriteLine(Sanitation.Desanitise(teststrXML, Sanitation.SanitationType.XML));
            Console.WriteLine(teststr);
            Console.WriteLine(teststrURL);
            Console.WriteLine(Sanitation.Desanitise(teststrURL, Sanitation.SanitationType.URL));
            Console.WriteLine(Sanitation.Desanitise("https://www.draw.io/?state=%7B%22ids%22:%5B%220B4pWasivaq5IZVY3NGxBdFNSVmM%22%5D,%22action%22:%22open%22,%22userId%22:%22105112610015834738502%22%7D#G0B4pWasivaq5IZVY3NGxBdFNSVmM", Sanitation.SanitationType.URL));

            Console.WriteLine(new object[] { 23, 43, 12, 42, 76, 34, 87, "hello", 'f' }.ToArrayString());
            /*
            RijndaelManaged rManaged = new RijndaelManaged();
            rManaged.BlockSize = 256;
            rManaged.GenerateKey();
            rManaged.GenerateIV();
            key = rManaged.Key;
            iv = rManaged.IV;
            */
            using (Aes lolAes = Aes.Create())
            {
                key = lolAes.Key;
                iv = lolAes.IV;
                Console.WriteLine("key: " + key.ToArrayString());
                Console.WriteLine("key length: " + key.Length.ToString());
                Console.WriteLine("iv:  " + iv.ToArrayString());
                Console.WriteLine("iv length: " + iv.Length.ToString());
                Console.WriteLine(teststr);
                string encstr = Encryption.Encrypt(teststr, key, iv);
                Console.WriteLine(encstr);
                Console.WriteLine(Encryption.Decrypt(encstr, key, iv));
                Console.WriteLine(Hashing.Hash(teststr, Hashing.HashAlgorithm.SHA256));
            }

            string[] emails = {
                "john@example.com", "j.bob@example.com", "j.bob.blob@example.com", ".jblob@exmaple.com", "j..k@example.com",
                "as.asdfew.fdsgdd.esfds.sdf@example.com", "butts@lol.example.com", "lol.wtf@www.lol.many.lols.exmaple.com", "\"lol\"@email.com",
                "sutff\"lol\"@exmaple.com", "things.\"cool\".morethings@email.com", "cool\".things.\"are@super.cool", "more.\"random\"thing@email.mail",
                "look.\"at.\"things@mail.com", "things.\\\"are\\\".suoer@cool.com", "stuff\\iscool@mailc.om", "(comment)stuff@emial.com",
                "stuff.things(comment)@email.com", "stuff(comment)@thing.com", "things@[192.168.0.123]", "stuff@[123.123.123.123]",
                "stuff@[1232.321.123.123]", "sutff@[1233.123.123.1]", "stuff@[1231.123.1.1]", "stuff@[2131.1.1.1]", "stuff@[IPv6:1231::13b:12]",
                "stuff@[IPv6::::]", "stuff@[IPv6:2001:db8:1283:12b3]", "stuff@[IPv6:2001:db8::]", "stuff@[IPv6:2001:db8]", "stuff@[IPv6:2001:db8:1]",
                "stuff@[IPv6]", "stuff@[IPv6:1]", "stuff@[1.1.2]", "stuff@[1.2]", "stuff@[1]", "stuff@[12314.1231.1234.1234]", "stuff@[666.0.0.1]"
            };
#if DEBUG
            foreach (string email in emails)
            {
                Console.WriteLine("vs100 <{0}>={1}", email, Email.IsValidEmail_vs100(email));
                Console.WriteLine("vs110 <{0}>={1}", email, Email.IsValidEmail_vs110(email));
            }
#endif

            using (HttpClient client = new HttpClient())
            {
                var responseString = client.GetStringAsync("http://www.example.com/lol");
                try { responseString.Wait(); } catch (AggregateException) { }
                if (!responseString.IsFaulted)
                    Console.WriteLine(responseString.Result);
                else
                    Console.WriteLine(responseString.Exception.InnerException.Message);
            }

            string jstring1 = "{\"hello\":\"world\", \"this\":\"is\", \"json\": 2, \"whoops\": [2, 1, 4, \"lol\", \"stuff\"]}";
            dynamic jobj1 = JsonConvert.DeserializeObject(jstring1);
            Console.WriteLine(jobj1.hello);
            foreach (object o in jobj1.whoops)
                Console.WriteLine(o);
            dynamic jobj2 = new Dictionary<string, object>() { { "look", "at" }, { "this", "awesome" }, { "test", new List<object> { "omg", "lol", 1, 4, 3 } } };
            string jstring2 = JsonConvert.SerializeObject(jobj2);
            Console.WriteLine(jstring2);

            //Genlib.WpfEx.NumericTextBox nt = new Genlib.WpfEx.NumericTextBox();
            System.Windows.Media.Color c = new System.Windows.Media.Color() { R = 23, G = 54, B = 183, A = 0xFF };
            string[] colorattempts = {
                "#98F215", "#F398F215", "#98f2e5", "#eebbffcc", "#3", "#eebbf", "bbffcc", "fcc", "ebbffcc", "eebbffcc",
                "#eebbffcc", "#eebbffcc", "ggeeqq", "998877661", "9988776611"
            };
            foreach (string s in colorattempts)
            {
                try { Console.WriteLine("Color {0} ~= {1}", s, ColorEx.FromString(s)); }
                catch { Console.WriteLine("Color {0} failed", s); }
            }

            Console.WriteLine(Extentions.ToArrayString(emails));
            List<string> l_emails = new List<string>(emails);
            Console.WriteLine(new object[] { 23, 45, 23, new object[] { 425, 14, "lol", 'c' } }.ToArrayString());
            Console.WriteLine(l_emails.ToArrayString());

            try { throw new Exception("lol"); }
            catch (Exception ex)
            {
                logwrap.WriteException(ex);
                logwrap.ExceptionFormatter = (exx, details) => string.Format("lol fukin dweeb\nmsg: {0}", exx.Message);
                logwrap.WriteException(ex);
            }

            dynamic dict = new Dictionary<string, object>();
            dict.Add("lol", JsonConvert.SerializeObject(new Dictionary<string, object>() { { "totes", "lol" }, { "meep", "moop" } }));
            string jdict = JsonConvert.SerializeObject(dict);
            Console.WriteLine(jdict);
            Console.WriteLine(JsonConvert.DeserializeObject(jdict));

            Console.WriteLine("{0} = {1:N0}, should be {2:N0}", "2016-03-22T13:17:57+00:00",
                DateTime.Parse("2016-03-22T13:17:57+00:00").ToUnixTime(), 1458652677); // new DateTime(2016, 3, 22, 13, 17, 57, DateTimeKind.Utc)

            Console.WriteLine(ChangeType.Delete);
            Console.WriteLine(ChangeType.Failed);
            Console.WriteLine(LolType.Delete);
            Console.WriteLine(LolType.Failed);
            Console.WriteLine(ChangeType.Valid("lolfailed"));

            Dictionary<string, object> tdict = dict as Dictionary<string, object>;
            Console.WriteLine(tdict.ToArray());
            Console.WriteLine(tdict.ToArray().ToDictionary());

            EventedDictionary edict = new EventedDictionary();
            ManualResetEvent waiter = new ManualResetEvent(false);
            edict.Changed += (sender, e) => { Console.WriteLine("event: " + e.NewValue.Value); waiter.Set(); };
            edict.Add("lol", "lol");
            edict["test"] = "test";
            waiter.WaitOne();

            Console.WriteLine("ticks in 1 seconds: {0:N0}", new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc).Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks);
            Console.WriteLine("10,000,000 ticks of datetime: {0}", new DateTime(10000000));
            DateTime testdate = DateTime.Parse("2016-03-29 09:54:12");
            Console.WriteLine("date: {0}, after unix time convertion twice: {1}", testdate, DateTimeEx.FromUnixTime(testdate.ToUnixTime()));

            Console.WriteLine(TestEnum.bla.ToString());
            Console.WriteLine((TestEnum)1);
            Console.WriteLine("lol: {0}, bla: {1}, another: {2}, one: {3}", TestEnum.lol.GetDescription(), TestEnum.bla.GetDescription(), TestEnum.another.GetDescription(), TestEnum.one.GetDescription());

            Random rand = new Random();
            Console.WriteLine("random int: {0}, {1}, {2}, {3}; random double: {4}, {5}, {6}, {7}", rand.Next(), rand.Next(), rand.Next(), rand.Next(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble(), rand.NextDouble());

            Console.WriteLine(tdict.GetType().FullName);
            Console.WriteLine(typeof(List<>));

            Console.WriteLine("-- Done --");
            Console.ReadKey();
            logwrap.Dispose();
        }
    }
}
