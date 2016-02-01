using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Abstract class parent to derived rooma
public abstract class Room {

    public abstract void CreateRoom();

    // Connect a room with current room (this) with a door in the middle of width or height
    public void ConnectRoom(ref Room room)
    {
        // If current room is above the other room
        if (gridPosX > room.GridPosX)
        {
            tabTiles[roomHeight / 2][0] = 'D';
            room.setTile((room.RoomHeight / 2), (room.RoomWidth - 1), 'D');

        }
        // If current room is under the other room
        if (gridPosX < room.GridPosX)
        {
            tabTiles[roomHeight / 2][room.RoomWidth - 1] = 'D';
            room.setTile((room.RoomHeight / 2), 0, 'D');
        }
        // If current room is after the other room
        if (gridPosY > room.GridPosY)
        {
            tabTiles[0][roomWidth / 2] = 'D';
            room.setTile((RoomHeight - 1), (room.RoomWidth / 2), 'D');
        }
        // If current room is before the other room
        if (gridPosY < room.GridPosY)
        {
            tabTiles[RoomHeight - 1][roomWidth / 2] = 'D';
            room.setTile(0, (room.RoomWidth / 2), 'D');
        }

    }

    // Get char tile of i,j position
    public char getTile(int i, int j)
    {
        return tabTiles[i][j];
    }
    // Set char tile of i,j position
    public void setTile(int i, int j, char tile)
    {
        tabTiles[i][j] = tile;
    }

    // Accessors-modificators
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
    public int NumberOfNeighbors
    {
        get { return numberOfNeighbors; }
        set { numberOfNeighbors = value; }
    }

    //Protected attributes
    protected int roomWidth;
    protected int roomHeight;
    protected int gridPosX;
    protected int gridPosY;
    protected int numberOfNeighbors;
    protected List<List<char>> tabTiles= new List<List<char>>();
}
