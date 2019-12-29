namespace BloomFilter
{
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            // Tests the Bloom filter implementation
            Console.WriteLine("\nDo you want to test with numbers or text? press any key for numbers, T for text.");
            var c = Console.ReadKey();
            Console.WriteLine();
            if (c.Key == ConsoleKey.T)
            {
                TestStringInput();
            }
            else
            {
                TestIntegerInput();
            }
        }

        static void TestIntegerInput()
        {
            int[] intInput = { 37, 23, 108, 59, 86, 64, 94, 14, 105, 17, 111, 65, 55,
                31, 79, 97, 78, 25, 50, 22, 66, 46, 104, 98, 81, 90, 68, 40, 103,
                77, 74, 18, 69, 82, 41, 4, 48, 83, 67, 6, 2, 95, 54, 100, 99, 84,
                34, 88, 27, 72, 32, 62, 9, 56, 109, 115, 33, 15, 91, 29, 85, 114,
                112, 20, 26, 30, 93, 96, 87, 42, 38, 60, 7, 73, 35, 12, 10, 57, 80,
                13, 52, 44, 16, 70, 8, 39, 107, 106, 63, 24, 92, 45, 75, 116, 5, 61,
                49, 101, 71, 11, 53, 43, 102, 110, 1, 58, 36, 28, 76, 47, 113, 21, 89, 51, 19, 3 };

            // Set up an integer Bloom filter
            var murmurInt = new MurmurHashProvider<int>();
            var bloomInt = new BloomFilter<int>(murmurInt);
            bloomInt.Initialize(1000, 0.03);

            foreach (int i in intInput)
            {
                bloomInt.Insert(i);
                Console.Write($"{i} ");
            }
            do
            {
                Console.WriteLine("\nEnter a number");

                string str = Console.ReadLine();

                // TODO: Possible conversion exception not handled in the test code
                int number = Convert.ToInt32(str);

                if (bloomInt.MightContain(number))
                {
                    Console.WriteLine();
                    Console.WriteLine($"\n{number} is probably present in the bloom filter.\n");

                    foreach (int i in intInput)
                    {
                        if (i == number)
                        {
                            Console.Write($"-->{i}<-- ");
                            continue;
                        }
                        Console.Write($"{i} ");
                    }
                }
                else
                {
                    Console.WriteLine($"\n{number} is not present in the bloom filter.\n");
                }
                Console.WriteLine("\nPress any key to continue or hit Esc to exit.\n");
            }
            while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        static void TestStringInput()
        {
            string longStr = $"A Bloom filter is a space-efficient probabilistic data structure conceived by " +
                    $"Burton Howard Bloom in 1970 that is used to test whether an element is a member of a set False positive matches" +
                    $" are possible but false negatives are not – in other words a query returns either possibly in set or definitely" +
                    $" not in set Elements can be added to the set but not removed though this can be addressed with the counting" +
                    $" Bloom filter variant the more items added the larger the probability of false positives Bloom proposed the technique" +
                    $" for applications where the amount of source data would require an impractically large amount of memory if conventional" +
                    $" error-free hashing techniques were applied He gave the example of a hyphenation algorithm for a dictionary of 500000" +
                    $" words out of which 90% follow simple hyphenation rules but the remaining 10% require expensive disk accesses to retrieve" +
                    $" specific hyphenation patterns With sufficient core memory an error-free hash could be used to eliminate all unnecessary " +
                    $"disk accesses on the other hand with limited core memory Bloom's technique uses a smaller hash area but still eliminates " +
                    $"most unnecessary accesses For example a hash area only 15% of the size needed by an ideal error-free hash still eliminates" +
                    $" 85% of the disk accesses";

            string[] strInput = longStr.Split();

            // String Bloom fiter
            var murmurStr = new MurmurHashProvider<string>();
            var bloomStr = new BloomFilter<string>(murmurStr);
            bloomStr.Initialize(500, 0.1);

            foreach (string str in strInput)
            {
                //For this test add an item only if it does not exist
                if (!bloomStr.MightContain(str))
                {
                    bloomStr.Insert(str);
                    Console.Write($"{str} ");
                }
            }
            Console.WriteLine();
            do
            {
                Console.WriteLine("\nEnter a word");

                string str = Console.ReadLine();

                if (bloomStr.MightContain(str))
                {                    
                    Console.WriteLine($"\n{str} is probably present in the bloom filter.\n");

                    foreach (string s in strInput)
                    {
                        if (s == str)
                        {
                            Console.Write($"-->{s}<-- ");
                            continue;
                        }
                        Console.Write($"{s} ");
                    }
                }
                else
                {
                    Console.WriteLine($"\n{str} is not present in the bloom filter.\n");
                }
                Console.WriteLine("\nPress any key to continue or hit Esc to exit.\n");
            }
            while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
