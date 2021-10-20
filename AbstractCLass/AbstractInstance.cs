using System;
using System.Reflection;
using System.Runtime;

namespace AbstractCLass
{

    abstract class Class1
    {
        public int number;
        public string str;

        public Class1()
        {
           
        }
        public Class1(int n, string s)
        {
            this.number = n;
            this.str = s;
        }
    }

    class AbstractInstance
    {
        static void Main(string[] args)
        {
            Type t = typeof(Class1);
            
            Console.WriteLine(t.IsAbstract);

            /*Class1 instance = new Class1() {};*/

            

            
            
        }
    }
}
