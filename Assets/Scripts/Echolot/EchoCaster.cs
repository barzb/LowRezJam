using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EchoCaster : MonoBehaviour
{
    public int   echoRange;    // in pixels
    public float echoSpeed;    // in 1/x seconds
    public float echoCooldown; // in seconds

    public bool castAtStart = false;
    public bool autoCast    = false;

    // for cooldown
    private float nextActivation = 0f;
    
    void Start()
    {
        if (castAtStart) { 
            CastEcho();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (autoCast || Input.GetButton("Action1"))
        {
            CastEcho();
        }
	}

    // cast an echo
    private void CastEcho()
    {
        if (Time.time < nextActivation) return;

        nextActivation = Time.time + echoCooldown;
        
        StartCoroutine(Circle());
    }

    // slowly expand the circle
    public IEnumerator Circle()
    {
        for (int r = 2; r < echoRange; r+=2)
        {
            int offsetX = (int)transform.position.x;
            int offsetY = (int)transform.position.y;
            int x, y, px, nx, py, ny, d;

            for (x = 0; x <= r; x++)
            {
                d = Mathf.RoundToInt(Mathf.Sqrt(r * r - x * x));

                for (y = 0; y <= d; y++)
                {
                    Color col = Map.Instance.fogOfWarColor;

                    col.a = Mathf.Clamp01(Mathf.Clamp01((Mathf.Sqrt((x * x) + (y * y))) / r) * 2f - 1f);
                   
                    px = offsetX + x; // +x
                    nx = offsetX - x; // -x
                    py = offsetY + y; // +y
                    ny = offsetY - y; // -y

                    MarkPixelAsVisible(px, py, col);
                    MarkPixelAsVisible(nx, py, col);
                    MarkPixelAsVisible(px, ny, col);
                    MarkPixelAsVisible(nx, ny, col);
                }
            }

            Texture2D fog = Map.Instance.Fog;
            fog.Apply();
            yield return new WaitForSeconds(1f/echoSpeed);
        }
    }
    
    public void MarkPixelAsVisible(int px, int py, Color col)
    {
        Texture2D fog = Map.Instance.Fog;
        fog.SetPixel(px, py, Map.CalcPixelColor(fog.GetPixel(px, py), col));
    }
}
