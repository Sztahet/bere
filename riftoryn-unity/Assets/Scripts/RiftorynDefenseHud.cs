using System.Collections.Generic;
using UnityEngine;

public partial class RiftorynTrial
{
    GUIStyle defenseBattleTitle,defenseBattleHeading,defenseBattleText,defenseBattleMuted,defenseBattleNumber,defenseBattleButton;
    readonly Dictionary<int,Texture2D> defenseElementIcons=new();
    static readonly Color DefenseGold=new(.88f,.72f,.43f), DefensePaper=new(.94f,.90f,.83f), DefenseMuted=new(.65f,.63f,.69f);
    static readonly Color DefenseBack=new(.055f,.045f,.08f,.97f), DefenseBorder=new(.30f,.25f,.32f,.95f);
    static readonly Color[] DefenseAttackerColors={new(.98f,.72f,.32f),new(.38f,.79f,.93f)};

    void InitDefenseBattleStyles()
    {
        if(defenseBattleTitle!=null)return;
        defenseBattleTitle=new GUIStyle(GUI.skin.label){fontSize=23,fontStyle=FontStyle.Bold,wordWrap=false,normal={textColor=DefensePaper}};
        defenseBattleHeading=new GUIStyle(GUI.skin.label){fontSize=16,fontStyle=FontStyle.Bold,wordWrap=false,normal={textColor=DefensePaper}};
        defenseBattleText=new GUIStyle(GUI.skin.label){fontSize=14,wordWrap=false,normal={textColor=DefensePaper}};
        defenseBattleMuted=new GUIStyle(GUI.skin.label){fontSize=13,wordWrap=false,normal={textColor=DefenseMuted}};
        defenseBattleNumber=new GUIStyle(GUI.skin.label){fontSize=12,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter,normal={textColor=Color.white}};
        defenseBattleButton=new GUIStyle(GUI.skin.label){fontSize=13,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter,normal={textColor=DefensePaper}};
    }
    void DefenseBattlePanel(Rect rect)
    {
        HudFill(rect,DefenseBorder);
        HudFill(new Rect(rect.x+1,rect.y+1,rect.width-2,rect.height-2),DefenseBack);
    }
    bool DefenseBattleButton(Rect rect,string label,bool primary)
    {
        bool hover=rect.Contains(Event.current.mousePosition);
        HudFill(rect,primary?(hover?new Color(.81f,.67f,.43f):new Color(.66f,.52f,.34f)):(hover?new Color(.24f,.21f,.27f):new Color(.13f,.11f,.16f)));
        Color old=GUI.color;GUI.color=primary?new Color(.12f,.09f,.12f):Color.white;
        bool clicked=GUI.Button(rect,label,defenseBattleButton);GUI.color=old;
        return clicked;
    }
    Texture2D DefenseElementIcon(int type)
    {
        if(!defenseElementIcons.TryGetValue(type,out var icon))
        {
            icon=Resources.Load<Texture2D>("CodexIcons/elements/"+ResourceNames[type]);
            defenseElementIcons[type]=icon;
        }
        return icon;
    }
    void DefenseBattleBar(Rect rect,float value,float maximum,Color color)
    {
        HudFill(rect,new Color(.12f,.10f,.15f));
        float fraction=maximum>0?Mathf.Clamp01(value/maximum):0;
        if(fraction>0)HudFill(new Rect(rect.x,rect.y,rect.width*fraction,rect.height),color);
    }
    void DrawDefenseRunnerCard(Runner runner,int number,Rect rect,bool en)
    {
        DefenseBattlePanel(rect);
        if(runner==null||!runner.obj)
        {
            GUI.Label(new Rect(rect.x+14,rect.y+14,rect.width-28,24),en?"NO SECOND ATTACKER":"BRAK DRUGIEGO ATAKUJĄCEGO",defenseBattleMuted);
            return;
        }
        Color accent=DefenseAttackerColors[number-1];
        HudFill(new Rect(rect.x,rect.y,3,rect.height),accent);
        var hero=MatchHero(runner.hero);
        Texture2D portrait=hero!=null?DraftPortrait(hero.id):null;
        Rect image=new(rect.x+12,rect.y+12,68,79);
        HudFill(image,new Color(.11f,.09f,.14f));
        if(portrait)GUI.DrawTexture(image,portrait,ScaleMode.ScaleAndCrop,true);
        HudFill(new Rect(image.x,image.y,20,20),accent);
        GUI.Label(new Rect(image.x,image.y,20,20),number.ToString(),defenseBattleNumber);
        float x=rect.x+92,width=rect.width-104;
        GUI.Label(new Rect(x,rect.y+10,width,22),HeroNames[runner.hero].ToUpperInvariant(),defenseBattleHeading);
        string state=runner.done?(en?"REACHED EXIT":"NA MECIE"):runner.hp<=0?(en?"DEFEATED":"POKONANY"):(en?"RUNNING":"W DRODZE");
        GUI.Label(new Rect(x,rect.y+32,width,18),state,defenseBattleMuted);
        float hp=Mathf.Max(0,runner.hp),max=MaxHp[runner.hero];
        DefenseBattleBar(new Rect(x,rect.y+57,width,9),hp,max,runner.hp<=0?DefenseMuted:accent);
        GUI.Label(new Rect(x,rect.y+70,width,18),$"{Mathf.CeilToInt(hp)} / {Mathf.CeilToInt(max)} HP",defenseBattleText);
        float progress=runner.done?1f:Mathf.Clamp01(Project(runner.obj.transform.position,out _)/total);
        GUI.Label(new Rect(rect.x+12,rect.y+102,rect.width-24,18),(en?"ROUTE  ":"TRASA  ")+Mathf.RoundToInt(progress*100)+"%",defenseBattleMuted);
        DefenseBattleBar(new Rect(rect.x+12,rect.y+123,rect.width-24,5),progress,1,accent);
    }
    void DrawDefenseTowerMarkers(float scale,Vector2 offset)
    {
        foreach(var tower in towers)
        {
            if(!tower.obj)continue;
            Vector3 screen=cam.WorldToScreenPoint(Slot(tower.slot)+Vector3.up*6f);
            if(screen.z<=0)continue;
            Vector2 p=new((screen.x-offset.x)/scale,(Screen.height-screen.y-offset.y)/scale);
            if(p.x<0||p.x>1600||p.y<0||p.y>900)continue;
            bool alive=tower.hp>0;Color color=alive?TowerColors[tower.type]:new Color(.35f,.34f,.38f);
            HudFill(new Rect(p.x-14,p.y-14,28,28),new Color(.035f,.03f,.05f,.94f));
            HudFill(new Rect(p.x-14,p.y-14,28,2),color);
            var icon=DefenseElementIcon(tower.type);
            if(icon){Color old=GUI.color;GUI.color=alive?Color.white:new Color(.45f,.45f,.45f);GUI.DrawTexture(new Rect(p.x-10,p.y-10,20,20),icon,ScaleMode.ScaleToFit,true);GUI.color=old;}
            else GUI.Label(new Rect(p.x-10,p.y-10,20,20),(tower.type+1).ToString(),defenseBattleNumber);
            DefenseBattleBar(new Rect(p.x-14,p.y+15,28,3),tower.hp,100,color);
        }
    }
    void DrawDefenseRunnerMarker(Runner runner,int number,float scale,Vector2 offset)
    {
        if(!IsRunning(runner)||!runner.obj)return;
        Vector3 screen=cam.WorldToScreenPoint(runner.obj.transform.position+Vector3.up*2f);
        if(screen.z<=0)return;
        Vector2 p=new((screen.x-offset.x)/scale,(Screen.height-screen.y-offset.y)/scale);
        if(p.x<0||p.x>1600||p.y<0||p.y>900)return;
        Runner other=number==1?ally:player;
        if(IsRunning(other)&&other.obj)
        {
            Vector3 otherScreen=cam.WorldToScreenPoint(other.obj.transform.position+Vector3.up*2f);
            if(otherScreen.z>0&&Vector2.Distance(new Vector2(screen.x,screen.y),new Vector2(otherScreen.x,otherScreen.y))<30f*scale)
                p.x+=number==1?-15f:15f;
        }
        Color color=DefenseAttackerColors[number-1];
        HudFill(new Rect(p.x-12,p.y-32,24,24),new Color(.035f,.03f,.05f,.94f));
        HudFill(new Rect(p.x-12,p.y-32,3,24),color);
        GUI.Label(new Rect(p.x-9,p.y-32,21,24),number.ToString(),defenseBattleNumber);
        DefenseBattleBar(new Rect(p.x-15,p.y-5,30,4),runner.hp,MaxHp[runner.hero],color);
    }
    void DrawDefenseBattle()
    {
        if(player==null||!player.obj)return;
        InitDefenseBattleStyles();
        DrawCombatTexts();
        float scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);
        if(scale<=0)return;
        Vector2 offset=new((Screen.width-1600*scale)*.5f,(Screen.height-900*scale)*.5f);
        Matrix4x4 previous=GUI.matrix;
        GUI.matrix=Matrix4x4.TRS(new Vector3(offset.x,offset.y,0),Quaternion.identity,Vector3.one*scale);
        bool en=RiftorynL10n.CurrentCode=="EN";
        DrawDefenseTowerMarkers(scale,offset);
        DrawDefenseRunnerMarker(player,1,scale,offset);
        DrawDefenseRunnerMarker(ally,2,scale,offset);
        int attackers=ally!=null?2:1,arrived=(player.done?1:0)+(ally!=null&&ally.done?1:0),activeTowers=0;
        foreach(var tower in towers)if(tower.hp>0)activeTowers++;
        Rect header=new(24,20,440,88);DefenseBattlePanel(header);
        HudFill(new Rect(header.x,header.y,4,header.height),DefenseGold);
        GUI.Label(new Rect(header.x+18,header.y+9,380,18),en?"RIFTORYN  /  DEFENSE TRIAL":"RIFTORYN  /  PRÓBA OBRONY",defenseBattleMuted);
        GUI.Label(new Rect(header.x+18,header.y+29,400,32),en?"HOLD THE LINE":"ZATRZYMAJ ATAKUJĄCYCH",defenseBattleTitle);
        GUI.Label(new Rect(header.x+18,header.y+62,400,18),en?"The towers fire automatically":"Wieże prowadzą automatyczny ostrzał",defenseBattleMuted);
        Rect summary=new(1180,20,396,88);DefenseBattlePanel(summary);
        int seconds=Mathf.FloorToInt(elapsed);
        GUI.Label(new Rect(summary.x+16,summary.y+11,110,18),en?"TIME":"CZAS",defenseBattleMuted);
        GUI.Label(new Rect(summary.x+16,summary.y+32,110,32),$"{seconds/60:00}:{seconds%60:00}",defenseBattleTitle);
        HudFill(new Rect(summary.x+135,summary.y+15,1,55),DefenseBorder);
        GUI.Label(new Rect(summary.x+155,summary.y+11,225,18),en?"REACHED EXIT":"NA MECIE",defenseBattleMuted);
        GUI.Label(new Rect(summary.x+155,summary.y+32,225,32),$"{arrived} / {attackers}",defenseBattleTitle);
        GUI.Label(new Rect(24,128,280,24),en?"ATTACKERS":"ATAKUJĄCY",defenseBattleHeading);
        DrawDefenseRunnerCard(player,1,new Rect(24,158,280,142),en);
        if(ally!=null)DrawDefenseRunnerCard(ally,2,new Rect(24,310,280,142),en);
        Rect fort=new(24,ally!=null?468:316,280,85);DefenseBattlePanel(fort);
        GUI.Label(new Rect(fort.x+14,fort.y+10,245,19),en?"DEFENSE":"TWOJA OBRONA",defenseBattleMuted);
        GUI.Label(new Rect(fort.x+14,fort.y+33,245,25),$"{activeTowers} / {towers.Count}  "+(en?"TOWERS ACTIVE":"WIEŻ AKTYWNYCH"),defenseBattleHeading);
        Rect legend=new(1305,128,271,Mathf.Min(370,60+towers.Count*38));DefenseBattlePanel(legend);
        GUI.Label(new Rect(legend.x+14,legend.y+11,240,22),en?"TOWERS":"WIEŻE",defenseBattleHeading);
        float y=legend.y+42;
        foreach(var tower in towers)
        {
            Color color=tower.hp>0?TowerColors[tower.type]:DefenseMuted;
            var icon=DefenseElementIcon(tower.type);
            if(icon){Color old=GUI.color;GUI.color=tower.hp>0?Color.white:new Color(.45f,.45f,.45f);GUI.DrawTexture(new Rect(legend.x+12,y+1,25,25),icon,ScaleMode.ScaleToFit,true);GUI.color=old;}
            string name=RiftorynL10n.Translate(TowerNames[tower.type]);
            string status=tower.hp<=0?(en?"DESTROYED":"ZNISZCZONA"):tower.disabled>0?(en?"DISABLED":"WYŁĄCZONA"):$"{Mathf.CeilToInt(tower.hp)} HP";
            GUI.Label(new Rect(legend.x+44,y,legend.width-56,19),name+"  ·  "+status,defenseBattleText);
            DefenseBattleBar(new Rect(legend.x+44,y+22,legend.width-60,4),tower.hp,100,color);
            y+=38;
        }
        if(phase==Phase.Finished)
        {
            bool win=arrived==0;
            Rect modal=new(525,320,550,235);DefenseBattlePanel(modal);
            HudFill(new Rect(modal.x,modal.y,modal.width,4),win?DefenseGold:new Color(.78f,.32f,.30f));
            GUI.Label(new Rect(modal.x+24,modal.y+20,500,32),win?(en?"DEFENSE HELD":"OBRONA UTRZYMANA"):(en?"THEY BROKE THROUGH":"ATAKUJĄCY PRZESZLI"),defenseBattleTitle);
            GUI.Label(new Rect(modal.x+24,modal.y+67,500,24),(en?"Attackers at the exit: ":"Atakujący na mecie: ")+arrived+" / "+attackers,defenseBattleText);
            GUI.Label(new Rect(modal.x+24,modal.y+98,500,20),(en?"Review the layout and try again.":"Zmień ustawienie lub powtórz próbę."),defenseBattleMuted);
            if(DefenseBattleButton(new Rect(modal.x+24,modal.y+155,235,48),en?"TRY AGAIN":"POWTÓRZ PRÓBĘ",true))
            {
                var layout=new List<(int slot,int type)>();foreach(var tower in towers)layout.Add((tower.slot,tower.type));
                Setup();foreach(var item in layout)AddTower(item.slot,item.type);
                GUI.matrix=previous;GUIUtility.ExitGUI();
            }
            if(DefenseBattleButton(new Rect(modal.x+270,modal.y+155,250,48),en?"MAIN MENU":"MENU GŁÓWNE",false))
            {ReturnToMenu();GUI.matrix=previous;GUIUtility.ExitGUI();}
        }
        GUI.matrix=previous;
    }
}
