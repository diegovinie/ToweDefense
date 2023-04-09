using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IBulletPooled
{
    private Transform target;
    private bool flying = true;
    float flyingTime = 0f;
    [SerializeField] float maxFlyingTime = 1.5f;
    public float speed = 70f;
    public int damage = 50;

    public GameObject impactEffect;
    [Header("For Explosive")]
    public float explosionRadius = 0f;

    [Header("For Guided")]
    [SerializeField] bool isGuided;
    [SerializeField] float attackDistance = 5f;
    BulletPool pool;

    public BulletPool Pool {
        get => pool;
        set
        {
            if (pool == null)
                pool = value;
            else
                throw new System.Exception("Bad pool use, this should ony get set once");
        }
    }

    public void Seek(Transform _target)
    {
        target = _target;

    }
    // Start is called before the first frame update
    void Start()
    {
        Pool = BulletPool.instance as BulletPool;
    }

    void OnEnable()
    {
        flyingTime = 0f;
        flying = true;
    }

    // Update is called once per frame
    void Update()
    {
        flyingTime += Time.deltaTime;

        if (flyingTime > maxFlyingTime) ReturnToPool();

        float distanceThisFrame = speed * Time.deltaTime;

        if (!flying) return;

        if (isGuided && target != null)
        {
            Vector3 dir = target.position - transform.position;
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
        ReturnToPool();
        GetEffectFromPool();
    }

    void ReturnToPool()
    {
        pool.Return(this);
    }

    void GetEffectFromPool()
    {
        GameObject effectIns = GameObjectPool.instance.Get( transform.position, transform.rotation);
        effectIns.SetActive(true);

        Timer.instance.SetTimeout(4f, () => { GameObjectPool.instance.Return(effectIns); });
    }
}
