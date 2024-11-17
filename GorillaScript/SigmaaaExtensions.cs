using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorillaScript
{
    // nerd stuff
    public static class SigmaaaExtensions
    {
        public static int CountOfValid<T>(this List<T> list)
        {
            return list.Count(item => item != null);
        }
        public static bool IsHigherThanOrEqualTo(this int currentValue, int comparisonValue)
        {
            return currentValue >= comparisonValue;
        }
    }
}
