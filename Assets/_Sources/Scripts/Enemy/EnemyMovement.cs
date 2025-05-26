using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private const string RunAnim = "IsRunning";
    private const string AttackAnim = "IsAttacking";

    [Header("Settings")]
    [SerializeField] private float speedMovement = 2f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;

    private Transform HumanPlayer;
    private Transform DogPlayer;
    private Vector2 direction;
    private float distanceHuman => Mathf.Abs(HumanPlayer.position.x - transform.position.x);
    private float distanceDog => Mathf.Abs(DogPlayer.position.x - transform.position.x);
    private bool followPlayer;

    void Start()
    {
        HumanPlayer = GameController.Instance.HumanPlayer.transform;
        DogPlayer = GameController.Instance.DogPlayer.transform;
    }

    void FixedUpdate()
    {
        if (distanceHuman < 3f || distanceDog < 3f)
        {
            Attack();
        }
        else
        {
            FollowMovement();
        }
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
    }

    public void StartFollowing()
    {
        followPlayer = true;
    }

    private void FollowMovement()
    {
        if (!followPlayer) return;

        anim.SetBool(AttackAnim, false);

        direction = new Vector2(Mathf.Sign(HumanPlayer.position.x - transform.position.x), 0);

        rig.linearVelocity = new Vector2(direction.x * speedMovement, rig.linearVelocityY);

        sprite.flipX = direction.x < 0;
    }

    
}
