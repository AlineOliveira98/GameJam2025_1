using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private const string RunAnim = "IsRunning";
    private const string AttackAnim = "IsAttacking";

    [Header("Settings")]
    [SerializeField] private float speedMovement = 2f;
    [SerializeField] private float attackRate = 1f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;

    private PlayerController HumanPlayer;
    private PlayerController DogPlayer;
    private Vector2 direction;
    private float distanceHuman => Mathf.Abs(HumanPlayer.transform.position.x - transform.position.x);
    private float distanceDog => Mathf.Abs(DogPlayer.transform.position.x - transform.position.x);
    private bool followPlayer;
    private float attackTimer;
    private bool isAttacking;

    void Start()
    {
        HumanPlayer = GameController.Instance.HumanPlayer;
        DogPlayer = GameController.Instance.DogPlayer;
    }

    void Update()
    {
        if (CanAttack())
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
            FollowMovement();
        }

        if (isAttacking)
        {
            if (attackTimer <= 0)
            {
                attackTimer = attackRate;
                Attack();
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    private bool CanAttack()
    {
        if (distanceHuman > 3f && distanceDog > 3f) return false;
        if (distanceDog < 3 && (DogPlayer as PlayerDog).IsStunned) return false;

        return true;
    }

    private void Attack()
    {
        anim.SetBool(AttackAnim, true);

        var targets = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("Player"));

        foreach (var target in targets)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(1f);
            }
        }

        Debug.Log("Enemy attacks!");
    }

    public void StartFollowing()
    {
        followPlayer = true;
    }

    private void FollowMovement()
    {
        if (!followPlayer) return;

        anim.SetBool(AttackAnim, false);

        direction = new Vector2(Mathf.Sign(HumanPlayer.transform.position.x - transform.position.x), 0);

        rig.linearVelocity = new Vector2(direction.x * speedMovement, rig.linearVelocityY);

        sprite.flipX = direction.x < 0;
    }

    
}
