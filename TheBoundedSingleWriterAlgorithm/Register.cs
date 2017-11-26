using System;

namespace TheBoundedSingleWriterAlgorithm
{
    internal class Register
    {
        int data;
        bool[] bitmask;
        bool toggle;
        int[] view;

        public Register() { }

        public Register(int data, int n) 
        {
            this.data = data;
            bitmask = new bool[n];
            view = new int[n];
        }

        public void AtomicUpdate(int newData, bool[] newBitmask, bool newToggle, int[] snapshot)
        {
            Object updLock = new Object();
            lock (updLock)
            {
                this.bitmask = newBitmask;
                this.toggle = newToggle;
                this.view = snapshot;
                this.data = newData;
            }
        }
    }
}