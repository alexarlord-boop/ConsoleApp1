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
            this.number = 1;
            this.str = "abx a";
        }
    }

    class AbstractInstance
    {
        static void Main(string[] args)
        {
            Type t = typeof(Class1);

            var inst = (Class1)Activator.CreateInstance(t, BindingFlags.SuppressChangeType | BindingFlags.DoNotWrapExceptions | BindingFlags.CreateInstance, null);
            

                

            /*Class1 instance = new Class1() {};*/

            

            
            
        }
    }
}
