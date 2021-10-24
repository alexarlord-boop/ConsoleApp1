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

            string inPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\book.txt";
            string outPath = "C:\\Users\\Александр\\OneDrive\\Документы\\testFiles\\stat2.txt";


            /*-------------------------WEB SERVICE PART-------------------------*/
            //1. getting text from user <- data file        | CLIENT     |
            //2. parsing text -> dict<string, int>          | SERVICE    | WEB + MULTITHREADING
            //3. creating content string                    | CLIENT     | REFLECTION
            //4. writing file                               | CLIENT     |

            
            var client = new ServiceReference1.Service1Client();

            //1-2.
            Dictionary<string, int> resultDict;
            resultDict = client.GetData(IOUtils.ReadFile(inPath));

            //3. 
            Type t = typeof(Generator);
            MethodInfo createContent = t.GetMethod("CreateContent", BindingFlags.NonPublic | BindingFlags.Instance);
            Generator c = (Generator)Activator.CreateInstance(t);
            string result = (string)createContent.Invoke(c, new Dictionary<string, int>[] { resultDict });

            //4.
            IOUtils.WriteFile(result, outPath);

        }
    }
}
