using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    private static Grid instance;

    private int[,]      map;
    private Texture2D   mask;
    private Sprite      sprite;
	
    public Texture2D Mask { get { return mask; } }
    public static Grid Instance { get { return instance; } }

    public void CreateGrid(int width, int height)
    {
        mask = new Texture2D(width, height);
        mask.filterMode = FilterMode.Point;
        Color test = new Color(0, 0, 0, 0.8f);
        for(int i = 0; i  < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                mask.SetPixel(i, j, test);
            }
        }
        mask.Apply();
    }

    void Start()
    {
        if (instance != null) {
            instance = this;
        }

        int gridSize = PixelPerfect.GridSize;
        CreateGrid(gridSize, gridSize);

        sprite = Sprite.Create(mask, new Rect(Vector2.zero, new Vector2(mask.width, mask.height)), new Vector2(0.5f, 0.5f), 1);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Update()
    {
        if (mask == null) return;

        // TEST
        if(Input.GetMouseButtonUp(0))
        {
            Vector2 clickPos = PixelPerfect.Align(30.34f, 29.98f);
            Debug.Log(clickPos.x + ", " + clickPos.y);

            mask.SetPixel((int)clickPos.x, (int)clickPos.y, Color.red);
            mask.Apply();
        }
    }
}
