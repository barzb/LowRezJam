using UnityEngine;
using System.Collections;

// METHODS FOR LOW RESOLUTION STUFF
public static class LOWREZ
{
    public static float GridSize = 64;

    public static Vector2 Align(Vector2 src)
    {
        src.x = Mathf.Floor(src.x / 64) * 64;
        src.y = Mathf.Floor(src.y / 64) * 64;

        return src;
    }
	
}
