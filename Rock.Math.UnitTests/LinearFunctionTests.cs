namespace LinearFunctionTests
{
    using Rock.Math;

    using NUnit.Framework;

    using PiecewiseFunctionTests;

    [TestFixture]
    public class ConstructorWithSlopeAndIntersect
    {
        [Test]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(-1.0)]
        [TestCase(-50.25)]
        [TestCase(1024.982)]
        public void SetsSlope(double slope)
        {
            var line = new LinearFunction(slope, 1.0);

            Assert.AreEqual(slope, line.Slope);
        }

        [Test]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(-1.0)]
        [TestCase(-50.25)]
        [TestCase(1024.982)]
        public void SetsYIntersect(double yIntersect)
        {
            var line = new LinearFunction(1.0, yIntersect);

            Assert.AreEqual(yIntersect, line.YIntersect);
        }
    }

    [TestFixture]
    public class CopyConstructor
    {
        [Test]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(-1.0)]
        [TestCase(-50.25)]
        [TestCase(1024.982)]
        public void SetsSlope(double slope)
        {
            var line = new LinearFunction(slope, 1.0);
            var otherLine = new LinearFunction(line);

            Assert.AreEqual(slope, otherLine.Slope);
        }

        [Test]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(-1.0)]
        [TestCase(-50.25)]
        [TestCase(1024.982)]
        public void SetsYIntersect(double yIntersect)
        {
            var line = new LinearFunction(1.0, yIntersect);
            var otherLine = new LinearFunction(line);

            Assert.AreEqual(yIntersect, otherLine.YIntersect);
        }
    }

    [TestFixture]
    public class PointSlopeConstructor
    {
        [Test]
        [TestCase(5.0, 0.0, 0.0)]
        [TestCase(2.25, 10.0, -2.13)]
        [TestCase(0.0, 0.0, 0.0)]
        [TestCase(-1.25, -5.0, 4.0)]
        public void SetsSlope(double slope, double x, double y)
        {
            var line = new LinearFunction(slope, x, y);
            Assert.AreEqual(slope, line.Slope);
        }

        [Test]
        [TestCase(5.0, 0.0, 5.0, 5.0)]
        [TestCase(2.25, 10.0, -2.13, -24.63)]
        [TestCase(0.0, 15.0, 0.0, 0.0)]
        [TestCase(-1.25, -5.0, 4.0, -2.25)]
        [TestCase(1.0, 7.5, 9.5, 2.0)]
        public void SetsYIntersect(double slope, double x, double y, double expectedYIntersect)
        {
            var line = new LinearFunction(slope, x, y);
            Assert.AreEqual(expectedYIntersect, line.YIntersect);
        }
    }

    [TestFixture]
    public class TwoPointConstructor
    {
        [Test]
        [TestCase(2.5, 3.25, 10.0, 25.75, 3.0)]
        [TestCase(5.0, 47.12, -2.0, -15.88, 9.0)]
        [TestCase(-5.0, 25.5, 20.2, -32.46, -2.3)]
        public void SetsSlope(double x1, double y1, double x2, double y2, double expectedSlope)
        {
            var line = new LinearFunction(x1, y1, x2, y2);

            Assert.AreEqual(expectedSlope, line.Slope, 0.0000000001);
        }

        [Test]
        [TestCase(2.5, 3.25, 10.0, 25.75, -4.25)]
        [TestCase(5.0, 47.12, -2.0, -15.88, 2.12)]
        [TestCase(-5.0, 25.5, 20.2, -32.46, 14.0)]
        public void SetsYIntersect(double x1, double y1, double x2, double y2, double expectedYIntersect)
        {
            var line = new LinearFunction(x1, y1, x2, y2);

            Assert.AreEqual(expectedYIntersect, line.YIntersect, 0.0000000001);
        }
    }

    [TestFixture]
    public class GetXValue
    {
        [Test]
        [TestCase(4, 0, 2, 8)]
        [TestCase(0.5, -2, 4, 0)]
        [TestCase(3, 1, 5, 16)]
        public void GetsTheRightValue(double slope, double yIntersect, double xValue, double yValue)
        {
            var function = new LinearFunction(slope, yIntersect);

            var x = function.GetXValue(yValue);

            Assert.AreEqual(xValue, x);
        }
    }

    public class AddDoubleOperator
    {
        [Test]
        [TestCase(100, -120, 12, 100, -108)]
        [TestCase(15, 0, 3, 15, 3)]
        [TestCase(-50, 20, -10, -50, 10)]
        public void CorrectlyAddsDouble(double firstSlope, double firstYIntersect, double second, double expectedResultSlope, double expectedResultYIntersect)
        {
            // Arrange
            var first = new LinearFunction(firstSlope, firstYIntersect);

            // Act
            var result = first + second;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResultSlope, result.Slope);
            Assert.AreEqual(expectedResultYIntersect, result.YIntersect);
        }
    }

    [TestFixture]
    public class AddLinearFunctionOperator
    {
        [Test]
        [TestCase(5, 0, 10, -5, 15, -5)]
        [TestCase(-8, 6, 10, -5, 2, 1)]
        [TestCase(100, -130, 15, 9, 115, -121)]
        public void CorrectlyAddsLinearFunction(double firstSlope, double firstYIntersect, double secondSlope, double secondYIntersect, double expectedResultSlope, double expectedResultYIntersect)
        {
            // Arrange
            var first = new LinearFunction(firstSlope, firstYIntersect);
            var second = new LinearFunction(secondSlope, secondYIntersect);

            // Act
            var result = first + second;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResultSlope, result.Slope);
            Assert.AreEqual(expectedResultYIntersect, result.YIntersect);
        }
    }

    [TestFixture]
    public class SubtractDoubleOperator
    {
        [Test]
        [TestCase(100, -120, 12, 100, -132)]
        [TestCase(15, 0, 3, 15, -3)]
        [TestCase(-50, 20, -10, -50, 30)]
        public void CorrectlySubtractsDouble(double firstSlope, double firstYIntersect, double second, double expectedResultSlope, double expectedResultYIntersect)
        {
            // Arrange
            var first = new LinearFunction(firstSlope, firstYIntersect);

            // Act
            var result = first - second;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResultSlope, result.Slope);
            Assert.AreEqual(expectedResultYIntersect, result.YIntersect);
        }
    }

    [TestFixture]
    public class SubtractLinearFunctionOperator
    {
        [Test]
        [TestCase(5, 0, 10, -5, -5, 5)]
        [TestCase(-8, 6, 10, -5, -18, 11)]
        [TestCase(100, -130, 15, 9, 85, -139)]
        public void CorrectlySubtractsLinearFunction(double firstSlope, double firstYIntersect, double secondSlope, double secondYIntersect, double expectedResultSlope, double expectedResultYIntersect)
        {
            // Arrange
            var first = new LinearFunction(firstSlope, firstYIntersect);
            var second = new LinearFunction(secondSlope, secondYIntersect);

            // Act
            var result = first - second;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResultSlope, result.Slope);
            Assert.AreEqual(expectedResultYIntersect, result.YIntersect);
        }
    }

    [TestFixture]
    public class TimesDoubleOperator
    {
        [Test]
        [TestCase(100, -120, 12, 1200, -1440)]
        [TestCase(15, 0, 3, 45, 0)]
        [TestCase(-50, 20, -10, 500, -200)]
        public void CorrectlyMultipliesDouble(double firstSlope, double firstYIntersect, double second, double expectedResultSlope, double expectedResultYIntersect)
        {
            // Arrange
            var first = new LinearFunction(firstSlope, firstYIntersect);

            // Act
            var result = first * second;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResultSlope, result.Slope);
            Assert.AreEqual(expectedResultYIntersect, result.YIntersect);
        }
    }

    [TestFixture]
    public class DividedByDoubleOperator
    {
        [Test]
        [TestCase(100, -120, 12, 8.3333333333333339, -10)]
        [TestCase(15, 0, 3, 5, 0)]
        [TestCase(-50, 20, -10, 5, -2)]
        public void CorrectlyDividesDouble(double firstSlope, double firstYIntersect, double second, double expectedResultSlope, double expectedResultYIntersect)
        {
            // Arrange
            var first = new LinearFunction(firstSlope, firstYIntersect);

            // Act
            var result = first / second;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResultSlope, result.Slope);
            Assert.AreEqual(expectedResultYIntersect, result.YIntersect);
        }
    }

    [TestFixture]
    public class LessThanPiecewiseFunctionOperator : PiecewiseFunctionTest
    {
        private PiecewiseFunction stepUpIncludeUpper;

        private PiecewiseFunction stepDownIncludeUpper;

        private PiecewiseFunction stepUpIncludeLower;

        private PiecewiseFunction stepDownIncludeLower;

        private PiecewiseFunction constantOne;

        [SetUp]
        public void Setup()
        {
            this.stepUpIncludeUpper = new PiecewiseFunction { { 1.0, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepUpIncludeLower = new PiecewiseFunction { { 1.0, false, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepDownIncludeUpper = new PiecewiseFunction { { 1.0, 2.0 }, { double.MaxValue, 1.0 } };
            this.stepDownIncludeLower = new PiecewiseFunction { { 1.0, false, 2.0 }, { double.MaxValue, 1.0 } };
            this.constantOne = new PiecewiseFunction { { double.MaxValue, 1.0 } };
        }

        [Test]
        public void ExcludesPieceEntirelyUnderPositiveLine()
        {
            var line = new LinearFunction(1.0, 2.0);

            var result = line < this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = line < this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 2.0);

            var result = line < this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = line < this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesBeforePositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = line < this.constantOne;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.99999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(2.0));
            Assert.AreEqual(0.0, result.GetValue(1000.0));
            Assert.AreEqual(0.0, result.GetValue(double.MaxValue));
        }

        [Test]
        public void IncludesAfterNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = line < this.constantOne;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(1.5));
            Assert.AreEqual(0.0, result.GetValue(2.99999));
            Assert.AreEqual(0.0, result.GetValue(3.0));
            Assert.AreEqual(1.0, result.GetValue(10.0));
            Assert.AreEqual(1.0, result.GetValue(1000.0));
            Assert.AreEqual(1.0, result.GetValue(double.MaxValue));
        }

        /* Note - all of the following tests are for endpoints, and as such they are only written
         * for the cases where the inequality goes from true to false or vice versa. Otherwise we
         * would not know whether it was obeying the correct logic at the endpoint.
         */
        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedUpperBound()
        {
            var yEqualsX = new LinearFunction(1.0, 0.0);

            var result = yEqualsX < this.stepUpIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedLowerBound()
        {
            var zeroPlusX = new LinearFunction(1.0, 0.0);

            var result = zeroPlusX < this.stepDownIncludeLower;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedUpperBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = twoMinusX < this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedLowerBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = twoMinusX < this.stepDownIncludeLower;

            Assert.AreEqual(1.0, result.GetValue(0.001));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class LessThanOrEqualToPiecewiseFunctionOperator : PiecewiseFunctionTest
    {
        private PiecewiseFunction stepUpIncludeUpper;

        private PiecewiseFunction stepDownIncludeUpper;

        private PiecewiseFunction stepUpIncludeLower;

        private PiecewiseFunction stepDownIncludeLower;

        private PiecewiseFunction constantOne;

        [SetUp]
        public void Setup()
        {
            this.stepUpIncludeUpper = new PiecewiseFunction { { 1.0, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepUpIncludeLower = new PiecewiseFunction { { 1.0, false, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepDownIncludeUpper = new PiecewiseFunction { { 1.0, 2.0 }, { double.MaxValue, 1.0 } };
            this.stepDownIncludeLower = new PiecewiseFunction { { 1.0, false, 2.0 }, { double.MaxValue, 1.0 } };
            this.constantOne = new PiecewiseFunction { { double.MaxValue, 1.0 } };
        }

        [Test]
        public void ExcludesPieceEntirelyUnderPositiveLine()
        {
            var line = new LinearFunction(1.0, 2.0);

            var result = line <= this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = line <= this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 2.5);

            var result = line <= this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = line <= this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesBeforeAndWhenPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = line <= this.constantOne;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.99999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(2.0));
            Assert.AreEqual(0.0, result.GetValue(1000.0));
            Assert.AreEqual(0.0, result.GetValue(double.MaxValue));
        }

        [Test]
        public void IncludesAfterAndWhenNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = line <= this.constantOne;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(1.5));
            Assert.AreEqual(0.0, result.GetValue(2.99999));
            Assert.AreEqual(1.0, result.GetValue(3.0));
            Assert.AreEqual(1.0, result.GetValue(10.0));
            Assert.AreEqual(1.0, result.GetValue(1000.0));
            Assert.AreEqual(1.0, result.GetValue(double.MaxValue));
        }

        /* Note - all of the following tests are for endpoints, and as such they are only written
         * for the cases where the inequality goes from true to false or vice versa. Otherwise we
         * would not know whether it was obeying the correct logic at the endpoint.
         */
        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedUpperBound()
        {
            var onePlusX = new LinearFunction(1.0, 1.0);

            var result = onePlusX <= this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedLowerBound()
        {
            var onePlusX = new LinearFunction(1.0, 1.0);

            var result = onePlusX <= this.stepUpIncludeLower;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedUpperBound()
        {
            var threeMinusX = new LinearFunction(-1.0, 3.0);

            var result = threeMinusX <= this.stepDownIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedLowerBound()
        {
            var threeMinusX = new LinearFunction(-1.0, 3.0);

            var result = threeMinusX <= this.stepUpIncludeLower;

            Assert.AreEqual(0.0, result.GetValue(0.001));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class GreaterThanPiecewiseFunctionOperator : PiecewiseFunctionTest
    {
        private PiecewiseFunction stepUpIncludeUpper;

        private PiecewiseFunction stepDownIncludeUpper;

        private PiecewiseFunction stepUpIncludeLower;

        private PiecewiseFunction stepDownIncludeLower;

        private PiecewiseFunction constantOne;

        [SetUp]
        public void Setup()
        {
            this.stepUpIncludeUpper = new PiecewiseFunction { { 1.0, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepUpIncludeLower = new PiecewiseFunction { { 1.0, false, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepDownIncludeUpper = new PiecewiseFunction { { 1.0, 2.0 }, { double.MaxValue, 1.0 } };
            this.stepDownIncludeLower = new PiecewiseFunction { { 1.0, false, 2.0 }, { double.MaxValue, 1.0 } };
            this.constantOne = new PiecewiseFunction { { double.MaxValue, 1.0 } };
        }

        [Test]
        public void ExcludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = line > this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderPositiveLine()
        {
            var line = new LinearFunction(1.0, 4.0);

            var result = line > this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = line > this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = line > this.stepDownIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesBeforeNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 2.0);

            var result = line > this.constantOne;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.99999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(2.0));
            Assert.AreEqual(0.0, result.GetValue(1000.0));
            Assert.AreEqual(0.0, result.GetValue(double.MaxValue));
        }

        [Test]
        public void IncludesAfterPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = line > this.constantOne;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(1.5));
            Assert.AreEqual(0.0, result.GetValue(2.99999));
            Assert.AreEqual(0.0, result.GetValue(3.0));
            Assert.AreEqual(1.0, result.GetValue(3.0001));
            Assert.AreEqual(1.0, result.GetValue(10.0));
            Assert.AreEqual(1.0, result.GetValue(1000.0));
            Assert.AreEqual(1.0, result.GetValue(double.MaxValue));
        }

        /* Note - all of the following tests are for endpoints, and as such they are only written
         * for the cases where the inequality goes from true to false or vice versa. Otherwise we
         * would not know whether it was obeying the correct logic at the endpoint.
         */
        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedUpperBound()
        {
            var onePlusX = new LinearFunction(1.0, 1.0);

            var result = onePlusX > this.stepDownIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedLowerBound()
        {
            var onePlusX = new LinearFunction(1.0, 1.0);

            var result = onePlusX > this.stepDownIncludeLower;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedUpperBound()
        {
            var threeMinusX = new LinearFunction(-1.0, 3.0);

            var result = threeMinusX > this.stepUpIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedLowerBound()
        {
            var threeMinusX = new LinearFunction(-1.0, 3.0);

            var result = threeMinusX > this.stepUpIncludeLower;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class GreaterThanOrEqualToPiecewiseFunctionOperator : PiecewiseFunctionTest
    {
        private PiecewiseFunction stepUpIncludeUpper;

        private PiecewiseFunction stepDownIncludeUpper;

        private PiecewiseFunction stepUpIncludeLower;

        private PiecewiseFunction stepDownIncludeLower;

        private PiecewiseFunction constantOne;

        [SetUp]
        public void Setup()
        {
            this.stepUpIncludeUpper = new PiecewiseFunction { { 1.0, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepUpIncludeLower = new PiecewiseFunction { { 1.0, false, 1.0 }, { double.MaxValue, 2.0 } };
            this.stepDownIncludeUpper = new PiecewiseFunction { { 1.0, 2.0 }, { double.MaxValue, 1.0 } };
            this.stepDownIncludeLower = new PiecewiseFunction { { 1.0, false, 2.0 }, { double.MaxValue, 1.0 } };
            this.constantOne = new PiecewiseFunction { { double.MaxValue, 1.0 } };
        }

        [Test]
        public void IncludesPieceEntirelyUnderPositiveLine()
        {
            var line = new LinearFunction(1.0, 2.0);

            var result = line >= this.stepUpIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = line >= this.stepDownIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 2.5);

            var result = line >= this.stepUpIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = line >= this.stepDownIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesAfterAndWhenPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = line >= this.constantOne;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.99999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(2.0));
            Assert.AreEqual(1.0, result.GetValue(1000.0));
            Assert.AreEqual(1.0, result.GetValue(double.MaxValue));
        }

        [Test]
        public void IncludesBeforeAndWhenNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = line >= this.constantOne;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(1.5));
            Assert.AreEqual(1.0, result.GetValue(2.99999));
            Assert.AreEqual(1.0, result.GetValue(3.0));
            Assert.AreEqual(0.0, result.GetValue(3.0001));
            Assert.AreEqual(0.0, result.GetValue(10.0));
            Assert.AreEqual(0.0, result.GetValue(1000.0));
            Assert.AreEqual(0.0, result.GetValue(double.MaxValue));
        }

        /* Note - all of the following tests are for endpoints, and as such they are only written
         * for the cases where the inequality goes from true to false or vice versa. Otherwise we
         * would not know whether it was obeying the correct logic at the endpoint.
         */
        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedUpperBound()
        {
            var onePlusX = new LinearFunction(1.0, 0.0);

            var result = onePlusX >= this.stepUpIncludeUpper;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesPositiveLinearFunctionAtIncludedLowerBound()
        {
            var zeroPlusX = new LinearFunction(1.0, 0.0);

            var result = zeroPlusX >= this.stepDownIncludeLower;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedUpperBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = twoMinusX >= this.stepUpIncludeUpper;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedLowerBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = twoMinusX >= this.stepDownIncludeLower;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.001));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }
    }


    [TestFixture]
    public class GetIntersection
    {
        [Test]
        [TestCase(0.0, 4.0)]
        [TestCase(-1000.0, 1000.0)]
        [TestCase(14.0, -1.0)]
        [TestCase(-14.0, 1.0)]
        [TestCase(2.0, 2.0)]
        [TestCase(-1.0, -1.0)]
        public void ReturnsNullForEqualLines(double slope, double yIntersect)
        {
            var line1 = new LinearFunction(slope, yIntersect);
            var line2 = new LinearFunction(slope, yIntersect);

            var intersection = line1.GetIntersection(line2);

            Assert.IsNull(intersection);
        }

        [Test]
        [TestCase(0.0, 1.0, 2.0)]
        [TestCase(0.0, -1.0, 1.0)]
        [TestCase(4.0, 1.0, -1.0)]
        [TestCase(1000.0, 500.0, 250.0)]
        public void ReturnsNullForParallelLines(double slope, double yIntersect1, double yIntersect2)
        {
            var line1 = new LinearFunction(slope, yIntersect1);
            var line2 = new LinearFunction(slope, yIntersect2);

            var intersection = line1.GetIntersection(line2);

            Assert.IsNull(intersection);
        }

        [Test]
        [TestCase(1.0, 0.0, 0.5, 1.0, 2.0)]
        [TestCase(1.0, 0.0, -1.0, 0.0, 0.0)]
        [TestCase(2.5, 10.0, -5.0, 25.0, 2.0)]
        public void ReturnsCorrectAnswerForNonParallelLines(
            double slope1,
            double intersect1,
            double slope2,
            double intersect2,
            double expectedValue)
        {
            var line1 = new LinearFunction(slope1, intersect1);
            var line2 = new LinearFunction(slope2, intersect2);

            var intersection = line1.GetIntersection(line2);

            Assert.IsNotNull(intersection);
            Assert.AreEqual(expectedValue, intersection.Value);
        }
    }
}
