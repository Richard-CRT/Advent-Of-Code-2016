using AdventOfCodeUtilities;
using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
    Node.Height = nodes.MaxBy(n => n.Y)!.Y + 1;
    Node.Width = nodes.MaxBy(n => n.X)!.X + 1;

    var nodesBySize = nodes.Select(n => n.Size).Order().ToArray();
    int medianSize = nodesBySize[nodesBySize.Length / 2];

    foreach (Node node in nodes)
    {
        if (node.Used > medianSize)
        {
            node.Big = true;
        }
    }

    var emptyNodes = nodes.Where(n => n.Used == 0);
    Debug.Assert(emptyNodes.Count() == 1);
    Node emptyNode = emptyNodes.First();
    Node dataNode = Node.Map[(Node.Width - 1, 0)];

    PathFindingNode initialPathFindingNode = PathFindingNode.GetOrCreatePathFindingNode(emptyNode.X, emptyNode.Y, dataNode.X, dataNode.Y);
    //initialPathFindingNode.Print();
    initialPathFindingNode.Cost = 0;
    PriorityQueue<PathFindingNode, Int64> remainingNodes = new();
    remainingNodes.Enqueue(initialPathFindingNode, 0);

    PathFindingNode? destPathFindingNode = null;
    while (remainingNodes.Count > 0)
    {
        PathFindingNode currentPathFindingNode = remainingNodes.Dequeue();
        if (!currentPathFindingNode.Visited)
        {
            if (currentPathFindingNode.DataNodeX == 0 && currentPathFindingNode.DataNodeY == 0)
            {
                destPathFindingNode = currentPathFindingNode;
                break;
            }

            currentPathFindingNode.ProcessNeighbours();
            foreach (PathFindingNode neighbourPathFindingNode in currentPathFindingNode.Neighbours.Where(pathFindingNode => !pathFindingNode.Visited))
            {
                Int64 newCost = currentPathFindingNode.Cost + 1;
                if (newCost < neighbourPathFindingNode.Cost)
                {
                    neighbourPathFindingNode.Cost = newCost;
                    //neighbourPathFindingNode.Path = new(currentPathFindingNode.Path);
                    //neighbourPathFindingNode.Path.Add(currentPathFindingNode);
                    remainingNodes.Enqueue(neighbourPathFindingNode, newCost);
                }
            }

            currentPathFindingNode.Visited = true;
        }
    }
    
    //destPathFindingNode!.Print();
    Console.WriteLine(destPathFindingNode!.Cost);
    Console.ReadLine();
}

P1();
P2();

public class Node
{
    public static int Height;
    public static int Width;
    public static Dictionary<(int, int), Node> Map = new();

    public int X;
    public int Y;
    public int Size;
    public int Used;
    public int Avail;

    public bool Big = false;

