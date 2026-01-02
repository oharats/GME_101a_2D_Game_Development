using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private bool _isVictory;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Level 1
        }

        if (Input.GetKeyDown(KeyCode.M) && _isVictory)
        {
            SceneManager.LoadScene(0); //Load Main Menu
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void Victory()
    {
        _isVictory = true;
    }

}
