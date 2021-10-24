using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading.Tasks;



namespace WcfService1
{
    public class ThreadHandler
    {
        private const string Pattern = "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]";
        public string result = "";
        public Dictionary<string, int> _dict = new Dictionary<string, int>();

        static int concurrencyLevel = Environment.ProcessorCount * 2;
        static int initCapacity = 10;
        static ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>(concurrencyLevel, initCapacity);


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

}