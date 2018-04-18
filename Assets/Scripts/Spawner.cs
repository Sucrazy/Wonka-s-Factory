using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public UIManager manager;    
    public float spawnTime;

    private bool stopSpawning = false;

    public void ChangeObject(GameObject newObj)
    {
        prefab = newObj;
    }

    public void SpawnTrigger()
    {
        if (stopSpawning == false)
        {
            InvokeRepeating("SpawnObject", 0, spawnTime);
            stopSpawning = true;
        }
        else
        {
            CancelInvoke();
            stopSpawning = false;
        }
    }

    private void SpawnObject()
    {
        if (manager.CanBuy(prefab.GetComponent<CandyInfo>().cost))
        {
            manager.AddToBalance(-(prefab.GetComponent<CandyInfo>().cost));
            GameObject obj = Instantiate(prefab, transform.position, transform.rotation);
            obj.transform.parent = GameObject.Find("Candy").transform; //places object in hierarchy
        }
    }
}
