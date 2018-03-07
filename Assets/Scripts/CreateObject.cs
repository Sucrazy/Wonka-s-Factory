using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour {

	Ray ray;
	RaycastHit hit;
	PathFollower pathList;
	public GameObject prefab;

	// Use this for initialization
	void Start () {
		pathList = GameObject.FindGameObjectWithTag("Machine").GetComponent<PathFollower> ();
	}
	
	// Update is called once per frame
	void Update () {
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			if (Input.GetMouseButtonDown(0)) {
				GameObject obj = Instantiate (prefab, hit.point, Quaternion.identity);


				pathList.path.Add (obj.transform);
			}

		}
	}
}

