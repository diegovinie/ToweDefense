using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int width = 10;
    public int height = 16;
    public GameObject ground;
    public GameObject path;
    public GameObject nodePrefab;
    public GameObject waypointPrefab;
    public Transform startPoint;
    public Transform endPoint;
    public GameObject nodeGroup;
    public GameObject waypointGroup;
    public int waypointTotal = 9;
    private GameObject[,] nodes;
    public List<GameObject> waypoints = new List<GameObject>();
    private bool[,] reachableSlots;
    private int nodeSize = 5;
    private (int, int) startSlot;
    private (int, int) endSlot;

    // Start is called before the first frame update
    void Start()
    {
        reachableSlots = new bool[width, height];

        startSlot = FindSlot(startPoint);
        endSlot = FindSlot(endPoint);

        // (int, int)[] points = MakeRandomPoints(6);


        // foreach (var point in points)
        // {
        //     CreateWaypoint(point.Item1, point.Item2);
        // }

        ScaleGround();
        GenerateWaypoints();
        GenerateNodes();
        DrawWaypointPath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateWaypoints()
    {
        Vector3 dir;
        bool isVertical = false;
        int steps = 2;

        int currentI = startSlot.Item1;
        int currentK = startSlot.Item2;
        int nextI;
        int nextK;

        for (int i = 0; i < waypointTotal - 1; i++)
        {
            steps = Random.Range(1, 4);
            dir = (endPoint.position - startPoint.position).normalized;

            if (isVertical)
            {
                nextI = currentI;
                nextK = currentK + steps;

            } else
            {
                nextI = currentI + steps;
                nextK = currentK;
            }

            SetReachableSlotsLine((currentI, currentK), (nextI, nextK));

            CreateWaypoint(nextI, nextK);

            isVertical = !isVertical;

            currentI = nextI;
            currentK = nextK;
        }

        CreateWaypoint(endSlot.Item1, endSlot.Item2);
    }

    public void GenerateNodes()
    {
        float delta = 5;
        Vector3 offset;
        GameObject node;
        nodes = new GameObject[width, height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (reachableSlots[i, j] == true) continue;

                offset = delta * (Vector3.right * i + Vector3.back * j);
                node = Instantiate(nodePrefab, transform.position + offset, transform.rotation);

                node.transform.SetParent(nodeGroup.transform);

                nodes[i, j] = node;
            }
        }
    }

    void DrawWaypointPath()
    {
        LineRenderer lr = path.GetComponent<LineRenderer>();
        lr.positionCount = waypointTotal + 1;
        Vector3 offset = Vector3.up * 2;

        lr.SetPosition(0, startPoint.transform.position + offset);

        for (int i = 0; i < waypointTotal; i++)
        {
            lr.SetPosition(i + 1, waypoints[i].transform.position + offset);
        }
    }

    void SetReachableSlotsLine((int, int) pairA, (int, int) pairB)
    {
        int ai = pairA.Item1;
        int ak = pairA.Item2;
        int bi = pairB.Item1;
        int bk = pairB.Item2;
        int di = bi - ai;
        int dk = bk - ak;

        if (di == 0)
        {
            for (int n = 0; n <= Mathf.Abs(dk); n++)
            {
                reachableSlots[ai, ak + n] = true;
            }
        } else if (dk == 0)
        {
            for (int n = 0; n <= Mathf.Abs(di); n++)
            {
                reachableSlots[ai + n, ak] = true;
            }
        } else
        {
            Debug.Log("SetBoolLine error ");
        }
    }

    void CreateWaypoint(int slotI, int slotK)
    {
        Vector3 offset = startPoint.position;
        GameObject wp;

        offset = new Vector3(slotI * nodeSize, 2, -slotK * nodeSize);

        wp = Instantiate(waypointPrefab, offset, Quaternion.identity);

        wp.transform.SetParent(waypointGroup.transform);

        waypoints.Add(wp);
    }

    (int, int) FindSlot(Transform target)
    {
        Vector3 pos = target.transform.position;

        return ((int)(pos.x / nodeSize), (int)(-pos.z / nodeSize));
    }

    (int, int)[] MakeRandomPoints(int total)
    {
        Random.Range(1, 5);
        // int inners = total -2;

        (int, int)[] points = new (int, int)[total];
        int currentI;
        int currentK;
        // int boundI = startSlot.Item1;
        // int boundK = startSlot.Item2;


        for (int n = 0; n < total; n++)
        {
            currentI = n == 0 ? startSlot.Item1 : Random.Range(2, width);
            currentK = n == total - 1 ? endSlot.Item2 : Random.Range(2, height);

            points[n] = (currentI, currentK);
        }

        return points;
    }

    void ScaleGround()
    {
        ground.transform.localScale = new Vector3(width * nodeSize, 1, height * nodeSize);

        Vector3 nextPos = new Vector3(
            transform.position.x + width / 2 * nodeSize - 2.5f,
            ground.transform.position.y,
            transform.position.z - height / 2 * nodeSize + 2.5f
        );

        ground.transform.position = nextPos;
    }
}
