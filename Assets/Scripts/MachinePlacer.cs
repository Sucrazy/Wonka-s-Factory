using UnityEngine;

public class MachinePlacer : MonoBehaviour
{
    private GridSystem grid;
    bool isSelling;
    Path pathList;
    public GameObject prefab;
    public GameObject corner;
    public UIManager manager;
    private Transform currentBuilding;

    void Start() //finds path
    {
        pathList = GameObject.FindGameObjectWithTag("Pather").GetComponent<Path>();
        isSelling = false;
        //SetItem(prefab);
    }

    private void Awake()
    {
        grid = FindObjectOfType<GridSystem>();
    }

    //public void SetItem(GameObject b)
    //{  
    //    currentBuilding = ((GameObject)Instantiate(b)).transform;
    //    currentBuilding.Rotate(new Vector3(0, 90, 0));
    //    currentBuilding.gameObject.layer = 2;
    //}

    //public void RotateMachine()
    //{
    //    currentBuilding.Rotate(new Vector3(0, 90, 0));
    //}

    private void Update()
    {
        RaycastHit hitInfo; //raycast finds click information
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            var finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
            finalPosition.y += 0.5f;
            //currentBuilding.transform.position = finalPosition;

            if (Input.GetMouseButtonDown(0)) //left click
            {
                if (!isSelling)
                {
                    if (manager.canBuy(prefab.GetComponent<MachineInfo>().cost))
                    {
                        manager.addtoBalance(-(prefab.GetComponent<MachineInfo>().cost));
                        PlaceMachineNear(hitInfo.point); //place machine near click
                    }
                }

                else if (hitInfo.transform.gameObject == pathList.path[pathList.path.Count - 2].gameObject)
                {
                    manager.addtoBalance(hitInfo.transform.GetComponent<MachineInfo>().cost);
                    pathList.path.RemoveAt(pathList.path.Count - 2);
                    Destroy(hitInfo.transform.gameObject);                
                }

            }
        }
    }

    public void changeSelling(bool change)
    {
        isSelling = change;
    }

    public void changeMachine(GameObject newObj)
    {
        prefab = newObj;
        //Destroy(currentBuilding.gameObject);
        //SetItem(prefab);
    }

    private void PlaceMachineNear(Vector3 clickPoint)
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
                    obj.gameObject.GetComponent<MachineInfo>().rotation = 1;
                }

                if (pathList.path.Count > 2)
                {
                       
                    var cornerCheck = pathList.path[pathList.path.Count - 3];
                    if (finalPosition.x != cornerCheck.position.x && finalPosition.z != cornerCheck.position.z)
                    {
                        GameObject newCorner;
                        var lastObjectPos = lastObject.position;
                        pathList.path.Remove(lastObject);
                        Destroy(lastObject.gameObject);
                        if (finalPosition.x > cornerCheck.position.x && finalPosition.z > cornerCheck.position.z)
                        {   
                            if (cornerCheck.GetComponent<MachineInfo>().rotation == 1 || cornerCheck.GetComponent<MachineInfo>().rotation == 3)
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 90, 0));
                                newCorner.gameObject.GetComponent<MachineInfo>().rotation = 0;
                            }
                            else
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 270, 0));
                                newCorner.gameObject.GetComponent<MachineInfo>().rotation = 1;
                            }
                            pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                            newCorner.transform.parent = GameObject.Find("Path").transform;
                        }
                        else if (finalPosition.x > cornerCheck.position.x && finalPosition.z < cornerCheck.position.z)
                        {
                            if (cornerCheck.GetComponent<MachineInfo>().rotation == 1 || cornerCheck.GetComponent<MachineInfo>().rotation == 3)
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 0, 0));
                                newCorner.gameObject.GetComponent<MachineInfo>().rotation = 2;
                            }
                            else
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 180, 0));
                                newCorner.gameObject.GetComponent<MachineInfo>().rotation = 1;
                            }
                            pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                            newCorner.transform.parent = GameObject.Find("Path").transform;
                        }

                        else if (finalPosition.x < cornerCheck.position.x && finalPosition.z > cornerCheck.position.z)
                        {
                            if (cornerCheck.GetComponent<MachineInfo>().rotation == 1 || cornerCheck.GetComponent<MachineInfo>().rotation == 3)
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 180, 0));
                                newCorner.gameObject.GetComponent<MachineInfo>().rotation = 0;
                            }
                            else
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 0, 0));
                                newCorner.gameObject.GetComponent<MachineInfo>().rotation = 3;
                            }
                            pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                            newCorner.transform.parent = GameObject.Find("Path").transform;
                        }

                        else if (finalPosition.x < cornerCheck.position.x && finalPosition.z < cornerCheck.position.z)
                        {
                            if (cornerCheck.GetComponent<MachineInfo>().rotation == 1 || cornerCheck.GetComponent<MachineInfo>().rotation == 3)
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 270, 0));
                                newCorner.GetComponent<MachineInfo>().rotation = 2;
                            }
                            else
                            {
                                newCorner = GameObject.Instantiate(corner, lastObjectPos, Quaternion.Euler(0, 90, 0));
                                newCorner.GetComponent<MachineInfo>().rotation = 1;
                            }
                            pathList.path.Insert(pathList.path.Count - 1, newCorner.transform);
                            newCorner.transform.parent = GameObject.Find("Path").transform;
                        }
                    }
                }
                obj.transform.parent = GameObject.Find("Path").transform; //places object in hierarchy
                pathList.path.Insert(pathList.path.Count - 1, (obj.transform)); //adds object to path
            }
        }
    }
}