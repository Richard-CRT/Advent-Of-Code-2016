using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
string doorId = inputList[0];

void P1()
{
    string password = "";
    long i = 0;
    for (int j = 0; j < 8; j++)
    {
        while (true)
        {
            string comb = doorId + i;
            i++;
            string md5 = CreateMD5(comb);
            if (md5[0..5] == "00000")
            {
                password += md5[5];
                break;
            }
        }
    }

    Console.WriteLine(password.ToLower());
    Console.ReadLine();
}

void P2()
{
    char[] password = new char[8];
    for (int n = 0; n < 8; n++)
        password[n] = '_';
    long i = 0;
    for (int j = 0; j < 8; j++)
    {
        while (true)
        {
            string comb = doorId + i;
            i++;
            string md5 = CreateMD5(comb);
            if (md5[0..5] == "00000")
            {
                int index;
                if (int.TryParse(md5[5].ToString(), out index) && index >= 0 && index < 8)
                {
                    if (password[index] == '_')
                    {
                        password[index] = md5[6];
                        break;
                    }
                }
            }
        }
    }

    Console.WriteLine(string.Join("", password).ToLower());
    Console.ReadLine();
}

P1();
P2();

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
