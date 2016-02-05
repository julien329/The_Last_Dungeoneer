using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {

    public GameObject panel;
    public GameObject door;
    public Transform player;

    private string[,] minimap;
    private GameObject[,] tabPanels;
    private bool needUpdate = true;
    private int playerPositionX;
    private int playerPositionY;

    void Start ()
    {
        InstanciatePanels();
        InitialiseMinimap();
	}
	
	void Update ()
    {
        playerPositionX = (int)(player.position.x / BaseMap.roomWidth);
        playerPositionY = (int)(player.position.y / BaseMap.roomHeight);

        if (minimap[playerPositionY, playerPositionX] != "fullVisible")
            needUpdate = true;

        if (needUpdate)
        {
            revealNeighbors(playerPositionY, playerPositionX);
            revealDoors(playerPositionY, playerPositionX);
            needUpdate = false;
        }
	}

    void InitialiseMinimap()
    {
        minimap = new string[BaseMap.roomGridY, BaseMap.roomGridX];

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

        minimap[(BaseMap.roomGridY / 2), (BaseMap.roomGridX / 2)] = "fullVisible";
    }

    void revealNeighbors(int i, int j)
    {
        Room room = BaseMap.tabRooms[i, j];
        if(tabPanels[i, j] != null)
            tabPanels[i, j].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        minimap[i, j] = "fullVisible";

        if (BaseMap.tabRooms[i, j] != null)
        {
            if ((i + 1 < BaseMap.roomGridX) && (BaseMap.tabRooms[i, j].getDoor(0)) && (BaseMap.tabRooms[i + 1, j] != null) && (minimap[i + 1, j] == "notVisible"))
            {
                minimap[i + 1, j] = "semiVisible";
                tabPanels[i + 1, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            }

            if ((i - 1 >= 0) && (BaseMap.tabRooms[i, j].getDoor(1)) && (BaseMap.tabRooms[i - 1, j] != null) && (minimap[i - 1, j] == "notVisible"))
            {
                minimap[i - 1, j] = "semiVisible";
                tabPanels[i - 1, j].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            }

            if ((j + 1 < BaseMap.roomGridY) && (BaseMap.tabRooms[i, j].getDoor(3)) && (BaseMap.tabRooms[i, j + 1] != null) && (minimap[i, j + 1] == "notVisible"))
            {
                minimap[i, j + 1] = "semiVisible";
                tabPanels[i, j + 1].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            }

            if ((j - 1 >= 0) && (BaseMap.tabRooms[i, j].getDoor(2)) && (BaseMap.tabRooms[i, j - 1] != null) && (minimap[i, j - 1] == "notVisible"))
            {
                minimap[i, j - 1] = "semiVisible";
                tabPanels[i, j - 1].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    void revealDoors(int i, int j)
    {
        if (BaseMap.tabRooms[i, j].getDoor(0))
        {
            GameObject doorClone = (GameObject)Instantiate(door, new Vector3((j * BaseMap.roomWidth) + (BaseMap.roomWidth / 2), ((i + 1) * BaseMap.roomHeight) - 0.5f, 0), Quaternion.identity);
            doorClone.transform.localScale = new Vector3(4, 2);
            doorClone.transform.parent = transform;
        }
    }

    void InstanciatePanels()
    {
        tabPanels = new GameObject[BaseMap.roomGridY, BaseMap.roomGridX];

        for (int i = 0; i < BaseMap.roomGridY; i++)
        {
            for (int j = 0; j < BaseMap.roomGridX; j++)
            {
                if (BaseMap.tabRooms[i, j] != null)
                {
                    GameObject panelClone = (GameObject)Instantiate(panel, new Vector3((j * BaseMap.roomWidth) + (BaseMap.roomWidth / 2), (i * BaseMap.roomHeight) + (BaseMap.roomHeight / 2), 0), Quaternion.identity);
                    panelClone.transform.localScale = new Vector3(BaseMap.roomWidth - 1, BaseMap.roomHeight - 1, 1);
                    panelClone.transform.parent = transform;
                    tabPanels[i, j] = panelClone;
                }
            }
        }
        tabPanels[(BaseMap.roomGridY / 2), (BaseMap.roomGridX / 2)].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }
}
