namespace PieceTests
{
    using NUnit.Framework;

    using Rock.Math;

    [TestFixture]
    public class Contains
    {
        [TestCase(false, -1.0, 1.0)]
        [TestCase(true, -1.0, 1.0)]
        [TestCase(false, -150.0, -100.0)]
        [TestCase(true, -150.0, -100.0)]
        [TestCase(false, 0.0, 5.0)]
        [TestCase(true, 0.0, 5.0)]
        [TestCase(false, 1000.0, 2500.0)]
        [TestCase(true, 1000.0, 2500.0)]
        public void ReturnsTrueForIncludedLowerBound(bool includeRight, double lower, double upper)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = includeRight, UpperBound = upper };

            Assert.IsTrue(piece.Contains(lower, lower, true));
        }

        [TestCase(false, -1.0, 1.0)]
        [TestCase(true, -1.0, 1.0)]
        [TestCase(false, 0.0, 5.0)]
        [TestCase(true, 0.0, 5.0)]
        [TestCase(false, 1000.0, 2500.0)]
        [TestCase(true, 1000.0, 2500.0)]
        public void ReturnsFalseForExcludedLowerBound(bool includeRight, double lower, double upper)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = includeRight, UpperBound = upper };

            Assert.IsFalse(piece.Contains(lower, lower, false));
        }

        [TestCase(false, -1.0, 1.0)]
        [TestCase(true, -1.0, 1.0)]
        [TestCase(false, -150.0, -100.0)]
        [TestCase(true, -150.0, -100.0)]
        [TestCase(false, 0.0, 5.0)]
        [TestCase(true, 0.0, 5.0)]
        [TestCase(false, 1000.0, 2500.0)]
        [TestCase(true, 1000.0, 2500.0)]
        public void ReturnsTrueForIncludedUpperBound(bool includeLeft, double lower, double upper)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = true, UpperBound = upper };

            Assert.IsTrue(piece.Contains(upper, lower, includeLeft));
        }

        [TestCase(false, -1.0, 1.0)]
        [TestCase(true, -1.0, 1.0)]
        [TestCase(false, 0.0, 5.0)]
        [TestCase(true, 0.0, 5.0)]
        [TestCase(false, 1000.0, 2500.0)]
        [TestCase(true, 1000.0, 2500.0)]
        public void ReturnsFalseForExcludedUpperBound(bool includeLeft, double lower, double upper)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = false, UpperBound = upper };

            Assert.IsFalse(piece.Contains(upper, lower, includeLeft));
        }

        [Test]
        [TestCase(true, true, -1.0, 1.0, 0.0)]
        [TestCase(true, false, -1.0, 1.0, 0.0)]
        [TestCase(false, true, -1.0, 1.0, 0.0)]
        [TestCase(false, false, -1.0, 1.0, 0.0)]
        [TestCase(true, true, -100.0, -10.0, -45.25)]
        [TestCase(true, false, -100.0, -10.0, -45.25)]
        [TestCase(false, true, -100.0, -10.0, -45.25)]
        [TestCase(false, false, -100.0, -10.0, -45.25)]
        [TestCase(true, true, 1000.0, 2000.0, 1000.001)]
        [TestCase(true, false, 1000.0, 2000.0, 1000.001)]
        [TestCase(false, true, 1000.0, 2000.0, 1000.001)]
        [TestCase(false, false, 1000.0, 2000.0, 1000.001)]
        [TestCase(true, true, 1000.0, 2000.0, 1999.999)]
        [TestCase(true, false, 1000.0, 2000.0, 1999.999)]
        [TestCase(false, true, 1000.0, 2000.0, 1999.999)]
        [TestCase(false, false, 1000.0, 2000.0, 1999.999)]
        public void ReturnsTrueBetweenBounds(bool includeLeft, bool includeRight, double lower, double upper, double between)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = false, UpperBound = upper };

            Assert.IsTrue(piece.Contains(between, lower, includeLeft));
        }

        [Test]
        [TestCase(true, true, -1.0, 1.0, -5.0)]
        [TestCase(true, false, -1.0, 1.0, -5.0)]
        [TestCase(false, true, -1.0, 1.0, -5.0)]
        [TestCase(false, false, -1.0, 1.0, -5.0)]
        [TestCase(true, true, -1.0, 1.0, -1.00001)]
        [TestCase(true, false, -1.0, 1.0, -1.00001)]
        [TestCase(false, true, -1.0, 1.0, -1.00001)]
        [TestCase(false, false, -1.0, 1.0, -1.00001)]
        [TestCase(true, true, 0.0, 100.0, -1.0)]
        [TestCase(true, false, 0.0, 100.0, -1.0)]
        [TestCase(false, true, 0.0, 100.0, -1.0)]
        [TestCase(false, false, 0.0, 100.0, -1.0)]
        [TestCase(true, true, 0.0, 100.0, -0.00001)]
        [TestCase(true, false, 0.0, 100.0, -0.00001)]
        [TestCase(false, true, 0.0, 100.0, -0.00001)]
        [TestCase(false, false, 0.0, 100.0, -0.00001)]
        [TestCase(true, true, 1000.0, 2000.0, 500.0)]
        [TestCase(true, false, 1000.0, 2000.0, 500.0)]
        [TestCase(false, true, 1000.0, 2000.0, 500.0)]
        [TestCase(false, false, 1000.0, 2000.0, 500.0)]
        public void ReturnsFalseBelowLowerBound(bool includeLeft, bool includeRight, double lower, double upper, double below)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = false, UpperBound = upper };

            Assert.IsFalse(piece.Contains(below, lower, includeLeft));
        }

        [Test]
        [TestCase(true, true, -1.0, 1.0, 5.0)]
        [TestCase(true, false, -1.0, 1.0, 5.0)]
        [TestCase(false, true, -1.0, 1.0, 5.0)]
        [TestCase(false, false, -1.0, 1.0, 5.0)]
        [TestCase(true, true, -1.0, 1.0, 1.00001)]
        [TestCase(true, false, -1.0, 1.0, 1.00001)]
        [TestCase(false, true, -1.0, 1.0, 1.00001)]
        [TestCase(false, false, -1.0, 1.0, 1.00001)]
        [TestCase(true, true, 0.0, 100.0, 101.0)]
        [TestCase(true, false, 0.0, 100.0, 101.0)]
        [TestCase(false, true, 0.0, 100.0, 101.0)]
        [TestCase(false, false, 0.0, 100.0, 101.0)]
        [TestCase(true, true, 0.0, 100.0, 100.00001)]
        [TestCase(true, false, 0.0, 100.0, 100.00001)]
        [TestCase(false, true, 0.0, 100.0, 100.00001)]
        [TestCase(false, false, 0.0, 100.0, 100.00001)]
        [TestCase(true, true, 1000.0, 2000.0, 2500.0)]
        [TestCase(true, false, 1000.0, 2000.0, 2500.0)]
        [TestCase(false, true, 1000.0, 2000.0, 2500.0)]
        [TestCase(false, false, 1000.0, 2000.0, 2500.0)]
        public void ReturnsFalseAboveUpperBound(bool includeLeft, bool includeRight, double lower, double upper, double above)
        {
            var piece = new PiecewiseFunction.Piece { IncludeUpperBound = false, UpperBound = upper };

            Assert.IsFalse(piece.Contains(above, lower, includeLeft));
        }
    }

    [TestFixture]
    public class GetIntersect
    {
        [Test]
        [TestCase(true, true, 0.0, 20.0, 3.5, -1.0, 0.5, 9.0)]
        [TestCase(true, true, 0.0, 10.0, -2.0, -1.0, -1.0, 1.0)]
        public void ReturnsCorrectValueWhenIntersectionExists(bool includeLeft, bool includeRight, double lower, double upper, double value, double yIntersect, double slope, double expected)
        {
            var piece = new PiecewiseFunction.Piece { UpperBound = upper, Value = value, IncludeUpperBound = includeRight };
            var function = new LinearFunction(slope, yIntersect);

            var intersect = piece.GetIntersect(function, lower, includeLeft);
            
            Assert.IsTrue(intersect.HasValue);
            Assert.AreEqual(expected, intersect.Value);
        }

        [TestCase(true, true, 0.0, 20.0, 3.5, -20.0, 0.5)]
        [TestCase(true, true, 0.0, 10.0, -2.0, -2.5, 0.0)]
        [TestCase(true, true, 0.0, 10.0, -2.0, -3.5, -1.0)]
        [TestCase(false, false, 0.0, 10.0, -2.0, -2.0, -1.0)]
        [TestCase(false, false, 0.0, 10.0, 3.0, -17.0, 2.0)]
        public void ReturnsNullWhenNoIntersectionExists(
            bool includeLeft,
            bool includeRight,
            double lower,
            double upper,
            double value,
            double yIntersect,
            double slope)
        {
            var piece = new PiecewiseFunction.Piece { UpperBound = upper, Value = value, IncludeUpperBound = includeRight };
            var function = new LinearFunction(slope, yIntersect);

            var intersect = piece.GetIntersect(function, lower, includeLeft);

            Assert.IsFalse(intersect.HasValue);
        }
    }
}
