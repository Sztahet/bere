using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ShadowCastingMode=UnityEngine.Rendering.ShadowCastingMode;

public partial class RiftorynTrial
{
    const int FireShot=0, LightningShot=2;
    const float LightningFadeTime=.18f;

    sealed class ShotVisual
    {
        public GameObject root;
        public int type;
        public Renderer body;
        public TrailRenderer trail;
        public LineRenderer bolt, glow;
        public float fadeEndsAt;
    }

    readonly ObjectPool<ShotVisual>[] shotPools=new ObjectPool<ShotVisual>[TowerColors.Length];
    readonly Material[] shotMaterials=new Material[TowerColors.Length];
    readonly List<ShotVisual> fadingShots=new();
    readonly Dictionary<int,ShotVisual> lanShotVisuals=new();
    readonly List<int> endedLanShots=new();
    Material shotLineMaterial;
    int nextShotId;

    Vector3 ShotOrigin(Tower tower,Vector3 target)
    {
        Vector3 muzzle=tower.obj.transform.position+Vector3.up*3f;
        if(tower.type!=FireShot) return muzzle;
        Vector3 towardTower=Vector3.ProjectOnPlane(muzzle-target,Vector3.up);
        return target+Vector3.up*7f+Vector3.ClampMagnitude(towardTower*.35f,3f);
    }

    void FireTowerShot(Tower tower,Vector3 origin,Vector3 aim,Runner target)
    {
        int type=tower.type;
        Vector3 offset=aim-origin;
        Vector3 direction=offset.sqrMagnitude>.0001f?offset.normalized:Vector3.down;
        float reach=offset.magnitude+1.5f;
        shots.Add(new Shot
        {
            visual=GetShotVisual(type,origin,direction,0),
            origin=origin,
            direction=direction,
            remaining=type==FireShot?reach:Mathf.Max(TowerRange[type],offset.magnitude+1f),
            damage=type==1?TowerDamage[type]*.25f/TowerRate[type]:TowerDamage[type],
            target=target,
            id=++nextShotId,
            type=type
        });
    }

    ShotVisual GetShotVisual(int type,Vector3 position,Vector3 direction,float travel)
    {
        var pool=shotPools[type]??=new ObjectPool<ShotVisual>(()=>CreateShotVisual(type),
            actionOnRelease:visual=>visual.root.SetActive(false),
            actionOnDestroy:visual=>Destroy(visual.root));
        var shot=pool.Get();
        MoveShotVisual(shot,position,direction,travel);
        shot.root.SetActive(true);
        if(shot.body) shot.body.enabled=type!=1;
        if(shot.trail)
        {
            shot.trail.Clear();
            shot.trail.emitting=type!=1;
        }
        if(shot.bolt) TintLightning(shot,1f);
        return shot;
    }

    void SetIceStream(Tower tower,bool visible,Vector3 target)
    {
        if(!visible)
        {
            if(tower.snowflakes!=null) foreach(var line in tower.snowflakes) line.enabled=false;
            return;
        }
        if(tower.snowflakes==null)
        {
            if(!shotLineMaterial) shotLineMaterial=new Material(Shader.Find("Sprites/Default"));
            tower.snowflakes=new LineRenderer[36];
            for(int i=0;i<tower.snowflakes.Length;i++)
            {
                var obj=new GameObject("Płatek śniegu");
                obj.transform.SetParent(tower.obj.transform,false);
                var line=tower.snowflakes[i]=obj.AddComponent<LineRenderer>();
                line.sharedMaterial=shotLineMaterial;
                line.positionCount=2;
                line.startWidth=line.endWidth=.035f;
                line.startColor=line.endColor=new Color(.65f,.9f,1f,.9f);
                line.shadowCastingMode=ShadowCastingMode.Off;
            }
        }
        Vector3 start=tower.obj.transform.position+Vector3.up*3f;
        Vector3 direction=target-start;
        if(direction.sqrMagnitude<.001f) return;
        if(Physics.Raycast(start,direction.normalized,out RaycastHit hit,direction.magnitude,~0,QueryTriggerInteraction.Ignore)) target=hit.point;
        Vector3 side=Vector3.Cross(direction.normalized,Vector3.up).normalized;
        if(side.sqrMagnitude<.01f) side=Vector3.right;
        Vector3 up=Vector3.Cross(side,direction.normalized).normalized;
        for(int i=0;i<12;i++)
        {
            float progress=Mathf.Repeat(Time.time*1.25f+i/12f,1f);
            float wobble=Mathf.Sin(Time.time*5f+i*2.4f)*.2f*progress;
            Vector3 center=Vector3.Lerp(start,target,progress)+side*wobble+up*Mathf.Cos(Time.time*4f+i)*.15f*progress;
            for(int arm=0;arm<3;arm++)
            {
                float angle=arm*Mathf.PI/3f+Time.time+i;
                Vector3 extent=(side*Mathf.Cos(angle)+up*Mathf.Sin(angle))*.13f;
                var line=tower.snowflakes[i*3+arm];line.enabled=true;line.SetPosition(0,center-extent);line.SetPosition(1,center+extent);
            }
        }
    }

