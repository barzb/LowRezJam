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
        
        StartCoroutine(Circle(Map.Instance.Fog, Map.Instance.visibleColor));
    }

    // slowly expand the circle
    public IEnumerator Circle(Texture2D tex, Color col)
    {
        for (int r = 0; r < echoRange; r++)
        {
            int offsetX = (int)transform.position.x;
            int offsetY = (int)transform.position.y;
            int x, y, px, nx, py, ny, d;

            for (x = 0; x <= r; x++)
            {
                d = Mathf.RoundToInt(Mathf.Sqrt(r * r - x * x));
                for (y = 0; y <= d; y++)
                {
                    px = offsetX + x; // +x
                    nx = offsetX - x; // -x
                    py = offsetY + y; // +y
                    ny = offsetY - y; // -y

                    // set a pixel in every quater of the circle
                    tex.SetPixel(px, py, col);
                    tex.SetPixel(nx, py, col);
                    tex.SetPixel(px, ny, col);
                    tex.SetPixel(nx, ny, col);
                }
            }
            tex.Apply();
            yield return new WaitForSeconds(1f/echoSpeed);
        }
    }
}
