using System;
using CS.Logic;
using NUnit.Framework;

namespace CS.Test
{
    public class Expect
    {
        public static void Exception<T>(Action action) where T: Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T exception)
            {
                return;
            }
            Assert.Fail(String.Format("{0} expected", typeof(T).ToString()));
        }

        public static void NoException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        public static void CoefficientEquals(Spline spline, int interval, double[] coeffs)
        {
            Assert.AreEqual(coeffs[0], spline.Coefficients[interval].A);
            Assert.AreEqual(coeffs[1], spline.Coefficients[interval].B);
            Assert.AreEqual(coeffs[2], spline.Coefficients[interval].C);
            Assert.AreEqual(coeffs[3], spline.Coefficients[interval].D);
        }

        public static void FloatsAreEqual(double expect, double actual)
        {
            const double tolerance = 0.00001;
            Assert.LessOrEqual(Math.Abs(expect - actual), tolerance, string.Format("floats not same. actual: {0} expected: {1}", actual, expect));
        }

        public static void FunctionsSameOnInterval(Func<double, double> function, Func<double, double> function2, double a, double b)
        {
            const int numberOfNodes = 1000;
            for (int i = 0; i < numberOfNodes; i++)
            {
                double x = a + ((b - a) / (numberOfNodes - 1)) * i;
                Expect.FloatsAreEqual(function(x), function2(x));
            }
        }
    }
}