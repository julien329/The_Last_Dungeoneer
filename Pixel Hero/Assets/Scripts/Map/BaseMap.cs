using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseMap : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();                     // List of different floor prefabs
    public List<GameObject> borderList = new List<GameObject>();                    // List of different wall prefabs
    public GameObject door;                                                         // Special colored door floor prefab
    public GameObject wall;                                                         // Special colored wall prefab
    public GameObject SpawnPoint;                                                   // Spawnpoint object
    public GameObject player;                                                       // Player object

    public static int roomHeight = 13;
    public static int roomWidth = 21;
    public int roomGridX = 10;
    public int roomGridY = 10;
    public int numberOfRoom = 15;
    public int groupingFactor = 100;
    public int maxNeighbours = 3;
    public int nbIteration = 10000;

    // Use this for initialization
    void Start()
    {
        // Basic Room array
        Room[,] tabRooms = new Room[roomGridY, roomGridX];

        InitialiseRooms(tabRooms);
        GenerateDungeon(tabRooms);
        PrintRooms(tabRooms);
        SpawnPlayer();
    }

    // Initialise the grid with starting room
    void InitialiseRooms(Room[,] tabRooms)
    {
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                tabRooms[i, j] = null;
            }
        }

        //Generate the starting room containing the player's spawnpoint at the middle of the map.
        tabRooms[(roomGridY / 2), (roomGridX / 2)] = new StartingRoom(roomWidth, roomHeight, (roomGridX / 2) * roomWidth, (roomGridY / 2) * roomHeight);
    }

    // Generate rooms randomly in the array
    void GenerateDungeon(Room[,] tabRooms)
    {
        int roomCounter = 0;
     
        // Loop until either the number of room match or too many iterations are done.
        while (roomCounter <= numberOfRoom / 4)
        {
            InitialiseRooms(tabRooms);
            roomCounter = 1;
            int endTimer = nbIteration;

            while (roomCounter < numberOfRoom && endTimer != 0)
            {
                endTimer--;

                // Get random position within grid range
                int i = Random.Range(0, roomGridY);
                int j = Random.Range(0, roomGridX);

                //Check if the generated position is a room and if this room does not have too many neighbors
                if (tabRooms[i, j] != null && tabRooms[i, j].NumberOfNeighbors < maxNeighbours)
                {
                    // Generate rooms randomly around the generated position
                    if (i + 1 < roomGridY)
                        GenerateRoom(tabRooms, i, j, (i + 1), j, ref roomCounter);
                    if (i - 1 >= 0)
                        GenerateRoom(tabRooms, i, j, (i - 1), j, ref roomCounter);
                    if (j + 1 < roomGridX)
                        GenerateRoom(tabRooms, i, j, i, (j + 1), ref roomCounter);
                    if (j - 1 >= 0)
                        GenerateRoom(tabRooms, i, j, i, (j - 1), ref roomCounter);
                }
            }
        }
    }

    // Generate a single room if the conditions are met.
    void GenerateRoom(Room[,] tabRooms, int i, int j, int new_i, int new_j, ref int roomCounter)
    {
        // If room at given position is not a room, if random groupingFactor match and if there is not enough room yet, create a room at given position.
        if ((tabRooms[new_i, new_j] == null) && (Random.Range(0f, 100f) <= groupingFactor) && (roomCounter < numberOfRoom))
        {
            tabRooms[new_i, new_j] = new StandartRoom(roomWidth, roomHeight, new_j * roomWidth, new_i * roomHeight);
            // If creating the room occurs too many neightbours to any room in the grid, delete the room
            if (TooManyNeighbours(tabRooms))
                tabRooms[new_i, new_j] = null;
            else
            {
                // Generate a door between the room and the new room
                tabRooms[i, j].ConnectRoom(ref tabRooms[new_i, new_j]);
                roomCounter++;
            }
        }
    }

    // check if any room in the grid has too many neightbours
    bool TooManyNeighbours(Room[,] tabRooms)
    {
        int[,] updatedNeighbors = new int[roomGridY, roomGridY];

        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                // If the position is a room
                if (tabRooms[i, j] != null)
                {
                    int roomCount = 0;

                    // For every neightbour top/down/left/right increment roomCount
                    if ((i + 1 < roomGridY) && (tabRooms[i + 1, j] != null))
                        roomCount++;
                    if ((i - 1 >= 0) && (tabRooms[i - 1, j] != null))
                        roomCount++;
                    if ((j + 1 < roomGridX) && (tabRooms[i, j + 1] != null))
                        roomCount++;
                    if ((j - 1 >= 0) && (tabRooms[i, j - 1] != null))
                        roomCount++;

                    updatedNeighbors[i, j] = roomCount;

                    // Return true if a room has too many neightbours
                    if (roomCount > maxNeighbours)
                        return true;
                }
            }
        }

        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                if (tabRooms[i, j] != null)
                    tabRooms[i, j].NumberOfNeighbors = updatedNeighbors[i,j];
            }
        }

        // Return false if all room have a number of neightbours less or equal to max value
        return false;
    }

    // Get every room to be instanciated on the map
    void PrintRooms(Room[,] tabRooms)
    {
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                // Send the room to be instanciated if the given position is a room
                if (tabRooms[i,j] != null)
                    InstanciateTiles(tabRooms[i, j]);
            }
        }
    }

    // Instanciate every tile as a clone of the original prefab
    void InstanciateTiles(Room room)
    {
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                // Check for the tile Char in order to instanciate the correct object
                switch (room.getTile(i, j))
                {
                    // Spawn a door tile
                    case 'D':
                        // Create a clone from the original prefab.
                        GameObject doorClone = (GameObject)Instantiate(door, new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        // Mark the clone as a child of the current GameObject
                        doorClone.transform.parent = transform;
                        break;
                    // Spawn a floor
                    case 'F':
                        // Create a clone from a random prefab from the list.
                        GameObject floorClone = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        // Mark the clone as a child of the current GameObject
                        floorClone.transform.parent = transform;
                        break;
                    // Spawn a null tile (out of bound)
                    case 'N':
                        // Create a clone from a random prefab from the list.
                        GameObject nullClone = (GameObject)Instantiate(borderList[Random.Range(0, borderList.Count - 1)], new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        // Mark the clone as a child of the current GameObject
                        nullClone.transform.parent = transform;
                        break;
                    // Spawn a wall tile
                    case 'W':
                        // Create a clone from the original prefab.
                        GameObject wallClone = (GameObject)Instantiate(wall, new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        // Mark the clone as a child of the current GameObject
                        wallClone.transform.parent = transform;
                        break;
                    // Spawn a hero (spawnpoint) and a floor tile under it
                    case 'H':
                        // Move spawnpoint location
                        SpawnPoint.transform.position = new Vector3(room.GridPosX + j, room.GridPosY + i, 0);
                        // Create a clone from a random prefab from the list.
                        GameObject playerTile = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + j, room.GridPosY + i, 0), Quaternion.identity);
                        // Mark the clone as a child of the current GameObject
                        playerTile.transform.parent = transform;
                        break;
                }
            }
        }
    }

    // Change player position to match the spawnpoint's position
    void SpawnPlayer()
    {
        player.transform.position = SpawnPoint.transform.position;
    }
}