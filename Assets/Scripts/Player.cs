using UnityEngine;
using Photon.Pun;
using TMPro;

[RequireComponent(typeof(MovementBehaviour))]
public class Player : MonoBehaviourPun, IPunObservable
{
    private MovementBehaviour movement;
    private Vector2 clientPos;
    private Vector2 velocityCache;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Vector2 targetVelocity;


    void Awake()
    {
        movement = GetComponent<MovementBehaviour>();
        rb = movement.GetRigidbody();

        if (!photonView.IsMine)
        {
            movement.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;

        }
    }



    void Update()
    {
        if (photonView.IsMine)
        {
            HandleInput();
        }
        else
        {
            SmoothMovement();
        }
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        movement.HandleMovement(horizontalInput);

        if (jumpPressed)
        {
            movement.HandleJump();
            photonView.RPC("RemoteJump", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RemoteJump()
    {
        // Executa apenas a animação, não o AddForce
        movement.TriggerJumpAnimationOnly(); // Novo método
    }



    private void SmoothMovement()
    {
        rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, Time.deltaTime * 10f));
    }




    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviando posi��o e velocidade para os outros
            stream.SendNext(transform.position);
            stream.SendNext(rb.linearVelocity);
        }
        else
        {
            // Recebendo do dono
            targetPosition = (Vector2)stream.ReceiveNext();
            targetVelocity = (Vector2)stream.ReceiveNext();

            // Lag compensation
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            targetPosition += targetVelocity * lag;
        }
    }

}
