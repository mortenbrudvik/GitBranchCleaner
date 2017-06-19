using System;

namespace GitBranchCleaner
{
    public class LogService
    {
        public void Info(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.Out.WriteLine(message);
        }
    }
}