using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedCalculator.Logic
{
    public class Calculator
    {
        string[] RPNAr { get; set; }
        public string RPNStr { get; private set; }
        public double Answer { get; private set; } = 0;
        public Calculator(string text)
        {
            RPNStr = ParseInRPN(text);
            Answer = Calculate();
        }
        bool CheckDigit(string text)                       //Проверяем на число
        {
            if (double.TryParse(text, out _))
                return true;
            return false;
        }
        bool CheckPlusMinus(string text)                                //Проверяем на операнды +-
        {
            if ("+-".Contains(text))
                return true;
            return false;
        }
        bool CheckMultDiv(string text)                                //Проверяем на операнды */
        {
            if ("*/".Contains(text))
                return true;
            return false;
        }
        bool CheckStartEnd(string text)                                //Проверяем на символ начала/конца выражения
        {
            if (text == "⊥")
                return true;
            return false;
        }
        bool CheckLetter(string text)
        {
            if (!CheckDigit(text.ToString()) && !CheckMultDiv(text) && !CheckPlusMinus(text) && !CheckStartEnd(text) && text != "(" && text != ")" && text != "^")
                return true;
            return false;
        }
        bool CheckFunc(string text)
        {
            if (text == "log" || text == "sin" || text == "cos" || text == "tg" || text == "ctg" || text == "ln")
                return true;
            return false;
        }
        string ParseInRPN(string text)
        {
            Stack<string> california = new Stack<string>();
            Stack<string> texas = new Stack<string>();
            StringBuilder output = new StringBuilder();
            int i = 0;
            while (true)
            {
                if (i == 0)
                {
                    texas.Push(text[i].ToString());                                                                
                    i++;
                }
                else if (CheckDigit(text[i].ToString()))                                                            //Если число
                {
                    StringBuilder num = new StringBuilder().Append(text[i]);
                    while (CheckDigit(text[i + 1].ToString()) || text[i + 1] == ',')                                                     
                    {
                        i++;
                        num.Append(text[i]);
                    }
                    california.Push(num.ToString());
                    i++;
                }
                else if (CheckPlusMinus(text[i].ToString()))                                                       //Если +-
                {
                    if (CheckStartEnd(texas.Peek()))
                    {
                        texas.Push(text[i].ToString());
                        i++;
                    }
                    else if (CheckPlusMinus(texas.Peek()) || CheckMultDiv(texas.Peek()) || texas.Peek() == "^" || CheckFunc(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == "(") 
                    {
                        texas.Push(text[i].ToString());
                        i++;
                    }
                }
                else if (CheckMultDiv(text[i].ToString()))                                                          //Если */
                {
                    if (CheckStartEnd(texas.Peek()) || CheckPlusMinus(texas.Peek()))
                    {
                        texas.Push(text[i].ToString());
                        i++;
                    }
                    else if (CheckMultDiv(texas.Peek()) || texas.Peek() == "^" || CheckFunc(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == "(")
                    {
                        texas.Push(text[i].ToString());
                        i++;
                    }
                }
                else if (text[i] == '(')
                {
                    texas.Push(text[i].ToString());
                    i++;
                }
                else if (text[i] == ')')
                {
                    if (CheckStartEnd(texas.Peek()))                                                                        //ошибка
                    {
                        throw new Exception("Некорректная формула");
                    }
                    else if (CheckPlusMinus(texas.Peek()) || CheckMultDiv(texas.Peek()) || texas.Peek() == "^" || CheckFunc(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == "(")
                    {
                        texas.Pop();
                        i++;
                    }
                }
                else if (CheckStartEnd(text[i].ToString()))
                {
                    if (CheckStartEnd(texas.Peek()))
                        break;
                    else if (CheckPlusMinus(texas.Peek()) || CheckMultDiv(texas.Peek()) || texas.Peek() == "^" || CheckFunc(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == "(")                                                                           //ошибка
                    {
                        throw new Exception("Некорректная формула");
                    }
                }
                else if (text[i] == '^')
                {
                    texas.Push(text[i].ToString());
                    i++;
                }
                else if (CheckLetter(text[i].ToString()))
                {
                    StringBuilder func = new StringBuilder().Append(text[i]);
                    while (CheckLetter(text[i + 1].ToString()))
                    {
                        i++;
                        func.Append(text[i]);
                    }
                    texas.Push(func.ToString());
                    i++;
                }
            }                                                                          
            RPNAr = california.ToArray();
            List<string> outputs = new List<string>(california);
            outputs.Reverse();
            for (int k = 0; k < outputs.Count; k++)
            {
                output.Append(outputs[k] + " ");
            }
            return output.ToString();
        }
        double Calculate()
        {
            Stack<string> calc = new Stack<string>();
            for (int i = RPNAr.Length - 1; i >= 0; i--)
            {
                if (CheckDigit(RPNAr[i]))
                    calc.Push(RPNAr[i]);
                else
                {
                    double x2 = Convert.ToDouble(calc.Pop());
                    double x1 = Convert.ToDouble(calc.Pop());
                    if (RPNAr[i] == "+")
                        calc.Push((x1 + x2).ToString());
                    else if (RPNAr[i] == "-")
                        calc.Push((x1 - x2).ToString());
                    else if (RPNAr[i] == "*")
                        calc.Push((x1 * x2).ToString());
                    else if (RPNAr[i] == "/")
                        calc.Push((x1 / x2).ToString());
                    else if (RPNAr[i] == "^")
                        calc.Push((Math.Pow(x1, x2)).ToString());
                    else if (RPNAr[i] == "log")
                        calc.Push((Math.Log(x2, x1).ToString()));
                }
            }
            return double.Parse(calc.Pop());
        }
    }
}
