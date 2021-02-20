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
            if (int.TryParse(text, out _))
                return true;
            return false;
        }
        bool CheckPlusMinus(char text)                                //Проверяем на операнды +-
        {
            if ("+-".Contains(text.ToString()))
                return true;
            return false;
        }
        bool CheckMultDiv(char text)                                //Проверяем на операнды */
        {
            if ("*/".Contains(text.ToString()))
                return true;
            return false;
        }
        bool CheckStartEnd(char text)                                //Проверяем на символ начала/конца выражения
        {
            if (text == '⊥')
                return true;
            return false;
        }
        string ParseInRPN(string text)
        {
            Stack<string> california = new Stack<string>();
            Stack<char> texas = new Stack<char>();
            StringBuilder output = new StringBuilder();
            int i = 0;
            while (true)
            {
                if (i == 0)
                {
                    texas.Push(text[i]);                                                                
                    i++;
                }
                else if (CheckDigit(text[i].ToString()))                                                            //Если число
                {
                    StringBuilder num = new StringBuilder().Append(text[i]);
                    while (CheckDigit(text[i + 1].ToString()))                                                     
                    {
                        i++;
                        num.Append(text[i]);
                    }
                    california.Push(num.ToString());
                    i++;
                }
                else if (CheckPlusMinus(text[i]))                                                       //Если +-
                {
                    if (CheckStartEnd(texas.Peek()))
                    {
                        texas.Push(text[i]);
                        i++;
                    }
                    else if (CheckPlusMinus(texas.Peek()) || CheckMultDiv(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == '(') 
                    {
                        texas.Push(text[i]);
                        i++;
                    }
                }
                else if (CheckMultDiv(text[i]))                                                          //Если */
                {
                    if (CheckStartEnd(texas.Peek()) || CheckPlusMinus(texas.Peek()))
                    {
                        texas.Push(text[i]);
                        i++;
                    }
                    else if (CheckMultDiv(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == '(')
                    {
                        texas.Push(text[i]);
                        i++;
                    }
                }
                else if (text[i] == '(')
                {
                    texas.Push(text[i]);
                    i++;
                }
                else if (text[i] == ')')
                {
                    if (CheckStartEnd(texas.Peek())) //ошибка
                    {
                        throw new Exception("Некорректная формула");
                    }
                    else if (CheckPlusMinus(texas.Peek()) || CheckMultDiv(texas.Peek()))
                    {
                        california.Push(texas.Pop().ToString());
                    }
                    else if (texas.Peek() == '(')
                    {
                        texas.Pop();
                        i++;
                    }
                }
                else if (CheckStartEnd(text[i]))
                {
                    if (CheckStartEnd(texas.Peek()))
                        break;
                    else if (CheckPlusMinus(texas.Peek()) || CheckMultDiv(texas.Peek()))
                        california.Push(texas.Pop().ToString());
                    else if (texas.Peek() == '(')  //ошибка
                    {
                        throw new Exception("Некорректная формула");
                    }
                }
            }                                                                          
            RPNAr = california.ToArray();
            while (california.Count != 0)
            {
                output.Append(california.Pop() + " ");
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
                    double x2 = double.Parse(calc.Pop());
                    double x1 = double.Parse(calc.Pop());
                    if (RPNAr[i] == "+")
                        calc.Push((x1 + x2).ToString());
                    else if (RPNAr[i] == "-")
                        calc.Push((x1 - x2).ToString());
                    else if (RPNAr[i] == "*")
                        calc.Push((x1 * x2).ToString());
                    else
                        calc.Push((x1 / x2).ToString());
                }
            }
            return double.Parse(calc.Pop());
        }
    }
}
