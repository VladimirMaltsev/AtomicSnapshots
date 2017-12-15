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

			Task.Run(() =>
			{
				for (int i = 2; i < 12; i += 2)
				{
					Console.WriteLine("Task #{0} begins to Update() register #{1} with val = {2}...",
						Task.CurrentId, 0, i);
					rm.Update(0, i);
					Thread.Sleep(100);
				}
				
			
			});
			
			Task.Run(() =>
			{
				for (int i = 1; i < 12; i += 2)
				{
					Console.WriteLine("Task #{0} begins to Update() register #{1} with val = {2}...",
						Task.CurrentId, 1, i);
					rm.Update(1, i);
					Thread.Sleep(100);
				}
				
			
			});
			
			
			for (var i = 0; i < countTask; i++)
			{	
					tasks.Add(Task.Run(() =>
					{
						Console.WriteLine("Task #{0} begins to Scan()...", Task.CurrentId);
						var arr = rm.Scan();
						Console.WriteLine("Task #{0} Scaned :>> {{ {1} , {2} }}\n", Task.CurrentId, arr[0], arr[1]);
					}));
				Thread.Sleep(100);

			}
			Task.WaitAll(tasks.ToArray());
			
		}
	}
}
