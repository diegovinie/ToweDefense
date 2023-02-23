using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GetPoints();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetPoints()
    {
        points = new Transform[transform.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}
