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

    private float randomMovementDuration = 0f;
    private Vector2 headingVector;

    // DEBUG
    Text ui;

    void Start()
    {
        player = PlayerControl.Instance.transform;
        pixCollider = GetComponent<PixelCollider>();

        ui = GameObject.Find("MOTH").GetComponent<Text>();
    }

    // Executed in Update()
    protected override void Execute()
    {
        Action start = Selector(PlayerInSight, Flee, SeekLight);
        start();

        randomMovementDuration -= Time.deltaTime;
        pixCollider.UpdateMovement(headingVector.x, headingVector.y, speed);
        ui.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void SeekLight()
    {
        ui.text = "SEEK LIGHT";

        MoveRandom(0.5f);
    }

    private void Flee()
    {
        ui.text = "FLEE";
        
        if (pixCollider.isColliding) {
            MoveRandom(0.2f);
        } else {
            headingVector = Vector3.Normalize(transform.position - player.position);
        }
    }

    private void MoveRandom(float duration)
    {
        if (randomMovementDuration <= 0)
        {
            randomMovementDuration = duration;
            float randX = UnityEngine.Random.Range(-100f, +100f);
            float randY = UnityEngine.Random.Range(-100f, +100f);

            headingVector = new Vector2(randX, randY).normalized;
        }

        Vector2 pp = transform.position;
        Debug.DrawLine(transform.position, pp + headingVector * 10f, Color.red);

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
