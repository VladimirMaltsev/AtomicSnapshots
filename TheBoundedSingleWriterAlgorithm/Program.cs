using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace TheBoundedSingleWriterAlgorithm
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const int n = 2;
            const int count_task = 10;
            
            var registers = new Register[n];

            for (var i = 0; i < n; i ++)
            {
                registers[i] = new Register(0, n);
            }
            //Console.WriteLine(((Register)registers[0]).getData());
            var rm = new RegisterManager(registers);
            
            for (var i = 0; i < count_task; i++)
            {
                if (i % 2 == 0)
                {
                    //Task.Run(() => rm.Update(i / 4 % 2, i));
                }
                else
                    Task.Run(() =>
                    {
                        Console.WriteLine(rm.Scan(0));
                    });

            }
            Console.ReadKey();
        }

        private static void printArr(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write("{0} ", arr[i]);
            }
            Console.WriteLine();
        }
    }
}
