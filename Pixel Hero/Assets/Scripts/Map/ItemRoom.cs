using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemRoom : Room {

    // Constructor
    public ItemRoom(int width, int height, int x, int y)
    {
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;

        // Generate room tiles
        CreateRoom();
    }

    // Generate room tile according to the item room layout
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
                placeItem(ref tile, j);
                placeCorner(ref tile, j);

                subList.Add(tile);
            }
            tabTiles.Add(subList);
        }
    }

    // Place the item in the middle of the room (TEMPORARY)
    private void placeItem(ref string tile, int j)
    {
        if (tabTiles.Count == roomHeight / 2 && j == roomWidth / 2)
            tile = "Item";
    }
}
