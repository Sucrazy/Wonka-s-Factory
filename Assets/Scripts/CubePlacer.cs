using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    private GridSystem grid;
    PathFollower pathList;
    public GameObject prefab;

    void Start()
    {
        pathList = GameObject.FindGameObjectWithTag("Machine").GetComponent<PathFollower>();
    }

    private void Awake()
    {
        grid = FindObjectOfType<GridSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                PlaceCubeNear(hitInfo.point);
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
            if (Vector3.Distance(lastObjectPos, finalPosition) <= 1.1f)
            {
                GameObject obj;
                if (lastObjectPos.z != finalPosition.z)
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0,0,0));
                }
                else
                {
                    obj = GameObject.Instantiate(prefab, finalPosition, Quaternion.Euler(0,90,0));
                }
                obj.transform.name = "ConveyerBelt";
                obj.transform.position = finalPosition;
                obj.transform.parent = GameObject.Find("Path").transform;
                pathList.path.Insert(pathList.path.Count - 1, (obj.transform));
            }
        }

        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = nearPoint;
    }
}