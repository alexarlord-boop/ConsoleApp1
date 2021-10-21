﻿using System;
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
        public  Dictionary<string, int> _dict = new Dictionary<string, int>();

        
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

        public void JobForAThread(ConcurrentDictionary<string, int> cd, List<string> textLines)
        {
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
                        cd.AddOrUpdate(cleanWord, 1, (key, oldValue) => oldValue + 1);
                    }

                }
            }
            Thread.Sleep(50);
        }

        public ConcurrentDictionary<string, int> ThreadCreateStat(string Text)
        {
            int initCapacity = 400000;
            int concurrencyLevel = Environment.ProcessorCount * 2;
            Console.WriteLine("Processor amount: " + concurrencyLevel.ToString());
            ConcurrentDictionary<string, int> cd = new ConcurrentDictionary<string, int>(concurrencyLevel, initCapacity);


            string[] textLines = Regex.Split(Text, Pattern);
            int totalLength = textLines.Length;
            int chunkLength = (int)Math.Ceiling(totalLength / (double)concurrencyLevel);
            var parts = Enumerable.Range(0, concurrencyLevel).
                Select(i => textLines.Skip(i * chunkLength).Take(chunkLength).ToList()).ToList();
            Console.WriteLine("Thread amount: " + parts.Count);


            // Parallel solution
            Action[] list = new Action[concurrencyLevel];
            for (int i = 0; i < concurrencyLevel; i++)
            {
                list.Append(() => JobForAThread(cd, parts[i]));
            }
            
            Parallel.Invoke(list);
            
            return cd;
        }

    }
}
