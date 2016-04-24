using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    private static Map instance;
    
    private bool[,]   collisionMap;    // true = air; false = collision
    private Texture2D fog;
    private Sprite    fogSprite;
    private float currentTime;
    private TerrainGen terrainGenerator;

    // PROPERTIES
    public Texture2D Fog { get { return fog; } }
    public bool[,] CollisionMap { get { return collisionMap; } }
    public static Map Instance { get { return instance; } }
    public float CurrentTime { get { return currentTime; } }

    // INSPRECTOR ATTRIBUTES
    public Color fogOfWarColor;
    public int mapWidth;
    public int mapHeight;

    // TEST
    public SpriteRenderer bgTex;

    System.Threading.Thread thread;

    void Awake()
    {
        // set static reference
        instance = this;        
        
        // create the fog
        CreateFogTexture();
        fogSprite = Sprite.Create(fog, new Rect(Vector2.zero, new Vector2(fog.width, fog.height)), new Vector2(0f, 0f), 1);
        GetComponent<SpriteRenderer>().sprite = fogSprite;

        collisionMap = new bool[mapWidth, mapHeight];
        terrainGenerator = GetComponent<TerrainGen>();
        // @BINA: Hier die Funktion einfügen, die das Terrain prozedural generiert
        // benutze "map" für kollisionen; also die passierbaren felder = true
        //terrainGenerator.noise2Texture();
        terrainGenerator.createHouses(10);

        ConvertTextureToCollisionMap(bgTex.sprite.texture, 1, 0.7f);
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

    public void MakeVisible(int x, int y, Color c)
    {
        fog.SetPixel(x, y, CalcPixelColor(fog.GetPixel(x, y), c));
        fog.Apply();
    }

    public static Color CalcPixelColor(Color c1, Color c2)
    {
        return c1.a < c2.a ? c1 : c2;
    }

    private void ConvertTextureToCollisionMap(Texture2D tex, int colorIndex, float treshold)
    {
        if(tex == null || tex.width != mapWidth || tex.height != mapHeight) {
            Debug.LogError("Texture not valid!");
            return;
        }

        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                collisionMap[i, j] = !(tex.GetPixel(i,j)[colorIndex] < treshold);
            }
        }
    }
    // slowly expand the circle
    private IEnumerator ReturnFog()
    {
        //while(true)
        {
            Debug.Log("tick");
            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {
                    Color currPixelCol = fog.GetPixel(i, j);
                    if (currPixelCol.a < fogOfWarColor.a + 0.01f || currPixelCol.a > fogOfWarColor.a - 0.01f)
                    {
                        currPixelCol.r += 0.2f;
                        currPixelCol = CalcPixelColor(currPixelCol, fogOfWarColor);
                        fog.SetPixel(i, j, currPixelCol);
                    }
                }
            }
            fog.Apply();
            
            yield return new WaitForSeconds(1f);
        }
    }
}
