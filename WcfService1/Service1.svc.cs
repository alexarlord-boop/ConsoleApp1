using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading.Tasks;



namespace WcfService1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.

    public class UsageCounter
    {
        private const string Pattern = "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]";
        public string result = "";
        public Dictionary<string, int> _dict = new Dictionary<string, int>();

        static int initCapacity = 400000;
        static int concurrencyLevel = Environment.ProcessorCount * 2;
        static ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>(concurrencyLevel, initCapacity);

        private string CreateStat(string Text)
        {
            string[] textLines = Regex.Split(Text, Pattern);
            string[] lineWords;

            foreach (string line in textLines)
            {
                lineWords = line.Split(' ');
                foreach (string word in lineWords)
                {
                    string cleanWord = Regex.Replace(word, "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]", string.Empty);
                    if (cleanWord.StartsWith("'") || cleanWord.EndsWith("'"))
                    {
                        cleanWord = Regex.Replace(cleanWord, "'", string.Empty);
                    }

                    cleanWord = cleanWord.ToLower();
                    if (cleanWord.Length >= 1)
                    {
                        if (this._dict.ContainsKey(cleanWord)) { this._dict[cleanWord] += 1; }
                        else { this._dict.Add(cleanWord, 1); }
                    }
                }
            }
            List<string> lstOfLines = new List<string>();
            int maxLenght = 5 + (from k in this._dict.Keys orderby k.Length descending select k).FirstOrDefault().Length;
            var sortedDictByValue = from pair in this._dict orderby pair.Value descending select pair;
            foreach (KeyValuePair<string, int> pair in sortedDictByValue)
            {
                int spaceLenght = maxLenght - (pair.Key.Length + pair.Value.ToString().Length);
                lstOfLines.Add(pair.Key + new string(' ', spaceLenght) + pair.Value.ToString());
            }
            return string.Join("\n", lstOfLines);

        }

        /*-------------------------THREADING PART-------------------------*/

        public static void JobForAThread(string line)
        {
            string[] lineWords;
            lineWords = line.Split(' ');
            foreach (string word in lineWords)
            {
                string cleanWord = Regex.Replace(word, "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]", string.Empty);
                if (cleanWord.StartsWith("'") || cleanWord.EndsWith("'"))
                {
                    cleanWord = Regex.Replace(cleanWord, "'", string.Empty);
                }

                cleanWord = cleanWord.ToLower();
                if (cleanWord.Length >= 1)
                {
                    cd.AddOrUpdate(cleanWord, 1, (key, oldValue) => oldValue + 1);
                }

            }
        }

        public Dictionary<string, int> ThreadCreateStat(string Text)
        {
            string[] textLines = Regex.Split(Text, Pattern);
            cd.Clear();

            var res = Parallel.ForEach(textLines, JobForAThread);

            return cd.ToDictionary(e => e.Key, e => e.Value);
        }
    }

    public class Service1 : IService1
    {
        public Dictionary<string, int> GetData(string text)
        {
            UsageCounter counter = new UsageCounter();
            return counter.ThreadCreateStat(text);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
