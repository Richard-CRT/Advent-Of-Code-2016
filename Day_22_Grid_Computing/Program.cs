using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
List<Node> nodes = inputList.Skip(2).Select(s => new Node(s)).ToList();

void P1()
{
    int viable = 0;
    foreach (Node nodeA in nodes)
    {
        if (nodeA.Used != 0)
        {
            foreach (Node nodeB in nodes)
            {
                if (nodeA != nodeB)
                {
                    if (nodeA.Used <= nodeB.Avail)
                        viable++;
                }
            }
        }
    }

    Console.WriteLine(viable);
    Console.ReadLine();
}

void P2()
{
    int result = 0;
    Console.WriteLine(result);
    Console.ReadLine();
}

P1();
P2();

public class Node
{
    public int X;
    public int Y;
    public int Size;
    public int Used;
    public int Avail;

    public Node (string s)
    {
        // /dev/grid/node-x0-y0     88T   67T    21T   76%
        var split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var nameSplit = split[0].Split('-');
        X = int.Parse(nameSplit[1][1..]);
        Y = int.Parse(nameSplit[2][1..]);
        Size = int.Parse(split[1][..^1]);
        Used = int.Parse(split[2][..^1]);
        Avail = int.Parse(split[3][..^1]);
    }
}