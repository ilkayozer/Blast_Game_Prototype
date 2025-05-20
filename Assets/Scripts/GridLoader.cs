using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GridLoader : MonoBehaviour
{
    [Header("Resources")]
    public GameObject Red;
    public GameObject Blue;
    public GameObject Green;
    public GameObject Yellow;
    public GameObject Stone;
    public GameObject Vase;
    public GameObject Box;
    public GameObject VerticalRocket;
    public GameObject HorizontalRocket;

    [HideInInspector]
    public int GridWidth;
    [HideInInspector]
    public int GridHeight;
    [HideInInspector]
    public int MoveCount;
    [HideInInspector]
    public int BoxObjective = 0;
    [HideInInspector]
    public int StoneObjective = 0;
    [HideInInspector]
    public int VaseObjective = 0;

    public Dictionary<string, GameObject> CubeMap;
    private Dictionary<string, GameObject> BlockMap;
    public Block[,] AllBlocks;

    public GameObject GameplayManager;
    public GameObject ObjectiveManager;


    public RectTransform BackgroundRect;
    private readonly float CellHeight = 99f;
    private readonly float CellWidth = 98f;

    void Awake()
    {
        CubeMap = new Dictionary<string, GameObject>
        {
            {"r", Red},
            {"g", Green},
            {"b", Blue},
            {"y", Yellow}
        };

        BlockMap = new Dictionary<string, GameObject>
        {
            {"r", Red},
            {"g", Green},
            {"b", Blue},
            {"y", Yellow},
            {"s", Stone},
            {"bo", Box},
            {"v", Vase},
            {"vro", VerticalRocket},
            {"hro", HorizontalRocket}
        };

    }

    void Start()
    {
        LevelData LevelData = LevelDataLoader.Instance.LoadLevelData(LevelDataLoader.Instance.LevelToLoad); 

        GridHeight = LevelData.grid_height;
        GridWidth = LevelData.grid_width;
        MoveCount = LevelData.move_count;

        AllBlocks = new Block[GridHeight, GridWidth];

        GenerateGrid(LevelData.grid);

        GameplayManager.SetActive(true);
        ObjectiveManager.SetActive(true);
    }


    void GenerateGrid(List<string> GridData)
    {
        float offsetX = (GridWidth - 1) / 4f;
        float offsetY = (GridHeight - 1) / 4f;

        int Row = GridHeight - 1;
        int Column = 0;

        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                int index = y * GridWidth + x;
                string CodeFromData = GridData[index];

                GameObject Resource = GetBlockResource(CodeFromData, out string ActualCode);
                Vector3 SpawnPos = new(transform.position.x + x * 0.5f, transform.position.y + y * 0.5f, -(transform.position.y + y * 0.5f));

                GameObject InstantiatedObject = Instantiate(Resource, SpawnPos, Quaternion.identity, transform);
                Block InstantiatedBlock = InstantiatedObject.GetComponent<Block>();
                CountObjectives(InstantiatedBlock);
                InstantiatedBlock.Code = ActualCode;
                InstantiatedBlock.GridPosition = new(SpawnPos.x, SpawnPos.y + 1);
                InstantiatedBlock.Row = Row;
                InstantiatedBlock.Column = x;

                AllBlocks[Row, Column] = InstantiatedBlock;

                Column++;
            }
            Row--;
            Column = 0;
        }
        transform.position = new Vector3(transform.position.x - offsetX, transform.position.y - offsetY, -(transform.position.y - offsetY));
        ResizeBackgroundImage();
    }


    private GameObject GetBlockResource(string Code, out string ActualCode)
    {
        if (BlockMap.ContainsKey(Code))
        {
            ActualCode = Code;
            return BlockMap[Code];
        }
        else if (Code == "rand")
        {
            List<string> RandCodes = new(CubeMap.Keys);
            int SelectedCode = Random.Range(0, RandCodes.Count);
            ActualCode = RandCodes[SelectedCode];
            return CubeMap[ActualCode];
        }
        else
        {
            ActualCode = null;
            return null;
        }
    }


    public void ResizeBackgroundImage()
    {
        BackgroundRect.sizeDelta = new Vector2(GridWidth * CellWidth, GridHeight * CellHeight);
    }

    private void CountObjectives(Block block)
    {
        if (block is Stone)
        {
            StoneObjective++;
        }
        else if (block is Box)
        {
            BoxObjective++;
        }
        else if (block is Vase)
        {
            VaseObjective++;
        }
        else
        {
            return;
        }
    }
    
}