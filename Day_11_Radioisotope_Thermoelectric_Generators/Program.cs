using AdventOfCodeUtilities;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

List<string> inputList = AoC.GetInputLines();

Node initialNode = new();
initialNode.ElevatorFloor = 0;
for (int floor = 0; floor < 4; floor++)
{
    var split = inputList[floor].Split(' ');
    for (int i = 0; i < split.Length; i++)
    {
        if (split[i].StartsWith("generator"))
            initialNode.DeviceToFloorMap["g-" + split[i - 1]] = floor;
        else if (split[i].StartsWith("microchip"))
            initialNode.DeviceToFloorMap["m-" + split[i - 1].Split('-').First()] = floor;
    }
}

Node.NodeLibrary[initialNode.ToString()] = initialNode;

void P1()
{
    List<Node> newNodesCreated = new() { initialNode };
    while (newNodesCreated.Any())
    {
        Node newNode = newNodesCreated[0];
        newNodesCreated.RemoveAt(0);
        newNodesCreated.AddRange(newNode.ProcessNeighbours());
    }

    initialNode.Cost = 0;
    HashSet<Node> remainingNodesToVisit = new(Node.NodeLibrary.Values);
    PriorityQueue<Node, Int64> remainingNodes = new(remainingNodesToVisit.Select(node => (node, node.Cost)));

    while (remainingNodesToVisit.Any())
    {
        Node currentNode = remainingNodes.Dequeue();

        foreach ((int additionalCost, Node neighbourNode) in currentNode.Neighbours.Where(pair => !pair.Item2.Visited))
        {
            Int64 newCost = currentNode.Cost + additionalCost;
            if (newCost < neighbourNode.Cost)
            {
                neighbourNode.Cost = newCost;
                remainingNodes.Enqueue(neighbourNode, neighbourNode.Cost);
            }
        }

        currentNode.Visited = true;
        remainingNodesToVisit.Remove(currentNode);
    }

    Node dest = Node.NodeLibrary.First(kvp => kvp.Value.ElevatorFloor == 3 && kvp.Value.DeviceToFloorMap.Values.All(floor => floor == 3)).Value;

    Console.WriteLine(dest.Cost);
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
    // Part 1 & 2
    public static Dictionary<string, Node> NodeLibrary = new();

    public List<(int, Node)> Neighbours = new();
    public Int64 Cost = Int64.MaxValue;
    public bool Visited = false;

    public int ElevatorFloor;
    public Dictionary<string, int> DeviceToFloorMap = new Dictionary<string, int>();

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
            int cost = 1;

            // Potential neighbours here, what could we do
            var devicesOnThisFloor = DeviceToFloorMap.Where(kvp => kvp.Value == ElevatorFloor).Select(kvp => kvp.Key).ToArray();
            var devicesOnDestFloor = DeviceToFloorMap.Where(kvp => kvp.Value == destFloor).Select(kvp => kvp.Key).ToArray();

            for (int deviceAIndex = 0; deviceAIndex < devicesOnThisFloor.Count(); deviceAIndex++)
            {
                string deviceA = devicesOnThisFloor[deviceAIndex];

                Node potentialNeighbour = new();
                potentialNeighbour.ElevatorFloor = destFloor;
                potentialNeighbour.DeviceToFloorMap = new Dictionary<string, int>(DeviceToFloorMap);
                potentialNeighbour.DeviceToFloorMap[deviceA] = destFloor;
                if (potentialNeighbour.CheckStateValid())
                {
                    string potentialNeighbourStringRep = potentialNeighbour.ToString();
                    Node? existingNeighbour;
                    if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                        potentialNeighbour = existingNeighbour;
                    else
                    {
                        Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                        newNodes.Add(potentialNeighbour);
                    }
                    Neighbours.Add((cost, potentialNeighbour));
                }

                for (int deviceBIndex = deviceAIndex + 1; deviceBIndex < devicesOnThisFloor.Count(); deviceBIndex++)
                {
                    string deviceB = devicesOnThisFloor[deviceBIndex];

                    potentialNeighbour = new();
                    potentialNeighbour.ElevatorFloor = destFloor;
                    potentialNeighbour.DeviceToFloorMap = new Dictionary<string, int>(DeviceToFloorMap);
                    potentialNeighbour.DeviceToFloorMap[deviceA] = destFloor;
                    potentialNeighbour.DeviceToFloorMap[deviceB] = destFloor;
                    if (potentialNeighbour.CheckStateValid())
                    {
                        string potentialNeighbourStringRep = potentialNeighbour.ToString();
                        Node? existingNeighbour;
                        if (Node.NodeLibrary.TryGetValue(potentialNeighbourStringRep, out existingNeighbour))
                            potentialNeighbour = existingNeighbour;
                        else
                        {
                            Node.NodeLibrary[potentialNeighbourStringRep] = potentialNeighbour;
                            newNodes.Add(potentialNeighbour);
                        }
                        Neighbours.Add((cost, potentialNeighbour));
                    }
                }
            }
        }

        return newNodes;
    }

    public bool CheckStateValid()
    {
        for (int floor = 0; floor < 4; floor++)
        {
            var microchipsOnThisFloor = DeviceToFloorMap.Where(kvp => kvp.Value == floor && kvp.Key.StartsWith("m-")).Select(kvp => kvp.Key[2..]).ToArray();
            var rtgsOnThisFloor = DeviceToFloorMap.Where(kvp => kvp.Value == floor && kvp.Key.StartsWith("g-")).Select(kvp => kvp.Key[2..]).ToArray();

            if (rtgsOnThisFloor.Length > 0)
            {
                foreach (string microchipOnThisFloor in microchipsOnThisFloor)
                {
                    if (rtgsOnThisFloor.Any(n => n != microchipOnThisFloor) && !rtgsOnThisFloor.Any(n => n == microchipOnThisFloor))
                        return false;
                }
            }
        }
        return true;
    }

    public override string ToString()
    {
        return $"{ElevatorFloor}-{string.Join(",", DeviceToFloorMap.Select(kvp => $"{kvp.Key}{kvp.Value}"))}";
    }

}