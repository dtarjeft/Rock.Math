# Rock.Math
A set of math utilities, primarily for representing different types of math functions.

## PiecewiseFunction
Represents a piecewise linear function with each piece having a slope of 0.

Operator overloads exist for transforming a piecewise function by multiplying/dividing/adding/subtracting constant values, as well as interacting with other piecewise functions (in which case the operation is performed in a 'piecewise' manner, similar to a bitwise operation, where for a given x-value, the new y-value is the result of the operation when applied to the y-values of the two piecewise functions at that x-value).

Piecewise functions can also be treated as ranges for which something is true or false, where 0.0 is 'false' and any other y-value is 'true'. Based on this definition, there are also piecewise operations for 'not', 'and', and 'or'.

## LinearFunction
Represents a (non-piecewise) linear function with any slope.
