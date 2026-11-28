using UnityEngine;

public partial class RiftorynTrial
{
    // Vareth's palette: basalt, forged bronze and ember; geometry is decorative only.
    static readonly Color VarethIron=new(.12f,.105f,.12f), VarethBronze=new(.62f,.38f,.19f),
        VarethGold=new(.86f,.61f,.29f), VarethEmber=new(1f,.30f,.07f), VarethGlow=new(1f,.69f,.20f),
        VarethCloth=new(.34f,.085f,.075f);

    void BuildVarethArt()
    {
        var root=new GameObject("Vareth | Wystrój kuźni");root.transform.SetParent(world.transform,false);
        // Wall ornaments are inset into the existing walls and have no colliders.
        for(int i=0;i<Map.Length-1;i++)
        {
            Vector3 a=P(Map[i]),b=P(Map[i+1]);Vector3 forward=(b-a).normalized;
            Vector3 side=new(-forward.z,0,forward.x);
            float length=Vector3.Distance(a,b);
            Vector3 center=Vector3.Lerp(a,b,.5f);
            for(int sign=-1;sign<=1;sign+=2)
            {
                Vector3 edge=center+side*(sign*2.45f)+Vector3.up*.105f;
                var inlay=Cube("Mosiężna krawędź szlaku",edge,new Vector3(.06f,.012f,length-.8f),VarethBronze,root.transform,false);
                inlay.transform.rotation=Quaternion.LookRotation(forward);
            }
            if(i%2!=0)continue;
            for(int face=-1;face<=1;face+=2)
            {
                if(!TryVarethWallFace(center,side,face,out Vector3 anchor))continue;
                Quaternion facing=Quaternion.LookRotation(-face*side);
                ForgePlaque(root.transform,anchor,facing,i%6==0);
                if(TryVarethWallFace(Vector3.Lerp(a,b,.18f),side,face,out Vector3 trimAnchor))
                    ForgeWallTrim(root.transform,trimAnchor,facing);
                if(i%6==0)ForgeBanner(root.transform,anchor,facing);
            }
        }
    }

    bool TryVarethWallFace(Vector3 center,Vector3 side,int sign,out Vector3 face)
    {
        const float grid=1.5f;
        center=new Vector3(Mathf.Round(center.x/grid)*grid,0,Mathf.Round(center.z/grid)*grid);
        for(int i=1;i<=6;i++)
        {
            Vector3 pos=center+side*(sign*i*grid);
            Project(pos,out float distance);
            if(distance>RoadHalf+1.1f&&distance<RoadHalf+5f)
            {
                face=pos-side*(sign*(grid*.5f+.02f));
                return true;
            }
        }
        face=Vector3.zero;return false;
    }

    void ForgeWallTrim(Transform root,Vector3 anchor,Quaternion facing)
    {
        var pillar=Cube("Kute żebro muru",anchor+Vector3.up*1.38f,new Vector3(.22f,2.6f,.12f),VarethIron,root,false);
        pillar.transform.rotation=facing;
        foreach(float y in new[]{.34f,2.48f})
        {
            var collar=Cube("Obręcz żebra",anchor+Vector3.up*y+facing*Vector3.forward*.08f,new Vector3(.31f,.10f,.10f),VarethGold,root,false);
            collar.transform.rotation=facing;
        }
    }

    void ForgeBanner(Transform root,Vector3 anchor,Quaternion facing)
    {
        var mount=Cube("Wspornik proporca",anchor+Vector3.up*3.12f,new Vector3(1.25f,.10f,.16f),VarethGold,root,false);
        mount.transform.rotation=facing;
        var banner=Cube("Proporzec Vareth",anchor+Vector3.up*2.76f+facing*Vector3.forward*.06f,new Vector3(.68f,.68f,.04f),VarethCloth,root,false);
        banner.transform.rotation=facing;
        var diamond=Cube("Znak na proporcu",anchor+Vector3.up*2.78f+facing*Vector3.forward*.10f,new Vector3(.20f,.20f,.025f),VarethGold,root,false);
        diamond.transform.rotation=facing*Quaternion.Euler(0,0,45);
    }

    void ForgePlaque(Transform root,Vector3 anchor,Quaternion facing,bool beacon)
    {
        // Local Z points towards the path; all objects avoid the physical route.
        GameObject Part(string name,Vector3 offset,Vector3 scale,Color color)
        {
            var part=Cube(name,anchor+facing*offset,scale,color,root,false);
            part.transform.rotation=facing;return part;
        }
        Part("Żelazna płyta kuźni",new Vector3(0,1.82f,0),new Vector3(.94f,1.28f,.10f),VarethIron);
        Part("Mosiężna rama",new Vector3(0,1.82f,.07f),new Vector3(.78f,1.10f,.08f),VarethBronze);
        Part("Wnętrze znaku",new Vector3(0,1.82f,.12f),new Vector3(.62f,.93f,.05f),VarethIron);
        var rune=Part("Runiczny znak żaru",new Vector3(0,1.88f,.17f),new Vector3(.34f,.34f,.06f),VarethEmber);
        rune.transform.rotation=facing*Quaternion.Euler(0,0,45);
        Part("Nacięcie runy",new Vector3(0,1.88f,.22f),new Vector3(.055f,.38f,.025f),VarethGlow);
        if(!beacon)return;
        Part("Półka paleniska",new Vector3(0,2.52f,.17f),new Vector3(.72f,.12f,.35f),VarethBronze);
        var flame=Part("Płomień kuźni",new Vector3(0,2.76f,.18f),new Vector3(.22f,.35f,.22f),VarethGlow);
        flame.transform.rotation=facing*Quaternion.Euler(0,0,45);
        var lamp=new GameObject("Blask paleniska");lamp.transform.SetParent(root,false);
        lamp.transform.position=anchor+facing*new Vector3(0,2.6f,.35f);
        var light=lamp.AddComponent<Light>();light.type=LightType.Point;light.color=new Color(1f,.45f,.16f);
        light.range=5f;light.intensity=.8f;light.shadows=LightShadows.None;
    }
}
