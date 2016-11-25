using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChebyshevForm
{
    class Chebyshev
    {
        public int N;                      
        public double[] coefs;
        public double start, end;
        public Func<double, double> function;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">Number of coefficients to be used.</param>
        /// <param name="start">start of the domain</param>
        /// <param name="end">end of the domain</param>
        /// <param name="function">function to approximate.</param>
        public Chebyshev(int n, double start, double end, Func<double, double> function)
        {
            N = n;
            this.start = start;
            this.end = end;
            this.function = function;

            createCoef();
        }

        /// <summary>
        /// Creates the coefficients for the function approximation.
        /// </summary>
        private void createCoef()
        {
            coefs = new double[N];

            for (int j = 0; j < N; j++)
            {
                coefs[j] = 0;

                for (int k = 1; k <= N; k++)
                {
                    coefs[j] += function(scaleFromUnity(Math.Cos(Math.PI * (k - 0.5) / N))) * Math.Cos(Math.PI * j * (k - 0.5) / N);
                }

                coefs[j] = coefs[j] * 2 / N;
            }
        }

        /// <summary>
        /// Evaluation of Chebicgev polybomials for the Chebichev nodes.
        /// Not used for now...
        /// </summary>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private double Tn(int j, int k)
        {
            return Math.Cos(Math.PI * j * (k - 0.5) / N);
        }

        /// <summary>
        /// Recursive version of the evaluation of the Chebychef polynomials.
        /// </summary>
        /// <param name="n">Order of polynomail to evaluate</param>
        /// <param name="x">Value to evaluate on</param>
        /// <returns>result.</returns>
        private double Tn_(int n, double x)
        {
            double xx = 0;

            switch (n)
            {
                case 0:
                    xx = 1;
                    break;
                case 1:
                    xx = x;
                    break;
                default:
                    xx = 2 * x * Tn_(n - 1, x) - Tn_(n - 2, x);
                    break;
            }
            return xx;
        }

        /// <summary>
        /// Iterative version of the evaluation of the Chebychef polynomials.
        /// </summary>
        /// <param name="n">Order of polynomail to evaluate</param>
        /// <param name="x">Value to evaluate on</param>
        /// <returns>result.</returns>
        private double Tn(int n, double x)
        {
            double x0 = 1;
            double x1 = x;
            double xx = 0;

            switch (n)
            {
                case 0:
                    xx = 1;
                    break;
                case 1:
                    xx = x;
                    break;
                default:
                    for (int i = 2; i <= n; i++)
                    {
                        xx = 2 * x * x1 - x0;
                        x0 = x1;
                        x1 = xx;
                    }
                    break;                    
            }
            return xx;
        }

        /// <summary>
        /// This is used to calculate the coefficients.
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        private double scaleFromUnity(double xx)
        {
            return ((start - end) * xx + start + end) / 2;
        }

        /// <summary>
        /// Takes the bounds of the functon to evaluate and scales it to [-1, 1]
        /// </summary>
        /// This is necessary as the Chebychev approximation only works in [-1,1]
        /// So any function to be approximated must first be scaled to this domain.
        /// Note: this means that functions with unbounded domains must undergo
        /// a transformation to a bounded one before doing this approximation.
        /// <param name="xx">value to scale</param>
        /// <returns>scaled value</returns>
        private double scaleToUnity(double xx)
        {
            return (2 * xx - end - start) / (start - end);
        }

        /// <summary>
        /// Evaluates the Chebyshev polynomial.
        /// </summary>
        /// <param name="x">Value to evaluate</param>
        /// <returns>he evaluated function</returns>
        public double evaluate(double x)
        {
            double xx = 0;

            x = scaleToUnity(x);

            for (int i = 0; i < N; i++)
            {
                xx += coefs[i] * Tn(i, x);
            }

            xx -= coefs[0] / 2;

            return xx;
        }
    }
}
