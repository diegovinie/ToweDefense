using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    public Transform model;
    private int waypointIndex = 0;

    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        target = Waypoints.points[0];
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;

        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime);

        if (model != null)
        {
            model.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
        }

        if (Vector3.Distance(target.position, transform.position) <= 0.2f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
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
            EndPath();
        }
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        WaveSpawner.instance.DecreaseEnemies();
        Destroy(gameObject);
    }
}
