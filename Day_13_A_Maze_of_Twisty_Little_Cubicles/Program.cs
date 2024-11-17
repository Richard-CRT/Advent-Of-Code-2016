using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();

void P12()
{
    Node initialNode = new(1, 1);

    initialNode.Cost = 0;
    PriorityQueue<Node, Int64> remainingNodes = new();
    remainingNodes.Enqueue(initialNode, 0);

    Node destNode = new(31, 39);

    Int64 destNodeCost = -1;
    int numberOf50Locations = -1;

    while (remainingNodes.Count > 0)
    {
        Node currentNode = remainingNodes.Dequeue();
        if (!currentNode.Visited)
        {
            if (currentNode == destNode)
            {
                destNodeCost = destNode.Cost;
                if (numberOf50Locations != -1)
                    break;
            }

            if (currentNode.Cost > 50)
            {
                numberOf50Locations = Node.NodeLibrary.Values.Count(node => node.Cost <= 50);
                if (destNodeCost != -1)
                    break;
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

    Console.WriteLine(destNodeCost);
    Console.ReadLine();
    Console.WriteLine(numberOf50Locations);
    Console.ReadLine();
}

Node.DesignersFavouriteNumber = int.Parse(inputList[0]);

P12();

public class Node
{
    // Part 1 & 2
    public static Dictionary<(int, int), Node> NodeLibrary = new();
    public static int DesignersFavouriteNumber = 0;

    public List<Node> Neighbours = new();
    public Int64 Cost = Int64.MaxValue;
    public bool Visited = false;

    public int X;
    public int Y;
    public bool Space = false;

    public Node(int x, int y)
    {
        X = x;
        Y = y;
        NodeLibrary[(x, y)] = this;

        Int64 eq = (x * x) + (3 * x) + (2 * x * y) + y + (y * y);
        eq += DesignersFavouriteNumber;
        string binaryString = Convert.ToString(eq, 2);
        int num1s = binaryString.Count(c => c == '1');
        Space = num1s % 2 == 0;            
    }

    public List<Node> ProcessNeighbours()
    {
        List<Node> newNodes = new();

        foreach ((int x, int y) in new List<(int, int)>() { (X + 1, Y), (X, Y + 1), (X - 1, Y), (X, Y - 1) })
        {
            if (x >= 0 && y >= 0)
            {
                Node? node;
                if (Node.NodeLibrary.TryGetValue((x,y), out node))
                {
                }
                else
                {
                    node = new(x, y);
                    newNodes.Add(node);
                }

                if (node.Space)
                    Neighbours.Add(node);
            }
        }

        return newNodes;
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
}