using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.transform.position = new Vector3(0.0f, -6.0f, 0.0f);
        }
    }
}
