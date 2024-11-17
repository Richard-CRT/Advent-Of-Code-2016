using AdventOfCodeUtilities;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

List<string> inputList = AoC.GetInputLines();

IEnumerable<char> Operate(IEnumerable<char> input)
{
    IEnumerable<char> b = input.Select(c => c == '1' ? '0' : '1').Reverse();
    IEnumerable<char> output = input.Append('0').Concat(b);
    return output;
}

List<char> Checksum(List<char> data)
{
    List<char> checksum = new();

    for (int i = 0; i < data.Count - 1; i+=2)
    {
        if (data[i] == data[i + 1])
            checksum.Add('1');
        else
            checksum.Add('0');
    }

    return checksum;
}

void P1()
{
    IEnumerable<char> data = inputList[0];
    int len = data.Count();

    const int diskLen = 272;

    while (len < diskLen)
    {
        data = Operate(data);
        len = len * 2 + 1;
    }

    IEnumerable<char> relevantData = data.Take(diskLen);

    List<char> checksum = Checksum(relevantData.ToList());
    while (checksum.Count % 2 == 0)
        checksum = Checksum(checksum);

    Console.WriteLine(string.Concat(checksum));
    Console.ReadLine();
}

void P2()
{
    IEnumerable<char> data = inputList[0];
    int len = data.Count();

    const int diskLen = 35651584;

    while (len < diskLen)
    {
        data = Operate(data);
        len = len * 2 + 1;
    }

    IEnumerable<char> relevantData = data.Take(diskLen);

    List<char> checksum = Checksum(relevantData.ToList());
    while (checksum.Count % 2 == 0)
        checksum = Checksum(checksum);

    Console.WriteLine(string.Concat(checksum));
    Console.ReadLine();
}

P1();
P2();
