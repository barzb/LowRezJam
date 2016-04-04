using UnityEngine;
using System.Collections;

public class PixelCollider : MonoBehaviour
{
    private static PixelCollider colliders;
    private bool[,] map;
    
	// Use this for initialization
	void Start () {
        map = Map.Instance.CollisionMap;
	}

    public void Move(float distanceX, float distanceY)
    {
        Move(new Vector3(distanceX, distanceY, 0f));
    }

    public void Move(Vector3 distance)
    {
        bool collision = false;
        bool targetReached = false;

        Vector3 targetPosition = transform.position + distance;
        Vector3 lastPosition = transform.position;
        Vector3 lastAlignedPosition = transform.position;

        // MOVE STEP BY STEP TOWARDS TARGET
        while(!collision && !targetReached)
        {
            Vector3 nextPos = Vector3.MoveTowards(lastPosition, targetPosition, 1);
            Vector3 nextAlignedPos = PixelPerfect.Align(nextPos);

            // CHECK IF NEXT POS COLLIDES WITH WALLS
            if(Collides(nextAlignedPos) || nextAlignedPos == lastAlignedPosition)
            {
                Vector3 justXMove = new Vector3(nextAlignedPos.x, lastAlignedPosition.y, 0f);
                if (Collides(justXMove) || justXMove == lastAlignedPosition)
                {
                    Vector3 justYMove = new Vector3(lastAlignedPosition.x, nextAlignedPos.y, 0f);
                    if(Collides(justYMove) || justYMove == lastAlignedPosition)
                    {
                        // NO MOVEMENT -> NO PATH AVAILABLE
                        collision = true;
                        nextPos = lastPosition;
                        nextAlignedPos = lastAlignedPosition;
                    } else {
                        // MOVE JUST THE Y-COMPONENT
                        nextPos = justYMove;
                        nextAlignedPos = justYMove;
                    }
                } else {
                    // MOVE JUST THE X-COMPONENT
                    nextPos = justXMove;
                    nextAlignedPos = justXMove;
                }
            } 
            // TARGET REACHED
            else if(nextAlignedPos == targetPosition) {
                targetReached = true;
                nextPos = PixelPerfect.Align(targetPosition);
                nextAlignedPos = nextPos;
            }

            // UPDATE POSITION
            lastPosition = nextPos;
            lastAlignedPosition = nextAlignedPos;
            transform.position = nextAlignedPos;
            
        }
    }

    private bool Collides(Vector3 checkPos)
    {
        int x = (int)checkPos.x;
        int y = (int)checkPos.y;

        // POS OUT OF X BOUNDS?
        if (x < 0) x = Map.Instance.mapWidth-1;
        if (x >= Map.Instance.mapWidth - 1) x = 0;

        // POS OUT OF Y BOUNDS?
        if (y < 0) y = Map.Instance.mapHeight-1;
        if (y >= Map.Instance.mapHeight - 1) y = 0;

        return !map[x, y];
    }
}
