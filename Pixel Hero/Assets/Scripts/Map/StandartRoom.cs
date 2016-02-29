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
            List<string> subList = new List<string>();
            for (int j = 0; j < roomWidth; j++)
            {
                string tile = "Null";
                placeFloor(ref tile);
                placeWall(ref tile, j);
                placeCorner(ref tile, j);

                subList.Add(tile);
            }
            tabTiles.Add(subList);
        }
    }
}
