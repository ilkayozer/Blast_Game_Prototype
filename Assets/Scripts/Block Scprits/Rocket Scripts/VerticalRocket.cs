using UnityEngine;
using PrimeTween;
using System.Collections.Generic;
using TMPro;

public class VerticalRocket : Rocket
{
    public GameObject UpPart;
    public GameObject DownPart;
    public ObjectiveManager objectiveManager;

    void Start()
    {
        objectiveManager = FindFirstObjectByType<ObjectiveManager>();
    }

    public Block[,] VerticalRocketClicked(Block[,] AllBlocks, int GridHeight, int GridWidth)
    {
        AllBlocks[Row, Column] = null;
        PlayDestroySequence(GridHeight, GridWidth);

        for (int row = 0; row < GridHeight; row++)
        {
            Block block = AllBlocks[row, Column];

            if (block is Cube cube)
            {
                AllBlocks[row, Column] = null;
                cube.PlayDestroySequence();
            }

            else if (block is Vase vase)
            {
                if (vase.Broken)
                {
                    objectiveManager.VaseObjectiveCount--;
                    objectiveManager.CurrentVase.GetComponentInChildren<TMP_Text>().text = objectiveManager.VaseObjectiveCount.ToString();
                    vase.PlayDestroySequence();
                    AllBlocks[vase.Row, vase.Column] = null;
                }
                else
                {
                    vase.UpdateBrokenVase();
                    vase.Broken = true;

                }
            }

            else if (block is Box box)
            {
                objectiveManager.BoxObjectiveCount--;
                objectiveManager.CurrentBox.GetComponentInChildren<TMP_Text>().text = objectiveManager.BoxObjectiveCount.ToString();
                AllBlocks[row, Column] = null;
                box.PlayDestroySequence();
            }
            else if (block is Stone stone)
            {
                objectiveManager.StoneObjectiveCount--;
                objectiveManager.CurrentStone.GetComponentInChildren<TMP_Text>().text = objectiveManager.StoneObjectiveCount.ToString();
                AllBlocks[row, Column] = null;
                stone.PlayDestroySequence();
            }

            else if (block is HorizontalRocket horizontalRocket)
            {
                AllBlocks = horizontalRocket.HorizontalRocketActivated(AllBlocks, GridHeight, GridWidth);
            }

            else if (block is VerticalRocket verticalRocket)
            {
                AllBlocks = verticalRocket.VerticalRocketActivated(AllBlocks, GridHeight, GridWidth);
            }
        }

        return AllBlocks;
    }

    public Block[,] VerticalRocketActivated(Block[,] AllBlocks, int GridHeight, int GridWidth)
    {
        AllBlocks[Row, Column] = null;
        PlayDestroySequence(GridHeight, GridWidth);

        for (int row = 0; row < GridHeight; row++)
        {
            Block block = AllBlocks[row, Column];

            if (block is Cube cube)
            {
                AllBlocks[row, Column] = null;
                cube.PlayDestroySequence();
            }

            else if (block is Vase vase)
            {
                if (vase.Broken)
                {
                    objectiveManager.VaseObjectiveCount--;
                    objectiveManager.CurrentVase.GetComponentInChildren<TMP_Text>().text = objectiveManager.VaseObjectiveCount.ToString();

                    vase.PlayDestroySequence();
                    AllBlocks[vase.Row, vase.Column] = null;
                }
                else
                {
                    vase.UpdateBrokenVase();
                    vase.Broken = true;

                }
            }

            else if (block is Box box)
            {
                objectiveManager.BoxObjectiveCount--;
                objectiveManager.CurrentBox.GetComponentInChildren<TMP_Text>().text = objectiveManager.BoxObjectiveCount.ToString();
                AllBlocks[row, Column] = null;
                box.PlayDestroySequence();
            }
            else if (block is Stone stone)
            {
                objectiveManager.StoneObjectiveCount--;
                objectiveManager.CurrentStone.GetComponentInChildren<TMP_Text>().text = objectiveManager.StoneObjectiveCount.ToString();
                AllBlocks[row, Column] = null;
                stone.PlayDestroySequence();
            }

            else if (block is HorizontalRocket horizontalRocket)
            {
                AllBlocks = horizontalRocket.HorizontalRocketActivated(AllBlocks, GridHeight, GridWidth);
            }

            else if (block is VerticalRocket verticalRocket)
            {
                AllBlocks = verticalRocket.VerticalRocketActivated(AllBlocks, GridHeight, GridWidth);
            }
        }
        return AllBlocks;
    }

    public void PlayDestroySequence(int GridHeight, int GridWidth)
    {
        
        float duration = 0.1f;
        
        Tween.Scale(transform, Vector3.zero, duration)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
        
    }

    public void SpawnParticles(int GridHeight, int GridWidth)
    {
        GameObject _UpPart = Instantiate(UpPart, transform.position, Quaternion.identity, transform);
        GameObject _DownPart = Instantiate(DownPart, transform.position, Quaternion.identity, transform);

        Vector2 _UpPartNewPos = new(transform.position.x, transform.position.y + Row * 0.5f);
        Vector2 _DownPartNewPos = new(transform.position.x, transform.position.y - ((GridHeight - Row) * 0.5f));

        Tween.Position(_UpPart.transform, _UpPartNewPos, 1f, Ease.Linear).OnComplete(() => Destroy(_UpPart.gameObject));
        Tween.Position(_DownPart.transform, _DownPartNewPos, 1f, Ease.Linear).OnComplete(() => Destroy(_DownPart.gameObject));

        Destroy(gameObject);
    }
}
