using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Parser
{
    public class UsageCounter
    {
        private readonly string _inPath;
        private readonly string _outPath;
        private string Text = "";
        private string Result = "";
        private readonly Dictionary<string, int> _dict = new Dictionary<string, int>();

        public UsageCounter(string inPathArg, string outPathArg)
        {
            _inPath = inPathArg;
            _outPath = outPathArg;
        }

        public void ReadFile()
        {
            try
            {
                FileStream stream = File.Open(this._inPath, FileMode.Open);
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);
                this.Text = System.Text.Encoding.Default.GetString(byteArray);
                Console.WriteLine("File reading succeed.");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public string GetText()
        {
            return this.Text;
        }
        public void ParseText()
        {
            string[] textLines = Regex.Split(this.Text, "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]");
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
        }
        public void CreateStat()
        {
            List<string> lstOfLines = new List<string>();
            int maxLenght = 5 + (from k in this._dict.Keys orderby k.Length descending select k).FirstOrDefault().Length;
            var sortedDictByValue = from pair in this._dict orderby pair.Value descending select pair;
            foreach (KeyValuePair<string, int> pair in sortedDictByValue)
            {
                int spaceLenght = maxLenght - (pair.Key.Length + pair.Value.ToString().Length);
                lstOfLines.Add(pair.Key + new string(' ', spaceLenght) + pair.Value.ToString());
            }
            this.Result = string.Join("\n", lstOfLines);

        }
        public void WriteFile()
        {
            try
            {
                FileStream outStream = File.Open(this._outPath, FileMode.Create);
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(this.Result);
                outStream.Write(byteArray, 0, byteArray.Length);
                Console.WriteLine("File writing succeed.");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
