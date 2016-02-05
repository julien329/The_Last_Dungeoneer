using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Standart room layout
public class StandartRoom : Room {

    // Constructor
    public StandartRoom(int width, int height, int x, int y)
    {
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;
        numberOfNeighbors = 0;

        // Generate room tiles
        CreateRoom();
    }
   
    // Generate room tile according to the standart room layout
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
}
