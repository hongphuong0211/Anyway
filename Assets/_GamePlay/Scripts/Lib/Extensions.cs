using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static string ToString<T>(this IList<T> list)
    {
        return string.Join(", ", list);
    }
}
