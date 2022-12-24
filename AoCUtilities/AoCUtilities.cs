//#define OVERRIDE
using System.Text.RegularExpressions;

namespace AdventOfCodeUtilities
{
    public class AoC
    {
        public static int GCF(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static int LCM(int a, int b)
        {
            return (a / GCF(a, b)) * b;
        }

        static public void DebugClear()
        {
#if DEBUG || OVERRIDE
            Console.Clear();
#endif
        }

        static public string? DebugReadLine()
        {
#if DEBUG || OVERRIDE
            return Console.ReadLine();
#else
            return "";
#endif
        }

        static public void DebugWriteLine()
        {
#if DEBUG || OVERRIDE
            Console.WriteLine();
#endif
        }

        static public void DebugWriteLine(string text, params object[] args)
        {
#if DEBUG || OVERRIDE
            string lineToWrite = string.Format(text, args);
            Console.WriteLine(lineToWrite);
#endif
        }

        static public void DebugWrite(string text, params object[] args)
        {
#if DEBUG || OVERRIDE
            string lineToWrite = string.Format(text, args);
            Console.Write(lineToWrite);
#endif
        }

        static public List<string> GetInputLines(string filename = "input.txt")
        {
            var inputFile = File.ReadAllLines(filename);
            return inputFile.ToList();
        }

        static public string GetInput(string filename = "input.txt")
        {
            var inputFile = File.ReadAllText(filename);
            return inputFile;
        }

        static public MatchCollection RegexMatch(string input, string pattern, bool multiline = false)
        {
            RegexOptions options;
            if (multiline)
                options = RegexOptions.Multiline;
            else
                options = RegexOptions.Singleline;

            return Regex.Matches(input, pattern, options);
        }
    }
}