using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{

    private Rigidbody2D rig;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //Minha movaimentacao
            ProcessInput();
        }
        else
        {
            //sincroniza outros players
            smoothMovement();
        }
    }

    private void ProcessInput()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement > 0)
        {

        }
        if (movement < 0)
        {

        }
        if (movement == 0)
        {

        }
    }

    private void smoothMovement()
    {

    }
}
