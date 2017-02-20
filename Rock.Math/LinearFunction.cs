namespace Rock.Math
{
    using System;

    /// <summary>
    /// Represents a straight-line function that can be expressed in the form "y = mx + b".
    /// </summary>
    public class LinearFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearFunction"/> class, with the given slope and y-intersect.
        /// </summary>
        /// <param name="slope">Slope of the function.</param>
        /// <param name="yIntersect">Y-intersect of the function.</param>
        public LinearFunction(double slope, double yIntersect)
        {
            this.Slope = slope;
            this.YIntersect = yIntersect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearFunction"/> class, with the given slope, that passes through the given point.
        /// </summary>
        /// <param name="slope">Slope of the function.</param>
        /// <param name="x">The x-coordinate of a point that the line passes through.</param>
        /// <param name="y">The y-coordinate of a point that the line passes through.</param>
        public LinearFunction(double slope, double x, double y)
        {
            this.Slope = slope;
            this.YIntersect = y - slope * x;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearFunction"/> class, creating a line that passes through the two points given.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point that the line passes through.</param>
        /// <param name="y1">The y-coordinate of the first point that the line passes through.</param>
        /// <param name="x2">The x-coordinate of the second point that the line passes through.</param>
        /// <param name="y2">The y-coordinate of the second point that the line passes through.</param>
        public LinearFunction(double x1, double y1, double x2, double y2)
        {
            if (x1.ApproximatelyEquals(x2))
            {
                throw new ArgumentException("X-coordinates were identical or approximately equal.");
            }

            this.Slope = (y2 - y1) / (x2 - x1);
            this.YIntersect = y1 - this.Slope * x1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearFunction"/> class, copied from another instance.
        /// </summary>
        /// <param name="other">The target <see cref="LinearFunction"/> to copy.</param>
        public LinearFunction(LinearFunction other)
        {
            this.Slope = other.Slope;
            this.YIntersect = other.YIntersect;
        }

        public double YIntersect { get; set; }

        public double Slope { get; set; }

        public double GetXValue(double yValue) => (yValue - this.YIntersect) / this.Slope;

        /// <summary>
        /// Finds the x-coordinate of the point where two linear functions meet (or null if they have approximately the same slope).
        /// </summary>
        /// <param name="other">The other function that this function may intersect with.</param>
        /// <returns>The x-coordinate of the intersection, or null if the functions have approximately the same slope.</returns>
        public double? GetIntersection(LinearFunction other)
        {
            if (this.Slope.ApproximatelyEquals(other.Slope))
            {
                return null;
            }

            /* Formula for the intersection of two (non-parallel) lines:
             * 
             * Assuming one line is y = ax + c and the other is y = bx + d,
             * 
             * x = (d - c) / (a - b)
             * y = a * ( (d - c) / (a - b) ) + c
             * 
             * Of course, we only need the x-value.
             * So, let's use the following:
             *   a - slope of first
             *   b - slope of second
             *   c - y-intersect of first
             *   d - y-intersect of second
             */

            return (other.YIntersect - this.YIntersect) / (this.Slope - other.Slope);
        }

        /// <summary>
        /// Creates a new linear function by adding the given value to the function's y-intersect, i.e., raising the line by this amount.
        /// </summary>
        /// <param name="first">A linear function.</param>
        /// <param name="second">A value by which to raise the linear function.</param>
        /// <returns>A new linear function.</returns>
        public static LinearFunction operator +(LinearFunction first, double second) => new LinearFunction(first.Slope, first.YIntersect + second);

        /// <summary>
        /// Creates a new linear function whose slope and y-intersect are equal to the sums of those of the argument functions.
        /// </summary>
        /// <param name="first">A linear function.</param>
        /// <param name="second">Another linear function.</param>
        /// <returns>A new linear function.</returns>
        public static LinearFunction operator +(LinearFunction first, LinearFunction second) => new LinearFunction(first.Slope + second.Slope, first.YIntersect + second.YIntersect);

        /// <summary>
        /// Creates a new linear function by subtracting the given value from the function's y-intersect, i.e., lowering the line by this amount.
        /// </summary>
        /// <param name="first">A linear function.</param>
        /// <param name="second">A value by which to lower the linear function.</param>
        /// <returns>A new linear function.</returns>
        public static LinearFunction operator -(LinearFunction first, double second) => new LinearFunction(first.Slope, first.YIntersect - second);

        /// <summary>
        /// Creates a new linear function whose slope and y-intersect are equal to the values of the first argument function minus the values of the second argument function.
        /// </summary>
        /// <param name="first">A linear function.</param>
        /// <param name="second">A linear function by which to reduce the other function.</param>
        /// <returns>A new linear function.</returns>
        public static LinearFunction operator -(LinearFunction first, LinearFunction second) => new LinearFunction(first.Slope - second.Slope, first.YIntersect - second.YIntersect);

        /// <summary>
        /// Creates a new linear function by multiplying the given value by the function's slope and y-intersect, i.e., scaling the function up by the provided ratio.
        /// </summary>
        /// <param name="first">A linear function.</param>
        /// <param name="second">A value by which to scale the linear function.</param>
        /// <returns>A new linear function.</returns>
        public static LinearFunction operator *(LinearFunction first, double second) => new LinearFunction(first.Slope * second, first.YIntersect * second);

        /// <summary>
        /// Creates a new linear function by dividing the given value by the function's slope and y-intersect, i.e., scaling the function down by the provided ratio.
        /// </summary>
        /// <param name="first">A linear function.</param>
        /// <param name="second">A value by which to scale down the linear function.</param>
        /// <returns>A new linear function.</returns>
        public static LinearFunction operator /(LinearFunction first, double second) => new LinearFunction(first.Slope / second, first.YIntersect / second);

        /// <summary>
        /// Creates a piecewise function that has a value of 1.0 wherever the linear function is greater than the piecewise function and 0.0 otherwise.
        /// </summary>
        /// <param name="linearFunction">A linear function.</param>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <returns>A piecewise function representing the ranges for which the inequality is true.</returns>
        public static PiecewiseFunction operator >(LinearFunction linearFunction, PiecewiseFunction piecewiseFunction) => piecewiseFunction < linearFunction;

        /// <summary>
        /// Creates a piecewise function that has a value of 1.0 wherever the linear function is less than the piecewise function and 0.0 otherwise.
        /// </summary>
        /// <param name="linearFunction">A linear function.</param>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <returns>A piecewise function representing the ranges for which the inequality is true.</returns>
        public static PiecewiseFunction operator <(LinearFunction linearFunction, PiecewiseFunction piecewiseFunction) => piecewiseFunction > linearFunction;

        /// <summary>
        /// Creates a piecewise function that has a value of 1.0 wherever the linear function is greater than or equal to the piecewise function and 0.0 otherwise.
        /// </summary>
        /// <param name="linearFunction">A linear function.</param>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <returns>A piecewise function representing the ranges for which the inequality is true.</returns>
        public static PiecewiseFunction operator >=(LinearFunction linearFunction, PiecewiseFunction piecewiseFunction) => piecewiseFunction <= linearFunction;

        /// <summary>
        /// Creates a piecewise function that has a value of 1.0 wherever the linear function is less than or equal to the piecewise function and 0.0 otherwise.
        /// </summary>
        /// <param name="linearFunction">A linear function.</param>
        /// <param name="piecewiseFunction">A piecewise function.</param>
        /// <returns>A piecewise function representing the ranges for which the inequality is true.</returns>
        public static PiecewiseFunction operator <=(LinearFunction linearFunction, PiecewiseFunction piecewiseFunction) => piecewiseFunction >= linearFunction;
    }
}
