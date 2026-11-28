using UnityEngine;

// Shared HP and target contract for experimental companions. The fox has its
// own animation/following component; the other two use temporary primitives.
public sealed class RiftorynSummonedCompanion : RiftorynCompanion
{
    Transform owner;
    RiftorynPetHealth health;
    RiftorynSparkFollower fox;
    float shield;
    float shieldTick;
    bool externalMotion;
    public override bool IsAlive=>health&&health.currentHp>0&&owner&&owner.gameObject.activeInHierarchy;
    public override Vector3 AimPosition=>transform.position+Vector3.up*.45f;
    public float CurrentHp=>health?health.currentHp:0;
    public float MaximumHp=>health?health.maxHp:0;
    public float Shield=>shield;

    public void Initialize(Transform ownerTransform,string petName,float maxHp,RiftorynSparkFollower foxFollower=null)
    {
        owner=ownerTransform;fox=foxFollower;externalMotion=foxFollower!=null;
        health=gameObject.AddComponent<RiftorynPetHealth>();health.petName=petName;health.maxHp=health.currentHp=maxHp;
    }
    public void Heal(float amount){if(IsAlive)health.currentHp=Mathf.Min(health.maxHp,health.currentHp+Mathf.Max(0,amount));}
    public void AddShield(float amount){if(IsAlive&&amount>0){if(shield<=0)shieldTick=1;shield+=amount;}}
    public override void ReceiveHit(float damage)
    {
        if(!IsAlive)return;
        float blocked=Mathf.Min(shield,Mathf.Max(0,damage));shield-=blocked;
        health.currentHp=Mathf.Max(0,health.currentHp-Mathf.Max(0,damage-blocked));
        if(fox)fox.TriggerHit();
        if(health.currentHp>0)return;
        if(fox)fox.companionVisible=false;
        else foreach(var renderer in GetComponentsInChildren<Renderer>())renderer.enabled=false;
    }
    void LateUpdate()
    {
        if(shield>0){shieldTick-=Time.deltaTime;while(shield>0&&shieldTick<=0){shield*=.9f;shieldTick+=1;if(shield<.01f)shield=0;}}
        if(!owner||!IsAlive||externalMotion)return;
        // Temporary visual follows the owner's collision-safe route; no pet collider.
        Vector3 desired=owner.position-owner.forward*1.1f+owner.right*.65f;
        foreach(var obstacle in Physics.OverlapSphere(desired+Vector3.up*.4f,.35f,~0,QueryTriggerInteraction.Ignore))
            if(obstacle.name=="Mur"){desired=owner.position;break;}
        transform.position=desired;
        transform.rotation=owner.rotation;
    }
}
