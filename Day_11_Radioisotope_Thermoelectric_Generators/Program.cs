using AdventOfCodeUtilities;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

List<string> inputList = AoC.GetInputLines();

List<string> chemistries = new();

Node initialNode = new();
initialNode.ElevatorFloor = 0;
for (int floor = 0; floor < 4; floor++)
{
    var split = inputList[floor].Split(' ');
    for (int i = 0; i < split.Length; i++)
    {
        if (split[i].StartsWith("generator") || split[i].StartsWith("microchip"))
        {
            string chemistry = split[i - 1].Split('-')[0];
            if (!chemistries.Contains(chemistry))
            {
                Node.ChemistryToIndexMap[chemistry] = chemistries.Count;
                Node.IndexToChemistryMap[chemistries.Count] = chemistry;
                chemistries.Add(chemistry);
            }

            if (split[i].StartsWith("generator"))
            {
                initialNode.GeneratorsByFloor[floor].Add(Node.ChemistryToIndexMap[chemistry]);
            }
            else if (split[i].StartsWith("microchip"))
            {
                initialNode.MicrochipsByFloor[floor].Add(Node.ChemistryToIndexMap[chemistry]);
            }
        }
    }
}

Node.ChemistryToIndexMap["elerium"] = chemistries.Count;
Node.IndexToChemistryMap[chemistries.Count] = "elerium";
chemistries.Add("elerium");
Node.ChemistryToIndexMap["dilithium"] = chemistries.Count;
Node.IndexToChemistryMap[chemistries.Count] = "dilithium";
chemistries.Add("dilithium");



