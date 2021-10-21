using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

using Parser;


namespace ConsoleApp1
{
    class Program
    {
        public static string ReadFile(string path)
        {
            try
            {
                FileStream stream = File.Open(path, FileMode.Open);
                byte[] byteArray = new byte[stream.Length];
                stream.Read(byteArray, 0, byteArray.Length);
                string text = System.Text.Encoding.Default.GetString(byteArray);
                Console.WriteLine("File reading succeed.");
                return text;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return ""; }
        }

        public static void WriteFile(string result, string path)
        {
            try
            {
                FileStream outStream = File.Open(path, FileMode.Create);
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(result);
                outStream.Write(byteArray, 0, byteArray.Length);
                Console.WriteLine("File writing succeed.");
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            string inPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\book.txt";
            string outPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\stat.txt";
            string fileData = "";
            string result = "";

            /*if (args.Length == 2)
            {
                inPath = args[0];
                outPath = args[1];
            }
            else if (args.Length != 0)
            {
                Console.WriteLine("Incorrect arguments.");
            }
            else
            {
                Console.Write("Input path:  ");
                inPath = Console.ReadLine();
                Console.Write("Output path: ");
                outPath = Console.ReadLine();
            }*/


            /*-------------------------REFLECTION PART-------------------------*/

            // reading data
            fileData = ReadFile(inPath);
            string[] arg = new string[] { fileData };
            
            if (fileData.Length == 0)
            {
                Console.WriteLine("Incorrect input data.");
            }
            else
            { 
                //creating object
                var t = typeof(UsageCounter);
                var counter = (UsageCounter)Activator.CreateInstance(t);
                MethodInfo createStat = counter.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x => x.Name == "CreateStat");

                //getting the result string via standard method
                stopwatch.Start();
                result = (string)createStat.Invoke(counter, arg);
                stopwatch.Stop();
                Console.Write("Private method stats: ");
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                //writing data
                WriteFile(result, outPath);

                stopwatch.Reset();

                //getting the result string via FOREACH thread method
                stopwatch.Start();
                result = counter.ThreadCreateStat(fileData);
                stopwatch.Stop();
                Console.Write("Thread method stats: ");
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                //writing new data
                WriteFile(result, outPath);

            }

            
           
        }
    }
}
