using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
string salt = inputList[0];

void P12(bool p2=false)
{
    PriorityQueue<Key, int> keys = new();
    List<Key> pending_keys = new();

    int indexWhenKeysHit64 = int.MaxValue - 1000;

    int index = 0;
    while (index < indexWhenKeysHit64 + 1000)
    {
        string trial = salt + index;
        string potential_key = CreateMD5(trial).ToLower();

        if (p2)
            for (int i = 0; i < 2016; i++)
                potential_key = CreateMD5(potential_key).ToLower();

        HashSet<char> fiveOfKindChars = new();

        bool addedTriplet = false;
        for (int i = 0; i < potential_key.Length - 2; i++)
        {
            if (potential_key[i] == potential_key[i + 1] &&
                potential_key[i + 1] == potential_key[i + 2])
            {
                if (!addedTriplet)
                {
                    addedTriplet = true;
                    pending_keys.Add(new(index, potential_key, potential_key[i]));
                }

                if (i < potential_key.Length - 4 &&
                    potential_key[i] == potential_key[i + 1] &&
                    potential_key[i + 1] == potential_key[i + 2] &&
                    potential_key[i + 2] == potential_key[i + 3] &&
                    potential_key[i + 3] == potential_key[i + 4])
                {
                    fiveOfKindChars.Add(potential_key[i]);
                }
            }
        }

        if (pending_keys.Any())
        {
            while (pending_keys[0].Index < index - 1000)
                pending_keys.RemoveAt(0);

            for (int i = 0; i < pending_keys.Count && pending_keys[i].Index < index; i++)
            {
                if (fiveOfKindChars.Contains(pending_keys[i].Triplet))
                {
                    keys.Enqueue(pending_keys[i], -pending_keys[i].Index);
                    pending_keys.RemoveAt(i);
                    i--;

                    if (keys.Count == 64)
                        indexWhenKeysHit64 = index;
                }
            }
        }

        index++;
    }

    while (keys.Count > 64)
        keys.Dequeue();

    Console.WriteLine(keys.Peek().Index);
    Console.ReadLine();
}

void P2()
{
    int result = 0;
    Console.WriteLine(result);
    Console.ReadLine();
}

P12();
P12(true);

static string CreateMD5(string input)
{
    // Use input string to calculate MD5 hash
    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
    {
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes); // .NET 5 +

        // Convert the byte array to hexadecimal string prior to .NET 5
        // StringBuilder sb = new System.Text.StringBuilder();
        // for (int i = 0; i < hashBytes.Length; i++)
        // {
        //     sb.Append(hashBytes[i].ToString("X2"));
        // }
        // return sb.ToString();
    }
}

class Key
{
    public int Index;
    public string Hash;
    public char Triplet;

    public Key(int index, string hash, char triplet)
    {
        Index = index;
        Hash = hash;
        Triplet = triplet;
    }
}