using UnityEngine;

public class AudioFollow : MonoBehaviour
{

    public Transform player; // Reference to the player's transform

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerControllerCreated -= SetPlayerRef;
    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        player = playerInstance.transform;
        return playerInstance;
    }

    void Update()
    {
        if (player == null) return;

        // Update the audio listener's position to follow the player
        transform.position = player.transform.position;
    }
}
