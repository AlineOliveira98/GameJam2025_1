using UnityEngine;

public class CameraFollowobject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float lateralOffset = 3f;
    [SerializeField] private float verticalOffset = -0.27f;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity;
    private bool isFacingRight = true;

    public void SetTarget(Transform t) => target = t;
    public void SetFacing(bool facingRight) => isFacingRight = facingRight;

    void Update()
    {
        if (target == null) return;

        float direction = isFacingRight ? 1f : -1f;
        Vector3 offset = new Vector3(direction * lateralOffset, verticalOffset, 0);
        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
