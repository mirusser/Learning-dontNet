namespace SimpleMath.Tests;

public class IsPrimeTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(0, false)]
    [TestCase(1, false)]
    [TestCase(2, true)]
    [TestCase(3, true)]
    [TestCase(4, false)]
    [TestCase(6, false)]
    [TestCase(9, false)]
    [TestCase(15, false)]
    [TestCase(25, false)]
    [TestCase(35, false)]
    [TestCase(5, true)]
    [TestCase(11, true)]
    [TestCase(23, true)]
    [TestCase(29, true)]
    public void IsPrimeTest(int n, bool expectedResult)
    {
        Assert.That(IsPrime.Run(n), Is.EqualTo(expectedResult));
    }
}