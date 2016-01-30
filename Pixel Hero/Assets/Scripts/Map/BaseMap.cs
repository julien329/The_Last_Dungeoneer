using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseMap : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();
    public List<GameObject> borderList = new List<GameObject>();
    public GameObject SpawnPoint;
    public GameObject player;

    static public int roomHeight = 13;
    static public int roomWidth = 19;
    public int roomGridX = 10;
    public int roomGridY = 10;
    public int numberOfRoom = 15;
    public int groupingFactor = 50;

    // Use this for initialization
    void Start()
    {
        Room[,] tabRooms = new Room[roomGridY, roomGridX];
        int[,] map = new int[roomGridY, roomGridX];

        InitialiseRooms(tabRooms, map);
        GenerateDungeon(tabRooms, map);
        PrintRooms(tabRooms);
        SpawnPlayer();
    }


    void GenerateDungeon(Room[,] tabRooms, int[,] map)
    {
        int roomCounter = 1;
        int endTimer = 1000;
        while (roomCounter < numberOfRoom && endTimer != 0)
        {
            endTimer--;
            for (int i = 1; i < roomGridY - 1; i++)
            {
                for (int j = 1; j < roomGridX - 1; j++)
                {
                    if (map[i, j] != 0 && map[i, j] != 2)
                    {
                        if ((map[i + 1, j] == 0) && (Random.Range(0, 100) <= groupingFactor) && (roomCounter < numberOfRoom) && (i >= roomGridY / 2))
                        {
                            map[i + 1, j] = 9; roomCounter++;
                        }
                        if ((map[i - 1, j] == 0) && (Random.Range(0, 100) <= groupingFactor) && (roomCounter < numberOfRoom) && (i <= roomGridY / 2))
                        {
                            map[i - 1, j] = 9; roomCounter++;
                        }
                        if ((map[i, j + 1] == 0) && (Random.Range(0, 100) <= groupingFactor) && (roomCounter < numberOfRoom) && (j >= roomGridX / 2))
                        {
                            map[i, j + 1] = 9; roomCounter++;
                        }
                        if ((map[i, j - 1] == 0) && (Random.Range(0, 100) <= groupingFactor) && (roomCounter < numberOfRoom) && (j <= roomGridX / 2))
                        {
                            map[i, j - 1] = 9; roomCounter++;
                        }

                        if (map[i, j] == 9)
                            map[i, j] = 2;
                    }
                }
            }
        }

        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                if (map[i, j] == 2)
                {
                    tabRooms[i, j] = gameObject.AddComponent<StandartRoom>();
                    tabRooms[i, j].SetAttributes(roomWidth, roomHeight, j * roomWidth, i * roomHeight);
                }
            }
        }

    }

    void InitialiseRooms(Room[,] tabRooms, int[,] map)
    {
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                tabRooms[i, j] = gameObject.AddComponent<StandartRoom>();
                tabRooms[i, j].SetAttributes(roomWidth, roomHeight, j * roomWidth, i * roomHeight);
                tabRooms[i, j].nullifyRoom();

                map[i, j] = 0;
            }
        }

        tabRooms[(roomGridY / 2), (roomGridX / 2)] = gameObject.AddComponent<StartingRoom>();
        tabRooms[(roomGridY / 2), (roomGridX / 2)].SetAttributes(roomWidth, roomHeight, (roomGridX / 2) * roomWidth, (roomGridY / 2) * roomHeight);

        map[(roomGridY / 2), (roomGridX / 2)] = 1;
    }

    void SpawnPlayer()
    {
        player.transform.position = SpawnPoint.transform.position;
    }

    void PrintRooms(Room[,] tabRooms)
    {
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                tabRooms[i,j].SpawnRoom(floorList, borderList, SpawnPoint);
            }
        }
    }
}
