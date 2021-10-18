using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace ConsoleApp1
{
    
    class Program
    {
        static void Main(string[] args)
        {
            string inPath = "";
            string outPath = "";
            if (args.Length == 2)
            {
                inPath = args[0];
                outPath = args[1];
            }
            else if(args.Length != 0)
            {
                Console.WriteLine("Incorrect arguments.");
            }
            else
            {
                Console.Write("Input path:  ");
                inPath = Console.ReadLine();
                Console.Write("Output path: ");
                outPath = Console.ReadLine();
            }

            //UsageCounter counter = new UsageCounter(inPath, outPath);
            
            counter.ReadFile();
            if (counter.GetText().Length == 0)
            {
                Console.WriteLine("Incorrect input data.");
            }
            else
            {
                counter.ParseText();
                counter.CreateStat();
                counter.WriteFile();
            }
        }
    }
}
