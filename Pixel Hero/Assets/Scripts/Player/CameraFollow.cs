using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.
    public Transform minimap;

    private Vector3 offset;                     // The initial offset from the target.
    private Vector3 targetCamPos;
    private Camera cam;
    private BaseMap mapSettings;

    private int playerPositionX;
    private int playerPositionY;
    private float height;
    private float width;
    private float roomWidth;
    private float roomHeight;

    void Start()
    {
        // Calculate the initial offset.
        offset = new Vector3(0, 0, -1);
        // Initialize camera
        cam = Camera.main;
        // Get orthographic camera height and width
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        mapSettings = GameObject.Find("Map").GetComponent<BaseMap>();
        roomWidth = mapSettings.roomWidth * mapSettings.tileWidth;
        roomHeight = mapSettings.roomHeight * mapSettings.tileHeight;
    }

    void Update()
    {
        // Position of the player in the room grid of the map
        playerPositionX = (int)(target.position.x / roomWidth);
        playerPositionY = (int)(target.position.y / roomHeight);

        // Create a postion the camera is aiming for based on the offset from the target.
        targetCamPos = target.position + offset;

        // If the room width is smaller than the camera width, focus on the middle of the room in x.
        if (roomWidth <= width)
            targetCamPos.x = playerPositionX * roomWidth + roomWidth / 2f - 1f;
        else
        {
            // If the border of the camera collides with the room limit, stop following the player in the x colliding direction
            // Compare the position of the player with the limits of the room + half the camera width, as the center of the camera follows the player.
            if (target.position.x <= (playerPositionX * roomWidth) + (width / 2f) - 1)
                targetCamPos.x = (playerPositionX * roomWidth) + (width / 2f) - 1;

            if (target.position.x > ((playerPositionX + 1) * roomWidth) - (width / 2f) - 1)
                targetCamPos.x = ((playerPositionX + 1) * roomWidth) - (width / 2f) - 1;
        }

        // If the room height is smaller than the camera height, focus on the middle of the room in y.
        if (roomHeight <= height)
            targetCamPos.y = playerPositionY * roomHeight + roomHeight / 2f - 1f;
        else
        {
            // If the border of the camera collides with the room limit, stop following the player in the y colliding direction
            // Compare the position of the player with the limits of the room + half the camera height, as the center of the camera follows the player.
            if (target.position.y <= (playerPositionY * roomHeight) + (height / 2f) - 1)
                targetCamPos.y = (playerPositionY * roomHeight) + (height / 2f) - 1;

            if (target.position.y > ((playerPositionY + 1) * roomHeight) - (height / 2f) - 1)
                targetCamPos.y = ((playerPositionY + 1) * roomHeight) - (height / 2f) - 1;
        }
   
        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

        // Minimap follows main camera
        minimap.transform.position = transform.position;
    }
}