    ShotVisual CreateShotVisual(int type)
    {
        var root=new GameObject("Pocisk: "+TowerNames[type]);
        root.transform.SetParent(world.transform,false);
        root.SetActive(false);
        var shot=new ShotVisual{root=root,type=type};
        if(!shotLineMaterial) shotLineMaterial=new Material(Shader.Find("Sprites/Default"));
        Color color=TowerColors[type];
        if(type==LightningShot)
        {
            shot.glow=CreateBoltLine(root.transform,"Poświata",.25f);
            shot.bolt=CreateBoltLine(root.transform,"Rdzeń",.085f);
            return shot;
        }

        bool faceted=type==1||type==4;
        var body=GameObject.CreatePrimitive(faceted?PrimitiveType.Cube:PrimitiveType.Sphere);
        body.name="Rdzeń";
        body.transform.SetParent(root.transform,false);
        var collider=body.GetComponent<Collider>();
        collider.enabled=false;
        Destroy(collider);
        shot.body=body.GetComponent<Renderer>();
        if(!shotMaterials[type])
        {
            var material=shotMaterials[type]=Mat(type==FireShot?new Color(1f,.58f,.18f):color);
            if(type!=4)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor",color*(type==FireShot?1.1f:.45f));
            }
        }
        shot.body.sharedMaterial=shotMaterials[type];
        shot.body.shadowCastingMode=ShadowCastingMode.Off;
        body.transform.localScale=type switch
        {
            FireShot=>Vector3.one*.62f,
            1=>new Vector3(.22f,.22f,.68f),
            3=>new Vector3(.31f,.26f,.51f),
            4=>new Vector3(.53f,.41f,.49f),
            5=>new Vector3(.13f,.13f,.86f),
            _=>Vector3.one*.45f
        };
        if(faceted) body.transform.localRotation=Quaternion.Euler(type==4?24f:0,0,45f);

