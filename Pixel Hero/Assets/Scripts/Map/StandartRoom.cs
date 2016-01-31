using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandartRoom : Room {

    public StandartRoom(int width, int height, int x, int y)     
    {
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;
        isScanned = false;

        CreateRoom();
    }

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

    private void placeFloor(ref char tile)
    {
        tile = 'F'; 
    }
    private void placeWall(ref char tile, int j)
    {
        if (tabTiles.Count == 0 || tabTiles.Count == roomHeight - 1 || j == 0 || j == roomWidth - 1)
            tile = 'W';
    }
}
