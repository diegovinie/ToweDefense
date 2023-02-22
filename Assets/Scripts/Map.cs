using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int size = 10;
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
        reachableSlots = new bool[size, size];

        startSlot = FindSlot(startPoint);
        endSlot = FindSlot(endPoint);

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
        int steps = 3;

        int currentI = startSlot.Item1;
        int currentK = startSlot.Item2;
        int nextI;
        int nextK;

        for (int i = 0; i < waypointTotal - 1; i++)
        {
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
        nodes = new GameObject[size, size];

        for (int j = 0; j < size; j++)
        {
            for (int i = 0; i < size; i++)
            {
                if (reachableSlots[j, i] == true) continue;

                offset = delta * (Vector3.right * j + Vector3.back * i);
                node = Instantiate(nodePrefab, transform.position + offset, transform.rotation);

                node.transform.SetParent(nodeGroup.transform);

                nodes[i, j] = node;
            }
        }
    }

    void DrawWaypointPath()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
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
}
