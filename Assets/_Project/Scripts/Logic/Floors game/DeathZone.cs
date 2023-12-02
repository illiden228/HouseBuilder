using Tools.Extensions;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private ReactiveEvent onFloorFallInDeathZone;

    public void Init(ReactiveEvent reactiveEvent)
    {
        onFloorFallInDeathZone = reactiveEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FloorView floorView))
        {
            onFloorFallInDeathZone.Notify();
        }
    }
}
