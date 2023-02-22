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
    private List<GameObject> waypoints;
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

        int currentX = startSlot.Item1;
        int currentY = startSlot.Item2;
        int nextX;
        int nextY;

        for (int i = 0; i < waypointTotal - 1; i++)
        {
            dir = (endPoint.position - startPoint.position).normalized;

            if (isVertical)
            {
                nextX = currentX;
                nextY = currentY + steps;

            } else
            {
                nextX = currentX + steps;
                nextY = currentY;
            }

            SetReachableSlotsLine((currentX, currentY), (nextX, nextY));

            CreateWaypoint(nextX, nextY);

            isVertical = !isVertical;
            currentX = nextX;
            currentY = nextY;
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

        // lr.SetPosition(1, endPoint.transform.position);
    }

    void SetReachableSlotsLine((int, int) pairA, (int, int) pairB)
    {
        int ax = pairA.Item1;
        int ay = pairA.Item2;
        int bx = pairB.Item1;
        int by = pairB.Item2;
        int dx = bx - ax;
        int dy = by - ay;

        if (dx == 0)
        {
            for (int i = 0; i <= Mathf.Abs(dy); i++)
            {
                reachableSlots[ax, ay + i] = true;
            }
        } else if (dy == 0)
        {
            for (int i = 0; i <= Mathf.Abs(dx); i++)
            {
                reachableSlots[ax + i, ay] = true;
            }
        } else
        {
            Debug.Log("SetBoolLine error ");
        }
    }

    void CreateWaypoint(int slotX, int slotZ)
    {
        Vector3 offset = startPoint.position;
        GameObject wp;

        offset = new Vector3(slotX * nodeSize, 2, -slotZ * nodeSize);

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
