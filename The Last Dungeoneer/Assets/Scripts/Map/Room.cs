using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Abstract class parent to derived rooma
public abstract class Room {

    public abstract void CreateRoom();

    // Connect a room with current room (this) with a door in the middle of width or height
    public void ConnectRoom(Room room)
    {
        // If current room is after the other room
        if (gridPosX > room.GridPosX)
        {
            tabTiles[roomHeight / 2][0] = "DoorL";
            room.setTile((room.RoomHeight / 2), (room.RoomWidth - 1), "DoorR");
            this.setDoor(2);
            room.setDoor(3);
        }
        // If current room is before the other room
        if (gridPosX < room.GridPosX)
        {
            tabTiles[roomHeight / 2][room.RoomWidth - 1] = "DoorR";
            room.setTile((room.RoomHeight / 2), 0, "DoorL");
            this.setDoor(3);
            room.setDoor(2);
        }
        // If current room is over the other room
        if (gridPosY > room.GridPosY)
        {
            tabTiles[0][roomWidth / 2] = "DoorB";
            room.setTile((RoomHeight - 1), (room.RoomWidth / 2), "DoorT");
            this.setDoor(1);
            room.setDoor(0);
        }
        // If current room is under the other room
        if (gridPosY < room.GridPosY)
        {
            tabTiles[RoomHeight - 1][roomWidth / 2] = "DoorT";
            room.setTile(0, (room.RoomWidth / 2), "DoorB");
            this.setDoor(0);
            room.setDoor(1);
        }

    }

    public int DistanceFrom(int i, int j)
    {
        return (Mathf.Abs((gridPosX / (roomWidth * tileSize)) - j) + Mathf.Abs((gridPosY / (roomHeight * tileSize)) - i));
    }

    // Place wall tiles around the room.
    protected void placeWall(ref string tile, int j)
    {
        if (tabTiles.Count == 0)
            tile = "WallB";
        if (tabTiles.Count == roomHeight - 1)
            tile = "WallT";
        if (j == 0)
            tile = "WallL";
        if (j == roomWidth - 1)
            tile = "WallR";
    }
    // Place floor tiles
    protected void placeFloor(ref string tile)
    {
        tile = "Floor";
    }
    // Place corner tiles
    protected void placeCorner(ref string tile, int j)
    {
        if (tabTiles.Count == 0 && j == 0)
            tile = "CornerBL";
        if (tabTiles.Count == 0 && j == roomWidth - 1)
            tile = "CornerBR";
        if (tabTiles.Count == roomHeight - 1 && j == 0)
            tile = "CornerTL";
        if (tabTiles.Count == roomHeight - 1 && j == roomWidth - 1)
            tile = "CornerTR";
    }

    // Get char tile of i,j position
    public string getTile(int i, int j)
    {
        return tabTiles[i][j];
    }
    // Set char tile of i,j position
    public void setTile(int i, int j, string tile)
    {
        tabTiles[i][j] = tile;
    }
    // Get doors position (0=up, 1=down, 2=left, 3=right)
    public bool getDoor(int pos)
    {
        return tabDoors[pos];
    }
    //SGet doors position(0=up, 1=down, 2=left, 3=right)
    public void setDoor(int pos)
    {
        tabDoors[pos] = true;
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
    protected int tileSize;
    protected int roomWidth;
    protected int roomHeight;
    protected int gridPosX;
    protected int gridPosY;
    protected int numberOfNeighbors;
    protected bool[] tabDoors= new bool[4];
    protected List<List<string>> tabTiles = new List<List<string>>();
}
