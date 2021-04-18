using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdvancedCalculator.Logic
{
    public class Calculator
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public string RPNStr { get; private set; }
        public Calculator(string x)
        {
            X = double.Parse(x);
            RPN.RPNAr[RPN.Index] = X;
            RPNStr = GetRPNStr();
            Y = GetAnswer();
        }
        
        string GetRPNStr()
        {
            StringBuilder @string = new StringBuilder();
            for (int i = RPN.RPNAr.Length - 1; i >= 0; i--)
            {
                if (RPN.RPNAr[i] is Operation)
                    @string.Append((RPN.RPNAr[i] as Operation).Name + " ");
                else
                    @string.Append(RPN.RPNAr[i].ToString() + " ");
            }
            return @string.ToString();
        }
        double GetAnswer()
        {
            Stack<double> calc = new Stack<double>();
            for (int i = RPN.RPNAr.Length - 1; i >= 0; i--)
            {
                if (RPN.RPNAr[i] is double)
                    calc.Push(Convert.ToDouble(RPN.RPNAr[i]));
                else
                {
                    if ((RPN.RPNAr[i] as Operation).CountParams == 2)
                    {
                        double[] @params = { calc.Pop(), calc.Pop() };  //x2 , x1
                        calc.Push((RPN.RPNAr[i] as Operation).Calculate(@params));
                    }
                    else
                    {
                        double[] @params = { calc.Pop() };
                        calc.Push((RPN.RPNAr[i] as Operation).Calculate(@params));
                    }
                }
            }
            return calc.Pop();
        }
    }
}
