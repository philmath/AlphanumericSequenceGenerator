using System;

namespace AlphanumericSequenceGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check command syntax
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong number of arguments.");
                Console.WriteLine("Syntax: AlphanumericSequenceGenerator lower_bound upper_bound");
                return;
            }

            // Generate the sequence
            var sequence = AlphanumericSequencer.GetSequence(args[0], args[1]);

            foreach (var item in sequence)
            {
                Console.WriteLine(item);
            }
        }
    }
}
