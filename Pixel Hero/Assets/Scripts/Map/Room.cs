using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Room : MonoBehaviour{

    public abstract void SetAttributes(int width, int height, int x, int y);
    public abstract void SpawnRoom(List<GameObject> floorList, List<GameObject> borderList, GameObject SpawnPoint);
    public abstract void CreateRoom();

    public void nullifyRoom()
    {
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                tabTiles[i][j] = 1;
            }
        }
        isNull = true;
    }

    public int getTile(int i, int j)
    {
        return tabTiles[i][j];
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

    protected int roomWidth { get; set; }
    protected int roomHeight { get; set; }
    protected int gridPosX { get; set; }
    protected int gridPosY { get; set; }
    protected bool isNull { get; set; }
    protected List<List<int>> tabTiles= new List<List<int>>();
    //protected int[,] tabTiles = new int[BaseMap.standartRoomHeight, BaseMap.standartRoomWidth];
}
