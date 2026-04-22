using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    [Header("Patrol Bounds")]
    public Transform leftBound;
    public Transform rightBound;

    private TurretEnemy turret;
    private Transform player;

    private bool engagingPlayer = false;
    private bool movingRight = true;

    private void Start()
    {
        Debug.Log("TurretMovement active");

        turret = GetComponent<TurretEnemy>();

        GameManager.Instance.OnPlayerControllerCreated += SetPlayerRef;

        if (GameManager.Instance.PlayerInstance != null)
            SetPlayerRef(GameManager.Instance.PlayerInstance);

        Debug.Log($"Patrolling between {leftBound} and {rightBound}");

        if (player == null)
        {
            Debug.LogWarning("TurretMovement: PlayerTransform is null!");
            return;
        }
    }

    private PlayerController SetPlayerRef(PlayerController playerInstance)
    {
        player = playerInstance.transform;
        Debug.Log("TurretMovement received player reference.");
        return playerInstance;
    }

    private void Update()
    {
        if (turret == null || turret.IsDead || player == null) return;

        float playerX = player.position.x;
        float myX = transform.position.x;

        bool playerInPatrol = playerX >= leftBound.position.x && playerX <= rightBound.position.x;
        float distance = Mathf.Abs(playerX - myX);

        engagingPlayer = playerInPatrol || distance <= turret.range;

        if (engagingPlayer)
        {
            MaintainDistance(playerX, myX);
        }
        else
        {
            Patrol();
        }

        // Flip sprite using shared reference
        turret.Sprite.flipX = (playerX < myX);
    }

    void MaintainDistance(float playerX, float myX)
    {
        float targetDistance = turret.range - 1f;
        float leftX = leftBound.position.x;
        float rightX = rightBound.position.x;

        // Two possible target positions: one on each side of the player
        float targetLeft = playerX - targetDistance;
        float targetRight = playerX + targetDistance;

        // Check which target is within bounds and closer to current position
        bool leftInBounds = targetLeft >= leftX && targetLeft <= rightX;
        bool rightInBounds = targetRight >= leftX && targetRight <= rightX;

        float chosenTargetX = myX;

        if (leftInBounds && rightInBounds)
        {
            // Pick whichever is closer
            chosenTargetX = Mathf.Abs(myX - targetLeft) < Mathf.Abs(myX - targetRight)
                ? targetLeft
                : targetRight;
        }
        else if (leftInBounds)
        {
            chosenTargetX = targetLeft;
        }
        else if (rightInBounds)
        {
            chosenTargetX = targetRight;
        }
        else
        {
            // No valid position—don’t move
            return;
        }

        // Move toward the chosen target
        float moveDirection = Mathf.Sign(chosenTargetX - myX);
        transform.Translate(Vector2.right * moveDirection * turret.MoveSpeed * Time.deltaTime);
    }



    void Patrol()
    {
        float myX = transform.position.x;
        float leftX = leftBound.position.x;
        float rightX = rightBound.position.x;

        float direction = movingRight ? 1f : -1f;
        transform.Translate(Vector2.right * direction * turret.MoveSpeed * Time.deltaTime);

        if (movingRight && myX >= rightX)
            movingRight = false;
        else if (!movingRight && myX <= leftX)
            movingRight = true;
    }
}
