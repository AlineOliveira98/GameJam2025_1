using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Lobby : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoom;
    public TMP_InputField joinRoom;

    //Metodo ao clicar no Button criar sala 
    //TODO - configurar numero maximo de players
    public void CreateRoomButton()
    {
        PhotonNetwork.CreateRoom(createRoom.text, new RoomOptions {MaxPlayers = 4}, null);
    }

    //Metodo ao clicar no button entrar na sala
    //TODO - add numero de participantes dps
    public void JoinRoomButton()
    {

        PhotonNetwork.JoinRoom(joinRoom.text, null);
    }

    //Metodo ao entrar a sala
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(2);
    }

    //Metodo ao dar erro ao entrar na sala
    //TODO - apagar debug dps
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Failed" + returnCode + " Message " + message);
    }
}
