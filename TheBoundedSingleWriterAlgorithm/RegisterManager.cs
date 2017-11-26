using System;
using System.Collections;

namespace TheBoundedSingleWriterAlgorithm
{
    class RegisterManager
    {
        private ArrayList registers;
        private bool[,] q_hshakes;

        public RegisterManager (ArrayList regs)
        {
            this.registers = regs;
            this.q_hshakes = new bool[registers.Count, registers.Count];
        }

        public int[] Scan()
        {
            var moved = new bool[registers.Count];
            
            while (true)
            {
                for (var i = 0; i < registers.Count - 1; i++)
                    for (var j = 0; j < registers.Count - 1; j ++)
                    {
                        q_hshakes[i, j] = ((Register)registers[j]).getBitmask()[i];
                        var a = CollectRegisters();
                        var b = CollectRegisters();

                        var result_ok = true;
                        for (var k = 0; k < a.Count; k ++)
                        {
                            foreach (var t in a)
                            {
                                if (((Register)a[k]).getBitmask()[i] == ((Register)b[k]).getBitmask()[i] == q_hshakes[j, i] &&
                                    ((Register)a[k]).getToggle() == ((Register)a[k]).getToggle()) continue;
                                result_ok = false;
                                break;
                            }
                            if (!result_ok)
                                break;
                        }

                        if (result_ok)
                        {
                            var view = new int[registers.Count];
                            for (var ii = 0; ii < registers.Count; ii++)
                                view[ii] = ((Register) a[ii]).getData();
                            return view;
                        }

                        for (var k = 0; k < a.Count; k++)
                        {
                            foreach (var t in a)
                            {
                                if (((Register)a[k]).getBitmask()[i] == ((Register)b[k]).getBitmask()[i] &&
                                    ((Register)b[k]).getBitmask()[i] == q_hshakes[j, i] &&
                                    ((Register)a[k]).getToggle() == ((Register)b[k]).getToggle()) continue;
                                if (moved[k])
                                    return ((Register)b[k]).getView();
                                moved[k] = true;
                            }
                        }
                    }
            }
        }

        public void update(int i, int value)
        {
            var newBitmask = new bool[registers.Count];
            for (var j = 0; j < registers.Count; j ++)
                newBitmask[j] = !q_hshakes[j, i];
            var view = Scan();
            ((Register)this.registers[i]).AtomicUpdate(value, newBitmask, 
                                                        !((Register)this.registers[i]).getToggle(), view);
        }

        private ArrayList CollectRegisters()
        {
            return (ArrayList) registers.Clone();
        }
    }

}