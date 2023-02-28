using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MonoBehaviour
{
    [SerializeField] Transform missilePrefab;
    [SerializeField] Transform missileGroup;
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

    void SpawnMissile()
    {
        Transform missile = Instantiate(missilePrefab, transform.position, transform.rotation);

        missile.SetParent(missileGroup);
        flyingMissiles.Add(missile);
    }

    IEnumerator Attack()
    {
        while (true)
        {
            SpawnMissile();

            yield return new WaitForSeconds(delay);
        }
    }
}
