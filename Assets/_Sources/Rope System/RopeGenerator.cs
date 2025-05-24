using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] private int ropeSize;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private HingeJoint2D hingePrefab;

    // private List<Transform> 

    // private Vector2 GetSegmentPosition(int index)
    // {
    //     var fraction = 1f / (float)segmentsCount;
    //     return Vector2.Lerp(startPoint.position, endPoint.position, fraction * index);
    // }


    [Button]
    private void GenerateRope()
    {

    }
}
