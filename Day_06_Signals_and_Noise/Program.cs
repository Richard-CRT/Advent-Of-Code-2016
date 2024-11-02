using AdventOfCodeUtilities;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();

Dictionary<char, int>[] map = new Dictionary<char, int>[inputList[0].Length];
for (int i = 0; i < inputList[0].Length; i++)
{
    map[i] = new();
    for (char c = 'a'; c <= 'z'; c++)
        map[i][c] = 0;
}
foreach (string l in inputList)
{
    for (int i = 0; i < inputList[0].Length; i++)
    {
        char c = l[i];
        map[i][c]++;
    }
}

void P1()
{
    string msg = "";
    for (int i = 0; i < inputList[0].Length; i++)
    {
        msg += map[i].OrderByDescending(kvp => kvp.Value).Select(kvp => kvp.Key).First();
    }

    Console.WriteLine(msg);
    Console.ReadLine();
}

void P2()
{
    string msg2 = "";
    for (int i = 0; i < inputList[0].Length; i++)
    {
        msg2 += map[i].OrderBy(kvp => kvp.Value).Where(kvp => kvp.Value > 0).Select(kvp => kvp.Key).First();
    }

    Console.WriteLine(msg2);
    Console.ReadLine();
}

P1();
P2();
