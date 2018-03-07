using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    private GridSystem grid;
    PathFollower pathList;

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
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = finalPosition;

        pathList.path.Add(obj.transform);
        //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = nearPoint;
    }
}