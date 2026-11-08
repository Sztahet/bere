using System;
using System.Reflection;
using UnityEngine;

// Temporary runtime probe; source is outside Assets and never enters a build.
public sealed class SparkTrialProbe : MonoBehaviour
{
    RiftorynTrial trial; RiftorynSparkFollower follower; Transform player; MethodInfo pointAt;
    public float along; public int wallSamples, samples, projectileSamples; public bool hitTriggered,finished;
    public void Initialize(RiftorynTrial t,RiftorynSparkFollower f,Transform p){trial=t;follower=f;player=p;pointAt=typeof(RiftorynTrial).GetMethod("At",BindingFlags.Instance|BindingFlags.NonPublic);}
    void Update(){
        if(finished||!player||!follower)return;
        along=Mathf.Min(48f,along+Time.deltaTime*4.4f);
        var next=(Vector3)pointAt.Invoke(trial,new object[]{along});var delta=next-player.position;player.position=next;
        if(delta.sqrMagnitude>.00001f)player.rotation=Quaternion.LookRotation(delta);
        if(along>21&&!hitTriggered){follower.TriggerHit();hitTriggered=true;}
        if(along>=48)finished=true;
    }
    void LateUpdate(){
        if(finished||!follower)return;var fox=GameObject.Find("Iskra — test wizualny");if(!fox)return;
        samples++;foreach(var c in Physics.OverlapSphere(fox.transform.position+Vector3.up*.45f,.30f,~0,QueryTriggerInteraction.Ignore)){if(c.name=="Mur"){wallSamples++;break;}}
        if(GameObject.Find("Pocisk"))projectileSamples++;
    }
}
public static class SparkTrialValidation
{
    public static string Start()
    {
        var trial=UnityEngine.Object.FindAnyObjectByType<RiftorynTrial>();
        var flags=BindingFlags.Instance|BindingFlags.NonPublic;
        trial.menuOpen=false;
        typeof(RiftorynTrial).GetMethod("Begin",flags).Invoke(trial,null);
        RiftorynSparkFieldSetup.Enable();
        var follower=UnityEngine.Object.FindAnyObjectByType<RiftorynSparkFollower>();
        var probe=new GameObject("Spark trial probe").AddComponent<SparkTrialProbe>();
        probe.Initialize(trial,follower,GameObject.Find("Gracz").transform);
        return "Visual-only test started";
    }
}
