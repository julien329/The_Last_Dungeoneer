using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

    public GameObject panel;
    public GameObject door;
    public Transform player;

    private string[,] minimap;
    private GameObject[,] tabPanels;
    private int playerPositionX;
    private int playerPositionY;

    void Start ()
    {
        InstanciatePanels();
        InitialiseMinimap();
    }
	
	void Update ()
    {
        // Get player position in the room grid.
        playerPositionX = (int)(player.position.x / BaseMap.roomWidth);
        playerPositionY = (int)(player.position.y / BaseMap.roomHeight);

        // If player position is a not visited room, and he is not int a inexisting room, update the minimap
        if (minimap[playerPositionY, playerPositionX] != "fullVisible" && BaseMap.tabRooms[playerPositionY, playerPositionX] != null)
        {
            revealNeighbors(playerPositionY, playerPositionX);
            revealDoors(playerPositionY, playerPositionX);
        }
	}

    // Scan tabRoom array to know the map layout and store it in minimap array as notVisible
    void InitialiseMinimap()
    {
        // Declare array same size a tabRoom array.
        minimap = new string[BaseMap.roomGridY, BaseMap.roomGridX];

        // Scan map for existing rooms
        for (int i = 0; i < BaseMap.roomGridY; i++)
        {
            for(int j = 0; j < BaseMap.roomGridX; j++)
            {
                if(BaseMap.tabRooms[i,j] != null)
                    minimap[i, j] = "notVisible";
                else
                    minimap[i, j] = "null";
            }
        }
        //Reveal neightbors and door of the starting room in the middle of the map
        revealNeighbors((BaseMap.roomGridY / 2), (BaseMap.roomGridX / 2));
        revealDoors((BaseMap.roomGridY / 2), (BaseMap.roomGridX / 2));
    }

    // Reveal neighbors of a room in grey color on the minimap
    void revealNeighbors(int i, int j)
    {
        // Get current room and set it to full visible and white color.
        tabPanels[i, j].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        minimap[i, j] = "fullVisible";

        //Check for room above if there is a door connecting with it. If so, set to semiVisible and grey color.
        if ((i + 1 < BaseMap.roomGridX) && (BaseMap.tabRooms[i, j].getDoor(0)) && (BaseMap.tabRooms[i + 1, j] != null) && (minimap[i + 1, j] == "notVisible"))
        {
            minimap[i + 1, j] = "semiVisible";
            tabPanels[i + 1, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        //Check for room under if there is a door connecting with it. If so, set to semiVisible and grey color.
        if ((i - 1 >= 0) && (BaseMap.tabRooms[i, j].getDoor(1)) && (BaseMap.tabRooms[i - 1, j] != null) && (minimap[i - 1, j] == "notVisible"))
        {
            minimap[i - 1, j] = "semiVisible";
            tabPanels[i - 1, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        //Check for room to the right if there is a door connecting with it. If so, set to semiVisible and grey color.
        if ((j + 1 < BaseMap.roomGridY) && (BaseMap.tabRooms[i, j].getDoor(3)) && (BaseMap.tabRooms[i, j + 1] != null) && (minimap[i, j + 1] == "notVisible"))
        {
            minimap[i, j + 1] = "semiVisible";
            tabPanels[i, j + 1].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        //Check for room to the left if there is a door connecting with it. If so, set to semiVisible and grey color.
        if ((j - 1 >= 0) && (BaseMap.tabRooms[i, j].getDoor(2)) && (BaseMap.tabRooms[i, j - 1] != null) && (minimap[i, j - 1] == "notVisible"))
        {
            minimap[i, j - 1] = "semiVisible";
            tabPanels[i, j - 1].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    // Reveal doors of a room on the minimap
    void revealDoors(int i, int j)
    {
        // Check for door up. If so, instanciate the door on the minimap and set it as child of this object.
        if (BaseMap.tabRooms[i, j].getDoor(0))
        {
            GameObject doorClone = (GameObject)Instantiate(door, new Vector3((j * BaseMap.roomWidth) + (BaseMap.roomWidth / 2), ((i + 1) * BaseMap.roomHeight) - 0.5f, 0), Quaternion.identity);
            doorClone.transform.localScale = new Vector3(4, 2);
            doorClone.transform.parent = transform;
        }
        // Check for door down. If so, instanciate the door on the minimap and set it as child of this object.
        if (BaseMap.tabRooms[i, j].getDoor(1))
        {
            GameObject doorClone = (GameObject)Instantiate(door, new Vector3((j * BaseMap.roomWidth) + (BaseMap.roomWidth / 2), (i * BaseMap.roomHeight) - 0.5f, 0), Quaternion.identity);
            doorClone.transform.localScale = new Vector3(4, 2);
            doorClone.transform.parent = transform;
        }
        // Check for door left. If so, instanciate the door on the minimap and set it as child of this object.
        if (BaseMap.tabRooms[i, j].getDoor(2))
        {
            GameObject doorClone = (GameObject)Instantiate(door, new Vector3((j * BaseMap.roomWidth) - 0.5f , (i * BaseMap.roomHeight) + (BaseMap.roomHeight / 2), 0), Quaternion.identity);
            doorClone.transform.localScale = new Vector3(2, 4);
            doorClone.transform.parent = transform;
        }
        // Check for door right. If so, instanciate the door on the minimap and set it as child of this object.
        if (BaseMap.tabRooms[i, j].getDoor(3))
        {
            GameObject doorClone = (GameObject)Instantiate(door, new Vector3(((j + 1) * BaseMap.roomWidth) - 0.5f, (i * BaseMap.roomHeight) + (BaseMap.roomHeight / 2), 0), Quaternion.identity);
            doorClone.transform.localScale = new Vector3(2, 4);
            doorClone.transform.parent = transform;
        }
    }

    // Instanciate minimap room panels
    void InstanciatePanels()
    {
        // Declare array of panel corresponding to the layout of the real map
        tabPanels = new GameObject[BaseMap.roomGridY, BaseMap.roomGridX];

        // Scan the real map for existing rooms
        for (int i = 0; i < BaseMap.roomGridY; i++)
        {
            for (int j = 0; j < BaseMap.roomGridX; j++)
            {
                // If a room is found, instanciate the panel at correct position, set it as children of this object and store it in the array for future use.
                if (BaseMap.tabRooms[i, j] != null)
                {
                    GameObject panelClone = (GameObject)Instantiate(panel, new Vector3((j * BaseMap.roomWidth) + (BaseMap.roomWidth / 2), (i * BaseMap.roomHeight) + (BaseMap.roomHeight / 2), 0), Quaternion.identity);
                    panelClone.transform.localScale = new Vector3(BaseMap.roomWidth - 1, BaseMap.roomHeight - 1, 1);
                    panelClone.transform.parent = transform;
                    tabPanels[i, j] = panelClone;
                }
            }
        }
    }
}
