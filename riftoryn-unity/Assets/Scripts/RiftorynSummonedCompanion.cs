using UnityEngine;

// Shared HP and target contract for experimental companions.
public sealed class RiftorynSummonedCompanion : RiftorynCompanion
{
    Transform owner;
    RiftorynPetHealth health;
    RiftorynSparkFollower fox;
    float shield;
    float shieldTick;
    bool externalMotion;
    bool leadOwner;
    Transform[] paws;
    Quaternion[] pawRest;
    Vector3 previousOwnerPosition;
    float gaitBlend,gaitPhase;
    public override bool IsAlive=>health&&health.currentHp>0&&owner&&owner.gameObject.activeInHierarchy;
    public override Vector3 AimPosition=>transform.position+Vector3.up*.45f;
    public float CurrentHp=>health?health.currentHp:0;
    public float MaximumHp=>health?health.maxHp:0;
    public float Shield=>shield;

    public void Initialize(Transform ownerTransform,string petName,float maxHp,RiftorynSparkFollower foxFollower=null,bool lead=false,string gaitPrefix=null)
    {
        owner=ownerTransform;fox=foxFollower;externalMotion=foxFollower!=null;leadOwner=lead;
        health=gameObject.AddComponent<RiftorynPetHealth>();health.petName=petName;health.maxHp=health.currentHp=maxHp;
        previousOwnerPosition=owner.position;
        if(!string.IsNullOrEmpty(gaitPrefix))
        {
            string[] names={gaitPrefix+"_Przod_L",gaitPrefix+"_Przod_R",gaitPrefix+"_Tyl_L",gaitPrefix+"_Tyl_R"};
            paws=new Transform[names.Length];pawRest=new Quaternion[names.Length];
            var transforms=GetComponentsInChildren<Transform>(true);
            for(int i=0;i<names.Length;i++)foreach(var part in transforms)if(part.name==names[i]){paws[i]=part;pawRest[i]=part.localRotation;break;}
        }
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
        // Bob leads Ana, while the other prototype stays behind its owner.
        Vector3 desired=owner.position+owner.forward*(leadOwner?1.35f:-1.1f)+owner.right*(leadOwner?.4f:.65f);
        Vector3 ray=desired-owner.position;
        foreach(var hit in Physics.SphereCastAll(owner.position+Vector3.up*.4f,.3f,ray.normalized,ray.magnitude,~0,QueryTriggerInteraction.Ignore))
            if(hit.collider.name=="Mur"){desired=owner.position;break;}
        foreach(var obstacle in Physics.OverlapSphere(desired+Vector3.up*.4f,.35f,~0,QueryTriggerInteraction.Ignore))
            if(obstacle.name=="Mur"){desired=owner.position;break;}
        transform.position=desired;
        transform.rotation=owner.rotation;
        if(paws!=null)AnimatePaws();
    }
    void AnimatePaws()
    {
        float dt=Mathf.Max(Time.deltaTime,.0001f);
        float speed=Vector3.Distance(owner.position,previousOwnerPosition)/dt;previousOwnerPosition=owner.position;
        gaitBlend=Mathf.MoveTowards(gaitBlend,speed>.15f?1f:0f,dt*7f);
        gaitPhase+=dt*(4f+Mathf.Clamp(speed,0,7f)*1.2f);
        float swing=Mathf.Sin(gaitPhase)*gaitBlend*25f;
        for(int i=0;i<paws.Length;i++)if(paws[i])paws[i].localRotation=pawRest[i]*Quaternion.Euler(i==0||i==3?swing:-swing,0,0);
    }
}
