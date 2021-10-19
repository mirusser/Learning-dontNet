using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using xUnitTesting.Units;

namespace xUnitTesting
{
    public class PropertyHashTests
    {
        [Fact]
        public void Should_PropertyHash_ConcatenatesSelectedFieldsInOrder()
        {
            //arrange
            var hasher = new PropertyHash();
            var item = new Cache.Item("url", "content", DateTime.Now);

            //act
            var hash = hasher.Hash(item, i => i.Url, i => i.Content);

            //assert
            hash.Should().Equals("urlcontent");
        }

        [Fact]
        public void Should_AlgorithmPropertyHash_AppliesHashingAlgorithToSeed()
        {
            //arrange
            var hasher = new AlgorithmPropertyHash("sha256");
            var item = new Cache.Item("url", "content", DateTime.Now);

            //act
            var hash = hasher.Hash(item, i => i.Url, i => i.Content);

            //assert
            hash.Should().Equals("9FyLxk+9z73XO8xhZ15emMaK+oa8aDg6LWiY6y40KyQ=");
        }
    }
}
