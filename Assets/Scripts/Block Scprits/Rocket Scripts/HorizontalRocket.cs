using UnityEngine;
using PrimeTween;
using TMPro;

public class HorizontalRocket : Rocket
{
    public GameObject RightPart;
    public GameObject LeftPart;
    private ObjectiveManager objectiveManager;

    void Start()
    {
        objectiveManager = FindFirstObjectByType<ObjectiveManager>();
    }


    public Block[,] HorizontalRocketClicked(Block[,] AllBlocks, int GridHeight, int GridWidth)
    {

        AllBlocks[Row, Column] = null;
        PlayDestroySequence();

        for (int col = 0; col < GridWidth; col++)
        {
            Block block = AllBlocks[Row, col];

            if (block is Cube cube)
            {
                AllBlocks[Row, col] = null;
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
                AllBlocks[Row, col] = null;
                box.PlayDestroySequence();
            }
            else if (block is Stone stone)
            {
                objectiveManager.StoneObjectiveCount--;
                objectiveManager.CurrentStone.GetComponentInChildren<TMP_Text>().text = objectiveManager.StoneObjectiveCount.ToString();
                AllBlocks[Row, col] = null;
                stone.PlayDestroySequence();
            }

            else if (block is HorizontalRocket horizontalRocket)
            {
                AllBlocks = horizontalRocket.HorizontalRocketClicked(AllBlocks, GridHeight, GridWidth);
            }

            else if (block is VerticalRocket verticalRocket)
            {
                AllBlocks = verticalRocket.VerticalRocketClicked(AllBlocks, GridHeight, GridWidth);
            }
        }



        return AllBlocks;
    }

    public Block[,] HorizontalRocketActivated(Block[,] AllBlocks, int GridHeight, int GridWidth)
    {
        AllBlocks[Row, Column] = null;
        PlayDestroySequence();

        for (int col = 0; col < GridWidth; col++)
        {
            Block block = AllBlocks[Row, col];

            if (block is Cube cube)
            {
                AllBlocks[Row, col] = null;
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
                AllBlocks[Row, col] = null;
                box.PlayDestroySequence();
            }
            else if (block is Stone stone)
            {
                objectiveManager.StoneObjectiveCount--;
                objectiveManager.CurrentStone.GetComponentInChildren<TMP_Text>().text = objectiveManager.StoneObjectiveCount.ToString();
                AllBlocks[Row, col] = null;
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

    public void PlayDestroySequence()
    {
        float duration = 0.1f;

        Tween.Scale(transform, Vector3.zero, duration)
            .OnComplete(() =>
            {
                SpawnParticles();
                Destroy(gameObject);
            });
    }

    void SpawnParticles()
    {
        
    }
}
