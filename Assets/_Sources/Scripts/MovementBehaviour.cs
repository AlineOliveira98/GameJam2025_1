using DG.Tweening;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private const string RunAnim = "IsRunning";
    private const string JumpAnim = "Jump";


    [Header("Settings")]
    [SerializeField] private float speedMovement;
    [SerializeField] private float jumpForce;


    [Header("References")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator anim;

    void Update()
    {
        Movement();
        Jump();
    }

    private void Movement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(horizontalInput * speedMovement, rig.linearVelocityY);

        sprite.flipX = horizontalInput < 0;
        anim.SetBool(RunAnim, horizontalInput != 0);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
