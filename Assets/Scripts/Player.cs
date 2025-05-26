using UnityEngine;
using Photon.Pun;
using Unity.Cinemachine;
using System.Collections;

public class Player : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rig;
    public float speed;
    public SpriteRenderer spriteRenderer;

    private Vector2 clientPos;

    public bool IsFacingRight = true;
    private CameraFollowobject _cameraRig;

    // Jump
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private bool jumpPressed;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (photonView.IsMine)
        {
            GameObject camTarget = new GameObject("CameraFollowTarget");
            camTarget.transform.position = transform.position;

            var camFollow = camTarget.AddComponent<CameraFollowobject>();
            camFollow.SetTarget(transform);
            camFollow.SetFacing(IsFacingRight);
            _cameraRig = camFollow;

            StartCoroutine(SetupCamera(camTarget.transform));
        }
    }

    private IEnumerator SetupCamera(Transform followTarget)
    {
        yield return null;

        foreach (var cam in Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            if (cam.name == "PlayerCamera")
            {
                cam.Follow = followTarget;
                yield break;
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Captura o botão de pulo no Update (mais preciso)
            if (Input.GetButtonDown("Jump"))
            {
                jumpPressed = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            ProcessInput();

            // Verifica se está no chão
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            // Executa o pulo se foi pressionado e está no chão
            if (jumpPressed && isGrounded)
            {
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, jumpForce);
                photonView.RPC("DoJump", RpcTarget.Others);
            }

            jumpPressed = false; // reseta o flag
        }
        else
        {
            smoothMovement();
        }
    }

    #region myClient
    private void ProcessInput()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            photonView.RPC("ChangeLeft", RpcTarget.Others);
            IsFacingRight = true;
            _cameraRig?.SetFacing(true);
        }
        else if (movement < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            photonView.RPC("ChangeRight", RpcTarget.Others);
            IsFacingRight = false;
            _cameraRig?.SetFacing(false);
        }
    }
    #endregion

    #region RPCs Functions
    [PunRPC]
    private void ChangeLeft()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    [PunRPC]
    private void ChangeRight()
    {
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    [PunRPC]
    private void DoJump()
    {
        rig.linearVelocity = new Vector2(rig.linearVelocity.x, jumpForce);
    }
    #endregion

    #region othersClients
    private void smoothMovement()
    {
        rig.position = Vector2.MoveTowards(rig.position, clientPos, Time.fixedDeltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rig.position);
            stream.SendNext(rig.linearVelocity);
        }
        else
        {
            clientPos = (Vector2)stream.ReceiveNext();
            rig.linearVelocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            clientPos += rig.linearVelocity * lag;
        }
    }
    #endregion
}
