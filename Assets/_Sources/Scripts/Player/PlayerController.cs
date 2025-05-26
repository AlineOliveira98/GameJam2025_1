using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rig;
    [SerializeField] protected MovementBehaviour movementBehaviour;

    public bool IsDead { get; set; }

    public virtual void TakeDamage(float damage)
    {

    }
}