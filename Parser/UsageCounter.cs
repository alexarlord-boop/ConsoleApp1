using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Parser
{
    public class UsageCounter
    {
        private const string Pattern = "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]";
        public string result = "";
        public Dictionary<string, int> _dict = new Dictionary<string, int>();

        static int initCapacity = 400000;
        static int concurrencyLevel = Environment.ProcessorCount * 2;
        static ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>(concurrencyLevel, initCapacity);



        private string RegularCreateStat(string Text)
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
        private string CreateContent(Dictionary<string, int> rd)
        {
            List<string> lstOfLines = new List<string>();
            int maxLenght = 5 + (from k in rd.Keys orderby k.Length descending select k).FirstOrDefault().Length;
            var sortedDictByValue = from pair in rd orderby pair.Value descending select pair;
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

        public string ThreadCreateStat(string Text)
        {
            string[] textLines = Regex.Split(Text, Pattern);
            cd.Clear();

            var res = Parallel.ForEach(textLines, JobForAThread);

            List<string> lstOfLines = new List<string>();
            int maxLenght = 5 + (from k in cd.Keys orderby k.Length descending select k).FirstOrDefault().Length;
            var sortedDictByValue = from pair in cd orderby pair.Value descending select pair;
            foreach (KeyValuePair<string, int> pair in sortedDictByValue)
            {
                int spaceLenght = maxLenght - (pair.Key.Length + pair.Value.ToString().Length);
                lstOfLines.Add(pair.Key + new string(' ', spaceLenght) + pair.Value.ToString());
            }
            return string.Join("\n", lstOfLines);
        }
    }
}
