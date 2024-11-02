using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
List<IP> ips = inputList.Select(s => new IP(s)).ToList();

void P1()
{
    int result = ips.Where(ip => ip.TLSSupported).Count();
    Console.WriteLine(result);
    Console.ReadLine();
}

void P2()
{
    int result = ips.Where(ip => ip.SSLSupported).Count();
    Console.WriteLine(result);
    Console.ReadLine();
}

P1();
P2();

public class IP
{
    public string Full;
    public List<string> SupernetSequences = new List<string>();
    public List<string> HypernetSequences = new List<string>();
    public bool TLSSupported = false;
    public bool SSLSupported = false;

    public IP(string s)
    {
        Full = s;
        var split = s.Split(new char[] { '[', ']' });
        for (int i = 0; i < split.Length; i += 2)
        {
            SupernetSequences.Add(split[i]);
        }
        for (int i = 1; i < split.Length; i+=2)
        {
            HypernetSequences.Add(split[i]);
        }

        bool containsAbba = false;
        for (int i = 0; i < Full.Length - 3; i++)
        {
            if (Full[i] != Full[i+1] && Full[i] == Full[i + 3] && Full[i + 1] == Full[i + 2])
            {
                containsAbba = true;
                break;
            }
        }
        bool abbaInHypernetSequence = false;
        foreach (string hypernetSequence in HypernetSequences)
        {
            for (int i = 0; i < hypernetSequence.Length - 3; i++)
            {
                if (hypernetSequence[i] != hypernetSequence[i + 1] && hypernetSequence[i] == hypernetSequence[i + 3] && hypernetSequence[i + 1] == hypernetSequence[i + 2])
                {
                    abbaInHypernetSequence = true;
                    break;
                }
            }
            if (abbaInHypernetSequence)
                break;
        }
        if (containsAbba && !abbaInHypernetSequence)
            TLSSupported = true;

        List<string> abasInSupernetSequences = new();
        foreach (string supernetSequence in SupernetSequences)
        {
            for (int i = 0; i < supernetSequence.Length - 2; i++)
            {
                if (supernetSequence[i] != supernetSequence[i + 1] && supernetSequence[i] == supernetSequence[i + 2])
                {
                    abasInSupernetSequences.Add(supernetSequence[i..(i + 3)]);
                }
            }
        }
        foreach (string abaInSupernetSequence in abasInSupernetSequences)
        {
            bool abaHasMatchInHypernet = false;
            foreach (string hypernetSequence in HypernetSequences)
            {
                for (int i = 0; i < hypernetSequence.Length - 2; i++)
                {
                    if (hypernetSequence[i] == abaInSupernetSequence[1] && hypernetSequence[i+1] == abaInSupernetSequence[0] &&
                        hypernetSequence[i] != hypernetSequence[i + 1] && hypernetSequence[i] == hypernetSequence[i + 2])
                    {
                        SSLSupported = true;
                        abaHasMatchInHypernet = true;
                        break;
                    }
                }
                if (abaHasMatchInHypernet)
                    break;
            }
            if (abaHasMatchInHypernet)
                break;
        }
    }
}