void P1()
{
    Node initialNodeP1 = initialNode.Copy();
    Node.NodeLibrary[initialNodeP1.UniqueRepr()] = initialNodeP1;

    // Code is preserved for doing process neighbours before or during path finding

    List<Node> newNodesCreated = new() { initialNodeP1 };
    while (newNodesCreated.Any())
    {
        Node newNode = newNodesCreated[0];
        newNodesCreated.RemoveAt(0);
        newNodesCreated.AddRange(newNode.ProcessNeighbours());
    }

    Node destNode = initialNodeP1.Copy();
    destNode.ElevatorFloor = 3;
    for (int floor = 0; floor < 3; floor++)
    {
        foreach (int microchip in destNode.MicrochipsByFloor[floor])
        {
            destNode.MicrochipsByFloor[floor].Remove(microchip);
            destNode.MicrochipsByFloor[3].Add(microchip);
        }
        foreach (int generator in destNode.GeneratorsByFloor[floor])
        {
            destNode.GeneratorsByFloor[floor].Remove(generator);
            destNode.GeneratorsByFloor[3].Add(generator);
        }
    }
    destNode = Node.NodeLibrary[destNode.UniqueRepr()];
    //Node.NodeLibrary.Add(destNode.UniqueRepr(), destNode);

    initialNodeP1.Cost = 0;
    PriorityQueue<Node, Int64> remainingNodes = new();
    remainingNodes.Enqueue(initialNodeP1, 0);

    while (remainingNodes.Count > 0)
    {
        Node currentNode = remainingNodes.Dequeue();
        if (!currentNode.Visited)
        {
            if (currentNode == destNode)
                break;

            //currentNode.ProcessNeighbours();
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

    Console.WriteLine(destNode.Cost);
    Console.ReadLine();
}

void P2()
{
    Node initialNodeP2 = initialNode.Copy();
    initialNodeP2.MicrochipsByFloor[0].Add(Node.ChemistryToIndexMap["elerium"]);
    initialNodeP2.GeneratorsByFloor[0].Add(Node.ChemistryToIndexMap["elerium"]);
    initialNodeP2.MicrochipsByFloor[0].Add(Node.ChemistryToIndexMap["dilithium"]);
    initialNodeP2.GeneratorsByFloor[0].Add(Node.ChemistryToIndexMap["dilithium"]);
    Node.NodeLibrary[initialNodeP2.UniqueRepr()] = initialNodeP2;

    // Code is preserved for doing process neighbours before or during path finding

    List<Node> newNodesCreated = new() { initialNodeP2 };
    while (newNodesCreated.Any())
    {
        Node newNode = newNodesCreated[0];
        newNodesCreated.RemoveAt(0);
        newNodesCreated.AddRange(newNode.ProcessNeighbours());
    }

    Node destNode = initialNodeP2.Copy();
    destNode.ElevatorFloor = 3;
    for (int floor = 0; floor < 3; floor++)
    {
        foreach (int microchip in destNode.MicrochipsByFloor[floor])
        {
            destNode.MicrochipsByFloor[floor].Remove(microchip);
            destNode.MicrochipsByFloor[3].Add(microchip);
        }
        foreach (int generator in destNode.GeneratorsByFloor[floor])
        {
            destNode.GeneratorsByFloor[floor].Remove(generator);
            destNode.GeneratorsByFloor[3].Add(generator);
        }
    }
    destNode = Node.NodeLibrary[destNode.UniqueRepr()];
    //Node.NodeLibrary.Add(destNode.UniqueRepr(), destNode);

    initialNodeP2.Cost = 0;
    PriorityQueue<Node, Int64> remainingNodes = new();
    remainingNodes.Enqueue(initialNodeP2, 0);

    while (remainingNodes.Count > 0)
    {
        Node currentNode = remainingNodes.Dequeue();
        if (!currentNode.Visited)
        {
            if (currentNode == destNode)
                break;

            //currentNode.ProcessNeighbours();
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

    Console.WriteLine(destNode.Cost);
    Console.ReadLine();
}

P1();
P2();

public class Node
{
    // Only static to tostring can use it
    public static Dictionary<string, int> ChemistryToIndexMap = new();
    public static Dictionary<int, string> IndexToChemistryMap = new();

    // Part 1 & 2
    public static Dictionary<string, Node> NodeLibrary = new();

    public List<Node> Neighbours = new();
    public Int64 Cost = Int64.MaxValue;
    public bool Visited = false;

    public int ElevatorFloor;
    public Dictionary<int, HashSet<int>> MicrochipsByFloor = new Dictionary<int, HashSet<int>>() {
        { 0, new HashSet<int>() },
        { 1, new HashSet<int>() },
        { 2, new HashSet<int>() },
        { 3, new HashSet<int>() }
    };
    public Dictionary<int, HashSet<int>> GeneratorsByFloor = new Dictionary<int, HashSet<int>>() {
        { 0, new HashSet<int>() },
        { 1, new HashSet<int>() },
        { 2, new HashSet<int>() },
        { 3, new HashSet<int>() }
    };

    public Node Copy()
    {
        Node node = new Node();

        node.ElevatorFloor = ElevatorFloor;
        node.GeneratorsByFloor = new(GeneratorsByFloor.Select(kvp => new KeyValuePair<int, HashSet<int>>(kvp.Key, new(kvp.Value))));
        node.MicrochipsByFloor = new(MicrochipsByFloor.Select(kvp => new KeyValuePair<int, HashSet<int>>(kvp.Key, new(kvp.Value))));

        return node;
    }

    public List<Node> ProcessNeighbours()
    {
        List<Node> newNodes = new();

        List<int> destFloors = new();
        if (ElevatorFloor > 0)
            destFloors.Add(ElevatorFloor - 1);
        if (ElevatorFloor < 3)
            destFloors.Add(ElevatorFloor + 1);

        foreach (int destFloor in destFloors)
        {
            //int cost = Math.Abs(destFloor - ElevatorFloor); // Artifact of previous approach, cost will always be 1

            // Potential neighbours here, what could we do
            var generatorsOnThisFloor = GeneratorsByFloor[ElevatorFloor];
            var microchipsOnThisFloor = MicrochipsByFloor[ElevatorFloor];
            var generatorsOnThisFloorArray = generatorsOnThisFloor.ToArray();
            var microchipsOnThisFloorArray = microchipsOnThisFloor.ToArray();

            Node potentialNeighbour;
            Node? existingNeighbour;
            string potentialNeighbourStringRep;

            for (int generatorAIndex = 0; generatorAIndex < generatorsOnThisFloorArray.Length; generatorAIndex++)
            {
                int generatorA = generatorsOnThisFloorArray[generatorAIndex];

                // Take just the generator
                potentialNeighbour = this.Copy();
                potentialNeighbour.ElevatorFloor = destFloor;
                potentialNeighbour.GeneratorsByFloor[destFloor].Add(generatorA);
                potentialNeighbour.GeneratorsByFloor[ElevatorFloor].Remove(generatorA);
                potentialNeighbourStringRep = potentialNeighbour.UniqueRepr();
                if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                    Neighbours.Add(existingNeighbour);
                else
                {
                    if (potentialNeighbour.CheckStateValid(ElevatorFloor, destFloor))
                    {
                        Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                        newNodes.Add(potentialNeighbour);
                        Neighbours.Add(potentialNeighbour);
                    }
                }

                // Take the microchip too
                if (microchipsOnThisFloor.Contains(generatorA))
                {
                    potentialNeighbour = this.Copy();
                    potentialNeighbour.ElevatorFloor = destFloor;
                    potentialNeighbour.GeneratorsByFloor[destFloor].Add(generatorA);
                    potentialNeighbour.GeneratorsByFloor[ElevatorFloor].Remove(generatorA);
                    potentialNeighbour.MicrochipsByFloor[destFloor].Add(generatorA);
                    potentialNeighbour.MicrochipsByFloor[ElevatorFloor].Remove(generatorA);
                    potentialNeighbourStringRep = potentialNeighbour.UniqueRepr();
                    if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                        Neighbours.Add(existingNeighbour);
                    else
                    {
                        if (potentialNeighbour.CheckStateValid(ElevatorFloor, destFloor))
                        {
                            Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                            newNodes.Add(potentialNeighbour);
                            Neighbours.Add(potentialNeighbour);
                        }
                    }
                }

                // Take another generator
                for (int generatorBIndex = generatorAIndex + 1; generatorBIndex < generatorsOnThisFloorArray.Length; generatorBIndex++)
                {
                    int generatorB = generatorsOnThisFloorArray[generatorBIndex];

                    potentialNeighbour = this.Copy();
                    potentialNeighbour.ElevatorFloor = destFloor;
                    potentialNeighbour.GeneratorsByFloor[destFloor].Add(generatorA);
                    potentialNeighbour.GeneratorsByFloor[destFloor].Add(generatorB);
                    potentialNeighbour.GeneratorsByFloor[ElevatorFloor].Remove(generatorA);
                    potentialNeighbour.GeneratorsByFloor[ElevatorFloor].Remove(generatorB);
                    potentialNeighbourStringRep = potentialNeighbour.UniqueRepr();
                    if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                        Neighbours.Add(existingNeighbour);
                    else
                    {
                        if (potentialNeighbour.CheckStateValid(ElevatorFloor, destFloor))
                        {
                            Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                            newNodes.Add(potentialNeighbour);
                            Neighbours.Add(potentialNeighbour);
                        }
                    }
                }
            }

            for (int microchipAIndex = 0; microchipAIndex < microchipsOnThisFloorArray.Length; microchipAIndex++)
            {
                int microchipA = microchipsOnThisFloorArray[microchipAIndex];

                // Take just the microchip
                potentialNeighbour = this.Copy();
                potentialNeighbour.ElevatorFloor = destFloor;
                potentialNeighbour.MicrochipsByFloor[destFloor].Add(microchipA);
                potentialNeighbour.MicrochipsByFloor[ElevatorFloor].Remove(microchipA);
                potentialNeighbourStringRep = potentialNeighbour.UniqueRepr();
                if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                    Neighbours.Add(existingNeighbour);
                else
                {
                    if (potentialNeighbour.CheckStateValid(ElevatorFloor, destFloor))
                    {
                        Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                        newNodes.Add(potentialNeighbour);
                        Neighbours.Add(potentialNeighbour);
                    }
                }

                // Microchip Generator combo handled by the other branch

                // Take another microchip
                for (int microchipBIndex = microchipAIndex + 1; microchipBIndex < microchipsOnThisFloorArray.Length; microchipBIndex++)
                {
                    int microchipB = microchipsOnThisFloorArray[microchipBIndex];

                    potentialNeighbour = this.Copy();
                    potentialNeighbour.ElevatorFloor = destFloor;
                    potentialNeighbour.MicrochipsByFloor[destFloor].Add(microchipA);
                    potentialNeighbour.MicrochipsByFloor[destFloor].Add(microchipB);
                    potentialNeighbour.MicrochipsByFloor[ElevatorFloor].Remove(microchipA);
                    potentialNeighbour.MicrochipsByFloor[ElevatorFloor].Remove(microchipB);
                    potentialNeighbourStringRep = potentialNeighbour.UniqueRepr();
                    if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                        Neighbours.Add(existingNeighbour);
                    else
                    {
                        if (potentialNeighbour.CheckStateValid(ElevatorFloor, destFloor))
                        {
                            Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                            newNodes.Add(potentialNeighbour);
                            Neighbours.Add(potentialNeighbour);
                        }
                    }
                }
            }
        }

        return newNodes;
    }

    public bool CheckStateValid(int floorChanged1, int floorChanged2)
    {
        return CheckFloorStateValid(floorChanged1) && CheckFloorStateValid(floorChanged2);
    }

    public bool CheckFloorStateValid(int floor)
    {
        var rtgsOnThisFloor = GeneratorsByFloor[floor];
        if (rtgsOnThisFloor.Any())
        {
            var microchipsOnThisFloor = MicrochipsByFloor[floor];

            var unpairedMicrochipsOnThisFloor = microchipsOnThisFloor.Except(rtgsOnThisFloor);
            if (unpairedMicrochipsOnThisFloor.Any())
                return false;
        }
        return true;
    }


    public override string ToString()
    {
        //return $"{ElevatorFloor}-{string.Join('|', GeneratorsByFloor.Select(kvp => $"{kvp.Key}:{string.Join(',', kvp.Value.Order().Select(i => Node.IndexToChemistryMap[i]))}"))}-{string.Join('|', MicrochipsByFloor.Select(kvp => $"{kvp.Key}:{string.Join(',', kvp.Value.Order().Select(i => Node.IndexToChemistryMap[i]))}"))}";
        return UniqueRepr();
    }

    public string UniqueRepr()
    {
        //return $"{ElevatorFloor}-{string.Join('|', GeneratorsByFloor.Select(kvp => $"{kvp.Key}:{string.Join(',', kvp.Value.Order())}"))}-{string.Join('|', MicrochipsByFloor.Select(kvp => $"{kvp.Key}:{string.Join(',', kvp.Value.Order())}"))}";

        // Pairs are all interchangeable, so the unique rep doesn't need to differentiate anything except pairs and elevator floor
        List<(int, int)> pairs = new();

        for (int i = 0; i < 4; i++)
        {
            foreach (int generator in GeneratorsByFloor[i])
            {
                for (int j = 0; j < 4; j++)
                {
                    if (MicrochipsByFloor[j].Contains(generator))
                    {
                        pairs.Add((i, j));
                        break;
                    }
                }
            }
        }

        return $"{ElevatorFloor}-{string.Join(',', pairs.Order())}";
    }
}