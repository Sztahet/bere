using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable] public class RiftorynHeroStats { public string id; public int hp,attack,petHp,petAttack; public float speed,petSpeed; }
[Serializable] public class RiftorynHeroStatsList { public RiftorynHeroStats[] heroes; }

// A self-contained training prototype. Values are deliberately exposed in one place for playtesting.
public class RiftorynTrial : MonoBehaviour
{
    const float Scale = .15f, RoadHalf = 2.65f, ProjectileSpeed = 22f;
    const float RollDuration=.35f, RollSpeed=12f, RollCooldown=10f, TowerWindup=.45f;
    static readonly Vector2[] Map = { new(400,330),new(475,330),new(475,245),new(315,245),new(315,410),new(550,410),new(550,170),new(240,170),new(240,485),new(625,485),new(625,95),new(165,95),new(165,560),new(700,560),new(700,35) };
    static readonly Vector2[] SlotMap = {new(445,205),new(405,205),new(365,205),new(275,290),new(275,350),new(355,450),new(415,450),new(475,450),new(590,360),new(590,290),new(590,220),new(510,130),new(450,130),new(390,130),new(330,130),new(200,235),new(200,320),new(200,400),new(305,525),new(385,525),new(465,525),new(545,525),new(665,450),new(665,370),new(665,290),new(665,210)};
    const int LucenHero = 9;
    static readonly string[] HeroNames={"Kael","Rian","Zet","Koneva","Niva","Liora","Elira","Dara","Nocten","Lucen"}, TowerNames={"Ogień","Lód","Błyskawice","Natura","Ziemia","Światło","Ciemność"}, ResourceNames={"fire","ice","lightning","nature","earth","light","darkness"};
    static readonly float[] MaxHp={170,110,100,60,125,120,120,125,115,105}, Speed={4.4f,5.2f,5.6f,5.3f,4.7f,4.8f,4.8f,4.7f,4.9f,5.6f}, Cooldowns={22,16,18,20,18,20,20,20,20,20}, TowerDamage={13,7,9,10,10,10,10}, TowerRate={1.4f,1.5f,1f,1.3f,1.3f,1.3f,1.3f}, TowerRange={13.8f,15f,13.8f,15f,15f,15f,15f};
    static readonly Color[] TowerColors={new(1f,.30f,.12f),new(.32f,.77f,1f),new(1f,.8f,.2f),new(.32f,.83f,.4f),new(.65f,.43f,.25f),new(1f,.91f,.55f),new(.55f,.28f,.75f)};
    class Runner { public int hero; public GameObject obj; public Vector3 velocity; public float hp,shield,shieldMax,shieldTick,cooldown,attackCooldown,boost,exposure,fogTick,along,damage; public bool done; public int uses; public float chillTime, hitFlash, slowImmunity; public readonly RiftorynPeriodicEffect burn=new(), poison=new(); }
    class FloatingCombatText { public Transform anchor; public Vector3 position; public string text; public Color color; public float started, offset; }
    class Tower { public int slot,type; public float hp=100,wait,disabled; public GameObject obj, marker; public LineRenderer range, warning; public Runner target; public RiftorynPatkaPreview petTarget; public float windup; }
    class Shot { public GameObject obj; public Vector3 direction; public float remaining,damage; public Runner target; public RiftorynPatkaPreview petTarget; public int type; }
    enum Phase { Setup, Playing, Finished }
    public bool menuOpen = true;
    Phase phase; int selectedHero=0, allyChoice=0, selectedType=0, selectingHero=-1, draftPreview=-1; bool initialDraft, briefing; CodexHero[] roster=Array.Empty<CodexHero>(); CodexRoleBonus[] roleBonuses=Array.Empty<CodexRoleBonus>(); RiftorynHeroStats[] heroStats=Array.Empty<RiftorynHeroStats>(); Vector2 abilityTooltipScroll; string lastAbilityTooltip=""; float abilityTooltipUntil; readonly Dictionary<string,Texture2D> draftPortraits=new(); bool custom, genericModelPreview; float elapsed,total, cameraYaw, pitch=18f, messageTimer, rollTime, rollCooldown; Vector3 rollDirection; string message="", result=""; int floatingSequence; readonly List<FloatingCombatText> combatTexts=new(); Runner player,ally; Camera cam; GameObject world,runnerRoot,slotRoot,fogRoot; GameObject[] slots; readonly List<Tower> towers=new(); readonly List<Shot> shots=new(); readonly List<GameObject> fogTiles=new(); readonly List<float> fogDistances=new(); float[] lengths; Mesh fogMesh; Material fogMaterial; GUIStyle title, small, heading, floatingStyle, hudCenterStyle, hudStatusStyle, draftHeroTitle, draftRoleStyle, draftCompactNameStyle,draftResistanceNameStyle,draftResistanceValueStyle; readonly Dictionary<string,Texture2D> statIcons=new(), statusIcons=new(); bool mouseCaptured;
    [Serializable] class LanInput { public float x,z; public bool ability,attack; }
    [Serializable] class LanRunner { public float x,z,yaw,hp,shield,shieldMax,cooldown,exposure,damage; public bool done; public int uses; }
    [Serializable] class LanShot { public float x,y,z; public int type; }
    [Serializable] class LanState { public float time,healTime,healX,healZ; public bool finished; public int hostHero,guestHero; public LanRunner host,guest; public float[] towerHp,towerDisabled; public LanShot[] shots; }
    RiftorynLan lan; bool lanPanel,lanPlaying; string lanAddress="127.0.0.1"; float lanSendAt,lanInputAt; Vector3 lanRemoteInput; bool lanRemoteAbility,lanRemoteAttack; int lanHero; string lanNotice=""; float healingTime,healingTick; Vector3 healingPosition; LineRenderer healingRing; readonly List<GameObject> lanShotVisuals=new();
    Vector3 P(Vector2 p) => new((p.x-400)*Scale,0,(330-p.y)*Scale);
    Vector3 Slot(int i)=>P(SlotMap[i]);
    void Start(){Application.targetFrameRate=60; lengths=new float[Map.Length-1];for(int i=0;i<lengths.Length;i++){lengths[i]=Vector2.Distance(Map[i],Map[i+1])*Scale;total+=lengths[i];} var data=Resources.Load<TextAsset>("codex");if(data){var catalog=JsonUtility.FromJson<CodexRoster>(data.text);if(catalog!=null&&catalog.heroes!=null){roster=catalog.heroes;roleBonuses=catalog.roleBonuses??Array.Empty<CodexRoleBonus>();}}var statsData=Resources.Load<TextAsset>("hero_stats");if(statsData){var parsed=JsonUtility.FromJson<RiftorynHeroStatsList>(statsData.text);if(parsed!=null&&parsed.heroes!=null)heroStats=parsed.heroes;}BuildWorld(); Setup(); gameObject.AddComponent<RiftorynMenu>();}
    Material Mat(Color c){var m=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard"));m.color=c;return m;}
    readonly Dictionary<Color,Material> worldMaterials=new();
    Texture2D wallTexture, floorTexture; Material floorMaterial;
    GameObject Cube(string name,Vector3 pos,Vector3 size,Color c,Transform parent,bool collision=true){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent);g.transform.position=pos;g.transform.localScale=size;if(!worldMaterials.TryGetValue(c,out var material)){material=Mat(c);worldMaterials.Add(c,material);}g.GetComponent<Renderer>().sharedMaterial=material;if(!collision)Destroy(g.GetComponent<Collider>());return g;}
    // A repeatable, staggered masonry pattern; all wall tiles share one texture.
    Texture2D MakeWallTexture(){
      const int size=256, rowHeight=64, blockWidth=128;var texture=new Texture2D(size,size,TextureFormat.RGBA32,true);texture.name="Kamienny mur";texture.wrapMode=TextureWrapMode.Repeat;texture.filterMode=FilterMode.Bilinear;
      var pixels=new Color32[size*size];for(int y=0;y<size;y++)for(int x=0;x<size;x++){
        int row=y/rowHeight, localY=y%rowHeight, shifted=(x+(row%2)*64)%size, localX=shifted%blockWidth, block=shifted/blockWidth;
        int hash=(row*131+block*71+17)*1103515245;float variation=((hash>>16)&15)/100f-.075f;
        int noise=((x*37+y*71+x*y*3)&15);float grain=(noise-7)*.003f;
        bool joint=localY<3||localX<3;bool edge=localY<6||localX<6||localY>60||localX>124;
        float value=joint?.32f:edge?.70f:.90f+variation+grain;byte v=(byte)Mathf.Clamp(Mathf.RoundToInt(value*255),0,255);pixels[y*size+x]=new Color32(v,v,v,255);
      }texture.SetPixels32(pixels);texture.Apply(true,false);return texture;
    }
    Texture2D MakeFloorTexture(){
      const int size=256, tile=64;var texture=new Texture2D(size,size,TextureFormat.RGBA32,true){name="Płyty posadzki",wrapMode=TextureWrapMode.Repeat,filterMode=FilterMode.Bilinear};var pixels=new Color32[size*size];
      for(int y=0;y<size;y++)for(int x=0;x<size;x++){int row=y/tile, lx=(x+(row%2)*32)%tile, ly=y%tile, column=((x+(row%2)*32)%size)/tile;int hash=(row*37+column*19+11);float shade=.80f+(hash%7)*.025f+(((x*13+y*31+x*y)&15)-7)*.0025f;bool joint=lx<2||ly<2;bool rim=lx<5||ly<5||lx>61||ly>61;float value=joint?.38f:rim?shade*.78f:shade;byte v=(byte)Mathf.Clamp(Mathf.RoundToInt(value*255),0,255);pixels[y*size+x]=new Color32(v,v,v,255);}
      texture.SetPixels32(pixels);texture.Apply(true,false);return texture;
    }
    float Project(Vector3 point,out float distance){float best=float.MaxValue,along=0,acc=0;for(int i=0;i<lengths.Length;i++){Vector3 a=P(Map[i]),b=P(Map[i+1]),v=b-a,w=point-a;float t=Mathf.Clamp01(Vector3.Dot(w,v)/v.sqrMagnitude);float d=Vector2.Distance(new Vector2(point.x,point.z),new Vector2(a.x+v.x*t,a.z+v.z*t));if(d<best){best=d;along=acc+t*lengths[i];}acc+=lengths[i];}distance=best;return along;}
    Vector3 At(float along){for(int i=0;i<lengths.Length;i++){if(along<=lengths[i])return Vector3.Lerp(P(Map[i]),P(Map[i+1]),along/lengths[i]);along-=lengths[i];}return P(Map[Map.Length-1]);}
    void BuildWorld(){world=new GameObject("Riftoryn | Labirynt");runnerRoot=new GameObject("Runnerzy");slotRoot=new GameObject("Pola budowy");fogRoot=new GameObject("Mgła");
      Cube("Fundament",new Vector3(3,-.35f,0),new Vector3(130,.5f,130),new Color(.065f,.055f,.10f),world.transform);
      // One world-aligned surface avoids overlapping coplanar road pieces at turns.
      floorTexture=MakeFloorTexture();floorMaterial=Mat(new Color(.26f,.24f,.30f));floorMaterial.mainTexture=floorTexture;floorMaterial.mainTextureScale=new Vector2(25,25);if(floorMaterial.HasProperty("_Glossiness"))floorMaterial.SetFloat("_Glossiness",0f);
      var floor=GameObject.CreatePrimitive(PrimitiveType.Cube);floor.name="Kamienna posadzka";floor.transform.SetParent(world.transform);floor.transform.position=new Vector3(3,.02f,0);floor.transform.localScale=new Vector3(130,.15f,130);floor.GetComponent<Renderer>().sharedMaterial=floorMaterial;Destroy(floor.GetComponent<Collider>());
      // Wider wall tops provide room around towers while the road clearance stays unchanged.
      wallTexture=MakeWallTexture();const float grid=1.5f;for(float x=-49.5f;x<=55.5f;x+=grid)for(float z=-49.5f;z<=55.5f;z+=grid){Vector3 pos=new(x,0,z);Project(pos,out float d);if(d>RoadHalf+1.1f&&d<RoadHalf+5f){int variant=Mathf.Abs(Mathf.RoundToInt(x/grid)*13+Mathf.RoundToInt(z/grid)*7)%3;Color stone=variant==0?new Color(.25f,.23f,.30f):variant==1?new Color(.29f,.27f,.34f):new Color(.32f,.29f,.36f);var wall=Cube("Mur",pos+Vector3.up*1.4f,new Vector3(grid,2.8f,grid),stone,world.transform);var wallMaterial=wall.GetComponent<Renderer>().sharedMaterial;wallMaterial.mainTexture=wallTexture;if(wallMaterial.HasProperty("_Glossiness"))wallMaterial.SetFloat("_Glossiness",0f);Cube("Korona muru",pos+Vector3.up*2.77f,new Vector3(grid-.04f,.10f,grid-.04f),new Color(.37f,.33f,.41f),world.transform,false);}}
      BuildFinishGate();foreach(int segment in new[]{2,6,10})BuildLandmark(segment);BuildDirectionMarks();
      fogMesh=CreateFogMesh();
      fogMaterial=new Material(Resources.Load<Shader>("RiftorynFog"));
      for(float a=0;a<=total;a+=2f){var tile=new GameObject("Mgła");tile.transform.SetParent(fogRoot.transform);tile.transform.position=At(a);tile.AddComponent<MeshFilter>().sharedMesh=fogMesh;var renderer=tile.AddComponent<MeshRenderer>();renderer.sharedMaterial=fogMaterial;renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;renderer.receiveShadows=false;tile.SetActive(false);fogTiles.Add(tile);fogDistances.Add(a);}
      slots=new GameObject[SlotMap.Length];for(int i=0;i<slots.Length;i++){var o=GameObject.CreatePrimitive(PrimitiveType.Cylinder);o.name="Pole "+(i+1);o.transform.SetParent(slotRoot.transform);o.transform.position=Slot(i)+Vector3.up*2.9f;o.transform.localScale=new Vector3(1.5f,.05f,1.5f);o.GetComponent<Renderer>().sharedMaterial=Mat(new Color(.67f,.49f,.22f));o.GetComponent<Collider>().isTrigger=true;slots[i]=o;}
      var portal=GameObject.CreatePrimitive(PrimitiveType.Cylinder);portal.name="META";portal.transform.position=At(total)+Vector3.up*.15f;portal.transform.localScale=new Vector3(3,.12f,3);portal.GetComponent<Renderer>().sharedMaterial=Mat(new Color(.95f,.7f,.23f));Destroy(portal.GetComponent<Collider>());
      var lightObj=new GameObject("Światło");var light=lightObj.AddComponent<Light>();light.type=LightType.Directional;light.color=new Color(1f,.88f,.78f);light.intensity=1.1f;lightObj.transform.rotation=Quaternion.Euler(48,-35,0);RenderSettings.ambientLight=new Color(.39f,.36f,.49f);cam=new GameObject("Kamera").AddComponent<Camera>();cam.tag="MainCamera";cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=new Color(.085f,.065f,.15f);cam.gameObject.AddComponent<AudioListener>();}
    void BuildFinishGate(){
      Vector3 end=At(total), forward=(P(Map[Map.Length-1])-P(Map[Map.Length-2])).normalized, side=new Vector3(-forward.z,0,forward.x);
      Color stone=new(.29f,.25f,.35f), gold=new(.68f,.49f,.22f), violet=new(.48f,.24f,.72f);
      for(int s=-1;s<=1;s+=2){Vector3 foot=end+side*(s*2.15f);Cube("Filar mety",foot+Vector3.up*1.8f,new Vector3(.75f,3.6f,.75f),stone,world.transform,false);Cube("Złota opaska",foot+Vector3.up*2.75f,new Vector3(.82f,.14f,.82f),gold,world.transform,false);var crystal=Cube("Kryształ mety",foot+Vector3.up*4.05f,new Vector3(.48f,.75f,.48f),violet,world.transform,false);crystal.transform.rotation=Quaternion.Euler(0,45,45);}
      var lintel=Cube("Nadproże mety",end+Vector3.up*3.65f,new Vector3(4.9f,.35f,.65f),stone,world.transform,false);lintel.transform.rotation=Quaternion.LookRotation(forward);
    }
    void BuildLandmark(int segment){
      const float grid=1.5f;
      Vector3 a=P(Map[segment]),b=P(Map[segment+1]),forward=(b-a).normalized,side=new Vector3(-forward.z,0,forward.x),center=Vector3.Lerp(a,b,.5f);Color stone=new(.22f,.19f,.28f),trim=new(.51f,.39f,.29f);
      // Match the wall grid and only build an arch when both supports have a wall below.
      center=new Vector3(Mathf.Round(center.x/grid)*grid,0,Mathf.Round(center.z/grid)*grid);
      Vector3[] supports=new Vector3[2];
      for(int index=0;index<2;index++){
        int sign=index==0?-1:1;bool found=false;
        for(int n=1;n<=6;n++){Vector3 p=center+side*(sign*n*grid);Project(p,out float d);if(d>RoadHalf+1.1f&&d<RoadHalf+5f){supports[index]=p;found=true;break;}}
        if(!found)return;
      }
      center=(supports[0]+supports[1])*.5f;
      foreach(Vector3 p in supports){Cube("Filar łuku",p+Vector3.up*3.47f,new Vector3(.55f,1.3f,.55f),stone,world.transform,false);Cube("Głowica łuku",p+Vector3.up*4.08f,new Vector3(.76f,.16f,.76f),trim,world.transform,false);}
      var beam=Cube("Łuk orientacyjny",center+Vector3.up*4.28f,new Vector3(Vector3.Distance(supports[0],supports[1])+.55f,.40f,.55f),stone,world.transform,false);beam.transform.rotation=Quaternion.LookRotation(forward);
      var crest=Cube("Znak łuku",center+Vector3.up*4.51f,new Vector3(.55f,.55f,.20f),trim,world.transform,false);crest.transform.rotation=Quaternion.LookRotation(forward)*Quaternion.Euler(0,0,45);
      var lamp=new GameObject("Światło łuku");lamp.transform.SetParent(world.transform);lamp.transform.position=center+Vector3.up*3.95f;var light=lamp.AddComponent<Light>();light.type=LightType.Point;light.color=new Color(1f,.73f,.43f);light.range=7f;light.intensity=.65f;light.shadows=LightShadows.None;
    }
    void BuildDirectionMarks(){
      Color gold=new(.62f,.47f,.29f);
      for(int i=0;i<lengths.Length;i++){Vector3 a=P(Map[i]),b=P(Map[i+1]),forward=(b-a).normalized,side=new Vector3(-forward.z,0,forward.x);for(float d=5;d<lengths[i]-4;d+=13){Vector3 tip=a+forward*d+Vector3.up*.108f;for(int sign=-1;sign<=1;sign+=2){Vector3 tail=tip-forward*.55f+side*(sign*.38f),mid=(tip+tail)*.5f;var stroke=Cube("Znak kierunku",mid,new Vector3(.065f,.012f,(tip-tail).magnitude),gold,world.transform,false);stroke.transform.rotation=Quaternion.LookRotation(tip-tail);}}}
    }
    // Several soft, crossed layers give the low fog volume from both overhead and TPP views.
    Mesh CreateFogMesh()
    {
        var vertices=new List<Vector3>(); var uv=new List<Vector2>(); var triangles=new List<int>();
        void Quad(Vector3 a,Vector3 b,Vector3 c,Vector3 d)
        {
            int i=vertices.Count; vertices.AddRange(new[]{a,b,c,d});
            uv.AddRange(new[]{new Vector2(0,0),new Vector2(1,0),new Vector2(1,1),new Vector2(0,1)});
            triangles.AddRange(new[]{i,i+1,i+2,i,i+2,i+3});
        }
        const float r=2.8f;
        foreach(float h in new[]{.25f,.65f,1.15f,1.75f,2.25f})
            Quad(new Vector3(-r,h,-r),new Vector3(r,h,-r),new Vector3(r,h,r),new Vector3(-r,h,r));
        Quad(new Vector3(-r,.10f,0),new Vector3(r,.10f,0),new Vector3(r,2.65f,0),new Vector3(-r,2.65f,0));
        Quad(new Vector3(0,.10f,-r),new Vector3(0,.10f,r),new Vector3(0,2.65f,r),new Vector3(0,2.65f,-r));
        var mesh=new Mesh{name="Warstwy mgły"};mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(triangles,0);mesh.RecalculateBounds();return mesh;
    }
    void OnDestroy(){if(lan)lan.Stop();ClearLanShots();if(healingRing&&healingRing.material)Destroy(healingRing.material);if(fogMesh)Destroy(fogMesh);if(fogMaterial)Destroy(fogMaterial);foreach(var material in worldMaterials.Values)if(material)Destroy(material);worldMaterials.Clear();if(wallTexture)Destroy(wallTexture);if(floorTexture)Destroy(floorTexture);if(floorMaterial)Destroy(floorMaterial);}
    Runner MakeRunner(int h,bool bot){var go=new GameObject(bot?"Sojusznik AI":"Gracz");go.transform.SetParent(runnerRoot.transform);go.transform.position=At(0)+(bot?Vector3.right*.8f:Vector3.zero);var body=GameObject.CreatePrimitive(PrimitiveType.Capsule);body.name="Bohater";body.transform.SetParent(go.transform,false);body.transform.localPosition=Vector3.up;body.transform.localScale=new Vector3(.85f,1,.85f);body.GetComponent<Renderer>().sharedMaterial=Mat(bot?new Color(.16f,.75f,.85f):new Color(.88f,.71f,.36f));body.GetComponent<Collider>().isTrigger=true;if(!bot&&genericModelPreview)AttachGenericPreview(go,body);if(h==3){body.GetComponent<Renderer>().sharedMaterial=Mat(new Color(.22f,.12f,.28f));var prefab=Resources.Load<GameObject>("Pets/patka");if(prefab){var pet=Instantiate(prefab,go.transform);pet.name="Patka";pet.transform.localPosition=new Vector3(0,.05f,2f);pet.transform.localRotation=Quaternion.Euler(0,180,0);pet.transform.localScale=Vector3.one*.42f;var ph=pet.AddComponent<RiftorynPetHealth>();ph.petName="Patka";ph.maxHp=ph.currentHp=50;pet.AddComponent<RiftorynPatkaPreview>();}}var r=new Runner{hero=h,obj=go,hp=MaxHp[h]};return r;}
    void AttachGenericPreview(GameObject runner,GameObject capsule)
    {
        var prefab=Resources.Load<GameObject>("Characters/generic-prototype");
        if(!prefab){Debug.LogWarning("Brak modelu testowego postaci; użyto kapsuły.");return;}
        var model=Instantiate(prefab,capsule.transform);
        model.name="Model testowy";
        var renderers=model.GetComponentsInChildren<Renderer>();
        if(renderers.Length==0){Destroy(model);Debug.LogWarning("Model testowy nie zawiera siatki; użyto kapsuły.");return;}
        Bounds bounds=renderers[0].bounds;foreach(var r in renderers)bounds.Encapsulate(r.bounds);
        if(bounds.size.y<=.001f){Destroy(model);Debug.LogWarning("Model testowy ma niepoprawny rozmiar; użyto kapsuły.");return;}
        model.transform.localScale*=1.8f/bounds.size.y;
        bounds=renderers[0].bounds;foreach(var r in renderers)bounds.Encapsulate(r.bounds);
        model.transform.position+=runner.transform.position-new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);
        capsule.GetComponent<Renderer>().enabled=false;
    }
    void Setup(){healingTime=0;healingTick=0;if(healingRing)healingRing.enabled=false;briefing=false;combatTexts.Clear();floatingSequence=0;selectingHero=-1;phase=Phase.Setup;elapsed=0;rollTime=rollCooldown=0;rollDirection=Vector3.zero;result="";message="";mouseCaptured=false;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;foreach(var s in shots)if(s.obj)Destroy(s.obj);shots.Clear();foreach(var t in towers)if(t.obj)Destroy(t.obj);towers.Clear();ReplaceRunners();foreach(var f in fogTiles)f.SetActive(false);for(int i=0;i<slots.Length;i++)slots[i].SetActive(custom);if(!custom){int[] ss={2,5,9,15,23};for(int i=0;i<ss.Length;i++)AddTower(ss[i],i%4);}SetOverhead();}
    int AllyHero()=>allyChoice==0?(selectedHero==2?0:2):allyChoice==1?-1:allyChoice-2;
    void ReplaceRunners()
    {
        // Destroy only the currently referenced runners: Destroy is deferred until end of frame.
        if(player!=null&&player.obj)Destroy(player.obj);
        if(ally!=null&&ally.obj)Destroy(ally.obj);
        player=MakeRunner(selectedHero,false);
        int hero=AllyHero();ally=hero>=0?MakeRunner(hero,true):null;
    }
    void SetOverhead(){cam.orthographic=true;cam.orthographicSize=58;cam.transform.position=new Vector3(3,100,-2);cam.transform.rotation=Quaternion.Euler(90,0,0);}
    void AddTower(int slot,int type){var root=new GameObject("Wieża "+TowerNames[type]);root.transform.position=Slot(slot)+Vector3.up*2.9f;Color accent=Color.Lerp(TowerColors[type],new Color(.30f,.27f,.34f),.35f);for(int n=0;n<4;n++){float angle=n*Mathf.PI*.5f;Vector3 offset=new Vector3(Mathf.Cos(angle)*1.05f,.07f,Mathf.Sin(angle)*1.05f);var rune=Cube("Runa żywiołu",root.transform.position+offset,new Vector3(.24f,.06f,.24f),accent,root.transform,false);rune.transform.rotation=Quaternion.Euler(0,45,0);}var prefab=Resources.Load<GameObject>("Towers/"+ResourceNames[type]);if(prefab){var model=Instantiate(prefab,root.transform);var renderers=model.GetComponentsInChildren<Renderer>();if(renderers.Length>0){Bounds bounds=renderers[0].bounds;foreach(var r in renderers)bounds.Encapsulate(r.bounds);float h=bounds.size.y;if(h>.001f){float k=3f/h;model.transform.localScale*=k;bounds=new Bounds(renderers[0].bounds.center,Vector3.zero);foreach(var r in renderers)bounds.Encapsulate(r.bounds);model.transform.position+=root.transform.position-new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);}}}else{var body=GameObject.CreatePrimitive(PrimitiveType.Cylinder);body.transform.SetParent(root.transform,false);body.transform.localPosition=Vector3.up;body.transform.localScale=new Vector3(1,1,1);body.GetComponent<Renderer>().sharedMaterial=Mat(TowerColors[type]);Destroy(body.GetComponent<Collider>());}var beacon=new GameObject("Blask żywiołu");beacon.transform.SetParent(root.transform,false);beacon.transform.localPosition=Vector3.up*2.8f;var glow=beacon.AddComponent<Light>();glow.type=LightType.Point;glow.color=TowerColors[type];glow.range=5f;glow.intensity=.5f;glow.shadows=LightShadows.None;var marker=GameObject.CreatePrimitive(PrimitiveType.Sphere);marker.transform.SetParent(root.transform,false);marker.transform.localPosition=Vector3.up*3.2f;marker.transform.localScale=Vector3.one*.55f;marker.GetComponent<Renderer>().sharedMaterial=Mat(TowerColors[type]);Destroy(marker.GetComponent<Collider>());var line=root.AddComponent<LineRenderer>();line.useWorldSpace=true;line.loop=true;line.positionCount=64;line.startWidth=line.endWidth=.08f;line.material=new Material(Shader.Find("Sprites/Default"));line.startColor=line.endColor=new Color(TowerColors[type].r,TowerColors[type].g,TowerColors[type].b,.6f);for(int i=0;i<64;i++){float a=i*Mathf.PI*2/64;line.SetPosition(i,root.transform.position+new Vector3(Mathf.Cos(a)*TowerRange[type],.1f,Mathf.Sin(a)*TowerRange[type]));}var warningObject=new GameObject("Ostrzeżenie przed strzałem");warningObject.transform.SetParent(root.transform,false);var warning=warningObject.AddComponent<LineRenderer>();warning.useWorldSpace=true;warning.positionCount=2;warning.startWidth=.09f;warning.endWidth=.04f;warning.material=new Material(Shader.Find("Sprites/Default"));warning.startColor=warning.endColor=new Color(TowerColors[type].r,TowerColors[type].g,TowerColors[type].b,.9f);warning.enabled=false;towers.Add(new Tower{slot=slot,type=type,obj=root,range=line,marker=marker,warning=warning});}
    void Begin(){phase=Phase.Playing;foreach(var s in slots)s.SetActive(false);foreach(var t in towers)t.range.enabled=false;cameraYaw=90;Capture(true);}
    void Capture(bool value){mouseCaptured=value;Cursor.lockState=value?CursorLockMode.Locked:CursorLockMode.None;Cursor.visible=!value;}
    public void OpenTraining(){menuOpen=false;Setup();StartDraft(0,true);}
    public void ReturnToMenu(){StopLan();menuOpen=true;Capture(false);Setup();}
    void Update(){if(menuOpen)return;if(player==null||!player.obj||(ally!=null&&!ally.obj)){if(runnerRoot&&cam)Setup();return;}if(messageTimer>0)messageTimer-=Time.deltaTime;if(lanPlaying && lan!=null && !lan.IsHost){UpdateLanClient();return;}if(phase==Phase.Setup){if(!briefing&&selectingHero<0&&custom&&RiftorynInput.PointerPressed&&RiftorynInput.PointerPosition.y<Screen.height-155){Ray ray=cam.ScreenPointToRay(RiftorynInput.PointerPosition);Plane plane=new(Vector3.up,new Vector3(0,2.9f,0));if(plane.Raycast(ray,out float dist)){Vector3 pos=ray.GetPoint(dist);int nearest=-1;float best=1.3f;for(int i=0;i<slots.Length;i++){float d=Vector3.Distance(pos,slots[i].transform.position);if(d<best){best=d;nearest=i;}}if(nearest>=0){int index=towers.FindIndex(t=>t.slot==nearest);if(index>=0){Destroy(towers[index].obj);towers.RemoveAt(index);}else if(towers.Count<5)AddTower(nearest,selectedType);}}}return;}if(phase!=Phase.Playing)return;
      float dt=Mathf.Min(Time.deltaTime,.05f);elapsed+=dt;rollCooldown=Mathf.Max(0,rollCooldown-dt);if(RiftorynInput.Escape)Capture(!mouseCaptured);if(mouseCaptured){cameraYaw+=RiftorynInput.Look.x*3;pitch=Mathf.Clamp(pitch-RiftorynInput.Look.y*2,0,60);}Vector3 playerBefore=player.obj.transform.position;Vector3 allyBefore=ally!=null?ally.obj.transform.position:Vector3.zero;if(player.hp>0&&!player.done){if(RiftorynInput.Ability)Skill(player);if(RiftorynInput.Roll)StartRoll();if(RiftorynInput.Attack&&player.hero==0)Attack(player);MovePlayer(dt);}StepRunner(player,dt);if(ally!=null){if(lanPlaying)StepLanAlly(dt);else StepBot(dt);StepRunner(ally,dt);}player.velocity=dt>0?(player.obj.transform.position-playerBefore)/dt:Vector3.zero;if(ally!=null)ally.velocity=dt>0?(ally.obj.transform.position-allyBefore)/dt:Vector3.zero;StepHealing(dt);StepTowers(dt);StepShots(dt);UpdateRunnerVisual(player,dt);if(ally!=null)UpdateRunnerVisual(ally,dt);rollTime=Mathf.Max(0,rollTime-dt);if(rollTime<=0)player.obj.transform.GetChild(0).localRotation=Quaternion.identity;float front=FogFront();for(int i=0;i<fogTiles.Count;i++)fogTiles[i].SetActive(elapsed>=5&&fogDistances[i]<front);if((player.done||player.hp<=0)&&(ally==null||ally.done||ally.hp<=0))End();if(lanPlaying)SendLanState(phase==Phase.Finished);UpdateCamera();}
    float FogFront()=>Mathf.Clamp01((elapsed-5f)/115f)*total;
    Vector3 MoveInput()
    {
        Vector3 forward=Quaternion.Euler(0,cameraYaw,0)*Vector3.forward,right=Quaternion.Euler(0,cameraYaw,0)*Vector3.right;
        return Vector3.ClampMagnitude(forward*RiftorynInput.Movement.y+right*RiftorynInput.Movement.x,1f);
    }
    void StartRoll()
    {
        if(rollCooldown>0||rollTime>0)return;
        Vector3 input=MoveInput();
        rollDirection=input.sqrMagnitude>.01f?input.normalized:player.obj.transform.forward;
        rollTime=RollDuration;rollCooldown=RollCooldown;
    }
    void MovePlayer(float dt)
    {
        Vector3 dir=rollTime>0?rollDirection:MoveInput();
        float mult=player.exposure>0?.9f:1f;if(player.boost>0)mult*=1.2f;if(player.chillTime>0)mult*=player.hero==4?.88f:.85f;
        float speed=rollTime>0?RollSpeed:Speed[player.hero]*mult;
        player.obj.transform.position=MoveAgainstWalls(player.obj.transform.position,dir*speed*dt);
        if(dir.sqrMagnitude>.01f)player.obj.transform.rotation=Quaternion.LookRotation(dir);
        if(rollTime>0)player.obj.transform.GetChild(0).localRotation=Quaternion.Euler(360f*(1f-rollTime/RollDuration),0,0);
        if(Vector3.Distance(player.obj.transform.position,At(total))<1.5f)player.done=true;
    }
    Vector3 MoveAgainstWalls(Vector3 position,Vector3 remaining)
    {
        // Sweep against the same visible blocks that stop projectiles, then slide along them.
        for(int pass=0;pass<3&&remaining.sqrMagnitude>.000001f;pass++){
            float length=remaining.magnitude;Vector3 direction=remaining/length;RaycastHit closest=new RaycastHit();float nearest=length+.01f;bool blocked=false;
            foreach(var hit in Physics.SphereCastAll(position+Vector3.up, .42f, direction, length+.01f, ~0, QueryTriggerInteraction.Ignore))
                if(hit.collider.name=="Mur"&&hit.distance<nearest){closest=hit;nearest=hit.distance;blocked=true;}
            if(!blocked){position+=remaining;break;}
            float travel=Mathf.Max(0,nearest-.01f);position+=direction*travel;
            remaining=Vector3.ProjectOnPlane(direction*(length-travel),closest.normal);
        }
        return position;
    }
    void StepRunner(Runner r,float dt){if(r.hp<=0||r.done)return;r.burn.Advance(dt,amount=>{if(r.hp>0)Damage(r,amount,true,false,"PODPALENIE");});r.poison.Advance(dt,amount=>{if(r.hp>0)Damage(r,amount,true,false,"TRUCIZNA");});r.chillTime=Mathf.Max(0,r.chillTime-dt);r.slowImmunity=Mathf.Max(0,r.slowImmunity-dt);if(r.hp<=0)return;r.cooldown=Mathf.Max(0,r.cooldown-dt);r.attackCooldown=Mathf.Max(0,r.attackCooldown-dt);r.boost=Mathf.Max(0,r.boost-dt);if(r.shield>0){r.shieldTick-=dt;while(r.shield>0&&r.shieldTick<=0){r.shield*=.9f;r.shieldTick+=1f;if(r.shield<.01f){r.shield=0;r.shieldMax=0;r.shieldTick=0;}}}float along=Project(r.obj.transform.position,out _);bool fog=elapsed>=5&&along<FogFront();if(fog){r.exposure+=dt;r.fogTick+=dt;while(r.fogTick>=1){r.fogTick-=1;Damage(r,MaxHp[r.hero]*Mathf.Floor(r.exposure)/100f,true,false,"MGŁA");}}else{r.exposure=Mathf.Max(0,r.exposure-dt);r.fogTick=0;}if(r.hp<=0||r.done)r.obj.SetActive(false);}
    void AddShield(Runner r,float amount){if(r==null||amount<=0)return;if(r.shield<=0)r.shieldTick=1f;r.shield+=amount;r.shieldMax=Mathf.Max(r.shieldMax,r.shield);}
    Tower Nearest(Runner r,float range){Tower best=null;float d=range;foreach(var t in towers){if(t.hp<=0)continue;float n=Vector3.Distance(r.obj.transform.position,t.obj.transform.position-Vector3.up*2.9f);if(n<d){best=t;d=n;}}return best;}
    void Skill(Runner r)
    {
        if(r.cooldown>0)return;
        if(r.hero>=6&&r.hero!=LucenHero){if(r==player)Notify("Zdolność tej postaci nie jest jeszcze dostępna w próbie.");return;}
        if(r.hero==4){Runner other=r==player?ally:player;Runner target=other!=null&&other.hp>0&&!other.done&&other.chillTime>0&&Vector3.Distance(r.obj.transform.position,other.obj.transform.position)<=8?other:r;target.chillTime=0;target.slowImmunity=2;r.cooldown=18;r.uses++;return;}
        if(r.hero==5){healingPosition=r.obj.transform.position;healingTime=5;healingTick=1;UpdateHealingRing();r.cooldown=20;r.uses++;return;}
        Tower t=Nearest(r,17.4f);if(r.hero!=0&&r.hero!=3&&t==null){if(r==player)Notify("Podejdź bliżej wieży, aby użyć umiejętności.");return;}
        if(r.hero==3){var pet=r.obj.GetComponentInChildren<RiftorynPatkaPreview>();if(!pet||!pet.Fly()){if(r==player)Notify(RiftorynCodexData.Text("Patka jest niedostępna.","Patsy is unavailable."));return;}}
        if (r.hero == LucenHero)
        {
            t.disabled = Mathf.Max(t.disabled, 2.5f);
            r.cooldown = Cooldowns[LucenHero];
            r.uses++;
            return;
        }
        r.cooldown=Cooldowns[r.hero];r.uses++;if(r.hero==3){}else if(r.hero==0){AddShield(r,100);Runner other=r==player?ally:player;if(other!=null&&other.hp>0&&!other.done&&Vector3.Distance(r.obj.transform.position,other.obj.transform.position)<9)AddShield(other,50);}else if(r.hero==1)HitTower(t,100);else{t.disabled=2;r.boost=2;}
    }
    void StepHealing(float dt)
    {
        if(healingTime<=0)return;float step=Mathf.Min(dt,healingTime);healingTime-=step;healingTick-=step;
        while(healingTick<=.0001f){foreach(var r in new[]{player,ally})if(r!=null&&r.hp>0&&!r.done&&Vector3.Distance(r.obj.transform.position,healingPosition)<=4)r.hp=Mathf.Min(MaxHp[r.hero],r.hp+11);healingTick+=1;}UpdateHealingRing();
    }
    void UpdateHealingRing()
    {
        if(healingTime<=0){if(healingRing)healingRing.enabled=false;return;}
        if(!healingRing){var obj=new GameObject("Pole życia Liory");obj.transform.SetParent(world.transform);healingRing=obj.AddComponent<LineRenderer>();healingRing.useWorldSpace=true;healingRing.loop=true;healingRing.positionCount=64;healingRing.startWidth=healingRing.endWidth=.1f;healingRing.material=new Material(Shader.Find("Sprites/Default"));healingRing.startColor=healingRing.endColor=new Color(.25f,1f,.45f,.9f);}
        healingRing.enabled=true;for(int i=0;i<64;i++){float a=i*Mathf.PI*2/64;healingRing.SetPosition(i,healingPosition+new Vector3(Mathf.Cos(a)*4,.13f,Mathf.Sin(a)*4));}
    }
    void Attack(Runner r){if(r.attackCooldown>0)return;Tower t=Nearest(r,7.8f);if(t==null){if(r==player)Notify("Podejdź bliżej wieży, aby ją zaatakować.");return;}r.attackCooldown=1;HitTower(t,25);}
    void HitTower(Tower t,float damage){t.hp-=damage;if(t.hp<=0){t.hp=0;t.marker.SetActive(false);foreach(var ren in t.obj.GetComponentsInChildren<Renderer>())ren.material.color=Color.gray;}}
    void StepBot(float dt){if(ally.hp<=0||ally.done)return;Tower t=Nearest(ally,ally.hero==0?7.8f:17.4f);if(t!=null&&ally.cooldown<=0)Skill(ally);if(t!=null&&ally.hero==0&&ally.attackCooldown<=0)Attack(ally);float speed=Speed[ally.hero]*(ally.exposure>0?.9f:1f)*(ally.boost>0?1.2f:1)*(ally.chillTime>0?.85f:1f);ally.along=Mathf.Min(total,ally.along+speed*dt);Vector3 old=ally.obj.transform.position, next=At(ally.along);ally.obj.transform.position=next;if((next-old).sqrMagnitude>.00001f)ally.obj.transform.rotation=Quaternion.LookRotation(next-old);if(ally.along>=total){ally.done=true;ally.obj.SetActive(false);}}
    // Solve |target + velocity * time - origin| = projectileSpeed * time.
    // If interception is impossible, aim at the current position instead.
    static Vector3 PredictAim(Vector3 origin, Vector3 target, Vector3 velocity, float projectileSpeed)
    {
        Vector3 offset=target-origin;
        float a=Vector3.Dot(velocity,velocity)-projectileSpeed*projectileSpeed;
        float b=2f*Vector3.Dot(offset,velocity), c=Vector3.Dot(offset,offset);
        float time=-1f;
        if(Mathf.Abs(a)<.0001f){if(Mathf.Abs(b)>.0001f)time=-c/b;}
        else
        {
            float discriminant=b*b-4f*a*c;
            if(discriminant>=0f)
            {
                float root=Mathf.Sqrt(discriminant), t1=(-b-root)/(2f*a), t2=(-b+root)/(2f*a);
                if(t1>0f)time=t1;
                if(t2>0f&&(time<0f||t2<time))time=t2;
            }
        }
        return time>0f?target+velocity*time:target;
    }
    void StepTowers(float dt)
    {
        foreach(var t in towers)
        {
            if(t.hp<=0){t.warning.enabled=false;continue;}
            t.disabled=Mathf.Max(0,t.disabled-dt);
            if(t.disabled>0){t.warning.enabled=false;t.target=null;t.petTarget=null;t.windup=0;continue;}
            if(t.petTarget!=null)
            {
                var pet=t.petTarget;
                if(!pet.IsAlive || Vector3.Distance(pet.AimPosition,Slot(t.slot))>=TowerRange[t.type])
                {t.petTarget=null;t.windup=0;t.warning.enabled=false;continue;}
                Vector3 start=t.obj.transform.position+Vector3.up*3f, aim=pet.AimPosition;
                t.warning.enabled=true;t.warning.SetPosition(0,start);t.warning.SetPosition(1,aim);t.windup-=dt;
                if(t.windup>0)continue;
                var bolt=GameObject.CreatePrimitive(PrimitiveType.Sphere);bolt.name="Pocisk";bolt.transform.position=start;bolt.transform.localScale=Vector3.one*.38f;Destroy(bolt.GetComponent<Collider>());bolt.GetComponent<Renderer>().sharedMaterial=Mat(TowerColors[t.type]);
                shots.Add(new Shot{obj=bolt,direction=(aim-start).normalized,remaining=Mathf.Max(TowerRange[t.type],Vector3.Distance(start,aim)+1f),damage=TowerDamage[t.type],petTarget=pet,type=t.type});
                t.petTarget=null;t.warning.enabled=false;t.wait=Mathf.Max(0,TowerRate[t.type]-TowerWindup);continue;
            }
            if(t.target!=null)
            {
                if(t.target.hp<=0||t.target.done||!t.target.obj||Vector3.Distance(t.target.obj.transform.position,Slot(t.slot))>=TowerRange[t.type])
                {t.target=null;t.windup=0;t.warning.enabled=false;continue;}
                Vector3 start=t.obj.transform.position+Vector3.up*3f;
                Vector3 aim=PredictAim(start,t.target.obj.transform.position+Vector3.up,t.target.velocity,ProjectileSpeed);
                t.warning.enabled=true;t.warning.SetPosition(0,start);t.warning.SetPosition(1,aim);
                t.windup-=dt;
                if(t.windup>0)continue;
                var bolt=GameObject.CreatePrimitive(PrimitiveType.Sphere);bolt.name="Pocisk";bolt.transform.position=start;bolt.transform.localScale=Vector3.one*.38f;Destroy(bolt.GetComponent<Collider>());bolt.GetComponent<Renderer>().sharedMaterial=Mat(TowerColors[t.type]);
                shots.Add(new Shot{obj=bolt,direction=(aim-start).normalized,remaining=Mathf.Max(TowerRange[t.type],Vector3.Distance(start,aim)+1f),damage=TowerDamage[t.type],target=t.target,type=t.type});
                t.target=null;t.warning.enabled=false;t.wait=Mathf.Max(0,TowerRate[t.type]-TowerWindup);
                continue;
            }
            t.warning.enabled=false;t.wait-=dt;if(t.wait>0)continue;
            // During the maneuver the nearest flying Patka takes priority over runners.
            RiftorynPatkaPreview lure=null;float lureDistance=TowerRange[t.type];
            foreach(var r in new[]{player,ally}){if(r==null||!r.obj||r.hp<=0||r.done)continue;var pet=r.obj.GetComponentInChildren<RiftorynPatkaPreview>();if(!pet||!pet.IsFlying)continue;float distance=Vector3.Distance(pet.AimPosition,Slot(t.slot));if(distance<lureDistance){lure=pet;lureDistance=distance;}}
            if(lure){t.petTarget=lure;t.windup=TowerWindup;continue;}
            Runner target=null;RiftorynPatkaPreview nearestPet=null;float d=TowerRange[t.type];
            foreach(var r in new[]{player,ally}){if(r==null||r.hp<=0||r.done)continue;float n=Vector3.Distance(r.obj.transform.position,Slot(t.slot));if(n<d){d=n;target=r;}}
            foreach(var r in new[]{player,ally}){if(r==null||!r.obj||r.hp<=0||r.done)continue;var pet=r.obj.GetComponentInChildren<RiftorynPatkaPreview>();if(!pet||!pet.IsAlive)continue;float n=Vector3.Distance(pet.AimPosition,Slot(t.slot));if(n<d){d=n;nearestPet=pet;}}
            if(nearestPet!=null){t.petTarget=nearestPet;t.windup=TowerWindup;}
            else if(target!=null){t.target=target;t.windup=TowerWindup;}
        }
    }
    void UpdateRunnerVisual(Runner r,float dt)
    {
        if(r==null||!r.obj||r.obj.transform.childCount==0)return;
        var renderer=r.obj.transform.GetChild(0).GetComponent<Renderer>();if(!renderer)return;
        Color color=r==player?new Color(.88f,.71f,.36f):new Color(.16f,.75f,.85f);
        if(r.chillTime>0)color=Color.Lerp(color,new Color(.25f,.75f,1f),.65f);
        if(r.poison.Remaining>0)color=Color.Lerp(color,new Color(.25f,1f,.2f),.6f);
        if(r.burn.Remaining>0)color=Color.Lerp(color,new Color(1f,.28f,.08f),.6f);
        if(r.hitFlash>0){color=Color.Lerp(color,Color.white,.8f);r.hitFlash=Mathf.Max(0,r.hitFlash-dt);}
        renderer.sharedMaterial.color=color;
    }
    bool IsDodging(Runner r)=>r==player&&rollTime>0;
    bool HasLineOfSight(Vector3 from,Vector3 to)
    {
        Vector3 delta=to-from;float distance=delta.magnitude;
        return distance<.001f||!Physics.Raycast(from,delta/distance,distance,~0,QueryTriggerInteraction.Ignore);
    }
    void LightningArc(Vector3 from,Vector3 to)
    {
        var arc=new GameObject("Przeskok błyskawicy");var line=arc.AddComponent<LineRenderer>();
        line.useWorldSpace=true;line.positionCount=2;line.SetPosition(0,from);line.SetPosition(1,to);
        line.startWidth=.16f;line.endWidth=.06f;line.material=new Material(Shader.Find("Sprites/Default"));
        line.startColor=new Color(1f,.9f,.35f);line.endColor=new Color(.55f,.8f,1f);
        Destroy(line.material,.2f);Destroy(arc,.2f);
    }
    void ApplyTowerHit(Runner victim,int type,float baseDamage,Vector3 impact)
    {
        if(victim.hp<=0||victim.done)return;
        if(IsDodging(victim)){ShowCombatText(victim,"DODGE",Color.white);return;}
        float direct = baseDamage;
        if (victim.hero == 1 && type == 0) direct *= .85f;
        if (victim.hero == LucenHero && type == 2) direct *= .75f;
        Damage(victim,direct,true,true,"WIEŻA: "+TowerNames[type].ToUpperInvariant());
        if(victim.hp<=0)return;
        if(type==0)victim.burn.Apply(3,3);
        else if(type==1&&victim.slowImmunity<=0)victim.chillTime=2;
        else if(type==3)victim.poison.Apply(4,3);
    }
    void ResolveShot(Shot shot,Runner victim,Vector3 impact)
    {
        if(IsDodging(victim)){ShowCombatText(victim,"DODGE",Color.white);return;}
        if(shot.type==0)
        {
            foreach(var r in new[]{player,ally})
            {
                if(r==null||r.hp<=0||r.done)continue;
                Vector3 center=r.obj.transform.position+Vector3.up;
                if(r==victim||(Vector3.Distance(center,impact)<=2f&&HasLineOfSight(impact,center)))
                    ApplyTowerHit(r,shot.type,shot.damage,impact);
            }
        }
        else
        {
            Vector3 first=victim.obj.transform.position+Vector3.up;
            // A lethal first hit can still arc from its impact to a nearby living target.
            ApplyTowerHit(victim,shot.type,shot.damage,impact);
            if(shot.type!=2)return;
            Runner next=null;float best=4f;
            foreach(var r in new[]{player,ally})
            {
                if(r==null||r==victim||r.hp<=0||r.done)continue;
                Vector3 center=r.obj.transform.position+Vector3.up;float distance=Vector3.Distance(first,center);
                if(distance<=best&&HasLineOfSight(first,center)){best=distance;next=r;}
            }
            if(next!=null){LightningArc(first,next.obj.transform.position+Vector3.up);ApplyTowerHit(next,2,shot.damage*.5f,first);}
        }
    }
    void StepShots(float dt)
    {
        for(int i=shots.Count-1;i>=0;i--)
        {
            var s=shots[i];float step=Mathf.Min(s.remaining,ProjectileSpeed*dt);Vector3 start=s.obj.transform.position;
            bool hitWall=Physics.Raycast(start,s.direction,out RaycastHit hit,step,~0,QueryTriggerInteraction.Ignore);
            Vector3 end=hitWall?hit.point:start+s.direction*step, segment=end-start;
            Runner victim=null;float first=float.PositiveInfinity;
            foreach(var r in new[]{player,ally})
            {
                if(r==null||r.hp<=0||r.done)continue;
                Vector3 center=r.obj.transform.position+Vector3.up;
                float q=segment.sqrMagnitude>0?Mathf.Clamp01(Vector3.Dot(center-start,segment)/segment.sqrMagnitude):0;
                if(Vector3.Distance(center,start+q*segment)<.8f&&q<first){first=q;victim=r;}
            }
            RiftorynPatkaPreview petVictim=null;
            foreach(var r in new[]{player,ally}){if(r==null||!r.obj)continue;var pet=r.obj.GetComponentInChildren<RiftorynPatkaPreview>();if(!pet||!pet.IsAlive)continue;Vector3 center=pet.AimPosition;float q=segment.sqrMagnitude>0?Mathf.Clamp01(Vector3.Dot(center-start,segment)/segment.sqrMagnitude):0;if(Vector3.Distance(center,start+q*segment)<.65f&&q<first){first=q;petVictim=pet;victim=null;}}
            if(petVictim){end=start+first*segment;petVictim.ReceiveHit(s.damage);}
            else if(victim!=null){end=start+first*segment;ResolveShot(s,victim,end);}
            s.obj.transform.position=end;s.remaining-=step;
            if(hitWall||victim!=null||petVictim!=null||s.remaining<=0){Destroy(s.obj);shots.RemoveAt(i);}
        }
    }
    void Damage(Runner r,float amount,bool shielded,bool avoidable=true,string source="OBRAŻENIA")
    {
        if(avoidable&&shielded&&IsDodging(r)){ShowCombatText(r,"DODGE",Color.white);return;}
        float absorbed=shielded?Mathf.Min(r.shield,amount):0, lost=amount-absorbed;
        r.shield-=absorbed;if(r.shield<=0){r.shield=0;r.shieldMax=0;r.shieldTick=0;}r.hp-=lost;r.damage+=lost;
        if(lost>0||absorbed>0)r.hitFlash=.18f;
        if(lost>0||absorbed>0)
        {
            string text=lost>0?$"−{lost:0.#}":"";
            if(absorbed>0)text+=(lost>0?"  ":"")+RiftorynL10n.Format("OSŁONA −{0:0.#}",absorbed);
            ShowCombatText(r,text,DamageColor(source));
        }
        if(r.hp<=0){r.hp=0;r.obj.SetActive(false);}
    }
    Color DamageColor(string source)
    {
        if(source=="PODPALENIE"||source=="WIEŻA: OGIEŃ")return new Color(1f,.42f,.18f);
        if(source=="WIEŻA: LÓD")return new Color(.38f,.83f,1f);
        if(source=="WIEŻA: BŁYSKAWICE")return new Color(1f,.86f,.24f);
        if(source=="TRUCIZNA"||source=="WIEŻA: NATURA")return new Color(.48f,1f,.35f);
        if(source=="MGŁA")return new Color(.85f,.55f,1f);
        return Color.white;
    }
    void ShowCombatText(Runner runner,string text,Color color)
    {
        if(runner==null||!runner.obj)return;
        if(combatTexts.Count>=24)combatTexts.RemoveAt(0);
        combatTexts.Add(new FloatingCombatText{anchor=runner.obj.transform,position=runner.obj.transform.position,text=text,color=color,started=Time.time,offset=(floatingSequence++%5-2)*.18f});
    }
    void DrawCombatTexts()
    {
        if(Event.current.type!=EventType.Repaint||!cam)return;
        if(floatingStyle==null)floatingStyle=new GUIStyle(GUI.skin.label){fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter,clipping=TextClipping.Overflow};
        const float duration=1.35f;
        for(int i=combatTexts.Count-1;i>=0;i--)
        {
            var item=combatTexts[i];float age=Time.time-item.started;
            if(age>=duration){combatTexts.RemoveAt(i);continue;}
            float progress=Mathf.Clamp01(age/duration);
            Vector3 anchor=item.anchor?item.anchor.position:item.position;
            Vector3 world=anchor+Vector3.up*(2.25f+progress*1.15f)+cam.transform.right*item.offset;
            Vector3 screen=cam.WorldToScreenPoint(world);if(screen.z<=0)continue;
            float width=260f,height=45f;
            Rect rect=new(screen.x-width*.5f,Screen.height-screen.y-height*.5f,width,height);
            floatingStyle.fontSize=Mathf.RoundToInt(Mathf.Lerp(30f,18f,progress));
            float alpha=progress<.65f?1f:Mathf.Clamp01((1f-progress)/.35f);
            floatingStyle.normal.textColor=new Color(0,0,0,alpha*.9f);
            RiftorynL10n.Label(new Rect(rect.x+2,rect.y+2,rect.width,rect.height),item.text,floatingStyle);
            floatingStyle.normal.textColor=new Color(item.color.r,item.color.g,item.color.b,alpha);
            RiftorynL10n.Label(rect,item.text,floatingStyle);
        }
    }
    void UpdateCamera(){if(player==null)return;Vector3 focus=player.obj.transform.position+Vector3.up*1.5f;Quaternion rot=Quaternion.Euler(pitch,cameraYaw,0);Vector3 desired=focus-rot*Vector3.forward*7.5f;Vector3 direction=desired-focus;if(Physics.SphereCast(focus,.2f,direction.normalized,out RaycastHit hit,direction.magnitude,~0,QueryTriggerInteraction.Ignore))desired=focus+direction.normalized*Mathf.Max(.5f,hit.distance-.2f);cam.orthographic=false;cam.transform.position=desired;cam.transform.rotation=rot;}
    void End(){phase=Phase.Finished;Capture(false);UpdateResult();}
    void UpdateResult(){int arrived=(player.done?1:0)+(ally!=null&&ally.done?1:0),count=ally==null?1:2;result=RiftorynL10n.Format("META {0}/{1}  •  {2}: {3}",arrived,count,HeroNames[player.hero],RiftorynL10n.Translate(player.done?"meta":"pokonany"))+(ally!=null?RiftorynL10n.Format(lanPlaying?"  •  {0}: {1}":"  •  {0} AI: {1}",HeroNames[ally.hero],RiftorynL10n.Translate(ally.done?"meta":"pokonany")):"")+RiftorynL10n.Format("\nCzas {0:F1} s  •  obrażenia {1:F0}  •  zdolności {2}",elapsed,player.damage,player.uses);}
    void Notify(string text){message=text;messageTimer=3;}
    // The minimap shares the same map coordinates as the world, with north at the top.
    Vector2 MinimapPoint(Vector3 position, Rect map)
    {
        Vector2 source=new(position.x/Scale+400,330-position.z/Scale);
        return new Vector2(map.x+(source.x-135f)/595f*map.width, map.y+(source.y-5f)/585f*map.height);
    }
    void HudFill(Rect rect, Color color)
    {
        Color previous=GUI.color; GUI.color=color; GUI.DrawTexture(rect,Texture2D.whiteTexture); GUI.color=previous;
    }
    void MinimapLine(Vector2 a, Vector2 b, float width, Color color)
    {
        Matrix4x4 previous=GUI.matrix;
        GUIUtility.RotateAroundPivot(Mathf.Atan2(b.y-a.y,b.x-a.x)*Mathf.Rad2Deg,a);
        HudFill(new Rect(a.x,a.y-width*.5f,Vector2.Distance(a,b),width),color);
        GUI.matrix=previous;
    }
    void MinimapMarker(Rect map, Runner runner, Color color, string label)
    {
        if(runner==null||runner.done)return;
        Vector2 p=MinimapPoint(runner.obj.transform.position,map);
        if(runner.hp<=0)
        {
            MinimapLine(p+new Vector2(-5,-5),p+new Vector2(5,5),3,new Color(1f,.22f,.22f));
            MinimapLine(p+new Vector2(-5,5),p+new Vector2(5,-5),3,new Color(1f,.22f,.22f));
            RiftorynL10n.Label(new Rect(p.x+6,p.y-10,80,20),label+" †",small);
            return;
        }
        HudFill(new Rect(p.x-5,p.y-5,10,10),Color.black);
        HudFill(new Rect(p.x-3,p.y-3,6,6),color);
        RiftorynL10n.Label(new Rect(p.x+6,p.y-10,50,20),label,small);
    }
    void HealthBar(Rect rect, float hp, float maximum, Color color)
    {
        HudFill(rect,new Color(.12f,.10f,.14f,.95f));
        float fraction=maximum>0?Mathf.Clamp01(hp/maximum):0;
        HudFill(new Rect(rect.x+1,rect.y+1,(rect.width-2)*fraction,rect.height-2),color);
    }
    void ShieldBar(Rect rect,Runner runner)
    {
        if(runner==null||runner.shield<=0)return;
        float valueWidth=Mathf.Min(58,rect.width*.32f),barWidth=rect.width-valueWidth-5;
        Rect bar=new(rect.x,rect.y,barWidth,8);HudFill(bar,new Color(.12f,.10f,.14f,.95f));
        float fraction=runner.shieldMax>0?Mathf.Clamp01(runner.shield/runner.shieldMax):0;
        HudFill(new Rect(bar.x+1,bar.y+1,(bar.width-2)*fraction,bar.height-2),new Color(.32f,.76f,.98f));
        if(hudStatusStyle==null)hudStatusStyle=new GUIStyle(small){fontSize=10,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleLeft,wordWrap=false};
        RiftorynL10n.Label(new Rect(rect.x+barWidth+5,rect.y-3,valueWidth,14),$"{Mathf.CeilToInt(runner.shield)} OSŁ.",hudStatusStyle);
    }
    float HudRunner(Runner runner, string label, Color color, float x, float y, float width)
    {
        string status=runner.done?RiftorynL10n.Translate("META"):runner.hp<=0?RiftorynL10n.Translate("POKONANY"):$"{Mathf.CeilToInt(runner.hp)} / {Mathf.CeilToInt(MaxHp[runner.hero])} HP";
        RiftorynL10n.Label(new Rect(x,y,width,20),$"{RiftorynL10n.Translate(label)} · {HeroNames[runner.hero]}   {status}",small);
        bool inFog=RunnerInFog(runner);int statusCount=ActiveStatusCount(runner,inFog);float statusHeight=StatusHeight(statusCount);
        if(statusCount>0)DrawStatusBadges(runner,x,y+20,width,inFog);
        float shieldHeight=runner.shield>0?13:0;float hpY=y+20+statusHeight+shieldHeight;
        if(shieldHeight>0)ShieldBar(new Rect(x,hpY-11,width,8),runner);
        HealthBar(new Rect(x,hpY,width,9),runner.hp,MaxHp[runner.hero],color);
        float remaining=runner.done?0f:Mathf.Max(0,total-Project(runner.obj.transform.position,out _));
        RiftorynL10n.Label(new Rect(x,hpY+12,width,20),RiftorynL10n.Format("Do mety: {0} m",Mathf.CeilToInt(remaining)),small);
        y+=55+statusHeight+shieldHeight;
        var pet=runner.obj.GetComponentInChildren<RiftorynPetHealth>(true);
        if(pet!=null)
        {
            string petStatus=pet.currentHp<=0?RiftorynL10n.Translate("POKONANY"):$"{Mathf.CeilToInt(pet.currentHp)} / {Mathf.CeilToInt(pet.maxHp)} HP";
            RiftorynL10n.Label(new Rect(x+10,y,width-10,20),$"PET · {RiftorynL10n.Translate(pet.petName)}   {petStatus}",small);
            HealthBar(new Rect(x+10,y+20,width-10,7),pet.currentHp,pet.maxHp,new Color(.68f,.48f,.92f));
            y+=33;
        }
        return y;
    }
    bool RunnerInFog(Runner runner)=>elapsed>=5&&runner!=null&&runner.obj&&Project(runner.obj.transform.position,out _) < FogFront();
    int ActiveStatusCount(Runner runner,bool inFog)=> (runner.burn.Remaining>0?1:0)+(runner.chillTime>0?1:0)+(runner.poison.Remaining>0?1:0)+(inFog?1:0);
    float StatusHeight(int count)=>((count+1)/2)*25f;
    Texture2D StatusIcon(string name){if(!statusIcons.TryGetValue(name,out var icon)){icon=Resources.Load<Texture2D>("StatusIcons/"+name);statusIcons[name]=icon;}return icon;}
    void DrawStatusBadges(Runner runner,float x,float y,float width,bool inFog)
    {
        if(hudStatusStyle==null)hudStatusStyle=new GUIStyle(small){fontSize=10,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleLeft,wordWrap=false};
        int count=ActiveStatusCount(runner,inFog);if(count==0)return;float badgeWidth=width/(count>1?2:1);int index=0;
        void Draw(string name,string iconName,float remaining,Color tint)
        {
            int item=index++;float bx=x+(item%2)*badgeWidth,by=y+(item/2)*25;Rect badge=new(bx,by,badgeWidth-3,23);
            HudFill(badge,new Color(.08f,.06f,.1f,.94f));
            Color old=GUI.color;float textX=bx+5;
            Texture2D icon=string.IsNullOrEmpty(iconName)?null:StatusIcon(iconName);
            if(icon!=null)
            {
                GUI.color=Color.white;
                GUI.DrawTexture(new Rect(bx+3,by+2,19,19),icon,ScaleMode.ScaleToFit,true);
                textX=bx+25;
            }
            GUI.color=tint;
            string text=name+" "+RiftorynL10n.Format("{0:F1}s",Mathf.Max(0,remaining));
            GUI.Label(new Rect(textX,by+2,Mathf.Max(0,badge.xMax-textX-3),badge.height-4),text,hudStatusStyle);
            GUI.color=old;
        }
        bool en=RiftorynL10n.CurrentCode=="EN";
        if(runner.burn.Remaining>0)Draw(en?"BURN":"OPARZENIE","burning",runner.burn.Remaining,new Color(1f,.55f,.25f));
        if(runner.chillTime>0)Draw(en?"CHILL":"CHŁÓD","chill",runner.chillTime,new Color(.5f,.85f,1f));
        if(runner.poison.Remaining>0)Draw(en?"POISON":"TRUCIZNA","poison",runner.poison.Remaining,new Color(.55f,1f,.4f));
        if(inFog)Draw(en?"FOG":"MGŁA",null,runner.exposure,new Color(.85f,.55f,1f));
    }
    void DrawFogExposure(){
        if(player==null||player.exposure<=0||phase!=Phase.Playing)return;
        float strength=Mathf.Clamp01(player.exposure/15f)*.12f;float edge=Mathf.Min(Screen.width,Screen.height)*.12f;
        for(int i=0;i<4;i++){float inset=edge*i/4f,thickness=edge/4f;Color tint=new Color(.35f,.12f,.48f,strength*(1f-i/4f));HudFill(new Rect(inset,inset,Screen.width-2*inset,thickness),tint);HudFill(new Rect(inset,Screen.height-inset-thickness,Screen.width-2*inset,thickness),tint);HudFill(new Rect(inset,inset,thickness,Screen.height-2*inset),tint);HudFill(new Rect(Screen.width-inset-thickness,inset,thickness,Screen.height-2*inset),tint);}
    }
    void DrawHud()
    {
        if(player==null||!player.obj||(ally!=null&&!ally.obj))return;
        float size=Mathf.Clamp(Mathf.Min(Screen.width,Screen.height)*.28f,120f,200f);
        Rect panel=new(Screen.width-size-16,Screen.height-size-42,size,size+26);
        GUI.Box(panel,"");
        RiftorynL10n.Label(new Rect(panel.x+9,panel.y+3,size-18,20),"MAPA  ·  N ↑",small);
        Rect map=new(panel.x+8,panel.y+23,size-16,size-5);
        HudFill(map,new Color(.08f,.07f,.1f,.95f));
        for(int i=0;i<Map.Length-1;i++)MinimapLine(MinimapPoint(P(Map[i]),map),MinimapPoint(P(Map[i+1]),map),Mathf.Max(2f,size*.023f),new Color(.43f,.40f,.39f));
        Vector2 goal=MinimapPoint(At(total),map);
        HudFill(new Rect(goal.x-3,goal.y-3,6,6),new Color(1f,.74f,.2f));
        MinimapMarker(map,player,new Color(1f,.75f,.25f),"TY");
        MinimapMarker(map,ally,new Color(.2f,.85f,.95f),"AI");
        if(ally!=null)
        {
            float hudWidth=Mathf.Min(210f,size+8f);
            int petCount=ally.obj.GetComponentInChildren<RiftorynPetHealth>(true)!=null?1:0;
            float statusHeight=StatusHeight(ActiveStatusCount(ally,RunnerInFog(ally)));
            float height=90+statusHeight+petCount*33;
            Rect team=new(16,Screen.height-height-16,hudWidth+20,height);
            GUI.Box(team,"");
            HudRunner(ally,"SOJUSZNIK",new Color(.18f,.72f,.82f),team.x+10,team.y+5,hudWidth);
        }
        DrawPlayerHealth(size);
    }
    void DrawPlayerHealth(float minimapSize)
    {
        float width=Mathf.Clamp(Screen.width-2f*(minimapSize+48f),180f,370f);
        float x=(Screen.width-width)*.5f, y=Screen.height-68f;
        var pet=player.obj.GetComponentInChildren<RiftorynPetHealth>(true);
        if(hudCenterStyle==null)hudCenterStyle=new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter,fontStyle=FontStyle.Bold,fontSize=15,normal={textColor=Color.white}};
        bool inFog=RunnerInFog(player);int statusCount=ActiveStatusCount(player,inFog);float shieldHeight=player.shield>0?13:0;float statusY=y-(statusCount>0?StatusHeight(statusCount)+4+shieldHeight:6+shieldHeight);
        HudFill(new Rect(x-5,y-5-shieldHeight,width+10,52+shieldHeight),new Color(.045f,.035f,.055f,.92f));
        if(statusCount>0)DrawStatusBadges(player,x,statusY,width,inFog);
        if(shieldHeight>0)ShieldBar(new Rect(x,y-12,width,8),player);
        if(pet!=null)
        {
            float petY=statusY-31;
            RiftorynL10n.Label(new Rect(x,petY,width,18),$"PET · {RiftorynL10n.Translate(pet.petName)}   {Mathf.CeilToInt(Mathf.Max(0,pet.currentHp))} / {Mathf.CeilToInt(pet.maxHp)} HP",small);
            HealthBar(new Rect(x,petY+18,width,7),pet.currentHp,pet.maxHp,new Color(.68f,.48f,.92f));
        }
        HealthBar(new Rect(x,y,width,25),player.hp,MaxHp[player.hero],new Color(.68f,.12f,.13f));
        string hp=player.done?RiftorynL10n.Translate("META"):player.hp<=0?RiftorynL10n.Translate("POKONANY"):$"{Mathf.CeilToInt(player.hp)} / {Mathf.CeilToInt(MaxHp[player.hero])} HP";
        RiftorynL10n.Label(new Rect(x,y,width,25),hp,hudCenterStyle);
        RiftorynL10n.Label(new Rect(x,y+27,width,20),RiftorynL10n.Format("{0}  ·  DO METY {1} m",HeroNames[player.hero],player.done?0:Mathf.CeilToInt(Mathf.Max(0,total-Project(player.obj.transform.position,out _)))),small);
    }
    void OnGUI(){if(menuOpen)return;if(title==null){title=new GUIStyle(GUI.skin.label){fontSize=24,fontStyle=FontStyle.Bold,normal={textColor=new Color(1,.83f,.52f)}};heading=new GUIStyle(GUI.skin.label){fontSize=16,fontStyle=FontStyle.Bold,normal={textColor=Color.white}};small=new GUIStyle(GUI.skin.label){fontSize=13,wordWrap=true,normal={textColor=Color.white}};}float w=Screen.width;if(lanPanel){DrawLanPanel();return;}if(phase==Phase.Setup&&selectingHero>=0){DrawDraft();return;}if(phase==Phase.Setup&&briefing){DrawBriefing();return;}GUI.Box(new Rect(0,0,w,phase==Phase.Setup?155:110),"");if(GUI.Button(new Rect(w-90,6,75,28),RiftorynL10n.CurrentCode+" ⇄"))RiftorynL10n.Toggle();RiftorynL10n.Label(new Rect(18,8,450,34),"RIFTORYN  /  PRÓBA BOHATERA",title);
      if(phase==Phase.Setup){
        if(GUI.Button(new Rect(620,42,110,30),"LAN")){lanPanel=true;GUIUtility.ExitGUI();}
        RiftorynL10n.Label(new Rect(18,44,75,24),"Bohater",small);
        if(RiftorynL10n.Button(new Rect(90,42,200,30),HeroNames[selectedHero]+RiftorynL10n.Translate("  ·  ZMIEŃ"))){StartDraft(0,false);GUIUtility.ExitGUI();}
        RiftorynL10n.Label(new Rect(310,44,85,24),"Sojusznik",small);
        string allyLabel=allyChoice==0?"AUTO · "+HeroNames[AllyHero()]:allyChoice==1?"BRAK":HeroNames[AllyHero()];
        if(RiftorynL10n.Button(new Rect(395,42,210,30),RiftorynL10n.Translate(allyLabel)+RiftorynL10n.Translate("  ·  ZMIEŃ"))){StartDraft(1,false);GUIUtility.ExitGUI();}
        RiftorynL10n.Label(new Rect(18,78,60,23),"Wieże",small);
        bool newCustom=RiftorynL10n.SelectionGrid(new Rect(90,78,230,28),custom?1:0,new[]{"Standard","Własne"},2)==1;
        if(newCustom!=custom){custom=newCustom;Setup();GUIUtility.ExitGUI();}
        if(custom){selectedType=RiftorynL10n.SelectionGrid(new Rect(340,78,560,28),selectedType,TowerNames,7);RiftorynL10n.Label(new Rect(910,82,100,25),$"{towers.Count} / 5",small);}
        if(RiftorynL10n.Button(new Rect(w-210,42,190,63),"ROZPOCZNIJ PRÓBĘ")){briefing=true;GUIUtility.ExitGUI();}
        if(RiftorynL10n.Button(new Rect(w-210,110,190,30),"← MENU GŁÓWNE")){ReturnToMenu();GUIUtility.ExitGUI();}
        bool newPreview=GUI.Toggle(new Rect(18,112,235,28),genericModelPreview,"Model testowy (bez animacji)");
        if(newPreview!=genericModelPreview){genericModelPreview=newPreview;ReplaceRunners();GUIUtility.ExitGUI();}
        RiftorynL10n.Label(new Rect(260,115,Mathf.Max(0,w-475),30),custom?"Kliknij złote pole, by postawić lub usunąć wieżę.":"Standardowy układ: 5 wież.",small);
      }
      else
      {
        if(player==null||!player.obj||(ally!=null&&!ally.obj))return;
        DrawFogExposure();DrawCombatTexts();DrawHud();DrawCombatHeader();
        if(messageTimer>0)RiftorynL10n.Label(new Rect(20,145,600,30),message,heading);
        if(phase==Phase.Finished)
        {
            UpdateResult();
            GUI.Box(new Rect(w/2-270,Screen.height/2-85,540,170),"");
            RiftorynL10n.Label(new Rect(w/2-245,Screen.height/2-65,490,65),result,heading);
            if(!lanPlaying&&RiftorynL10n.Button(new Rect(w/2-225,Screen.height/2+20,210,45),"ZAGRAJ PONOWNIE")){Setup();StartDraft(0,true);GUIUtility.ExitGUI();}
            if(RiftorynL10n.Button(new Rect(w/2+15,Screen.height/2+20,210,45),"MENU GŁÓWNE")){ReturnToMenu();GUIUtility.ExitGUI();}
        }
      }
    }
    void DrawCombatHeader()
    {
        float w=Screen.width;
        RiftorynL10n.Label(new Rect(18,40,w-36,24),RiftorynL10n.Format("CEL: DOTRZYJ DO METY   •   CZAS {0:F1} s   •   MGŁA {1:F1} s",elapsed,player.exposure),heading);
        if(lanPlaying&&!lan.IsHost){RiftorynL10n.Label(new Rect(18,67,w-36,24),"LAN · "+HeroNames[player.hero].ToUpperInvariant()+"   •   Q: UMIEJĘTNOŚĆ   •   WASD: RUCH   •   MYSZ: KAMERA   •   ESC: KURSOR   •   UNIK NIEDOSTĘPNY",small);return;}
        RiftorynL10n.Label(new Rect(18,67,w-36,24),RiftorynL10n.Format("Q: {0}   •   SPACJA / UNIK: {1}   •   OSŁONA: {2:F0}   •   WASD: RUCH   •   MYSZ: KAMERA   •   ESC: KURSOR",player.cooldown>0?player.cooldown.ToString("F1")+" s":RiftorynL10n.Translate("GOTOWA"),rollCooldown>0?rollCooldown.ToString("F1")+" s":RiftorynL10n.Translate("GOTOWY"),player.shield),small);
    }
    string AbilityDescription()
    {
        if(selectedHero==4)return "NIVA · Q: usuwa spowolnienie u najbliższego spowolnionego sojusznika w zasięgu, w przeciwnym razie u siebie; 2 s odporności.";
        if(selectedHero==5)return "LIORA · Q: pole pod postacią leczy pobliskich sojuszników przez 5 s.";
        if(selectedHero>=6&&selectedHero!=LucenHero)return HeroNames[selectedHero].ToUpperInvariant()+" · umiejętność niedostępna w tym prototypie.";
        if(selectedHero==0)return "KAEL · Q: osłona dla Ciebie i pobliskiego sojusznika. E: testowy atak na pobliską wieżę.";
        if(selectedHero==1)return "RIAN · Q: zadaje obrażenia pobliskiej wieży. Podejdź w jej zasięg.";
        if(selectedHero==3)return RiftorynCodexData.Text("KONEVA · Q: Patka wznosi się, krąży i odciąga ogień wież. Model Konevy jest zastępczy.","KONEVA · Q: Patsy flies in circles and draws tower fire. Koneva uses a placeholder model.");
        if(selectedHero==LucenHero)return RiftorynCodexData.Text("LUCEN · Q: wyłącza najbliższą wieżę na 2,5 s. Otrzymuje o 25% mniej obrażeń Błyskawic. Model jest zastępczy.","LUCEN · Q: disables the nearest tower for 2.5 s. Takes 25% less Lightning damage. Uses a placeholder model.");
        return "ZET · Q: wyłącza pobliską wieżę i przyspiesza Cię na chwilę. Podejdź w jej zasięg.";
    }
    void DrawBriefing()
    {
        Matrix4x4 old=GUI.matrix;float scale=Mathf.Min(Screen.width/1200f,Screen.height/800f);
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1200*scale)*.5f,(Screen.height-800*scale)*.5f,0),Quaternion.identity,new Vector3(scale,scale,1));
        HudFill(new Rect(0,0,1200,800),new Color(.045f,.035f,.07f,1));
        RiftorynL10n.Label(new Rect(45,32,1100,42),"PRZED PRÓBĄ · CO TESTUJEMY",title);
        RiftorynL10n.Label(new Rect(48,85,1080,52),"Dostań się do złotego portalu, unikając ostrzału i uciekając przed mgłą. Sprawdź, czy potrafisz odczytać zagrożenia i reagować na czas.",heading);
        GUI.Box(new Rect(45,155,535,450),"");GUI.Box(new Rect(600,155,555,450),"");
        RiftorynL10n.Label(new Rect(65,175,490,30),"STEROWANIE I CEL",heading);
        RiftorynL10n.Label(new Rect(65,212,490,130),"WASD — ruch  ·  mysz — kamera\nSPACJA — unik; podczas przewrotu pociski nie ranią. Odnowienie 10 s.\nQ — umiejętność bohatera.\nESC — zwolnij lub przechwyć kursor.",small);
        RiftorynL10n.Label(new Rect(65,355,490,100),AbilityDescription(),heading);
        RiftorynL10n.Label(new Rect(65,475,490,110),"Na minimapie: TY — złoty, AI — błękitny, czerwony krzyż — miejsce śmierci. W panelu drużyny zobaczysz HP, statusy i dystans do mety.",small);
        RiftorynL10n.Label(new Rect(620,175,500,30),"ZAGROŻENIA",heading);
        RiftorynL10n.Label(new Rect(620,212,500,265),"Linia w kolorze wieży ostrzega o przygotowywanym strzale. Wieże celują z wyprzedzeniem; mury blokują pociski.\n\nOGIEŃ — obrażenia obszarowe i podpalenie.\nLÓD — wychłodzenie i wolniejszy ruch.\nBŁYSKAWICE — przeskok na pobliskiego sojusznika.\nNATURA — trucizna zadająca obrażenia w czasie.",small);
        RiftorynL10n.Label(new Rect(620,480,500,110),"MGŁA pojawia się po 5 s, spowalnia i z czasem zadaje coraz większe obrażenia. Statusy widać nad paskiem HP; kolorowe liczby obrażeń unoszą się nad postacią.",small);
        RiftorynL10n.Label(new Rect(50,625,1080,45),"To wersja testowa: zwróć uwagę, czy widzisz ostrzał, rozumiesz przyczynę obrażeń i masz czas zareagować.",small);
        if(GUI.Button(new Rect(1070,30,75,32),RiftorynL10n.CurrentCode+" ⇄"))RiftorynL10n.Toggle();
        if(RiftorynL10n.Button(new Rect(45,700,250,50),"← ZMIEŃ USTAWIENIE")){briefing=false;GUI.matrix=old;GUIUtility.ExitGUI();}
        if(RiftorynL10n.Button(new Rect(865,700,290,50),"ROZPOCZNIJ PRÓBĘ →")){briefing=false;Begin();GUI.matrix=old;GUIUtility.ExitGUI();}
        GUI.matrix=old;
    }

    int PlayableHero(string id)
    {
        switch(id){case "kael":return 0;case "rian":return 1;case "zet":return 2;case "koneva":return 3;case "niva":return 4;case "liora":return 5;case "elira":return 6;case "dara":return 7;case "nocten":return 8;case "lucen":return LucenHero;default:return -1;}
    }
    Texture2D DraftPortrait(string id)
    {
        if(string.IsNullOrEmpty(id))return null;
        if(!draftPortraits.TryGetValue(id,out var texture)){texture=Resources.Load<Texture2D>("Portraits/"+id);draftPortraits[id]=texture;}
        return texture;
    }
    static readonly string[] DraftRoles={"Tank","Wsparcie","Specjalista","Przywoływacz"};
    static readonly string[] DraftElements={"Ogień","Lód","Błyskawice","Natura","Ziemia","Światło","Ciemność"};
    static readonly string[] DraftElementIcons={"fire","ice","lightning","nature","earth","light","darkness"};

    void EnsureLan()
    {
        if(lan)return;lan=gameObject.AddComponent<RiftorynLan>();
        lan.PeerConnected+=()=>{if(!lan.IsHost)lan.Send("R"+lanHero,true);};
        lan.PeerDisconnected+=()=>{lanPlaying=false;Capture(false);ClearLanShots();Setup();lanPanel=true;};
        lan.Received+=ReceiveLan;
    }
    void StopLan(){lanPlaying=false;lanPanel=false;lanRemoteInput=Vector3.zero;lanRemoteAbility=false;lanRemoteAttack=false;lanNotice="";if(lan)lan.Stop();ClearLanShots();}
    void DrawLanPanel()
    {
        float x=Screen.width*.5f-300,y=Screen.height*.5f-210;
        GUI.Box(new Rect(x,y,600,420),"LAN · TEST 2 GRACZY");
        GUI.Label(new Rect(x+20,y+35,560,40),"Wybierz bohatera przed połączeniem. Bez duplikatów.\nUnik drugiego gracza nie jest jeszcze dostępny.");
        int[] picks={0,1,2,4,5,6,7,8,LucenHero};string[] names={"Kael","Rian","Zet","Niva","Liora","Elira*","Dara*","Nocten*","Lucen"};
        int index=Array.IndexOf(picks,lanHero);if(index<0)index=0;
        GUI.enabled=!lanPlaying&&(lan==null||!lan.Active||(!lan.IsHost&&lan.Connected));
        int chosen=GUI.SelectionGrid(new Rect(x+20,y+85,560,60),index,names,4);GUI.enabled=true;
        if(chosen!=index){lanHero=picks[chosen];lanNotice="";if(lan&&lan.Connected&&!lan.IsHost)lan.Send("R"+lanHero,true);}
        GUI.Label(new Rect(x+20,y+150,560,25),"* Brak aktywnej umiejętności w tym prototypie.");
        GUI.Label(new Rect(x+20,y+182,160,25),"Adres gospodarza:");lanAddress=GUI.TextField(new Rect(x+180,y+182,220,28),lanAddress,64);
        EnsureLan();
        if(!lan.Active){if(GUI.Button(new Rect(x+20,y+225,250,42),"UTWÓRZ GRĘ"))lan.StartHost(this);if(GUI.Button(new Rect(x+330,y+225,250,42),"DOŁĄCZ"))lan.StartClient(this,lanAddress);}
        else if(GUI.Button(new Rect(x+20,y+225,250,42),"PRZERWIJ ŁĄCZENIE")){lan.Stop();lanNotice="";}
        GUI.Label(new Rect(x+20,y+280,560,55),lan.Status+"\n"+lanNotice+"\nUDP 7777 · sieć lokalna");
        if(GUI.Button(new Rect(x+380,y+360,200,40),"POWRÓT")){StopLan();GUIUtility.ExitGUI();}
    }
    void PrepareLan(bool host,int hostHero,int guestHero)
    {
        custom=false;selectedHero=host?hostHero:guestHero;allyChoice=(host?guestHero:hostHero)+2;Setup();
        lanPlaying=true;lanPanel=false;lanRemoteInput=Vector3.zero;lanRemoteAbility=false;lanRemoteAttack=false;lanSendAt=0;lanInputAt=Time.unscaledTime;Begin();
    }
    void StepLanAlly(float dt)
    {
        if(ally.hp<=0||ally.done)return;
        if(lanRemoteAbility){Skill(ally);lanRemoteAbility=false;}if(lanRemoteAttack){if(ally.hero==0)Attack(ally);lanRemoteAttack=false;}
        Vector3 dir=Time.unscaledTime-lanInputAt<.5f?lanRemoteInput:Vector3.zero;
        float mult=ally.exposure>0?.9f:1f;if(ally.boost>0)mult*=1.2f;if(ally.chillTime>0)mult*=ally.hero==4?.88f:.85f;
        ally.obj.transform.position=MoveAgainstWalls(ally.obj.transform.position,dir*Speed[ally.hero]*mult*dt);
        if(dir.sqrMagnitude>.01f)ally.obj.transform.rotation=Quaternion.LookRotation(dir);
        if(Vector3.Distance(ally.obj.transform.position,At(total))<1.5f)ally.done=true;
    }
    LanRunner CaptureLanRunner(Runner r)=>new LanRunner{x=r.obj.transform.position.x,z=r.obj.transform.position.z,yaw=r.obj.transform.eulerAngles.y,hp=r.hp,shield=r.shield,shieldMax=r.shieldMax,cooldown=r.cooldown,exposure=r.exposure,damage=r.damage,done=r.done,uses=r.uses};
    void ApplyLanRunner(Runner r,LanRunner s){r.obj.transform.position=new Vector3(s.x,0,s.z);r.obj.transform.rotation=Quaternion.Euler(0,s.yaw,0);r.hp=s.hp;r.shield=s.shield;r.shieldMax=s.shieldMax;r.cooldown=s.cooldown;r.exposure=s.exposure;r.damage=s.damage;r.done=s.done;r.uses=s.uses;r.obj.SetActive(s.hp>0&&!s.done);}
    void SendLanState(bool force=false)
    {
        if(!lanPlaying||!lan.IsHost||!lan.Connected||(!force&&Time.unscaledTime<lanSendAt))return;lanSendAt=Time.unscaledTime+.05f;
        var state=new LanState{time=elapsed,healTime=healingTime,healX=healingPosition.x,healZ=healingPosition.z,finished=phase==Phase.Finished,hostHero=player.hero,guestHero=ally.hero,host=CaptureLanRunner(player),guest=CaptureLanRunner(ally),towerHp=new float[towers.Count],towerDisabled=new float[towers.Count],shots=new LanShot[shots.Count]};
        for(int i=0;i<towers.Count;i++){state.towerHp[i]=towers[i].hp;state.towerDisabled[i]=towers[i].disabled;}
        for(int i=0;i<shots.Count;i++){var p=shots[i].obj.transform.position;state.shots[i]=new LanShot{x=p.x,y=p.y,z=p.z,type=shots[i].type};}
        lan.Send("S"+JsonUtility.ToJson(state),state.finished||force);
    }
    void ReceiveLan(string message)
    {
        if(string.IsNullOrEmpty(message))return;
        if(lan.IsHost&&message[0]=='R'&&!lanPlaying){if(!int.TryParse(message.Substring(1),out int guest)||guest<0||guest>=HeroNames.Length||guest==3)return;if(guest==lanHero){lan.Send("D",true);lanNotice="Drugi gracz musi wybrać inną postać.";return;}lanNotice="";PrepareLan(true,lanHero,guest);SendLanState(true);}
        else if(!lan.IsHost&&message=="D"){lanNotice="Ta postać jest już wybrana przez gospodarza. Wybierz inną.";}
        else if(lan.IsHost&&message[0]=='I'&&lanPlaying){var input=JsonUtility.FromJson<LanInput>(message.Substring(1));if(input==null)return;lanRemoteInput=Vector3.ClampMagnitude(new Vector3(input.x,0,input.z),1);lanInputAt=Time.unscaledTime;lanRemoteAbility|=input.ability;lanRemoteAttack|=input.attack;}
        else if(!lan.IsHost&&message[0]=='S'){
            var state=JsonUtility.FromJson<LanState>(message.Substring(1));if(state==null||state.host==null||state.guest==null)return;
            if(!lanPlaying){if(state.hostHero<0||state.hostHero>=HeroNames.Length||state.guestHero<0||state.guestHero>=HeroNames.Length)return;PrepareLan(false,state.hostHero,state.guestHero);}elapsed=state.time;healingTime=state.healTime;healingPosition=new Vector3(state.healX,0,state.healZ);UpdateHealingRing();ApplyLanRunner(player,state.guest);ApplyLanRunner(ally,state.host);
            for(int i=0;i<towers.Count&&i<state.towerHp.Length;i++){if(towers[i].hp>0&&state.towerHp[i]<=0)HitTower(towers[i],towers[i].hp);towers[i].hp=state.towerHp[i];towers[i].disabled=state.towerDisabled[i];}
            UpdateLanShots(state.shots);if(state.finished&&phase!=Phase.Finished)End();
        }
    }
    void UpdateLanClient()
    {
        if(RiftorynInput.Escape)Capture(!mouseCaptured);if(mouseCaptured){cameraYaw+=RiftorynInput.Look.x*3;pitch=Mathf.Clamp(pitch-RiftorynInput.Look.y*2,0,60);}
        if(phase==Phase.Playing){bool ability=RiftorynInput.Ability,attack=RiftorynInput.Attack;if(Time.unscaledTime>=lanSendAt||ability||attack){var input=MoveInput();lan.Send("I"+JsonUtility.ToJson(new LanInput{x=input.x,z=input.z,ability=ability,attack=attack}),ability||attack);lanSendAt=Time.unscaledTime+.05f;}}
        float front=FogFront();for(int i=0;i<fogTiles.Count;i++)fogTiles[i].SetActive(elapsed>=5&&fogDistances[i]<front);UpdateRunnerVisual(player,Time.deltaTime);UpdateRunnerVisual(ally,Time.deltaTime);UpdateCamera();
    }
    void ClearLanShots(){foreach(var o in lanShotVisuals)if(o)Destroy(o);lanShotVisuals.Clear();}
    void UpdateLanShots(LanShot[] states){if(states==null)return;while(lanShotVisuals.Count>states.Length){int i=lanShotVisuals.Count-1;Destroy(lanShotVisuals[i]);lanShotVisuals.RemoveAt(i);}while(lanShotVisuals.Count<states.Length){var o=GameObject.CreatePrimitive(PrimitiveType.Sphere);o.name="Pocisk LAN";o.transform.localScale=Vector3.one*.38f;Destroy(o.GetComponent<Collider>());lanShotVisuals.Add(o);}for(int i=0;i<states.Length;i++){var s=states[i];lanShotVisuals[i].transform.position=new Vector3(s.x,s.y,s.z);lanShotVisuals[i].GetComponent<Renderer>().material.color=TowerColors[Mathf.Clamp(s.type,0,TowerColors.Length-1)];}}

    void StartDraft(int slot,bool first)
    {
        selectingHero=slot;initialDraft=first;draftPreview=-1;lastAbilityTooltip="";abilityTooltipScroll=Vector2.zero;
    }
    void FinishDraftChoice(int hero)
    {
        if(selectingHero==0){selectedHero=hero;if(allyChoice>=2&&AllyHero()==hero)allyChoice=0;}
        else allyChoice=hero+2;
        ReplaceRunners();
        if(initialDraft&&selectingHero==0){selectingHero=1;draftPreview=-1;}
        else {selectingHero=-1;initialDraft=false;}
    }
    void FinishDraftAlly(int choice)
    {
        allyChoice=choice;ReplaceRunners();selectingHero=-1;initialDraft=false;
    }
    void DraftFrame(Rect rect,Color border,Color fill)
    {
        HudFill(rect,border);HudFill(new Rect(rect.x+1,rect.y+1,rect.width-2,rect.height-2),fill);
    }
    void DraftCard(Rect card,int index)
    {
        CodexHero hero=roster[index];int playable=PlayableHero(hero.id);
        bool duplicate=selectingHero==1&&playable>=0&&playable==selectedHero;
        bool available=playable>=0&&!duplicate;
        Color outline=index==draftPreview?new Color(.90f,.73f,.43f):new Color(.31f,.26f,.35f);
        HudFill(card,outline);
        Rect inside=new(card.x+2,card.y+2,card.width-4,card.height-4);
        HudFill(inside,new Color(.15f,.10f,.19f));
        var portrait=DraftPortrait(hero.id);if(portrait)GUI.DrawTexture(inside,portrait,ScaleMode.ScaleAndCrop);
        if(!available)HudFill(inside,new Color(.06f,.055f,.075f,.74f));
        HudFill(new Rect(inside.x,inside.yMax-25,inside.width,25),new Color(.035f,.025f,.05f,.92f));
        if(draftCompactNameStyle==null)draftCompactNameStyle=new GUIStyle(small){fontSize=9,wordWrap=false};
        RiftorynL10n.Label(new Rect(inside.x+4,inside.yMax-24,inside.width-6,22),hero.name.ToUpperInvariant(),inside.width<60?draftCompactNameStyle:small);
        if(RiftorynL10n.Button(card,GUIContent.none,GUIStyle.none)){draftPreview=index;lastAbilityTooltip="";abilityTooltipScroll=Vector2.zero;}
    }
    void DrawDraft()
    {
        Matrix4x4 oldMatrix=GUI.matrix;
        float scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)*.5f,(Screen.height-900*scale)*.5f,0),Quaternion.identity,new Vector3(scale,scale,1));
        HudFill(new Rect(0,0,1600,900),new Color(.045f,.035f,.07f,1));
        Color panel=new(.095f,.08f,.13f), border=new(.29f,.25f,.34f), gold=new(.87f,.72f,.46f);
        RiftorynL10n.Label(new Rect(25,15,800,24),"✦  PRÓBA BOHATERA  /  WYBÓR DRUŻYNY",small);
        RiftorynL10n.Label(new Rect(22,43,900,40),"WYBIERZ BOHATERA",title);
        RiftorynL10n.Label(new Rect(24,86,900,25),"Wybierz portret, sprawdź bohatera i potwierdź wybór.",small);
        if(GUI.Button(new Rect(1330,35,70,38),RiftorynL10n.CurrentCode+" ⇄"))RiftorynL10n.Toggle();
        if(RiftorynL10n.Button(new Rect(1415,35,155,38),initialDraft?"← MENU":"← POWRÓT"))
        {
            if(initialDraft)ReturnToMenu();else selectingHero=-1;
            GUI.matrix=oldMatrix;GUIUtility.ExitGUI();
        }
        DraftFrame(new Rect(22,135,170,500),border,panel);
        RiftorynL10n.Label(new Rect(35,150,150,24),"TWOJA DRUŻYNA",small);
        DraftFrame(new Rect(35,185,144,74),selectingHero==0?gold:border,new Color(.06f,.055f,.09f));
        RiftorynL10n.Label(new Rect(45,195,125,55),initialDraft&&selectingHero==0?"TY\nWYBIERA TERAZ":RiftorynL10n.Translate("TY\n")+HeroNames[selectedHero],small);
        DraftFrame(new Rect(35,270,144,74),selectingHero==1?gold:border,new Color(.06f,.055f,.09f));
        string allyText=selectingHero==1?"WYBIERA TERAZ":allyChoice==1?"BRAK":HeroNames[AllyHero()];
        RiftorynL10n.Label(new Rect(45,280,125,55),RiftorynL10n.Translate("SOJUSZNIK AI\n")+RiftorynL10n.Translate(allyText),small);
        DraftFrame(new Rect(205,135,925,710),border,panel);
        RiftorynL10n.Label(new Rect(225,148,300,25),"BOHATEROWIE",heading);
        RiftorynL10n.Label(new Rect(965,150,155,22),"4 role × 7 żywiołów",small);
        for(int col=0;col<DraftElements.Length;col++)
        {
            float x=295+col*115;
            var icon=Resources.Load<Texture2D>("CodexIcons/elements/"+DraftElementIcons[col]);
            if(icon)GUI.DrawTexture(new Rect(x+38,182,28,28),icon,ScaleMode.ScaleToFit);
            RiftorynL10n.Label(new Rect(x,212,112,20),DraftElements[col],small);
        }
        for(int row=0;row<DraftRoles.Length;row++)
        {
            float y=238+row*126;
            var icon=Resources.Load<Texture2D>("CodexIcons/roles/"+RiftorynCodexData.RoleIcon(roleBonuses,DraftRoles[row]));
            if(icon)GUI.DrawTexture(new Rect(235,y+20,35,35),icon,ScaleMode.ScaleToFit);
            if(draftRoleStyle==null)draftRoleStyle=new GUIStyle(small){fontSize=11,wordWrap=false};
            RiftorynL10n.Label(new Rect(215,y+62,82,42),DraftRoles[row],draftRoleStyle);
            for(int col=0;col<DraftElements.Length;col++)
            {
                var matches=new List<int>();
                for(int i=0;i<roster.Length;i++)if(roster[i]!=null&&roster[i].role==DraftRoles[row]&&roster[i].element==DraftElements[col])matches.Add(i);
                float cardWidth=(106-(matches.Count-1)*3)/Mathf.Max(1,matches.Count);
                for(int j=0;j<matches.Count;j++)DraftCard(new Rect(295+col*115+j*(cardWidth+3),y,cardWidth,116),matches[j]);
            }
        }
        RiftorynL10n.Label(new Rect(225,748,875,24),"Przyciemnieni bohaterowie są niedostępni w tej próbie.",small);
        string action=selectingHero==0?"POTWIERDŹ SWOJEGO BOHATERA":"POTWIERDŹ SOJUSZNIKA";
        bool canChoose=draftPreview>=0&&draftPreview<roster.Length&&PlayableHero(roster[draftPreview].id)>=0&&!(selectingHero==1&&PlayableHero(roster[draftPreview].id)==selectedHero);
        bool previous=GUI.enabled;GUI.enabled=canChoose;
        if(RiftorynL10n.Button(new Rect(465,786,390,42),action)){int pick=PlayableHero(roster[draftPreview].id);FinishDraftChoice(pick);GUI.matrix=oldMatrix;GUIUtility.ExitGUI();}
        GUI.enabled=previous;
        if(selectingHero==1)
        {
            if(RiftorynL10n.Button(new Rect(865,776,240,27),"WYBIERZ AUTOMATYCZNIE")){FinishDraftAlly(0);GUI.matrix=oldMatrix;GUIUtility.ExitGUI();}
            if(RiftorynL10n.Button(new Rect(865,808,240,27),"GRAJ BEZ SOJUSZNIKA")){FinishDraftAlly(1);GUI.matrix=oldMatrix;GUIUtility.ExitGUI();}
        }
        DraftFrame(new Rect(1145,135,425,710),border,panel);
        if(draftPreview>=0&&draftPreview<roster.Length)DrawDraftDetails(roster[draftPreview]);
        else RiftorynL10n.Label(new Rect(1165,165,375,80),"Wybierz portret, aby zobaczyć informacje o bohaterze.",small);
        GUI.matrix=oldMatrix;
    }
    RiftorynHeroStats FindHeroStats(string id)
    {
        foreach(var stat in heroStats)if(stat!=null&&stat.id==id)return stat;
        return null;
    }
    void DraftStat(float x,float y,float width,string label,string value)
    {
        DraftFrame(new Rect(x,y,width,52),new Color(.28f,.23f,.32f),new Color(.075f,.062f,.105f));
        string iconName=label=="HP"?"health":label=="ATAK"?"attack":label=="RUCH m/s"?"speed":null;
        if(iconName!=null)
        {
            if(!statIcons.TryGetValue(iconName,out var icon)){icon=Resources.Load<Texture2D>("StatIcons/"+iconName);statIcons[iconName]=icon;}
            if(icon)GUI.DrawTexture(new Rect(x+6,y+6,38,38),icon,ScaleMode.ScaleToFit);
            else RiftorynL10n.Label(new Rect(x+7,y+4,width-14,18),label,small);
            RiftorynL10n.Label(new Rect(x+48,y+15,width-51,24),value,heading);
        }
        else
        {
            RiftorynL10n.Label(new Rect(x+7,y+4,width-14,18),label,small);
            RiftorynL10n.Label(new Rect(x+7,y+23,width-14,24),value,heading);
        }
    }
    string DraftAbility(Rect row,string key,string abilityName,string description)
    {
        if(string.IsNullOrEmpty(abilityName)||string.IsNullOrEmpty(description))return null;
        bool hovered=row.Contains(Event.current.mousePosition);
        DraftFrame(row,hovered?new Color(.87f,.72f,.46f):new Color(.28f,.23f,.32f),new Color(.085f,.07f,.12f));
        RiftorynL10n.Label(new Rect(row.x+8,row.y+5,75,row.height-8),key,small);
        string name=RiftorynL10n.Translate(abilityName);
        int separator=name.IndexOf(" · ",StringComparison.Ordinal);
        if(separator>=0)name=name.Substring(separator+3);
        GUI.Label(new Rect(row.x+84,row.y+4,row.width-94,row.height-8),name,small);
        return hovered?description:null;
    }
    void DrawDraftDetails(CodexHero hero)
    {
        int playable=PlayableHero(hero.id);
        var stats=FindHeroStats(hero.id);
        Rect portraitRect=new(1160,150,180,225);
        HudFill(portraitRect,new Color(.06f,.045f,.085f));
        var portrait=DraftPortrait(hero.id);
        if(portrait)GUI.DrawTexture(portraitRect,portrait,ScaleMode.ScaleAndCrop);
        RiftorynL10n.Label(new Rect(1352,157,202,46),RiftorynL10n.Translate(hero.role)+"  /  "+RiftorynL10n.Translate(hero.element),small);
        if(draftHeroTitle==null)draftHeroTitle=new GUIStyle(title){fontSize=22,wordWrap=true};
        GUI.Label(new Rect(1352,205,202,58),hero.name.ToUpperInvariant(),draftHeroTitle);
        string availability=playable<0?"W PRZYGOTOWANIU":selectingHero==1&&playable==selectedHero?"WYBRANY PRZEZ GRACZA":"DOSTĘPNY W PRÓBIE";
        RiftorynL10n.Label(new Rect(1352,285,200,52),availability,small);
        string summary=!string.IsNullOrEmpty(hero.concept)&&hero.concept==hero.desc?(!string.IsNullOrEmpty(hero.pet)?"Najedź na towarzysza poniżej, aby zobaczyć opis.":"Opis koncepcji zobaczysz po najechaniu poniżej."):hero.desc??"";
        RiftorynL10n.Label(new Rect(1160,384,395,54),summary,small);
        RiftorynL10n.Label(new Rect(1160,439,395,20),playable>=0?"STATYSTYKI PRÓBY":"STATYSTYKI PROJEKTOWE",small);
        string hp=stats!=null?stats.hp.ToString():"—", attack=stats!=null?stats.attack.ToString():"—", speed=stats!=null?RiftorynL10n.Format("{0:0.0}",stats.speed):"—";
        string cooldown=hero.cd>0?hero.cd+" s":"—";
        if(playable>=0)
        {
            hp=MaxHp[playable].ToString("0");speed=RiftorynL10n.Format("{0:0.0}",Speed[playable]);cooldown=Cooldowns[playable]+" s";
            attack=playable==0?"25*":"—";
        }
        DraftStat(1160,462,94,"HP",hp);
        DraftStat(1259,462,94,"ATAK",attack);
        DraftStat(1358,462,94,"RUCH m/s",speed);
        DraftStat(1457,462,98,"Q",cooldown);
        if(stats!=null&&stats.petHp>0)
        {
            string petName=RiftorynL10n.Translate(hero.pet??"");
            RiftorynL10n.Label(new Rect(1160,519,395,20),RiftorynL10n.Translate("TOWARZYSZ")+": "+petName,small);
            DraftStat(1160,542,126,"HP",stats.petHp.ToString());
            DraftStat(1292,542,126,"ATAK",stats.petAttack.ToString());
            DraftStat(1424,542,131,"RUCH m/s",RiftorynL10n.Format("{0:0.0}",stats.petSpeed));
        }
        else if(playable==0)RiftorynL10n.Label(new Rect(1160,519,395,20),"* Testowy atak E przeciw wieży.",small);
        else if(playable>0)RiftorynL10n.Label(new Rect(1160,519,395,20),"Atak podstawowy niedostępny w próbie.",small);
        float abilityShift=stats!=null&&stats.petHp>0?80f:0f;
        RiftorynL10n.Label(new Rect(1160,545+abilityShift,395,22),"UMIEJĘTNOŚCI",heading);
        string hovered=null;
        string passive=RiftorynCodexData.Text(hero.pass,hero.passEn), active=RiftorynCodexData.Text(hero.attack,hero.attackEn), defense=RiftorynCodexData.Text(hero.def,hero.defEn);
        string passiveKey="P", defenseKey="OBRONA*";
        if(playable==0){passiveKey="P*";passive=RiftorynL10n.Translate("Planowana pasywka; nie działa jeszcze w próbie. ")+RiftorynL10n.Translate(hero.pass);active="Natychmiast dodaje 100 osłony Tobie oraz 50 pobliskiemu sojusznikowi; osłona traci 10% aktualnej wartości na sekundę.";}
        else if(playable==1){passive="Otrzymujesz o 15% mniej bezpośrednich obrażeń Ognia.";active="Zadaje 100 obrażeń najbliższej wieży w zasięgu.";}
        else if(playable==2){active="Wyłącza najbliższą wieżę w zasięgu na 2 s i zwiększa Twoją prędkość o 20% na 2 s.";}
        if(!string.IsNullOrEmpty(defense))defense=RiftorynL10n.Translate("Umiejętność projektowa, niedostępna w próbie. ")+RiftorynL10n.Translate(defense);
        int abilityRow=0;
        var roleBonus=RiftorynCodexData.RoleBonus(roleBonuses,hero.role);
        if(roleBonus!=null){float rowY=570+abilityShift+abilityRow++*28;var roleIcon=Resources.Load<Texture2D>("CodexIcons/roles/"+roleBonus.icon);if(roleIcon)GUI.DrawTexture(new Rect(1163,rowY+2,20,20),roleIcon,ScaleMode.ScaleToFit);string roleName=RiftorynCodexData.Text(roleBonus.name,roleBonus.nameEn);string roleDescription=RiftorynCodexData.Text(roleBonus.description,roleBonus.descriptionEn);hovered=DraftAbility(new Rect(1188,rowY,367,25),RiftorynCodexData.Text("KLASA","CLASS"),roleName,roleDescription)??hovered;}
        string passiveName=RiftorynCodexData.Text(hero.passName,hero.passNameEn);
        if(!string.IsNullOrEmpty(passiveName)&&!string.IsNullOrEmpty(passive))
            hovered=DraftAbility(new Rect(1160,570+abilityShift+abilityRow++*28,395,25),passiveKey,passiveName,passive)??hovered;
        if(!string.IsNullOrEmpty(hero.attackName)&&!string.IsNullOrEmpty(active))
            hovered=DraftAbility(new Rect(1160,570+abilityShift+abilityRow++*28,395,25),"Q",RiftorynCodexData.Text(hero.attackName,hero.attackNameEn),active)??hovered;
        if(!string.IsNullOrEmpty(hero.defName)&&!string.IsNullOrEmpty(defense))
            hovered=DraftAbility(new Rect(1160,570+abilityShift+abilityRow++*28,395,25),defenseKey,RiftorynCodexData.Text(hero.defName,hero.defNameEn),defense)??hovered;
        if(!string.IsNullOrEmpty(hero.pet))
        {
            string petDescription=!string.IsNullOrEmpty(hero.concept)?hero.concept:hero.desc;
            if(!string.IsNullOrEmpty(petDescription))
            {
                petDescription=RiftorynL10n.Translate("Opis towarzysza — koncepcja niedostępna w próbie. ")+RiftorynL10n.Translate(petDescription);
                hovered=DraftAbility(new Rect(1160,570+abilityShift+abilityRow++*28,395,25),"PET",hero.pet,petDescription)??hovered;
            }
        }
        if(abilityRow==0)
        {
            if(!string.IsNullOrEmpty(hero.concept))hovered=DraftAbility(new Rect(1160,570+abilityShift,395,25),"INFO","KONCEPCJA",hero.concept)??hovered;
            else RiftorynL10n.Label(new Rect(1160,574+abilityShift,395,24),"Brak opisanych umiejętności.",small);
        }
        float tooltipY=686+abilityShift;
        bool overTooltip=new Rect(1160,tooltipY,395,Mathf.Min(143,845-tooltipY)).Contains(Event.current.mousePosition);
        if(hovered!=null)abilityTooltipUntil=Time.unscaledTime+.3f;
        else if(!string.IsNullOrEmpty(lastAbilityTooltip)&&(overTooltip||Time.unscaledTime<abilityTooltipUntil))hovered=lastAbilityTooltip;
        if(hovered!=lastAbilityTooltip){abilityTooltipScroll=Vector2.zero;lastAbilityTooltip=hovered;}
        Rect tooltipRect=new(1160,tooltipY,395,Mathf.Min(143,845-tooltipY));
        DraftFrame(tooltipRect,new Color(.28f,.23f,.32f),new Color(.065f,.055f,.09f));
        RiftorynL10n.Label(new Rect(1170,tooltipY+7,375,20),string.IsNullOrEmpty(hovered)?RiftorynCodexData.Text("ODPORNOŚCI NA ŻYWIOŁY","ELEMENTAL RESISTANCES"):"OPIS",small);
        if(string.IsNullOrEmpty(hovered))DrawDraftResistanceTable(new Rect(1170,tooltipY+27,375,tooltipRect.height-32),hero);
        else
        {
            string text=RiftorynL10n.Translate(hovered);
            Rect view=new(1170,tooltipY+29,375,Mathf.Max(25,tooltipRect.height-34));
            float textHeight=Mathf.Max(view.height,small.CalcHeight(new GUIContent(text),view.width-20));
            abilityTooltipScroll=GUI.BeginScrollView(view,abilityTooltipScroll,new Rect(0,0,view.width-20,textHeight));
            GUI.Label(new Rect(0,0,view.width-20,textHeight),text,small);
            GUI.EndScrollView();
        }
    }
    void DrawDraftResistanceTable(Rect area,CodexHero hero)
    {
        if(draftResistanceNameStyle==null){draftResistanceNameStyle=new GUIStyle(small){fontSize=10,fontStyle=FontStyle.Bold,wordWrap=false};draftResistanceValueStyle=new GUIStyle(small){fontSize=11,fontStyle=FontStyle.Bold,wordWrap=false};}
        const float gap=3;float cellWidth=(area.width-gap*3)/4,cellHeight=(area.height-gap)/2;
        for(int i=0;i<RiftorynCodexData.Elements.Length;i++)
        {
            int row=i/4,col=i%4,value=RiftorynCodexData.Resistance(hero,RiftorynCodexData.Elements[i]);
            float x=area.x+col*(cellWidth+gap),y=area.y+row*(cellHeight+gap);
            DraftFrame(new Rect(x,y,cellWidth,cellHeight),new Color(.28f,.23f,.32f),new Color(.085f,.07f,.12f));
            var icon=Resources.Load<Texture2D>("CodexIcons/elements/"+DraftElementIcons[i]);float iconSize=Mathf.Min(16,cellHeight-5);
            if(icon)GUI.DrawTexture(new Rect(x+3,y+3,iconSize,iconSize),icon,ScaleMode.ScaleToFit);
            RiftorynL10n.Label(new Rect(x+iconSize+5,y+2,cellWidth-iconSize-8,Mathf.Max(12,cellHeight*.48f)),RiftorynL10n.Translate(RiftorynCodexData.Elements[i]),draftResistanceNameStyle);
            Color previous=GUI.color;GUI.color=value>0?new Color(.45f,.88f,.62f):value<0?new Color(1f,.46f,.43f):Color.white;
            RiftorynL10n.Label(new Rect(x+iconSize+5,y+cellHeight*.48f,cellWidth-iconSize-8,Mathf.Max(12,cellHeight*.48f)),(value>0?"+":"")+value+"%",draftResistanceValueStyle);GUI.color=previous;
        }
    }

}
