using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
List<Instruction> instructions = inputList.Select(s => new Instruction(s)).ToList();

void Process(Dictionary<char, Int64> registerFile)
{
    for (Int64 pc = 0; pc < instructions.Count; pc++)
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
            case "tgl":
                switch (instruction.Operands[0][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        Int64 targetPC = pc + registerFile[instruction.Operands[0][0]];
                        if (targetPC >= 0 && targetPC < instructions.Count)
                        {
                            Instruction targetInstruction = instructions[(int)targetPC];
                            switch (targetInstruction.Opcode)
                            {
                                case "inc":
                                    targetInstruction.Opcode = "dec";
                                    break;
                                case "dec":
                                case "tgl":
                                    targetInstruction.Opcode = "inc";
                                    break;
                                case "jnz":
                                    targetInstruction.Opcode = "cpy";
                                    break;
                                case "cpy":
                                    targetInstruction.Opcode = "jnz";
                                    break;
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                break;
            case "mul":
                switch (instruction.Operands[1][0])
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                        switch (instruction.Operands[0][0])
                        {
                            case 'a':
                            case 'b':
                            case 'c':
                            case 'd':
                                registerFile[instruction.Operands[1][0]] = registerFile[instruction.Operands[1][0]] * registerFile[instruction.Operands[0][0]];
                                break;
                            default:
                                registerFile[instruction.Operands[1][0]] = registerFile[instruction.Operands[1][0]] * int.Parse(instruction.Operands[0]);
                                break;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                break;
        }
        //Console.WriteLine($"{pc} {instruction}");
        //Console.WriteLine(string.Join(' ', registerFile.Select(kvp => $"{kvp.Key}:{kvp.Value}")));
        //Console.ReadLine();
    }
}

void P1()
{
    Dictionary<char, Int64> registerFile = new();
    registerFile['a'] = 7;
    registerFile['b'] = 0;
    registerFile['c'] = 0;
    registerFile['d'] = 0;

    Process(registerFile);

    Console.WriteLine(registerFile['a']);
    Console.ReadLine();
}

void P2()
{
    instructions = inputList.Select(s => new Instruction(s)).ToList();
    // Requires optimisation
    instructions[17].Operands[0] = "-6";
    instructions.RemoveRange(2, 8);
    instructions.Insert(2, new Instruction("mul b a"));
    instructions.RemoveRange(5, 4);
    instructions.Insert(5, new Instruction("mul 2 c"));

    Dictionary<char, Int64> registerFile = new();
    registerFile['a'] = 12;
    registerFile['b'] = 0;
    registerFile['c'] = 0;
    registerFile['d'] = 0;

    Process(registerFile);

    Console.WriteLine(registerFile['a']);
    Console.ReadLine();
}

P1();
P2();

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
