using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(PixelCollider))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class Owl : IBehaviour
{
    private PixelCollider pixCollider;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D bodyCollider;

    private Transform player;
    private Vector3[] lastPositions;
    private int seekIndex;

    private Vector3 restingPosition;
    private Vector2 headingVector;
    private bool seek;
    private bool aware;
    private float awareTime;

    public int sightRadius = 20;
    public int seekRadius = 60;
    public float speed = 0.25f;
    public float maxAwareTime = 3f;

    // Use this for initialization
    void Start ()
    {
        pixCollider = GetComponent<PixelCollider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bodyCollider = GetComponent<CircleCollider2D>();

        player = PlayerControl.Instance.transform;
        lastPositions = new Vector3[5];
        seekIndex = 0;

        seek = false;
        aware = false;
        awareTime = 0f;

        for(int i = 0; i < 5; i++) {
            lastPositions[i] = player.position;
        }
        InvokeRepeating("UpdatePlayerPositions", 0f, 0.5f);

        restingPosition = transform.position;
	}
	
    protected override void Execute()
    {
        aware = false;

        if(PlayerInSight()) {
            SeekPlayer();
        } else if(PlayerInSight(true)) {
            BeAware();
        } else {
            Rest();
        }

        lastPositions[0] = player.position;
        
        pixCollider.UpdateMovement(headingVector.x, headingVector.y, speed);
        Vector2 pos = transform.position;
        Debug.DrawLine(pos, pos + headingVector * 10f, Color.cyan);
    }

    private bool PlayerInSight(bool checkSeekRadius = false)
    {
        float checkRadius = checkSeekRadius ? ((seekRadius + sightRadius) / 2) : sightRadius;
        checkRadius = seek ? seekRadius : checkRadius;
        
        return (Mathf.Abs(Vector3.Distance(transform.position, player.position)) < checkRadius);
    }

    private void SeekPlayer()
    {
        seek = true;
        if (pixCollider.RayCastHits(transform.position, lastPositions[seekIndex]))
        {
            seekIndex = (seekIndex + 1) % 5;
            Debug.DrawLine(lastPositions[seekIndex], transform.position, Color.red);
        } else
        {
            Debug.DrawLine(lastPositions[seekIndex], transform.position, Color.blue);
        }
        headingVector = -Vector3.Normalize(transform.position - lastPositions[seekIndex]);
    }

    
    private void BeAware()
    {
        aware = true;
        awareTime += Time.deltaTime;
        if(awareTime > maxAwareTime)
        {
            seek = true;
        }
    }

    private void Rest()
    {
        aware = false;
        seek = false;
        awareTime = 0f;
        headingVector.x = 0f;
        headingVector.y = 0f;
    }

    private void UpdatePlayerPositions()
    {
        for(int i = 4; i > 0; i--)
        {
            lastPositions[i] = lastPositions[i - 1];
        }
        seekIndex--;
        if (seekIndex < 0)
            seekIndex = 0;

        lastPositions[0] = player.position;
    }

    void OnDrawGizmos()
    {
        if (Time.time < 1f)
            return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < 5; i++)
            Gizmos.DrawWireSphere(lastPositions[i], 3f);

        Gizmos.color = Color.yellow;
        if (aware)
            Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
