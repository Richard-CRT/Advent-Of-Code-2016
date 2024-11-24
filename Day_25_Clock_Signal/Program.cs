using AdventOfCodeUtilities;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
List<Instruction> instructions = inputList.Select(s => new Instruction(s)).ToList();

bool Process(Dictionary<char, Int64> registerFile)
{
    int i = 0;
    Int64 previousValue = 1;
    bool success = true;
    for (Int64 pc = 0; pc < instructions.Count && i < 100_000; pc++, i++)
    {
        Instruction instruction = instructions[(int)pc];
        switch (instruction.Opcode)
        {
            case "cpy":
                switch (instruction.Operands[0][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        registerFile[instruction.Operands[1][0]] = registerFile[instruction.Operands[0][0]];
                        break;
                    default:
                        registerFile[instruction.Operands[1][0]] = int.Parse(instruction.Operands[0]);
                        break;
                }
                break;
            case "inc":
                registerFile[instruction.Operands[0][0]]++;
                break;
            case "dec":
                registerFile[instruction.Operands[0][0]]--;
                break;
            case "jnz":
                Int64 valToCheck0;
                switch (instruction.Operands[0][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        valToCheck0 = registerFile[instruction.Operands[0][0]];
                        break;
                    default:
                        valToCheck0 = int.Parse(instruction.Operands[0]);
                        break;
                }

                if (valToCheck0 != 0)
                {
                    Int64 distanceToJump = 0;
                    switch (instruction.Operands[1][0])
                    {
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                            distanceToJump += registerFile[instruction.Operands[1][0]];
                            break;
                        default:
                            distanceToJump = int.Parse(instruction.Operands[1]);
                            break;
                    }

                    pc += distanceToJump - 1;
                }
                break;
            case "out":
                switch (instruction.Operands[0][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        Int64 outValue = registerFile[instruction.Operands[0][0]];
                        if (outValue == previousValue)
                        {
                            success = false;
                            break;
                        }
                        previousValue = outValue;
                        //Console.WriteLine(outValue);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                break;
        }
    }
    return success;
}

void P1()
{
    Dictionary<char, Int64> registerFile = new();

    int i = 0;
    bool success = false;
    while (!success)
    {
        registerFile['a'] = i;
        registerFile['b'] = 0;
        registerFile['c'] = 0;
        registerFile['d'] = 0;

        success = Process(registerFile);

        i++;
    }
    i--;
    Console.WriteLine(i);
    Console.ReadLine();
}

P1();

public class Instruction
{
    public string Opcode = "";
    public string[] Operands;

    public Instruction(string s)
    {
        var split = s.Split(' ');
        Opcode = split[0];
        Operands = split.Skip(1).ToArray();
    }

    public override string ToString()
    {
        return $"{Opcode} {string.Join(' ', Operands)}";
    }
}
