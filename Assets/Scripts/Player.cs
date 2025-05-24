using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MovementBehaviour))]
public class Player : MonoBehaviourPun, IPunObservable
{
    private MovementBehaviour movement;
    private Vector2 clientPos;
    private Vector2 velocityCache;

    void Awake()
    {
        movement = GetComponent<MovementBehaviour>();

        // Desativa o script de movimento dos jogadores remotos
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


    private void SmoothMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, clientPos, Time.deltaTime * 10f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Rigidbody2D rig = movement.GetRigidbody();

        if (stream.IsWriting)
        {
            stream.SendNext(rig.position);
            stream.SendNext(rig.linearVelocity);
        }
        else
        {
            clientPos = (Vector2)stream.ReceiveNext();
            velocityCache = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            clientPos += velocityCache * lag;
        }
    }
}
