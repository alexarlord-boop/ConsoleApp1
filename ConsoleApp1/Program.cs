using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;


namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();

            string inPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\book.txt";
            string outPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\stat2.txt";
            string fileData = "";
            string result = "";


            /*-------------------------WEB SERVICE PART-------------------------*/
            //1. getting text from user <- console:ReadLine | CLIENT
            //2. parsing text -> dict<string, int>          | SERVICE    парсит текст и возвращает словарь
            //   getting result                             | SERVICE    из словаря создает строку
            //3. writing file                               | CLIENT     
            
            var client = new ServiceReference1.Service1Client();
            
            //3.
            result = client.GetData(Console.ReadLine());
            //Console.WriteLine(result);
            //4.
            IOUtils.WriteFile(result, outPath);

            
        }
    }
}
