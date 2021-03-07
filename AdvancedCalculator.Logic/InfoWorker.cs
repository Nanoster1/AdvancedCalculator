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
            string[] info = range.Split("--");
            double doubleRange = double.Parse(info[1]) - double.Parse(info[0]) + 1;
            var ar = new List<double>();
            bool c = true;
            for (double i = 0; i < doubleRange; i += step)
            {
                if (c && i > Math.Abs(double.Parse(info[0])) && CheckOnSwapSign(info[0], info[1]))
                {
                    i = Math.Abs(double.Parse(info[0]));
                    c = false;
                }
                ar.Add(i + double.Parse(info[0]));
            }
            return ar;
        }
        bool CheckOnSwapSign(string s1, string s2)
        {
            if (s1.Contains("-") && !s2.Contains("-"))
                return true;
            return false;
        }
    }
}
