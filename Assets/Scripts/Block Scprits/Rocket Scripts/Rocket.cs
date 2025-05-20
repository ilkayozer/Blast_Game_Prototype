using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : Block
{
    public HashSet<int> UpdateAffectedColumns(HashSet<int> affectedColumns, int GridWidth)
    {
        for (int added = 0; added < GridWidth; added++)
        {
            affectedColumns.Add(added);
        }

        return affectedColumns;
    }

    public List<Rocket> FindAdjacentRockets(Block[,] AllBlocks, int GridHeight, int GridWidth)
    {
        List<Rocket> foundRockets = new();

        int[,] directions = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

        for (int i = 0; i < 4; i++)
        {
            int newRow = Row + directions[i, 0];
            int newCol = Column + directions[i, 1];

            if (newRow < 0 || newRow >= GridHeight || newCol < 0 || newCol >= GridWidth)
                continue;

            Block neighbor = AllBlocks[newRow, newCol];
            if (neighbor is Rocket rocketNeighbor)
            {
                foundRockets.Add(rocketNeighbor);
            }
        }

        return foundRockets;
    }

    /*
    public Block[,] ActivateRocketCombo(Block[,] AllBlocks, int GridHeight, int GridWidth, List<Rocket> AdjacentRockets)
    {
        for (int DestroyRow = Row - 1; DestroyRow <= Row + 1; DestroyRow++)
        {
            if (DestroyRow < 0 || DestroyRow >= GridHeight)
                continue;

            for (int col = 0; col < GridWidth; col++)
            {
                AllBlocks = DestroyBlockForRockets(AllBlocks, GridHeight, GridWidth, DestroyRow, col);
            }
        }

        for (int DestroyColumn = Column - 1; DestroyColumn < GridWidth; DestroyColumn++)
        {
            if (DestroyColumn < 0 || DestroyColumn >= GridWidth)
                continue;

            for (int row = 0; row < GridHeight; row++)
            {
                AllBlocks = DestroyBlockForRockets(AllBlocks, GridHeight, GridWidth, row, DestroyColumn);
            }
        }


        return AllBlocks;
    }
    */

    /*
    public Block[,] DestroyBlockForRockets(Block[,] AllBlocks, int GridHeight, int GridWidth, int row, int col)
    {
        Block block = AllBlocks[row, col];

        if (block is Cube cube)
        {
            AllBlocks[row, col] = null;
            cube.PlayDestroySequence();
        }

        else if (block is Vase vase)
        {
            // VASE KONTROLLERİ YAZILACAK
        }

        else if (block is Obstacle obs)
        {
            AllBlocks[row, col] = null;
            obs.PlayDestroySequence();
        }

        else if (block is HorizontalRocket horizontalRocket)
        {
            AllBlocks = horizontalRocket.HorizontalRocketActivated(AllBlocks, GridHeight, GridWidth);
        }

        else if (block is VerticalRocket verticalRocket)
        {
            AllBlocks = verticalRocket.VerticalRocketActivated(AllBlocks, GridHeight, GridWidth);
        }

        return AllBlocks;
    }
    */
}


