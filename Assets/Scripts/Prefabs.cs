using UnityEngine;
using System.Collections;

public class Prefabs : MonoBehaviour
{
    public GameObject _moth;
    public static GameObject Moth { get; set; }
    
    public GameObject _light;
    public static GameObject Light { get; set; }


	// Use this for initialization
	void Start () {
        Moth = _moth;
        Light = _light;
	}
}
