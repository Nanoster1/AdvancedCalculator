using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedCalculator.Logic
{
    public abstract class Operation
    {
        public abstract string Name { get; }
        public abstract int CountParams { get; }
        public abstract double Calculate(double[] @params);
        public abstract int Prior { get; }
    }
    public class Plus : Operation
    {
        public override string Name => "+";
        public override int CountParams => 2;
        public override double Calculate(double[] @params) { return @params[1] + @params[0]; }
        public override int Prior => 3;
    }
    public class Minus : Operation
    {
        public override string Name => "-";
        public override int CountParams => 2;
        public override double Calculate(double[] @params) { return @params[1] - @params[0]; }
        public override int Prior => 3;
    }
    class Mult : Operation
    {
        public override string Name => "*";
        public override int CountParams => 2;
        public override double Calculate(double[] @params) { return @params[1] * @params[0]; }
        public override int Prior => 2;
    }
    class Div : Operation
    {
        public override string Name => "/";
        public override int CountParams => 2;
        public override double Calculate(double[] @params) { return @params[1] / @params[0]; }
        public override int Prior => 2;
    }
    class Log : Operation
    {
        public override string Name => "log";
        public override int CountParams => 2;
        public override double Calculate(double[] @params) { return Math.Log(@params[0], @params[1]); }                    //params 1 = основание
        public override int Prior => 1;
    }
    class Sin : Operation
    {
        public override string Name => "sin";
        public override int CountParams => 1;
        public override double Calculate(double[] @params) { return Math.Sin(@params[0]); }
        public override int Prior => 1;
    }
    class Cos : Operation
    {
        public override string Name => "cos";
        public override int CountParams => 1;
        public override double Calculate(double[] @params) { return Math.Cos(@params[0]); }
        public override int Prior => 1;
    }
    class Tan : Operation
    {
        public override string Name => "tg";
        public override int CountParams => 1;
        public override double Calculate(double[] @params) { return Math.Tan(@params[0]); }
        public override int Prior => 1;
    }
}
