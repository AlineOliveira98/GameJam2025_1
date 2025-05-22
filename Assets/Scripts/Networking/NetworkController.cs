using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkController : MonoBehaviourPunCallbacks
{
    public GameObject connectedScreen;
    public GameObject disconnetedScreen;

    public void ConectarButton()
    {
        PhotonNetwork.ConnectUsingSettings(); //Conectar no Servidor
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default); //entra no lobby
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnetedScreen.SetActive(true); //Quando falha a conexao
    }

    public override void OnJoinedLobby()
    {
        connectedScreen.SetActive(true); //Login apos o lobby 
    }
}
