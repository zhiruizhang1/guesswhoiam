using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExtensionClass
{
    private static Random rand = new Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int count = list.Count;
        while (count > 1)
        {
            count--;
            int randIndex = rand.Next(count + 1);
            T value = list[randIndex];
            list[randIndex] = list[count];
            list[count] = value;
        }
    }
}

