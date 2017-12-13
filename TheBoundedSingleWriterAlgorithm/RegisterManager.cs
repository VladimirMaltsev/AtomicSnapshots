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
			var moved = new bool[registers.Length];

			while (true)
			{
				for (var j = 0; j < registers.Length; j++)
				{
					q_hshakes[ind, j] = registers[j].GetBitmask()[ind];
				}
				
				var a = CollectRegisters();
				var b = CollectRegisters();

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

				var view = new int[registers.Length];
				for (var ii = 0; ii < registers.Length; ii++)
					view[ii] = a[ii].GetData();
				return view;
			}
		}

		public void Update(int i, int value)
		{
			var newBitmask = new bool[registers.Length];
			for (var j = 0; j < registers.Length; j++)
				newBitmask[j] = !q_hshakes[j, i];
			var view = Scan(i);
			
			registers[i].AtomicUpdate(value,
				newBitmask, !registers[i].GetToggle(), view);
			//Console.Write("\nWrite... Task #{0} >> arr = {{ {1} , {2} }}", Task.CurrentId, view[0], arr[1]);
		}

		private Register[] CollectRegisters()
		{
			return (Register[])registers.Clone();
		}

	}



}