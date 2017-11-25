using System;
using System.Collections;

namespace TheBoundedSingleWriterAlgorithm
{
    class RegisterManager
    {
        private ArrayList<Register> registers;

        public RegisterManager (ArrayList<Register> regs)
        {
            this.registers = regs;
        }

        public int[] Scan()
        {
            int[ , ] q_hshakes = new int[registers.Count, registers.Count];
            bool[] moved = new bool[registers.Count];
            
            while (true)
            {
                for (i = 0; i < registers.Count - 1; i++)
                    for ( j = 0; j < registers.Count - 1; j ++)
                    {
                        q_hshakes[i, j] = registers.Item[j].bitmask[i];
                        ArryaList<Register> a = CollectRegisters();
                        ArrayList<Register> b = CollectRegisters();

                        bool result_ok = true;
                        for (int k = 0; k < a.Count; k ++)
                        {
                            for (int ind = 0; ind < a.Count; ind ++)
                            {
                                if (!(a.Item[k].bitmask[i] == b.Item[k].bitmask[i] == q_hshakes[j, i] &&
                                    a.item[k].toggle == b.item[k].toggle))
                                {
                                    result_ok = false;
                                    break;
                                }
                            }
                            if (!result_ok)
                                break;
                        }

                        if (result_ok)
                        {
                            int[] view = new view[registers.Count];
                            for (int i = 0; i < registers.Count; i++)
                                view[i] = a.Item[i].data;
                            return view;
                        }

                        for (int k = 0; k < a.Count; k++)
                        {
                            for (int ind = 0; ind < a.Count; ind++)
                            {
                                if (a.Item[k].bitmask[i] != b.Item[k].bitmask[i] ||
                                    b.Item[k].bitmask[i] != q_hshakes[j, i] ||
                                    a.item[k].toggle != b.item[k].toggle)
                                {
                                    if (moved[k])
                                        return b.Item[k].view;
                                    moded[k] = true;
                                }
                            }
                        }
                    }
            }
        }

        private ArrayList<Register> CollectRegister()
        {
            return registers.Clone();
        }
    }

}