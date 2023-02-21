using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public static int EnemiesAlive;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;

    private float countdown = 2f;

    private int waveIndex = 0;

    public TextMeshProUGUI waveCountdownText;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EnemiesAlive > 0) return;

        if (countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];
        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f/ wave.rate);
        }
        waveIndex++;

        if (waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
