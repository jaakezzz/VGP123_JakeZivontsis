using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    public Transform player; // Reference to the player's transform

    void Update()
    {
        if (player == null) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(player.position.x + 3.5f, minX, maxX);

        // Update the camera's position to follow the player
        transform.position = pos;
    }
}
