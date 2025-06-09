using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Life,
        Powerup,
        Score
    }

    public PickupType pickupType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            switch (pickupType)
            {
                case PickupType.Life:
                    GameManager.Instance.Lives++;
                    break;
                case PickupType.Powerup:
                    pc.SpeedChange();
                    break;
                case PickupType.Score:
                    pc.score += 10;
                    break;
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            switch (pickupType)
            {
                case PickupType.Life:
                    GameManager.Instance.Lives++;
                    break;
                case PickupType.Powerup:
                    pc.SpeedChange();
                    break;
                case PickupType.Score:
                    pc.score += 10;
                    break;
            }

            Destroy(gameObject);
        }
    }
}
