using UnityEngine;

public class LevelDataLoader : MonoBehaviour
{
    public int LevelToLoad ;

    public static LevelDataLoader Instance { get; private set; }

    private void Awake()
    {
        // Eğer başka bir instance varsa yok et
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // İlk instance'ı oluştur
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") > 1)
        {
            LevelToLoad = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            LevelToLoad = 1;
            PlayerPrefs.SetInt("CurrentLevel", LevelToLoad);
            PlayerPrefs.Save();
        }
    }

    public LevelData LoadLevelData(int LevelNumber)
    {
        string path = $"Levels/level_{LevelNumber}";
        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile != null)
        {
            return JsonUtility.FromJson<LevelData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON file does not exist: " + path);
            return null;
        }

    }
}
