using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenBorder : MonoBehaviour
{
    public RectTransform panelLeft;
    public RectTransform panelRight;

    // Use this for initialization
    void Start () {
        Vector3 playerPosition = PlayerControl.Instance.transform.position;

        Vector2 leftOffset  = Camera.main.WorldToScreenPoint(PixelPerfect.Align(playerPosition + Vector3.left  * 32f));
        Vector2 rightOffset = Camera.main.WorldToScreenPoint(PixelPerfect.Align(playerPosition + Vector3.right * 32f));

        panelLeft.position  = leftOffset;
        panelRight.position = rightOffset;
	}
}
