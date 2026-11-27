using System.Collections.Generic;
using UnityEngine;

public partial class RiftorynTrial
{
    static readonly string[] ArenaNames={"Vareth · Ogień","Nivoren · Lód","Zerath · Błyskawice","Verdan · Natura","Kharum · Ziemia","Aurel · Światło","Vesper · Ciemność"};
    static readonly string[] ArenaDescriptions={"Dziedziniec kuźni i rozżarzony kamień","Chłodny szlak przełęczy i lodowe kryształy","Kamienne przekaźniki i złote przewodniki","Zielone ogrody i kamienie szlaku","Kamieniarski labirynt i miedziane znaki","Jasne mury i znaki obserwatorów","Ciemny kamień i fioletowe kryształy"};
    // Each pair is a grid point. All seven paths have 32 segments, 360 m total.
    static readonly string[] ArenaPaths={
        "334353524232313040415161626364656656464535362616060504142423131202",
        "333435454656666555546463626151413121111222232425151606050414130302",
        "333424141505040313232232424353544445555666656463625251616050403020",
        "333424141312222131414252626151504030201000010203040515162625353646",
        "334344545352515040302010000111213141423222121323241404051525353626",
        "332313121121223231302010000102030414242535344443536364656656463626",
        "333231414030202111100001020304051514243444546465555646363525261606"
    };
    static readonly Color[] ArenaWalls={new(.30f,.16f,.14f),new(.22f,.30f,.37f),new(.24f,.25f,.35f),new(.20f,.30f,.22f),new(.32f,.26f,.20f),new(.36f,.34f,.30f),new(.20f,.17f,.29f)};
    static readonly Color[] ArenaFloors={new(.30f,.20f,.18f),new(.24f,.31f,.36f),new(.24f,.25f,.31f),new(.24f,.30f,.23f),new(.31f,.27f,.23f),new(.38f,.36f,.31f),new(.22f,.20f,.29f)};
    static readonly Color[] ArenaAccents={new(.85f,.34f,.15f),new(.52f,.82f,1f),new(.90f,.75f,.24f),new(.43f,.72f,.34f),new(.68f,.46f,.27f),new(.96f,.83f,.49f),new(.55f,.32f,.75f)};

    Color ArenaFloorColor()=>activeArena<0?new Color(.26f,.24f,.30f):ArenaFloors[activeArena];
    Color ArenaWallColor(int variant)
    {
        if(activeArena<0) return variant==0?new Color(.25f,.23f,.30f):variant==1?new Color(.29f,.27f,.34f):new Color(.32f,.29f,.36f);
        return ArenaWalls[activeArena]*new Color(variant==0?.86f:variant==1?1f:1.12f,variant==0?.86f:variant==1?1f:1.12f,variant==0?.86f:variant==1?1f:1.12f,1);
    }
    Color ArenaTrimColor()=>activeArena<0?new Color(.37f,.33f,.41f):Color.Lerp(ArenaWalls[activeArena],ArenaAccents[activeArena],.38f);
    void ArenaDecoration(Vector3 pos)
    {
        var marker=Cube("Znak krainy",pos+Vector3.up*3.03f,new Vector3(.30f,.42f,.30f),ArenaAccents[activeArena],world.transform,false);
        marker.transform.rotation=Quaternion.Euler(0,45,activeArena==1||activeArena==6?45:0);
    }

    void MeasureArena()
    {
        total=0;
        lengths=new float[Map.Length-1];
        for(int i=0;i<lengths.Length;i++)
        {
            lengths[i]=Vector2.Distance(Map[i],Map[i+1])*Scale;
            total+=lengths[i];
        }
        fogArrivalTime=Mathf.Max(MinFogArrivalTime,total/Mathf.Min(Speed)+CleanRunMargin);
    }

    void ConfigureArena(int index)
    {
        activeArena=index;
        if(index<0){Map=DefaultMap;SlotMap=DefaultSlotMap;MeasureArena();return;}
        string encoded=ArenaPaths[index];
        Map=new Vector2[encoded.Length/2];
        for(int i=0;i<Map.Length;i++) Map[i]=new Vector2(175+(encoded[2*i]-'0')*75,105+(encoded[2*i+1]-'0')*75);
        MeasureArena();
        var points=new List<Vector2>();
        for(int i=0;i<Map.Length-1;i++)
        {
            Vector2 dir=(Map[i+1]-Map[i]).normalized;
            Vector2 side=new Vector2(-dir.y,dir.x);
            for(int sign=-1;sign<=1;sign+=2)
            {
                Vector2 pos=(Map[i]+Map[i+1])*.5f+side*(sign*35f);
                Project(P(pos),out float distance);
                if(distance<3.9f||distance>7.5f) continue;
                bool tooClose=false;
                foreach(var other in points) if(Vector2.Distance(pos,other)<25f){tooClose=true;break;}
                if(!tooClose) points.Add(pos);
            }
        }
        SlotMap=points.ToArray();
    }