    public Node(string s)
    {
        // /dev/grid/node-x0-y0     88T   67T    21T   76%
        var split = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var nameSplit = split[0].Split('-');
        X = int.Parse(nameSplit[1][1..]);
        Y = int.Parse(nameSplit[2][1..]);
        Size = int.Parse(split[1][..^1]);
        Used = int.Parse(split[2][..^1]);
        Avail = int.Parse(split[3][..^1]);

        Map[(X, Y)] = this;
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

public class PathFindingNode
{
    public static Dictionary<string, PathFindingNode> PathFindingNodeLibrary = new();
    public static PathFindingNode GetOrCreatePathFindingNode(int emptyNodeX, int emptyNodeY, int dataNodeX, int dataNodeY)
    {
        PathFindingNode potentialPathFindingNode = new(emptyNodeX, emptyNodeY, dataNodeX, dataNodeY);

        PathFindingNode? existingPathFindingNode;
        if (PathFindingNodeLibrary.TryGetValue(potentialPathFindingNode.ToCacheRep(), out existingPathFindingNode))
        {
            return existingPathFindingNode;
        }
        else
        {
            PathFindingNodeLibrary[potentialPathFindingNode.ToCacheRep()] = potentialPathFindingNode;
            return potentialPathFindingNode;
        }
    }

    public List<PathFindingNode> Neighbours = new();
    public Int64 Cost = Int64.MaxValue;
    //public List<PathFindingNode> Path = new();
    public bool Visited = false;

    public int EmptyNodeX;
    public int EmptyNodeY;
    public int DataNodeX;
    public int DataNodeY;

    public PathFindingNode(int emptyNodeX, int emptyNodeY, int dataNodeX, int dataNodeY)
    {
        this.EmptyNodeX = emptyNodeX;
        this.EmptyNodeY = emptyNodeY;
        this.DataNodeX = dataNodeX;
        this.DataNodeY = dataNodeY;
    }

    public void Print()
    {
        for (int y = 0; y < Node.Height; y++)
        {
            for (int x = 0; x < Node.Width; x++)
            {
                Node node = Node.Map[(x, y)];
                if (x == EmptyNodeX && y == EmptyNodeY)
                    Console.Write('_');
                else if (x == DataNodeX && y == DataNodeY)
                    Console.Write('G');
                else if (node.Big)
                    Console.Write('#');
                else
                    Console.Write('.');
                Console.Write(' ');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public void ProcessNeighbours()
    {
        // Empty node is left or right of data node
        if ((EmptyNodeX == DataNodeX - 1 && EmptyNodeY == DataNodeY) ||
            (EmptyNodeX == DataNodeX + 1 && EmptyNodeY == DataNodeY))
            Neighbours.Add(PathFindingNode.GetOrCreatePathFindingNode(DataNodeX, EmptyNodeY, EmptyNodeX, DataNodeY));
        // Empty node is above or below data node
        if ((EmptyNodeX == DataNodeX && EmptyNodeY == DataNodeY - 1) ||
            (EmptyNodeX == DataNodeX && EmptyNodeY == DataNodeY + 1))
            Neighbours.Add(PathFindingNode.GetOrCreatePathFindingNode(EmptyNodeX, DataNodeY, DataNodeX, EmptyNodeY));

        // Empty could swap with node to the left
        if (EmptyNodeX > 0 && !Node.Map[(EmptyNodeX - 1, EmptyNodeY)].Big && (EmptyNodeX - 1 != DataNodeX || EmptyNodeY != DataNodeY))
            Neighbours.Add(PathFindingNode.GetOrCreatePathFindingNode(EmptyNodeX - 1, EmptyNodeY, DataNodeX, DataNodeY));
        // Empty could swap with node to the right
        if (EmptyNodeX < Node.Width - 1 && !Node.Map[(EmptyNodeX + 1, EmptyNodeY)].Big && (EmptyNodeX + 1 != DataNodeX || EmptyNodeY != DataNodeY))
            Neighbours.Add(PathFindingNode.GetOrCreatePathFindingNode(EmptyNodeX + 1, EmptyNodeY, DataNodeX, DataNodeY));
        // Empty could swap with node above
        if (EmptyNodeY > 0 && !Node.Map[(EmptyNodeX, EmptyNodeY - 1)].Big && (EmptyNodeX != DataNodeX || EmptyNodeY - 1 != DataNodeY))
            Neighbours.Add(PathFindingNode.GetOrCreatePathFindingNode(EmptyNodeX, EmptyNodeY - 1, DataNodeX, DataNodeY));
        // Empty could swap with node below
        if (EmptyNodeY < Node.Height - 1 && !Node.Map[(EmptyNodeX, EmptyNodeY + 1)].Big && (EmptyNodeX != DataNodeX || EmptyNodeY + 1 != DataNodeY))
            Neighbours.Add(PathFindingNode.GetOrCreatePathFindingNode(EmptyNodeX, EmptyNodeY + 1, DataNodeX, DataNodeY));
    }

    public string ToCacheRep()
    {
        return $"({EmptyNodeX},{EmptyNodeY})({DataNodeX},{DataNodeY})";
    }
}