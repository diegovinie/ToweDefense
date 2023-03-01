using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSMissile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float initialSpeed = 6f;
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float impulseTime = 6f;
    [SerializeField] float flyingTime = 20f;
    float speed;
    float timer = 0f;
    bool isTracking = false;
    Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        speed = initialSpeed;

        StartCoroutine(StartTracking());
    }

    // Update is called once per frame
    void Update()
    {
        distance = target ? target.position - transform.position : new Vector3(100, 100, 100);
        timer += Time.deltaTime;

        ApplyImpulse();
        TrackTarget();

        if (timer > flyingTime) Explode();
        if (distance.magnitude < 6) Explode();
    }

    IEnumerator StartTracking()
    {
        yield return new WaitForSeconds(impulseTime);

        isTracking = true;
    }

    void ApplyImpulse()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
    void TrackTarget()
    {
        if (!isTracking && target) return;

        Quaternion lookRotation = Quaternion.LookRotation(distance);

        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

        transform.rotation = Quaternion.Euler(rotation);
    }

    void Explode()
    {
        Destroy(gameObject);

        if (distance.magnitude < 6) Destroy(target.gameObject);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
