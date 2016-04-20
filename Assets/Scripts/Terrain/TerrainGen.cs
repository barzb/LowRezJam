using UnityEngine;
using System.Collections;

public class TerrainGen : MonoBehaviour {

    private Sprite sprite;
    private Texture2D tex;
    public float scale = 1.0f;
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
                if (noise > 0.3f) pixelColor = Color.red;
                if (noise > 0.6) pixelColor = Color.blue;

                tex.SetPixel(i, j, pixelColor);
            }
        }
        tex.Apply(); // änderungen werden angezeigt

        sprite = Sprite.Create(tex, new Rect(0, 0, Map.Instance.mapWidth, Map.Instance.mapHeight), new Vector2(0f, 0f), 1);
        GetComponent<SpriteRenderer>().sprite = sprite;

    }

    // Update is called once per frame
    void Update () {
	
	}
}
