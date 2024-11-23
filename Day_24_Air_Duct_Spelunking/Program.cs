using AdventOfCodeUtilities;
using System;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();

Coord.Height = inputList.Count;
Coord.Width = inputList[0].Length;

for (int y = 0; y < Coord.Height; y++)
{
    for (int x = 0; x < Coord.Width; x++)
    {
        Coord.Map[(x, y)] = new Coord(x, y, inputList[y][x]);
    }
}

void P12()
{
    string targetCacheRep = new string(Coord.Map.Values.Where(n => n.POI != ' ').Select(n => n.POI).Order().ToArray());
    Node.NumberOfPOIs = targetCacheRep.Length;

    Coord startCoord = Coord.Map.Values.First(n => n.POI == '0');
    Node initialNode = Node.GetOrCreateNode(startCoord.X, startCoord.Y, "0");

    initialNode.Cost = 0;
    PriorityQueue<Node, Int64> remainingNodes = new();
    remainingNodes.Enqueue(initialNode, 0);

    Node? p1DestNode = null;
    Node? p2DestNode = null;
    while (remainingNodes.Count > 0)
    {
        Node currentNode = remainingNodes.Dequeue();
        if (!currentNode.Visited)
        {
            if (p1DestNode is null && currentNode.PointsVisited.Length == Node.NumberOfPOIs)
            {
                p1DestNode = currentNode;
            }
            if (currentNode.PointsVisited.Length == Node.NumberOfPOIs + 1)
            {
                p2DestNode = currentNode;
                break;
            }

            currentNode.ProcessNeighbours();
            foreach (Node neighbourNode in currentNode.Neighbours.Where(node => !node.Visited))
            {
                Int64 newCost = currentNode.Cost + 1;
                if (newCost < neighbourNode.Cost)
                {
                    neighbourNode.Cost = newCost;
                    //neighbourNode.Path = new(currentNode.Path);
                    //neighbourNode.Path.Add(currentNode);
                    remainingNodes.Enqueue(neighbourNode, newCost);
                }
            }

            currentNode.Visited = true;
        }
    }

    Console.WriteLine(p1DestNode!.Cost);
    Console.ReadLine();
    Console.WriteLine(p2DestNode!.Cost);
    Console.ReadLine();
}

P12();

public class Coord
{
    public static int Height;
    public static int Width;
    public static Dictionary<(int, int), Coord> Map = new();

    public int X;
    public int Y;
    public bool Wall;
    public char POI = ' ';

    public Coord(int x, int y, char c)
    {
        X = x;
        Y = y;
        Wall = c == '#';
        if (!Wall)
        {
            if (c != '.')
                POI = c;
        }
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

public class Node
{
    public static int NumberOfPOIs = -1;

    public static Node GetOrCreateNode(int x, int y, string currentPointsVisited)
    {
        string proposedPointsVisited = currentPointsVisited;
        Coord coord = Coord.Map[(x, y)];
        if (currentPointsVisited.Length == Node.NumberOfPOIs)
        {
            if (coord.POI == '0')
            {
                proposedPointsVisited += 'Z';
            }
        }
        else if (currentPointsVisited.Length < Node.NumberOfPOIs)
        {
            if (coord.POI != ' ')
            {
                if (!currentPointsVisited.Contains(coord.POI))
                    proposedPointsVisited += coord.POI;
            }
            proposedPointsVisited = new(proposedPointsVisited.Order().ToArray());
        }

        Node potentialNode = new(x, y, proposedPointsVisited);

        Node? existingNode;
        var potentialCacheRep = potentialNode.ToCacheRep();
        if (NodeLibrary.TryGetValue(potentialCacheRep, out existingNode))
        {
            return existingNode;
        }
        else
        {
            NodeLibrary[potentialCacheRep] = potentialNode;
            return potentialNode;
        }
    }

    // Part 1 & 2
    public static Dictionary<(int, int, string), Node> NodeLibrary = new();

    public List<Node> Neighbours = new();
    public Int64 Cost = Int64.MaxValue;
    public List<Node> Path = new();
    public bool Visited = false;

    public int X;
    public int Y;
    public string PointsVisited;
    public bool Finished = false;

    public Node(int x, int y, string pointsVisited)
    {
        X = x;
        Y = y;
        PointsVisited = pointsVisited;
    }

    public void ProcessNeighbours()
    {
        if (X > 0 && !Coord.Map[(X - 1, Y)].Wall) Neighbours.Add(Node.GetOrCreateNode(X - 1, Y, PointsVisited));
        if (X < Coord.Width - 1 && !Coord.Map[(X + 1, Y)].Wall) Neighbours.Add(Node.GetOrCreateNode(X + 1, Y, PointsVisited));
        if (Y > 0 && !Coord.Map[(X, Y - 1)].Wall) Neighbours.Add(Node.GetOrCreateNode(X, Y - 1, PointsVisited));
        if (Y < Coord.Height - 1 && !Coord.Map[(X, Y + 1)].Wall) Neighbours.Add(Node.GetOrCreateNode(X, Y + 1, PointsVisited));
    }

    public (int, int, string) ToCacheRep()
    {
        return (X, Y, PointsVisited);
    }

    public override string ToString()
    {
        return $"{X}, {Y}, {PointsVisited}, {Finished}";
    }
}
