using System;
using NUnit.Framework;

namespace Tests
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase(10)]
    public void Test1(int value)
    {
      Assert.That(value, Is.EqualTo(value));
    }
  }
}