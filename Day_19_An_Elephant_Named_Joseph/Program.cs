using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();

int numElves = int.Parse(inputList[0]);

Elf firstElf = new(1);
Elf firstElfP2 = new(1);
Elf iterateElf = firstElf;
Elf iterateElfP2 = firstElfP2;
for (int i = 1; i < numElves; i++)
{
    Elf nextElf = new(i + 1);
    iterateElf.NextElf = nextElf;
    nextElf.PreviousElf = iterateElf;
    iterateElf = nextElf;

    Elf nextElfP2 = new(i + 1);
    iterateElfP2.NextElf = nextElfP2;
    nextElfP2.PreviousElf = iterateElfP2;
    iterateElfP2 = nextElfP2;
}
iterateElf.NextElf = firstElf;
firstElf.PreviousElf = iterateElf;
iterateElfP2.NextElf = firstElfP2;
firstElfP2.PreviousElf = iterateElfP2;


void P1()
{
    Elf currentElf = firstElf;
    while (currentElf.NextElf != currentElf)
    {
        currentElf.Presents += currentElf.NextElf!.Presents;
        currentElf.NextElf = currentElf.NextElf!.NextElf!;
        currentElf = currentElf.NextElf!;
    }

    Console.WriteLine(currentElf.Number);
    Console.ReadLine();
}

void P2()
{
    int elfCount = numElves;
    Elf currentElf = firstElfP2;

    Elf oppositeElf = currentElf;
    for (int i = 0; i < elfCount / 2; i++) // Integer divison purposeful
        oppositeElf = oppositeElf.NextElf!;

    while (elfCount > 1)
    {
        Elf victimElf = oppositeElf;
        oppositeElf = oppositeElf.NextElf!;
        if (elfCount % 2 == 1)
            oppositeElf = oppositeElf.NextElf!;

        victimElf.PreviousElf!.NextElf = victimElf.NextElf;
        victimElf.NextElf!.PreviousElf = victimElf.PreviousElf;
        elfCount--;

        currentElf.Presents += victimElf.Presents;
        currentElf = currentElf.NextElf!;
    }

    Console.WriteLine(currentElf.Number);
    Console.ReadLine();
}

P1();
P2();

class Elf
{
    public int Number;
    public int Presents = 1;
    public Elf? PreviousElf = null;
    public Elf? NextElf = null;

    public Elf(int number)
    {
        Number = number;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}
