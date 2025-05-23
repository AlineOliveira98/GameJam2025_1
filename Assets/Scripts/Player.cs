using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rig;
    public float speed;
    private int lastDirection = 0;
    public SpriteRenderer spriteRenderer;

    private Vector2 clientPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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

    #region myClient
    private void ProcessInput()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement > 0 && lastDirection != 1)
        {
            transform.rotation = Quaternion.Euler(0, 0f, 0);
            this.photonView.RPC("ChangeRight", RpcTarget.Others);
            lastDirection = 1;
        }
        else if (movement < 0 && lastDirection != -1)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            this.photonView.RPC("ChangeLeft", RpcTarget.Others);
            lastDirection = -1;
        }
        else if (movement == 0)
        {
            lastDirection = 0;
        }

        

    }

    #endregion

    #region RPCs Functions
    [PunRPC]
    private void ChangeLeft()
    {
        //spriteRenderer.flipX = true;
        transform.rotation = Quaternion.Euler(0, 0f, 0);
    }

    [PunRPC]
    private void ChangeRight()
    {
        //spriteRenderer.flipY = true;
        transform.rotation = Quaternion.Euler(0, -180f, 0);
    }

    #endregion

    #region othersClients
    private void smoothMovement()
    {
        //transform.position = Vector3.Lerp(transform.position, clientPos, Time.fixedDeltaTime);
        float lerpSpeed = 10f; // Ajustável
        rig.position = Vector2.Lerp(rig.position, clientPos, lerpSpeed * Time.fixedDeltaTime);

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



        //if (stream.IsWriting)
        //{
        //    stream.SendNext(transform.position);
        //}
        //else if (stream.IsReading)
        //{
        //    clientPos = (Vector2)stream.ReceiveNext();
        //}
    }
    #endregion
}
