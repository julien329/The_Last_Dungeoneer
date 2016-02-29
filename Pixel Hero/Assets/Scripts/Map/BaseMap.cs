using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BaseMap : MonoBehaviour
{
    public List<GameObject> floorList = new List<GameObject>();                    
    public List<GameObject> doorList = new List<GameObject>();              
    public List<GameObject> wallList = new List<GameObject>();
    public List<GameObject> cornerList = new List<GameObject>();
    public GameObject SpawnPoint;                                                   // Spawnpoint object
    public GameObject player;                                                       // Player object
    public GameObject item;
    public Transform minimapCam;
    public Transform mainCam;

    public int roomHeight = 13;
    public int roomWidth = 17;
    public int roomGridX = 10;
    public int roomGridY = 10;
    public int tileWidth;
    public int tileHeight;
    public int numberOfRoom = 15;
    public int groupingFactor = 100;
    public int maxNeighbours = 3;
    public int nbIteration = 10000;

    private float restartTimer = 0;

    public Room[,] tabRooms;


    // Use this for initialization (called before all Start())
    void Awake()
    {
        InitialiseRooms();
        GenerateDungeon();
        AddBossAndKeyRooms();
        AddItemRoom();
        PrintRooms();
        SpawnPlayer();
    }

    // Update is called once by frame
    void Update()
    {
        // If R is kept pressed, count up with timer
        if (Input.GetKey(KeyCode.R))
            restartTimer += Time.deltaTime;
        // If R is released, restart timer at 0
        if (Input.GetKeyUp(KeyCode.R))
            restartTimer = 0;
        // If timer is greater or equal to 2, restart level
        if (restartTimer >= 1)
            SceneManager.LoadScene(0);
    }

    // Initialise the grid with starting room
    void InitialiseRooms()
    {
        // Basic Room array
        tabRooms = new Room[roomGridY, roomGridX];
        //Generate the starting room containing the player's spawnpoint at the middle of the map.
        tabRooms[(roomGridY / 2), (roomGridX / 2)] = new StartingRoom(roomWidth, roomHeight, (roomGridX / 2) * (roomWidth * tileWidth), (roomGridY / 2) * (roomHeight * tileHeight));
    }

    // Generate rooms randomly in the array
    void GenerateDungeon()
    {
        int roomCounter = 0;

        // Loop until either the number of room match or too many iterations are done.
        while (roomCounter <= numberOfRoom / 4)
        {
            InitialiseRooms();
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
            tabRooms[new_i, new_j] = new StandartRoom(roomWidth, roomHeight, new_j * (tileWidth * roomWidth), new_i * (tileHeight * roomHeight));
            // If creating the room occurs too many neightbours to any room in the grid, delete the room
            if (TooManyNeighbours())
                tabRooms[new_i, new_j] = null;
            else
            {
                // Generate a door between the room and the new room
                tabRooms[i, j].ConnectRoom(tabRooms[new_i, new_j]);
                roomCounter++;
            }
        }
    }

    // Add a boss and KeyRoom away from each other
    void AddBossAndKeyRooms()
    {
        // Create boss room first at the farthest position in the map from the spawnpoint
        int[] posBoss = FarthestRoomFrom(roomGridY / 2, roomGridX / 2);
        tabRooms[posBoss[0], posBoss[1]] = new BossRoom(roomWidth, roomHeight, (posBoss[1] * tileWidth) * roomWidth, (posBoss[0] * tileHeight) * roomHeight);
        ConnectToExistingRooms(posBoss[0], posBoss[1]);

        // Create keyRoom at the farthest position from the created boss room
        int[] posKey = FarthestRoomFrom(posBoss[0], posBoss[1]);
        tabRooms[posKey[0], posKey[1]] = new KeyRoom(roomWidth, roomHeight, (posKey[1] * tileWidth) * roomWidth, (posKey[0] * tileHeight) * roomHeight);
        ConnectToExistingRooms(posKey[0], posKey[1]);
    }

    // Find the farthest position in the map from the given coordinates
    int[] FarthestRoomFrom(int y, int x)
    {
        // Initialize max distance and the position it has been found.
        int max = 0;
        int[] position = new int[2];

        // Scan the map
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                // Make sure there is a room and that it is a standartRoom
                if (tabRooms[i, j] != null && tabRooms[i, j].GetType() == typeof(StandartRoom))
                {
                    // If new distance is greater, save new max and postion
                    if (tabRooms[i, j].DistanceFrom(y, x) > max)
                    {
                        max = tabRooms[i, j].DistanceFrom(y, x);
                        position[0] = i; position[1] = j;
                    }
                    // If the distance is equal to max, save new position if the has haas less neightbours (prefer a more isolated room)
                    else if (tabRooms[i, j].DistanceFrom(y, x) == max && tabRooms[i, j].NumberOfNeighbors < tabRooms[position[0], position[1]].NumberOfNeighbors)
                    {
                        position[0] = i; position[1] = j;
                    }
                }
            }
        }
        return position;
    }


    // Add an item room to the map, replacing a standart room
    void AddItemRoom()
    {
        int i, j;
        // Generate random positions until it finds a standart room
        do {
            i = Random.Range(0, roomGridY - 1);
            j = Random.Range(0, roomGridX - 1);
        } while (tabRooms[i, j] == null || tabRooms[i, j].GetType() != typeof(StandartRoom) || tabRooms[i, j].NumberOfNeighbors > 2);
       
        // Replace the room with an item room
        tabRooms[i,j] = new ItemRoom(roomWidth, roomHeight, (j * tileWidth) * roomWidth, (i * tileHeight) * roomHeight);
        ConnectToExistingRooms(i, j);
    }

    // Check for doors in neightbors room for possible connections.
    void ConnectToExistingRooms(int i, int j)
    {
        // Connect the room at the given position if the rooms around have a door for it.
        if (!OutOfBounds(i + 1, j) && tabRooms[i + 1, j] != null && tabRooms[i + 1, j].getDoor(1))
            tabRooms[i, j].ConnectRoom(tabRooms[i + 1, j]);

        if (!OutOfBounds(i - 1, j) && tabRooms[i - 1, j] != null && tabRooms[i - 1, j].getDoor(0))
            tabRooms[i, j].ConnectRoom(tabRooms[i - 1, j]);

        if (!OutOfBounds(i, j + 1) && tabRooms[i, j + 1] != null && tabRooms[i, j + 1].getDoor(2))
            tabRooms[i, j].ConnectRoom(tabRooms[i, j + 1]);

        if (!OutOfBounds(i, j - 1) && tabRooms[i, j - 1] != null && tabRooms[i, j - 1].getDoor(3))
            tabRooms[i, j].ConnectRoom(tabRooms[i, j - 1]);
    }

    // check if any room in the grid has too many neightbours
    bool TooManyNeighbours()
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
                    if (!OutOfBounds(i + 1, j) && (tabRooms[i + 1, j] != null))
                        roomCount++;
                    if (!OutOfBounds(i - 1, j) && (tabRooms[i - 1, j] != null))
                        roomCount++;
                    if (!OutOfBounds(i, j + 1) && (tabRooms[i, j + 1] != null))
                        roomCount++;
                    if (!OutOfBounds(i, j - 1) && (tabRooms[i, j - 1] != null))
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
                    tabRooms[i, j].NumberOfNeighbors = updatedNeighbors[i, j];
            }
        }
        // Return false if all room have a number of neightbours less or equal to max value
        return false;
    }

    // Get every room to be instanciated on the map
    void PrintRooms()
    {
        for (int i = 0; i < roomGridY; i++)
        {
            for (int j = 0; j < roomGridX; j++)
            {
                // Send the room to be instanciated if the given position is a room
                if (tabRooms[i, j] != null)
                {
                    GameObject roomClone = new GameObject(tabRooms[i, j].GetType().ToString() + "[" + i + "," + j + "]");
                    roomClone.transform.parent = transform;
                    roomClone.transform.position = new Vector3((roomWidth * tileWidth) * j, (roomHeight * tileHeight) * i, 0);
                    InstanciateTiles(tabRooms[i, j], roomClone);
                }
            }
        }
    }

    // Instanciate every tile as a clone of the original prefab
    void InstanciateTiles(Room room, GameObject roomClone)
    {
        // Create empty gameObjects as parent for each type of tiles
        GameObject floors = new GameObject("Floors");
        floors.transform.parent = roomClone.transform;
        floors.transform.position = new Vector3(room.GridPosX, room.GridPosY, 0);

        GameObject doors = new GameObject("Doors");
        doors.transform.parent = roomClone.transform;
        doors.transform.position = new Vector3(room.GridPosX, room.GridPosY, 0);

        GameObject walls = new GameObject("Walls");
        walls.transform.position = new Vector3(room.GridPosX, room.GridPosY, 0);
        walls.transform.parent = roomClone.transform;

        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                // Check for the tile Char in order to instanciate the correct object
                switch (room.getTile(i, j))
                {
                    // Spawn a floor
                    case "Floor":
                        // Create a clone from a random prefab from the list.
                        GameObject floorClone = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        floorClone.name = "Floor[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        floorClone.transform.parent = floors.transform;
                        break;
                    // Spawn a hero (spawnpoint) and a floor tile under it
                    case "Hero":
                        // Move spawnpoint location
                        SpawnPoint.transform.position = new Vector3(room.GridPosX + j, room.GridPosY + i, 0);
                        // Create a clone from a random prefab from the list.
                        GameObject playerTile = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        playerTile.name = "Floor[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        playerTile.transform.parent = floors.transform;
                        break;
                    // Spawn an item
                    case "Item":
                        GameObject itemTile = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        itemTile.name = "Floor[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        itemTile.transform.parent = floors.transform;

                        // Create a clone from the item prefab.
                        GameObject itemClone = (GameObject)Instantiate(item, new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        itemClone.name = "Item";
                        // Mark the clone as a child of the current GameObject
                        itemClone.transform.parent = roomClone.transform;
                        break;
                    case "WallT":
                        // Create a clone from the original prefab.
                        GameObject wallTClone = (GameObject)Instantiate(wallList[0], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        wallTClone.name = "Wall[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        wallTClone.transform.parent = walls.transform;
                        break;
                    case "WallB":
                        // Create a clone from the original prefab.
                        GameObject wallBClone = (GameObject)Instantiate(wallList[1], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        wallBClone.name = "Wall[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        wallBClone.transform.parent = walls.transform;
                        break;
                    case "WallL":
                        // Create a clone from the original prefab.
                        GameObject wallLClone = (GameObject)Instantiate(wallList[2], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        wallLClone.name = "Wall[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        wallLClone.transform.parent = walls.transform;
                        break;
                    case "WallR":
                        // Create a clone from the original prefab.
                        GameObject wallRClone = (GameObject)Instantiate(wallList[3], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        wallRClone.name = "Wall[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        wallRClone.transform.parent = walls.transform;
                        break;
                    case "CornerBL":
                        // Create a clone from the original prefab.
                        GameObject cornerBLClone = (GameObject)Instantiate(cornerList[2], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        cornerBLClone.name = "Corner[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        cornerBLClone.transform.parent = walls.transform;
                        break;
                    case "CornerBR":
                        // Create a clone from the original prefab.
                        GameObject cornerBRClone = (GameObject)Instantiate(cornerList[3], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        cornerBRClone.name = "Corner[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        cornerBRClone.transform.parent = walls.transform;
                        break;
                    case "CornerTL":
                        // Create a clone from the original prefab.
                        GameObject cornerTLClone = (GameObject)Instantiate(cornerList[0], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        cornerTLClone.name = "Corner[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        cornerTLClone.transform.parent = walls.transform;
                        break;
                    case "CornerTR":
                        // Create a clone from the original prefab.
                        GameObject cornerTRClone = (GameObject)Instantiate(cornerList[1], new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0), Quaternion.identity);
                        cornerTRClone.name = "Corner[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        cornerTRClone.transform.parent = walls.transform;
                        break;
                    case "DoorT":
                        // Create a clone from the original prefab.
                        GameObject doorTClone = (GameObject)Instantiate(doorList[0]);
                        doorTClone.transform.position = new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0);
                        doorTClone.name = "Door[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        doorTClone.transform.parent = doors.transform;
                        break;
                    case "DoorB":
                        // Create a clone from the original prefab.
                        GameObject doorBClone = (GameObject)Instantiate(doorList[1]);
                        doorBClone.transform.position = new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0);
                        doorBClone.name = "Door[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        doorBClone.transform.parent = doors.transform;
                        break;
                    case "DoorL":
                        // Create a clone from the original prefab.
                        GameObject doorLClone = (GameObject)Instantiate(doorList[2]);
                        doorLClone.transform.position = new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0);
                        doorLClone.name = "Door[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        doorLClone.transform.parent = doors.transform;
                        break;
                    case "DoorR":
                        // Create a clone from the original prefab.
                        GameObject doorRClone = (GameObject)Instantiate(doorList[3]);
                        doorRClone.transform.position = new Vector3(room.GridPosX + (j * tileWidth), room.GridPosY + (i * tileHeight), 0);
                        doorRClone.name = "Door[" + i + "," + j + "]";
                        // Mark the clone as a child of the current GameObject
                        doorRClone.transform.parent = doors.transform;
                        break;

                }
            }
        }
    }

    // Change playerand cameras positions to match the spawnpoint's position
    void SpawnPlayer()
    {
        player.transform.position = SpawnPoint.transform.position;
        mainCam.transform.position = SpawnPoint.transform.position;
        minimapCam.transform.position = SpawnPoint.transform.position;
    }

    // Check if the given position is out of bounds in the map.
    bool OutOfBounds(int i, int j)
    {
        if (i < 0 || i >= roomGridY || j < 0 || j >= roomGridX)
            return true;
        return false;
    }
}