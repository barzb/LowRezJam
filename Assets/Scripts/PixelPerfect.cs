using UnityEngine;
using System.Collections;

public class PixelPerfect : MonoBehaviour
{
    private static int GridSize = 64;
    

    // set new position
	public void MoveTo(Vector2 target)
    {
        transform.position = Align(target);
    }

    // set new position
    public void MoveTo(float x, float y)
    {
        transform.position = Align(new Vector2(x, y));
    }

    // move towards a target with specified speed
    public void MoveTowards(Vector2 direction, float speed)
    {
        float dt = Time.deltaTime;

        // TODO  (Auf collision achten! RigidBody?)
    }



    // Align coordinates to pixel perfect coordinates (depending on grid size)

    public static Vector2 Align(Vector2 pos)
    {
        pos.x = Align(pos.x);
        pos.y = Align(pos.y);

        return pos;
    }

    public static float Align(float source)
    {
        return Mathf.Floor(source / GridSize) * GridSize;
    }
}
