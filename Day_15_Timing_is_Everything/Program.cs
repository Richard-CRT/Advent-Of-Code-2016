using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();

List<(int, int)> discs = new();
foreach (string disc in inputList)
{
    var split = disc.Split(' ');
    int positions = int.Parse(split[3]);
    int offset = int.Parse(split[11][..^1]);
    discs.Add((positions, offset));
}

void P12()
{
    Int64 t;
    for (t = 0; ; t++)
    {
        bool works = true;
        for (int d = 0; d < discs.Count; d++)
        {
            (int positions, int offset) = discs[d];
            Int64 offsetTime = t + d + 1;
            int tPosition = (int)((offset + offsetTime) % positions);
            if (tPosition != 0)
            {
                works = false;
                break;
            }
        }
        if (works)
            break;
    }

    Console.WriteLine(t);
    Console.ReadLine();
}

P12();
discs.Add((11, 0));
P12();
