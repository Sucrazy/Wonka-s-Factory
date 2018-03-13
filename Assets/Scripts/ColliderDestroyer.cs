using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroyer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {
            Destroy(gameObject);
        }

        if (other.tag == "Machine")
        {
            Destroy(this.gameObject);
        }
    }
}
