using UnityEngine;
using System.Collections;

public class EchoCaster : MonoBehaviour
{
    public int   echoRange;    // in pixels
    public float echoSpeed;    // in 1/x seconds
    public float echoCooldown; // in seconds

    // for cooldown
    private float nextActivation = 0f;
    
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetButton("Action1"))
        {
            CastEcho();
        }
	}

    // cast an echo
    private void CastEcho()
    {
        if (Time.time < nextActivation) return;

        nextActivation = Time.time + echoCooldown;
        
        StartCoroutine(Circle(Map.Instance.Fog));
    }

    // slowly expand the circle
    public IEnumerator Circle(Texture2D tex)
    {
        for (int r = 2; r < echoRange; r++)
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

                    col.a = Mathf.Clamp01((Mathf.Sqrt((x * x) + (y * y))) / r) * 2f - 1f;

                    px = offsetX + x; // +x
                    nx = offsetX - x; // -x
                    py = offsetY + y; // +y
                    ny = offsetY - y; // -y

                    // set a pixel in every quater of the circle
                    tex.SetPixel(px, py, CalcPixelColor(tex.GetPixel(px, py), col));
                    tex.SetPixel(nx, py, CalcPixelColor(tex.GetPixel(nx, py), col));
                    tex.SetPixel(px, ny, CalcPixelColor(tex.GetPixel(px, ny), col));
                    tex.SetPixel(nx, ny, CalcPixelColor(tex.GetPixel(nx, ny), col));
                }
            }
            tex.Apply();
            yield return new WaitForSeconds(1f/echoSpeed);
        }
    }

    private Color CalcPixelColor(Color c1, Color c2)
    {
        return c1.a < c2.a ? c1 : c2;
    }
}
