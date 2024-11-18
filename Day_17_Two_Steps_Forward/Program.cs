using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
string passcode = inputList[0];

void P12()
{
    Node initialNode = Node.GetOrCreateNode(0, 0, passcode);

    initialNode.Cost = 0;
    PriorityQueue<Node, Int64> remainingNodes = new();
    remainingNodes.Enqueue(initialNode, 0);

    Node? destNode = null;
    Node? furthestEndNode = null;
    while (remainingNodes.Count > 0)
    {
        Node currentNode = remainingNodes.Dequeue();
        if (!currentNode.Visited)
        {
            if (currentNode.X == 3 & currentNode.Y == 3)
            {
                if (destNode is null)
                {
                    destNode = currentNode;
                    Console.WriteLine(destNode.Path[passcode.Length..]);
                    Console.ReadLine();
                }
                furthestEndNode = currentNode;
                //break;
            }

            currentNode.ProcessNeighbours();
            foreach (Node neighbourNode in currentNode.Neighbours.Where(node => !node.Visited))
            {
                Int64 newCost = currentNode.Cost + 1;
                if (newCost < neighbourNode.Cost)
                {
                    neighbourNode.Cost = newCost;
                    remainingNodes.Enqueue(neighbourNode, newCost);
                }
            }

            currentNode.Visited = true;
        }
    }

    Console.WriteLine(furthestEndNode!.Cost);
    Console.ReadLine();
}

P12();




public class Node
{
    public static Node GetOrCreateNode(int x, int y, string path)
    {
        string hash = AoC.MD5(path);

        Node? node;
        if (NodeLibrary.TryGetValue(hash, out node))
        {
            return node;
        }
        else
        {
            node = new(x, y, hash, path);
            NodeLibrary[hash] = node;
            return node;
        }
    }

    // Part 1 & 2
    public static Dictionary<string, Node> NodeLibrary = new();

    public List<Node> Neighbours = new();
    public Int64 Cost = Int64.MaxValue;
    public bool Visited = false;

    public int X;
    public int Y;
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;
    public string Path;

    public Node(int x, int y, string hash, string path)
    {
        X = x;
        Y = y;

        Up = hash[0] >= 'B';
        Down = hash[1] >= 'B';
        Left = hash[2] >= 'B';
        Right = hash[3] >= 'B';

        Path = path;
    }

    public List<Node> ProcessNeighbours()
    {
        List<Node> newNodes = new();

        if (X != 3 || Y != 3)
        {
            if (Up && Y > 0)
                Neighbours.Add(Node.GetOrCreateNode(X, Y - 1, Path + 'U'));
            if (Down && Y < 3)
                Neighbours.Add(Node.GetOrCreateNode(X, Y + 1, Path + 'D'));
            if (Left && X > 0)
                Neighbours.Add(Node.GetOrCreateNode(X - 1, Y, Path + 'L'));
            if (Right && X < 3)
                Neighbours.Add(Node.GetOrCreateNode(X + 1, Y, Path + 'R'));
        }

        return newNodes;
    }
}
