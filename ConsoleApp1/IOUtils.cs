using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace ConsoleApp1
{
    class IOUtils
    {
        public static string ReadFile(string path)
        {
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open))
                {
                    byte[] byteArray = new byte[stream.Length];
                    stream.Read(byteArray, 0, byteArray.Length);
                    string text = System.Text.Encoding.Default.GetString(byteArray);
                    Console.WriteLine("File reading succeed.");
                    return text;
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); return ""; }
        }

        public static void WriteFile(string result, string path)
        {
            //Console.WriteLine(result);
            try
            {
                using (FileStream outStream = File.Open(path, FileMode.Create))
                {
                    byte[] byteArray = Encoding.Default.GetBytes(result);
                    outStream.Write(byteArray, 0, byteArray.Length);
                    Console.WriteLine("File writing succeed.");
                }
                
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

    }
}
