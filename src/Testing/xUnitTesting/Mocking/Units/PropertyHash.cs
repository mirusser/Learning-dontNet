using System;
using System.Security.Cryptography;
using System.Text;

namespace xUnitTesting.Mocking.Units
{
    public class PropertyHash
    {
        public virtual string Hash<T>(T input, params Func<T, string>[] selectors)
        {
            StringBuilder builder = new();
            foreach (var selector in selectors)
            {
                builder.Append(selector(input));
            }

            return builder.ToString();
        }
    }

    public interface IHashAlgorithmFactory
    {
        HashAlgorithm Create();
    }

    public class AlgorithmPropertyHash
    {
        private readonly IHashAlgorithmFactory _hashAlgorithmFactory;

        public AlgorithmPropertyHash(IHashAlgorithmFactory hashAlgorithmFactory)
        {
            _hashAlgorithmFactory = hashAlgorithmFactory;
        }

        public string Hash(string seed)
        {
            var seedBytes = Encoding.UTF8.GetBytes(seed);

            using var algorithm = _hashAlgorithmFactory.Create();
            var hashBytes = algorithm.ComputeHash(seedBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}