using UnityEngine;

public class MachinePlacer : MonoBehaviour
{
    private GridSystem grid;
    Path pathList;
    public GameObject prefab;

    public UIManager manager;

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
        if (Input.GetMouseButtonDown(1)) //right click
        {
            RaycastHit hitInfo; //raycast finds click information
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (manager.canBuy(prefab.GetComponent<MachineInfo>().cost))
            {
                manager.addtoBalance(-(prefab.GetComponent<MachineInfo>().cost));
                if (Physics.Raycast(ray, out hitInfo))
                {
                    PlaceMachineNear(hitInfo.point); //place machine near click
                }
            }
        }
    }

    public void changeMachine(GameObject newObj)
    {
        prefab = newObj;
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
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0, 90f, 0));
                }
                obj.transform.parent = GameObject.Find("Path").transform; //places object in hierarchy
                pathList.path.Insert(pathList.path.Count - 1, (obj.transform)); //adds object to path
            }
        }
    }
}