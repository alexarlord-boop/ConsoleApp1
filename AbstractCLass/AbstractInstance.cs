using System;
using System.Reflection;

namespace AbstractCLass
{

    abstract class Class1
    {
        public int number;
        public string str;

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
            Console.WriteLine(t.Name);
            //var c = t.Assembly.CreateInstance(t.Name);

            ConstructorInfo ci = t.GetConstructor(new Type[] { typeof(int), typeof(string) });
            //object instance = ci.Invoke(new object[] { 10, "str"});
            var innstance = (Class1)Activator.CreateInstance(t, 1, "str");

            
            
        }
    }
}
