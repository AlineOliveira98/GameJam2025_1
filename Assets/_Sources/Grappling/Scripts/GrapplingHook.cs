using System;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;

    public Vector3 grapplePoint;
    public bool isGrappling;
    private DistanceJoint2D joint;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        DisableHook();
    }

    void Update()
    {
        GenerateRope();
        CheckFinalRopePosition();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrappling)
                EnableHook();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableHook();
            isGrappling = false;
        }

        if (rope.enabled)
        {
            rope.SetPosition(1, transform.position);
        }
    }

    private void CheckFinalRopePosition()
    {
        if (!isGrappling) return;
        
        var distance = Vector2.Distance(transform.position, grapplePoint);

        if (distance < 1.5f)
        {
            TryGoUpSide();
        }
    }

    private void TryGoUpSide()
    {
        var hit = Physics2D.OverlapCircle(grapplePoint, 0.5f, LayerMask.GetMask("Ground"));

        if (hit != null)
        {
            Vector2 target = (Vector2) grapplePoint + Vector2.up * 1.5f;
            transform.position = target;
            DisableHook();
            isGrappling = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(grapplePoint, 0.5f);
    }

    private void GenerateRope()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast
            (
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero,
                Mathf.Infinity,
                grappleLayer
            );

            if (hit.collider != null)
            {
                grapplePoint = hit.collider.transform.position;
                grapplePoint.z = 0;
                joint.connectedAnchor = grapplePoint;
                joint.distance = grappleLength;

                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;

                isGrappling = true;
            }
        }
    }

    private void EnableHook()
    {
        joint.enabled = true;
    }
    
    private void DisableHook()
    {
        joint.enabled = false;
        rope.enabled = false;
    }
}
