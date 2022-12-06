//#define OVERRIDE

namespace AdventOfCodeUtilities
{
    public class AoCUtilities
    {
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
    }
}