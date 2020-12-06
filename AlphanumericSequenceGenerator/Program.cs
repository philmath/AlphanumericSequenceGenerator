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

            var sequencer = new AlphanumericSequencer(args[0], args[1]);

            // Generate the sequence
            var sequence = sequencer.GetSequence();

            foreach (var item in sequence)
            {
                Console.WriteLine(item);
            }
        }
    }
}
