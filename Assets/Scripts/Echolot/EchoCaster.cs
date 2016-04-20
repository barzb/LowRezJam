using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(CircleCollider2D))]
public class EchoCaster : MonoBehaviour
{
    private static int maxEchoRange = 100;

    public bool isLight = false;

    public int   echoRange;    // in pixels
    public float echoSpeed;    // in 1/x seconds
    public float echoCooldown; // in seconds

    public bool castAtStart = false;
    public bool autoCast    = false;

    // for cooldown
    private float nextActivation = 0f;

    private CircleCollider2D circleCol;
    
    void Start()
    {
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.isTrigger = true;
        if (castAtStart) { 
            CastEcho();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (autoCast) {
            CastEcho();
        }
	}

    // cast an echo
    public void CastEcho()
    {
        if (Time.time < nextActivation) return;

        nextActivation = Time.time + echoCooldown;
        
        StartCoroutine(Circle());
    }

    // slowly expand the circle
    public IEnumerator Circle()
    {
        int r = (isLight ? echoRange : 2);
        while (r <= echoRange)
        {
            int offsetX = (int)transform.position.x;
            int offsetY = (int)transform.position.y;

            DrawCircle(offsetX, offsetY, r);
            yield return new WaitForSeconds(1f/echoSpeed);
            r += 2;
        }
        if(!isLight) { 
            circleCol.radius = 1f;
        }
    }

    private void DrawCircle(int offsetX, int offsetY, int r)
    {
        Texture2D fog = Map.Instance.Fog;
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


                MarkPixelAsVisible(px, py, col, fog);
                MarkPixelAsVisible(nx, py, col, fog);
                MarkPixelAsVisible(px, ny, col, fog);
                MarkPixelAsVisible(nx, ny, col, fog);

                if((d == y+2 || r == x+2) && !isLight)
                {
                    col.a = 0.1f;
                    fog.SetPixel(px, py, col);
                    fog.SetPixel(nx, py, col);
                    fog.SetPixel(px, ny, col);
                    fog.SetPixel(nx, ny, col);
                }
            }
        }

        circleCol.radius = r * 0.9f;
        fog.Apply();
    }
    
    public void MarkPixelAsVisible(int px, int py, Color col, Texture2D tex)
    {
        tex.SetPixel(px, py, Map.CalcPixelColor(tex.GetPixel(px, py), col));
    }


}