        var trail=shot.trail=root.AddComponent<TrailRenderer>();
        trail.sharedMaterial=shotLineMaterial;
        trail.shadowCastingMode=ShadowCastingMode.Off;
        trail.emitting=false;
        trail.time=type==FireShot?.28f:.13f;
        trail.minVertexDistance=.08f;
        trail.startWidth=type==FireShot?.52f:body.transform.localScale.x;
        trail.endWidth=0;
        trail.colorGradient=new Gradient
        {
            colorKeys=new[]
            {
                new GradientColorKey(type==FireShot?new Color(1f,.72f,.29f):color,0),
                new GradientColorKey(color,.4f),
                new GradientColorKey(color*.5f,1)
            },
            alphaKeys=new[]{new GradientAlphaKey(.9f,0),new GradientAlphaKey(.5f,.45f),new GradientAlphaKey(0,1)}
        };
        return shot;
    }

    LineRenderer CreateBoltLine(Transform parent,string name,float width)
    {
        var obj=new GameObject(name);
        obj.transform.SetParent(parent,false);
        var line=obj.AddComponent<LineRenderer>();
        line.sharedMaterial=shotLineMaterial;
        line.shadowCastingMode=ShadowCastingMode.Off;
        line.useWorldSpace=false;
        line.positionCount=7;
        line.startWidth=width*.22f;
        line.endWidth=width;
        return line;
    }

    static void MoveShotVisual(ShotVisual shot,Vector3 position,Vector3 direction,float travel)
    {
        direction=direction.sqrMagnitude>.0001f?direction:Vector3.forward;
        Vector3 up=Mathf.Abs(direction.y)>.98f?Vector3.forward:Vector3.up;
        shot.root.transform.SetPositionAndRotation(position,Quaternion.LookRotation(direction,up));
        if(!shot.bolt) return;

        float length=Mathf.Min(travel,4.2f);
        shot.bolt.enabled=shot.glow.enabled=length>.01f;
        for(int i=0;i<shot.bolt.positionCount;i++)
        {
            float t=i/(float)(shot.bolt.positionCount-1);
            float amplitude=Mathf.Min(length*.12f,.28f)*Mathf.Sin(t*Mathf.PI);
            float side=i%2==0?1f:-1f;
            Vector3 point=new(side*amplitude,Mathf.Sin(i*2.8f+travel*2f)*amplitude,t*length-length);
            shot.bolt.SetPosition(i,point);
            shot.glow.SetPosition(i,point);
        }
    }

    static void TintLightning(ShotVisual shot,float opacity)
    {
        shot.bolt.startColor=new Color(1f,.8f,.35f,opacity*.35f);
        shot.bolt.endColor=new Color(1f,.98f,.83f,opacity);
        shot.glow.startColor=new Color(1f,.66f,.12f,opacity*.1f);
        shot.glow.endColor=new Color(1f,.79f,.24f,opacity*.62f);
    }

    void FadeShotVisual(ShotVisual shot)
    {
        if(shot.body) shot.body.enabled=false;
        if(shot.trail) shot.trail.emitting=false;
        shot.fadeEndsAt=Time.time+(shot.trail?shot.trail.time:LightningFadeTime);
        fadingShots.Add(shot);
    }

    void UpdateFadingShots()
    {
        for(int i=fadingShots.Count-1;i>=0;i--)
        {
            var shot=fadingShots[i];
            float remaining=shot.fadeEndsAt-Time.time;
            if(remaining<=0)
            {
                shotPools[shot.type].Release(shot);
                fadingShots.RemoveAt(i);
            }
            else if(shot.bolt) TintLightning(shot,remaining/LightningFadeTime);
        }
    }

    void ClearShotVisuals()
    {
        foreach(var shot in shots) shotPools[shot.type].Release(shot.visual);
        shots.Clear();
        ClearLanShots();
        foreach(var shot in fadingShots) shotPools[shot.type].Release(shot);
        fadingShots.Clear();
    }

    void DisposeShotVisuals()
    {
        foreach(var shot in shots) Destroy(shot.visual.root);
        foreach(var shot in lanShotVisuals.Values) Destroy(shot.root);
        foreach(var shot in fadingShots) Destroy(shot.root);
        foreach(var pool in shotPools) pool?.Dispose();
        foreach(var material in shotMaterials) if(material) Destroy(material);
        if(shotLineMaterial) Destroy(shotLineMaterial);
    }

    void ClearLanShots()
    {
        foreach(var shot in lanShotVisuals.Values) shotPools[shot.type].Release(shot);
        lanShotVisuals.Clear();
    }

    void UpdateLanShots(LanShot[] states)
    {
        if(states==null) return;
        endedLanShots.AddRange(lanShotVisuals.Keys);
        for(int i=0;i<states.Length;i++)
        {
            var state=states[i];
            int id=state.id==0?-1-i:state.id;
            int type=Mathf.Clamp(state.type,0,TowerColors.Length-1);
            Vector3 position=new(state.x,state.y,state.z);
            Vector3 direction=new(state.dx,state.dy,state.dz);
            float travel=state.travel;
            endedLanShots.Remove(id);
            if(lanShotVisuals.TryGetValue(id,out var shot))
            {
                if(shot.type==type)
                {
                    // Older hosts only send position and element.
                    if(state.id==0)
                    {
                        direction=(position-shot.root.transform.position).normalized;
                        travel=4.2f;
                    }
                    MoveShotVisual(shot,position,direction,travel);
                    continue;
                }
                shotPools[shot.type].Release(shot);
            }
            lanShotVisuals[id]=GetShotVisual(type,position,direction,travel);
        }
        foreach(int id in endedLanShots)
        {
            FadeShotVisual(lanShotVisuals[id]);
            lanShotVisuals.Remove(id);
        }
        endedLanShots.Clear();
    }
}
