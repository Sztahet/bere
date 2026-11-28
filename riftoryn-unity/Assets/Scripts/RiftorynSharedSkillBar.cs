using UnityEngine;

public partial class RiftorynTrial
{
    const int SharedActiveSkillCount=3, SharedSkillIconRows=4;
    const float SkillTileGap=7f, SkillBarBottom=16f;
    static readonly Color SkillTrayColor=new(.045f,.034f,.060f,.96f);

    Texture2D sharedSkillIcons;
    GUIStyle skillTimerStyle, skillTimerShadow, skillKeyStyle;
    bool loadedSkillIcons;

    float SharedSkillTileSize()=>
        UsesUnifiedRules&&phase==Phase.Playing&&IsRunning(player)
            ?Mathf.Clamp(Mathf.Round(Mathf.Min(Screen.width,Screen.height)*.09f),36f,64f)
            :0f;

    static int SharedSkillIconRow(int hero)=>hero switch
    {
        KaelHero=>0,
        NeraHero=>1,
        ZetHero=>2,
        KonevaHero=>3,
        _=>0
    };

    // The original atlas has four complete kits. Reuse its existing symbols for
    // newer kits and the older, Q-only prototypes until they have individual art.
    static (int row,int column) SkillIcon(int hero,int slot)
    {
        if(hero==MikoHero) return slot switch { 0=>(2,1), 1=>(0,1), _=>(2,0) };
        if(hero==SarnaHero) return slot switch { 0=>(2,2), 1=>(1,2), _=>(1,0) };
        if(hero==SailaHero) return slot switch { 0=>(1,1), 1=>(1,0), _=>(2,0) };
        if(hero==1) return slot switch { 0=>(0,1), 1=>(0,2), _=>(2,0) };
        if(hero==4) return slot switch { 0=>(1,0), 1=>(1,2), _=>(2,2) };
        if(hero==5) return slot switch { 0=>(1,2), 1=>(1,0), _=>(0,0) };
        if(hero==ThoraHero) return slot switch { 0=>(1,2), 1=>(0,0), _=>(0,2) };
        if(hero==BromHero) return slot switch { 0=>(0,2), 1=>(0,0), _=>(1,0) };
        if(hero==MorrakHero) return slot switch { 0=>(3,1), 1=>(3,2), _=>(3,0) };
        if(hero==PyrraHero) return slot switch { 0=>(0,0), 1=>(0,2), _=>(3,2) };
        if(hero==AnaHero) return slot switch { 0=>(1,2), 1=>(3,1), _=>(0,0) };
        if(hero==LumenHero) return slot switch { 0=>(1,0), 1=>(2,1), _=>(2,2) };
        if(!IsSharedHero(hero)) return hero switch
        {
            1=>(0,1), 4=>(1,0), 5=>(1,2), 9=>(1,0), 10=>(1,1), _=>(0,0)
        };
        return (SharedSkillIconRow(hero),slot);
    }

    static Color SharedSkillAccent(int hero)=>hero switch
    {
        KaelHero=>new Color(.96f,.59f,.30f),
        NeraHero=>new Color(.59f,.91f,.94f),
        ZetHero=>new Color(.58f,.72f,1f),
        KonevaHero=>new Color(.88f,.62f,.96f),
        MikoHero=>new Color(.85f,.78f,.35f),
        SarnaHero=>new Color(.57f,.85f,.42f),
        SailaHero=>new Color(.55f,.86f,1f),
        1=>new Color(1f,.58f,.31f),
        4=>new Color(.55f,.86f,1f),
        5=>new Color(.57f,.85f,.42f),
        ThoraHero=>new Color(.57f,.85f,.42f),
        BromHero=>new Color(.76f,.57f,.34f),
        MorrakHero=>new Color(.70f,.48f,.90f),
        PyrraHero=>new Color(1f,.58f,.31f),
        AnaHero=>new Color(.57f,.85f,.42f),
        LumenHero=>new Color(1f,.90f,.58f),
        _=>new Color(.91f,.74f,.47f)
    };

    void DrawSharedSkillBar(float tile)
    {
        if(!loadedSkillIcons)
        {
            sharedSkillIcons=Resources.Load<Texture2D>("shared-skill-icons");
            loadedSkillIcons=true;
        }
        if(skillTimerStyle==null)
        {
            skillTimerStyle=new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter,
                fontStyle=FontStyle.Bold,normal={textColor=Color.white}};
            skillTimerShadow=new GUIStyle(skillTimerStyle){normal={textColor=new Color(.035f,.025f,.045f)}};
            skillKeyStyle=new GUIStyle(skillTimerStyle){fontSize=12};
        }

