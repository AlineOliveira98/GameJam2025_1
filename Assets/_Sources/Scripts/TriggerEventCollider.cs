using UnityEngine;
using UltEvents;

public class TriggerEventCollider : MonoBehaviour
{
    public UltEvent OnTriggerEnterEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnterEvent?.Invoke();
    }
}
