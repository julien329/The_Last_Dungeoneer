using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Room {

    public abstract void CreateRoom();

    public void ConnectRoom(ref Room room)
    {
        if (gridPosX > room.GridPosX)
        {
            tabTiles[roomHeight / 2][0] = 'D';
            room.setTile((room.RoomHeight / 2), (room.RoomWidth - 1), 'D');

        }
        if (gridPosX < room.GridPosX)
        {
            tabTiles[roomHeight / 2][room.RoomWidth - 1] = 'D';
            room.setTile((room.RoomHeight / 2), 0, 'D');
        }
        if (gridPosY > room.GridPosY)
        {
            tabTiles[0][roomWidth / 2] = 'D';
            room.setTile((RoomHeight - 1), (room.RoomWidth / 2), 'D');
        }
        if (gridPosY < room.GridPosY)
        {
            tabTiles[RoomHeight - 1][roomWidth / 2] = 'D';
            room.setTile(0, (room.RoomWidth / 2), 'D');
        }

    }

    public char getTile(int i, int j)
    {
        return tabTiles[i][j];
    }
    public void setTile(int i, int j, char tile)
    {
        tabTiles[i][j] = tile;
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
    public bool IsScanned
    {
        get { return isScanned; }
        set { isScanned = value; }
    }

    protected int roomWidth;
    protected int roomHeight;
    protected int gridPosX;
    protected int gridPosY;
    protected bool isScanned;
    protected List<List<char>> tabTiles= new List<List<char>>();
}
