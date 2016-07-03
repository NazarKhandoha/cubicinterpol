using System;
using System.Linq;

namespace CS.Logic
{
    public class Calculator
    {
        public Spline FindSpline(int numberOfIntervals, double a, double b, Func<double, double> function,
            double leftBound = 0, double rightBound = 0)
        {
            if (numberOfIntervals < 2)
            {
                throw new ArgumentException("Number of intervals should not be less 2");
            }
            int numberOfSplines = numberOfIntervals + 1;

            var spline = new Spline(numberOfSplines)
            {
                Nodes = CalculateNodes(numberOfIntervals, a, b),
                A = a,
                B = b
            };

            double h = (b - a)/numberOfIntervals;

            var f = new double[numberOfSplines];
            for (int i = 0; i < numberOfSplines; i++)
            {
                f[i] = function(spline.Nodes[i]);
            }

            spline.Coefficients[0].A = f[0];
            spline.Coefficients[0].C = leftBound;
            spline.Coefficients[numberOfSplines - 1].C = rightBound;

            CalculateCoefficientsC(spline.Coefficients, f, h);

            for (int i = 1; i < numberOfSplines; i++)
            {
                spline.Coefficients[i].A = f[i];
                spline.Coefficients[i].B = (h/6)*(2*spline.Coefficients[i].C + spline.Coefficients[i - 1].C) +
                                           (f[i] - f[i - 1])/h;
                spline.Coefficients[i].D = (spline.Coefficients[i].C - spline.Coefficients[i - 1].C)/h;
            }
            return spline;
        }

        private void CalculateCoefficientsC(SplineCoefficients[] coefficients, double[] function, double h)
        {
            int numberOfSplines = coefficients.Length;
            var diagonal = new double[numberOfSplines, 3];
            var rightPart = new double[numberOfSplines];

            diagonal[0, 1] = 1;
            diagonal[numberOfSplines - 1, 1] = 1;
            rightPart[0] = coefficients[0].C;
            rightPart[numberOfSplines - 1] = coefficients[numberOfSplines - 1].C;

            for (int i = 1; i < numberOfSplines - 1; i++)
            {
                diagonal[i, 0] = h;
                diagonal[i, 1] = 4*h;
                diagonal[i, 2] = h;
            }

            for (int i = 1; i < numberOfSplines - 1; i++)
            {
                double[] f = function;
                rightPart[i] = 6*(f[i + 1] - 2*f[i] + f[i - 1])/h;
            }

            var result = Solve(diagonal, rightPart);

            for (int i = (numberOfSplines - 1) - 1; i >= 1; i--)
            {
                coefficients[i].C = result[i];
            }
        }

        private static double[] CalculateNodes(int numberOfIntervals, double a, double b)
        {
            int numberOfNodes = numberOfIntervals + 1;

            var nodes = new double[numberOfNodes];

            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes[i] = a + ((b - a) / numberOfIntervals) * i;
            }
            return nodes;
        }

        public double[] Solve(double[,] diagonal, double[] rightPart)
        {
            var n = diagonal.GetLength(0);
            var result = new double[n];
            var cModified = new double[n];
            var fModified = new double[n];
            ValidateMatrix(diagonal, n);

            {
                int i = 0;

                double c = diagonal[i, 1];
                double f = rightPart[i];

                cModified[i] = c;
                fModified[i] = f;
            }

            for (int i = 1; i < n; i++)
            {
                double a = diagonal[i, 0];
                double c = diagonal[i, 1];
                double b = diagonal[i-1, 2];
                double f = rightPart[i];

                cModified[i] = c - a / cModified[i - 1] * b;
                fModified[i] = f - a * fModified[i - 1] / cModified[i - 1];
            }

            result[n - 1] = fModified[n - 1]/cModified[n - 1];
            for (int i = (n - 1) - 1; i >= 0; i--)
            {
                double b = diagonal[i, 2];
                result[i] = (fModified[i] - b * result[i + 1]) / cModified[i];
            }
            return result;
        }

        private static void ValidateMatrix(double[,] diagonal, int n)
        {
            if (diagonal[0, 0] != 0)
            {
                throw new ArgumentException("A[0] must be 0");
            }
            if (diagonal[n - 1, 2] != 0)
            {
                throw new ArgumentException("B[n-1] must be 0");
            }
        }

        public ComparisonTable Compare(Func<double, double> function, Func<double, double> derivative, Spline spline, int numberOfIntervals)
        {
            var comparisonTable = new ComparisonTable
            {
                NumberOfComparisonIntervals = numberOfIntervals,
                NumberOfSplineIntervals = spline.Coefficients.Length - 1
            };
            var nodes = CalculateNodes(numberOfIntervals, spline.A, spline.B);
            comparisonTable.Values = new ComparisonTableLine[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                comparisonTable.Values[i] = new ComparisonTableLine
                {
                    Number = i,
                    X = nodes[i],
                    Function = function(nodes[i]),
                    Spline = spline.Value(nodes[i]),
                    FunctionDerivative = derivative(nodes[i]),
                    SplineDerivative = spline.DerivativeValue(nodes[i])
                };

                comparisonTable.Values[i].AbsDifference = Math.Abs(
                    comparisonTable.Values[i].Function - comparisonTable.Values[i].Spline);

                comparisonTable.Values[i].AbsDerivativesDifference = Math.Abs(
                    comparisonTable.Values[i].FunctionDerivative 
                    - comparisonTable.Values[i].SplineDerivative);
                
            }

            comparisonTable.MaximumDifference = comparisonTable.Values.Select(_ => _.AbsDifference).Max();
            comparisonTable.MaximumDerivativesDifference =
                comparisonTable.Values.Select(_ => _.AbsDerivativesDifference).Max();
            return comparisonTable;
        }
    }

    public class ComparisonTable
    {
        public ComparisonTableLine[] Values { get; set; }
        public int NumberOfSplineIntervals { get; set; }
        public int NumberOfComparisonIntervals { get; set; }
        public double MaximumDifference { get; set; }
        public double MaximumDerivativesDifference { get; set; }
    }

    public class ComparisonTableLine
    {
        public int Number { get; set; }
        public double X { get; set; }
        public double Function { get; set; }
        public double Spline { get; set; }
        public double AbsDifference { get; set; }
        public double FunctionDerivative { get; set; }
        public double SplineDerivative { get; set; }
        public double AbsDerivativesDifference { get; set; }
    }
}