        skillTimerStyle.fontSize=skillTimerShadow.fontSize=Mathf.RoundToInt(tile*.35f);
        float width=SharedActiveSkillCount*tile+(SharedActiveSkillCount-1)*SkillTileGap;
        float left=Mathf.Round((Screen.width-width)*.5f);
        float top=Screen.height-SkillBarBottom-tile;
        HudFill(new Rect(left-6,top-6,width+12,tile+12),SkillTrayColor);
        Color accent=SharedSkillAccent(player.hero);
        var cooldowns=SharedAbilityCooldowns(player.hero);
        int hovered=-1;
        for(int slot=0;slot<SharedActiveSkillCount;slot++)
        {
            Rect bounds=new(left+slot*(tile+SkillTileGap),top,tile,tile);
            bool available=slot==0||IsSharedHero(player.hero);
            float remaining=slot==0?player.cooldown:available?SharedCooldownLeft(player,slot==2):0f;
            float duration=slot==0?Cooldowns[player.hero]:slot==1?cooldowns.secondary:cooldowns.ultimate;
            DrawSharedSkillTile(bounds,accent,slot,remaining,duration,available);
            if(!mouseCaptured&&bounds.Contains(Event.current.mousePosition)) hovered=slot;
        }
        if(hovered>=0) DrawSharedSkillTooltip(hovered,top);
    }

    void DrawSharedSkillTile(Rect bounds,Color accent,int slot,float remaining,float duration,bool available)
    {
        bool ready=available&&remaining<=0;
        HudFill(bounds,ready?Color.Lerp(SkillTrayColor,accent,.73f):new Color(.24f,.21f,.30f));
        Rect inset=new(bounds.x+2,bounds.y+2,bounds.width-4,bounds.height-4);
        HudFill(inset,Color.Lerp(new Color(.075f,.062f,.105f),accent,.14f));
        if(sharedSkillIcons&&available)
        {
            float columnWidth=1f/SharedActiveSkillCount, rowHeight=1f/SharedSkillIconRows;
            var icon=SkillIcon(player.hero,slot);
            GUI.DrawTextureWithTexCoords(inset,sharedSkillIcons,
                new Rect(icon.column*columnWidth,1f-(icon.row+1)*rowHeight,columnWidth,rowHeight),true);
        }

        if(!available)
        {
            HudFill(inset,new Color(.035f,.026f,.052f,.8f));
            GUI.Label(inset,"—",skillTimerStyle);
        }

        if(available&&!ready)
        {
            float fraction=Mathf.Clamp01(remaining/duration);
            HudFill(inset,new Color(.035f,.026f,.052f,.20f));
            HudFill(new Rect(inset.x,inset.y,inset.width,inset.height*fraction),new Color(.025f,.019f,.042f,.62f));
            string timer=remaining>=3f?Mathf.CeilToInt(remaining).ToString()
                :RiftorynL10n.Format("{0:F1}",Mathf.Ceil(remaining*10f)*.1f);
            Rect timerBounds=new(inset.x,inset.y-2,inset.width,inset.height);
            GUI.Label(new Rect(timerBounds.x+1,timerBounds.y+1,timerBounds.width,timerBounds.height),timer,skillTimerShadow);
            GUI.Label(timerBounds,timer,skillTimerStyle);
        }

        float keyWidth=Mathf.Clamp(bounds.width*.32f,14f,21f);
        Rect key=new(bounds.xMax-keyWidth-3,bounds.yMax-17,keyWidth,15);
        HudFill(key,SkillTrayColor);
        GUI.Label(key,SharedSkillKeys[slot],skillKeyStyle);
        if(ready&&available) HudFill(new Rect(inset.x,inset.y,inset.width,2),accent);
    }

    void DrawSharedSkillTooltip(int slot,float top)
    {
        (string name,string description) info=IsSharedHero(player.hero)?SharedSkillInfo(player.hero,slot):slot==0
            ?(RiftorynCodexData.Text("Zdolność testowa","Prototype ability"),RiftorynCodexData.Text("Dotychczasowa zdolność tej postaci.","This hero's existing prototype ability."))
            :(RiftorynCodexData.Text("W przygotowaniu","In development"),RiftorynCodexData.Text("Ta umiejętność nie jest jeszcze dostępna w Unity.","This ability is not available in Unity yet."));
        float width=Mathf.Min(316f,Screen.width-24f);
        float textHeight=Mathf.Max(28f,small.CalcHeight(new GUIContent(info.description),width-24f));
        float height=textHeight+42f;
        Rect tooltip=new((Screen.width-width)*.5f,top-height-10f,width,height);
        HudFill(tooltip,new Color(.32f,.25f,.40f));
        HudFill(new Rect(tooltip.x+1,tooltip.y+1,tooltip.width-2,tooltip.height-2),SkillTrayColor);
        GUI.Label(new Rect(tooltip.x+12,tooltip.y+6,tooltip.width-24,24),
            $"{SharedSkillKeys[slot]} · {info.name}",heading);
        GUI.Label(new Rect(tooltip.x+12,tooltip.y+31,tooltip.width-24,textHeight),info.description,small);
    }
}
