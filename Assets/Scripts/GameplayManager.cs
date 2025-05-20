using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public GridLoader GridLoader;
    public ObjectiveManager objectiveManager;
    private int GridHeight;
    private int GridWidth;
    private Block[,] AllBlocks;
    private bool IsBusy = false;
    public int MoveCount;
    public TMP_Text MoveCountText;
    public bool IsLevelFinished = false;

    //private int activeAnimations = 0;

    void Start()
    {
        GridHeight = GridLoader.GridHeight;
        GridWidth = GridLoader.GridWidth;
        AllBlocks = GridLoader.AllBlocks;
        MoveCount = GridLoader.MoveCount;

        UpdateRocketStateCubes();
        UpdateMoveCount();
    }

    void Update()
    {
        if (IsBusy) return;
        if (IsLevelFinished) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = new(worldPos.x, worldPos.y);
            HashSet<int> affectedColumns = new();

            Block clicked = FindBlockAtPosition(pos);
            if (clicked == null)
            {
                return;
            }
            else if (clicked is Cube clickedCube)
            {
                affectedColumns = HandleCubeClick(clickedCube, affectedColumns);
            }
            else if (clicked is Rocket clickedRocket)
            {
                affectedColumns = HandleRocketClick(clickedRocket, affectedColumns);
            }

            if (affectedColumns != null)
            {
                MoveCount--;
                DropBlocks(affectedColumns);
                UpdateRocketStateCubes();
                UpdateMoveCount();
                objectiveManager.IsObjectiveCompleted();
                objectiveManager.IsLevelCompleted();
            }


            IsBusy = false;
        }
    }

    private Block FindBlockAtPosition(Vector2 pos)
    {
        float threshold = 0.25f;

        for (int row = 0; row < GridHeight; row++)
        {
            for (int col = 0; col < GridWidth; col++)
            {
                if (AllBlocks[row, col] == null)
                {
                    continue;
                }
                if (Vector2.Distance(pos, AllBlocks[row, col].transform.position) <= threshold)
                {
                    return AllBlocks[row, col];
                }
            }
        }
        return null;
    }

    private HashSet<int> HandleCubeClick(Cube clicked, HashSet<int> affectedColumns)
    {

        List<Cube> connected = new();
        bool[,] visited = new bool[GridHeight, GridWidth];
        bool RocketCreated = false;

        FindConnectedCubes(clicked.Row, clicked.Column, clicked.Code, visited, connected);

        if (connected.Count < 2) return null;

        IsBusy = true;

        if (connected.Count >= 4)
        {
            int SelectedRocket = UnityEngine.Random.Range(0, 2);
            Block rocket = null;

            if (SelectedRocket == 0)
            {
                GameObject NewRocket = Instantiate(GridLoader.HorizontalRocket, clicked.transform.position, Quaternion.identity, GridLoader.transform);
                rocket = NewRocket.GetComponent<Block>();
                rocket.Code = "hro";
            }
            else if (SelectedRocket == 1)
            {
                GameObject NewRocket = Instantiate(GridLoader.VerticalRocket, clicked.transform.position, Quaternion.identity, GridLoader.transform);
                rocket = NewRocket.GetComponent<Block>();
                rocket.Code = "vro";
            }

            rocket.Row = clicked.Row;
            rocket.Column = clicked.Column;
            AllBlocks[clicked.Row, clicked.Column] = rocket;

            RocketCreated = true;
        }

        foreach (Cube cube in connected)
        {
            if (RocketCreated)
            {
                if (clicked.Row != cube.Row || clicked.Column != cube.Column)
                {
                    AllBlocks[cube.Row, cube.Column] = null;
                }
            }
            else
            {
                AllBlocks[cube.Row, cube.Column] = null;
            }

            affectedColumns.Add(cube.Column);

            cube.PlayDestroySequence();
        }



        affectedColumns = BreakObstaclesNextTo(connected, affectedColumns);


        return affectedColumns;
    }

    private HashSet<int> HandleRocketClick(Rocket rocket, HashSet<int> affectedColumns)
    {
        IsBusy = true;
        if (rocket is HorizontalRocket horizontalRocket)
        {
            AllBlocks = horizontalRocket.HorizontalRocketClicked(AllBlocks, GridHeight, GridWidth);
        }

        else if (rocket is VerticalRocket verticalRocket)
        {
            AllBlocks = verticalRocket.VerticalRocketClicked(AllBlocks, GridHeight, GridWidth);
        }

        affectedColumns = rocket.UpdateAffectedColumns(affectedColumns, GridWidth);

        return affectedColumns;
    }

    private void PrintColumnSet(HashSet<int> columns)
    {
        string output = "Affected Columns: ";

        foreach (int col in columns)
        {
            output += col + " ";
        }
    }

    private void DropBlocks(HashSet<int> affectedColumns)
    {
        IsBusy = true;
        int NullCount = 0;

        foreach (int col in affectedColumns)
        {
            for (int row = GridHeight - 1; row >= 0; row--)
            {
                if (AllBlocks[row, col] == null)
                {
                    NullCount++;
                }
                else if (NullCount != 0 && (AllBlocks[row, col].Code == "r" || AllBlocks[row, col].Code == "b" || AllBlocks[row, col].Code == "y" || AllBlocks[row, col].Code == "g" || AllBlocks[row, col].Code == "v" || AllBlocks[row, col].Code == "vro" || AllBlocks[row, col].Code == "hro"))
                {
                    Block block = AllBlocks[row, col];
                    AllBlocks[row + NullCount, col] = block;
                    AllBlocks[row, col] = null;
                    block.Row += NullCount;

                    block.GridPosition = new(block.GridPosition.x, block.GridPosition.y - NullCount * 0.5f);

                    Vector3 NewPos = new(block.transform.position.x, block.transform.position.y - NullCount * 0.5f, block.transform.position.z + NullCount * 0.5f);

                    Tween.Position(block.transform, NewPos, 0.5f, Ease.OutCubic).OnComplete(() => { IsBusy = false; });
                }
                else if (AllBlocks[row, col].Code == "s" || AllBlocks[row, col].Code == "bo")
                {
                    NullCount = 0;
                }
            }

            SpawnNewCubes(NullCount, col);

            NullCount = 0;
        }
        PrintMatrix();
    }

    private void SpawnNewCubes(int CubeCount, int col)
    {
        IsBusy = true;

        float offsetX = GridWidth / 2f;
        float offsetY = GridHeight / 2f;

        for (int i = 0; i < CubeCount; i++)
        {
            List<string> Codes = new(GridLoader.CubeMap.Keys);
            int RandCode = UnityEngine.Random.Range(0, Codes.Count);
            string SelectedCode = Codes[RandCode];

            float SpawnPosY = GridLoader.transform.position.y + GridHeight * 0.5f + i * 0.5f;
            Vector3 SpawnPos = new(GridLoader.transform.position.x + col * 0.5f, SpawnPosY, -19f + GridLoader.transform.position.z - offsetY - i * 0.5f);

            GameObject NewCube = Instantiate(GridLoader.CubeMap[SelectedCode], SpawnPos, Quaternion.identity, GridLoader.transform);
            Block InstantiatedNewCube = NewCube.GetComponent<Block>();
            InstantiatedNewCube.Code = SelectedCode;
            InstantiatedNewCube.Row = CubeCount - i - 1;
            InstantiatedNewCube.Column = col;
            AllBlocks[CubeCount - i - 1, col] = InstantiatedNewCube;

            Vector3 NewPos = new(InstantiatedNewCube.transform.position.x, InstantiatedNewCube.transform.position.y - CubeCount * 0.5f, InstantiatedNewCube.transform.position.z + CubeCount * 0.5f);

            Tween.Position(InstantiatedNewCube.transform, NewPos, 0.5f, Ease.OutCubic).OnComplete(() => { IsBusy = false; });
        }
    }

    private HashSet<int> BreakObstaclesNextTo(List<Cube> cubes, HashSet<int> affectedColumns)
    {
        List<Vase> VisitedVase = new();

        foreach (Cube cube in cubes)
        {
            int row = cube.Row;
            int col = cube.Column;

            int[,] directions = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

            for (int i = 0; i < 4; i++)
            {
                int newRow = row + directions[i, 0];
                int newCol = col + directions[i, 1];

                if (newRow >= 0 && newRow < GridHeight && newCol >= 0 && newCol < GridWidth)
                {
                    Block neighbor = AllBlocks[newRow, newCol];

                    if (neighbor is Box box)
                    {
                        objectiveManager.BoxObjectiveCount--;
                        objectiveManager.CurrentBox.GetComponentInChildren<TMP_Text>().text = objectiveManager.BoxObjectiveCount.ToString();
                        affectedColumns.Add(box.Column);
                        box.PlayDestroySequence();
                        AllBlocks[box.Row, box.Column] = null;
                    }
                    else if (neighbor is Vase vase && !VisitedVase.Contains(vase))
                    {
                        VisitedVase.Add(vase);
                        if (vase.Broken)
                        {
                            objectiveManager.VaseObjectiveCount--;
                            objectiveManager.CurrentVase.GetComponentInChildren<TMP_Text>().text = objectiveManager.VaseObjectiveCount.ToString();

                            affectedColumns.Add(vase.Column);
                            vase.PlayDestroySequence();
                            AllBlocks[vase.Row, vase.Column] = null;
                        }
                        else
                        {
                            vase.UpdateBrokenVase();
                            vase.Broken = true;

                        }
                    }
                }
            }
        }

        return affectedColumns;
    }


    private void PrintMatrix()
    {
        string output = "";

        for (int row = 0; row < GridHeight; row++) 
        {
            for (int col = 0; col < GridWidth; col++)
            {
                Block block = AllBlocks[row, col];

                if (block is Cube cube)
                    output += cube.Code + " ";
                else if (block != null)
                    output += "X "; 
                else
                    output += "N ";
            }

            output += "\n";
        }

    
    }

    public void UpdateRocketStateCubes()
    {
        bool[,] visited = new bool[GridHeight, GridWidth];

        for (int row = 0; row < GridHeight; row++)
        {
            for (int col = 0; col < GridWidth; col++)
            {
                if (visited[row, col]) continue;

                if (AllBlocks[row, col] is Cube cube)
                {
                    List<Cube> connected = new();
                    FindConnectedCubes(row, col, cube.Code, visited, connected);

                    bool shouldBeRocket = connected.Count >= 4;
                    foreach (Cube c in connected)
                    {
                        c.SetRocketMode(shouldBeRocket);
                    }
                }
            }
        }
    }

    private void FindConnectedCubes(int row, int col, string matchCode, bool[,] visited, List<Cube> result)
    {
        if (row < 0 || row >= GridHeight || col < 0 || col >= GridWidth) return;
        if (visited[row, col]) return;

        Block block = AllBlocks[row, col];
        if (block is not Cube cube) return;
        if (cube.Code != matchCode) return;

        visited[row, col] = true;
        result.Add(cube);

        // 4 yönlü DFS
        FindConnectedCubes(row + 1, col, matchCode, visited, result);
        FindConnectedCubes(row - 1, col, matchCode, visited, result);
        FindConnectedCubes(row, col + 1, matchCode, visited, result);
        FindConnectedCubes(row, col - 1, matchCode, visited, result);
    }

    private void UpdateMoveCount()
    {
        MoveCountText.text = MoveCount.ToString();
    }

}