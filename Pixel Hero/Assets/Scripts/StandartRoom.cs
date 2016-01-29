using UnityEngine;
using System.Collections;

public class StandartRoom {

    public StandartRoom(int w, int h, int x, int y)
    {
        width = w;
        height = h;
        xPos = x;
        yPos = y;
        isNull = false;

        CreateRoom();
    }

    public void nullifyRoom()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                tabTiles[i, j] = 1;
            }
        }
        isNull = true;
    }

    public void CreateRoom()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
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
        if (i <= 0 + 1 || i >= height - 2 || j <= 0 + 1 || j >= width - 2)
            tabTiles[i, j] = 1;
    }
    private void placeDoors(int i, int j)
    {
        if (i == height / 2)
            if (j <= 0 + 1 || j >= width - 2)
                tabTiles[i, j] = 2;
        if (j == width / 2)
            if (i <= 0 + 1 || i >= height - 2)
                tabTiles[i, j] = 2;
    }

    public int getTile(int i, int j)
    {
        return tabTiles[i, j];
    }
    public int getXpos(int x)
    {
        return xPos + x;
    }
    public int getYpos(int y)
    {
        return yPos + y;
    }
    public bool getIsNull()
    {
        return isNull;
    }

    private int width;
    private int height;
    private int xPos;
    private int yPos;
    private bool isNull;
    private int[,] tabTiles = new int[BaseMap.standartRoomHeight, BaseMap.standartRoomWidth];
}
