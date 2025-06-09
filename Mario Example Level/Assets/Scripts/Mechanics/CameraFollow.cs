using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private Transform player; // Reference to the player's transform

    private void Awake()
    {
        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;
    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        player = playerInstance.transform;
        return playerInstance;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(player.position.x + 3.5f, minX, maxX);

        // Update the camera's position to follow the player
        transform.position = pos;
    }
}
