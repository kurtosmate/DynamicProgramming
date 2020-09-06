using System;
using System.Linq;

namespace DynamicProgramming
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#\t#\tHello Dynamic Programming!\t#\t#");
            RunAllDynamicProgramming();
            Console.ReadKey();
        }

        private static void RunAllDynamicProgramming()
        {
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                                                            .Where(c => c.GetInterfaces().Contains(typeof(IDynamicProgramming)))
                                                            .Reverse())
            {
                WriteHeader(type);

                IDynamicProgramming instance = (IDynamicProgramming)Activator.CreateInstance(type);
                instance.Start();

            }
        }

        static int index = 1;
        private static void WriteHeader(Type type)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{index}.\t{type.Namespace.Split('.').Last()}");
            Console.ResetColor();

            index++;
        }
    }
}
