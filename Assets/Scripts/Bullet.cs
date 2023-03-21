using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private bool flying = true;
    public float speed = 70f;
    public int damage = 50;
    public float explosionRadius = 0f;
    public GameObject impactEffect;

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
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // if (dir.magnitude <= distanceThisFrame)
        // {
        //     HitTarget();
        //     return;
        // }

        if (!flying) return;

        transform.Translate(Vector3.forward * distanceThisFrame, Space.Self);
        // if is missile
        // transform.LookAt(target);
    }

    void HitTarget(Transform hitted)
    {


        if (explosionRadius > 0f)
        {
            Explode();
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
        float distance = Vector3.Distance(transform.position, enemyGO.transform.position);
        float rangedDamage = 50 - 50 * distance / explosionRadius;

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
