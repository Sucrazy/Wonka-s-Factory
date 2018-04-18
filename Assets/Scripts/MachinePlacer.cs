using System.Collections.Generic;
using UnityEngine;

public class MachinePlacer : MonoBehaviour
{
    private GridSystem grid;
    private bool isSelling;
    private bool isValidSquare;
    private bool pathComplete;
    private List<Transform> pathList;
    private Transform highlightBuild;

    public GameObject prefab;
    public GameObject corner;
    public GameObject placeToBuild;
    public UIManager manager;
    
    void Start() //finds path
    {
        pathList = GameObject.FindGameObjectWithTag("Pather").GetComponent<Path>().path;
        isSelling = false;
        SetItem(placeToBuild);
    }

    private void Awake()
    {
        grid = FindObjectOfType<GridSystem>();
    }

    public void SetItem(GameObject placeToBuild)
    {
        highlightBuild = ((GameObject)Instantiate(placeToBuild)).transform;
        highlightBuild.gameObject.layer = 2;
    }

    public void ChangeMachine(GameObject newObj)
    {
        prefab = newObj;
    }

    public void ChangeSelling(bool change)
    {
        isSelling = change;
    }

    private bool CheckValidPlacement(Vector3 finalPosition, Transform lastObject)
    {
        return
            (Physics.CheckSphere(finalPosition, 0.1f) != true && //if object is not there
            Vector3.Distance(lastObject.position, finalPosition) <= 1.1f &&  //&& if next machine is close enough
            (lastObject.tag != "Path" && //&& if it wont turn a machine into a corner
            ((lastObject.position.z != finalPosition.z && lastObject.gameObject.GetComponent<MachineInfo>().rotation == 1) ||
            (lastObject.position.x != finalPosition.x && lastObject.gameObject.GetComponent<MachineInfo>().rotation == 0))) != true &&
            pathComplete != true);
    }

    private void Update()
    {
        RaycastHit hitInfo; //raycast finds click information
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 finalPosition = grid.GetNearestPointOnGrid(hitInfo.point);
            
            if (highlightBuild.transform.position != finalPosition) //if cursor in different sqaure
            {
                Vector3 tempPos = finalPosition; tempPos.y += 0.5f;
                highlightBuild.transform.position = finalPosition; //change highlight
                isValidSquare = CheckValidPlacement(tempPos, pathList[pathList.Count - 2]);
                if (!isValidSquare) //color red
                {
                    highlightBuild.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, .1f, .6f));
                }
                else //color green
                {
                    highlightBuild.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1, .1f, .6f));
                }
            }

            finalPosition.y = 0.5f;
            if (Input.GetMouseButtonDown(0)) //left click
            {
                if (!isSelling) //not selling
                {
                    if (isValidSquare)
                    {
                        if (manager.CanBuy(prefab.GetComponent<MachineInfo>().cost)) //if enough money
                        {
                            manager.AddToBalance(-(prefab.GetComponent<MachineInfo>().cost));
                            PlaceMachineNear(finalPosition, pathList[pathList.Count - 2]); //place machine near click
                            isValidSquare = false; //its not valid anymore, color red
                            highlightBuild.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0, .1f, .6f));
                            if (Vector3.Distance(finalPosition, pathList[pathList.Count - 1].position) <= 1.1f) //if last place object is at the export
                            {
                                PlaceMachineNear(pathList[pathList.Count - 1].position, pathList[pathList.Count - 2]); //do sketchy workaround
                                Destroy(pathList[pathList.Count - 2].gameObject);
                                pathList.RemoveAt(pathList.Count - 2);
                                pathComplete = true;
                            }
                        }
                    }
                }

                else if (hitInfo.transform.gameObject == pathList[pathList.Count - 2].gameObject) //selling
                {
                    manager.AddToBalance(hitInfo.transform.GetComponent<MachineInfo>().cost);
                    pathList.RemoveAt(pathList.Count - 2);
                    Destroy(hitInfo.transform.gameObject);
                    if (pathComplete)
                        pathComplete = false;
                }
            }
        }
    }

    private void PlaceMachineNear(Vector3 finalPosition, Transform lastObject)
    {
        GameObject obj;
        if (lastObject.position.z != finalPosition.z) //determintes orientation of machine and instantiates it
        {
            obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0, 90f, 0));
            obj.gameObject.GetComponent<MachineInfo>().rotation = 1;
        }

        obj.transform.parent = GameObject.Find("Path").transform; //places object in hierarchy
        pathList.Insert(pathList.Count - 1, (obj.transform)); //adds object to path

        if (pathList.Count >= 4) //massive code block logic for making corners, greater than 3 because entrance exit, last placed object, and need a 4th to make corner
        {

            var cornerCheck = pathList[pathList.Count - 4]; //minus 4: because 0 indexing, exit, last placed path, and we need the one previous
            if (finalPosition.x != cornerCheck.position.x && finalPosition.z != cornerCheck.position.z) //if a corner is necessary
            {
                GameObject newCorner;
                var lastObjectPos = lastObject.position;
                pathList.Remove(lastObject); //destroy last object
                Destroy(lastObject.gameObject);
                if (finalPosition.x > cornerCheck.position.x && finalPosition.z > cornerCheck.position.z) //if else statements determine orientation of corner piece
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
                    pathList.Insert(pathList.Count - 2, newCorner.transform);
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
                    pathList.Insert(pathList.Count - 2, newCorner.transform);
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
                    pathList.Insert(pathList.Count - 2, newCorner.transform);
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
                    pathList.Insert(pathList.Count - 2, newCorner.transform);
                    newCorner.transform.parent = GameObject.Find("Path").transform;
                }
            }
        }
    }
}
