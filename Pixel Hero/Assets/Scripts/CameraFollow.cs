﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    private Vector3 offset;                     // The initial offset from the target.
    private  Vector3 newPosition;
    private Camera cam;

    private int playerPositionX;
    private int playerPositionY;
    private float height;
    private float width;

    void Start()
    {
        // Calculate the initial offset.
        offset = new Vector3(0, 0, -1);
        // Initialize camera
        cam = Camera.main;
        // Get orthographic camera height and width
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }

    void FixedUpdate()
    {
        // Position of the player in the room grid of the map
        playerPositionX = (int)(target.position.x / BaseMap.roomWidth);
        playerPositionY = (int)(target.position.y / BaseMap.roomHeight);

        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position + offset;

        // If a border of the camera collides with the room limit, stop following the player in the colliding direction
        // Compare the position of the player with the limits of the room + half the camera dimensions, as the center of the camera follows the player.
        if (target.position.x <= (playerPositionX * BaseMap.roomWidth) + (width / 2))
            targetCamPos.x = (playerPositionX * BaseMap.roomWidth) + (width / 2);

        if (target.position.x > ((playerPositionX + 1) * BaseMap.roomWidth) - (width / 2) - 1)
            targetCamPos.x = ((playerPositionX + 1) * BaseMap.roomWidth) - (width / 2) - 1;

        if (target.position.y <= (playerPositionY * BaseMap.roomHeight) + (height / 2))
            targetCamPos.y = (playerPositionY * BaseMap.roomHeight) + (height / 2);

        if (target.position.y > ((playerPositionY + 1) * BaseMap.roomHeight) - (height / 2) - 1)
            targetCamPos.y = ((playerPositionY + 1) * BaseMap.roomHeight) - (height / 2) - 1;


        // If the room is smaller than the camera size, focus on the middle of the room
        if (BaseMap.roomWidth < width)
            targetCamPos.x = playerPositionX * BaseMap.roomWidth + BaseMap.roomWidth / 2;

        if(BaseMap.roomHeight < height)
            targetCamPos.y = playerPositionY * BaseMap.roomHeight + BaseMap.roomHeight / 2;

   
        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
