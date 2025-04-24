using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset between the camera and the player

    void Update()
    {
        // Update the camera's position to follow the player
        transform.position = player.position + offset;
    }
}