    void RebuildArena(int index)
    {
        ClearHeatBarrier();ClearShotVisuals();ClearEarthStrikes();ClearVinePatches();
        foreach(var tower in towers)
        {
            if(!tower.obj) continue;
            tower.obj.SetActive(false);
            if(tower.range&&tower.range.sharedMaterial) Destroy(tower.range.sharedMaterial);
            if(tower.warning&&tower.warning.sharedMaterial) Destroy(tower.warning.sharedMaterial);
            Destroy(tower.obj);
        }
        towers.Clear();
        foreach(var obj in new[]{world,runnerRoot,slotRoot,fogRoot}) if(obj){obj.SetActive(false);Destroy(obj);}
        if(fogMesh) Destroy(fogMesh);
        if(fogMaterial) Destroy(fogMaterial);
        if(wallTexture) Destroy(wallTexture);
        if(floorTexture) Destroy(floorTexture);
        if(floorMaterial) Destroy(floorMaterial);
        world=null;runnerRoot=null;slotRoot=null;fogRoot=null;cam=null;player=null;ally=null;healingRing=null;
        matchExtras.Clear();fogTiles.Clear();fogDistances.Clear();visibleFogTiles=0;
        ConfigureArena(index);
        BuildWorld();
    }

    void DrawArenaThumbnail(int index,Rect rect)
    {
        HudFill(rect,new Color(.035f,.03f,.05f,.9f));
        string path=ArenaPaths[index];
        Vector2 Point(int i)=>new Vector2(rect.x+8+(path[i*2]-'0')*(rect.width-16)/6f,rect.y+8+(path[i*2+1]-'0')*(rect.height-16)/6f);
        for(int i=0;i<path.Length/2-1;i++)
        {
            Vector2 a=Point(i),b=Point(i+1);
            Rect line=Mathf.Abs(a.x-b.x)<.01f
                ?new Rect(a.x-1.25f,Mathf.Min(a.y,b.y),2.5f,Mathf.Abs(a.y-b.y)+1)
                :new Rect(Mathf.Min(a.x,b.x),a.y-1.25f,Mathf.Abs(a.x-b.x)+1,2.5f);
            HudFill(line,ArenaAccents[index]);
        }
        Vector2 start=Point(0),end=Point(path.Length/2-1);
        HudFill(new Rect(start.x-3,start.y-3,6,6),Color.white);
        HudFill(new Rect(end.x-3,end.y-3,6,6),new Color(1f,.65f,.15f));
    }

    void DrawArenaSelection()
    {
        Matrix4x4 old=GUI.matrix;
        float scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);
        if(scale<=0) return;
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,Vector3.one*scale);
        GUI.Box(new Rect(15,15,1570,870),"");
        GUI.Label(new Rect(45,35,1250,40),"WYBIERZ MAPĘ · SIEDEM KRAIN",title);
        GUI.Label(new Rect(45,85,1450,35),"Obie drużyny zagrają na tej samej trasie. Każda arena ma własny układ i oprawę.",heading);
        for(int i=0;i<ArenaNames.Length;i++)
        {
            int col=i%4,row=i/4;float x=45+col*380,y=155+row*205;
            GUI.Box(new Rect(x,y,350,175),"");
            Color previous=GUI.color;GUI.color=ArenaAccents[i];
            GUI.DrawTexture(new Rect(x+12,y+12,8,150),Texture2D.whiteTexture);GUI.color=previous;
            GUI.Label(new Rect(x+35,y+20,300,30),ArenaNames[i],heading);
            GUI.Label(new Rect(x+35,y+60,175,55),ArenaDescriptions[i],small);
            DrawArenaThumbnail(i,new Rect(x+230,y+20,95,90));
            if(GUI.Button(new Rect(x+35,y+120,280,38),matchArenaChoice==i?"WYBRANA":"WYBIERZ")) matchArenaChoice=i;
        }
        GUI.Label(new Rect(45,620,1400,35),"Jedna droga · około 360 m · automatyczne wieże · bez ulepszeń",heading);
        if(GUI.Button(new Rect(45,720,260,55),"← WRÓĆ DO DRAFTU")) matchArenaSelection=false;
        if(GUI.Button(new Rect(325,720,360,55),"ROZSTAW WIEŻE I ZACZNIJ →"))
        {
            matchOurSeed=Random.Range(1,int.MaxValue);matchTheirSeed=Random.Range(1,int.MaxValue);
            matchArenaSelection=false;RebuildArena(matchArenaChoice);StartMatchRound(1);
        }
        GUI.matrix=old;
    }
}
