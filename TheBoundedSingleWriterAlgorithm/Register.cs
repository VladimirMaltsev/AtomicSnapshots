using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace TheBoundedSingleWriterAlgorithm
{

	internal class Register
	{
		static Object updLock = new Object();

		private int id;
		private int data;
		private bool[] bitmask;
		private bool toggle;
		private int[] view;

		public Register(int data, int id, int n)
		{
			this.id = id;
			this.data = data;
			bitmask = new bool[n];
			view = new int[n];
		}

		public bool[] GetBitmask()
		{
			return bitmask;
		}

		public bool GetToggle()
		{
			return toggle;
		}

		public int GetData()
		{
			return data;
		}

		public int[] GetView()
		{
			return view;
		}

		public void AtomicUpdate(int newData, bool[] newBitmask, bool newToggle, int[] snapshot)
		{

			lock (updLock)
			{
				this.bitmask = newBitmask;
				this.toggle = newToggle;
				this.view = snapshot;
				//Console.WriteLine("Task #{0} updating his register #{1}", Task.CurrentId, id);
				
				this.data = newData;
				Console.WriteLine("Task #{0} updated his register #{2} with data = {1}\n", Task.CurrentId, data, id);
			}
		}
	}
}