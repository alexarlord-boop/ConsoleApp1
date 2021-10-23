using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;


namespace ConsoleApp1
{
    class Program
    {
        static string CreateContent(Dictionary<string, int> rd)
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
        static void Main()
        {

            string inPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\test.txt";
            string outPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\stat2.txt";

            /*-------------------------WEB SERVICE PART-------------------------*/
            //1. getting text from user <- data file        | CLIENT
            //2. parsing text -> dict<string, int>          | SERVICE    парсит текст и возвращает словарь
            //3. getting result                             | CLIENT     из словаря создает строку
            //4. writing file                               | CLIENT     
            
            var client = new ServiceReference1.Service1Client();

            //1-2.
            Dictionary<string, int> resultDict;
            resultDict = client.GetData(IOUtils.ReadFile(inPath));

            //3.
            string result = CreateContent(resultDict);

            //4.
            IOUtils.WriteFile(result, outPath);

        }
    }
}
