using System.Threading.Tasks;
using UnityEngine;

public class PlayerDog : PlayerController
{
    [Header("Settings")]
    [SerializeField] private float distanceToGrabHuman = 2.0f;
    [SerializeField] private int amountHitsToStun = 3;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float stunRecoveryTime = 3f;

    private bool isGrabbed;
    private int currentHits;
    private float stunRecoveryTimer;

    private bool IsCloseToHuman => GameController.Instance.DistanceBetweenPlayers < distanceToGrabHuman;
    private PlayerHuman PlayerHuman => GameController.Instance.HumanPlayer as PlayerHuman;
    public bool IsStunned {get; private set;}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGrabbed)
                Release();
            else if (IsCloseToHuman)
                Grab();
        }

        RecoveryFromHit();
    }

    public void Attack()
    {

    }

    public void Bark()
    {

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        currentHits++;

        if (currentHits >= amountHitsToStun)
        {
            Stun();
        }
    }

    private async void Stun()
    {
        currentHits = 0;
        stunRecoveryTimer = 0;
        IsStunned = true;

        DisableMovement();

        await Task.Delay((int)(stunDuration * 1000));

        IsStunned = false;
        EnableMovement();
    }

    private void RecoveryFromHit()
    {
        if (currentHits <= 0) return;

        stunRecoveryTimer += Time.deltaTime;

        if (stunRecoveryTimer >= stunRecoveryTime)
        {
            currentHits = Mathf.Max(0, currentHits - 1);
            stunRecoveryTimer = 0f;
        }
    }

    public void Grab()
    {
        transform.position = (Vector2)PlayerHuman.transform.position + Vector2.up * 1.5f;

        transform.SetParent(PlayerHuman.transform);
        isGrabbed = true;

        DisableMovement();
    }

    public void Release()
    {
        transform.SetParent(null);
        isGrabbed = false;

        EnableMovement();
    }

    private void EnableMovement()
    {
        rig.bodyType = RigidbodyType2D.Dynamic;
        movementBehaviour.CanMove = true;
        movementBehaviour.CanJump = true;
    }

    private void DisableMovement()
    {
        rig.bodyType = RigidbodyType2D.Kinematic;
        rig.linearVelocity = Vector2.zero;

        movementBehaviour.CanMove = false;
        movementBehaviour.CanJump = false;
    }
}
