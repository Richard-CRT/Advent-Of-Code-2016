using AdventOfCodeUtilities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
const int inputLength = 8;

Letter LetterFromString(string s)
{
    Letter firstLetter = new Letter(s[0]);
    Letter iterateLetter = firstLetter;
    for (int i = 1; i < s.Length; i++)
    {
        char c = s[i];
        Letter nextLetter = new Letter(c);
        iterateLetter.NextLetter = nextLetter;
        nextLetter.PreviousLetter = iterateLetter;
        iterateLetter = nextLetter;
    }
    iterateLetter.NextLetter = firstLetter;
    firstLetter.PreviousLetter = iterateLetter;
    return firstLetter;
}

Letter GetLetterAtIndex(Letter firstLetter, int index)
{
    Letter iterateLetter = firstLetter;
    for (int i = 0; i < index; i++)
        iterateLetter = iterateLetter.NextLetter;
    return iterateLetter;
}

void Print(Letter firstLetter)
{
    Letter iterateLetter = firstLetter;
    do
    {
        Console.Write(iterateLetter.C);
        iterateLetter = iterateLetter.NextLetter;
    } while (iterateLetter != firstLetter);
    Console.WriteLine();
}

int GetIndex(Letter firstLetter, char c)
{
    Letter iterateLetter = firstLetter;
    for (int i = 0; i < inputLength; i++)
    {
        if (iterateLetter.C == c)
            return i;
        iterateLetter = iterateLetter.NextLetter;
    }
    throw new Exception();
}

Letter SwapIndexes(Letter firstLetter, int pos1, int pos2)
{
    Letter letterAtPos1 = GetLetterAtIndex(firstLetter, pos1);
    Letter letterAtPos2 = GetLetterAtIndex(firstLetter, pos2);

    if (pos1 == 0)
        firstLetter = letterAtPos2;
    if (pos2 == 0)
        firstLetter = letterAtPos1;

    if (letterAtPos1.NextLetter == letterAtPos2)
    {
        Letter letterBeforePos1 = letterAtPos1.PreviousLetter;
        Letter letterAfterPos2 = letterAtPos2.NextLetter;

        letterBeforePos1.NextLetter = letterAtPos2;
        letterAtPos2.PreviousLetter = letterBeforePos1;
        letterAtPos2.NextLetter = letterAtPos1;
        letterAtPos1.PreviousLetter = letterAtPos2;
        letterAtPos1.NextLetter = letterAfterPos2;
        letterAfterPos2.PreviousLetter = letterAtPos1;
    }
    else if (letterAtPos2.NextLetter == letterAtPos1)
    {
        Letter letterBeforePos2 = letterAtPos2.PreviousLetter;
        Letter letterAfterPos1 = letterAtPos1.NextLetter;

        letterBeforePos2.NextLetter = letterAtPos1;
        letterAtPos1.PreviousLetter = letterBeforePos2;
        letterAtPos1.NextLetter = letterAtPos2;
        letterAtPos2.PreviousLetter = letterAtPos1;
        letterAtPos2.NextLetter = letterAfterPos1;
        letterAfterPos1.PreviousLetter = letterAtPos2;
    }
    else
    {
        Letter letterBeforePos1 = letterAtPos1.PreviousLetter;
        Letter letterAfterPos1 = letterAtPos1.NextLetter;
        Letter letterBeforePos2 = letterAtPos2.PreviousLetter;
        Letter letterAfterPos2 = letterAtPos2.NextLetter;

        letterBeforePos1.NextLetter = letterAtPos2;
        letterAfterPos1.PreviousLetter = letterAtPos2;
        letterAtPos2.PreviousLetter = letterBeforePos1;
        letterAtPos2.NextLetter = letterAfterPos1;

        letterBeforePos2.NextLetter = letterAtPos1;
        letterAfterPos2.PreviousLetter = letterAtPos1;
        letterAtPos1.PreviousLetter = letterBeforePos2;
        letterAtPos1.NextLetter = letterAfterPos2;
    }

    return firstLetter;
}

Letter Rotate(Letter firstLetter, bool right, int count)
{
    if (right)
    {
        for (int i = 0; i < count; i++)
            firstLetter = firstLetter.PreviousLetter;
    }
    else
    {
        for (int i = 0; i < count; i++)
            firstLetter = firstLetter.NextLetter;
    }

    return firstLetter;
}

