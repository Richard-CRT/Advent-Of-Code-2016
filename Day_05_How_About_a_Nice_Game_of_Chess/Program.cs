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
            string md5 = AoC.MD5(comb);
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
            string md5 = AoC.MD5(comb);
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
