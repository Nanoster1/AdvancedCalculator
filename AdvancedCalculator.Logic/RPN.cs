using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvancedCalculator.Logic
{
    public static class RPN
    {
        public static List<object> RPNAr { get; private set; }
        public static List<int> Indexes { get; private set; } = new List<int>();
        public static void GetRPN(string text)
        {
            List<object> expression = ParseExpression(text);
            RPNAr = ParseInRPN(expression).ToList();
            GetIndexes();
        }
        static void GetIndexes()
        {
            while (RPNAr.Contains("x"))
            {
                int i = RPNAr.IndexOf("x");
                Indexes.Add(i);
                RPNAr[i] = null;
            }
        }
        static List<object> ParseExpression(string text)                                                              //Парсим выражение в лист объектов
        {
            List<object> expression = new List<object>();
            for (int i = 0; i < text.Length; i++)
            {
                if (CheckDigit(text[i].ToString()) || ((text[i] == '-' || text[i] == '+') && !CheckDigit(text[i - 1].ToString()) && text[i - 1] != ')'))                                                            //Если число
                    expression.Add(ReadNumber(text, ref i));
                else if (CheckLeftBracket(text[i]) || CheckRightBracket(text[i]) || CheckStartEnd(text[i]))
                    expression.Add(text[i].ToString());
                else if (char.IsWhiteSpace(text[i]))
                    i++;
                else
                {
                    expression.Add(ReadOperation(text, ref i));
                }
            }
            return expression;
        }
        static object ReadNumber(string text, ref int i)
        {
            if (text[i] == 'x')
                return "x";
            else
            {
                StringBuilder num = new StringBuilder().Append(text[i]);
                while (CheckDigit(text[i + 1].ToString()) || text[i + 1] == ',')
                {
                    i++;
                    num.Append(text[i]);
                }
                return double.Parse(num.ToString());
            }
        }
        static Operation ReadOperation(string text, ref int i)
        {
            StringBuilder @string = new StringBuilder().Append(text[i]);
            while (!CheckDigit(text[i + 1].ToString()) && text[i + 1] != '(' && text[i + 1] != ')')
            {
                if ("+-*/".Contains(text[i]))
                    break;
                i++;
                @string.Append(text[i]);
            }
            return ChooseOp(@string.ToString());
        }
        static bool CheckDigit(string text)                       //Проверяем на число
        {
            if (double.TryParse(text.ToString(), out _) || text == "x")
                return true;
            return false;
        }
        static bool CheckStartEnd(object text)                                //Проверяем на символ начала/конца выражения
        {
            if (text.ToString() == "⊥")
                return true;
            return false;
        }
        static bool CheckLeftBracket(object text)
        {
            if (text.ToString() == "(")
                return true;
            return false;
        }
        static bool CheckRightBracket(object text)
        {
            if (text.ToString() == ")")
                return true;
            return false;
        }
        static public Operation ChooseOp(object sym)
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
        static object[] ParseInRPN(List<object> expression)
        {
            Stack<object> california = new Stack<object>();
            Stack<object> texas = new Stack<object>();
            int i = 0;
            while (true)
            {
                if (i == 0)                                                     //Символ начала строки идёт сразу во 2-й стек 
                {
                    texas.Push(expression[i]);
                    i++;
                }
                else if (CheckDigit(expression[i].ToString()))
                {
                    california.Push(expression[i]);
                    i++;
                }
                else if (CheckLeftBracket(expression[i]))
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
                    switch ((expression[i] as Operation).Prior)
                    {
                        case (1):
                            texas.Push(expression[i]);
                            i++;
                            break;
                        case (2):
                            if (!(texas.Peek() is Operation) || (texas.Peek() as Operation).Prior == 3)
                            {
                                texas.Push(expression[i]);
                                i++;
                            }
                            else
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
                else if (CheckStartEnd(expression[i]))
                {
                    if (texas.Peek() is Operation)
                        california.Push(texas.Pop());
                    else if (CheckLeftBracket(texas.Peek()))                                                                           //ошибка
                        throw new Exception("Левая скобка в конце выражения");
                    else
                        break;
                }
            }
            return california.ToArray();
        }
    }
}
