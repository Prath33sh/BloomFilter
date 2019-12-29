// TODO: Add more negative scenarios and edge cases
namespace BloomFilter.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [System.Serializable]
    public class Foo
    {
        int i;
        string str;
        public Foo(int number, string text)
        {
            i = number;
            str = text;
        }
    }

    [TestClass()]
    public class BloomFilterTests
    {
        int[] intArray;
        IHashProvider<int> murmurInt;
        IBloomFilter<int> bloomInt;

        public BloomFilterTests()
        {
            // Arrange
            this.intArray = new[] { 1, 2, -3, 4, 5, 6, -7, 8, 9, 10 };
            this.murmurInt = new MurmurHashProvider<int>(); // TODO: This should be mocked/substituted
            this.bloomInt = new BloomFilter<int>(this.murmurInt);
            this.bloomInt.Initialize(10, 0.03);
        }

        [TestMethod()]
        public void InsertedElementReturnsPositiveResult()
        {                       
            // Act
            foreach (int i in intArray)
            {
                bloomInt.Insert(i);
            }

            // Assert
            Assert.IsTrue(bloomInt.MightContain(8));
        }

        [TestMethod()]
        public void NonExistentElementWillNotBeFound()
        {
            // Act
            foreach (int i in intArray)
            {
                bloomInt.Insert(i);
            }

            // Assert
            Assert.IsFalse(bloomInt.MightContain(3));
        }

        [TestMethod()]
        public void ForGivenInputSizeAndErrorRateReturnsFilterSize()
        {
            // Arrange
            var filter = new BloomFilter<int>(null);
            
            // Act
            var size = filter.GetOptimumFilterSize(10, 0.03);
            
            // Assert
            Assert.AreEqual(72, size);
        }

        [TestMethod()]
        public void ForGivenFilterSizeAndInputSizeReturnsHashFunctionCount()
        {
            // Arrange
            var filter = new BloomFilter<int>(null);

            // Act
            var hashCount = filter.GetOptimumHashes(72, 10);

            // Assert
            Assert.AreEqual(4, hashCount);
        }

        [TestMethod()]
        public void ForStringInputTheFilterWorksAsExpected()
        {
            // Arrange
            var stringInput = $"A Bloom filter is a space-efficient probabilistic data structure" +
                $" conceived by Burton Howard Bloom in 1970" +
                $" that is used to test whether an element is a member of a set";
            var strArray = stringInput.Split();
            IHashProvider<string> murmurStr = new MurmurHashProvider<string>(); 
            IBloomFilter<string> bloomStr = new BloomFilter<string>(murmurStr);
            bloomStr.Initialize(50, 0.02);

            // Act
            foreach (string str in strArray)
            {
                bloomStr.Insert(str);
            }

            // Assert
            Assert.IsTrue(bloomStr.MightContain("element"));
            Assert.IsTrue(bloomStr.MightContain("1970"));
            Assert.IsFalse(bloomStr.MightContain("infinite"));
            Assert.IsFalse(bloomStr.MightContain("possibility"));
        }

        [TestMethod()]
        public void SmallExpectedSizeReturnsFalsePositives()
        {
            // Arrange
            var stringInput = $"A Bloom filter is a space-efficient probabilistic data structure" +
                $" conceived by Burton Howard Bloom in 1970" +
                $" that is used to test whether an element is a member of a set";
            var strArray = stringInput.Split();
            // Use a weak hashing algorithm
            IHashProvider<string> naiveStr = new NaiveHashProvider<string>(); 
            IBloomFilter<string> bloomStr = new BloomFilter<string>(naiveStr);
            bloomStr.Initialize(10, 0.02);

            // Act
            foreach (string str in strArray)
            {
                bloomStr.Insert(str);
            }

            // Assert
            // With a smaller filter size the collisions will be more resulting in false positives
            Assert.IsTrue(bloomStr.MightContain("infinite"));
        }

        [TestMethod()]
        public void WithAnyObjectReturnsExpectedResults()
        {
            // Arrange/Act
            IHashProvider<Foo> murmurFoo = new MurmurHashProvider<Foo>();
            IBloomFilter<Foo> bloomFoo = new BloomFilter<Foo>(murmurFoo);
            bloomFoo.Initialize(10, 0.03);
            var fooArray = new Foo[20];
            for (var i = 0; i < 20; i++)
            {
                fooArray[i] = new Foo(i, $"element{i}");
                bloomFoo.Insert(fooArray[i]);
            }

            // Assert
            Assert.IsTrue(bloomFoo.MightContain(fooArray[4]));
            Assert.IsTrue(bloomFoo.MightContain(fooArray[17]));
            Assert.IsFalse(bloomFoo.MightContain(new Foo(21, "element21")));
        }
    }
}