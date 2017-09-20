namespace DoubleExtensionTests
{
    using Rock.Math;

    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class ApproximatelyEquals
    {
        public static IEnumerable<double> Doubles
            =>
                new[]
                {
                    double.NegativeInfinity,
                    double.MinValue,
                    -1000000000000.0,
                    -100.0,
                    -10.0,
                    -1.0,
                    -0.01,
                    -0.0000000000001,
                    0.0,
                    0.0000000000001,
                    0.01,
                    1.0,
                    10.0,
                    100.0,
                    1000000000000.0,
                    double.MaxValue,
                    double.PositiveInfinity
                };

        [TestCaseSource(nameof(Doubles))]
        public void ReturnsTrueForSameNumber(double number) => Assert.IsTrue(number.ApproximatelyEquals(number));

        [TestCaseSource(nameof(Doubles))]
        public void ReturnsTrueForInfinitesimallyLargerNumber(double number)
        {
            var otherNumber = number + double.Epsilon;
            Assert.IsTrue(number.ApproximatelyEquals(otherNumber));
        }

        [TestCaseSource(nameof(Doubles))]
        public void ReturnsTrueForInfinitesimallySmallerNumber(double number)
        {
            var otherNumber = number - double.Epsilon;
            Assert.IsTrue(number.ApproximatelyEquals(otherNumber));
        }

        [TestCaseSource(nameof(Doubles))]
        public void ReturnsTrueForSlightlyLargerNumber(double number)
        {
            var otherNumber = number + 0.000000000000001;
            Assert.IsTrue(number.ApproximatelyEquals(otherNumber));
        }

        [TestCaseSource(nameof(Doubles))]
        public void ReturnsTrueForSlightlySmallerNumber(double number)
        {
            var otherNumber = number - 0.000000000000001;
            Assert.IsTrue(number.ApproximatelyEquals(otherNumber));
        }

        [Test]
        [TestCase(double.NegativeInfinity, double.MinValue)]
        [TestCase(-100000.0, -50000.0)]
        [TestCase(-100000.00, -99999)]
        [TestCase(-100000.00, -99999.99)]
        [TestCase(-100.0, -99.99)]
        [TestCase(-3.875, 3.8749999)]
        [TestCase(-2.0, -1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(0.0, 1.0)]
        [TestCase(1.0, 2.0)]
        [TestCase(3.8749999, 3.875)]
        [TestCase(99.99, 100.0)]
        [TestCase(99999.99, 100000.00)]
        [TestCase(99999, 100000.00)]
        [TestCase(50000.0, 100000.0)]
        [TestCase(double.MaxValue, double.PositiveInfinity)]
        public void ReturnsFalseForLargerNumber(double number, double largerNumber) => Assert.IsFalse(number.ApproximatelyEquals(largerNumber));

        [Test]
        [TestCase(double.MinValue, double.NegativeInfinity)]
        [TestCase(-50000.0, -100000.0)]
        [TestCase(-99999, -100000.00)]
        [TestCase(-99999.99, -100000.00)]
        [TestCase(-99.99, -100.0)]
        [TestCase(3.8749999, -3.875)]
        [TestCase(-1.0, -2.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(2.0, 1.0)]
        [TestCase(3.875, 3.8749999)]
        [TestCase(100.0, 99.99)]
        [TestCase(100000.00, 99999.99)]
        [TestCase(100000.00, 99999)]
        [TestCase(100000.0, 50000.0)]
        [TestCase(double.PositiveInfinity, double.MaxValue)]
        public void ReturnsFalseForSmallerNumber(double number, double smallerNumber) => Assert.IsFalse(number.ApproximatelyEquals(smallerNumber));
    }

    [TestFixture]
    public class IsApproximatelyZero
    {
        [Test]
        public void ReturnsTrueForZero()
        {
            const double Number = 0.0;
            Assert.IsTrue(Number.IsApproximatelyZero());
        }

        [Test]
        [TestCase(0.000000000001)]
        [TestCase(0.0000000000001)]
        [TestCase(0.00000000000001)]
        public void ReturnsTrueForVerySmallPositiveNumbers(double number) => Assert.IsTrue(number.IsApproximatelyZero());

        [Test]
        [TestCase(-0.000000000001)]
        [TestCase(-0.0000000000001)]
        [TestCase(-0.00000000000001)]
        public void ReturnsTrueForVerySmallNegativeNumbers(double number) => Assert.IsTrue(number.IsApproximatelyZero());

        [Test]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.MinValue)]
        [TestCase(-1000000000000.0)]
        [TestCase(-100.0)]
        [TestCase(-10.0)]
        [TestCase(-1.0)]
        [TestCase(-0.01)]
        [TestCase(-0.000001)]
        [TestCase(-0.000000001)]
        [TestCase(0.000000001)]
        [TestCase(0.000001)]
        [TestCase(0.01)]
        [TestCase(1.0)]
        [TestCase(10.0)]
        [TestCase(100.0)]
        [TestCase(1000000000000.0)]
        [TestCase(double.MaxValue)]
        [TestCase(double.PositiveInfinity)]
        public void ReturnsFalseForNumbersThatAreNotVerySmall(double number) => Assert.IsFalse(number.IsApproximatelyZero());
    }
}
