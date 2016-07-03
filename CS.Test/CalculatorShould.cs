using System;
using CS.Logic;
using NUnit.Framework;

namespace CS.Test
{

    [TestFixture]
    public class SplineViewModelShould
    {
        [Test]
        public void ProvideCoefficientsTable()
        {
            var calculator = new Calculator();
            var function = new Func<double, double>(x => 6*x*x*x*x);
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 2, function: function);
            var splineViewModel = new SplineViewModel(spline);

            Assert.AreEqual(1, splineViewModel.Coefficients[0].Number);
            Assert.AreEqual(0, splineViewModel.Coefficients[0].LeftX);
            Assert.AreEqual(1, splineViewModel.Coefficients[0].RightX);

            Assert.AreEqual(spline.Coefficients[1].A, splineViewModel.Coefficients[0].A);
            Assert.AreEqual(spline.Coefficients[1].B, splineViewModel.Coefficients[0].B);
            Assert.AreEqual(spline.Coefficients[1].C, splineViewModel.Coefficients[0].C);
            Assert.AreEqual(spline.Coefficients[1].D, splineViewModel.Coefficients[0].D);

            Assert.AreEqual(2, splineViewModel.Coefficients[1].Number);
            Assert.AreEqual(1, splineViewModel.Coefficients[1].LeftX);
            Assert.AreEqual(2, splineViewModel.Coefficients[1].RightX);

