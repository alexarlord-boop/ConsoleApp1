using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Parser
{
    public class Generator
    {


        /*------------------------WEB SERVICE PART------------------------*/
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
    }
}
