namespace CS.Logic
{
    public class SplineViewModel
    {
        public SplineViewModel(Spline spline)
        {
            var numberOfSplines = spline.Coefficients.Length - 1;
            Coefficients = new SplineCoefficientsViewModel[numberOfSplines];
            for (var i = 0; i < numberOfSplines; i++)
            {
                Coefficients[i] = new SplineCoefficientsViewModel
                {
                    A = spline.Coefficients[i + 1].A,
                    B = spline.Coefficients[i + 1].B,
                    C = spline.Coefficients[i + 1].C,
                    D = spline.Coefficients[i + 1].D,
                    LeftX = spline.Nodes[i],
                    RightX = spline.Nodes[i + 1],
                    Number = i + 1
                };
            }
        }

        public SplineCoefficientsViewModel[] Coefficients { get; private set; }
    }
}