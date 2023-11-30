using Tools.Extensions;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public ReactiveEvent OnFloorFallInDeathZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FloorView floorView))
        {
            OnFloorFallInDeathZone.Notify();
        }
    }
}
