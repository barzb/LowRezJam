using UnityEngine;
using System.Collections.Generic;

public class MothSpawn : MonoBehaviour
{
    public float spawnTime = 10f;
    public int minPlayerDistance = 40;
    public int maxNumMoths = 5;
    

    private static List<Moth> moths;
    private static List<MothSpawn> spawners;
    private static int maxMoths = 0;
    private static float timePassed;
    private static int nextSpawnerIndex;

    private Transform player;
    private bool masterSpawn;

	// Use this for initialization
	void Start () {
        if (moths == null) {
            moths = new List<Moth>();
        }

        maxMoths += maxNumMoths;

        masterSpawn = false;

        if(spawners == null) {
            spawners = new List<MothSpawn>();
            masterSpawn = true;
            nextSpawnerIndex = 0;
        }

        spawners.Add(this);

        player = PlayerControl.Instance.transform;
        timePassed = 0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!masterSpawn) return;

        if (moths.Count > maxMoths) return;

        MothSpawn spawn = spawners[nextSpawnerIndex];
        if(Vector3.Distance(player.position, spawn.transform.position) > spawn.minPlayerDistance)
        {
            if (timePassed >= spawnTime)
            {
                GameObject moth = Instantiate(Prefabs.Moth, spawn.transform.position, Quaternion.identity) as GameObject;
                
                moth.transform.SetParent(spawn.transform);
                moths.Add(moth.GetComponent<Moth>());

                timePassed = 0f;
                nextSpawnerIndex = (nextSpawnerIndex + 1) % spawners.Count;

                Debug.Log("Spawn #" + nextSpawnerIndex + " has spawned a moth");

            }
        }
        else
        {
            nextSpawnerIndex = (nextSpawnerIndex + 1) % spawners.Count;
        }

        if(timePassed < spawnTime) { 
            timePassed += Time.deltaTime;
        }
    }
    
    public static void RemoveMoth(Moth m)
    {
        moths.Remove(m);
    }
}
