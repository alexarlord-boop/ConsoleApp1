using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

using Parser;

namespace ConsoleApp1
{
    class Program
    {
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
            string fileData = IOUtils.ReadFile(inPath);
            resultDict = client.GetData(fileData);

            //3. using reflection
            Type t = typeof(UsageCounter);
            MethodInfo createContent = t.GetMethod("CreateContent", BindingFlags.NonPublic | BindingFlags.Instance);
            UsageCounter c = (UsageCounter)Activator.CreateInstance(t);
            Dictionary<string, int>[] paramms = new Dictionary<string, int>[] { resultDict };
            string result = (string)createContent.Invoke(c, paramms);

            //4.
            IOUtils.WriteFile(result, outPath);

        }
    }
}
