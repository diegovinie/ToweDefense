using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI roundsText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable() {
        roundsText.text = PlayerStats.Rounds.ToString();
    }

    public void Retry()
    {
        // the index is from Build Settings, but another way:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    public void Menu()
    {
        Debug.Log("Go to menu");
    }
}
