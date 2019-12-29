namespace BloomFilter
{
    using System;
    using System.Text;

    // The IHashProvider implementation using a naive non-standard hashing algorithm added just for demonstration.
    // This class supports only int and string types.
    public class NaiveHashProvider<T> : IHashProvider<T>
    {
        // Function returns a pair of hashes for the input value using a naive algorithm
        // item: the item for which the hashes need to be generated
        // Returns and int array of hashes.
        public int[] GetHashPair(T item)
        {
            byte[] itemBytes;
            var hashDuo = new int[2];

            // For simple types use a direct conversion
            if (typeof(T) == typeof(int))
            {
                itemBytes = BitConverter.GetBytes((int)(object)item);

            }
            else if (typeof(T) == typeof(string))
            {
                itemBytes = Encoding.ASCII.GetBytes((string)(object)item);
            }
            else
            {
                throw new NotSupportedException();
            }

            int total1 = 0, total2 = 0;
            for (var i = 0; i < itemBytes.Length; i++)
            {
                // Adds a prime number to the current byte value, multiplies by the i and gets the total sum.
                total1 += (itemBytes[i] + 7) * i;
                total2 += (itemBytes[i] + 29) * i;
            }

            hashDuo[0] = total1;
            hashDuo[1] = total2;

            return hashDuo;
        }
    }
}
