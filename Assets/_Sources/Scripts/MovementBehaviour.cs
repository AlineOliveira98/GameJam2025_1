using UnityEngine;
using Photon.Pun;

public class MovementBehaviour : MonoBehaviourPun
{
    [Header("Settings")]
    [SerializeField] private float speedMovement;
    [SerializeField] private float jumpForce;

    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;

    private const string RunAnim = "IsRunning";

    public void HandleMovement(float horizontalInput)
    {
        rig.linearVelocity = new Vector2(horizontalInput * speedMovement, rig.linearVelocity.y);

        sprite.flipX = horizontalInput < 0;
        anim.SetBool(RunAnim, horizontalInput != 0);

        photonView.RPC("ChangeSpriteDirection", RpcTarget.Others, horizontalInput < 0);
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    public void HandleJump()
    {
        if (IsGrounded())
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
    }



    [PunRPC]
    private void ChangeSpriteDirection(bool flipX)
    {
        sprite.flipX = flipX;
    }

    public void TriggerJumpAnimationOnly()
    {
        anim.SetTrigger("Jump"); // Só a animação
    }


    public Rigidbody2D GetRigidbody() => rig;
}