            Assert.AreEqual(spline.Coefficients[2].A, splineViewModel.Coefficients[1].A);
            Assert.AreEqual(spline.Coefficients[2].B, splineViewModel.Coefficients[1].B);
            Assert.AreEqual(spline.Coefficients[2].C, splineViewModel.Coefficients[1].C);
            Assert.AreEqual(spline.Coefficients[2].D, splineViewModel.Coefficients[1].D);
        }
    }

    [TestFixture]
    public class CalculatorShould
    {
        private Calculator calculator;
        private readonly Func<double, double> constantZero = x => 0;

        [SetUp]
        public void SetUp()
        {
            calculator = new Calculator();
        }

        [Test]
        public void ReturnTrivialSpline()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero);

            Expect.CoefficientEquals(spline, 0, new double[]{0, 0, 0, 0});
            Expect.CoefficientEquals(spline, 1, new double[]{0, 0, 0, 0});
            Expect.CoefficientEquals(spline, 2, new double[]{0, 0, 0, 0});

            Assert.AreEqual(spline.Nodes, new[]{0, 0.5, 1});
        }

        [Test]
        public void SplitIntervalByNodes()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero);

            Assert.AreEqual(3, spline.Coefficients.Length);
            Assert.AreEqual(3, spline.Nodes.Length);
            Assert.AreEqual(new[] {0, 0.5, 1}, spline.Nodes);
        }

        [Test]
        public void InterpolateTrivialFunctionExactly()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(0, spline.Value(0.25));
            Assert.AreEqual(0, spline.Value(0.75));
            Assert.AreEqual(0, spline.Value(1));
        }

        [Test]
        public void InterpolateLinearFunctionExactly()
        {
            var function = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 2, function: function);

            Assert.AreEqual(0, spline.Value(0));
            Assert.AreEqual(0.5, spline.Value(0.5));
            Assert.AreEqual(1, spline.Value(1));
            Assert.AreEqual(1.5, spline.Value(1.5));
            Assert.AreEqual(2, spline.Value(2));
        }


        [Test]
        public void InterpolateSquareFunctionExactly()
        {
            var function = new Func<double, double>(x => x*x);
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: -2, b: 2, function: function, leftBound: 2, rightBound: 2);

            Expect.FloatsAreEqual(0, spline.Value(0));
            
            Expect.FloatsAreEqual(0.25, spline.Value(0.5));
            Expect.FloatsAreEqual(0.25, spline.Value(-0.5));
            
            Expect.FloatsAreEqual(1, spline.Value(1));
            Expect.FloatsAreEqual(1, spline.Value(-1));

            Expect.FloatsAreEqual(2.25, spline.Value(1.5));
            Expect.FloatsAreEqual(2.25, spline.Value(-1.5));

            Expect.FloatsAreEqual(4, spline.Value(2));
            Expect.FloatsAreEqual(4, spline.Value(-2));
        }

        [Test]
        public void InterpolateCubicFunctionExactly()
        {
            var function = new Func<double, double>(x => x*x*x - 10*x*x - 2*x - 2);
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: -2, b: 2, function: function, leftBound: -32, rightBound: -8);
            const double numberOfNodes = 100;
            Expect.FunctionsSameOnInterval(function, spline.Value, a: -2, b: 2);
        }
        
        [Test]
        public void UseSmoothnessConditions()
        {
            var function = new Func<double, double>(x => 4 * x * x - 2);
            var spline = calculator.FindSpline(numberOfIntervals: 6, a: -4, b: 4, function: function, leftBound: 4, rightBound: 4);
            for (int i = 1; i < 6; i++)
            {
                var x = spline.Nodes[i - 1];
                Expect.FloatsAreEqual(spline.Value(i - 1, x), spline.Value(i, x));
            }
        }

        [Test]
        public void UseSmoothnessConditions2()
        {
            var function = new Func<double, double>(x => 4 * x - 2);
            var spline = calculator.FindSpline(numberOfIntervals: 4, a: -4, b: 4, function: function);
            for (int i = 1; i < 4; i++)
            {
                var x = spline.Nodes[i - 1];
                Assert.AreEqual(spline.Value(i - 1, x), spline.Value(i, x));
            }
        }

        [Test]
        public void SetSplineCoeffAEqualsFunctionValue()
        {
            var function = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 3, a: 0, b: 3, function: function);

            Assert.AreEqual(0, spline.Coefficients[0].A);
            Assert.AreEqual(1, spline.Coefficients[1].A);
            Assert.AreEqual(2, spline.Coefficients[2].A);
            Assert.AreEqual(3, spline.Coefficients[3].A);
        }

        [Test]
        public void AcceptBoundaryConditions()
        {
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero, leftBound: 5,
                rightBound: 6);
            Assert.AreEqual(5, spline.Coefficients[0].C);
            Assert.AreEqual(6, spline.Coefficients[2].C);
        }

        [Test]
        public void DoNotAcceptLessThan2Intervals()
        {
            Expect.Exception<ArgumentException>(() => calculator.FindSpline(numberOfIntervals: 0, a: 0, b: 1, function: constantZero));
            Expect.Exception<ArgumentException>(() => calculator.FindSpline(numberOfIntervals: 1, a: 0, b: 1, function: constantZero));
            Expect.NoException(() => calculator.FindSpline(numberOfIntervals: 2, a: 0, b: 1, function: constantZero));
        }

        [Test]
        public void ProvideTridiagonalMatrixAlgorithm()
        {
            var rightPart = new double[] { 1, 1, 1 };
            var diagonal1 = new double[,]
            {
                {1, 1, 1},
                {1, 2, 1},
                {1, 1, 0},
            };
            Expect.Exception<ArgumentException>(() => calculator.Solve(diagonal1, rightPart));
            var diagonal2 = new double[,]
            {
                {0, 1, 1},
                {1, 2, 1},
                {1, 1, 1},
            };
            Expect.Exception<ArgumentException>(() => calculator.Solve(diagonal2, rightPart));
        }

        [Test]
        public void ProvideTridiagonalMatrixAlgorithm2()
        {
            var diagonal = new double[,]
            {
                {0,-2, 1},
                {1, 1, 1},
                {1, 1, 0},
            };
            var rightPart = new double[]
            {
                1, 
                1, 
                1
            };
            var result = calculator.Solve(diagonal, rightPart);

            Assert.AreEqual(new[] { 0, 1, 0 }, result);
        }

        [Test]
        public void ShouldCanCompareFunctionAndSpline()
        {
            var function = new Func<double, double>(x => 1);
            var fakeFunction = new Func<double, double>(x => 0);
            var spline = calculator.FindSpline(numberOfIntervals: 4, a: 0, b: 1, function: fakeFunction);
            const int numberOfIntervals = 2;
            var comparisonTable = calculator.Compare(function, constantZero, spline, numberOfIntervals);
            Assert.AreEqual(numberOfIntervals + 1, comparisonTable.Values.Length);
            
            Assert.AreEqual(0, comparisonTable.Values[0].Number);
            Assert.AreEqual(0, comparisonTable.Values[0].X);      
  
            Assert.AreEqual(1, comparisonTable.Values[1].Number);
            Assert.AreEqual(.5,comparisonTable.Values[1].X);

            Assert.AreEqual(2, comparisonTable.Values[2].Number);
            Assert.AreEqual(1, comparisonTable.Values[2].X);

            for (int i = 0; i < numberOfIntervals; i++)
            {
                Assert.AreEqual(1, comparisonTable.Values[i].Function);
                Assert.AreEqual(0, comparisonTable.Values[i].Spline);
                Assert.AreEqual(1, comparisonTable.Values[i].AbsDifference);
                Assert.AreEqual(0, comparisonTable.Values[i].FunctionDerivative);
                Assert.AreEqual(0, comparisonTable.Values[i].SplineDerivative);
                Assert.AreEqual(0, comparisonTable.Values[i].AbsDerivativesDifference);
            }

            Assert.AreEqual(4, comparisonTable.NumberOfSplineIntervals);
            Assert.AreEqual(2, comparisonTable.NumberOfComparisonIntervals);
            
            Assert.AreEqual(1, comparisonTable.MaximumDifference);
            Assert.AreEqual(0, comparisonTable.MaximumDerivativesDifference);

        }

        [Test]
        public void CanCalculateSplineDerivative()
        {
            var function = new Func<double, double>(x => x*x*x + x*x + 5*x ); 
            var derivative = new Func<double, double>(x => 3*x*x + 2*x + 5 );
            var spline = calculator.FindSpline(numberOfIntervals: 2, a: -2, b: 2, function: function, leftBound: -10,
                rightBound: 14);
            Expect.FunctionsSameOnInterval(function, spline.Value, -2, 2);
            Expect.FunctionsSameOnInterval(derivative, spline.DerivativeValue, -2, 2);

            function = new Func<double, double>(x => x * x * 0.5);
            derivative = new Func<double, double>(x => x);
            spline = calculator.FindSpline(numberOfIntervals: 2, a: -3, b: 3, function: function, leftBound: 1, rightBound: 1);
            Expect.FunctionsSameOnInterval(function, spline.Value, -1, 3);
            Expect.FunctionsSameOnInterval(derivative, spline.DerivativeValue, -1, 3);
        }

        [Test]
        public void CanCompareFunctionDerivativeAndSplineDerivative()
        {
            var function = new Func<double, double>(x => 5*x);
            var derivative = new Func<double, double>(x => 5);
            var fakeFunction = new Func<double, double>(x => x * x * 0.5);
            var fakeDerivative = new Func<double, double>(x => x);
            var spline = calculator.FindSpline(numberOfIntervals: 4, a: -1, b: 3, function: fakeFunction, leftBound: 1, rightBound: 1);
            
            const int numberOfIntervals = 2;
            var comparisonTable = calculator.Compare(function, derivative, spline, numberOfIntervals);
            Assert.AreEqual(6, comparisonTable.MaximumDerivativesDifference);
            
        }
    }
}
