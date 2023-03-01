using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MonoBehaviour
{
    [SerializeField] Transform missilePrefab;
    [SerializeField] Transform missileGroup;
    [SerializeField] Transform defensesGroup;
    [SerializeField] List<Transform> flyingMissiles = new List<Transform>();

    [SerializeField] float delay = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnMissile(Transform target)
    {
        Transform missile = Instantiate(missilePrefab, transform.position, transform.rotation);
        missile.gameObject.GetComponent<SSMissile>().SetTarget(target);
        missile.SetParent(missileGroup);
        flyingMissiles.Add(missile);
    }

    IEnumerator Attack()
    {
        while (true)
        {
            Transform target = SelectTarget();

            if (target) SpawnMissile(target);

            yield return new WaitForSeconds(delay);
        }
    }

    Transform SelectTarget()
    {
        return defensesGroup.childCount > 0 ? defensesGroup.GetChild(Random.Range(0, defensesGroup.childCount)) : null;
    }
}
