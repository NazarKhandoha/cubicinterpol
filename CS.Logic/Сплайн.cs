namespace CS.Logic
{
    public class Spline
    {
        internal Spline(int numberOfSplines)
        {
            Coefficients = new SplineCoefficients[numberOfSplines];

            for (int i = 0; i < numberOfSplines; i++)
            {
                Coefficients[i] = new SplineCoefficients();
            }
        }

        public SplineCoefficients[] Coefficients { get; set; }
        public double[] Nodes { get; set; }
        public double A { get; set; }
        public double B { get; set; }

        public double Value(int interval, double x)
        {
            var a = Coefficients[interval].A;
            var b = Coefficients[interval].B;
            var c = Coefficients[interval].C;
            var d = Coefficients[interval].D;
            var xi = Nodes[interval];
            var dx = x - xi;

            return a + b * dx + (1.0/2.0) * c * dx * dx + (1.0/6.0) * d * dx * dx * dx;
        }

        public double Value(double x)
        {
            var intervalIndex = GetIntervalIndex(x);

            var value = Value(intervalIndex, x);

            return value;
        }

        private int GetIntervalIndex(double x)
        {
            var a = Nodes[0];
            var b = Nodes[Nodes.Length - 1];

            var numberOfIntervals = Coefficients.Length - 1;
            var intervalX = (x - a);
            var intervalIndex = (int) ((intervalX/(b - a))*numberOfIntervals) + 1;

            if (x >= b)
            {
                intervalIndex = numberOfIntervals;
            }
            return intervalIndex;
        }

        public double DerivativeValue(int interval, double x)
        {
            var b = Coefficients[interval].B;
            var c = Coefficients[interval].C;
            var d = Coefficients[interval].D;
            var xi = Nodes[interval];
            var dx = x - xi;

            return b  +  c * dx + (1.0 / 2.0) * d * dx * dx;
        }

        public double DerivativeValue(double x)
        {
            var intervalIndex = GetIntervalIndex(x);
            var value = DerivativeValue(intervalIndex, x);
            return value;
        }

    }
}