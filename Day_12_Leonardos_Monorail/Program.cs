using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();

void Process(Dictionary<char, int> registerFile)
{
    for (int pc = 0; pc < inputList.Count; pc++)
    {
        string instruction = inputList[pc];
        var split = instruction.Split(' ');
        string opcode = split[0];
        switch (opcode)
        {
            case "cpy":
                switch (split[1][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        registerFile[split[2][0]] = registerFile[split[1][0]];
                        break;
                    default:
                        registerFile[split[2][0]] = int.Parse(split[1]);
                        break;
                }
                break;
            case "inc":
                registerFile[split[1][0]]++;
                break;
            case "dec":
                registerFile[split[1][0]]--;
                break;
            case "jnz":
                switch (split[1][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        if (registerFile[split[1][0]] != 0)
                            pc += int.Parse(split[2]) - 1;
                        break;
                    default:
                        if (int.Parse(split[1]) != 0)
                            pc += int.Parse(split[2]) - 1;
                        break;
                }
                break;
        }
    }
}

void P1()
{
    Dictionary<char, int> registerFile = new();
    registerFile['a'] = 0;
    registerFile['b'] = 0;
    registerFile['c'] = 0;
    registerFile['d'] = 0;

    Process(registerFile);

    Console.WriteLine(registerFile['a']);
    Console.ReadLine();
}

void P2()
{
    Dictionary<char, int> registerFile = new();
    registerFile['a'] = 0;
    registerFile['b'] = 0;
    registerFile['c'] = 1;
    registerFile['d'] = 0;

    Process(registerFile);

    Console.WriteLine(registerFile['a']);
    Console.ReadLine();
}

P1();
P2();
