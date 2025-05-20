using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectNextLevel : MonoBehaviour
{
    public TMP_Text LevelText;
    public Button PlayButton;

    void Start()
    {
        PlayButton.interactable = false;
        Invoke(nameof(EnableButton), 0.1f);
        /*
        if (PlayerPrefs.GetInt("CurrentLevel") > 1)
        {
            SavedLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", SavedLevel);
        }
        */
        if (PlayerPrefs.GetInt("CurrentLevel") > 10)
        {
            FinishedPLevelButton();
        }
        else
        {
            UpdateLevelButton();
        }

    }

    public void UpdateLevelButton()
    {
        LevelText.text = "Level " + LevelDataLoader.Instance.LevelToLoad.ToString();
    }

    public void FinishedPLevelButton()
    {
        PlayButton.interactable = false;
        LevelText.text = "Finished";
    }

    public void PlayNextLevel()
    {
        Debug.Log("levelbutton tetiklendi");
        SceneManager.LoadScene("LevelScene");
    }

    public void SetLevelPref1()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.Save();
    }
    
    void EnableButton()
    {
        PlayButton.interactable = true;
    }
}