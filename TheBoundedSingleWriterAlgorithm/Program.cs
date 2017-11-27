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
			const int countTask = 10;

			var registers = new Register[n];

			for (var i = 0; i < n; i++)
			{
				registers[i] = new Register(0, n);
			}
			//Console.WriteLine(((Register)registers[0]).getData());
			var rm = new RegisterManager(registers);

			for (var i = 0; i < countTask; i++)
			{
				if (i % 2 == 0)
				{
					var i1 = i;
					Task.Run(() => rm.Update(i1 / 4 % 2, i1));
				}
				else
					Task.Run(() =>
					{
						var arr = rm.Scan();
						Console.Write("Reading... Task #{0} ", Task.CurrentId);
						printArr(arr);
					});

			}
			Task.WaitAll();
			Task.Run(() =>
			{
				var arr = rm.Scan();
				Console.Write("Reading... Task #{0} ", Task.CurrentId);
				printArr(arr);
			});
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
