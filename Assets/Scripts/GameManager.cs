using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;
    public GameObject gameOverUI;
    public string nextLevel = "Level02";
    public int levelToUnlock = 2;
    public SceneFader sceneFader;

    // Start is called before the first frame update
    void Start()
    {
        GameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.E)) EndGame();

        if (GameIsOver) return;

        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {   GameIsOver = true;

        gameOverUI.SetActive(true);
    }

    public void WinLevel()
    {
        Debug.Log("Level Won!");
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(nextLevel);

    }
}
