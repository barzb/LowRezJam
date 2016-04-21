using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent (typeof(PixelCollider))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(CircleCollider2D))]
public class Moth : IBehaviour
{
    public int sightRadius = 10;
    public float speed = 0.3f;

    private Transform player;
    private PixelCollider pixCollider;
    
    private SpriteRenderer sprite;

    private float movementDuration = 0f;
    private Vector2 headingVector;

    private static GameObject[] lights;
        
    void Start()
    {
        player = PlayerControl.Instance.transform;
        pixCollider = GetComponent<PixelCollider>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        
        lights = GameObject.FindGameObjectsWithTag("LIGHT");

        InvokeRepeating("FindNearestLight", 0f, 5f);
    }

    // Executed in Update()
    protected override void Execute()
    {
        Action start = Selector(PlayerInSight, Flee, SeekLight);
        start();

        if (UnityEngine.Random.Range(0f, 100f) > 99f) {
            MoveRandom(UnityEngine.Random.Range(0.01f, 0.2f));
        }
        movementDuration -= Time.deltaTime;
        pixCollider.UpdateMovement(headingVector.x, headingVector.y, speed);
        Vector2 pos = transform.position;
        Debug.DrawLine(pos, pos + headingVector * 10f, Color.cyan);
    }
    
    private Vector3 nearestLightPos; 
    private void SeekLight()
    { 
        if(movementDuration <= 0)
        {
            headingVector = Vector3.Normalize(nearestLightPos - transform.position);
            movementDuration = UnityEngine.Random.Range(0.1f, 0.6f);
        }
        
        Debug.DrawLine(transform.position, nearestLightPos, Color.green);

        if (pixCollider.isColliding) {
            movementDuration = 0f;
            MoveRandom(0.3f);
        }
    }

    private void Flee()
    {        
        if (pixCollider.isColliding) {
            movementDuration = 0f;
            MoveRandom(0.1f);
        } else if (movementDuration <= 0) { 
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
    }

    private bool PlayerInSight()
    {
        return (Mathf.Abs(Vector3.Distance(transform.position, player.position)) < sightRadius);
    }

    private void FindNearestLight()
    {
        GameObject nearestLight = lights[0];
        for (int i = 1; i < lights.Length; i++)
        {
            float d1 = Mathf.Abs(Vector2.Distance(transform.position, nearestLight.transform.position));
            float d2 = Mathf.Abs(Vector2.Distance(transform.position, lights[i].transform.position));
            if (d1 > d2)
            {
                nearestLight = lights[i];
            }
        }

        nearestLightPos = nearestLight.transform.position;
    }

    private void Hide()
    {
        sprite.enabled = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        sprite.enabled = true;        
    }
    void OnTriggerExit2D(Collider2D other)
    {
         Invoke("Hide", 1f);
    }
}
