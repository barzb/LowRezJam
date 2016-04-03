using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    private static float FPS = 1f/30f;
    private float speed = 0.3f;

    private float[] distancePassed;
    private float timePassed;

    private Text debugText;

	// Use this for initialization
	void Start () {
        debugText = GameObject.FindGameObjectWithTag("DEBUG_UI").GetComponent<Text>();

        distancePassed = new float[2];
        timePassed = 0f;
        transform.position = PixelPerfect.Align(transform.position);
	}
	
	// Update is called once per frame
	void Update ()
    {

        // ---- PIXEL PERFECT MOVEMENT ----
        // get axes
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");
        
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
            Move();
            timePassed -= FPS;
        }
    }

    // since Update() has no reliable framerate, this method is called 30 times a second
    // we could just move the player one unit per frame in Update(), but that would result in
    // an unstable speed; if we scale the unity moved per frame with deltaTime, the resulting
    // number would be float instead of pixelPerfect (int) 
    // --> this method "collects" all movement made by Update() and rounds the distance to a
    // pixelPerfect number!
    void Move()
    {
        debugText.text = "Distance: " + distancePassed[0] + ", " + distancePassed[1];

        // scale with speed and round to pixelPerfect numbers
        float distanceX = PixelPerfect.Align(distancePassed[0] * speed);
        float distanceY = PixelPerfect.Align(distancePassed[1] * speed);
        
        debugText.text += "\n" + distanceX + ", " + distanceY;

        // reset if movement complete
        if(distanceX != 0) distancePassed[0] = 0;
        if(distanceY != 0) distancePassed[1] = 0;

        // update position
        transform.position += new Vector3(distanceX, distanceY, 0f);
    }
}
