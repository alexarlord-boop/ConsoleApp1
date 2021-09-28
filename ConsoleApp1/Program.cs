using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace ConsoleApp1
{
    class UsageCounter
    {
        private readonly string _onPath;
        private readonly string _outPath;
        string Text = "";
        string Result = "";
        private Dictionary<string, int> _dict = new Dictionary<string, int>();
        

        public UsageCounter(string inPathArg, string outPathArg)
        {
            _onPath = inPathArg;
            _outPath = outPathArg;
        }

        
        public void ReadFile()
        {
            
            try
            {
                using FileStream stream = File.Open(this._onPath, FileMode.Open);
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);
                this.Text = System.Text.Encoding.Default.GetString(byteArray);
                Console.WriteLine("File Reading succeed.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void ParseText()
        {

            string[] txtLines = Regex.Split(this.Text, "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]");
            string[] lineWords;

            // addition to the dict
            foreach (string line in txtLines)
            {
                lineWords = line.Split(" ");
                foreach (string word in lineWords)
                {
                    string cleanWord = Regex.Replace(word, "[0-9\"\\.. %°“„…:;«»,\\r\\n!?\\-–XVI()]", string.Empty);
                    if(cleanWord.StartsWith("'") || cleanWord.EndsWith("'"))
                    {
                        cleanWord = Regex.Replace(cleanWord, "'", string.Empty);
                    }
                    
                    cleanWord = cleanWord.ToLower();

                    /*// checking the result
                    Regex rgx = new Regex("[а-яa-z]");
                    if ((cleanWord.Length > 1) & !(rgx.IsMatch(cleanWord)))
                    {
                        Console.WriteLine(line);
                    }*/

                    if (cleanWord.Length >= 1)
                    {
                        if (this._dict.ContainsKey(cleanWord))
                        {
                            this._dict[cleanWord] += 1;
                        }
                        else
                        {
                            this._dict.Add(cleanWord, 1);
                        }
                    }
                }
            }
        }
        public void CreateStat()
        {
            if (this._dict.Count() == 0){this.Result = "";}
            else
            {
                string row = "";
                int maxLenght = 5 + (from k in this._dict.Keys orderby k.Length descending select k).FirstOrDefault().Length;
                List<string> lst = new List<string>();

                var sortedDictByValue = from pair in this._dict orderby pair.Value descending select pair;
                foreach (KeyValuePair<string, int> pair in sortedDictByValue)
                {
                    int keyLenght = pair.Key.Length;
                    int valLenght = pair.Value.ToString().Length;
                    int spaceLenght = maxLenght - (keyLenght + valLenght);
                    row = pair.Key + new string(' ', spaceLenght) + pair.Value.ToString();
                    lst.Add(row);
                }
                this.Result = string.Join("\n", lst);
            }
        }

        public void WriteFile()
        {
            try
            {
                using FileStream outStream = File.Open(this._outPath, FileMode.Create);
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(this.Result);
                outStream.Write(byteArray, 0, byteArray.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            // creating UsageCounter obj
            //string inPath = "C:\\Users\\Александр\\OneDrive\\Документы\\book.txt";
            //string outPath = "C:\\Users\\Александр\\OneDrive\\Документы\\word_stat.txt";
            Console.WriteLine("Input path: ");
            string inPath = Console.ReadLine();
            Console.WriteLine("Output path: ");
            string outPath = Console.ReadLine();

            UsageCounter counter = new UsageCounter(inPath, outPath);
            
            counter.ReadFile();

            counter.ParseText();

            counter.CreateStat();
          
            counter.WriteFile();

        }
    }
}

