namespace PiecewiseFunctionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Rock.Math;

    using NUnit.Framework;

    public static class TestHelpers
    {
        

        #region Convenience Functions
        public static PiecewiseFunction Zero => new PiecewiseFunction { { double.MaxValue, 0.0 } };
        public static PiecewiseFunction StepDownIncludeRight => new PiecewiseFunction
        {
            { 1.0, 5.0 },
            { 2.0, 4.0 },
            { 3.0, 3.0 },
            { 4.0, 2.0 },
            { 5.0, 1.0 }
        };

        public static PiecewiseFunction StepDownIncludeLeft => new PiecewiseFunction
        {
            { 1.0, false, 5.0 },
            { 2.0, false, 4.0 },
            { 3.0, false, 3.0 },
            { 4.0, false, 2.0 },
            { 5.0, false, 1.0 }
        };

        public static PiecewiseFunction StepUpIncludeRight => new PiecewiseFunction
        {
            { 1.0, 1.0 },
            { 2.0, 2.0 },
            { 3.0, 3.0 },
            { 4.0, 4.0 },
            { 5.0, 5.0 }
        };

        public static PiecewiseFunction StepUpIncludeLeft => new PiecewiseFunction
        {
            { 1.0, false, 1.0 },
            { 2.0, false, 2.0 },
            { 3.0, false, 3.0 },
            { 4.0, false, 4.0 },
            { 5.0, false, 5.0 }
        };

        public static PiecewiseFunction Floor => new PiecewiseFunction
        {
            { 1.0, false, 0.0 },
            { 2.0, false, 1.0 },
            { 3.0, false, 2.0 },
            { 4.0, false, 3.0 },
            { 5.0, false, 4.0 },
            { 6.0, false, 5.0 },
            { 7.0, false, 6.0 },
            { 8.0, false, 7.0 },
            { 9.0, false, 8.0 },
            { 10.0, false, 9.0 }
        };

        public static PiecewiseFunction Ceiling => new PiecewiseFunction
        {
            { 1.0, 1.0 },
            { 2.0, 2.0 },
            { 3.0, 3.0 },
            { 4.0, 4.0 },
            { 5.0, 5.0 },
            { 6.0, 6.0 },
            { 7.0, 7.0 },
            { 8.0, 8.0 },
            { 9.0, 9.0 },
            { 10.0, 10.0 }
        };

        public static PiecewiseFunction Round => new PiecewiseFunction
        {
            { 0.5, false, 0.0 },
            { 1.5, false, 1.0 },
            { 2.5, false, 2.0 },
            { 3.5, false, 3.0 },
            { 4.5, false, 4.0 },
            { 5.5, false, 5.0 },
        };

        public static PiecewiseFunction RoundMidpointUp => new PiecewiseFunction
        {
            { 0.5, false, 0.0 },
            { 1.5, false, 1.0 },
            { 2.5, false, 2.0 },
            { 3.5, false, 3.0 },
            { 4.5, false, 4.0 },
            { 5.5, false, 5.0 },
        };

        public static PiecewiseFunction RoundMidpointToEven => new PiecewiseFunction
        {
            { 0.5, true, 0.0 },
            { 1.5, false, 1.0 },
            { 2.5, true, 2.0 },
            { 3.5, false, 3.0 },
            { 4.5, true, 4.0 },
            { 5.5, false, 5.0 },
        };

        public static PiecewiseFunction Even => new PiecewiseFunction
        {
            { 0.0, false, 0.0 },
            { 1.0, false, 1.0 },
            { 2.0, false, 0.0 },
            { 3.0, false, 1.0 },
            { 4.0, false, 0.0 },
            { 5.0, false, 1.0 },
            { 6.0, false, 0.0 },
            { 7.0, false, 1.0 },
            { 8.0, false, 0.0 },
            { 9.0, false, 1.0 },
            { 10.0, false, 0.0 }
        };

        public static PiecewiseFunction LessThanFive => new PiecewiseFunction { { 5.0, false, 1.0 } };

        public static PiecewiseFunction LessThanOrEqualToFive => new PiecewiseFunction { { 5.0, 1.0 } };

        public static PiecewiseFunction GreaterThanFive => new PiecewiseFunction { { 5.0, true, 0.0 }, { double.MaxValue, 1.0 } };

        public static PiecewiseFunction GreaterThanOrEqualToFive => new PiecewiseFunction { { 5.0, false, 0.0 }, { double.MaxValue, 1.0 } };

        public static PiecewiseFunction LessThanTwo => new PiecewiseFunction { { 2.0, false, 1.0 } };

        public static PiecewiseFunction LessThanOrEqualToTwo => new PiecewiseFunction { { 2.0, 1.0 } };

        public static PiecewiseFunction TwoIfLessThanTwo => new PiecewiseFunction { { 2.0, false, 2.0 } };

        public static PiecewiseFunction TwoIfLessThanOrEqualToTwo => new PiecewiseFunction { { 2.0, 2.0 } };

        public static PiecewiseFunction GreaterThanTwo => new PiecewiseFunction { { 2.0, true, 0.0 }, { double.MaxValue, 1.0 } };

        public static PiecewiseFunction GreaterThanOrEqualToTwo => new PiecewiseFunction { { 2.0, false, 0.0 }, { double.MaxValue, 1.0 } };

        public static PiecewiseFunction TwoIfGreaterThanTwo => new PiecewiseFunction { { 2.0, true, 0.0 }, { double.MaxValue, 2.0 } };

        public static PiecewiseFunction TwoIfGreaterThanOrEqualToTwo => new PiecewiseFunction { { 2.0, false, 0.0 }, { double.MaxValue, 2.0 } };
        #endregion

        #region Helper Asserts
        public static void VerifyUpperBound(PiecewiseFunction function, double upperBound, bool includeUpperBound, bool second)
        {
            var found = false;

            foreach (var piece in function)
            {
                if (upperBound.ApproximatelyEquals(piece.UpperBound))
                {
                    if (second)
                    {
                        second = false;
                    }
                    else
                    {
                        Assert.AreEqual(includeUpperBound, piece.IncludeUpperBound);
                        found = true;
                        break;
                    }
                }
            }

            Assert.IsTrue(found, "Did not find " + (second ? "second " : string.Empty) + "piece with upper bound of '" + upperBound + "'.");
        }

        public static void VerifyMaxValuePiece(PiecewiseFunction function, double expectedValue, double lowerBound, bool includeLowerBound)
        {
            Assert.Greater(function.Count, 1);
            Assert.AreEqual(lowerBound, function[function.Count - 2].UpperBound);
            Assert.AreNotEqual(includeLowerBound, function[function.Count - 2].IncludeUpperBound);

            VerifyMaxValuePiece(function, expectedValue);
        }

        public static void VerifyMaxValuePiece(PiecewiseFunction function, double expectedValue)
        {
            Assert.AreEqual(double.MaxValue, function.Last().UpperBound);
            Assert.AreEqual(true, function.Last().IncludeUpperBound);
            Assert.AreEqual(expectedValue, function.Last().Value);
        }

        public static void VerifyConstantFunction(PiecewiseFunction function, double expectedConstantValue)
        {
            Assert.AreEqual(1, function.Count);
            Assert.AreEqual(double.MaxValue, function[0].UpperBound);
            Assert.AreEqual(true, function[0].IncludeUpperBound);
            Assert.AreEqual(expectedConstantValue, function[0].Value);
        }
        #endregion
    }

    [TestFixture]
    public class IsEquivalent 
    {
        public static IEnumerable<TestCaseData> PiecewiseFunctions()
        {
            var functions = new List<PiecewiseFunction>
            {
                TestHelpers.Zero,
                TestHelpers.StepUpIncludeRight,
                TestHelpers.StepUpIncludeLeft,
                TestHelpers.StepDownIncludeRight,
                TestHelpers.StepDownIncludeLeft,
                TestHelpers.Floor,
                TestHelpers.Ceiling,
                TestHelpers.Round,
                TestHelpers.RoundMidpointUp,
                TestHelpers.RoundMidpointToEven,
                TestHelpers.LessThanFive,
                TestHelpers.LessThanOrEqualToFive,
                TestHelpers.GreaterThanFive,
                TestHelpers.GreaterThanOrEqualToFive,
                TestHelpers.LessThanTwo,
                TestHelpers.LessThanOrEqualToTwo,
                TestHelpers.GreaterThanTwo,
                TestHelpers.GreaterThanOrEqualToTwo,
                TestHelpers.TwoIfLessThanTwo,
                TestHelpers.TwoIfLessThanOrEqualToTwo,
                TestHelpers.TwoIfGreaterThanTwo,
                TestHelpers.TwoIfGreaterThanOrEqualToTwo,
                TestHelpers.Even
            };
            foreach (var function in functions)
            {
                yield return new TestCaseData(function);
            }
        }
        [Test]
        public void ReturnsTrueForEmptyFunctionAndZeroValuedFunction()
        {
            var emptyFunction = new PiecewiseFunction();

            Assert.IsTrue(TestHelpers.Zero.IsEquivalent(emptyFunction));
            Assert.IsTrue(emptyFunction.IsEquivalent(TestHelpers.Zero));
        }
        [TestCaseSource(nameof(PiecewiseFunctions))]
        public void ReturnsTrueForCopiedFunction(PiecewiseFunction function)
        {
            var copiedFunction = new PiecewiseFunction(function);

            Assert.IsTrue(function.IsEquivalent(copiedFunction));
            Assert.IsTrue(copiedFunction.IsEquivalent(function));
        }

        [Test]
        public void ReturnsTrueForIdenticallyConstructedFunctions()
        {
            var firstFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { 20.125, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            var secondFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { 20.125, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            Assert.IsTrue(firstFunction.IsEquivalent(secondFunction));
            Assert.IsTrue(secondFunction.IsEquivalent(firstFunction));
        }

        [Test]
        [TestCase(10.0, true)]
        [TestCase(10.0, false)]
        [TestCase(11.125, true)]
        [TestCase(11.125, false)]
        [TestCase(35.7, true)]
        [TestCase(35.7, false)]
        public void ReturnsTrueForFunctionsWithBrokenUpPiecesButIdenticalValues(double breakPoint, bool includeBreakpoint)
        {
            var firstFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { breakPoint, includeBreakpoint, 2.5 },
                { 100.0, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            var secondFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { 100.0, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            Assert.IsTrue(firstFunction.IsEquivalent(secondFunction));
            Assert.IsTrue(secondFunction.IsEquivalent(firstFunction));
        }

        [Test]
        [TestCase(0.0, 1.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(1.0, -1.0)]
        [TestCase(-1.0, 1.0)]
        [TestCase(-1.0, 5.5)]
        [TestCase(-5.5, 2.5)]
        [TestCase(2.131, 2.13)]
        [TestCase(5.0, 1000.0)]
        public void ReturnsFalseForConstantFunctionsWithDifferentPieceValues(double firstValue, double secondValue)
        {
            var firstFunction = new PiecewiseFunction { { double.MaxValue, firstValue } };
            var secondFunction = new PiecewiseFunction { { double.MaxValue, secondValue } };

            Assert.IsFalse(firstFunction.IsEquivalent(secondFunction));
            Assert.IsFalse(secondFunction.IsEquivalent(firstFunction));
        }

        [Test]
        [TestCase(0.0, 1.0)]
        [TestCase(1.0, 0.0)]
        [TestCase(0.0, -1.0)]
        [TestCase(-1.0, 0.0)]
        [TestCase(1.0, -1.0)]
        [TestCase(-1.0, 1.0)]
        [TestCase(-1.0, 5.5)]
        [TestCase(-5.5, 2.5)]
        [TestCase(2.131, 2.13)]
        [TestCase(5.0, 1000.0)]
        public void ReturnsFalseForVariableFunctionsWithDifferentPieceValues(double firstValue, double secondValue)
        {
            var firstFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, firstValue },
                { 20.0, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            var secondFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, secondValue },
                { 20.0, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            Assert.IsFalse(firstFunction.IsEquivalent(secondFunction));
            Assert.IsFalse(secondFunction.IsEquivalent(firstFunction));
        }

        [Test]
        public void ReturnsFalseForVariableFunctionsWithDifferentIncludeRightValues()
        {
            var firstFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { 20.0, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            var secondFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, true, 1.0 },
                { 7.5, 1.5 },
                { 20.0, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            Assert.IsFalse(firstFunction.IsEquivalent(secondFunction));
            Assert.IsFalse(secondFunction.IsEquivalent(firstFunction));
        }

        [Test]
        [TestCase(10.0, 11.0)]
        [TestCase(10.0, 10.00001)]
        [TestCase(12.0, 1000.0)]
        [TestCase(25.32451, 30.125)]
        public void ReturnsFalseForVariableFunctionsWithDifferentEndpoints(double firstValue, double secondValue)
        {
            var firstFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { firstValue, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            var secondFunction = new PiecewiseFunction
            {
                { 0.0, 0.0 },
                { 5.0, false, 1.0 },
                { 7.5, 1.5 },
                { secondValue, false, 2.5 },
                { double.MaxValue, 0.0 }
            };

            Assert.IsFalse(firstFunction.IsEquivalent(secondFunction));
            Assert.IsFalse(secondFunction.IsEquivalent(firstFunction));
        }
    }

    [TestFixture]
    public class HasOverlap 
    {
        [Test]
        public void ReturnsFalseForTwoFunctionsThatAreNeverTrue()
        {
            var zero1 = new PiecewiseFunction { { double.MaxValue, 0.0 } };
            var zero2 = new PiecewiseFunction { { double.MaxValue, 0.0 } };

            Assert.IsFalse(PiecewiseFunction.HasOverlap(zero1, zero2));
        }

        [Test]
        public void ReturnsFalseIfOneFunctionIsAlwaysFalse()
        {
            var zero = new PiecewiseFunction { { double.MaxValue, 0.0 } };
            var one = new PiecewiseFunction { { double.MaxValue, 1.0 } };

            Assert.IsFalse(PiecewiseFunction.HasOverlap(zero, one));
            Assert.IsFalse(PiecewiseFunction.HasOverlap(one, zero));
        }

        [Test]
        public void ReturnsTrueIfIdenticalRangesAreTrue()
        {
            var first = new PiecewiseFunction { { 0.0, 0.0 }, { 2.0, 1.0 }, { double.MaxValue, 0.0 } };
            var second = new PiecewiseFunction { { 0.0, 0.0 }, { 2.0, 1.0 }, { double.MaxValue, 0.0 } };

            Assert.IsTrue(PiecewiseFunction.HasOverlap(first, second));
            Assert.IsTrue(PiecewiseFunction.HasOverlap(first, second));
        }

        [Test]
        public void ReturnsTrueIfNonIdenticalRangesShareARange()
        {
            var first = new PiecewiseFunction { { 0.0, 0.0 }, { 2.0, 1.0 }, { double.MaxValue, 0.0 } };
            var second = new PiecewiseFunction { { -1.0, 0.0 }, { 1.0, 1.0 }, { double.MaxValue, 0.0 } };

            Assert.IsTrue(PiecewiseFunction.HasOverlap(first, second));
            Assert.IsTrue(PiecewiseFunction.HasOverlap(first, second));
        }

        [Test]
        public void ReturnsTrueIfNonIdenticalRangesShareAnEndpoint()
        {
            var first = new PiecewiseFunction { { -1.0, false, 0.0 }, { double.MaxValue, 1.0 } };
            var second = new PiecewiseFunction { { -1.0, true, 1.0 }, { double.MaxValue, 0.0 } };

            Assert.IsTrue(PiecewiseFunction.HasOverlap(first, second));
            Assert.IsTrue(PiecewiseFunction.HasOverlap(first, second));
        }
    }

    [TestFixture]
    public class AddFunction 
    {
        [Test]
        [TestCase(-1.0, 1.0, null)]
        [TestCase(-1.0, 1.0, true)]
        [TestCase(-1.0, 1.0, false)]
        public void DoesNotAllowPiecesThatGoBackwards(double upperBound, double value, bool? includeUpperBound)
        {
            var function = new PiecewiseFunction { { 0.0, true, 0.0 } };

            if (includeUpperBound.HasValue)
            {
                Assert.Throws<ArgumentException>(()=>function.Add(upperBound, includeUpperBound.Value, value));
            }
            else
            {
                Assert.Throws<ArgumentException>(() => function.Add(upperBound, value));
            }
        }

        [Test]
        public void DoesNotAllowPiecesWithValueNaN()
        {
            Assert.Throws<ArgumentException>(() => new PiecewiseFunction { { 0.0, true, double.NaN } });
        }

        [Test]
        public void DoesNotAllowPiecesWithUpperBoundNaN()
        {
            Assert.Throws<ArgumentException>(() => new PiecewiseFunction { { double.NaN, true, 0.0 } });
        }

        [Test]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void DoesNotAllowPiecesWithInfiniteUpperBound(double value)
        {
            Assert.Throws<ArgumentException>(() => new PiecewiseFunction { { value, true, 0.0 } });
        }
    }

    [TestFixture]
    public class AddManyFunction 
    {
        [Test]
        public void HandlesEmptyInput()
        {
            var result = PiecewiseFunction.AddMany(new PiecewiseFunction[0]);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void AddsValuesForTwoFunctions()
        {
            var result = PiecewiseFunction.AddMany(new[] { TestHelpers.Floor, TestHelpers.Round });

            Assert.AreEqual(0.0, result.GetValue(0.25));
            Assert.AreEqual(1.0, result.GetValue(0.75));
            Assert.AreEqual(2.0, result.GetValue(1.25));
            Assert.AreEqual(3.0, result.GetValue(1.75));
            Assert.AreEqual(4.0, result.GetValue(2.25));
            Assert.AreEqual(5.0, result.GetValue(2.75));
            Assert.AreEqual(6.0, result.GetValue(3.25));
            Assert.AreEqual(7.0, result.GetValue(3.75));
            Assert.AreEqual(8.0, result.GetValue(4.25));
            Assert.AreEqual(9.0, result.GetValue(4.75));
            Assert.AreEqual(10.0, result.GetValue(5.25));
        }
    }

    public class GetValueFunction 
    {
        [Test]
        public void GetsTheRightValue()
        {
            var lessThanFive = new PiecewiseFunction { { 5.0, false, 1.0 } };
            var twoIfGreaterThanFive = new PiecewiseFunction { { 5.0, 0.0 }, { double.MaxValue, 2.0 } };

            Assert.AreEqual(1.0, lessThanFive.GetValue(0));
            Assert.AreEqual(1.0, lessThanFive.GetValue(2.5));
            Assert.AreEqual(1.0, lessThanFive.GetValue(4.999));
            Assert.AreEqual(0.0, lessThanFive.GetValue(5.0));
            Assert.AreEqual(0.0, lessThanFive.GetValue(1000.0));

            Assert.AreEqual(0.0, twoIfGreaterThanFive.GetValue(0));
            Assert.AreEqual(0.0, twoIfGreaterThanFive.GetValue(2.5));
            Assert.AreEqual(0.0, twoIfGreaterThanFive.GetValue(4.999));
            Assert.AreEqual(0.0, twoIfGreaterThanFive.GetValue(5.0));
            Assert.AreEqual(2.0, twoIfGreaterThanFive.GetValue(1000.0));
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void ThrowsArgumentExceptionForInvalidXValues(double value)
        {
            var lessThanFive = new PiecewiseFunction { { 5.0, false, 1.0 } };
            var six = new PiecewiseFunction { { double.MaxValue, 6.0 } };

            Assert.Throws<ArgumentException>(() => lessThanFive.GetValue(value));
            Assert.Throws<ArgumentException>(() => six.GetValue(value));
        }
    }

    [TestFixture]
    public class PlusOperator 
    {
        [Test]
        [TestCase(-1.0)]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(5.0)]
        [TestCase(1.41421356)]
        [TestCase(Math.PI)]
        public void GetsCorrectValue(double value)
        {
            var function = new PiecewiseFunction { { 3.0, 1.0 }, { 5.0, false, 2.0 }, { double.MaxValue, 1.0 } };

            var combined = function + value;

            Assert.AreEqual(1.0 + value, combined.GetValue(0.0));
            Assert.AreEqual(1.0 + value, combined.GetValue(2.5));
            Assert.AreEqual(1.0 + value, combined.GetValue(2.99999));
            Assert.AreEqual(1.0 + value, combined.GetValue(3.0));
            Assert.AreEqual(2.0 + value, combined.GetValue(3.00001));
            Assert.AreEqual(2.0 + value, combined.GetValue(4.0));
            Assert.AreEqual(2.0 + value, combined.GetValue(4.99999));
            Assert.AreEqual(1.0 + value, combined.GetValue(5.0));
            Assert.AreEqual(1.0 + value, combined.GetValue(5.00001));
            Assert.AreEqual(1.0 + value, combined.GetValue(10000.0));
            Assert.AreEqual(1.0 + value, combined.GetValue(double.MaxValue));
        }

        [Test]
        [TestCase(-20.0, true)]
        [TestCase(0.0, false)]
        [TestCase(3.0, true)]
        [TestCase(5.0, false)]
        [TestCase(20.0, true)]
        [TestCase(double.MaxValue, true)]
        public void GetsCorrectUpperBounds(double upperBound, bool includeUpperBound)
        {
            var function = new PiecewiseFunction { { -20.0, 1.0 }, { 0.0, false, 0.0 }, { 3.0, 1.0 }, { 5.0, false, 2.0 }, { 20.0, 1.0 }, { double.MaxValue, 0.0 } };

            var combined = function + 2.0;

            TestHelpers.VerifyUpperBound(combined, upperBound, includeUpperBound, false);
        }

        [Test]
        public void ExtendsFunctionToDoubleMaxValueIfNecessary()
        {
            var function = new PiecewiseFunction { { -20.0, 1.0 }, { 0.0, false, 0.0 }, { 3.0, 1.0 }, { 5.0, false, 2.0 }, { 20.0, 1.0 } };

            var combined = function + 2.0;

            TestHelpers.VerifyMaxValuePiece(combined, 2.0, 20.0, false);
        }
    }

    [TestFixture]
    public class PlusPiecewiseFunctionOperator 
    {
        [Test]
        [TestCase(double.MinValue, 1.0)]
        [TestCase(2.0, 1.0)]
        [TestCase(2.00001, 2.0)]
        [TestCase(3.5, 2.0)]
        [TestCase(4.99999, 2.0)]
        [TestCase(5.0, 1.0)]
        [TestCase(1000.0, 1.0)]
        [TestCase(double.MaxValue, 1.0)]
        public void GetsCorrectValues(double xValue, double yValue)
        {
            var function = TestHelpers.GreaterThanTwo + TestHelpers.LessThanFive;

            Assert.AreEqual(yValue, function.GetValue(xValue));
        }

        [Test]
        [TestCase(0.5, 6.0)]
        [TestCase(0.99999, 6.0)]
        [TestCase(1.0, 5.0)]
        [TestCase(1.00001, 6.0)]
        [TestCase(1.5, 6.0)]
        [TestCase(1.99999, 6.0)]
        [TestCase(2.0, 5.0)]
        [TestCase(2.00001, 6.0)]
        [TestCase(2.5, 6.0)]
        [TestCase(2.99999, 6.0)]
        [TestCase(3.0, 5.0)]
        [TestCase(3.00001, 6.0)]
        [TestCase(3.5, 6.0)]
        [TestCase(3.99999, 6.0)]
        [TestCase(4.0, 5.0)]
        [TestCase(4.00001, 6.0)]
        [TestCase(4.5, 6.0)]
        [TestCase(4.99999, 6.0)]
        [TestCase(5.0, 5.0)]
        [TestCase(5.00001, 0.0)]
        public void GetsCorrectValuesForMismatchedUpperBound(double xValue, double yValue)
        {
            var function = TestHelpers.StepDownIncludeLeft + TestHelpers.StepUpIncludeRight;

            Assert.AreEqual(yValue, function.GetValue(xValue));
        }

        [Test]
        public void HandlesMismatchedUpperBoundsAtTheEndOfAFunction()
        {
            // Both end at 5.0, but one includes it and the other does not
            var function = TestHelpers.LessThanFive + TestHelpers.LessThanOrEqualToFive;

            Assert.AreEqual(2.0, function.GetValue(4.99999));
            Assert.AreEqual(1.0, function.GetValue(5.0));
            Assert.AreEqual(0.0, function.GetValue(5.00001));

            TestHelpers.VerifyUpperBound(function, 5.0, false, false);
            TestHelpers.VerifyUpperBound(function, 5.0, true, true);

            function = TestHelpers.LessThanOrEqualToFive + TestHelpers.LessThanFive;

            TestHelpers.VerifyUpperBound(function, 5.0, false, false);
            TestHelpers.VerifyUpperBound(function, 5.0, true, true);

            Assert.AreEqual(2.0, function.GetValue(4.99999));
            Assert.AreEqual(1.0, function.GetValue(5.0));
            Assert.AreEqual(0.0, function.GetValue(5.00001));
        }

        [Test]
        public void ExtendsResultToDoubleMaxValue()
        {
            var function = TestHelpers.LessThanFive + TestHelpers.LessThanOrEqualToFive;

            TestHelpers.VerifyMaxValuePiece(function, 0.0, 5.0, false);
        }
    }

    [TestFixture]
    public class SubtractDoubleOperator 
    {
        [Test]
        [TestCase(-1.0)]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(5.0)]
        [TestCase(1.41421356)]
        [TestCase(Math.PI)]
        public void GetsCorrectValue(double value)
        {
            var function = new PiecewiseFunction { { 3.0, 1.0 }, { 5.0, false, 2.0 }, { double.MaxValue, 1.0 } };

            var combined = function - value;

            Assert.AreEqual(1.0 - value, combined.GetValue(0.0));
            Assert.AreEqual(1.0 - value, combined.GetValue(2.5));
            Assert.AreEqual(1.0 - value, combined.GetValue(2.99999));
            Assert.AreEqual(1.0 - value, combined.GetValue(3.0));
            Assert.AreEqual(2.0 - value, combined.GetValue(3.00001));
            Assert.AreEqual(2.0 - value, combined.GetValue(4.0));
            Assert.AreEqual(2.0 - value, combined.GetValue(4.99999));
            Assert.AreEqual(1.0 - value, combined.GetValue(5.0));
            Assert.AreEqual(1.0 - value, combined.GetValue(5.00001));
            Assert.AreEqual(1.0 - value, combined.GetValue(10000.0));
            Assert.AreEqual(1.0 - value, combined.GetValue(double.MaxValue));
        }

        [Test]
        [TestCase(-20.0, true)]
        [TestCase(0.0, false)]
        [TestCase(3.0, true)]
        [TestCase(5.0, false)]
        [TestCase(20.0, true)]
        [TestCase(double.MaxValue, true)]
        public void GetsCorrectUpperBounds(double upperBound, bool includeUpperBound)
        {
            var function = new PiecewiseFunction { { -20.0, 1.0 }, { 0.0, false, 0.0 }, { 3.0, 1.0 }, { 5.0, false, 2.0 }, { 20.0, 1.0 }, { double.MaxValue, 0.0 } };

            var combined = function - 2.0;

            TestHelpers.VerifyUpperBound(combined, upperBound, includeUpperBound, false);
        }

        [Test]
        public void ExtendsFunctionToDoubleMaxValueIfNecessary()
        {
            var function = new PiecewiseFunction { { -20.0, 1.0 }, { 0.0, false, 0.0 }, { 3.0, 1.0 }, { 5.0, false, 2.0 }, { 20.0, 1.0 } };

            var combined = function - 2.0;

            TestHelpers.VerifyMaxValuePiece(combined, -2.0, 20.0, false);
        }
    }

    [TestFixture]
    public class MultiplyDoubleOperator 
    {
        [Test]
        [TestCase(-1.0)]
        [TestCase(0.0)]
        [TestCase(1.0)]
        [TestCase(5.0)]
        [TestCase(1.41421356)]
        [TestCase(Math.PI)]
        public void GetsCorrectValue(double value)
        {
            var function = new PiecewiseFunction { { 3.0, 1.0 }, { 5.0, false, 2.0 }, { double.MaxValue, 1.0 } };

            var combined = function * value;

            Assert.AreEqual(1.0 * value, combined.GetValue(0.0));
            Assert.AreEqual(1.0 * value, combined.GetValue(2.5));
            Assert.AreEqual(1.0 * value, combined.GetValue(2.99999));
            Assert.AreEqual(1.0 * value, combined.GetValue(3.0));
            Assert.AreEqual(2.0 * value, combined.GetValue(3.00001));
            Assert.AreEqual(2.0 * value, combined.GetValue(4.0));
            Assert.AreEqual(2.0 * value, combined.GetValue(4.99999));
            Assert.AreEqual(1.0 * value, combined.GetValue(5.0));
            Assert.AreEqual(1.0 * value, combined.GetValue(5.00001));
            Assert.AreEqual(1.0 * value, combined.GetValue(10000.0));
            Assert.AreEqual(1.0 * value, combined.GetValue(double.MaxValue));
        }

        [Test]
        [TestCase(-20.0, true)]
        [TestCase(0.0, false)]
        [TestCase(3.0, true)]
        [TestCase(5.0, false)]
        [TestCase(20.0, true)]
        [TestCase(double.MaxValue, true)]
        public void GetsCorrectUpperBounds(double upperBound, bool includeUpperBound)
        {
            var function = new PiecewiseFunction { { -20.0, 1.0 }, { 0.0, false, 0.0 }, { 3.0, 1.0 }, { 5.0, false, 2.0 }, { 20.0, 1.0 }, { double.MaxValue, 0.0 } };

            var combined = function * 2.0;

            TestHelpers.VerifyUpperBound(combined, upperBound, includeUpperBound, false);
        }

        [Test]
        public void ExtendsFunctionToDoubleMaxValueIfNecessary()
        {
            var function = new PiecewiseFunction { { -20.0, 1.0 }, { 0.0, false, 0.0 }, { 3.0, 1.0 }, { 5.0, false, 2.0 }, { 20.0, 1.0 } };

            var combined = function * 2.0;

            TestHelpers.VerifyMaxValuePiece(combined, 0.0, 20.0, false);
        }
    }

    public abstract class InequalityTest 
    {
        public abstract PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second);

        public abstract bool GreaterThan { get; }

        public abstract bool LessThan { get; }

        public abstract bool EqualTo { get; }

        [Test]
        public void GetsCorrectValuesWithinPieces()
        {
            var function = this.RunOperation(TestHelpers.LessThanOrEqualToFive, TestHelpers.GreaterThanTwo);

            Assert.AreEqual(this.GreaterThan ? 1.0 : 0.0, function.GetValue(1.0));
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, function.GetValue(3.5));
            Assert.AreEqual(this.LessThan ? 1.0 : 0.0, function.GetValue(6.0));

            var reverseFunction = this.RunOperation(TestHelpers.GreaterThanTwo, TestHelpers.LessThanOrEqualToFive);

            Assert.AreEqual(this.LessThan ? 1.0 : 0.0, reverseFunction.GetValue(1.0));
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, reverseFunction.GetValue(3.5));
            Assert.AreEqual(this.GreaterThan ? 1.0 : 0.0, reverseFunction.GetValue(6.0));
        }

        [Test]
        public void GetsCorrectValueWhenOneFunctionEndsAndIncludesRight()
        {
            var function = this.RunOperation(TestHelpers.LessThanOrEqualToFive, TestHelpers.GreaterThanTwo);

            Assert.AreEqual(this.GreaterThan ? 1.0 : 0.0, function.GetValue(2.0));
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, function.GetValue(5.0));

            var reverseFunction = this.RunOperation(TestHelpers.GreaterThanTwo, TestHelpers.LessThanOrEqualToFive);

            Assert.AreEqual(this.LessThan ? 1.0 : 0.0, reverseFunction.GetValue(2.0));
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, reverseFunction.GetValue(5.0));
        }

        [Test]
        public void GetsCorrectValueWhenOneFunctionEndsAndDoesNotIncludeRight()
        {
            var function = this.RunOperation(TestHelpers.LessThanFive, TestHelpers.GreaterThanOrEqualToTwo);
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, function.GetValue(2.0));

            var greaterThanOrEqualsFunction = this.RunOperation(TestHelpers.GreaterThanOrEqualToTwo, TestHelpers.GreaterThanOrEqualToFive);
            Assert.AreEqual(this.GreaterThan ? 1.0 : 0.0, greaterThanOrEqualsFunction.GetValue(2.0));

            var lessThanFunction = this.RunOperation(TestHelpers.LessThanTwo, TestHelpers.LessThanFive);
            Assert.AreEqual(this.LessThan ? 1.0 : 0.0, lessThanFunction.GetValue(2.0));
        }

        [Test]
        public void GetsCorrectValueWhenBothFunctionsEndAndIncludeRight()
        {
            var sameValueFunctions = this.RunOperation(TestHelpers.LessThanOrEqualToTwo, TestHelpers.LessThanOrEqualToTwo);
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, sameValueFunctions.GetValue(2.0));

            var firstGreater = this.RunOperation(TestHelpers.TwoIfLessThanOrEqualToTwo, TestHelpers.LessThanOrEqualToTwo);
            Assert.AreEqual(this.GreaterThan ? 1.0 : 0.0, firstGreater.GetValue(2.0));

            var firstLesser = this.RunOperation(TestHelpers.LessThanOrEqualToTwo, TestHelpers.TwoIfLessThanOrEqualToTwo);
            Assert.AreEqual(this.LessThan ? 1.0 : 0.0, firstLesser.GetValue(2.0));
        }

        [Test]
        public void GetsCorrectValueWhenBothFunctionsEndAndDoNotIncludeRight()
        {
            var sameValueFunctions = this.RunOperation(TestHelpers.GreaterThanOrEqualToTwo, TestHelpers.Even);
            Assert.AreEqual(this.EqualTo ? 1.0 : 0.0, sameValueFunctions.GetValue(2.0));

            var firstGreater = this.RunOperation(TestHelpers.TwoIfGreaterThanOrEqualToTwo, TestHelpers.GreaterThanOrEqualToTwo);
            Assert.AreEqual(this.GreaterThan ? 1.0 : 0.0, firstGreater.GetValue(2.0));

            var firstLesser = this.RunOperation(TestHelpers.GreaterThanOrEqualToTwo, TestHelpers.TwoIfGreaterThanOrEqualToTwo);
            Assert.AreEqual(this.LessThan ? 1.0 : 0.0, firstLesser.GetValue(2.0));
        }

        [Test]
        public void ExtendsFirstFunctionToDoubleMaxValue()
        {
            var negativeOneIfGreaterThanFive = TestHelpers.GreaterThanFive * -1.0;
            var greaterThanFunction = this.RunOperation(TestHelpers.LessThanTwo, negativeOneIfGreaterThanFive);
            TestHelpers.VerifyMaxValuePiece(greaterThanFunction, this.GreaterThan ? 1.0 : 0.0);

            var lessThanFunction = this.RunOperation(TestHelpers.LessThanOrEqualToTwo, TestHelpers.GreaterThanFive);
            TestHelpers.VerifyMaxValuePiece(lessThanFunction, this.LessThan ? 1.0 : 0.0);

            var equalToFunction = this.RunOperation(TestHelpers.LessThanTwo, TestHelpers.Zero);
            TestHelpers.VerifyMaxValuePiece(equalToFunction, this.EqualTo ? 1.0 : 0.0);
        }

        [Test]
        public void ExtendsSecondFunctionToDoubleMaxValue()
        {
            var greaterThanFunction = this.RunOperation(TestHelpers.GreaterThanFive, TestHelpers.LessThanTwo);
            TestHelpers.VerifyMaxValuePiece(greaterThanFunction, this.GreaterThan ? 1.0 : 0.0);

            var negativeOneIfGreaterThanFive = TestHelpers.GreaterThanFive * -1.0;
            var lessThanFunction = this.RunOperation(negativeOneIfGreaterThanFive, TestHelpers.LessThanTwo);
            TestHelpers.VerifyMaxValuePiece(lessThanFunction, this.LessThan ? 1.0 : 0.0);

            var equalToFunction = this.RunOperation(TestHelpers.Zero, TestHelpers.LessThanTwo);
            TestHelpers.VerifyMaxValuePiece(equalToFunction, this.EqualTo ? 1.0 : 0.0);
        }

        public void ExtendsBothFunctionsToDoubleMaxValue()
        {
            var function = this.RunOperation(TestHelpers.LessThanFive, TestHelpers.LessThanTwo);

            TestHelpers.VerifyMaxValuePiece(function, this.EqualTo ? 1.0 : 0.0, 5.0, true);
        }
    }

    [TestFixture]
    public class GreaterThanPiecewiseFunctionOperator : InequalityTest
    {
        public override PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second) => first > second;

        public override bool GreaterThan => true;

        public override bool LessThan => false;

        public override bool EqualTo => false;
    }

    [TestFixture]
    public class GreaterThanOrEqualsPiecewiseFunctionOperator : InequalityTest
    {
        public override PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second) => first >= second;

        public override bool GreaterThan => true;

        public override bool LessThan => false;

        public override bool EqualTo => true;
    }

    [TestFixture]
    public class LessThanPiecewiseFunctionOperator : InequalityTest
    {
        public override PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second) => first < second;

        public override bool GreaterThan => false;

        public override bool LessThan => true;

        public override bool EqualTo => false;
    }

    [TestFixture]
    public class LessThanOrEqualsPiecewiseFunctionOperator : InequalityTest
    {
        public override PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second) => first <= second;

        public override bool GreaterThan => false;

        public override bool LessThan => true;

        public override bool EqualTo => true;
    }

    [TestFixture]
    public class EqualsPiecewiseFunctionOperator : InequalityTest
    {
        public override PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second) => PiecewiseFunction.EqualTo(first, second);

        public override bool GreaterThan => false;

        public override bool LessThan => false;

        public override bool EqualTo => true;
    }

    [TestFixture]
    public class NotEqualsPiecewiseFunctionOperator : InequalityTest
    {
        public override PiecewiseFunction RunOperation(PiecewiseFunction first, PiecewiseFunction second) => PiecewiseFunction.NotEqualTo(first, second);

        public override bool GreaterThan => true;

        public override bool LessThan => true;

        public override bool EqualTo => false;
    }

    [TestFixture]
    public class GreaterThanLinearFunctionOperator 
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

            var result = stepUpIncludeUpper > line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = stepDownIncludeUpper > line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 2.0);

            var result = stepUpIncludeUpper > line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = stepDownIncludeUpper > line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesBeforePositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = this.constantOne > line;

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

            var result = this.constantOne > line;

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

            var result = this.stepUpIncludeUpper > yEqualsX;

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

            var result = this.stepDownIncludeLower > zeroPlusX;

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

            var result = this.stepUpIncludeUpper > twoMinusX;

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

            var result = this.stepDownIncludeLower > twoMinusX;

            Assert.AreEqual(1.0, result.GetValue(0.001));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class GreaterThanOrEqualToLinearFunctionOperator 
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

            var result = this.stepUpIncludeUpper >= line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = this.stepDownIncludeUpper >= line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 2.5);

            var result = this.stepUpIncludeUpper >= line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = this.stepDownIncludeUpper >= line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesBeforeAndWhenPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = this.constantOne >= line;

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

            var result = this.constantOne >= line;

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

            var result = this.stepDownIncludeUpper >= onePlusX;

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

            var result = this.stepUpIncludeLower >= onePlusX;

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

            var result = this.stepDownIncludeUpper >= threeMinusX;

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

            var result = this.stepUpIncludeLower >= threeMinusX;

            Assert.AreEqual(0.0, result.GetValue(0.001));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class LessThanLinearFunctionOperator 
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

            var result = this.stepUpIncludeUpper < line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderPositiveLine()
        {
            var line = new LinearFunction(1.0, 4.0);

            var result = this.stepDownIncludeUpper < line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = this.stepUpIncludeUpper < line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = this.stepDownIncludeUpper < line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesBeforeNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 2.0);

            var result = this.constantOne < line;

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

            var result = this.constantOne < line;

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

            var result = this.stepDownIncludeUpper < onePlusX;

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

            var result = this.stepDownIncludeLower < onePlusX;

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

            var result = this.stepUpIncludeUpper < threeMinusX;

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

            var result = this.stepUpIncludeLower < threeMinusX;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class LessThanOrEqualToLinearFunctionOperator 
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

            var result = this.stepUpIncludeUpper <= line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = this.stepDownIncludeUpper <= line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 2.5);

            var result = this.stepUpIncludeUpper <= line;

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = this.stepDownIncludeUpper <= line;

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesAfterAndWhenPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = this.constantOne <= line;

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

            var result = this.constantOne <= line;

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

            var result = this.stepUpIncludeUpper <= onePlusX;

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

            var result = this.stepDownIncludeLower <= zeroPlusX;

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

            var result = this.stepUpIncludeUpper <= twoMinusX;

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

            var result = this.stepDownIncludeLower <= twoMinusX;

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
    public class EqualToLinearFunctionOperator 
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

            var result = PiecewiseFunction.EqualTo(this.stepUpIncludeUpper, line);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = PiecewiseFunction.EqualTo(this.stepDownIncludeUpper, line);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = PiecewiseFunction.EqualTo(this.stepDownIncludeUpper, line);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = PiecewiseFunction.EqualTo(this.stepDownIncludeUpper, line);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesWhenPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = PiecewiseFunction.EqualTo(this.constantOne, line);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.99999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(2.0));
            Assert.AreEqual(0.0, result.GetValue(1000.0));
            Assert.AreEqual(0.0, result.GetValue(double.MaxValue));
        }

        [Test]
        public void IncludesWhenNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = PiecewiseFunction.EqualTo(this.constantOne, line);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(1.5));
            Assert.AreEqual(0.0, result.GetValue(2.99999));
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

            var result = PiecewiseFunction.EqualTo(this.stepUpIncludeUpper, onePlusX);

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

            var result = PiecewiseFunction.EqualTo(this.stepDownIncludeLower, zeroPlusX);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedUpperBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = PiecewiseFunction.EqualTo(this.stepUpIncludeUpper, twoMinusX);

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
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = PiecewiseFunction.EqualTo(this.stepDownIncludeLower, twoMinusX);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(0.0, result.GetValue(0.001));
            Assert.AreEqual(0.0, result.GetValue(0.5));
            Assert.AreEqual(0.0, result.GetValue(0.999));
            Assert.AreEqual(1.0, result.GetValue(1.0));
            Assert.AreEqual(0.0, result.GetValue(1.0001));
            Assert.AreEqual(0.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesIntersectionAtPieceThatSpansASinglePoint()
        {
            var linear = new LinearFunction(25, 0);
            var pwf = new PiecewiseFunction
            {
                { 0.8, false, 2 },  // min <  x <  .8 : 2
                { 0.8, 20 },        // .8  <= x <= .8 : 20
            };

            PiecewiseFunction result = null;
            Assert.DoesNotThrow(() => result = PiecewiseFunction.EqualTo(pwf, linear));
            Assert.AreEqual(0.0, result.GetValue(0.8 - .000005));
            Assert.AreEqual(1.0, result.GetValue(0.8));
            Assert.AreEqual(0.0, result.GetValue(0.8 + .000005));
        }
    }

    [TestFixture]
    public class NotEqualToLinearFunctionOperator 
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

            var result = PiecewiseFunction.NotEqualTo(this.stepUpIncludeUpper, line);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverPositiveLine()
        {
            var line = new LinearFunction(1.0, -2.0);

            var result = PiecewiseFunction.NotEqualTo(this.stepDownIncludeUpper, line);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyUnderNegativeLine()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = PiecewiseFunction.NotEqualTo(this.stepDownIncludeUpper, line);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void IncludesPieceEntirelyOverNegativeLine()
        {
            var line = new LinearFunction(-1.0, -2.0);

            var result = PiecewiseFunction.NotEqualTo(this.stepDownIncludeUpper, line);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(1.0));
        }

        [Test]
        public void ExcludesWhenPositiveLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(1.0, 0.0);

            var result = PiecewiseFunction.NotEqualTo(this.constantOne, line);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.99999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(2.0));
            Assert.AreEqual(1.0, result.GetValue(1000.0));
            Assert.AreEqual(1.0, result.GetValue(double.MaxValue));
        }

        [Test]
        public void ExcludesWhenNegativeLineIntersectsMiddleOfPiece()
        {
            var line = new LinearFunction(-1.0, 4.0);

            var result = PiecewiseFunction.NotEqualTo(this.constantOne, line);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(1.5));
            Assert.AreEqual(1.0, result.GetValue(2.99999));
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
            var onePlusX = new LinearFunction(1.0, 0.0);

            var result = PiecewiseFunction.NotEqualTo(this.stepUpIncludeUpper, onePlusX);

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

            var result = PiecewiseFunction.NotEqualTo(this.stepDownIncludeLower, zeroPlusX);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedUpperBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = PiecewiseFunction.NotEqualTo(this.stepUpIncludeUpper, twoMinusX);

            Assert.AreEqual(1.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }

        [Test]
        public void HandlesNegativeLinearFunctionAtIncludedLowerBound()
        {
            var twoMinusX = new LinearFunction(-1.0, 2.0);

            var result = PiecewiseFunction.NotEqualTo(this.stepDownIncludeLower, twoMinusX);

            Assert.AreEqual(0.0, result.GetValue(0.0));
            Assert.AreEqual(1.0, result.GetValue(0.001));
            Assert.AreEqual(1.0, result.GetValue(0.5));
            Assert.AreEqual(1.0, result.GetValue(0.999));
            Assert.AreEqual(0.0, result.GetValue(1.0));
            Assert.AreEqual(1.0, result.GetValue(1.0001));
            Assert.AreEqual(1.0, result.GetValue(1.5));
        }
    }

    [TestFixture]
    public class HighestNonZeroPoint
    {
        [Test]
        public void GetsUpperBoundOfSingleNonZeroPiece()
        {
            var function = new PiecewiseFunction { { 100.0, 1.0 }, { double.MaxValue, 0.0 } };

            Assert.AreEqual(100.0, function.HighestNonZeroPoint());
        }

        [Test]
        public void GetsUpperBoundOfHigherOfTwoAdjacentNonZeroPieces()
        {
            var function = new PiecewiseFunction { { 5.0, 0.0 }, { 10.0, 2.0 }, { 15.0, 1.0 } };

            Assert.AreEqual(15.0, function.HighestNonZeroPoint());
        }

        [Test]
        public void GetsUpperBoundOfHigherOfTwoNonAdjacentNonZeroPieces()
        {
            var function = new PiecewiseFunction { { 10.0, 1.0 }, { 20.0, 0.0 }, { 30.0, 1.0 } };

            Assert.AreEqual(30.0, function.HighestNonZeroPoint());
        }

        [Test]
        public void GetsSinglePointWithZeroWidth()
        {
            var function = new PiecewiseFunction { { 100.0, false, 0.0 }, { 100.0, true, 1.0 }, { double.MaxValue, 0.0 } };
            Assert.AreEqual(100.0, function.HighestNonZeroPoint());
        }

        [Test]
        [TestCase(double.MaxValue, true)]
        [TestCase(double.MaxValue, false)]
        [TestCase(0.0, true)]
        [TestCase(0.0, false)]
        public void GetsNotANumberForAFunctionThatIsAlwaysZero(double maxValue, bool includeRight)
        {
            var zeroFunction = new PiecewiseFunction { { maxValue, true, 0.0 } };

            var highestPoint = zeroFunction.HighestNonZeroPoint();

            Assert.IsNaN(highestPoint);
        }

        [Test]
        public void GetsNotANumberForEmptyFunction()
        {
            var blankFunction = new PiecewiseFunction();

            var highestPoint = blankFunction.HighestNonZeroPoint();

            Assert.IsNaN(highestPoint);
        }
    }

    [TestFixture]
    public class HighestZeroPoint
    {
        [Test]
        public void GetsUpperBoundOfSingleZeroPiece()
        {
            var function = new PiecewiseFunction { { 100.0, 0.0 }, { double.MaxValue, 1.0 } };

            Assert.AreEqual(100.0, function.HighestZeroPoint());
        }

        [Test]
        public void GetsUpperBoundOfHigherOfTwoAdjacentZeroPieces()
        {
            var function = new PiecewiseFunction { { 5.0, 0.0 }, { 10.0, 0.0 }, { double.MaxValue, 1.0 } };

            Assert.AreEqual(10.0, function.HighestZeroPoint());
        }

        [Test]
        public void GetsUpperBoundOfHigherOfTwoNonAdjacentZeroPieces()
        {
            var function = new PiecewiseFunction { { 10.0, 0.0 }, { 20.0, 1.0 }, { 30.0, 0.0 }, { double.MaxValue, 1.0 } };

            Assert.AreEqual(30.0, function.HighestZeroPoint());
        }

        [Test]
        public void GetsSinglePointWithZeroWidth()
        {
            var function = new PiecewiseFunction { { 100.0, false, 1.0 }, { 100.0, true, 0.0 }, { double.MaxValue, 1.0 } };
            Assert.AreEqual(100.0, function.HighestZeroPoint());
        }

        [Test]
        [TestCase(double.MaxValue, true)]
        [TestCase(double.MaxValue, false)]
        [TestCase(0.0, true)]
        [TestCase(0.0, false)]
        public void GetsDoubleMaxValueForAFunctionThatIsAlwaysZero(double maxValue, bool includeRight)
        {
            var zeroFunction = new PiecewiseFunction { { maxValue, true, 0.0 } };

            var highestPoint = zeroFunction.HighestZeroPoint();

            Assert.AreEqual(double.MaxValue, highestPoint);
        }

        [Test]
        public void GetsDoubleMaxValueForEmptyFunction()
        {
            var blankFunction = new PiecewiseFunction();

            var highestPoint = blankFunction.HighestZeroPoint();

            Assert.AreEqual(double.MaxValue, highestPoint);
        }

        [Test]
        public void GetsNotANumberForAFunctionThatIsNeverZero()
        {
            var oneFunction = new PiecewiseFunction { { double.MaxValue, true, 1.0 } };

            var highestPoint = oneFunction.HighestZeroPoint();

            Assert.IsNaN(highestPoint);
        }
    }

    [TestFixture]
    public class LowestNonZeroPoint
    {
        [Test]
        public void GetsLowerBoundOfSingleNonZeroPiece()
        {
            var function = new PiecewiseFunction { { 5.0, 0.0 }, { 10.0, 1.0 } };

            Assert.AreEqual(5.0, function.LowestNonZeroPoint());
        }

        [Test]
        public void GetsLowerBoundOfLowerOfTwoAdjacentNonZeroPieces()
        {
            var function = new PiecewiseFunction { { 10.0, 0.0 }, { 20.0, 1.0 }, { 30.0, 1.0 } };

            Assert.AreEqual(10.0, function.LowestNonZeroPoint());
        }

        [Test]
        public void GetsLowerBoundOfLowerOfTwoNonAdjacentNonZeroPieces()
        {
            var function = new PiecewiseFunction { { 50.0, 0.0 }, { 75.0, 1.0 }, { 125.0, 0.0 }, { 200.0, 1.0 } };

            Assert.AreEqual(50.0, function.LowestNonZeroPoint());
        }

        [Test]
        public void GetsSinglePointWithZeroWidth()
        {
            var function = new PiecewiseFunction { { 100.0, false, 0.0 }, { 100.0, true, 1.0 }, { double.MaxValue, 0.0 } };
            Assert.AreEqual(100.0, function.LowestNonZeroPoint());
        }

        [Test]
        [TestCase(double.MaxValue, true)]
        [TestCase(double.MaxValue, false)]
        [TestCase(0.0, true)]
        [TestCase(0.0, false)]
        public void GetsNotANumberForAFunctionThatIsAlwaysZero(double maxValue, bool includeRight)
        {
            var zeroFunction = new PiecewiseFunction { { maxValue, true, 0.0 } };

            var lowestPoint = zeroFunction.LowestNonZeroPoint();

            Assert.IsNaN(lowestPoint);
        }

        [Test]
        public void GetsNotANumberForEmptyFunction()
        {
            var blankFunction = new PiecewiseFunction();

            var lowestPoint = blankFunction.LowestNonZeroPoint();

            Assert.IsNaN(lowestPoint);
        }
    }

    [TestFixture]
    public class LowestZeroPoint
    {
        [Test]
        public void GetsLowerBoundOfSingleZeroPiece()
        {
            var function = new PiecewiseFunction { { 5.0, 1.0 }, { 10.0, 0.0 } };

            Assert.AreEqual(5.0, function.LowestZeroPoint());
        }

        [Test]
        public void GetsLowerBoundOfLowerOfTwoAdjacentZeroPieces()
        {
            var function = new PiecewiseFunction { { 10.0, 1.0 }, { 20.0, 0.0 }, { 30.0, 0.0 } };

            Assert.AreEqual(10.0, function.LowestZeroPoint());
        }

        [Test]
        public void GetsLowerBoundOfLowerOfTwoNonAdjacentZeroPieces()
        {
            var function = new PiecewiseFunction { { 50.0, 1.0 }, { 75.0, 0.0 }, { 125.0, 1.0 }, { 200.0, 0.0 } };

            Assert.AreEqual(50.0, function.LowestZeroPoint());
        }

        [Test]
        public void GetsSinglePointWithZeroWidth()
        {
            var function = new PiecewiseFunction { { 100.0, false, 1.0 }, { 100.0, true, 0.0 }, { double.MaxValue, 1.0 } };
            Assert.AreEqual(100.0, function.LowestZeroPoint());
        }

        [Test]
        public void GetsNotANumberForAFunctionThatIsNeverZero()
        {
            var zeroFunction = new PiecewiseFunction { { double.MaxValue, true, 1.0 } };

            var lowestPoint = zeroFunction.LowestZeroPoint();

            Assert.IsNaN(lowestPoint);
        }

        [Test]
        public void GetsMinDoubleValueForEmptyFunction()
        {
            var blankFunction = new PiecewiseFunction();

            var lowestPoint = blankFunction.LowestZeroPoint();

            Assert.AreEqual(double.MinValue, lowestPoint);
        }

        [Test]
        public void GetsUpperBoundOfLastPieceIfNoneIsZeroButFunctionIsNotDefinedToDoubleMaxValue()
        {
            var incompleteFunction = new PiecewiseFunction { { 0.0, 1.0 }, { 10000.0, 2.0 } };

            var lowestPoint = incompleteFunction.LowestZeroPoint();

            Assert.AreEqual(10000.0, lowestPoint);
        }
    }
}
