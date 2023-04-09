using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Transform target;
    Enemy targetEnemy;
    AudioSource fireSound;

    [Header("General")]
    public float range = 15f;
    [Header("Use Bullets (default)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    [Header("Use Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public int damageOverTime = 30;
    public float slowPct = 0.5f;
    public ParticleSystem impactEffect;
    public Light impactLight;
    [Header("Use Guided")]
    public bool useGuided;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public Transform partToTilt;
    public float turnSpeed = 10f;
    public float offsetCorrection = 1f;     // this the distance between rotate point and fire point, important for correct aiming

    public GameObject bulletPrefab;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        if (useLaser)
        {
            lineRenderer.enabled = false;
            impactLight.enabled = false;
        }

        fireSound = GetComponent<AudioSource>();

        impactEffect.Stop();

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (enemyDistance < shortestDistance)
            {
                shortestDistance = enemyDistance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();

            if (targetEnemy == null)
            {
                targetEnemy = nearestEnemy.GetComponentInParent<Enemy>();
            }
        } else {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            if (useLaser) {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactLight.enabled = false;
                    impactEffect.Stop();
                    fireSound.Stop();
                }
            }

            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        } else
        {
            if (IsTargetInAngularRange() && fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;

            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Quaternion lookTilt = Quaternion.LookRotation(target.position - firePoint.position);

        if (partToTilt != null)
        {
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            Vector3 tilt = Quaternion.Lerp(partToTilt.rotation, lookTilt, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
            partToTilt.rotation = Quaternion.Euler(tilt.x, rotation.y, 0);
        } else if (useGuided)
        {
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);
        } else
        {
            dir = target.position - (transform.position + new Vector3(0, offsetCorrection, 0));
            lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(rotation);
        }
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowPct);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactLight.enabled = true;
            impactEffect.Play();
            fireSound.Play();
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized * 1f;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        Bullet  bullet = BulletPool.instance.Get(firePoint.position, firePoint.rotation);
        bullet.gameObject.SetActive(true);
        fireSound.Stop();
        fireSound.Play();

        if (bullet != null) bullet.Seek(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    bool IsTargetInAngularRange()
    {
        Vector3 dir = firePoint.position - target.position;

        float angle = Vector3.Angle(dir, firePoint.eulerAngles);

        return angle < 90f;
    }
}
