using UnityEngine;
using System.Collections;

public enum Cardinalite { NORTH, EAST, SOUTH, WEST }

public class MoveNextRoom : MonoBehaviour {

    public Cardinalite cardinalite;
    private float travelDistance = 3.25f;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.tag == "Player")
            SkipDoor();
    }

    void SkipDoor()
    {
        switch (cardinalite)
        {
            case Cardinalite.NORTH :
                Vector3 newPosUp = new Vector3(player.transform.position.x, player.transform.position.y + travelDistance);
                player.transform.position = newPosUp;
                break;

            case Cardinalite.SOUTH :
                Vector3 newPosDown = new Vector3(player.transform.position.x, player.transform.position.y - travelDistance);
                player.transform.position = newPosDown;
                break;
            case Cardinalite.EAST:
                Vector3 newPosRight = new Vector3(player.transform.position.x + travelDistance, player.transform.position.y);
                player.transform.position = newPosRight;
                break;

            case Cardinalite.WEST:
                Vector3 newPosLeft = new Vector3(player.transform.position.x - travelDistance, player.transform.position.y);
                player.transform.position = newPosLeft;
                break;
        }
    }
}
