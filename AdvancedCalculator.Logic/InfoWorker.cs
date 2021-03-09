using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedCalculator.Logic
{
    public class InfoWorker
    {
        public List<Calculator> Calculators { get; set; }
        public List<double> Range { get; private set; }
        public InfoWorker(string expression, string range, string step)
        {
            double dStep = double.Parse(step);
            Range = SetRange(range, dStep);
            Calculators = SetCalcs(expression);
        } 
        List<Calculator> SetCalcs(string expression)
        {
            List<Calculator> calculators = new List<Calculator>();
            RPN rpn = new RPN("⊥" + expression + "⊥");
            int index = 0;
            bool neg = false;
            string oldX = "x";
            if (rpn.Rpn.Contains('x'))
            {
                index = rpn.Rpn.IndexOf('x');
            }
            else 
            {
                index = rpn.Rpn.IndexOf("-x");
                neg = true;
                oldX = "-x";
            }
            for (int i = 0; i < Range.Count; i++)
            {
                Calculator calculator = new Calculator(rpn, Range[i], index, neg, oldX);
                calculators.Add(calculator);
                oldX = Range[i].ToString();
            }
                return calculators;
        }
        List<double> SetRange(string range, double step)
        {
            string[] info = range.Split("--");
            double doubleRange = double.Parse(info[1]) - double.Parse(info[0]) + 1;
            int afterq = 0;
            if (step.ToString().Contains(","))
                afterq = step.ToString().Split(",")[1].Length;
            else
                afterq = 0;
            var ar = new List<double>();
            for (double i = 0; i < doubleRange; i += step)
            {
                i = Math.Round(i, afterq);
                ar.Add(Math.Round(i + double.Parse(info[0]), afterq));
            }
            return ar;
        }
    }
}
