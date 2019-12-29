namespace BloomFilter
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    using Murmur;

    // The IHashProvider implementation using Murmur3 hashing algorithm.
    public class MurmurHashProvider<T> : IHashProvider<T>
    {
        // Keeps an instance of Murmur hashing 3rd party implementation
        private Murmur32 murmur;

        // Constructor
        public MurmurHashProvider()
        {
            // Crete an instance of the Murmur hashing 3rd party implementation 
            this.murmur = MurmurHash.Create32();
        }

        // Function returns a pair of hashes for the input value using Murmur3 algorithm
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
                // For everything else use a memory stream
                var obj = (object)item;
                if (obj == null)
                {
                    throw new ArgumentNullException();
                }

                var bf = new BinaryFormatter();
                var ms = new MemoryStream();
                bf.Serialize(ms, obj);

                itemBytes = ms.ToArray();
            }

            // Generate a first hash and then use it to get another one
            var hash1 = this.murmur.ComputeHash(itemBytes);
            var hash2 = this.murmur.ComputeHash(hash1);

            // Get int representations
            hashDuo[0] = BitConverter.ToInt32(hash1, 0);
            hashDuo[1] = BitConverter.ToInt32(hash2, 0);

            return hashDuo;
        }
    }
}
