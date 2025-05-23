using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        float randomPos = Random.Range(-3f, 2f);
        PhotonNetwork.Instantiate(Player.name, new Vector2(Player.transform.position.x + randomPos, Player.transform.position.y), Quaternion.identity);
    }
}
