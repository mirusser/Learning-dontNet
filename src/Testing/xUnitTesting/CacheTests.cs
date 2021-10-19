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
    public class CacheTests
    {
        [Fact]
        public void Should_CachesItemWithinTimeSpan()
        {
            //arrange
            var cache = new Cache(TimeSpan.FromDays(1));
            cache.Add(new("url", "content", DateTime.Now));

            //act
            var contains = cache.Contains("url");

            //assert
            contains.Should().BeTrue();
        }

        [Fact]
        public void Should_Contains_ReturnFalse_WhenOutsideTimeSpan()
        {
            //arrange
            var cache = new Cache(TimeSpan.FromDays(1));
            cache.Add(new("url", "content", DateTime.Now.AddDays(-2)));

            //act
            var contains = cache.Contains("url");

            //assert
            contains.Should().BeFalse();
        }

        [Fact]
        public void Should_Contains_ReturnsFalse_WhenDoesntContainItem()
        {
            //arrange
            var cache = new Cache(TimeSpan.FromDays(1));

            //act
            var contains = cache.Contains("url");

            //assert
            contains.Should().BeFalse();
        }
    }
}
