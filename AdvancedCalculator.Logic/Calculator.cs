using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdvancedCalculator.Logic
{
    public class Calculator
    {
        public List<object> RPNAr { get; private set; }
        public string RPNStr { get; private set; }
        public double Answer { get; private set; }
        public double X { get; private set; }
        public Calculator(RPN rpn, double x, int index, bool neg, string oldX)
        {
            X = x;
            if (neg == false)
                rpn.Rpn[index] = x;
            else
                 rpn.Rpn[index] = x * -1;
            RPNAr = rpn.Rpn;
            RPNStr = rpn.RPNStr.Replace(oldX, x.ToString()).Replace("--", "");
            Answer = GetAnswer();
        }
        double GetAnswer()
        {
            Stack<double> calc = new Stack<double>();
            for (int i = RPNAr.Count - 1; i >= 0; i--)
            {
                if (RPNAr[i] is double)
                    calc.Push(Convert.ToDouble(RPNAr[i]));
                else
                {
                    if ((RPNAr[i] as Operation).CountParams == 2)
                    {
                        double[] @params = { calc.Pop(), calc.Pop() };  //x2 , x1
                        calc.Push((RPNAr[i] as Operation).Calculate(@params));
                    }
                    else
                    {
                        double[] @params = { calc.Pop() };
                        calc.Push((RPNAr[i] as Operation).Calculate(@params));
                    }
                }
            }
            return calc.Pop();
        }
    }
}
