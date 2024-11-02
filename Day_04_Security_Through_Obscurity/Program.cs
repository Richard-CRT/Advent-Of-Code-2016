using AdventOfCodeUtilities;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
List<Room> roomList = inputList.Select(l => new Room(l)).ToList();
List<Room> realRoomList = roomList.Where(r => r.CorrectChecksum == r.Checksum).ToList();

void P1()
{
    int sum = realRoomList.Select(r => r.SectorID).Sum();
    Console.WriteLine(sum);
    Console.ReadLine();
}

void P2()
{
    int result = realRoomList.Where(r => r.DecryptedName == "northpole object storage").First().SectorID;
    Console.WriteLine(result);
    Console.ReadLine();
}

P1();
P2();

public class Room
{
    public string Name;
    public int SectorID;
    public string Checksum;
    public string CorrectChecksum;
    public string DecryptedName;
    public Dictionary<char, int> Foo = new();

    public Room(string line)
    {
        var split = line.Split('[');
        int dashSplitIndex = split[0].LastIndexOf('-');
        Name = split[0][..dashSplitIndex];
        SectorID = int.Parse(split[0][(dashSplitIndex+1)..]);
        Checksum = split[^1][..^1];

        for (char c = 'a'; c <= 'z'; c++)
        {
            Foo[c] = 0;
        }

        foreach (char c in Name)
        {
            if (Foo.ContainsKey(c))
                Foo[c]++;
        }

        CorrectChecksum = string.Join("", Foo.OrderByDescending(c => c.Value).Take(5).Select(kvp => kvp.Key));
        DecryptedName = string.Join("", Name.Select(c => c == '-' ? ' ' : (char)((((int)(c - 'a') + SectorID) % 26) + 'a')));
    }
}