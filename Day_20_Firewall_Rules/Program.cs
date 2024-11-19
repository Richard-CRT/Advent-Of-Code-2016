using AdventOfCodeUtilities;
using System.Text.RegularExpressions;

List<string> inputList = AoC.GetInputLines();
uint max = 4294967295;

List<(uint, uint)> allowedRanges = new() { (0, max) };

foreach (string s in inputList)
{
    var split = s.Split('-');
    uint lower = uint.Parse(split[0]);
    uint upper = uint.Parse(split[1]);
    //foreach ((int, int) range in allowedRanges)
    for (int i = 0; i < allowedRanges.Count;)
    {
        (uint allowedRangeLower, uint allowedRangeUpper) = allowedRanges[i];
        if (allowedRangeLower >= lower && allowedRangeUpper <= upper)
        {
            // disallowed contains allowed, just delete it
            allowedRanges.RemoveAt(i);
        }
        else if (lower > allowedRangeLower && upper < allowedRangeUpper)
        {
            // disallowed is entirely within allowed, split and break
            (uint, uint) replacementRange1 = (allowedRangeLower, lower - 1);
            (uint, uint) replacementRange2 = (upper + 1, allowedRangeUpper);
            allowedRanges[i] = replacementRange1;
            allowedRanges.Insert(i + 1, replacementRange2);
            i += 2;
            break;
        }
        else if (lower <= allowedRangeLower && upper >= allowedRangeLower)
        {
            (uint, uint) replacementRange = (upper + 1, allowedRangeUpper);
            allowedRanges[i] = replacementRange;
            i++;
        }
        else if (lower <= allowedRangeUpper && upper >= allowedRangeUpper)
        {
            (uint, uint) replacementRange = (allowedRangeLower, lower - 1);
            allowedRanges[i] = replacementRange;
            i++;
        }
        else
        {
            i++;
        }
    }
}

void P1()
{
    uint lowest = uint.MaxValue;
    foreach ((uint lower, uint upper) in allowedRanges)
    {
        lowest = Math.Min(lowest, lower);
    }

    Console.WriteLine(lowest);
    Console.ReadLine();
}

void P2()
{
    uint count = 0;
    foreach ((uint lower, uint upper) in allowedRanges)
    {
        count += (upper - lower) + 1;
    }

    Console.WriteLine(count);
    Console.ReadLine();
}

P1();
P2();
