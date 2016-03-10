using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Starting room layout
public class StartingRoom : Room {

    // Contructor
    public StartingRoom(int width, int height, int x, int y, int size)
    {
        tileSize = size;
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;
        numberOfNeighbors = 0;

        // Generate room tiles
        CreateRoom();
    }

    // Generate room tiles according to the starting room layout
    public override void CreateRoom()
    {
        for (int i = 0; i < roomHeight; i++)
        {
            List<string> subList = new List<string>();
            for (int j = 0; j < roomWidth; j++)
            {
                string tile = "Null";
                placeFloor(ref tile);
                placeWall(ref tile, j);
                placeCorner(ref tile, j);
                placeCharacter(ref tile, j);

                subList.Add(tile);
            }
            tabTiles.Add(subList);
        }
    }
    // Player charracter spawnpoint
    private void placeCharacter(ref string tile, int j)
    {
        if (j == roomWidth / 2 && tabTiles.Count == (roomHeight / 2))
            tile = "Hero";
    }
}
