using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
//const int width = 7;
//const int height = 3;
const int width = 50;
const int height = 6;
bool[][] map = new bool[height][];
for (int y = 0; y < height; y++)
    map[y] = new bool[width];

void P12()
{
    foreach (string s in inputList)
    {
        var instructionSplit = s.Split(' ');
        if (instructionSplit[0] == "rect")
        {
            var coordSplit = instructionSplit[1].Split('x');
            for (int y = 0; y < int.Parse(coordSplit[1]); y++)
            {
                for (int x = 0; x < int.Parse(coordSplit[0]); x++)
                {
                    map[y][x] = true;
                }
            }
        }
        else if (instructionSplit[0] == "rotate")
        {
            var id = instructionSplit[2].Split('=');
            var rowColumnIndex = int.Parse(id[1]);
            var magnitude = int.Parse(instructionSplit[4]);

            if (instructionSplit[1] == "column")
            {
                int columnIndex = rowColumnIndex;
                int shift = magnitude % height;

                bool[] buffer = new bool[shift];
                int j = 0;
                for (int i = height - shift; i < height; i++, j++)
                    buffer[j] = map[i][columnIndex];
                for (int i = height - shift - 1; i >= 0; i--)
                    map[i + shift][columnIndex] = map[i][columnIndex];
                for (int i = 0; i < shift; i++)
                    map[i][columnIndex] = buffer[i];
            }
            else if (instructionSplit[1] == "row")
            {
                int rowIndex = rowColumnIndex;
                int shift = magnitude % width;

                bool[] buffer = new bool[shift];
                int j = 0;
                for (int i = width - shift; i < width; i++, j++)
                    buffer[j] = map[rowIndex][i];
                for (int i = width - shift - 1; i >= 0; i--)
                    map[rowIndex][i + shift] = map[rowIndex][i];
                for (int i = 0; i < shift; i++)
                    map[rowIndex][i] = buffer[i];
            }
        }
    }

    int result = 0;

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (map[y][x])
                result++;
        }
    }

    Console.WriteLine(result);
    Console.ReadLine();

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (map[y][x])
                Console.Write("█");
            else
                Console.Write("░");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.ReadLine();
}

P12();
