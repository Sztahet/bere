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
        UsesUnifiedRules&&phase==Phase.Playing&&IsRunning(player)&&IsSharedHero(player.hero)
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

    static Color SharedSkillAccent(int hero)=>hero switch
    {
        KaelHero=>new Color(.96f,.59f,.30f),
        NeraHero=>new Color(.59f,.91f,.94f),
        ZetHero=>new Color(.58f,.72f,1f),
        KonevaHero=>new Color(.88f,.62f,.96f),
        MikoHero=>new Color(.85f,.78f,.35f),
        SarnaHero=>new Color(.57f,.85f,.42f),
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
            float remaining=slot==0?player.cooldown:SharedCooldownLeft(player,slot==2);
            float duration=slot==0?Cooldowns[player.hero]:slot==1?cooldowns.secondary:cooldowns.ultimate;
            DrawSharedSkillTile(bounds,accent,slot,remaining,duration);
            if(!mouseCaptured&&bounds.Contains(Event.current.mousePosition)) hovered=slot;
        }
        if(hovered>=0) DrawSharedSkillTooltip(hovered,top);
    }

    void DrawSharedSkillTile(Rect bounds,Color accent,int slot,float remaining,float duration)
    {
        bool ready=remaining<=0;
        HudFill(bounds,ready?Color.Lerp(SkillTrayColor,accent,.73f):new Color(.24f,.21f,.30f));
        Rect inset=new(bounds.x+2,bounds.y+2,bounds.width-4,bounds.height-4);
        HudFill(inset,Color.Lerp(new Color(.075f,.062f,.105f),accent,.14f));
        if(sharedSkillIcons&&player.hero!=MikoHero&&player.hero!=SarnaHero)
        {
            float columnWidth=1f/SharedActiveSkillCount, rowHeight=1f/SharedSkillIconRows;
            int row=SharedSkillIconRow(player.hero);
            GUI.DrawTextureWithTexCoords(inset,sharedSkillIcons,
                new Rect(slot*columnWidth,1f-(row+1)*rowHeight,columnWidth,rowHeight),true);
        }

        if(!ready)
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
        if(ready) HudFill(new Rect(inset.x,inset.y,inset.width,2),accent);
    }

    void DrawSharedSkillTooltip(int slot,float top)
    {
        var info=SharedSkillInfo(player.hero,slot);
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
