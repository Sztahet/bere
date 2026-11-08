using System;

// One non-stacking damage-over-time effect. Refreshing never postpones the next tick.
public sealed class RiftorynPeriodicEffect
{
    public float Remaining { get; private set; }
    public float DamagePerTick { get; private set; }
    float untilTick;

    public void Apply(float damage, float duration)
    {
        if (damage <= 0 || duration <= 0 || (Remaining > 0 && damage < DamagePerTick)) return;
        if (Remaining <= 0) untilTick = 1f;
        DamagePerTick = damage;
        Remaining = duration;
    }

    public void Advance(float dt, Action<float> onTick)
    {
        if (Remaining <= 0 || dt <= 0) return;
        float activeTime = Math.Min(dt, Remaining);
        untilTick -= activeTime;
        while (untilTick <= 0.00001f)
        {
            onTick(DamagePerTick);
            untilTick += 1f;
        }
        Remaining = Math.Max(0, Remaining - dt);
        if (Remaining <= 0) DamagePerTick = 0;
    }
}
