using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

	public List<Transform> path = new List<Transform>();
    public float speed;
	public float reachDist;
	public int currentPoint;
    public PathFollower pathList;


	// Use this for initialization
	void Start () {
        pathList = GameObject.FindGameObjectWithTag("Machine").GetComponent<PathFollower>();
        path = pathList.path;
    }

	// Update is called once per frame
	void Update () {
		//while (path[currentPoint] != null) 
		{
			float dist = Vector3.Distance (path [currentPoint].position, transform.position);

            Vector3 moveTo = path[currentPoint].position;
            moveTo.y += .625f;

            if (dist <= 1.5)
			    transform.position = Vector3.MoveTowards (transform.position, moveTo, speed);

			if (dist == reachDist) {
				currentPoint++;
			}

		}

	}

	void Draw()
	{
		if (path.Count > 0) 
			for (int i = 0; i < path.Count; i++) {
				if (path [i] != null) {
					Gizmos.DrawSphere (path [i].position, reachDist);
				}
			}	
	}
}
