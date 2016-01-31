using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Starting room layout
public class StartingRoom : Room {

    // Contructor
    public StartingRoom(int width, int height, int x, int y)
    {
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;

        // Generate room tiles
        CreateRoom();
    }

    // Generate room tiles according to the starting room layout
    public override void CreateRoom()
    {
        for (int i = 0; i < roomHeight; i++)
        {
            List<char> subList = new List<char>();
            for (int j = 0; j < roomWidth; j++)
            {
                char tile = 'N';
                placeFloor(ref tile);
                placeWall(ref tile, j);
                placeCharacter(ref tile, j);

                subList.Add(tile);
            }
            tabTiles.Add(subList);
        }
    }

    // Place floor tiles
    private void placeFloor(ref char tile)
    {
        tile = 'F';
    }
    // Place wall tiles around the room.
    private void placeWall(ref char tile, int j)
    {
        if (tabTiles.Count == 0 || tabTiles.Count == roomHeight - 1 || j == 0 || j == roomWidth - 1)
            tile = 'W';
    }
    // Player charracter spawnpoint
    private void placeCharacter(ref char tile, int j)
    {
        if (j == roomWidth / 2 && tabTiles.Count == (roomHeight / 2) - 1)
            tile = 'H';
    }
}
