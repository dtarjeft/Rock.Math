namespace Rock.Math
{
    using System;

    /// <summary>
    /// Helper methods for doubles.
    /// </summary>
    public static class DoubleExtensions
    {
        public const double ApproximatelyZero = 0.000000000001;

        /// <summary>
        /// Checks whether two doubles are essentially equal, other than some amount that could be due to rounding errors.
        /// </summary>
        /// <param name="a">A double to compare.</param>
        /// <param name="b">Another double to compare.</param>
        /// <param name="approximatelyZero">An optional parameter to specify a value that is considered 'zero' for our purposes. Cannot be negative.</param>
        /// <returns><c>true</c> if the numbers are equal, or within .0001% of each other. <c>false</c> if not.</returns>
        public static bool ApproximatelyEquals(this double a, double b, double approximatelyZero = ApproximatelyZero)
        {
            if (a == b)
            {
                return true;
            }

            if (a.IsApproximatelyZero(approximatelyZero))
            {
                return b.IsApproximatelyZero(approximatelyZero);
            }

            var greater = a;
            var lesser = b;

            if (Math.Abs(greater) < Math.Abs(lesser))
            {
                greater = b;
                lesser = a;
            }

            return Math.Abs((greater - lesser) / greater) < approximatelyZero;
        }

        /// <summary>
        /// Checks whether a double is effectively about zero.
        /// </summary>
        /// <param name="a">A double to check.</param>
        /// <param name="approximatelyZero">An optional parameter to specify a value that is considered 'zero' for our purposes. Cannot be negative.</param>
        /// <returns><c>true</c> if the double is very close to zero, <c>false</c> is not.</returns>
        public static bool IsApproximatelyZero(this double a, double approximatelyZero = ApproximatelyZero)
        {
            if (approximatelyZero < 0.0)
            {
                throw new ArgumentException(
                    string.Format("Optional parameter must be greater than or equal to zero (was {0}).", approximatelyZero),
                    "approximatelyZero");
            }

            return Math.Abs(a) <= approximatelyZero;
        }
    }
}
