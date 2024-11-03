using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> instructions = AoC.GetInputLines();

void P12()
{
    Dictionary<int, Bot> Bots = new();
    Dictionary<int, int> Outputs = new();

    foreach (string instruction in instructions)
    {
        var instructionSplit = instruction.Split(' ');
        if (instructionSplit[0] == "value")
        {
            int microchip = int.Parse(instructionSplit[1]);
            int botNum = int.Parse(instructionSplit[5]);
            Bot? bot;
            if (!Bots.TryGetValue(botNum, out bot))
            {
                bot = new(botNum);
                Bots[botNum] = bot;
            }
            bot.Microchips.Add(microchip);
        }
        else if (instructionSplit[0] == "bot")
        {
            int botNum = int.Parse(instructionSplit[1]);
            Bot? bot;
            if (!Bots.TryGetValue(botNum, out bot))
            {
                bot = new(botNum);
                Bots[botNum] = bot;
            }

            if (instructionSplit[5] == "bot")
                bot.LowBot = int.Parse(instructionSplit[6]);
            else if (instructionSplit[5] == "output")
                bot.LowOutput = int.Parse(instructionSplit[6]);

            if (instructionSplit[10] == "bot")
                bot.HighBot = int.Parse(instructionSplit[11]);
            else if (instructionSplit[10] == "output")
                bot.HighOutput = int.Parse(instructionSplit[11]);
        }
    }

    int p1Result = -1;

    bool changeMade = true;
    while(changeMade)
    {
        changeMade = false;
        foreach (Bot bot in Bots.Values)
        {
            if (bot.Microchips.Count == 2)
            {
                int min = bot.Microchips.Min();
                int max = bot.Microchips.Max();

                if (min == 17 && max == 61)
                {
                    p1Result = bot.Num;
                }

                if (bot.LowBot != -1)
                    Bots[bot.LowBot].Microchips.Add(min);
                else
                    Outputs[bot.LowOutput] = min;
                if (bot.HighBot != -1)
                    Bots[bot.HighBot].Microchips.Add(max);
                else
                    Outputs[bot.HighOutput] = max;
                bot.Microchips.Clear();
                changeMade = true;
            }
        }
    }

    Console.WriteLine(p1Result);
    Console.ReadLine();
    Console.WriteLine(Outputs[0] * Outputs[1] * Outputs[2]);
    Console.ReadLine();
}

P12();

public class Bot
{
    public int Num;
    public List<int> Microchips = new();
    public int LowBot = -1;
    public int LowOutput = -1;
    public int HighBot = -1;
    public int HighOutput = -1;

    public Bot(int botNum)
    {
        Num = botNum;
    }
}