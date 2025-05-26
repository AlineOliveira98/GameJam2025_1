using UnityEngine;

public class PlayerHuman : PlayerController
{
    public void ThrowHook()
    {

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Death();
    }

    private void Death()
    {
        IsDead = true;
        movementBehaviour.CanMove = false;
        movementBehaviour.CanJump = false;
    }
}
