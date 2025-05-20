using UnityEngine;
using UnityEngine.SceneManagement;

public class FailButtons : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
