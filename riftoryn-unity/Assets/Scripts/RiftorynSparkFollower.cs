using System;
using System.Collections.Generic;
using UnityEngine;

// Visual-only experiment: no health, damage, colliders, targeting or game-rule changes.
public sealed class RiftorynSparkFollower : MonoBehaviour
{
    public Transform target;
    public float followDistance=1.6f;
    public bool outlineEnabled;
    public float distanceTravelled { get; private set; }
    public float maxGap { get; private set; }
    public int hitCount { get; private set; }
    readonly List<Vector3> trail=new();
    readonly Dictionary<Renderer,Material> colors=new();
    readonly Dictionary<Material,Color> baseColors=new();
    GameObject fox; Animation anim; Material outline; Light flash;
    string idle,run,hit,current="";float hitUntil,flashUntil;
    Vector3 lastPosition; bool valid;
    public void Initialize(Transform owner,bool useOutline=false)
    {
        target=owner;outlineEnabled=useOutline;
        var prefab=Resources.Load<GameObject>("Pets/spark-preview");
        if(!prefab||!target){enabled=false;return;}
        fox=Instantiate(prefab,target.position,Quaternion.identity,transform);fox.name="Iskra — test wizualny";fox.transform.localScale=Vector3.one*1.2f;
        foreach(var collider in fox.GetComponentsInChildren<Collider>())Destroy(collider);
        var materialCache=new Dictionary<string,Material>();
        foreach(var renderer in fox.GetComponentsInChildren<Renderer>())
        {
            var source=renderer.sharedMaterial;if(!source)continue;string name=source.name;
            if(!materialCache.TryGetValue(name,out var material)){
                Color c=name.Contains("jasne")?new Color(.91f,.72f,.51f):name.Contains("rudy")?new Color(.63f,.17f,.055f):name.Contains("żar")?new Color(1f,.39f,.045f):new Color(.18f,.065f,.045f);
                material=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard"));material.color=c;materialCache.Add(name,material);baseColors.Add(material,c);
            }
            colors.Add(renderer,material);
        }
        var shader=Resources.Load<Shader>("SparkPreviewOutline");if(shader)outline=new Material(shader);
        SetOutline(useOutline);
        anim=fox.GetComponent<Animation>();if(!anim)anim=fox.AddComponent<Animation>();
        foreach(var clip in Resources.LoadAll<AnimationClip>("Pets/spark-preview")){
            if(!clip.legacy||clip.name.StartsWith("__preview__",StringComparison.Ordinal))continue;
            if(clip.name.IndexOf("Idle",StringComparison.OrdinalIgnoreCase)>=0)idle=clip.name;
            if(clip.name.IndexOf("Run",StringComparison.OrdinalIgnoreCase)>=0)run=clip.name;
            if(clip.name.IndexOf("Hit",StringComparison.OrdinalIgnoreCase)>=0)hit=clip.name;
            anim.AddClip(clip,clip.name);
        }
        var glow=new GameObject("Błysk trafienia");glow.transform.SetParent(fox.transform,false);glow.transform.localPosition=Vector3.up*.35f;flash=glow.AddComponent<Light>();flash.type=LightType.Point;flash.color=new Color(1,.55f,.18f);flash.range=3;flash.intensity=0;flash.shadows=LightShadows.None;
        trail.Add(target.position);lastPosition=fox.transform.position;valid=true;Play(idle,true);
    }
    public void SetOutline(bool enabledOutline){outlineEnabled=enabledOutline&&outline;foreach(var pair in colors)pair.Key.sharedMaterials=outlineEnabled?new[]{pair.Value,outline}:new[]{pair.Value};}
    void Play(string clip,bool loop){if(!anim||string.IsNullOrEmpty(clip)||current==clip)return;anim[clip].wrapMode=loop?WrapMode.Loop:WrapMode.Once;anim.CrossFade(clip,.08f);current=clip;}
    public void TriggerHit(){if(!valid||string.IsNullOrEmpty(hit))return;hitCount++;current="";Play(hit,false);hitUntil=Time.time+anim[hit].length;flashUntil=Time.time+.16f;}
    void LateUpdate()
    {
        if(!valid||!fox)return;if(!target){Destroy(gameObject);return;}
        bool active=target.gameObject.activeInHierarchy;if(fox.activeSelf!=active)fox.SetActive(active);if(!active)return;
        Vector3 now=target.position;float step=Vector3.Distance(now,trail[trail.Count-1]);
        if(step>5f){trail.Clear();trail.Add(now);}else if(step>.025f)trail.Add(now);
        float remaining=followDistance;Vector3 desired=trail[0],tangent=target.forward;
        for(int i=trail.Count-1;i>0;i--){float length=Vector3.Distance(trail[i],trail[i-1]);if(length>=remaining){desired=Vector3.Lerp(trail[i],trail[i-1],remaining/length);tangent=(trail[i]-trail[i-1]).normalized;if(i>1)trail.RemoveRange(0,i-1);break;}remaining-=length;}
        // Keep the companion visible beside the trail, but fall back to its centre near walls.
        Vector3 lateral=new Vector3(-tangent.z,0,tangent.x)*.7f;Vector3 offset=desired+lateral;bool blocked=false;
        foreach(var c in Physics.OverlapSphere(offset+Vector3.up*.5f,.42f,~0,QueryTriggerInteraction.Ignore))if(c.name=="Mur"){blocked=true;break;}
        if(!blocked)desired=offset;
        fox.transform.position=desired;Vector3 delta=desired-lastPosition;delta.y=0;float speed=Time.deltaTime>0?delta.magnitude/Time.deltaTime:0;
        if(delta.sqrMagnitude>.00001f)fox.transform.rotation=Quaternion.Slerp(fox.transform.rotation,Quaternion.LookRotation(delta)*Quaternion.Euler(0,-90,0),Mathf.Clamp01(Time.deltaTime*12));
        distanceTravelled+=delta.magnitude;maxGap=Mathf.Max(maxGap,Vector3.Distance(desired,now));lastPosition=desired;
        bool hitActive=Time.time<hitUntil;
        if(!hitActive)Play(speed>.15f?run:idle,true);
        bool flashing=Time.time<flashUntil;foreach(var item in baseColors)item.Key.color=flashing?Color.Lerp(item.Value,Color.white,.72f):item.Value;
        fox.transform.localScale=Vector3.one*(flashing?1.32f:1.2f);if(flash)flash.intensity=flashing?1.1f:0;
    }
    void OnDestroy(){if(outline)Destroy(outline);foreach(var material in baseColors.Keys)if(material)Destroy(material);}
}
