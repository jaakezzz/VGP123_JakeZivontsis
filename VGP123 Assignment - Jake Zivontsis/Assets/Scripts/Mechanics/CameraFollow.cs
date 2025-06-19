using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    public Transform player; // Reference to the player's transform

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


        if (player.position.y < -3.2f)
        {
            pos.y = Mathf.Clamp(player.position.y + 2f, -1000f, 1000f);
        }
        else pos.y = 1.25f;

        if (player.position.y < -14f)
        {
            pos.y = -17.8f;
        }

        // Update the camera's position to follow the player
        transform.position = pos;
    }
}
