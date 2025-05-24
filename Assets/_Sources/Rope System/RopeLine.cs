using UnityEngine;

public class RopeLine : MonoBehaviour
{
    private RopeGenerator rope;
    private LineRenderer line;


    // void Awake()
    // {
    //     rope = GetComponent<RopeGenerator>();
    //     line = GetComponent<LineRenderer>();

    //     line.enabled = true;
    //     line.positionCount = rope.segments.Lenght;
    // }

    // void Update()
    // {
    //     for (int i = 0; i < rope.segments.Length; i++)
    //     {
    //         line.SetPosition(i, rope.segments[i].position); 
    //     }
    // }
}