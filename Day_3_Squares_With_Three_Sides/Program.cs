using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoCUtilities.GetInputLines();
(int, int, int)[] Rows = inputList.Select(x => { Match m = AoCUtilities.RegexMatch(x, @" *(\d+) +(\d+) +(\d+)")[0]; return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value)); }).ToArray();

void P1()
{
    int possibleCount = 0;
    foreach ((int s1, int s2, int s3) in Rows)
    {
        if (s1 + s2 > s3 && s1 + s3 > s2 && s2 + s3 > s1)
        {
            possibleCount++;
        }
    }
    Console.WriteLine(possibleCount);
    Console.ReadLine();
}

void P2()
{
    int possibleCount = 0;
    for (int i = 0; i < Rows.Length; i += 3)
    {
        List<(int, int, int)> triangles = new List<(int, int, int)>();
        triangles.Add((Rows[i].Item1, Rows[i + 1].Item1, Rows[i + 2].Item1));
        triangles.Add((Rows[i].Item2, Rows[i + 1].Item2, Rows[i + 2].Item2));
        triangles.Add((Rows[i].Item3, Rows[i + 1].Item3, Rows[i + 2].Item3));
        foreach ((int s1, int s2, int s3) in triangles)
        {
            if (s1 + s2 > s3 && s1 + s3 > s2 && s2 + s3 > s1)
            {
                possibleCount++;
            }
        }
    }
    Console.WriteLine(possibleCount);
    Console.ReadLine();
}

P1();
P2();