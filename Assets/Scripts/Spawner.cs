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
            GameObject obj = Instantiate(prefab, transform.position, transform.rotation);
            obj.transform.parent = GameObject.Find("Candy").transform; //places object in hierarchy
        }
    }

    public void changeObject(GameObject newObj)
    {
        prefab = newObj;
    }
}
