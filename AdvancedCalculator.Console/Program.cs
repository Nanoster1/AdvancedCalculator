namespace AdvancedCalculator.Console
{
    using System;
    using System.IO;
    using System.Text;
    using AdvancedCalculator.Logic;
    class Program
    {
        static void Main()
        {
            Files files = new Files();
            files.WriteOutput();
        }
    }
}