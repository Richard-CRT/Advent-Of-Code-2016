using AdventOfCodeUtilities;

List<string> inputList = AoC.GetInputLines();
List<string> Instructions = inputList[0].Split(", ").ToList();

HashSet<(int, int)> history = new HashSet<(int, int)>();
int direction = 0;
int x = 0;
int y = 0;
int p2x = 0;
int p2y = 0;
bool foundFirstRepeat = false;
foreach (string s in Instructions)
{
    if (s[0] == 'R')
        direction = (direction + 1) % 4;
    else if (s[0] == 'L')
    {
        if (direction == 0)
            direction = 3;
        else
            direction = direction - 1;
    }

    int distance = int.Parse(s.Substring(1));
    switch (direction)
    {
        case 0:
            for (int i = 0; i < distance; i++)
            {
                y++;

                if (!foundFirstRepeat && history.Contains((x, y)))
                {
                    p2x = x;
                    p2y = y;
                    foundFirstRepeat = true;
                }
                history.Add((x, y));
            }
            break;
        case 1:
            for (int i = 0; i < distance; i++)
            {
                x++;

                if (!foundFirstRepeat && history.Contains((x, y)))
                {
                    p2x = x;
                    p2y = y;
                    foundFirstRepeat = true;
                }
                history.Add((x, y));
            }
            break;
        case 2:
            for (int i = 0; i < distance; i++)
            {
                y--;

                if (!foundFirstRepeat && history.Contains((x, y)))
                {
                    p2x = x;
                    p2y = y;
                    foundFirstRepeat = true;
                }
                history.Add((x, y));
            }
            break;
        case 3:
            for (int i = 0; i < distance; i++)
            {
                x--;

                if (!foundFirstRepeat && history.Contains((x, y)))
                {
                    p2x = x;
                    p2y = y;
                    foundFirstRepeat = true;
                }
                history.Add((x, y));
            }
            break;
        default: throw new NotImplementedException();
    }
}

void P1()
{
    Console.WriteLine(Math.Abs(x + y));
    Console.ReadLine();
}

void P2()
{
    Console.WriteLine(Math.Abs(p2x + p2y));
    Console.ReadLine();
}

P1();
P2();