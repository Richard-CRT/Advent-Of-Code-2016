using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
string row1 = inputList[0];
int rowWidth = row1.Length;

string getRow(string previousRow)
{
    string row = "";
    for (int x = 0; x < rowWidth; x++)
    {
        bool leftTrap = x > 0 ? previousRow[x - 1] == '^' : false;
        bool rightTrap = x < rowWidth - 1 ? previousRow[x + 1] == '^' : false;
        bool centerTrap = previousRow[x] == '^';

        bool trap = (leftTrap && centerTrap && !rightTrap) || (!leftTrap && centerTrap && rightTrap) || (leftTrap && !centerTrap && !rightTrap) || (!leftTrap && !centerTrap && rightTrap);
        row += trap ? '^' : '.';
    }
    return row;
}

void P12()
{
    List<string> rows = new() { row1 };

    int y = 1;
    for (; y < 40; y++)
    {
        string row = getRow(rows[y - 1]);
        rows.Add(row);
    }

    Console.WriteLine(rows.Sum(s => s.Count(c => c == '.')));
    Console.ReadLine();

    for (; y < 400_000; y++)
    {
        string row = getRow(rows[y - 1]);
        rows.Add(row);
    }

    Console.WriteLine(rows.Sum(s => s.Count(c => c == '.')));
    Console.ReadLine();
}

P12();
