using UnityEngine;

public class PlayerDog : PlayerController
{
    [Header("Settings")]
    [SerializeField] private float damage;
    [SerializeField] private float distanceToGrabHuman = 2.0f;
    [SerializeField] private int stunHit = 3;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float stunRecoveryTime = 3f;

    private bool isGrabbed;

    private bool IsCloseToHuman => GameController.Instance.DistanceBetweenPlayers < distanceToGrabHuman;
    private PlayerHuman PlayerHuman => GameController.Instance.HumanPlayer as PlayerHuman;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isGrabbed)
                Release();
            else if (IsCloseToHuman)
                Grab();
        }
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
    }

    public void Grab()
    {
        transform.position = (Vector2)PlayerHuman.transform.position + Vector2.up * 1.5f;

        transform.SetParent(PlayerHuman.transform);
        isGrabbed = true;

        rig.bodyType = RigidbodyType2D.Kinematic;
        rig.linearVelocity = Vector2.zero;

        movementBehaviour.CanMove = false;
        movementBehaviour.CanJump = false;

    }

    public void Release()
    {
        transform.SetParent(null);
        isGrabbed = false;

        rig.bodyType = RigidbodyType2D.Dynamic;
        
        movementBehaviour.CanMove = true;
        movementBehaviour.CanJump = true;
    }
}
