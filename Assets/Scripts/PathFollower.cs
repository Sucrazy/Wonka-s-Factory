using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

	public List<Transform> path = new List<Transform>();
    public float speed;
	public float reachDist;
	public int currentPoint;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//while (path[currentPoint] != null) 
		{
			float dist = Vector3.Distance (path [currentPoint].position, transform.position);

			transform.position = Vector3.MoveTowards (transform.position, path [currentPoint].position, speed);

			if (dist == reachDist) {
				currentPoint++;
			}

			if (currentPoint >= path.Count) {
				currentPoint = 0;
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
