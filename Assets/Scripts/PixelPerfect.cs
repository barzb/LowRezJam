using UnityEngine;
using System.Collections;

public static class PixelPerfect
{
    public static int GridSize = 64;

    // Align coordinates to pixel perfect coordinates (depending on grid size)
    // OVERLOADED METHODS
    public static Vector2 Align(Vector2 pos)
    {
        pos.x = Align(pos.x);
        pos.y = Align(pos.y);

        return pos;
    }
    public static Vector2 Align(float x, float y)
    {
        x = Align(x);
        y = Align(y);

        return new Vector2(x, y);
    }
    public static Vector3 Align(Vector3 pos)
    {
        return Align(pos.x, pos.y);
    }
    public static float Align(float source)
    {
        return Mathf.Round(source);
    }
}
