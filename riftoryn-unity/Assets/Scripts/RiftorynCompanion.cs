using UnityEngine;

public abstract class RiftorynCompanion : MonoBehaviour
{
    public abstract bool IsAlive { get; }
    public abstract Vector3 AimPosition { get; }
    public virtual bool IsLuring=>false;
    public abstract void ReceiveHit(float damage);
}
