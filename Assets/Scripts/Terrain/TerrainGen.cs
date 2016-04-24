using UnityEngine;
using System.Collections;

public class TerrainGen : MonoBehaviour {

    private Sprite sprite;
    private Texture2D tex;
    public float scale = 1.0f;
    public GameObject housePrefab;
	// Use this for initialization
	void Start () {
        
        
    }

    public void noise2Texture()
    {
        tex = new Texture2D(Map.Instance.mapWidth, Map.Instance.mapHeight);
        for (int i = 0; i < tex.width; i++)
        {
            for(int j = 0; j < tex.height; j++)
            {
                float x = i / scale;
                float y = j / scale;
                float noise = Mathf.PerlinNoise(x,y);
                
                Color pixelColor = Color.white;
                if (noise > 0.1f) pixelColor = Color.red;
                if (noise > 0.6) pixelColor = Color.blue;

                tex.SetPixel(i, j, pixelColor);
            }
        }
        tex.Apply(); // änderungen werden angezeigt

        sprite = Sprite.Create(tex, new Rect(0, 0, Map.Instance.mapWidth, Map.Instance.mapHeight), new Vector2(0f, 0f), 1);
        sprite.texture.filterMode = FilterMode.Point;
        GetComponent<SpriteRenderer>().sprite = sprite;

    }

    GameObject[] houses;
    public void createHouses(int housesToCreate)
    {
        houses = new GameObject[housesToCreate];
       // int[] xCoordArray = new int[housesToCreate];
        //int[] yCoordArray = new int[housesToCreate]; ;

        for (int i = 0; i < housesToCreate; i++)
        {
            
            houses[i] = Instantiate(housePrefab, GenerateRandomPosition(), Quaternion.identity)as GameObject;

            houses[i].name = "House" + i;

    
        }


    }
    private static Vector2 CheckPosition(Vector2 p, Vector2 box)
    {
        if (p.x < 0f) p.x = Map.Instance.mapWidth - 1f;
        else if (p.x >= Map.Instance.mapWidth) p.x = 0f;

        if (p.y < 0f) p.y = Map.Instance.mapHeight - 1f;
        else if (p.y >= Map.Instance.mapHeight) p.y = 0f;
        return p;
    }

    public static Vector2 GenerateRandomPosition(Vector2 objSize)
    {
        int xCoord = Random.Range(0, Map.Instance.mapWidth);
        int yCoord = Random.Range(0, Map.Instance.mapHeight);
        return CheckPosition(PixelPerfect.Align(xCoord, yCoord), objSize);
    }

    /*  private bool CheckHouseCollision(int checkIndex)
      {
          BoxCollider2D mine = houses[checkIndex].GetComponent<BoxCollider2D>();
          Vector2 myPos = ...;
          foreach(GameObject house in houses)
          {
              if (houses[checkIndex] == house) continue;
              BoxCollider2D yours = house.GetComponent<BoxCollider2D>();
              Vector2 yourPos = ...;
              if()
          }
      }
  */

    // Update is called once per frame
    void Update () {
       // if(Input.GetKeyUp(KeyCode.Space))
            CollisionDetector.CheckCollisions = true;
	}
}
