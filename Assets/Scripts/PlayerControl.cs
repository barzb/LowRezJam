using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(EchoCaster))]
[RequireComponent (typeof(PixelCollider))]
public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance;
    private PixelCollider pixCollider;
    private EchoCaster echo;

    private static float FPS = 1f/30f;
    private float speed = 0.3f;

    private float[] distancePassed;
    private float timePassed;

    private Text debugText;

    public static PlayerControl Instance { get { return instance; } }
    public EchoCaster Echolot { get { return echo; } }

    public Animator anim;

	// Use this for initialization
	void Awake ()
    {
        instance = this;

        debugText = GameObject.FindGameObjectWithTag("DEBUG_UI").GetComponent<Text>();

        distancePassed = new float[2];
        timePassed = 0f;
        transform.position = PixelPerfect.Align(transform.position);

        pixCollider = GetComponent<PixelCollider>();
        echo = GetComponent<EchoCaster>();

        if(anim == null) {
            Debug.Log("Bat Animator not assigned on Player");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // TELEPORT PLAYER IF HE IS OUT OF BOUNDS
        CheckPlayerPosition();

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

        if(anim != null)
        {
            if (xSpeed > 0)
                anim.SetTrigger("RIGHT");
            else if (xSpeed < 0)
                anim.SetTrigger("LEFT");
            else if (ySpeed < 0)
                anim.SetTrigger("UP");
            else if(ySpeed > 0)
                anim.SetTrigger("DOWN");

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
        //  XXX REMOVE IN RELEASE XXX
        debugText.text = "PRESS 'E' TO USE ECHOLOT \nDistance: " + distancePassed[0] + ", " + distancePassed[1];

        // scale with speed and round to pixelPerfect numbers
        float distanceX = PixelPerfect.Align(distancePassed[0] * speed);
        float distanceY = PixelPerfect.Align(distancePassed[1] * speed);
        
        // reset if movement complete
        if(distanceX != 0) distancePassed[0] = 0;
        if(distanceY != 0) distancePassed[1] = 0;

        // update position
        //transform.position += new Vector3(distanceX, distanceY, 0f);
        pixCollider.Move(distanceX, distanceY);
    }

    void CheckPlayerPosition()
    {
        Vector3 p = transform.position;
        
        if (p.x < 0f) p.x = Map.Instance.mapWidth - 1f;
        else if (p.x >= Map.Instance.mapWidth) p.x = 0f;

        if (p.y < 0f) p.y = Map.Instance.mapHeight - 1f;
        else if (p.y >= Map.Instance.mapHeight) p.y = 0f;

        transform.position = p;
    }
}
