using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;
    public float startHealth = 100;
    private float health;
    public int worth = 50;
    public GameObject deathEffect;

    public Image healthBar;


    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow( float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        PlayerStats.Money += worth;

        WaveSpawner.EnemiesAlive--;

        Destroy(effect, 2f);
        Destroy(gameObject);
    }
}