Letter Operate(Letter firstLetter, string instruction, bool reverse)
{
    var split = instruction.Split(' ');
    if (split[0] == "swap")
    {
        if (split[1] == "position")
        {
            int pos1 = int.Parse(split[2]);
            int pos2 = int.Parse(split[5]);
            firstLetter = SwapIndexes(firstLetter, pos1, pos2);
        }
        else if (split[1] == "letter")
        {
            int pos1 = GetIndex(firstLetter, split[2][0]);
            int pos2 = GetIndex(firstLetter, split[5][0]);
            firstLetter = SwapIndexes(firstLetter, pos1, pos2);
        }
    }
    else if (split[0] == "rotate")
    {
        if (split[1] == "based")
        {
            if (!reverse)
            {
                int pos = GetIndex(firstLetter, split[6][0]);
                firstLetter = Rotate(firstLetter, true, 1 + pos + (pos >= 4 ? 1 : 0));
                /*
                0 -> +1 -> 1
                1 -> +2 -> 3
                2 -> +3 -> 5
                3 -> +4 -> 7
                4 -> +6 -> 10 = 2
                5 -> +7 -> 12 = 4
                6 -> +8 -> 14 = 6
                7 -> +9 -> 16 = 8
                */
            }
            else
            {
                int pos = GetIndex(firstLetter, split[6][0]);
                //firstLetter = Rotate(firstLetter, false, 1 + pos + (pos >= 4 ? 1 : 0));
                if (pos % 2 == 0)
                    firstLetter = Rotate(firstLetter, false, (pos / 2) + 5);
                else
                    firstLetter = Rotate(firstLetter, false, (pos + 1) / 2);
            }
        }
        else
        {
            int count = int.Parse(split[2]);
            firstLetter = Rotate(firstLetter, split[1] == "right" ? !reverse : reverse, count);
        }
    }
    else if (split[0] == "reverse")
    {
        int pos1 = int.Parse(split[2]);
        int pos2 = int.Parse(split[4]);
        Debug.Assert(pos2 > pos1);
        int subsetLen = pos2 - pos1 + 1;
        List<Letter> subset = new();
        Letter letterStartSubset = GetLetterAtIndex(firstLetter, pos1);
        Letter iterateSubset = letterStartSubset;
        for (int i = 0; i < subsetLen; i++)
        {
            subset.Add(iterateSubset);
            iterateSubset = iterateSubset.NextLetter;
        }

        Letter letterEndSubset = subset.Last();

        Letter letterBeforeSubset = letterStartSubset.PreviousLetter;
        Letter letterAfterSubset = letterEndSubset.NextLetter;

        if (pos1 == 0)
            firstLetter = letterEndSubset;

        for (int i = 0; i < subsetLen - 1; i++)
        {
            subset[i].PreviousLetter = subset[i + 1];
        }
        for (int i = 1; i < subsetLen; i++)
        {
            subset[i].NextLetter = subset[i - 1];
        }

        if (letterAfterSubset == letterStartSubset)
        {
            letterStartSubset.NextLetter = letterEndSubset;
            letterEndSubset.PreviousLetter = letterStartSubset;
        }
        else
        {
            letterBeforeSubset.NextLetter = letterEndSubset;
            letterEndSubset.PreviousLetter = letterBeforeSubset;

            letterStartSubset.NextLetter = letterAfterSubset;
            letterAfterSubset.PreviousLetter = letterStartSubset;
        }
    }
    else if (split[0] == "move")
    {
        int pos1 = int.Parse(split[2]);
        int pos2 = int.Parse(split[5]);

        if (reverse)
        {
            int temp = pos1;
            pos1 = pos2;
            pos2 = temp;
        }

        Debug.Assert(pos1 != pos2);

        Letter letterAtPos1 = GetLetterAtIndex(firstLetter, pos1);
        Letter letterAtPos2 = GetLetterAtIndex(firstLetter, pos2);

        if (pos1 == 0)
            firstLetter = letterAtPos1.NextLetter;
        if (pos2 == 0)
            firstLetter = letterAtPos1;

        // Remove letter from pos1
        letterAtPos1.PreviousLetter.NextLetter = letterAtPos1.NextLetter;
        letterAtPos1.NextLetter.PreviousLetter = letterAtPos1.PreviousLetter;

        if (pos2 < pos1)
        {
            Letter letterBeforePos2 = letterAtPos2.PreviousLetter;
            letterBeforePos2.NextLetter = letterAtPos1;
            letterAtPos2.PreviousLetter = letterAtPos1;
            letterAtPos1.NextLetter = letterAtPos2;
            letterAtPos1.PreviousLetter = letterBeforePos2;
        }
        else
        {
            Letter letterAfterPos2 = letterAtPos2.NextLetter;
            letterAfterPos2.PreviousLetter = letterAtPos1;
            letterAtPos2.NextLetter = letterAtPos1;
            letterAtPos1.NextLetter = letterAfterPos2;
            letterAtPos1.PreviousLetter = letterAtPos2;
        }
    }
    return firstLetter;
}

void P1()
{
    string input = "abcdefgh";
    Letter firstLetter = LetterFromString(input);

    foreach (string instruction in inputList)
    {
        firstLetter = Operate(firstLetter, instruction, false);
    }

    Print(firstLetter);
    Console.ReadLine();
}

void P2()
{
    var reversedInstructions = new List<string>(inputList);
    reversedInstructions.Reverse();

    string input = "fbgdceah";
    Letter firstLetter = LetterFromString(input);

    foreach (string instruction in reversedInstructions)
    {
        firstLetter = Operate(firstLetter, instruction, true);
    }

    Print(firstLetter);
    Console.ReadLine();
}

P1();
P2();

class Letter
{
    public Letter NextLetter;
    public Letter PreviousLetter;
    public char C;

    public Letter(char c)
    {
        C = c;

        NextLetter = this;
        PreviousLetter = this;
    }

    public override string ToString()
    {
        return C.ToString();
    }
}
