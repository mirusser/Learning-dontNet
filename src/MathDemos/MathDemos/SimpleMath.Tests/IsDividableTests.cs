namespace SimpleMath.Tests;

public class IsDividableTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase(6, 2, true)]
    [TestCase(6, 3, true)]
    [TestCase(6, 4, false)]
    [TestCase(6, 5, false)]
    [TestCase(6, 6, true)]
    [TestCase(7, 2, false)]
    [TestCase(7, 3, false)]
    [TestCase(7, 4, false)]
    [TestCase(7, 5, false)]
    [TestCase(7, 7, true)]
    public void IsDividableTest(int number, int divider, bool expectedResult)
    {
        Assert.That(IsDividable.Run(number, divider), Is.EqualTo(expectedResult));
    }
}