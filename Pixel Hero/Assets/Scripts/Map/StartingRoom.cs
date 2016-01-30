using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartingRoom : Room {

    public override void SetAttributes(int width, int height, int x, int y)
    {
        roomWidth = width;
        roomHeight = height;
        gridPosX = x;
        gridPosY = y;
        isNull = true;

        CreateRoom();
    }

    public override void CreateRoom()
    {
        for (int i = 0; i < roomHeight; i++)
        {
            List<int> subList = new List<int>();
            for (int j = 0; j < roomWidth; j++)
            {
                int tile = 0;
                placeFloor(ref tile);
                placeBounds(ref tile, j);
                placeDoors(ref tile, j);
                placeCharacters(ref tile, j);
                placeObjects(ref tile, j);

                subList.Add(tile);
            }
            tabTiles.Add(subList);
        }
        isNull = false;
    }

    public override void SpawnRoom(List<GameObject> floorList, List<GameObject> borderList, GameObject SpawnPoint)
    {
        for (int i = 0; i < roomHeight; i++)
        {
            for (int j = 0; j < roomWidth; j++)
            {
                switch (tabTiles[i][j])
                {
                    case 2:
                    case 0:
                        GameObject floorClone = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(gridPosX + j, gridPosY + i, 0), Quaternion.identity);
                        floorClone.transform.parent = transform;
                        break;
                    case 1:
                        GameObject boundClone = (GameObject)Instantiate(borderList[Random.Range(0, borderList.Count - 1)], new Vector3(gridPosX + j, gridPosY + i, 0), Quaternion.identity);
                        boundClone.transform.parent = transform;
                        break;
                    case 4:
                        SpawnPoint.transform.position = new Vector3(gridPosX + j, gridPosY + i, 0);
                        GameObject playerTile = (GameObject)Instantiate(floorList[Random.Range(0, floorList.Count - 1)], new Vector3(gridPosX + j, gridPosY + i, 0), Quaternion.identity);
                        playerTile.transform.parent = transform;
                        break;
                }
            }
        }
    }

    private void placeFloor(ref int tile)
    {
        tile = 0;
    }

    private void placeBounds(ref int tile, int j)
    {
        if (tabTiles.Count == 0 || tabTiles.Count == roomHeight - 1 || j == 0 || j == roomWidth - 1)
            tile = 1;
    }

    private void placeDoors(ref int tile, int j)
    {
        if (tabTiles.Count == roomHeight / 2)
            if (j == 0 || j == roomWidth - 1)
                tile = 2;
        if (j == roomWidth / 2)
            if (tabTiles.Count == 0 || tabTiles.Count == roomHeight - 1)
                tile = 2;
    }

    private void placeCharacters(ref int tile, int j)
    {
        if (j == roomWidth / 2 && tabTiles.Count == (roomHeight / 2) - 1)
            tile = 4;
    }

    private void placeObjects(ref int tile, int j)
    {

    }
}
