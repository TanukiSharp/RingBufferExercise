using System;

namespace RingBufferExercise
{
    class Program
    {
        // In interactive mode, each time you press a key, the console is cleared
        // and the next iteration is ran, then paused until you press a key again.
        private static readonly bool IsInteractive = true;

        // This determines whether to print the indexes line or not.
        // Values can be confused with indexes sometimes, so it can be useful to hide them (indexes).
        private static readonly bool ShowIndexes = true;

        static void Main()
        {
            var rb = new RingBuffer<int>(5);

            Console.WriteLine("Initial state");
            Console.WriteLine();
            Console.WriteLine(rb.ToString(ShowIndexes));

            if (IsInteractive)
                Console.ReadKey(true);

            Read(rb);

            Write(rb, 51);
            Write(rb, 52);
            Write(rb, 53);
            Write(rb, 54);
            Write(rb, 55);
            Write(rb, 56);
            Write(rb, 57);

            Read(rb);
            Read(rb);

            Write(rb, 58);
            Write(rb, 59);
            Write(rb, 60);
            Write(rb, 61);

            Read(rb);
            Read(rb);

            Write(rb, 62);

            Read(rb);
            Read(rb);
            Read(rb);
            Read(rb);
            Read(rb);

            Write(rb, 63);
            Write(rb, 64);
        }

        #region Print methods

        private static void PrintSeparatorLine()
        {
            Console.WriteLine(new string('-', 79));
        }

        private static void Read(RingBuffer<int> rb)
        {
            bool result = rb.TryRead(out int item);

            if (IsInteractive)
                Console.Clear();
            else
            {
                PrintSeparatorLine();
                Console.WriteLine();
            }

            if (result)
                Console.WriteLine($"Read: {item}");
            else
                Console.WriteLine("Read: Could not read");
            Console.WriteLine();
            Console.WriteLine(rb.ToString(ShowIndexes));

            if (IsInteractive)
                Console.ReadKey(true);
        }

        private static void Write(RingBuffer<int> rb, int value)
        {
            rb.Write(value);

            if (IsInteractive)
                Console.Clear();
            else
            {
                PrintSeparatorLine();
                Console.WriteLine();
            }

            Console.WriteLine($"Write: {value}");
            Console.WriteLine();
            Console.WriteLine(rb.ToString(ShowIndexes));

            if (IsInteractive)
                Console.ReadKey(true);
        }

        #endregion
    }
}
