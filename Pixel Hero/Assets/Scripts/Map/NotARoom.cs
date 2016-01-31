using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotARoom : Room {

    public NotARoom(int width, int height, int x, int y)
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
                subList.Add(tile);
            }
            tabTiles.Add(subList);
        }
    }
}
