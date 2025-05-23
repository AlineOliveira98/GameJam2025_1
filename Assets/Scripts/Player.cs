using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rig;
    public float speed;
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

        if (movement > 0)
        {
            //TODO - flip
            //spriteRenderer.flipX = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
            this.photonView.RPC("ChangeLeft", RpcTarget.Others);
        }
        if (movement < 0)
        {
            //TODO - flip
            //spriteRenderer.flipX = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
            this.photonView.RPC("ChangeRight", RpcTarget.Others);
        }
        if (movement == 0)
        {

        }
    }

    #endregion

    #region RPCs Functions
    [PunRPC]
    private void ChangeLeft()
    {
        //spriteRenderer.flipX = false;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    [PunRPC]
    private void ChangeRight()
    {
        //spriteRenderer.flipX = true;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    #endregion

    #region othersClients
    private void smoothMovement()
    {
        //transform.position = Vector3.Lerp(transform.position, clientPos, Time.fixedDeltaTime);
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