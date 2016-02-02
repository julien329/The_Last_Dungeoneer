using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.

    int playerPositionX;
    int playerPositionY;
    int caseX;
    int caseY;
    Vector3 newPosition;

    void Start()
    {
        // Calculate the initial offset.
        offset = new Vector3(0, 0, -1);

        caseX = (int)(target.position.x / BaseMap.roomWidth);
        caseY = (int)(target.position.y / BaseMap.roomHeight);
    }

    void FixedUpdate()
    {
        playerPositionX = (int)(target.position.x / BaseMap.roomWidth);
        playerPositionY = (int)(target.position.y / BaseMap.roomHeight);

        if (playerPositionX != caseX || playerPositionY != caseY)
        {
            caseX = playerPositionX;
            caseY = playerPositionY;
            newPosition = new Vector3(caseX * BaseMap.roomWidth + BaseMap.roomWidth/2, caseY * BaseMap.roomHeight + BaseMap.roomHeight / 2, transform.position.z);
            transform.position = newPosition;
        }

        // Create a postion the camera is aiming for based on the offset from the target.
        //Vector3 targetCamPos = target.position + offset;
        // Smoothly interpolate between the camera's current position and it's target position.
        //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

    }
}
