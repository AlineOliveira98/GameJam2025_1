using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] private int ropeSize;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private HingeJoint2D hingePrefab;

    private Transform[] segments;

    private Vector2 GetSegmentPosition(int index)
    {
        var fraction = 1f / (float)ropeSize;
        return Vector2.Lerp(startPoint.position, endPoint.position, fraction * index);
    }


    [Button]
    private void GenerateRope()
    {
        segments = new Transform[ropeSize];

        for (int i = 0; i < segments.Length; i++)
        {
            var currJoint = Instantiate(hingePrefab, GetSegmentPosition(i), Quaternion.identity, transform);
            segments[i] = currJoint.transform;

            if (i <= 0) continue;

            currJoint.connectedBody = segments[i - 1].GetComponent<Rigidbody2D>();
        }
    }

    private void OnDrawGizmos()
    {
        if (startPoint == null || endPoint == null) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < ropeSize; i++)
        {
            Vector2 posAtIndex = GetSegmentPosition(i);
            Gizmos.DrawSphere(posAtIndex, 0.1f);
        }
    }
}
