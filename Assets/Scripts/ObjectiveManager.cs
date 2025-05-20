using TMPro;
using UnityEngine;


public class ObjectiveManager : MonoBehaviour
{
    public GridLoader gridLoader;
    public CelebrationEffect celebrationEffect;
    public GameplayManager gameplayManager;
    public GameObject failMenu;

    public int BoxObjectiveCount;
    public int StoneObjectiveCount;
    public int VaseObjectiveCount;
    private int HowManyObjective = 0;

    private bool[] Objectives;

    public GameObject Box;
    public GameObject Stone;
    public GameObject Vase;

    [HideInInspector]
    public GameObject CurrentBox;
    [HideInInspector]
    public GameObject CurrentStone;
    [HideInInspector]
    public GameObject CurrentVase;

    private bool BoxBool = false;
    private bool StoneBool = false;
    private bool VaseBool = false;

    private bool StoneObjectiveCompleted = true;
    private bool BoxObjectiveCompleted = true;
    private bool VaseObjectiveCompleted = true;

    void Start()
    {
        BoxObjectiveCount = gridLoader.BoxObjective;
        StoneObjectiveCount = gridLoader.StoneObjective;
        VaseObjectiveCount = gridLoader.VaseObjective;

        if (BoxObjectiveCount > 0)
        {
            BoxObjectiveCompleted = false;
            BoxBool = true;
            HowManyObjective++;
        }
        if (StoneObjectiveCount > 0)
        {
            StoneObjectiveCompleted = false;
            StoneBool = true;
            HowManyObjective++;
        }
        if (VaseObjectiveCount > 0)
        {
            VaseObjectiveCompleted = false;
            VaseBool = true;
            HowManyObjective++;
        }
        InstantiateObjectives();

    }

    private void InstantiateObjectives()
    {
        if (HowManyObjective == 1)
        {
            OneObjective();
        }
        else if (HowManyObjective == 2)
        {
            TwoObjective();
        }
        else if (HowManyObjective == 3)
        {
            ThreeObjective();
        }
    }

    private void OneObjective()
    {
        if (BoxBool)
        {
            CurrentBox = Instantiate(Box, transform.position, Quaternion.identity, transform);
            TMP_Text boxText = CurrentBox.GetComponentInChildren<TMP_Text>();
            boxText.text = BoxObjectiveCount.ToString();
        }
        else if (StoneBool)
        {
            CurrentStone = Instantiate(Stone, transform.position, Quaternion.identity, transform);
            TMP_Text stoneText = CurrentStone.GetComponentInChildren<TMP_Text>();
            stoneText.text = StoneObjectiveCount.ToString();
        }
        else if (VaseBool)
        {
            CurrentVase = Instantiate(Vase, transform.position, Quaternion.identity, transform);
            TMP_Text vaseText = CurrentVase.GetComponentInChildren<TMP_Text>();
            vaseText.text = VaseObjectiveCount.ToString();
        }
    }

    private void TwoObjective()
    {
        if (BoxBool && StoneBool)
        {
            Vector3 NewPos1 = new(transform.position.x - 0.3f, transform.position.y, transform.position.z);
            Vector3 NewPos2 = new(transform.position.x + 0.3f, transform.position.y, transform.position.z);

            CurrentBox = Instantiate(Box, NewPos1, Quaternion.identity, transform);
            TMP_Text boxText = CurrentBox.GetComponentInChildren<TMP_Text>();
            boxText.text = BoxObjectiveCount.ToString();
            CurrentBox.transform.localScale = new(1.2f, 1.2f, 1.2f);

            CurrentStone = Instantiate(Stone, NewPos2, Quaternion.identity, transform);
            TMP_Text stoneText = CurrentStone.GetComponentInChildren<TMP_Text>();
            stoneText.text = StoneObjectiveCount.ToString();
            CurrentStone.transform.localScale = new(1.2f, 1.2f, 1.2f);
        }
    }

    private void ThreeObjective()
    {
        Vector3 NewPos1 = new(transform.position.x - 0.3f, transform.position.y + 0.3f, transform.position.z);
        Vector3 NewPos2 = new(transform.position.x + 0.3f, transform.position.y + 0.3f, transform.position.z);
        Vector3 NewPos3 = new(transform.position.x, transform.position.y - 0.15f, transform.position.z);

        CurrentBox = Instantiate(Box, NewPos1, Quaternion.identity, transform);
        TMP_Text boxText = CurrentBox.GetComponentInChildren<TMP_Text>();
        boxText.text = BoxObjectiveCount.ToString();
        CurrentBox.transform.localScale = new(1f, 1f, 1f);

        CurrentStone = Instantiate(Stone, NewPos2, Quaternion.identity, transform);
        TMP_Text stoneText = CurrentStone.GetComponentInChildren<TMP_Text>();
        stoneText.text = StoneObjectiveCount.ToString();
        CurrentStone.transform.localScale = new(1f, 1f, 1f);

        CurrentVase = Instantiate(Vase, NewPos3, Quaternion.identity, transform);
        TMP_Text VaseText = CurrentVase.GetComponentInChildren<TMP_Text>();
        VaseText.text = VaseObjectiveCount.ToString();
        CurrentVase.transform.localScale = new(1f, 1f, 1f);
    }

    public void IsObjectiveCompleted()
    {
        if (BoxBool && BoxObjectiveCount <= 0)
        {
            CurrentBox.transform.Find("Count").gameObject.SetActive(false);
            CurrentBox.transform.Find("Completed").gameObject.SetActive(true);
            BoxObjectiveCompleted = true;
        }

        if (StoneBool && StoneObjectiveCount <= 0)
        {
            CurrentStone.transform.Find("Count").gameObject.SetActive(false);
            CurrentStone.transform.Find("Completed").gameObject.SetActive(true);
            StoneObjectiveCompleted = true;
        }

        if (VaseBool && VaseObjectiveCount <= 0)
        {
            CurrentVase.transform.Find("Count").gameObject.SetActive(false);
            CurrentVase.transform.Find("Completed").gameObject.SetActive(true);
            VaseObjectiveCompleted = true;
        }
    }


    public void IsLevelCompleted()
    {
        if (BoxObjectiveCompleted && StoneObjectiveCompleted && VaseObjectiveCompleted)
        {
            gameplayManager.IsLevelFinished = true;
            LevelDataLoader.Instance.LevelToLoad++;
            PlayerPrefs.SetInt("CurrentLevel", LevelDataLoader.Instance.LevelToLoad);
            PlayerPrefs.Save();

            celebrationEffect.PlayCelebration();
        }
        else if (gameplayManager.MoveCount <= 0)
        {
            gridLoader.gameObject.SetActive(false);
            gameplayManager.IsLevelFinished = true;
            celebrationEffect.ShowGameOver();
        }
    }
    
}