using System;
using System.Collections.Generic;
using System.Text;

namespace Roommates
{
    class NumberUtils
    {
        public static bool isInt(string x)
        {
            int value;
            if (int.TryParse(x, out value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isBetween(int numToTest, int min, int max = 2147483647)
        {
            if ((numToTest >= min) && (numToTest <= max))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
