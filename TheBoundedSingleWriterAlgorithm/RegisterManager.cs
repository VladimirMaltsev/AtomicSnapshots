using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;

namespace TheBoundedSingleWriterAlgorithm
{
	class RegisterManager
	{
		private Register[] registers;
		private readonly bool[,] q_hshakes;

		public RegisterManager(Register[] regs)
		{
			this.registers = regs;
			this.q_hshakes = new bool[registers.Length, registers.Length];
		}

		public int[] Scan(int ind = 0)
		{
			//Console.WriteLine("Scan >> Task #{0}", Task.CurrentId);
			var moved = new bool[registers.Length];

			while (true)
			{
				//Console.WriteLine("     >> While >> Task #{0}", Task.CurrentId);
				for (var j = 0; j < registers.Length - 1; j++)
				{
					q_hshakes[ind, j] = registers[j].GetBitmask()[ind];
				}


				var a = CollectRegisters();
				var b = CollectRegisters();
				//               printRegs(a);
				//               printRegs(b);

				var resultOk = true;
				for (var k = 0; k < a.Length; k++)
				{
					if (a[k].GetBitmask()[ind] == b[k].GetBitmask()[ind] &&
						b[k].GetBitmask()[ind] == q_hshakes[ind, k] &&
						a[k].GetToggle() == b[k].GetToggle()) continue;
					if (moved[k])
					{
						return b[k].GetView();
					}
					moved[k] = true;

					resultOk = false;
					break;
				}

				if (!resultOk) continue;

				//Console.WriteLine("Result ok");
				var view = new int[registers.Length];
				for (var ii = 0; ii < registers.Length; ii++)
					view[ii] = a[ii].GetData();
				//Console.WriteLine(view[0]);
				return view;
			}
		}

		public void Update(int i, int value)
		{
			//Console.WriteLine("Update >> Task #{0}", Task.CurrentId);
			var newBitmask = new bool[registers.Length];
			for (var j = 0; j < registers.Length; j++)
				newBitmask[j] = !q_hshakes[j, i];
			var view = Scan(i);
			//printArr(view);
			registers[i].AtomicUpdate(value,
				newBitmask, !registers[i].GetToggle(), view);
		}

		private Register[] CollectRegisters()
		{
			return (Register[])registers.Clone();
		}

		private void printRegs(Register[] regs)
		{
			foreach (Register r in regs)
			{
				Console.Write("{0} ", r.GetData());
			}
			Console.WriteLine();
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