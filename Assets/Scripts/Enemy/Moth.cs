using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent (typeof(PixelCollider))]
public class Moth : IBehaviour
{
    public int sightRadius = 10;
    public float speed = 0.3f;

    private Transform player;
    private PixelCollider pixCollider;

    private float movementDuration = 0f;
    private Vector2 headingVector;

    private static GameObject[] lights;

    // DEBUG
    Text ui;

    void Start()
    {
        player = PlayerControl.Instance.transform;
        pixCollider = GetComponent<PixelCollider>();

        ui = GameObject.Find("MOTH").GetComponent<Text>();

        lights = GameObject.FindGameObjectsWithTag("LIGHT");
    }

    // Executed in Update()
    protected override void Execute()
    {
        Action start = Selector(PlayerInSight, Flee, SeekLight);
        start();

        movementDuration -= Time.deltaTime;
        pixCollider.UpdateMovement(headingVector.x, headingVector.y, speed);
        ui.transform.position = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 pos = transform.position;
        Debug.DrawLine(pos, pos + headingVector * 10f, Color.cyan);
    }
    
    private Vector3 nearestLightPos; // only 4 debug
    private void SeekLight()
    {
        ui.text = "SEEK LIGHT";

        if(movementDuration <= 0 && lights.Length > 0)
        {
            GameObject nearestLight = lights[0];
            for(int i = 1; i < lights.Length; i++)
            {
                float d1 = Vector2.Distance(transform.position, nearestLight.transform.position);
                float d2 = Vector2.Distance(transform.position, lights[i].transform.position);
                if (d1 > d2) {
                    nearestLight = lights[i];
                }
            }
            headingVector = Vector3.Normalize(nearestLight.transform.position - transform.position);
            movementDuration = 2f;

            nearestLightPos = nearestLight.transform.position;
        }
        
        Debug.DrawLine(transform.position, nearestLightPos, Color.green);

        if (pixCollider.isColliding) {
            movementDuration = 0f;
            MoveRandom(0.5f);
        }
    }

    private void Flee()
    {
        ui.text = "FLEE";
        
        if (pixCollider.isColliding) {
            MoveRandom(0.2f);
        } else {
            headingVector = Vector3.Normalize(transform.position - player.position);
        }

        Debug.DrawLine(player.position, transform.position, Color.red);
    }

    private void MoveRandom(float duration)
    {
        if (movementDuration <= 0)
        {
            movementDuration = duration;
            float randX = UnityEngine.Random.Range(-100f, +100f);
            float randY = UnityEngine.Random.Range(-100f, +100f);

            headingVector = new Vector2(randX, randY).normalized;
        }
        
        ui.text = "MOVE RANDOM: " + (int)headingVector.x + ", " + (int)headingVector.y;
    }

    private bool PlayerInSight()
    {
        Vector3 playerPosition = player.position;

        return (Vector3.Distance(transform.position, playerPosition) < sightRadius);
    }

    private Vector3 FindNearestLight()
    {
        return Vector3.one;
    }        
}
