using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace TheBoundedSingleWriterAlgorithm
{
         
    internal class Register
    {
        static Object updLock = new Object();
        
        private int data;
        private bool[] bitmask;
        private bool toggle;
        private int[] view;

        public Register()
        {
        }

        public Register(int data, int n)
        {
            this.data = data;
            bitmask = new bool[n];
            view = new int[n];
        }

        public bool[] getBitmask()
        {
            return bitmask;
        }

        public bool getToggle()
        {
            return toggle;
        }

        public int getData()
        {
            return data;
        }

        public int[] getView()
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
                this.data = newData;
            }
        }
    }
}