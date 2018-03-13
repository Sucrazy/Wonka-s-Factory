using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    public float speed;
	public float reachDist;
	public int currentPoint;
    public Path pathList;


	// Use this for initialization
	void Start () {
        pathList = GameObject.FindGameObjectWithTag("Pather").GetComponent<Path>(); //finds path
    }

	// Update is called once per frame
	void Update () {
		{
			float dist = Vector3.Distance (pathList.path[currentPoint].position, transform.position);

            Vector3 moveTo = pathList.path[currentPoint].position;
            moveTo.y += .625f;

            if (dist <= 1.5) //distance between two machines isn't too large
			    transform.position = Vector3.MoveTowards (transform.position, moveTo, speed);

			if (dist == reachDist) //start moving to next machine
				currentPoint++;
		}
	}
}
