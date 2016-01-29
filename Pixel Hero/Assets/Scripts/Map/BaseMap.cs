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
                for (int y = 0; y < standartRoomHeight; y++)
                {
                    for (int x = 0; x < standartRoomWidth; x++)
                    {
                        int tile = tabRooms[i, j].getTile(y, x);

                        switch (tile)
                        {
                            case 2:
                            case 0:
                                GameObject floorClone = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(tabRooms[i, j].GridPosX + x, tabRooms[i, j].GridPosY + y, 0), Quaternion.identity);
                                floorClone.transform.parent = transform;
                                break;
                            case 1:
                                GameObject boundClone = (GameObject)Instantiate(borderList[Random.Range(0, borderList.Count - 1)], new Vector3(tabRooms[i, j].GridPosX + x, tabRooms[i, j].GridPosY + y, 0), Quaternion.identity);
                                boundClone.transform.parent = transform;
                                break;
                        }
                    }
                }
            }
        }
    }
}
