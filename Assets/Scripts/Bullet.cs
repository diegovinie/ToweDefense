using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private bool flying = true;
    float flyingTime = 0f;
    public float speed = 70f;
    public int damage = 50;

    public GameObject impactEffect;
    [Header("For Explosive")]
    public float explosionRadius = 0f;

    [Header("For Guided")]
    [SerializeField] bool isGuided;
    [SerializeField] float attackDistance = 5f;

    public void Seek(Transform _target)
    {
        target = _target;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        flyingTime += Time.deltaTime;

        if (flyingTime > 20f) Destroy(gameObject);

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (!flying) return;

        if (isGuided)
        {
            // transform.LookAt(target);
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 dire = lookRotation.eulerAngles;

            if (Vector3.ProjectOnPlane(dir, Vector3.up).magnitude < attackDistance)
            {
                transform.rotation = lookRotation;
            } else
            {
                transform.rotation = Quaternion.Euler(0, dire.y, 0);

            }
        }

        transform.Translate(Vector3.forward * distanceThisFrame, Space.Self);
    }

    void HitTarget(Transform hitted)
    {


        if (explosionRadius > 0f)
        {
            Explode();
            Debug.Log("Explode " + hitted.name);
        }
        else
        {
            Damage(hitted);
        }

    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                DamageRanged(collider.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            HitTarget(other.transform.parent);
        }

        Crash();
    }

    void DamageRanged(GameObject enemyGO)
    {
        Enemy enemy = enemyGO.GetComponent<Enemy>();

        if (!enemy) enemy =enemyGO.GetComponentInParent<Enemy>();

        float distance = Vector3.Distance(transform.position, enemyGO.transform.position);
        float rangedDamage = damage - damage * distance / explosionRadius;

        if (rangedDamage > 0f) enemy.TakeDamage(rangedDamage);
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    void Crash()
    {
        flying = false;
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        Destroy(gameObject);
    }
}
