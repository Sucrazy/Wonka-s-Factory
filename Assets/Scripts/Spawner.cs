using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public UIManager manager;
    public bool stopSpawning = false;
    public float spawnTime; //time between spawns
    public float spawnDelay; //time for first spawn

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
	}

    public void SpawnObject()
    {
        if (manager.canBuy(prefab.GetComponent<CandyInfo>().cost))
        {
            manager.addtoBalance(-(prefab.GetComponent<CandyInfo>().cost));
            Instantiate(prefab, transform.position, transform.rotation);
        }
    }

    public void changeObject(GameObject newObj)
    {
        prefab = newObj;
    }
}
