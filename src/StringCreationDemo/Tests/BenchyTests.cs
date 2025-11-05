namespace Tests;

using NUnit.Framework;

[TestFixture]
public class BenchyTests
{
    private string expectedValue = null!;
    private Benchy sut = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // Runs once before all tests in this class
        expectedValue = "Pas*********";
    }

    [SetUp]
    public void Setup()
    {
        // Runs before each test
        sut = new Benchy();
    }

    [Test]
    public void MaskNaive_ReturnsMaskedValue()
    {
        var result = sut.MaskNaive();
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void MaskStringBuilder_ReturnsMaskedValue()
    {
        var result = sut.MaskStringBuilder();
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void MaskNewString_ReturnsMaskedValue()
    {
        var result = sut.MaskNewString();
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    public void MaskStringCreate_ReturnsMaskedValue()
    {
        var result = sut.MaskStringCreate();
        Assert.That(result, Is.EqualTo(expectedValue));
    }
}
