using AdventOfCodeUtilities;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

List<string> inputList = AoC.GetInputLines();
string data = inputList[0];

void P1()
{
    /*
    string output = "";
    for (int i = 0; i < data.Length;)
    {
        char c = data[i];
        if (c == '(')
        {
            int j = i;
            while (data[j] != ')')
                j++;
            string marker = data.Substring(i + 1, j - i - 1);
            var markerSplit = marker.Split('x');
            int numChars = int.Parse(markerSplit[0]);
            int numRepeats = int.Parse(markerSplit[1]);
            string repeatingString = data.Substring(j + 1, numChars);
            for (int n = 0; n < numRepeats; n++)
                output += repeatingString;
            i = j + 1 + numChars;
        }
        else
        {
            output += c;
            i++;
        }
    }

    Console.WriteLine(output.Length);
    Console.ReadLine();
    */

    int outputLen = 0;
    for (int i = 0; i < data.Length;)
    {
        char c = data[i];
        if (c == '(')
        {
            int j = i;
            while (data[j] != ')')
                j++;
            string marker = data.Substring(i + 1, j - i - 1);
            var markerSplit = marker.Split('x');
            int numChars = int.Parse(markerSplit[0]);
            int numRepeats = int.Parse(markerSplit[1]);
            outputLen += numRepeats * numChars;
            i = j + 1 + numChars;
        }
        else
        {
            outputLen++;
            i++;
        }
    }

    Console.WriteLine(outputLen);
    Console.ReadLine();
}

void P2()
{
    RecursiveString topLevelRecursiveString = new(1, data);

    Console.WriteLine(topLevelRecursiveString.GetLength());
    Console.ReadLine();
}

P1();
P2();

public class RecursiveString
{
    public bool Literal;
    public char LiteralChar = ' ';
    public int Repeats;
    public List<RecursiveString> RecursiveStrings = new List<RecursiveString>();


    public RecursiveString(char c)
    {
        Literal = true;
        LiteralChar = c;
    }

    public RecursiveString(int repeats, string s)
    {
        Literal = false;

        Repeats = repeats;
        for (int i = 0; i < s.Length;)
        {
            char c = s[i];
            if (c == '(')
            {
                int j = i;
                while (s[j] != ')')
                    j++;
                string marker = s.Substring(i + 1, j - i - 1);
                var markerSplit = marker.Split('x');
                int numChars = int.Parse(markerSplit[0]);
                int numRepeats = int.Parse(markerSplit[1]);
                string repeatingString = s.Substring(j + 1, numChars);
                RecursiveStrings.Add(new(numRepeats, repeatingString));
                i = j + 1 + numChars;
            }
            else
            {
                RecursiveStrings.Add(new(c));
                i++;
            }
        }
    }

    public long GetLength()
    {
        if (Literal)
            return 1;
        else
            return Repeats * RecursiveStrings.Sum(l => l.GetLength());
    }

    public override string ToString()
    {
        if (Literal)
            return LiteralChar.ToString();
        else
            return $"{{{Repeats}*{ string.Join("", RecursiveStrings)}}}";
    }
}
