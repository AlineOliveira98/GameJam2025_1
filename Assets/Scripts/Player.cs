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
        movement.HandleJump(); // Executa o pulo nos clientes remotos
    }


    [SerializeField] private float smoothSpeed = 10f;

    private void SmoothMovement()
    {
        // Move suavemente o objeto até a posição recebida
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviando posição e velocidade para os outros
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
