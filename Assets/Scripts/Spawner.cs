using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public bool stopSpawning = false;
    public float spawnTime; //time between spawns
    public float spawnDelay; //time for first spawn

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnObject()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
