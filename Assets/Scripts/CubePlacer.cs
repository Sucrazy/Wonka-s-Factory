using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    private GridSystem grid;
    Path pathList;
    public GameObject prefab;

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
                PlaceCubeNear(hitInfo.point); //place cube near click
            }
        }
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        finalPosition.y = 0.5f;
        if (Physics.CheckSphere(finalPosition, 0.1f) != true)
        {
            var lastObjectPos = pathList.path[pathList.path.Count - 2].position;
            if (Vector3.Distance(lastObjectPos, finalPosition) <= 1.1f) //check if next machine is close enough
            {
                GameObject obj;
                if (lastObjectPos.z != finalPosition.z) //determintes orientation of machine and instantiates it
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0,0,0));
                }
                else
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0,90,0));
                }
                obj.transform.name = "ConveyerBelt"; 
                obj.transform.parent = GameObject.Find("Path").transform; //places object in hierarchy
                pathList.path.Insert(pathList.path.Count - 1, (obj.transform)); //adds object to path
            }
        }
    }
}