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
                Console.WriteLine("Syntax: AlphanumericEnumerationGenerator lowerbound upperbound");
                Console.WriteLine("        Both lowerbound and upperbound must contains only letters (case sensitive) and digits.");
                return;
            }

            Console.WriteLine("Alphanumeric Enumeration Generator is starting...");

            var sequencer = new AlphanumericSequencer(args[0], args[1]);

            // Generate the enumeration
            var sequence = sequencer.GetSequence();

            foreach (var item in sequence)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Ended alphanumeric enumeration generation.");
        }
    }
}
