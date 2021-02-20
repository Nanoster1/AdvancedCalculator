using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdvancedCalculator.Logic
{
    public class Files
    {
        public static FileInfo Output { get; private set; } = new FileInfo(Environment.CurrentDirectory + "\\Output.txt");
        public static FileInfo Text { get; private set; } = new FileInfo(Environment.CurrentDirectory + "\\Text.txt");
        public static List<int> Range { get; private set; } = SetRange();
        public static string Function { get; private set; } = "⊥" + File.ReadAllLines(Text.FullName)[1] + "⊥";
        static List<Calculator> Calculators { get; set; } = SetCalcs(Function);
        static List<Calculator> SetCalcs(string function)
        {
            try
            {
                List<Calculator> calculators = new List<Calculator>();
                for (int i = 0; i < Range.Count; i++)
                {
                    string a = Range[i].ToString();
                    Calculator calculator = new Calculator(function.Replace("x", a));
                    calculators.Add(calculator);
                }
                return calculators;
            }
            catch
            {
                return new List<Calculator>();
            }
        }
        static List<int> SetRange()
        {
            string[] info = File.ReadAllLines(Text.FullName)[0].Split(" ");
            int range = int.Parse(info[2]) - int.Parse(info[0]) + 1;
            List<int> ar = new List<int>();
            for (int i = 0; i < range; i++)
            {
                ar.Add(i + int.Parse(info[0]));
            }
            return ar;
        } 
        public static void WriteOutput()
        {
            if (Calculators.Count == 0) 
                File.WriteAllText(Output.FullName, "Некорректная формула");
            else 
            {
                List<string> output = new List<string>();
                output.Add("ОПЗ:");
                for (int i = 0; i < Range.Count; i++)
                {
                    output.Add($"x = {Range[i].ToString()}            {Calculators[i].RPNStr}");
                }
                output.Add(" ");
                output.Add("Значения функции:");
                for (int i = 0; i < Range.Count; i++)
                {
                    output.Add($"x = {Range[i].ToString()}            {Calculators[i].Answer}");
                }
                File.WriteAllLines(Output.FullName, output); 
            }
        }
    }
}
