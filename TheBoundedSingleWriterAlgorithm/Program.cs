using System.Collections;

namespace TheBoundedSingleWriterAlgorithm
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const int n = 2;
            var registers = new ArrayList();

            for (var i = 0; i < n; i ++)
            {
                var r = new Register(0, n);
            }
        }
    }
}
