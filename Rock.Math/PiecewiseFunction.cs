namespace Rock.Math
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a function that can be expressed by a set of pieces, where each piece has a constant value and is valid for a specific range.
    /// Functions are considered to be defined between double.MinValue and double.MaxValue.
    /// If the pieces of the function do not cover the full range, the value is assumed to be 0.0 between the end of the final piece and double.MaxValue.
    /// </summary>
    [DataContract]
    public class PiecewiseFunction : IReadOnlyList<PiecewiseFunction.Piece>
    {
        private readonly List<Piece> pieces;

        #region Helper Functions

        private static readonly Func<Piece, double, Piece> MultiplyByConstant =
            (p, d) => new Piece { UpperBound = p.UpperBound, IncludeUpperBound = p.IncludeUpperBound, Value = p.Value * d };

        private static readonly Func<Piece, double, Piece> DivideByConstant =
            (p, d) => new Piece { UpperBound = p.UpperBound, IncludeUpperBound = p.IncludeUpperBound, Value = p.Value / d };

        private static readonly Func<Piece, double, Piece> AddConstant =
            (p, d) => new Piece { UpperBound = p.UpperBound, IncludeUpperBound = p.IncludeUpperBound, Value = p.Value + d };

        private static readonly Func<Piece, double, Piece> SubtractConstant =
            (p, d) => new Piece { UpperBound = p.UpperBound, IncludeUpperBound = p.IncludeUpperBound, Value = p.Value - d };

        private static readonly Func<Piece, Piece> NotFunction =
            p => new Piece { UpperBound = p.UpperBound, IncludeUpperBound = p.IncludeUpperBound, Value = p.Value.IsApproximatelyZero() ? 1.0 : 0.0 };

        private static readonly Func<object, double, double> IdentityFunction = (c, v) => v;

        private static readonly Func<object, double, double> NegationFunction = (c, v) => -1.0 * v;

        private static readonly Func<object, double, double> ZeroFunction = (c, v) => 0.0;

        private static readonly Func<object, double, double> IsTrueFunction = (c, v) => v.IsApproximatelyZero() ? 0.0 : 1.0;

        private static readonly Func<object, double, double> IsFalseFunction = (c, v) => v.IsApproximatelyZero() ? 1.0 : 0.0;

        private static readonly Func<object, double, double> IsGreaterThanZero = (c, v) => v.IsApproximatelyZero() ? 0.0 : (v > 0.0 ? 1.0 : 0.0);

        private static readonly Func<object, double, double> IsLessThanZero = (c, v) => v.IsApproximatelyZero() ? 0.0 : (v < 0.0 ? 1.0 : 0.0);

        private static readonly Func<object, double, double> IsGreaterThanOrEqualToZero = (c, v) => v.IsApproximatelyZero() ? 1.0 : (v > 0.0 ? 1.0 : 0.0);

        private static readonly Func<object, double, double> IsLessThanOrEqualToZero = (c, v) => v.IsApproximatelyZero() ? 1.0 : (v < 0.0 ? 1.0 : 0.0);

        private static readonly Func<object, double, double> ZeroOrGreater = (c, v) => v > 0.0 ? v : 0.0;

        private static readonly Func<object, double, double> ZeroOrLess = (c, v) => v < 0.0 ? v : 0.0;

        private static readonly Func<object, double, double, double> MultiplyPieces = (c, v1, v2) => v1 * v2;

        private static readonly Func<object, double, double, double> DividePieces = (c, v1, v2) => v1 / v2;

        private static readonly Func<object, double, double, double> AddPieces = (c, v1, v2) => v1 + v2;

        private static readonly Func<object, double, double, double> SubtractPieces = (c, v1, v2) => v1 - v2;

        private static readonly Func<object, double, double, double> GreaterOf = (c, v1, v2) => v1 > v2 ? v1 : v2;

        private static readonly Func<object, double, double, double> LesserOf = (c, v1, v2) => v1 < v2 ? v1 : v2;

        private static readonly Func<object, double, double, double> GreaterThan =
            (c, v1, v2) => v1.ApproximatelyEquals(v2) ? 0.0 : (v1 > v2 ? 1.0 : 0.0);

        private static readonly Func<object, double, double, double> LessThan =
            (c, v1, v2) => v1.ApproximatelyEquals(v2) ? 0.0 : (v1 < v2 ? 1.0 : 0.0);

        private static readonly Func<object, double, double, double> GreaterThanOrEquals =
            (c, v1, v2) => v1.ApproximatelyEquals(v2) ? 1.0 : (v1 > v2 ? 1.0 : 0.0);

        private static readonly Func<object, double, double, double> LessThanOrEquals =
            (c, v1, v2) => v1.ApproximatelyEquals(v2) ? 1.0 : (v1 < v2 ? 1.0 : 0.0);

        private static readonly Func<object, double, double, double> IsEqualTo = (c, v1, v2) => v1.ApproximatelyEquals(v2) ? 1.0 : 0.0;

        private static readonly Func<object, double, double, double> IsNotEqualTo = (c, v1, v2) => v1.ApproximatelyEquals(v2) ? 0.0 : 1.0;

        private static readonly Func<object, double, double, double> PiecewiseAnd =
            (c, v1, v2) => v1.IsApproximatelyZero() || v2.IsApproximatelyZero() ? 0.0 : 1.0;

        private static readonly Func<object, double, double, double> PiecewiseXOr =
            (c, v1, v2) => v1.IsApproximatelyZero() ^ v2.IsApproximatelyZero() ? 1.0 : 0.0;

        private static readonly Func<object, double, double, double> PiecewiseOr =
            (c, v1, v2) => (!v1.IsApproximatelyZero() || !v2.IsApproximatelyZero()) ? 1.0 : 0.0;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecewiseFunction"/> class. Sets the lower bound of the first piece and therefore the entire function to 0.0.
        /// </summary>
        public PiecewiseFunction()
        {
            this.pieces = new List<Piece>();
        }

        private PiecewiseFunction(List<Piece> pieces)
        {
            this.pieces = pieces;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecewiseFunction"/> class, copied from another instance.
        /// </summary>
        /// <param name="other">The target <see cref="PiecewiseFunction"/> to copy.</param>
        public PiecewiseFunction(PiecewiseFunction other)
        {
            this.pieces = new List<Piece>(other);
        }

        public int Count => this.pieces.Count;

        public Piece this[int index] => this.pieces[index];

        /// <summary>
        /// Given a piecewise function, returns an essentially-identical function that merges any adjacent pieces with the same value.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static PiecewiseFunction Consolidate(PiecewiseFunction function)
        {
            var newFunction = new PiecewiseFunction(function);

            for (var i = newFunction.Count - 1; i > 0; i--)
            {
                var highPiece = newFunction[i];
                var lowPiece = newFunction[i - 1];

                if (!highPiece.Value.ApproximatelyEquals(lowPiece.Value))
                {
                    continue;
                }
                // Combine the two pieces
                var newPiece = new Piece
                {
                    IncludeUpperBound = highPiece.IncludeUpperBound,
                    UpperBound = highPiece.UpperBound,
                    Value = highPiece.Value
                };

                newFunction.pieces.RemoveAt(i);
                newFunction.pieces.RemoveAt(i - 1);
                newFunction.pieces.Insert(i - 1, newPiece);
            }

            return newFunction;
        }

        /// <summary>
        /// Creates a new function by applying the specified transformation to the pieces of the existing function.
        /// </summary>
        /// <param name="function">Initial function.</param>
        /// <param name="transformation">Piece transformation.</param>
        /// <returns>A new piecewise function.</returns>
        public static PiecewiseFunction Modify(PiecewiseFunction function, Func<Piece, Piece> transformation)
        {
            var impliedZeroPiece = !function.pieces[function.pieces.Count - 1].UpperBound.ApproximatelyEquals(double.MaxValue);

            var newPieces = new List<Piece>(impliedZeroPiece ? function.Count + 1 : function.Count);

            for (var i = 0; i < function.Count; i++)
            {
                var newPiece = transformation(function[i]);

                if (double.IsPositiveInfinity(newPiece.UpperBound))
                {
                    newPiece.UpperBound = double.MaxValue;
                }

                newPieces.Add(newPiece);
            }

            if (impliedZeroPiece)
            {
                newPieces.Add(transformation(new Piece { IncludeUpperBound = true, UpperBound = double.MaxValue, Value = 0.0 }));
            }

            return new PiecewiseFunction(newPieces);
        }

        /// <summary>
        /// Creates a new function by applying the specified transformation to the pieces of the existing function, using the given double value.
        /// </summary>
        /// <param name="function">Initial function.</param>
        /// <param name="transformation">Piece transformation.</param>
        /// <param name="constant">A double constant used by the transformation.</param>
        /// <returns>A new piecewise function.</returns>
        public static PiecewiseFunction Modify(PiecewiseFunction function, Func<Piece, double, Piece> transformation, double constant)
        {
            var impliedZeroPiece = !function.pieces[function.pieces.Count - 1].UpperBound.ApproximatelyEquals(double.MaxValue);

            var newPieces = new List<Piece>(impliedZeroPiece ? function.Count + 1 : function.Count);

            for (var i = 0; i < function.Count; i++)
            {
                var newPiece = transformation(function[i], constant);

                if (double.IsPositiveInfinity(newPiece.UpperBound))
                {
                    newPiece.UpperBound = double.MaxValue;
                }

                newPieces.Add(transformation(function[i], constant));
            }

            if (impliedZeroPiece)
            {
                newPieces.Add(transformation(new Piece { IncludeUpperBound = true, UpperBound = double.MaxValue, Value = 0.0 }, constant));
            }

            return new PiecewiseFunction(newPieces);
        }

        /// <summary>
        /// Creates a new function representing the combination of the inputted functions, using the given mechanism to combine their values.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <param name="combination">The mechanism of combining the inputted function values.</param>
        /// <param name="evaluation1">The mechanism of getting a value from the first function once the second has ended.</param>
        /// <param name="evaluation2">The mechanism of getting a value from the second function once the first has ended.</param>
        /// <returns>A piecewise function built from the component functions.</returns>
        public static PiecewiseFunction Combine(
            PiecewiseFunction first,
            PiecewiseFunction second,
            Func<object, double, double, double> combination,
            Func<object, double, double> evaluation1,
            Func<object, double, double> evaluation2) => Combine(first, second, null, combination, evaluation1, evaluation2);

        /// <summary>
        /// Creates a new function representing the combination of the inputted functions, using the given mechanism to combine their values.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <param name="context">Some type of data context that might be referenced by the combination and evaluation functions</param>
        /// <param name="combination">The mechanism of combining the inputted function values.</param>
        /// <param name="evaluation1">The mechanism of getting a value from the first function once the second has ended.</param>
        /// <param name="evaluation2">The mechanism of getting a value from the second function once the first has ended.</param>
        /// <returns>A piecewise function built from the component functions.</returns>
        public static PiecewiseFunction Combine<T>(
            PiecewiseFunction first,
            PiecewiseFunction second,
            T context,
            Func<T, double, double, double> combination,
            Func<T, double, double> evaluation1,
            Func<T, double, double> evaluation2)
        {
            var newPieces = new List<Piece>();

            var firstIndex = 0;
            var secondIndex = 0;

            while (firstIndex < first.pieces.Count && secondIndex < second.pieces.Count)
            {
                var firstCurrent = first.pieces[firstIndex];
                var secondCurrent = second.pieces[secondIndex];

                var currentValue = combination(context, first.pieces[firstIndex].Value, second.pieces[secondIndex].Value);

                if (firstCurrent.UpperBound.ApproximatelyEquals(secondCurrent.UpperBound))
                {
                    // These both stop at the same point, which is tricky
                    if (firstCurrent.IncludeUpperBound == secondCurrent.IncludeUpperBound)
                    {
                        // They both also either include or do not include that point, so we can resolve them simultaneously
                        AddPieceNonRedundantly(newPieces, firstCurrent.UpperBound, firstCurrent.IncludeUpperBound, currentValue);

                        firstIndex++;
                        secondIndex++;
                    }
                    else
                    {
                        // One ends right BEFORE the given point, whereas one ends AT the given point.
                        // We will need to make a new section that ends BEFORE the given point, then
                        // a new section that begins and ends AT the given point. (The latter should
                        // be handled just by advancing the function.)
                        AddPieceNonRedundantly(newPieces, firstCurrent.UpperBound, false, currentValue);

                        if (firstCurrent.IncludeUpperBound)
                        {
                            secondIndex++;
                        }
                        else
                        {
                            firstIndex++;
                        }
                    }
                }
                else
                {
                    Piece endingPiece;

                    // Find the next point where a section ends
                    var nextBreakpoint = Lower(firstCurrent.UpperBound, secondCurrent.UpperBound);

                    if (nextBreakpoint.ApproximatelyEquals(firstCurrent.UpperBound))
                    {
                        endingPiece = firstCurrent;
                        firstIndex++;
                    }
                    else
                    {
                        endingPiece = secondCurrent;
                        secondIndex++;
                    }

                    AddPieceNonRedundantly(newPieces, endingPiece.UpperBound, endingPiece.IncludeUpperBound, currentValue);
                }
            }

            // One might continue after the other
            while (firstIndex < first.pieces.Count)
            {
                var firstCurrent = first.pieces[firstIndex];

                AddPieceNonRedundantly(
                    newPieces,
                    firstCurrent.UpperBound,
                    firstCurrent.IncludeUpperBound,
                    evaluation1(context, firstCurrent.Value));

                firstIndex++;
            }

            while (secondIndex < second.pieces.Count)
            {
                var secondCurrent = second.pieces[secondIndex];

                AddPieceNonRedundantly(newPieces, secondCurrent.UpperBound, secondCurrent.IncludeUpperBound, evaluation2(context, secondCurrent.Value));

                secondIndex++;
            }

            if (!newPieces[newPieces.Count - 1].UpperBound.ApproximatelyEquals(double.MaxValue))
            {
                AddPieceNonRedundantly(newPieces, double.MaxValue, true, combination(context, 0.0, 0.0));
            }

            return new PiecewiseFunction(newPieces);
        }

        /// <summary>
        /// Determines if there is any point where both functions are true (i.e. non-zero).
        /// </summary>
        /// <param name="first">The first function to examine.</param>
        /// <param name="second">The second function to examine.</param>
        /// <returns><c>true</c> if both functions have a non-zero y-value at some same x-value, <c>false</c> otherwise.</returns>
        public static bool HasOverlap(
            PiecewiseFunction first,
            PiecewiseFunction second)
        {
            var firstIndex = 0;
            var secondIndex = 0;

            while (firstIndex < first.pieces.Count && secondIndex < second.pieces.Count)
            {
                var firstCurrent = first.pieces[firstIndex];
                var secondCurrent = second.pieces[secondIndex];

                if (firstCurrent.Value != 0.0 && secondCurrent.Value != 0.0)
                {
                    if (!firstCurrent.Value.IsApproximatelyZero() && !secondCurrent.Value.IsApproximatelyZero())
                    {
                        return true;
                    }
                }

                if (firstCurrent.UpperBound.ApproximatelyEquals(secondCurrent.UpperBound))
                {
                    // These both stop at the same point, which is tricky
                    if (firstCurrent.IncludeUpperBound == secondCurrent.IncludeUpperBound)
                    {
                        firstIndex++;
                        secondIndex++;
                    }
                    else
                    {
                        // One ends right BEFORE the given point, whereas one ends AT the given point.
                        // We will need to examine section that ends BEFORE the given point, then
                        // a section that begins and ends AT the given point. (The latter should
                        // be handled just by advancing the function.)
                        if (firstCurrent.IncludeUpperBound)
                        {
                            secondIndex++;
                        }
                        else
                        {
                            firstIndex++;
                        }
                    }
                }
                else
                {
                    // Find the next point where a section ends
                    var nextBreakpoint = Lower(firstCurrent.UpperBound, secondCurrent.UpperBound);

                    if (nextBreakpoint.ApproximatelyEquals(firstCurrent.UpperBound))
                    {
                        firstIndex++;
                    }
                    else
                    {
                        secondIndex++;
                    }
                }
            }

            return false;
        }

        private static void AddPieceNonRedundantly(List<Piece> pieces, double upperBound, bool includeUpperBound, double value)
        {
            if (pieces.Count > 0)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                // ApproximatelyEquals is too slow and this primarily is for catching
                // cases dealing with constraint ranges, where everything is 1.0 or 0.0.
                if (value == pieces[pieces.Count - 1].Value)
                {
                    pieces.RemoveAt(pieces.Count - 1);
                }
            }

            pieces.Add(new Piece { UpperBound = upperBound, IncludeUpperBound = includeUpperBound, Value = value });
        }

        /// <summary>
        /// Creates a new function equal to the inputted function, but multiplied throughout by the given amount.
        /// </summary>
        /// <param name="function">A piecewise function.</param>
        /// <param name="constantValue">Some value by which to multiply the function.</param>
        /// <returns>A new piecewise function where, for a given X-value, the Y-value should be equal to the Y-value of the inputted function times the inputted double value.</returns>
        public static PiecewiseFunction operator *(PiecewiseFunction function, double constantValue) => Modify(function, MultiplyByConstant, constantValue);

        /// <summary>
        /// Creates a new function representing the piecewise product of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the product of the Y-values of the two functions at the same X-value.</returns>
        public static PiecewiseFunction operator *(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, MultiplyPieces, ZeroFunction, ZeroFunction);

        /// <summary>
        /// Creates a new function representing the piecewise quotient of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the Y-value of the first divided by the Y-value of the second at the same X-value.</returns>
        public static PiecewiseFunction operator /(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, DividePieces, ZeroFunction, ZeroFunction);

        /// <summary>
        /// Creates a new function equal to the inputted function, but increased throughout by the given amount.
        /// </summary>
        /// <param name="function">A piecewise function.</param>
        /// <param name="constantValue">Some value by which to increase the function.</param>
        /// <returns>A new piecewise function where, for a given X-value, the Y-value should be equal to the Y-value of the inputted function plus the inputted double value.</returns>
        public static PiecewiseFunction operator +(PiecewiseFunction function, double constantValue) => Modify(function, AddConstant, constantValue);

        /// <summary>
        /// Creates a new function equal to the inputted function, but decreased throughout by the given amount.
        /// </summary>
        /// <param name="function">A piecewise function.</param>
        /// <param name="constantValue">Some value by which to decrease the function.</param>
        /// <returns>A new piecewise function where, for a given X-value, the Y-value should be equal to the Y-value of the inputted function minus the inputted double value.</returns>
        public static PiecewiseFunction operator -(PiecewiseFunction function, double constantValue) => Modify(function, SubtractConstant, constantValue);

        /// <summary>
        /// Creates a new function representing the sum of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the sum of the Y-values of the two functions at the same X-value.</returns>
        public static PiecewiseFunction operator +(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, AddPieces, IdentityFunction, IdentityFunction);

        /// <summary>
        /// Creates a new function representing the difference of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the Y-value of the first function less the Y-value of the second at the same X-value.</returns>
        public static PiecewiseFunction operator -(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, SubtractPieces, IdentityFunction, NegationFunction);

        /// <summary>
        /// Creates a new function representing the greater value of the two functions at all possible X-values.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the greater of the Y-values of the two functions at the same X-value.</returns>
        public static PiecewiseFunction Greater(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, GreaterOf, ZeroOrGreater, ZeroOrGreater);

        /// <summary>
        /// Creates a new function representing the lesser value of the two functions at all possible X-values.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the lesser of the Y-values of the two functions at the same X-value.</returns>
        public static PiecewiseFunction Lesser(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, LesserOf, ZeroOrLess, ZeroOrLess);

        /// <summary>
        /// Creates a new function representing the sum of the inputted functions.
        /// </summary>
        /// <param name="functions">A set of piecewise functions.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal to the sum of the Y-values of the functions at the same X-value.</returns>
        public static PiecewiseFunction AddMany(PiecewiseFunction[] functions)
        {
            var count = functions.Length;

            var indexes = new int[count];
            var pieces = new Piece?[count];

            var currentValue = 0.0;
            var upperBound = double.MaxValue;
            var ending = new bool[count];

            var anyExist = false;

            int current;
            for (current = 0; current < functions.Length; current++)
            {
                Piece? piece = null;

                if (functions[current].Count > 0)
                {
                    piece = functions[current][0];

                    currentValue += piece.Value.Value;

                    if (piece.Value.UpperBound.ApproximatelyEquals(upperBound))
                    {
                        ending[current] = true;
                    }
                    else if (piece.Value.UpperBound < upperBound)
                    {
                        Clear(ending);
                        ending[current] = true;
                        upperBound = piece.Value.UpperBound;
                    }

                    anyExist = true;
                }

                pieces[current] = piece;
            }

            var newFunctionPieces = new List<Piece>();

            while (anyExist)
            {
                var notIncludeUpperBound = false;

                for (var i = 0; i < functions.Length; i++)
                {
                    if (!ending[i])
                    {
                        continue;
                    }
                    if (pieces[i] != null && pieces[i].Value.IncludeUpperBound)
                    {
                        continue;
                    }
                    // We can go ahead and move the enumerator forward on these pieces as we know they are ending
                    notIncludeUpperBound = true;
                    if (functions[i].Count > indexes[i] + 1)
                    {
                        indexes[i] = indexes[i] + 1;
                        pieces[i] = functions[i][indexes[i]];
                    }
                    else
                    {
                        pieces[i] = null;
                    }
                }

                if (notIncludeUpperBound)
                {
                    // End this section with a point that does not include the right bound (pieces are already incremented)
                    newFunctionPieces.Add(new Piece { UpperBound = upperBound, IncludeUpperBound = false, Value = currentValue });
                }
                else
                {
                    // End this section and increment the pieces
                    newFunctionPieces.Add(new Piece { UpperBound = upperBound, IncludeUpperBound = true, Value = currentValue });
                    for (var i = 0; i < functions.Length; i++)
                    {
                        if (!ending[i])
                        {
                            continue;
                        }
                        if (functions[i].Count > indexes[i] + 1)
                        {
                            indexes[i] = indexes[i] + 1;
                            pieces[i] = functions[i][indexes[i]];
                        }
                        else
                        {
                            pieces[i] = null;
                        }
                    }
                }

                anyExist = false;

                currentValue = 0.0;
                Clear(ending);
                upperBound = double.MaxValue;

                for (var i = 0; i < count; i++)
                {
                    if (!pieces[i].HasValue)
                    {
                        continue;
                    }
                    anyExist = true;
                    currentValue += pieces[i].Value.Value;

                    if (pieces[i].Value.UpperBound.ApproximatelyEquals(upperBound))
                    {
                        ending[i] = true;
                    }
                    else if (pieces[i].Value.UpperBound < upperBound)
                    {
                        Clear(ending);
                        ending[i] = true;
                        upperBound = pieces[i].Value.UpperBound;
                    }
                }
            }

            return new PiecewiseFunction(newFunctionPieces);
        }

        private static void Clear(bool[] flags)
        {
            for (var i = 0; i < flags.Length; i++)
            {
                flags[i] = false;
            }
        }

        /// <summary>
        /// Creates a new function representing the logical-AND of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal 1 if both functions have a nonzero Y-value at the same X-value.</returns>
        public static PiecewiseFunction operator &(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, PiecewiseAnd, ZeroFunction, ZeroFunction);

        /// <summary>
        /// Creates a new function representing the logical-OR of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal 1 if either function has a nonzero Y-value at the same X-value.</returns>
        public static PiecewiseFunction operator |(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, PiecewiseOr, IsTrueFunction, IsTrueFunction);

        /// <summary>
        /// Creates a new function representing the logical-NOT of the inputted function (i.e., with value 1 where the function has value 0 and value 0 where the function has any non-zero value).
        /// </summary>
        /// <param name="function">A piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value is 1 if the original function's Y-value was 0, and otherwise the Y-value is 0.</returns>
        public static PiecewiseFunction LogicalNot(PiecewiseFunction function) => Modify(function, NotFunction);

        /// <summary>
        /// Creates a new function representing the logical-XOR of the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value should be equal 1 if either the first function or the second function has a nonzero Y-value at the same X-value, but not both.</returns>
        public static PiecewiseFunction operator ^(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, PiecewiseXOr, IsTrueFunction, IsTrueFunction);

        /// <summary>
        /// Creates a new function representing the inequality "Greater Than" for the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the first function should be greater than the Y-value of the second function at the same X-value.</returns>
        public static PiecewiseFunction operator >(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, GreaterThan, IsGreaterThanZero, IsLessThanZero);

        /// <summary>
        /// Creates a new piecewise function representing the inequality "Greater Than" for the inputted piecewise function and linear function.
        /// </summary>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <param name="linearFunction">A linear function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the piecewise function should be 1.0 if the piecewise function is greater than the linear function at the same X-value, and 0.0 otherwise.</returns>
        public static PiecewiseFunction operator >(PiecewiseFunction piecewiseFunction, LinearFunction linearFunction) => PiecewiseLinearInequality(piecewiseFunction, linearFunction, false, false, true);

        /// <summary>
        /// Creates a new function representing the inequality "Greater Than or Equal To" for the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the first function should be greater than or equal to the Y-value of the second function at the same X-value.</returns>
        public static PiecewiseFunction operator >=(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, GreaterThanOrEquals, IsGreaterThanOrEqualToZero, IsLessThanOrEqualToZero);

        /// <summary>
        /// Creates a new piecewise function representing the inequality "Greater Than Or Equal To" for the inputted piecewise function and linear function.
        /// </summary>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <param name="linearFunction">A linear function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the piecewise function should be 1.0 if the piecewise function is greater than or equal to the linear function at the same X-value, and 0.0 otherwise.</returns>
        public static PiecewiseFunction operator >=(PiecewiseFunction piecewiseFunction, LinearFunction linearFunction) => PiecewiseLinearInequality(piecewiseFunction, linearFunction, false, true, true);

        /// <summary>
        /// Creates a new function representing the inequality "Less Than" for the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the first function should be less than the Y-value of the second function at the same X-value.</returns>
        public static PiecewiseFunction operator <(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, LessThan, IsLessThanZero, IsGreaterThanZero);

        /// <summary>
        /// Creates a new piecewise function representing the inequality "Less Than" for the inputted piecewise function and linear function.
        /// </summary>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <param name="linearFunction">A linear function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the piecewise function should be 1.0 if the piecewise function is less than the linear function at the same X-value, and 0.0 otherwise.</returns>
        public static PiecewiseFunction operator <(PiecewiseFunction piecewiseFunction, LinearFunction linearFunction) => PiecewiseLinearInequality(piecewiseFunction, linearFunction, true, false, false);

        /// <summary>
        /// Creates a new function representing the inequality "Less Than or Equal To" for the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the first function should be less than or equal to the Y-value of the second function at the same X-value.</returns>
        public static PiecewiseFunction operator <=(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, LessThanOrEquals, IsLessThanOrEqualToZero, IsGreaterThanOrEqualToZero);

        /// <summary>
        /// Creates a new piecewise function representing the inequality "Less Than Or Equal To" for the inputted piecewise function and linear function.
        /// </summary>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <param name="linearFunction">A linear function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the piecewise function should be 1.0 if the piecewise function is less than or equal to the linear function at the same X-value, and 0.0 otherwise.</returns>
        public static PiecewiseFunction operator <=(PiecewiseFunction piecewiseFunction, LinearFunction linearFunction) => PiecewiseLinearInequality(piecewiseFunction, linearFunction, true, true, false);

        /// <summary>
        /// Creates a new piecewise function representing the inequality "Equal To" for the inputted piecewise function and linear function.
        /// </summary>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <param name="linearFunction">A linear function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the piecewise function should be 1.0 if the piecewise function is equal to the linear function at the same X-value, and 0.0 otherwise.</returns>
        public static PiecewiseFunction EqualTo(PiecewiseFunction piecewiseFunction, LinearFunction linearFunction) => PiecewiseLinearInequality(piecewiseFunction, linearFunction, false, true, false);

        /// <summary>
        /// Creates a new piecewise function representing the inequality "Not Equal To" for the inputted piecewise function and linear function.
        /// </summary>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <param name="linearFunction">A linear function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the piecewise function should be 1.0 if the piecewise function is not equal to the linear function at the same X-value, and 0.0 otherwise.</returns>
        public static PiecewiseFunction NotEqualTo(PiecewiseFunction piecewiseFunction, LinearFunction linearFunction) => PiecewiseLinearInequality(piecewiseFunction, linearFunction, true, false, true);

        /// <summary>
        /// Creates a new function representing the equality "Equal To" for the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the first function should be equal to the Y-value of the second function at the same X-value.</returns>
        public static PiecewiseFunction EqualTo(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, IsEqualTo, IsFalseFunction, IsFalseFunction);

        /// <summary>
        /// Creates a new function representing the inequality "Not Equal To" for the inputted functions.
        /// </summary>
        /// <param name="first">A piecewise function.</param>
        /// <param name="second">Another piecewise function.</param>
        /// <returns>A piecewise function where, for a given X-value, the Y-value of the first function should be not equal to the Y-value of the second function at the same X-value.</returns>
        public static PiecewiseFunction NotEqualTo(PiecewiseFunction first, PiecewiseFunction second) => Combine(first, second, IsNotEqualTo, IsTrueFunction, IsTrueFunction);

        public IEnumerator<Piece> GetEnumerator() => this.pieces.GetEnumerator();

        /// <inheritdoc/>
        public override string ToString()
        {
            var subStrings = new List<string>();

            for (var i = 0; i < this.Count; i++)
            {
                var piece = this[i];

                if (i >= 1)
                {
                    var previousPiece = this[i - 1];

                    subStrings.Add(
                        $"{previousPiece.UpperBound}{(previousPiece.IncludeUpperBound ? " < x" : " <= x")}{(piece.IncludeUpperBound ? " <= " : " < ")}{piece.UpperBound}: {piece.Value}");
                }
                else
                {
                    subStrings.Add(
                        $"{double.MinValue} <= x {(piece.IncludeUpperBound ? "<= " : "< ")}{piece.UpperBound}: {piece.Value}");
                }
            }

            return string.Join(", \r\n", subStrings);
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Add a piece to the end of the function. Only works if the LowerBound and IncludeLeft are appropriate to the function built thus far.
        /// </summary>
        /// <param name="piece">Piece to add.</param>
        public void Add(Piece piece)
        {
            if (double.IsInfinity(piece.UpperBound) || double.IsNaN(piece.UpperBound))
            {
                throw new ArgumentException("Invalid upper bound: " + piece.UpperBound);
            }

            if (double.IsNaN(piece.Value))
            {
                throw new ArgumentException("Invalid piece value: " + piece.Value);
            }

            if (this.pieces.Count > 0)
            {
                var lastPiece = this.pieces[this.pieces.Count - 1];

                if (piece.UpperBound.ApproximatelyEquals(lastPiece.UpperBound))
                {
                    if (lastPiece.IncludeUpperBound)
                    {
                        throw new ArgumentException(
                            "Cannot add a piece that ends at the same upper bound if the previous piece contained its upper bound.");
                    }
                }
                else if (piece.UpperBound < lastPiece.UpperBound)
                {
                    throw new ArgumentException("Cannot add a piece with an upper bound below the previous piece's upper bound.");
                }
            }

            this.pieces.Add(piece);
        }

        /// <summary>
        /// Add a piece to the end of the function. Lower bound, and whether it is included, will be determined automatically. Upper bound will be included by default.
        /// </summary>
        /// <param name="upperBound">Upper bound of the piece.</param>
        /// <param name="value">Value of the function for this piece.</param>
        public void Add(double upperBound, double value) => this.Add(upperBound, true, value);

        /// <summary>
        /// Add a piece to the end of the function. Lower bound, and whether it is included, will be determined automatically.
        /// </summary>
        /// <param name="upperBound">Upper bound of the piece.</param>
        /// <param name="includeUpperBound">Whether the upper bound should be included in this piece or the next.</param>
        /// <param name="value">Value of the function for this piece.</param>
        public void Add(double upperBound, bool includeUpperBound, double value)
        {
            var piece = new Piece
            {
                Value = value,
                IncludeUpperBound = includeUpperBound,
                UpperBound = upperBound,
            };

            this.Add(piece);
        }

        /// <summary>
        /// Gets the value of the function at this point.
        /// </summary>
        /// <param name="x">X-value of the point to check.</param>
        /// <returns>Y-value of the function at this X-value.</returns>
        public double GetValue(double x)
        {
            if (double.IsNegativeInfinity(x))
            {
                throw new ArgumentException("Function undefined at this point (negative infinity).");
            }

            if (double.IsPositiveInfinity(x))
            {
                throw new ArgumentException("Function undefined at this point (infinity).");
            }

            if (double.IsNaN(x))
            {
                throw new ArgumentException("Function undefined at this point (NaN).");
            }

            for (var i = 0; i < this.Count; i++)
            {
                var piece = this[i];

                if (x < piece.UpperBound || (x.ApproximatelyEquals(piece.UpperBound) && piece.IncludeUpperBound))
                {
                    return piece.Value;
                }
            }

            return 0;
        }

        /// <summary>
        /// Determines whether two piecewise functions have equal Y-values throughout.
        /// </summary>
        /// <param name="otherFunction">A piecewise function for comparison.</param>
        /// <returns><c>true</c> if the two functions have the same value at every point, <c>false</c> otherwise.</returns>
        public bool IsEquivalent(PiecewiseFunction otherFunction)
        {
            if (otherFunction == null)
            {
                return false;
            }

            var firstEnum = this.GetEnumerator();
            var secondEnum = otherFunction.GetEnumerator();

            var firstCurrent = firstEnum.MoveNext() ? (Piece?)firstEnum.Current : null;
            var secondCurrent = secondEnum.MoveNext() ? (Piece?)secondEnum.Current : null;

            while (firstCurrent != null && secondCurrent != null)
            {
                if (!firstCurrent.Value.Value.ApproximatelyEquals(secondCurrent.Value.Value))
                {
                    return false;
                }

                if (firstCurrent.Value.UpperBound.ApproximatelyEquals(secondCurrent.Value.UpperBound))
                {
                    // These both stop at the same point, which is tricky
                    if (firstCurrent.Value.IncludeUpperBound == secondCurrent.Value.IncludeUpperBound)
                    {
                        // They both also either include or do not include that point, so we can resolve them simultaneously
                        firstCurrent = firstEnum.MoveNext() ? (Piece?)firstEnum.Current : null;
                        secondCurrent = secondEnum.MoveNext() ? (Piece?)secondEnum.Current : null;
                    }
                    else
                    {
                        // One ends right BEFORE the given point, whereas one ends AT the given point.
                        // We will need to move to the next item on the one that has ended, but not the other.
                        if (firstCurrent.Value.IncludeUpperBound)
                        {
                            secondCurrent = secondEnum.MoveNext() ? (Piece?)secondEnum.Current : null;
                        }
                        else
                        {
                            firstCurrent = firstEnum.MoveNext() ? (Piece?)firstEnum.Current : null;
                        }
                    }
                }
                else
                {
                    // Find the next point where a section ends
                    var nextBreakpoint = Lower(firstCurrent.Value.UpperBound, secondCurrent.Value.UpperBound);

                    if (nextBreakpoint.ApproximatelyEquals(firstCurrent.Value.UpperBound))
                    {
                        firstCurrent = firstEnum.MoveNext() ? (Piece?)firstEnum.Current : null;
                    }
                    else
                    {
                        secondCurrent = secondEnum.MoveNext() ? (Piece?)secondEnum.Current : null;
                    }
                }
            }

            // If one of the functions continues after the other function, make sure it only has values of 0
            while (firstCurrent != null)
            {
                if (!firstCurrent.Value.Value.IsApproximatelyZero())
                {
                    return false;
                }

                firstCurrent = firstEnum.MoveNext() ? (Piece?)firstEnum.Current : null;
            }

            while (secondCurrent != null)
            {
                if (!secondCurrent.Value.Value.IsApproximatelyZero())
                {
                    return false;
                }

                secondCurrent = secondEnum.MoveNext() ? (Piece?)secondEnum.Current : null;
            }

            // If we get to here, they matched the whole way
            return true;
        }

        /// <summary>
        /// Gets the highest X-value for which there is a non-zero Y-value, or double.NegativeInfinity if there is no non-zero Y-value.
        /// </summary>
        /// <returns>An X-value.</returns>
        public double HighestNonZeroPoint()
        {
            for (var i = this.Count - 1; i >= 0; i--)
            {
                var piece = this[i];

                if (!piece.Value.IsApproximatelyZero())
                {
                    return piece.UpperBound;
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Gets the lowest X-value for which there is a non-zero Y-value, or double.PositiveInfinity if there is no non-zero Y-value.
        /// </summary>
        /// <returns>An X-value.</returns>
        public double LowestNonZeroPoint()
        {
            for (var i = 0; i < this.Count; i++)
            {
                var piece = this[i];

                if (!piece.Value.IsApproximatelyZero())
                {
                    return i == 0 ? double.MinValue : this[i - 1].UpperBound;
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Gets the highest X-value for which there is a zero Y-value, or double.NegativeInfinity if there is no zero Y-value.
        /// </summary>
        /// <returns>An X-value.</returns>
        public double HighestZeroPoint()
        {
            if (this.Count == 0)
            {
                return double.MaxValue;
            }

            if (this[this.Count - 1].UpperBound < double.MaxValue ||
                !this[this.Count - 1].IncludeUpperBound)
            {
                return double.MaxValue;
            }

            for (var i = this.Count - 1; i >= 0; i--)
            {
                var piece = this[i];

                if (piece.Value.IsApproximatelyZero())
                {
                    return piece.UpperBound;
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Gets the lowest X-value for which there is a zero Y-value, or double.PositiveInfinity if there is no zero Y-value.
        /// </summary>
        /// <returns>An X-value.</returns>
        public double LowestZeroPoint()
        {
            if (this.Count == 0)
            {
                return double.MinValue;
            }

            for (var i = 0; i < this.Count; i++)
            {
                var piece = this[i];

                if (piece.Value.IsApproximatelyZero())
                {
                    return i == 0 ? double.MinValue : this[i - 1].UpperBound;
                }
            }

            if (this[this.Count - 1].UpperBound < double.MaxValue ||
                !this[this.Count - 1].IncludeUpperBound)
            {
                return this[this.Count - 1].UpperBound;
            }

            return double.NaN;
        }

        /// <summary>
        /// Gets the lower-valued number.
        /// </summary>
        /// <param name="a">First number.</param>
        /// <param name="b">Second number.</param>
        /// <returns>Whichever of the two numbers is lower.</returns>
        private static double Lower(double a, double b) => a < b ? a : b;

        /// <summary>
        /// Gets a new piecewise function that represents the specified inequality between a piecewise function and a linear function.
        /// </summary>
        /// <param name="piecewiseFunction">The piecewise function. Treatest as the first argument to the inequality.</param>
        /// <param name="linearFunction">The linear function. Treated as the second argument to the inequality.</param>
        /// <param name="lessThan">Whether the inequality is satisfied when the piecewise function is less than the linear function (for inequalities 'less than', 'less than or equal to', or 'not equal to').</param>
        /// <param name="equalTo">Whether the inequality is satisfied when the piecewise function is equal to the linear function (for the inequalities 'equal to', 'less than or equal to', or 'greater than or equal to').</param>
        /// <param name="greaterThan">Whether the inequality is satisfied when the piecewise function is greater than the linear function (for the inequalities 'greater than', 'greater than or equal to', or 'not equal to').</param>
        /// <returns>A new piecewise function with a value of 1 for all x-values where the inequality was satisfied and a value of 0 for all where it was not.</returns>
        private static PiecewiseFunction PiecewiseLinearInequality(
            PiecewiseFunction piecewiseFunction,
            LinearFunction linearFunction,
            bool lessThan,
            bool equalTo,
            bool greaterThan)
        {
            if (lessThan == equalTo && equalTo == greaterThan)
            {
                throw new ArgumentException(
                    "It is not valid to call PiecewiseLinearEquality and pass in the same value for lessThan, equalTo, and greaterThan. That implies that you're getting a new function that is always true or always false no matter what the arguments were.");
            }

            var result = new PiecewiseFunction();

            // Possibly we just have a constant function instead of a linear function
            if (linearFunction.Slope == 0.0)
            {
                foreach (var piece in piecewiseFunction)
                {
                    if (piece.Value.ApproximatelyEquals(linearFunction.YIntersect))
                    {
                        result.Add(piece.UpperBound, piece.IncludeUpperBound, equalTo ? 1.0 : 0.0);
                    }
                    else if (piece.Value > linearFunction.YIntersect)
                    {
                        result.Add(piece.UpperBound, piece.IncludeUpperBound, greaterThan ? 1.0 : 0.0);
                    }
                    else
                    {
                        result.Add(piece.UpperBound, piece.IncludeUpperBound, lessThan ? 1.0 : 0.0);
                    }
                }

                // TODO: Treat the rest of the function as 0.0?
                return result;
            }

            var currentLowerBound = double.MinValue;
            var includeLowerBound = true;

            foreach (var piece in piecewiseFunction)
            {
                var intersection = linearFunction.GetXValue(piece.Value);

                var positiveSlope = linearFunction.Slope > 0;

                var trueBeforeIntersection = (positiveSlope & greaterThan) | (!positiveSlope & lessThan);
                var trueAfterIntersection = (positiveSlope & lessThan) | (!positiveSlope & greaterThan);

                if (intersection.ApproximatelyEquals(currentLowerBound))
                {
                    if (equalTo != trueAfterIntersection && includeLowerBound)
                    {
                        result.Add(currentLowerBound, true, equalTo ? 1.0 : 0.0);
                    }

                    if (!piece.UpperBound.ApproximatelyEquals(currentLowerBound))
                    {
                        result.Add(piece.UpperBound, piece.IncludeUpperBound, trueAfterIntersection ? 1.0 : 0.0);
                    }
                }
                else if (intersection.ApproximatelyEquals(piece.UpperBound))
                {
                    if (trueBeforeIntersection != equalTo & piece.IncludeUpperBound)
                    {
                        result.Add(piece.UpperBound, false, trueBeforeIntersection ? 1.0 : 0.0);
                        result.Add(piece.UpperBound, true, equalTo ? 1.0 : 0.0);
                    }
                    else
                    {
                        result.Add(piece.UpperBound, piece.IncludeUpperBound, trueBeforeIntersection ? 1.0 : 0.0);
                    }
                }
                else if (intersection < currentLowerBound)
                {
                    result.Add(piece.UpperBound, piece.IncludeUpperBound, trueAfterIntersection ? 1.0 : 0.0);
                }
                else if (intersection > piece.UpperBound)
                {
                    result.Add(piece.UpperBound, piece.IncludeUpperBound, trueBeforeIntersection ? 1.0 : 0.0);
                }
                else
                {
                    if (trueBeforeIntersection == equalTo)
                    {
                        result.Add(intersection, true, trueBeforeIntersection ? 1.0 : 0.0);
                    }
                    else
                    {
                        result.Add(intersection, false, trueBeforeIntersection ? 1.0 : 0.0);

                        if (equalTo != trueAfterIntersection)
                        {
                            result.Add(intersection, true, equalTo ? 1.0 : 0.0);
                        }
                    }

                    result.Add(piece.UpperBound, piece.IncludeUpperBound, trueAfterIntersection ? 1.0 : 0.0);
                }

                currentLowerBound = piece.UpperBound;
                includeLowerBound = !piece.IncludeUpperBound;
            }

            return result;
        }

        public bool IsEverNonZero()
        {
            return this.Any(p => !p.Value.IsApproximatelyZero());
        }

        public struct Piece
        {
            /// <summary>
            /// Gets or sets a value indicating whether the upper bound is inclusive or exclusive.
            /// </summary>
            /// <value>
            /// <c>true</c> if the upper bound is part of this piece, <c>false</c> if not.
            /// </value>
            public bool IncludeUpperBound;

            /// <summary>
            /// Gets or sets the upper limit of the piece. This exact value is included only if IncludeUpperBound is <c>true</c>.
            /// </summary>
            public double UpperBound;

            /// <summary>
            /// Gets or sets the value for this piece.
            /// </summary>
            public double Value;

            /// <summary>
            /// Gets the X-value where this piece intersects with the given linear function.
            /// </summary>
            /// <param name="function">A linear function to check against.</param>
            /// <param name="lowerBound">The lower bound to use for this piece.</param>
            /// <param name="includeLowerBound">Whether or not to consider a match valid if it is at the lower bound.</param>
            /// <returns>An X-value between the lower and upper bound of this piece, or null if no intersection is found.</returns>
            public double? GetIntersect(LinearFunction function, double lowerBound, bool includeLowerBound)
            {
                // Gets the X-value where the value of the linear function equals the value of this piece
                var functionXValue = function.GetXValue(this.Value);

                if (functionXValue.ApproximatelyEquals(lowerBound))
                {
                    if (includeLowerBound)
                    {
                        return functionXValue;
                    }

                    return null;
                }

                if (functionXValue.ApproximatelyEquals(this.UpperBound))
                {
                    if (this.IncludeUpperBound)
                    {
                        return functionXValue;
                    }

                    return null;
                }

                if (functionXValue < lowerBound)
                {
                    return null;
                }

                if (functionXValue > this.UpperBound)
                {
                    return null;
                }

                return functionXValue;
            }

            /// <summary>
            /// Determines if the specified X-value is within the range of this piece.
            /// </summary>
            /// <param name="value">The X-value to check.</param>
            /// <param name="lowerBound">The lower bound to use for this piece.</param>
            /// <param name="includeLowerBound">Whether or not to consider a match valid if it is at the lower bound.</param>
            /// <returns><c>true</c> if this piece covers the specified X-value, <c>false</c> if not.</returns>
            public bool Contains(double value, double lowerBound, bool includeLowerBound)
            {
                if (value.ApproximatelyEquals(lowerBound) && !includeLowerBound)
                {
                    return false;
                }

                if (value.ApproximatelyEquals(this.UpperBound) && !this.IncludeUpperBound)
                {
                    return false;
                }

                if (value < lowerBound)
                {
                    return false;
                }

                return !(value > this.UpperBound);
            }
        }
    }
}
