using System;
using System.Collections.Generic;
using UnityEngine;

// Isolated inspection scene: never modifies the trial or its rules.
public sealed class RiftorynSparkPreview : MonoBehaviour
{
    Animation animationPlayer;
    GameObject fox;
    Material outline;
    readonly Dictionary<Renderer,Material> baseMaterials=new();
    string idleClip,runClip,hitClip,current="Idle",resume="Idle";
    float scale=1f;
    bool outlined;
    void Start()
    {
        var ground=GameObject.CreatePrimitive(PrimitiveType.Plane);ground.name="Podłoże podglądu";
        ground.transform.localScale=new Vector3(2,1,2);ground.GetComponent<Renderer>().material.color=new Color(.20f,.18f,.23f);
        var player=GameObject.CreatePrimitive(PrimitiveType.Capsule);player.name="Skala gracza — kapsuła 2 m";
        player.transform.position=new Vector3(0,1,0);player.transform.localScale=new Vector3(.85f,1,.85f);
        player.GetComponent<Renderer>().material.color=new Color(.82f,.65f,.35f);
        var prefab=Resources.Load<GameObject>("Pets/spark-preview");
        if(prefab)
        {
            fox=Instantiate(prefab,new Vector3(1.8f,0,1.5f),Quaternion.identity);fox.name="Iskra Spark — podgląd";
            ApplyPreviewColors();
            var shader=Resources.Load<Shader>("SparkPreviewOutline");if(shader)outline=new Material(shader);
            animationPlayer=fox.GetComponent<Animation>();if(!animationPlayer)animationPlayer=fox.AddComponent<Animation>();
            foreach(var clip in Resources.LoadAll<AnimationClip>("Pets/spark-preview"))
            {
                if(!clip.legacy||clip.name.StartsWith("__preview__",StringComparison.Ordinal))continue;
                if(clip.name.IndexOf("Idle",StringComparison.OrdinalIgnoreCase)>=0)idleClip=clip.name;
                if(clip.name.IndexOf("Run",StringComparison.OrdinalIgnoreCase)>=0)runClip=clip.name;
                if(clip.name.IndexOf("Hit",StringComparison.OrdinalIgnoreCase)>=0)hitClip=clip.name;
                animationPlayer.AddClip(clip,clip.name);
            }
            SetVariant(1,false);Play("Idle");
        }
        var lightObject=new GameObject("Światło podglądu");var sun=lightObject.AddComponent<Light>();sun.type=LightType.Directional;sun.intensity=1.1f;sun.color=new Color(1,.89f,.76f);
        lightObject.transform.rotation=Quaternion.Euler(48,-35,0);RenderSettings.ambientLight=new Color(.39f,.36f,.49f);
        var cameraObject=new GameObject("Kamera gracza");var camera=cameraObject.AddComponent<Camera>();camera.tag="MainCamera";
        var focus=new Vector3(0,1.5f,0);var rotation=Quaternion.Euler(18,0,0);
        cameraObject.transform.SetPositionAndRotation(focus-rotation*Vector3.forward*7.5f,rotation);
        camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.085f,.065f,.15f);
    }
    void ApplyPreviewColors()
    {
        var materials=new Dictionary<string,Material>();
        foreach(var renderer in fox.GetComponentsInChildren<Renderer>())
        {
            var source=renderer.sharedMaterial;if(!source)continue;string name=source.name;
            if(!materials.TryGetValue(name,out var material))
            {
                Color color=name.Contains("jasne")?new Color(.91f,.72f,.51f):name.Contains("rudy")?new Color(.63f,.17f,.055f):name.Contains("żar")?new Color(1f,.39f,.045f):new Color(.18f,.065f,.045f);
                material=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard"));material.color=color;materials.Add(name,material);
            }
            renderer.sharedMaterial=material;baseMaterials.Add(renderer,material);
        }
    }
    public void SetVariant(float size,bool useOutline)
    {
        if(!fox)return;scale=size;outlined=useOutline&&outline;fox.transform.localScale=Vector3.one*size;
        foreach(var pair in baseMaterials)pair.Key.sharedMaterials=outlined?new[]{pair.Value,outline}:new[]{pair.Value};
    }
    public void Play(string label)
    {
        string clip=label=="Run"?runClip:label=="Hit"?hitClip:idleClip;
        if(!animationPlayer||string.IsNullOrEmpty(clip))return;
        if(label=="Hit"&&current!="Hit")resume=current;
        animationPlayer[clip].wrapMode=label=="Hit"?WrapMode.Once:WrapMode.Loop;
        animationPlayer.Play(clip);current=label;
    }
    void Update(){if(current=="Hit"&&animationPlayer&&!animationPlayer.isPlaying)Play(resume);}
    void OnGUI()
    {
        GUI.Box(new Rect(12,12,510,145),"ISKRA / SPARK — PODGLĄD");
        GUI.Label(new Rect(25,36,470,20),"Kamera próby · skala: "+scale.ToString("0.00")+" · obrys: "+(outlined?"tak":"nie"));
        if(GUI.Button(new Rect(25,60,145,27),"NATURALNA"))SetVariant(1,false);
        if(GUI.Button(new Rect(180,60,145,27),"120%"))SetVariant(1.2f,false);
        if(GUI.Button(new Rect(335,60,165,27),"120% + OBRYS"))SetVariant(1.2f,true);
        if(GUI.Button(new Rect(25,98,145,27),"BEZCZYNNOŚĆ"))Play("Idle");
        if(GUI.Button(new Rect(180,98,145,27),"BIEG"))Play("Run");
        if(GUI.Button(new Rect(335,98,165,27),"TRAFIENIE"))Play("Hit");
        GUI.Label(new Rect(25,128,470,20),animationPlayer&&idleClip!=null&&runClip!=null&&hitClip!=null?"Animacja: "+current:"Brak modelu lub klipu Idle / Run / Hit — sprawdź import.");
    }
    void OnDestroy(){if(outline)Destroy(outline);var unique=new HashSet<Material>(baseMaterials.Values);foreach(var material in unique)if(material)Destroy(material);}
}
