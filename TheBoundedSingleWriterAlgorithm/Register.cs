using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBoundedSingleWriterAlgorithm
{
    class Register
    {
        int data;
        int[] bitmask;
        bool togle;
        int[] view;

        public Register(int data, int n) 
        {
            this.data = data;
            bitmask = new int[n];
            view = new int[n];
        }

        public void AtomicUpdate(int newData, int[] newBitmask, bool newTogle, int[] snapshot)
        {
            Object updLock = new Object();
            lock (updLock)
            {
                this.bitmask = newBitmask;
                this.togle = newTogle;
                this.view = snapshot;
                this.data = newData;
            }
        }
    }
}