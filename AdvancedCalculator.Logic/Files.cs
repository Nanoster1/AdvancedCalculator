using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdvancedCalculator.Logic
{
    public class Files
    {
        string[] Text { get; set; }
        List<double> Range { get; set; }
        string Function { get; set; }
        List<Calculator> Calculators { get; set; }
        double Step { get; set; }
        public Files(string[] text)
        {
            string path = Environment.CurrentDirectory + "\\Text.txt";
            File.AppendAllLines(path, text);
            Text = File.ReadAllLines(path);
            Function = "⊥" + Text[1] + "⊥";
            Step = SetStep();
            Range = SetRange();
            Calculators = SetCalcs(Function);
        }
        public Files()
        {
             string path = Environment.CurrentDirectory + "\\Text.txt";
            Text = File.ReadAllLines(path);
            Function = "⊥" + Text[1] + "⊥";
            Step = SetStep();
            Range = SetRange();
            Calculators = SetCalcs(Function);
        }
        double SetStep()
        {
            return double.Parse(Text[2]);
        }
        List<Calculator> SetCalcs(string function)
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
        List<double> SetRange()
        {
            string[] info = Text[0].Split(" ");
            int range = int.Parse(info[2]) - int.Parse(info[0]) + 1;
            var ar = new List<double>();
            for (double i = 0; i < range; i += Step)
            {
                ar.Add(i + double.Parse(info[0]));
            }
            return ar;
        } 
        public void WriteOutput()
        {
            string path = Environment.CurrentDirectory + "\\Output.txt";
            if (Calculators.Count == 0) 
                File.WriteAllText(path, "Некорректная формула");
            else 
            {
                List<string> output = new List<string>() { "ОПЗ:" };
                for (int i = 0; i < Range.Count; i++)
                {
                    output.Add($"x = {Range[i]}            {Calculators[i].RPNStr}");
                }
                output.Add(" ");
                output.Add("Значения функции:");
                for (int i = 0; i < Range.Count; i++)
                {
                    output.Add($"x = {Range[i]}            {Calculators[i].Answer}");
                }
                File.WriteAllLines(path, output); 
            }
        }
    }
}
