using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseMap : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();
    public List<GameObject> borderList = new List<GameObject>();
    public GameObject door;
    public GameObject wall;
    public GameObject SpawnPoint;
    public GameObject player;

    static public int roomHeight = 13;
    static public int roomWidth = 19;
    public int roomGridX = 10;
    public int roomGridY = 10;
    public int numberOfRoom = 15;
    public int groupingFactor = 100;
    public int maxNeighbours = 3;

    // Use this for initialization
    void Start()
    {
        Room[,] tabRooms = new Room[roomGridY, roomGridX];

        InitialiseRooms(tabRooms);
        GenerateDungeon(tabRooms);
        PrintRooms(tabRooms);
        SpawnPlayer();
    }


    void GenerateDungeon(Room[,] tabRooms)
    {
        int roomCounter = 1;
        int endTimer = 10000;
        while (roomCounter < numberOfRoom && endTimer != 0)
        {
            endTimer--;

            int i = Random.Range(1, roomGridY - 1);
            int j = Random.Range(1, roomGridX - 1);

            if (tabRooms[i, j].GetType() != typeof(NotARoom))
            {
                if ((tabRooms[i + 1, j].GetType() == typeof(NotARoom)) && (Random.Range(0f, 100) <= groupingFactor) && (roomCounter < numberOfRoom))
                {
                    tabRooms[i + 1, j] = new StandartRoom(roomWidth, roomHeight, j * roomWidth, (i + 1) * roomHeight);
                    if (TooManyNeighbours(tabRooms))
                        tabRooms[i + 1, j] = new NotARoom(roomWidth, roomHeight, j * roomWidth, (i + 1) * roomHeight);
                    else
                    {
                        tabRooms[i, j].ConnectRoom(ref tabRooms[i + 1, j]);
                        roomCounter++;
                    }
                }
                if ((tabRooms[i - 1, j].GetType() == typeof(NotARoom)) && (Random.Range(0f, 100) <= groupingFactor) && (roomCounter < numberOfRoom))
                {
                    tabRooms[i - 1, j] = new StandartRoom(roomWidth, roomHeight, j * roomWidth, (i - 1) * roomHeight);
                    if (TooManyNeighbours(tabRooms))
                        tabRooms[i - 1, j] = new NotARoom(roomWidth, roomHeight, j * roomWidth, (i - 1) * roomHeight);
                    else
                    {
                        tabRooms[i, j].ConnectRoom(ref tabRooms[i - 1, j]);
                        roomCounter++;
                    }
                }
                if ((tabRooms[i, j + 1].GetType() == typeof(NotARoom)) && (Random.Range(0f, 100) <= groupingFactor) && (roomCounter < numberOfRoom))
                {
                    tabRooms[i, j + 1] = new StandartRoom(roomWidth, roomHeight, (j + 1) * roomWidth, i * roomHeight);
                    if (TooManyNeighbours(tabRooms))
                        tabRooms[i, j + 1] = new NotARoom(roomWidth, roomHeight, (j + 1) * roomWidth, i * roomHeight);
                    else
                    {
                        tabRooms[i, j].ConnectRoom(ref tabRooms[i, j + 1]);
                        roomCounter++;
                    }
                }
                if ((tabRooms[i, j - 1].GetType() == typeof(NotARoom)) && (Random.Range(0f, 100) <= groupingFactor) && (roomCounter < numberOfRoom))
                {
                    tabRooms[i, j - 1] = new StandartRoom(roomWidth, roomHeight, (j - 1) * roomWidth, i * roomHeight);
                    if (TooManyNeighbours(tabRooms))
                        tabRooms[i, j - 1] = new NotARoom(roomWidth, roomHeight, (j - 1) * roomWidth, i * roomHeight);
                    else
                    {
                        tabRooms[i, j].ConnectRoom(ref tabRooms[i, j - 1]);
                        roomCounter++;
                    }
                }
            }
        }
    }

    void InitialiseRooms(Room[,] tabRooms)
    {
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                tabRooms[i, j] = new NotARoom(roomWidth, roomHeight, j * roomWidth, i * roomHeight);
            }
        }

        tabRooms[(roomGridY / 2), (roomGridX / 2)] = new StartingRoom(roomWidth, roomHeight, (roomGridX / 2) * roomWidth, (roomGridY / 2) * roomHeight);
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
                InstanciateTiles(tabRooms[i, j]);
            }
        }
    }

    void InstanciateTiles(Room room)
    {
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                switch (room.getTile(i, j))
                {
                    case 'D':
                        GameObject doorClone = (GameObject)Instantiate(door, new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        doorClone.transform.parent = transform;
                        break;
                    case 'F':
                        GameObject floorClone = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        floorClone.transform.parent = transform;
                        break;
                    case 'N':
                        GameObject nullClone = (GameObject)Instantiate(borderList[Random.Range(0, borderList.Count - 1)], new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        nullClone.transform.parent = transform;
                        break;
                    case 'W':
                        GameObject wallClone = (GameObject)Instantiate(wall, new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        wallClone.transform.parent = transform;
                        break;
                    case 'H':
                        SpawnPoint.transform.position = new Vector3(room.GridPosX + j, room.GridPosY + i, 0);
                        GameObject playerTile = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        playerTile.transform.parent = transform;
                        break;
                }
            }
        }
    }

    bool TooManyNeighbours(Room[,] tabRooms)
    {
        for (int i = 1; i < roomGridY - 1; i++)
        {
            for (int j = 1; j < roomGridX - 1; j++)
            {
                int roomCount = 0;

                if (tabRooms[i + 1, j].GetType() != typeof(NotARoom))
                    roomCount++;
                if (tabRooms[i - 1, j].GetType() != typeof(NotARoom))
                    roomCount++;
                if (tabRooms[i, j + 1].GetType() != typeof(NotARoom))
                    roomCount++;
                if (tabRooms[i, j - 1].GetType() != typeof(NotARoom))
                    roomCount++;

                if (roomCount > maxNeighbours)
                    return true;

            }
        }
        return false;
    }
}