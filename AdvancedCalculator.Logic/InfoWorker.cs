using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedCalculator.Logic
{
    public class InfoWorker
    {
        public List<Calculator> Calculators { get; set; }
        public InfoWorker(string expression, string range, string step)
        {
            double dStep = double.Parse(step);
            List<double> lRange = SetRange(range, dStep);
            Calculators = SetCalcs(expression, lRange);
        } 
        List<Calculator> SetCalcs(string expression, List<double> lRange)
        {
                List<Calculator> calculators = new List<Calculator>();
                for (int i = 0; i < lRange.Count; i++)
                {
                    Calculator calculator = new Calculator("⊥" + expression + "⊥", lRange[i].ToString());
                    calculators.Add(calculator);
                }
                return calculators;
        }
        List<double> SetRange(string range, double step)
        {
            string[] info = range.Split("-");
            double doubleRange = double.Parse(info[1]) - double.Parse(info[0]) + 1;
            var ar = new List<double>();
            for (double i = 0; i < doubleRange; i += step)
            {
                ar.Add(i + double.Parse(info[0]));
            }
            return ar;
        }
    }
}
