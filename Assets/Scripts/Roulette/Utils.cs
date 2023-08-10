using System;
using System.Collections.Generic;

namespace Roulette
{
    public static class Utils
    {
        public static List<T> ShuffleList<T>(List<T> list)
        {
            Random rng = new();
            int n = list.Count;

            while (n > 1)
            {
                n -= 1;

                int k = rng.Next(n + 1);

                (list[n], list[k]) = (list[k], list[n]);
            }

            return list;
        }
    }
}
