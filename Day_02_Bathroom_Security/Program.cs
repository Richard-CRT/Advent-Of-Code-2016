using AdventOfCodeUtilities;

List<string> inputList = AoC.GetInputLines();

int[,] KeypadMapP1 = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
char[,] KeypadMapP2 = new char[5, 5] { { ' ', ' ', '1', ' ', ' ' }, { ' ', '2', '3', '4', ' ' }, { '5', '6', '7', '8', '9' }, { ' ', 'A', 'B', 'C', ' ' }, { ' ', ' ', 'D', ' ', ' ' }, };

void P1()
{
    string code = "";
    int x = 1;
    int y = 1;
    foreach (string s in inputList)
    {
        foreach (char c in s)
        {
            switch (c)
            {
                case 'U': y--; break;
                case 'D': y++; break;
                case 'L': x--; break;
                case 'R': x++; break;
            }
            if (y < 0) y = 0;
            else if (y > 2) y = 2;
            if (x < 0) x = 0;
            else if (x > 2) x = 2;
        }
        code += KeypadMapP1[y, x];
    }

    Console.WriteLine(code);
    Console.ReadLine();
}

void P2()
{
    string code = "";
    int x = 0;
    int y = 2;
    foreach (string s in inputList)
    {
        foreach (char c in s)
        {
            switch (c)
            {
                case 'U':
                    if (y > 0 && KeypadMapP2[y - 1, x] != ' ')
                        y--;
                    break;
                case 'D':
                    if (y < 4 && KeypadMapP2[y + 1, x] != ' ')
                        y++;
                    break;
                case 'L':
                    if (x > 0 && KeypadMapP2[y, x - 1] != ' ')
                        x--;
                    break;
                case 'R':
                    if (x < 4 && KeypadMapP2[y, x + 1] != ' ')
                        x++;
                    break;
            }
        }
        code += KeypadMapP2[y, x];
    }

    Console.WriteLine(code);
    Console.ReadLine();
}

P1();
P2();