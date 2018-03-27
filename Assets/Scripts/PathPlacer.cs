using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    private GridSystem grid;
    Path pathList;
    public GameObject prefab;
    public GameObject corner;

    void Start() //finds path
    {
        pathList = GameObject.FindGameObjectWithTag("Pather").GetComponent<Path>();
    }

    private void Awake()
    {
        grid = FindObjectOfType<GridSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left click
        {
            RaycastHit hitInfo; //raycast finds click information
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                PlacePathNear(hitInfo.point); //place machine near click
            }
        }
    }

    private void PlacePathNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        finalPosition.y = 0.5f;
        if (Physics.CheckSphere(finalPosition, 0.1f) != true)
        {
            var lastObject = pathList.path[pathList.path.Count - 2];
            if (Vector3.Distance(lastObject.position, finalPosition) <= 1.1f) //check if next machine is close enough
            {
                GameObject obj;
                if (lastObject.position.z != finalPosition.z) //determintes orientation of machine and instantiates it
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0,0,0));
                }
                else
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0,90f,0));
                }

                if (pathList.path.Count > 2)
                {
                    var cornerCheck = pathList.path[pathList.path.Count - 3];
                    
                    if (finalPosition.x > cornerCheck.position.x && finalPosition.z > cornerCheck.position.z)
                    {
                        pathList.path.Remove(lastObject);
                        var lastObjectPos = lastObject.position;
                        Destroy(lastObject.gameObject);
                        GameObject newCorner;
                        Debug.Log(cornerCheck.rotation.y);
                        if (cornerCheck.rotation.y == 0.7071068f)
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 90, 0));
                        else
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 270, 0));
                        pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                    }
                    else if (finalPosition.x > cornerCheck.position.x && finalPosition.z < cornerCheck.position.z)
                    {
                        pathList.path.Remove(lastObject);
                        var lastObjectPos = lastObject.position;
                        Destroy(lastObject.gameObject);
                        GameObject newCorner;
                        Debug.Log(cornerCheck.rotation.y);
                        if (cornerCheck.rotation.y == 0.7071068f)
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 0, 0));
                        else
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 180, 0));
                        pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                    }

                    else if (finalPosition.x < cornerCheck.position.x && finalPosition.z > cornerCheck.position.z)
                    {
                        pathList.path.Remove(lastObject);
                        var lastObjectPos = lastObject.position;
                        Destroy(lastObject.gameObject);
                        GameObject newCorner;
                        Debug.Log(cornerCheck.rotation.y);
                        if (cornerCheck.rotation.y == 0.7071068f)
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 180, 0));
                        else
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 0, 0));
                        pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                    }

                    else if (finalPosition.x < cornerCheck.position.x && finalPosition.z < cornerCheck.position.z)
                    {
                        pathList.path.Remove(lastObject);
                        var lastObjectPos = lastObject.position;
                        Destroy(lastObject.gameObject);
                        GameObject newCorner;
                        Debug.Log(cornerCheck.rotation.y);
                        if (cornerCheck.rotation.y == 0.7071068f)
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 270, 0));
                        else
                            newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 90, 0));
                        pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                    }
                }
                obj.transform.name = "ConveyerBelt"; 
                obj.transform.parent = GameObject.Find("Path").transform; //places object in hierarchy
                pathList.path.Insert(pathList.path.Count - 1, (obj.transform)); //adds object to path
            }
        }
    }
}