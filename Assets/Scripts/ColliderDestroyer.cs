using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroyer : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit") //if candy collides with exit, destroy
        {
            Destroy(gameObject);
        }

        if (other.tag == "Machine") //if candy collides with each other, destroy
        { 
            Destroy(this.gameObject);
        }
    }
}
