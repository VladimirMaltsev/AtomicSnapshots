using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TheBoundedSingleWriterAlgorithm
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			const int n = 2;
			const int countTask = 10;
			var registers = new Register[n];

			for (var i = 0; i < n; i++)
			{
				registers[i] = new Register(0, i, n);
			}
			
			var tasks = new List<Task>();
			var rm = new RegisterManager(registers);

			for (var i = 0; i < countTask; i++)
			{
				if (i % 2 == 0)
				{
					var i1 = i + 2;
					Task.Run(() =>
					{
						Console.WriteLine("Task #{0} begins to Update() register #{1} with val = {2}...", 
							Task.CurrentId, i1 / 4 % 2, i1);
						rm.Update(i1 / 4 % 2, i1);
					});
				}
				else
					tasks.Add(Task.Run(() =>
					{
						Console.WriteLine("Task #{0} begins to Scan()...", Task.CurrentId);
						var arr = rm.Scan();
						Console.WriteLine("Task #{0} Scaned :>> {{ {1} , {2} }}\n", Task.CurrentId, arr[0], arr[1]);
					}));

			}
			Task.WaitAll(tasks.ToArray());
			
			
			//Console.ReadKey();
		}
	}
}
