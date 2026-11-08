using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

// Trial values: 2 m ahead, 6.5 m/s, 4 s flight, three circles and 75% dodge.
public class RiftorynPatkaPreview : MonoBehaviour
{
    const float FollowDistance=2f, Speed=6.5f, FlightDuration=4f, Radius=1.5f, Height=2.5f;
    PlayableGraph graph;
    AnimationClipPlayable playable;
    AnimationClip run, flight;
    Transform owner;
    RiftorynPetHealth health;
    float flightTime;
    Vector3 previousPosition;
    bool flying;
    public bool IsFlying => flying && health != null && health.currentHp > 0;
    public bool IsAlive => health != null && health.currentHp > 0 && gameObject.activeInHierarchy;
    public Vector3 AimPosition => transform.position + Vector3.up*.45f;
    public bool Fly()
    {
        if (!IsAlive || flying) return false;
        flightTime=0; flying=true; SetClip(flight); return true;
    }
    public void ReceiveHit(float damage)
    {
        if (!IsAlive) return;
        if (IsFlying && Random.value < .75f) return;
        health.currentHp=Mathf.Max(0, health.currentHp-damage);
        if (health.currentHp<=0) { flying=false; foreach(var r in GetComponentsInChildren<Renderer>()) r.enabled=false; }
    }
    void Start()
    {
        owner=transform.parent;
        health=GetComponent<RiftorynPetHealth>();
        if(owner) transform.rotation=owner.rotation;
        foreach(var clip in Resources.LoadAll<AnimationClip>("Pets/patka"))
        {
            if(clip.name.StartsWith("__preview__")) continue;
            if(clip.name.Contains("Patka_Bieg")) run=clip;
            if(clip.name.Contains("Patka_Lot")) flight=clip;
        }
        var animator=GetComponent<Animator>(); if(!animator) animator=gameObject.AddComponent<Animator>();
        graph=PlayableGraph.Create("Patka"); AnimationPlayableOutput.Create(graph,"Patka",animator);
        previousPosition=transform.position; SetClip(run); graph.Play();
    }
    void SetClip(AnimationClip clip)
    {
        if(!graph.IsValid() || !clip) return;
        if(playable.IsValid()) playable.Destroy();
        playable=AnimationClipPlayable.Create(graph,clip); graph.GetOutput(0).SetSourcePlayable(playable);
    }
    void LateUpdate()
    {
        if(!owner || !IsAlive) return;
        // Preserve world position when the parent moves; follow with the pet's own speed.
        transform.position=previousPosition;
        Vector3 target=owner.position+owner.forward*FollowDistance;
        if(flying)
        {
            flightTime+=Time.deltaTime;
            float t=Mathf.Clamp01(flightTime/FlightDuration), angle=t*Mathf.PI*6f;
            float lift=Mathf.SmoothStep(0,1,Mathf.Min(t/.15f,(1-t)/.2f));
            target=owner.position+new Vector3(Mathf.Cos(angle)*Radius,Height*lift,Mathf.Sin(angle)*Radius);
            transform.position=target;
            transform.rotation=Quaternion.LookRotation(new Vector3(-Mathf.Sin(angle),0,Mathf.Cos(angle)));
            if(t>=1){flying=false;SetClip(run);}
        }
        else
        {
            Vector3 before=transform.position;
            transform.position=Vector3.MoveTowards(before,target,Speed*Time.deltaTime);
            Vector3 direction=target-before;direction.y=0;
            if(direction.sqrMagnitude>.01f) transform.rotation=Quaternion.LookRotation(direction);
        }
        previousPosition=transform.position;
        var active=flying?flight:run;
        if(playable.IsValid() && active && active.length>0 && playable.GetTime()>=active.length) playable.SetTime(playable.GetTime()%active.length);
    }
    void OnDestroy(){if(graph.IsValid())graph.Destroy();}
}
