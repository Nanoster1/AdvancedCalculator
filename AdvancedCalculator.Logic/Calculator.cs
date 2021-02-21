using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedCalculator.Logic
{
    public class Calculator
    {
        object[] RPNAr { get; set; }
        public string RPNStr { get; private set; }
        public double Answer { get; private set; }
        public object[] GetRPN(string text)
        {
            List<object> expression = ParseExpression(text);
            return ParseInRPN(expression);
        }
        List<object> ParseExpression(string text)                                                              //Парсим выражение в лист объектов
        {
            List<object> expression = new List<object>();
            for (int i = 0; i < text.Length; i++)
            {
                if (CheckDigit(text[i].ToString()))                                                            //Если число
                {
                    StringBuilder num = new StringBuilder().Append(text[i]);
                    while (CheckDigit(text[i + 1].ToString()) || text[i + 1] == ',')
                    {
                        i++;
                        num.Append(text[i]);
                    }
                    expression.Add(double.Parse(num.ToString()));
                }
                else if (CheckLeftBracket(text[i]) || CheckRightBracket(text[i]) || CheckStartEnd(text[i]))
                    expression.Add(text[i].ToString());
                else if ("lsct+".Contains(text[i]))
                {
                    StringBuilder oper = new StringBuilder().Append(text[i]);
                    while(!(CheckDigit(text[i + 1].ToString())) && !"-/*^()".Contains(text[i + 1]))
                    {
                        i++;
                        oper.Append(text[i]);
                    }
                    Operation op = ChooseOp(oper);
                    expression.Add(op);
                }
                else if ("-/*^".Contains(text[i]))
                {
                    Operation op = ChooseOp(text[i]);
                    expression.Add(op);
                }
                else
                    continue;
            }
            return expression;
        }
        object[] ParseInRPN(List<object> expression)
        {
            Stack<object> california = new Stack<object>();
            Stack<object> texas = new Stack<object>();
            int i = 0;
            while (true)
            {
                if (i == 0) 
                {
                    texas.Push(expression[i]);
                    i++;
                }
                else if (CheckStartEnd(expression[i]))                      //Убрать в конец
                {
                    if (texas.Peek() is Operation)
                        california.Push(texas.Pop());
                    else if (CheckLeftBracket(texas.Peek()))                                                                           //ошибка
                        throw new Exception("Левая скобка в конце выражения");
                    else
                        break;
                }
                else if (expression[i] is double)
                {
                    california.Push(expression[i]);
                    i++;
                }
                else if (CheckLeftBracket(expression[i]))             //хз работает ли
                {
                    texas.Push(expression[i]);
                    i++;
                }
                else if (CheckRightBracket(expression[i]))
                { 
                    if (texas.Peek() is Operation)
                        california.Push(texas.Pop());
                    else if (CheckLeftBracket(texas.Peek()))                                                       
                    {
                        texas.Pop();
                        i++;
                    }
                    else
                        throw new Exception("Некорректная формула");
                }
                else if (expression[i] is Operation)
                {
                    switch((expression[i] as Operation).Prior)
                    {
                        case (1):
                            texas.Push(expression[i]);
                            i++;
                            break;
                        case (2):
                            if ((texas.Peek() is Operation) == false || (texas.Peek() as Operation).Prior == 3)
                            {
                                texas.Push(expression[i]);
                                i++;
                            }
                            else if ((texas.Peek() as Operation).Prior < 3)
                                california.Push(texas.Pop());
                            break;
                        case (3):
                            if (texas.Peek() is Operation)
                                california.Push(texas.Pop());
                            else
                            {
                                texas.Push(expression[i]);
                                i++;
                            }
                            break;
                    }
                }
            }
            return california.ToArray();
        }
        string GetRPNStr()
        {
            StringBuilder @string = new StringBuilder();
            for (int i = RPNAr.Length - 1; i > 0; i--)
            {
                if (RPNAr[i] is Operation)
                    @string.Append((RPNAr[i] as Operation).Name + " ");
                else
                    @string.Append(RPNAr[i].ToString() + " ");
            }
            @string.Append((RPNAr[0] as Operation).Name);
            return @string.ToString();
        }
        public Calculator(string text)
        {
            RPNAr = GetRPN(text);
            RPNStr = GetRPNStr();
            Answer = GetAnswer();
        }
        public Operation ChooseOp(object sym)
        {
            Operation op = null;
            switch (sym.ToString())
            {
                case ("+"):
                    op = new Plus();
                    break;
                case ("-"):
                    op = new Minus();
                    break;
                case ("*"):
                    op = new Mult();
                    break;
                case ("/"):
                    op = new Div();
                    break;
                case ("log"):
                    op = new Log();
                    break;
                case ("sin"):
                    op = new Sin();
                    break;
                case ("cos"):
                    op = new Cos();
                    break;
                case ("tg"):
                    op = new Tan();
                    break;
                case ("^"):
                    op = new Rank();
                    break;
                case ("sqrt"):
                    op = new Sqrt();
                    break;
            }
            return op;
        }
        
        bool CheckDigit(string text)                       //Проверяем на число
        {
            if (double.TryParse(text, out _))
                return true;
            return false;
        }
        bool CheckStartEnd(object text)                                //Проверяем на символ начала/конца выражения
        {
            if (text.ToString() == "⊥")
                return true;
            return false;
        }
        bool CheckLeftBracket(object text)
        {
            if (text.ToString() == "(")
                return true;
            return false;
        }
        bool CheckRightBracket(object text)
        {
            if (text.ToString() == ")")
                return true;
            return false;
        }
        double GetAnswer()
        {
            Stack<double> calc = new Stack<double>();
            for (int i = RPNAr.Length - 1; i >= 0; i--)
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
