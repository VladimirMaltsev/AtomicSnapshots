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

        public RegisterManager (Register[] regs)
        {
            this.registers = regs;
            this.q_hshakes = new bool[registers.Length, registers.Length];
        }

        public int[] Scan(int ind)
        {
            Console.WriteLine("Scan {0}", Task.CurrentId);
            var moved = new bool[registers.Length];

            while (true)
            {
              
                for (var j = 0; j < registers.Length - 1; j++)
                {
                    q_hshakes[ind, j] = ((Register) registers[j]).getBitmask()[ind];
                }
                    
                
                var a = CollectRegisters();
                var b = CollectRegisters();
               // printRegs(a);
               // printRegs(b);

                var result_ok = true;
                for (var k = 0; k < a.Length; k++)
                {
                    if (a[k].getBitmask()[ind] == b[k].getBitmask()[ind] == q_hshakes[ind, k] &&
                        a[k].getToggle() == a[k].getToggle()) continue;
                    result_ok = false;
                    break; 
                }

                if (result_ok)
                {
                    Console.WriteLine("Result ok");
                    var view = new int[registers.Length];
                    for (var ii = 0; ii < registers.Length; ii++)
                        view[ii] =  a[ii].getData();
                    Console.WriteLine(view[0]);
                    return view;
                }

                for (var k = 0; k < a.Length; k++)
                {
                    if (a[k].getBitmask()[ind] ==  b[k].getBitmask()[ind] &&
                        b[k].getBitmask()[ind] == q_hshakes[ind, k] &&
                        a[k].getToggle() ==  b[k].getToggle()) continue;
                    if (moved[k])
                    {
                        return b[k].getView();
                    }
                    moved[k] = true;   
                    
                }
            }
        }

        public void Update(int i, int value)
        {
            Console.WriteLine("Update {0}", Task.CurrentId);
            var newBitmask = new bool[registers.Length];
            for (var j = 0; j < registers.Length; j ++)
                newBitmask[j] = !q_hshakes[j, i];
            var view = Scan(i);
            ((Register)this.registers[i]).AtomicUpdate(value, newBitmask, 
                                                        !((Register)this.registers[i]).getToggle(), view);
        }

        private Register[] CollectRegisters()
        {
            return (Register[]) registers.Clone();
        }

        private void printRegs(Register[] regs)
        {
            foreach(Register r in regs)
            {
                Console.Write("{0} ", r.getData());
            }
            Console.WriteLine();
        }
    }

}