using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChebyshevForm
{
    public partial class Form1 : Form
    {
        Chebyshev cheb;
        double start, end;  // Start and end of domain to do the approximation on

        public Form1()
        {
            InitializeComponent();

            start = 0;
            end = 0.5;
        }

        /// <summary>
        /// Function to approximate
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        private double function(double xx)
        {
            //return Math.Exp(xx);
            return Math.Acos(xx);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            double x1, x2, error;

            if (int.TryParse(textBox1.Text.Trim(), out n))
            {
                cheb = new Chebyshev(n, start, end, function);
            }

            StringBuilder strb = new StringBuilder();

            error = 0;

            for (double xx = start; xx <= end; xx+= (end - start)/10)
            {
                x1 = function(xx);
                x2 = cheb.evaluate(xx);
                strb.Append(string.Format("{0:f6} {1:f6} {2:f6} {3:f6}\n", xx, x1, x2, Math.Abs(x1 - x2)));
                error = Math.Max(error, Math.Abs(x1 - x2));
            }
            strb.Append(string.Format("Max error = {0}", error));

            strb.Append("\n\n");

            for (int i = 0; i < n; i++)
            {
                strb.Append(string.Format("c[{0}] = {1}\n", i, cheb.coefs[i]));
            }

            strb.Append(string.Format("\nstart = {0};\n", start));
            strb.Append(string.Format("end = {0};\n", end));
            strb.Append("\n\n");

            strb.Append(string.Format("double func(double x)\n{{\n"));
            strb.Append(string.Format("    int i;\n"));
            strb.Append(string.Format("    double y, z;\n\n"));
            strb.Append(string.Format("    y = (2 * x - end - start) / (start - end);\n"));
            strb.Append(string.Format("    z = 0;\n\n"));
            strb.Append(string.Format("    for (i = 0; i < {0}; i++)\n", n));
            strb.Append(string.Format("        z += c[i] * Tn(i, y);\n\n"));
            strb.Append(string.Format("    z -= c[0] / 2;\n", cheb.coefs[0]));
            strb.Append(string.Format("    return z\n"));
            strb.Append(string.Format("}}\n"));

            strb.Append("\n\n");
            strb.Append("private double Tn(int n, double x)\n");
            strb.Append("{\n");
            strb.Append("    double x0 = 1;\n");
            strb.Append("    double x1 = x;\n");
            strb.Append("    double xx = 0;\n");
            strb.Append("    \n");
            strb.Append("    switch (n)\n");
            strb.Append("    {\n");
            strb.Append("        case 0:\n");
            strb.Append("            return 1;\n");
            strb.Append("        case 1:\n");
            strb.Append("            return x;\n");
            strb.Append("        default:\n");
            strb.Append("            for (int i = 2; i <= n; i++)\n");
            strb.Append("            {\n");
            strb.Append("                xx = 2 * x * x1 - x0;\n");
            strb.Append("                x0 = x1;\n");
            strb.Append("                x1 = xx;\n");
            strb.Append("            }\n");
            strb.Append("            return xx;\n");
            strb.Append("    }\n");
            strb.Append("}\n");



            richTextBox1.Text = strb.ToString();
        }
    }
}
