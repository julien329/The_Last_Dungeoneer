using UnityEngine;
using System.Collections;

public class StandartRoom : Room {

    public StandartRoom(int width, int height, int x, int y)     
    {
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;
        isNull = false;

        CreateRoom();
    }

    public void nullifyRoom()
    {
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                tabTiles[i, j] = 1;
            }
        }
        isNull = true;
    }

    public void CreateRoom()
    {
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                placeFloor(i, j);
                placeBounds(i, j);
                placeDoors(i, j);
            }
        }
    }

    private void placeFloor(int i, int j)
    {
        tabTiles[i, j] = 0;
    }
    private void placeBounds(int i, int j)
    {
        if (i == 0 || i == roomHeight || j == 0 || j == roomWidth)
            tabTiles[i, j] = 1;
    }
    private void placeDoors(int i, int j)
    {
        if (i == roomHeight / 2)
            if (j == 0 || j == roomWidth)
                tabTiles[i, j] = 2;
        if (j == roomWidth / 2)
            if (i == 0 || i == roomHeight)
                tabTiles[i, j] = 2;
    }

    public int getTile(int i, int j)
    {
        return tabTiles[i, j];
    }


    
    public int RoomWidth
    {
        get { return roomWidth; }
        set { roomWidth = value; }
    }
    public int RoomHeight
    {
        get { return roomHeight; }
        set { roomHeight = value; }
    }
    public int GridPosX
    {
        get { return gridPosX; }
        set { gridPosX = value; }
    }
    public int GridPosY
    {
        get { return gridPosY; }
        set { gridPosY = value; }
    }
    public bool IsNull
    {
        get { return isNull; }
        set { isNull = value; }
    }

    private bool isNull;
    private int gridPosX;
    private int gridPosY;
    private int roomHeight;
    private int roomWidth;
    private int[,] tabTiles = new int[BaseMap.standartRoomHeight, BaseMap.standartRoomWidth];
}
