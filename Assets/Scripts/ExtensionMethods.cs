using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2Int Settt(this Vector2Int vector2Int, int x, int y)
    {
        vector2Int.x = x;
        vector2Int.y = y;
        return vector2Int;
    }
}
