using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedMovement;
    [SerializeField] private Rigidbody2D rig;

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(horizontalInput * speedMovement, rig.linearVelocityY);
    }
}
