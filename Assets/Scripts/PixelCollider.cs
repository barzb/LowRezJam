using UnityEngine;
using System.Collections;

public class PixelCollider : MonoBehaviour
{
    private static float FPS = 1f / 30f;

    private bool[,] map;
    private float[] distancePassed;
    private float timePassed;

    public bool isColliding { get; set; }

    // Use this for initialization
    void Start () {
        map = Map.Instance.CollisionMap;
        distancePassed = new float[2];
        timePassed = 0f;
    }

    // since Update() has no reliable framerate, this method is called 30 times a second
    // we could just move the player one unit per frame in Update(), but that would result in
    // an unstable speed; if we scale the unity moved per frame with deltaTime, the resulting
    // number would be float instead of pixelPerfect (int) 
    // --> this method "collects" all movement made by Update() and rounds the distance to a
    // pixelPerfect number!
    public void UpdateMovement(float xSpeed, float ySpeed, float speed)
    {
        // scale with deltaTime; add to distancePassed since last movement
        distancePassed[0] += xSpeed * Time.deltaTime * 100f;
        distancePassed[1] += ySpeed * Time.deltaTime * 100f;

        // reset if not moving
        if (xSpeed == 0f) distancePassed[0] = 0;
        if (ySpeed == 0f) distancePassed[1] = 0;

        // increment timePassed
        timePassed += Time.deltaTime;

        // called 30 times a second
        if (timePassed > FPS)
        {
            // scale with speed and round to pixelPerfect numbers
            float distanceX = PixelPerfect.Align(distancePassed[0] * speed);
            float distanceY = PixelPerfect.Align(distancePassed[1] * speed);

            // reset if movement complete
            if (distanceX != 0) distancePassed[0] = 0;
            if (distanceY != 0) distancePassed[1] = 0;

            // update position
            //transform.position += new Vector3(distanceX, distanceY, 0f);
            isColliding = Move(distanceX, distanceY);
            timePassed -= FPS;
        }

        CheckPosition();
    }

    void CheckPosition()
    {
        Vector3 p = transform.position;

        if (p.x < 0f) p.x = Map.Instance.mapWidth - 1f;
        else if (p.x >= Map.Instance.mapWidth) p.x = 0f;

        if (p.y < 0f) p.y = Map.Instance.mapHeight - 1f;
        else if (p.y >= Map.Instance.mapHeight) p.y = 0f;

        transform.position = p;
    }

    public bool Move(float distanceX, float distanceY)
    {
        return Move(new Vector3(distanceX, distanceY, 0f));
    }

    public bool Move(Vector3 distance)
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

        return collision;
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
