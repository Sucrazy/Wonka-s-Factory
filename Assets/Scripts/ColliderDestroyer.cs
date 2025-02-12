﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDestroyer : MonoBehaviour {

    public UIManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit") //if candy collides with exit, destroy
        {
            Destroy(gameObject);
        }

        if (other.tag == "Candy")
        {
           manager.AddToBalance(other.gameObject.GetComponent<CandyInfo>().cost);
        }
    }
}
