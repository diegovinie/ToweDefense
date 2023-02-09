using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 20f;
    private Transform target;
    private int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        target = Waypoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;

        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(target.position, transform.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex < Waypoints.points.Length - 1)
        {
            waypointIndex++;
            target = Waypoints.points[waypointIndex];
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
