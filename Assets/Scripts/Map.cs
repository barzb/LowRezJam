using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    private static Map instance;
    
    private bool[,]   map;    // true = air; false = collision
    private Texture2D fog;
    private Sprite    fogSprite;
	
    // PROPERTIES
    public Texture2D Fog { get { return fog; } }
    public static Map Instance { get { return instance; } }

    // INSPRECTOR ATTRIBUTES
    public Color fogOfWarColor;
    public int mapWidth;
    public int mapHeight;

    void Start()
    {
        // set static reference
        instance = this;        
        
        // create the fog
        CreateFogTexture();
        fogSprite = Sprite.Create(fog, new Rect(Vector2.zero, new Vector2(fog.width, fog.height)), new Vector2(0f, 0f), 1);
        GetComponent<SpriteRenderer>().sprite = fogSprite;

        map = new bool[mapWidth, mapHeight];
        // @BINA: Hier die Funktion einfügen, die das Terrain prozedural generiert
        // benutze "map" für kollisionen; also die passierbaren felder = true
    }

    // create the fog texture which masks the terrain
    // can be made visible with EchoCaster
    public void CreateFogTexture()
    {
        fog = new Texture2D(mapWidth, mapHeight);

        // No Anti-Aliasing! (for PixelPerfect display)
        fog.filterMode = FilterMode.Point;

        // set every pixel black
        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                fog.SetPixel(i, j, fogOfWarColor);
            }
        }
        // apply changes to texture
        fog.Apply();
    }


    void Update()
    {

    }
}
