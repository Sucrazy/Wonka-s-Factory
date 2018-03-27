using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    public float speed;
	public float reachDist;
	public int currentPoint;
    public Path pathList;

    public GameObject strawberry;
    public GameObject cockroach;

	// Use this for initialization
	void Start () {
        pathList = GameObject.FindGameObjectWithTag("Pather").GetComponent<Path>(); //finds path
    }

	// Update is called once per frame
	void Update () {
		{
            GameObject obj = pathList.path[currentPoint].gameObject;
			float dist = Vector3.Distance (pathList.path[currentPoint].position, transform.position);

            Vector3 moveTo = pathList.path[currentPoint].position;
            moveTo.y += .625f;

            if (dist <= 1.5) //distance between two machines isn't too large
			    transform.position = Vector3.MoveTowards (transform.position, moveTo, speed);

            if (dist == reachDist) //start moving to next machine
            {
                if (obj.CompareTag("Path") || obj.CompareTag("PathCorner"))
                    currentPoint++;

                else if (obj.CompareTag("Zapper"))
                {
                    switch (gameObject.tag)
                    {
                        case "Strawberry":
                            GameObject candyChange = GameObject.Instantiate(cockroach, gameObject.transform.position, gameObject.transform.rotation);
                            PathFollower cChange = candyChange.GetComponent<PathFollower>();
                            cChange.currentPoint = currentPoint + 1;
                            Destroy(this.gameObject);
                            break;

                        case "Cockroach":
                            candyChange = GameObject.Instantiate(strawberry, gameObject.transform.position, gameObject.transform.rotation);
                            cChange = candyChange.GetComponent<PathFollower>();
                            cChange.currentPoint = currentPoint + 1;
                            Destroy(this.gameObject);
                            break;

                        default:
                            Debug.Log("Nothing");
                            currentPoint++;
                            break;
                    }
                }

                else
                    currentPoint++;

            }
		}
	}
}
