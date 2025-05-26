using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] private bool localMode;
    [SerializeField] private GameObject Player;

    [Header("Players Local Mode")]
    [SerializeField] private bool isHumanPlayer;
    [SerializeField] private PlayerController humanPlayer;
    [SerializeField] private PlayerController dogPlayer;

    public PlayerController HumanPlayer => humanPlayer;
    public PlayerController DogPlayer => dogPlayer;
    
    public float DistanceBetweenPlayers { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(!localMode) SpawnPlayer();
    }

    void Update()
    {
        if (humanPlayer == null || dogPlayer == null) return;

        DistanceBetweenPlayers = (humanPlayer.transform.position - dogPlayer.transform.position).sqrMagnitude;
    }

    public void SpawnPlayer()
    {
        float randomPos = Random.Range(-3f, 2f);
        PhotonNetwork.Instantiate(Player.name, new Vector2(Player.transform.position.x + randomPos, Player.transform.position.y), Quaternion.identity);
    }
}
