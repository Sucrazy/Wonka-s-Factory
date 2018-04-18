using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    private float speed = .05f;
    private float reachDist = .18f;
    private int currentPoint = 0;
    private Path pathList;

    public GameObject zapperConversion;
    public GameObject conversionConversion;
    public GameObject tappyConversion;

    // Use this for initialization
    void Start()
    {
        pathList = GameObject.FindGameObjectWithTag("Pather").GetComponent<Path>(); //finds path
    }

    // Update is called once per frame
    void Update()
    {
        {
            try //try catch to destroy object if any incorrect indexing occurs (such as when deleting machines)
            {
                GameObject obj = pathList.path[currentPoint].gameObject;
                Vector3 moveTo = pathList.path[currentPoint].position;
                float dist = Vector3.Distance(moveTo, transform.position);
                
                moveTo.y += .18f;

                if (dist <= 1.25) //distance between two machines isn't too large
                    transform.position = Vector3.MoveTowards(transform.position, moveTo, speed);

                if (dist == reachDist) //start moving to next machine
                {
                    if (obj.CompareTag("Path"))
                        currentPoint++;

                    else if (obj.CompareTag("Zapper"))
                    {
                        GameObject candyChange = GameObject.Instantiate(zapperConversion, gameObject.transform.position, gameObject.transform.rotation);
                        PathFollower cChange = candyChange.GetComponent<PathFollower>();
                        cChange.currentPoint = currentPoint + 1;
                        Destroy(this.gameObject);
                    }

                    else if (obj.CompareTag("Conversion"))
                    {
                        GameObject candyChange = GameObject.Instantiate(conversionConversion, gameObject.transform.position, gameObject.transform.rotation);
                        PathFollower cChange = candyChange.GetComponent<PathFollower>();
                        cChange.currentPoint = currentPoint + 1;
                        Destroy(this.gameObject);
                    }

                    else
                        currentPoint++;
                }
            }

            catch (System.Exception)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
