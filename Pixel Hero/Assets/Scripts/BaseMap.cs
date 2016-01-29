using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseMap : MonoBehaviour
{

    public List<GameObject> floorList = new List<GameObject>();
    public List<GameObject> borderList = new List<GameObject>();
    static public int standartRoomHeight = 13;
    static public int standartRoomWidth = 19;
    public int roomGridX = 10;
    public int roomGridY = 10;
    public int pourcentageRoom = 75;

    // Use this for initialization
    void Start()
    {
        PrintRooms();
    }

    protected class StandartRoom
    {
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
        private int[,] tabTiles = new int[standartRoomHeight, standartRoomWidth];
    }

    void PrintRooms()
    {
        StandartRoom[,] tabRooms = new StandartRoom[roomGridY, roomGridX];
        
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                tabRooms[i, j] = new StandartRoom(standartRoomWidth, standartRoomHeight, j * standartRoomWidth, i * standartRoomHeight);
                int number = Random.Range(0, 100);
                if (number <= 100 - pourcentageRoom)
                    tabRooms[i, j].nullifyRoom();
            }
        }

        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                for (int iPos = 0; iPos < standartRoomHeight; iPos++)
                {
                    for (int jPos = 0; jPos < standartRoomWidth; jPos++)
                    {
                        int tile = tabRooms[i, j].getTile(iPos, jPos);

                        switch (tile)
                        {
                            case 0:
                                Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(tabRooms[i, j].getXpos(jPos), tabRooms[i, j].getYpos(iPos), 0), Quaternion.identity);
                                break;
                            case 1:
                                Instantiate(borderList[Random.Range(0, borderList.Count - 1)], new Vector3(tabRooms[i, j].getXpos(jPos), tabRooms[i, j].getYpos(iPos), 0), Quaternion.identity);
                                break;
                            case 2:
                                Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(tabRooms[i, j].getXpos(jPos), tabRooms[i, j].getYpos(iPos), 0), Quaternion.identity);
                                break;
                        }
                    }
                }
            }
        }
    }
}
