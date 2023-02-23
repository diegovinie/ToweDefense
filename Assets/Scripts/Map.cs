using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Dimensions")]
    public int width = 10;
    public int height = 16;
    public float nodeSize = 4.5f;
    [Header("Prefabs")]
    public GameObject nodePrefab;
    public GameObject waypointPrefab;
    [Header("Other References")]
    public Transform startPoint;
    public Transform endPoint;
    public GameObject ground;
    public GameObject path;
    public GameObject nodeGroup;
    public GameObject waypointGroup;
    public List<GameObject> waypoints = new List<GameObject>();
    private GameObject[,] nodes;
    private bool[,] reachableSlots;
    private (int, int) startSlot;
    private (int, int) endSlot;

    // Start is called before the first frame update
    void Start()
    {
        reachableSlots = new bool[width, height];
        startSlot = FindSlot(startPoint);
        endSlot = FindSlot(endPoint);

        ScaleGround();
        GeneratePathC();
        GenerateNodes();
        PlaceEndPoint();
        DrawWaypointPath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GeneratePathC()
    {
        bool isVertical = false;
        // random deltas
        int di;
        int dk;
        // max random
        int maxRI = 5;
        int maxRK = 4;
        // boundaries
        int minI = startSlot.Item1;
        int minK = startSlot.Item2;
        int maxI = width - 1;
        int maxK = height - 1;

        int currentI = minI;
        int currentK = minK;
        int nextI;
        int nextK;
        // directions unitary
        int dirI = 1;
        int dirK = 1;

        bool working = true;

        while (working)
        {
            di = Random.Range(3, maxRI);
            dk = Random.Range(2, maxRK);

            if (isVertical)
            {
                nextI = currentI;
                nextK = currentK + dirK * dk;

            }
            else
            {
                if (currentI + 4 > maxI)
                {
                    dirI = -1;
                }

                nextI = currentI + dirI * di;
                nextK = currentK;
            }

            if (nextI >= maxI)
            {
                nextI = maxI;
                working = false;
            } else if (nextK >= maxK)
            {
                nextK = maxK;
                working = false;
            } else if (nextI <= minI && dirI < 0)
            {
                nextI = minI;
                working = false;
            }

            SetReachableSlotsLine((currentI, currentK), (nextI, nextK));

            CreateWaypoint(nextI, nextK);

            isVertical = !isVertical;

            currentI = nextI;
            currentK = nextK;
        }
    }

    public void GenerateNodes()
    {
        Vector3 offset;
        GameObject node;
        nodes = new GameObject[width, height];

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (reachableSlots[i, j] == true) continue;

                offset = nodeSize * (Vector3.right * i + Vector3.back * j);
                node = Instantiate(nodePrefab, transform.position + offset, transform.rotation);

                node.transform.SetParent(nodeGroup.transform);

                nodes[i, j] = node;
            }
        }
    }

    void DrawWaypointPath()
    {
        LineRenderer lr = path.GetComponent<LineRenderer>();
        lr.positionCount = waypoints.Count + 1;
        Vector3 offset = Vector3.up * 2;

        lr.SetPosition(0, startPoint.transform.position + offset);

        for (int i = 0; i < waypoints.Count; i++)
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

        System.Func<int, int> sgn = delegate(int x) { return (int)(x / Mathf.Abs(x)); };

        if (di == 0)
        {
            for (int n = 0; n <= Mathf.Abs(dk); n++)
            {
                reachableSlots[ai, ak + sgn(dk) * n] = true;
            }
        } else if (dk == 0)
        {
            for (int n = 0; n <= Mathf.Abs(di); n++)
            {
                reachableSlots[ai + sgn(di) * n, ak] = true;
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

    void PlaceEndPoint()
    {
        GameObject lastWp = waypoints.FindLast(x => true);

        endPoint.transform.position = lastWp.transform.position;
    }
}
