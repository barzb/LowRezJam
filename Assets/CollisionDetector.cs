using UnityEngine;
using System.Collections;

public class CollisionDetector : MonoBehaviour {

    public static bool CheckCollisions = true;
    private int counter = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (this.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.NameToLayer("Terrain")))
        {
            if (counter < 6)
            {
                int newX = (int)(this.transform.position.x + 50) % Map.Instance.mapWidth - 72;
                int newY = (int)(this.transform.position.y + 50) % Map.Instance.mapHeight - 72;

                this.transform.position = new Vector3(newX, newY);
                counter++;
                Debug.Log("House " + this.gameObject.name + " Collides for " + counter + " time(s). Move house to " + newX + " , " + newY);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        */
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!CheckCollisions) return;

        if (counter < 20)
        {
            this.transform.position = TerrainGen.GenerateRandomPosition();
            counter++;
            Debug.Log("House " + this.gameObject.name + " Collides for " + counter + " time(s).");
        }
        else
        {
            Destroy(this.gameObject);
        }

        CheckCollisions = false;
    }


}
