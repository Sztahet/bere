using UnityEngine;

public abstract class RiftorynCompanion : MonoBehaviour
{
    readonly RiftorynPeriodicEffect poison=new();
    public abstract bool IsAlive { get; }
    public abstract Vector3 AimPosition { get; }
    public virtual bool IsLuring=>false;
    public abstract void ReceiveHit(float damage);
    // Periodic damage is already applied, so it is not a new attack to dodge.
    protected virtual void ReceivePeriodicDamage(float damage)=>ReceiveHit(damage);
    public void ApplyPoison(float damage,float duration){if(IsAlive)poison.Apply(damage,duration);}
    public void AdvanceEffects(float dt){if(IsAlive)poison.Advance(dt,damage=>{if(IsAlive)ReceivePeriodicDamage(damage);});}
}
