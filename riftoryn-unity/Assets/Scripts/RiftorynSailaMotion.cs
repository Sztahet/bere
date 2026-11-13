using System;
using UnityEngine;

// Provisional runtime animation for the PoC rig; can be replaced by authored clips.
public sealed class RiftorynSailaMotion : MonoBehaviour
{
    Transform actor;
    Transform[] bones;
    Quaternion[] rest;
    Vector3 previous;
    float blend, phase, casting;
    static readonly string[] Names={"DEF-thigh.L","DEF-thigh.R","DEF-shin.L","DEF-shin.R","DEF-upper_arm.L","DEF-upper_arm.R","DEF-forearm.L","DEF-forearm.R","DEF-spine.003"};
    public void Initialize(Transform runner)
    {
        actor=runner;previous=actor.position;var all=GetComponentsInChildren<Transform>(true);
        bones=new Transform[Names.Length];rest=new Quaternion[Names.Length];
        for(int i=0;i<Names.Length;i++){bones[i]=Array.Find(all,t=>t.name==Names[i]);if(bones[i])rest[i]=bones[i].localRotation;}
    }
    public void Cast(){casting=.65f;}
    void LateUpdate()
    {
        if(!actor||bones==null)return;
        float dt=Mathf.Max(Time.deltaTime,.0001f);float speed=Vector3.Distance(actor.position,previous)/dt;previous=actor.position;
        blend=Mathf.MoveTowards(blend,speed>.15f?1:0,dt*7);phase+=dt*(4f+Mathf.Clamp(speed,0,7)*1.2f);casting=Mathf.Max(0,casting-dt);
        float s=Mathf.Sin(phase)*blend;float cast=casting>0?Mathf.Sin((.65f-casting)/.65f*Mathf.PI):0;
        Rotate(0,26*s);Rotate(1,-26*s);Rotate(2,Mathf.Max(0,-s)*25);Rotate(3,Mathf.Max(0,s)*25);
        Rotate(4,-17*s-24*cast);Rotate(5,17*s-24*cast);Rotate(6,8*blend-18*cast);Rotate(7,8*blend-18*cast);
        Rotate(8,Mathf.Sin(Time.time*1.7f)*1.2f);
    }
    void Rotate(int i,float angle){if(bones[i])bones[i].localRotation=rest[i]*Quaternion.Euler(angle,0,0);}
}
