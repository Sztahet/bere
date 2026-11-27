using System.Collections.Generic;
using UnityEngine;

public partial class RiftorynTrial
{
    // The prototype uses the heroes whose basic trial skills already exist.
    static readonly int[] MatchRoster={0,1,2,3,4,5,9,10,PyrraHero,12,13,SarnaHero};
    readonly List<int> matchTeam=new(), matchOpponents=new();
    readonly List<Runner> runners=new();
    int matchRound, matchOurArrivals, matchTheirArrivals, matchOurSeed, matchTheirSeed;
    float matchOurTime, matchTheirTime;
    readonly RiftorynGameSettings matchSettings=new();
    bool matchDraft, matchArenaSelection, matchConfiguring, matchDefenseSetup;
    readonly List<(int slot,int type)> matchOurDefense=new();
    int matchArenaChoice;
    int MatchPickCount=>matchSettings.PickCount;
    bool MatchTeamTurn(int index)=>matchSettings.IsTeamTurn(index);
    int matchPickIndex, matchPreview=-1;
    float matchAiPickAt;
    string matchAiExplanation="";

    CodexHero MatchHero(int hero)
    {
        foreach(var item in roster) if(item!=null&&PlayableHero(item.id)==hero) return item;
        return null;
    }

    bool MatchAvailable(int hero)=>System.Array.IndexOf(MatchRoster,hero)>=0
        &&!matchTeam.Contains(hero)&&!matchOpponents.Contains(hero);

    float MatchAiScore(int hero)
    {
        var candidate=MatchHero(hero);
        if(candidate==null) return Speed[hero]+MaxHp[hero]*.01f;
        int sameRole=0,sameElement=0,availableRole=0,enemyRole=0;
        foreach(int other in matchOpponents)
        {
            var item=MatchHero(other);
            if(item==null) continue;
            if(item.role==candidate.role) sameRole++;
            if(item.element==candidate.element) sameElement++;
        }
        foreach(int other in MatchRoster)
        {
            var item=MatchHero(other);
            if(item!=null&&item.role==candidate.role&&MatchAvailable(other)) availableRole++;
        }
        foreach(int other in matchTeam)
        {
            var item=MatchHero(other);
            if(item!=null&&item.role==candidate.role) enemyRole++;
        }
        // Reward missing functions and scarce roles; avoid claiming unimplemented combat counters.
        float roleWeight=candidate.role=="Tank"?7f:candidate.role=="Wsparcie"?5f:
            candidate.role=="Specjalista"?3.5f:2.5f;
        float roleScore=sameRole==0?roleWeight: -1.5f*sameRole;
        float scarcity=sameRole==0&&availableRole>0?2f/availableRole:0f;
        float contest=sameRole==0&&enemyRole>0?0.5f:0f;
        float diversity=sameElement==0?1.25f:-.75f*sameElement;
        return roleScore+scarcity+contest+diversity+Speed[hero]*.4f+MaxHp[hero]*.012f;
    }

    void MatchConfirmPick(int hero)
    {
        if(!matchDraft||matchPickIndex>=MatchPickCount||!MatchAvailable(hero)) return;
        bool human=MatchTeamTurn(matchPickIndex);
        if(human) matchTeam.Add(hero); else matchOpponents.Add(hero);
        matchPickIndex++;
        matchPreview=-1;
        if(!human)
        {
            var data=MatchHero(hero);
            matchAiExplanation=data==null?HeroNames[hero]:HeroNames[hero]+" · "+data.role+" · "+data.element;
        }
        if(matchPickIndex<MatchPickCount&&!MatchTeamTurn(matchPickIndex))
            matchAiPickAt=Time.unscaledTime+.7f;
    }

    void UpdateMatchDraft()
    {
        if(!matchDraft||matchPickIndex>=MatchPickCount||MatchTeamTurn(matchPickIndex)||Time.unscaledTime<matchAiPickAt) return;
        int best=-1;float bestScore=float.NegativeInfinity;
        foreach(int hero in MatchRoster)
        {
            if(!MatchAvailable(hero)) continue;
            float score=MatchAiScore(hero);
            if(score>bestScore){bestScore=score;best=hero;}
        }
        if(best>=0) MatchConfirmPick(best);
    }

    IEnumerable<Runner> AllRunners()=>runners;

    public void OpenMatch()
    {
        mode=TrialMode.Match;
        menuOpen=false;
        matchRound=0;
        matchDraft=false; matchArenaSelection=false; matchConfiguring=true; matchDefenseSetup=false; matchArenaChoice=0;
        matchOurDefense.Clear();
        matchPickIndex=0; matchPreview=-1; matchAiPickAt=0; matchAiExplanation="";
        matchTeam.Clear();
        matchOpponents.Clear();
        selectedHero=0;
        allyChoice=2+1;
        Setup();
    }

    void DrawMatchSettings()
    {
        Matrix4x4 old=GUI.matrix;
        float scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);
        if(scale<=0)return;
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,Vector3.one*scale);
        GUI.Box(new Rect(45,35,1510,825),"");
        GUI.Label(new Rect(80,65,1000,40),"RIFTORYN · USTAWIENIA GRY",title);
        GUI.Label(new Rect(80,115,1300,35),"Jedna gra: wybierz skład, przeciwnika i sposób przygotowania obrony.",heading);
        GUI.Label(new Rect(95,180,650,30),"LICZBA BOHATERÓW W DRUŻYNIE",heading);
        for(int i=1;i<=5;i++)
        {
            int count=i;Color previous=GUI.backgroundColor;
            GUI.backgroundColor=matchSettings.TeamSize==i?new Color(.75f,.62f,.40f):Color.white;
            if(GUI.Button(new Rect(95+(i-1)*115,220,100,48),count.ToString())) matchSettings.TeamSize=count;
            GUI.backgroundColor=previous;
        }
        GUI.Label(new Rect(95,310,650,30),"DRUGA DRUŻYNA",heading);
        if(GUI.Button(new Rect(95,350,245,48),matchSettings.Opponent?"✓ DRUŻYNA AI":"DRUŻYNA AI")) matchSettings.Opponent=true;
        if(GUI.Button(new Rect(350,350,245,48),!matchSettings.Opponent?"✓ BEZ PRZECIWNIKA":"BEZ PRZECIWNIKA")) matchSettings.Opponent=false;
        GUI.Label(new Rect(95,445,650,30),"ROZMIESZCZENIE WIEŻ",heading);
        if(GUI.Button(new Rect(95,485,245,48),!matchSettings.ManualDefense?"✓ AUTOMATYCZNIE":"AUTOMATYCZNIE")) matchSettings.ManualDefense=false;
        if(GUI.Button(new Rect(350,485,245,48),matchSettings.ManualDefense?"✓ USTAW SAMODZIELNIE":"USTAW SAMODZIELNIE")) matchSettings.ManualDefense=true;
        GUI.Label(new Rect(95,545,1200,60),matchSettings.Opponent
            ?"Automatycznie: obie obrony ustawia AI. Ręcznie: ustawiasz swoją obronę, a AI przygotowuje przeciwną."
            :"Automatycznie: AI przygotuje obronę. Ręcznie: sam przygotujesz układ do swojej próby.",small);
        GUI.Label(new Rect(840,180,600,30),"POŁĄCZENIE",heading);
        if(GUI.Button(new Rect(840,220,245,48),!matchSettings.LegacyLan?"✓ LOKALNIE":"LOKALNIE"))matchSettings.LegacyLan=false;
        if(GUI.Button(new Rect(1095,220,245,48),matchSettings.LegacyLan?"✓ LAN · TEST":"LAN · TEST"))matchSettings.LegacyLan=true;
        GUI.Label(new Rect(840,282,550,100),matchSettings.LegacyLan
            ?"LAN korzysta na razie ze starszego testu dwóch osób. Wspólny mecz sieciowy i jego ustawienia nie są jeszcze połączone."
            :"Lokalnie sterujesz jednym bohaterem; pozostałymi kieruje AI.",small);
        GUI.Label(new Rect(840,400,600,30),"LICZBA WIEŻ NA DRUŻYNĘ",heading);
        for(int i=1;i<=7;i++)
        {
            Color previous=GUI.backgroundColor;
            GUI.backgroundColor=matchSettings.TowerLimit==i?new Color(.75f,.62f,.40f):Color.white;
            if(GUI.Button(new Rect(840+(i-1)*70,440,62,44),i.ToString()))matchSettings.TowerLimit=i;
            GUI.backgroundColor=previous;
        }
        GUI.Label(new Rect(840,495,540,45),"W trybie ręcznym możesz postawić mniej wież niż wybrany limit.",small);
        if(GUI.Button(new Rect(95,745,250,52),"← MENU")){ReturnToMenu();GUI.matrix=old;GUIUtility.ExitGUI();}
        if(GUI.Button(new Rect(360,745,330,52),matchSettings.LegacyLan?"OTWÓRZ TEST LAN →":"PRZEJDŹ DO DRAFTU →"))
        {
            if(matchSettings.LegacyLan){OpenLanTest();GUI.matrix=old;GUIUtility.ExitGUI();}
            matchConfiguring=false;matchDraft=true;
        }
        GUI.matrix=old;
    }

    void StartMatchDefenseSetup()
    {
        matchDraft=false;matchArenaSelection=false;matchDefenseSetup=true;
        selectedHero=matchTeam[0];allyChoice=matchTeam.Count>1?matchTeam[1]+2:1;
        Setup();
    }

    void FinishMatchDefenseSetup()
    {
        matchOurDefense.Clear();
        foreach(var tower in towers)matchOurDefense.Add((tower.slot,tower.type));
        matchDefenseSetup=false;
        StartMatchRound(1);
    }

    void StartMatchRound(int round)
    {
        matchRound=round;
        matchDraft=false;
        var team=round==1?matchTeam:matchOpponents;
        selectedHero=team[0];
        allyChoice=team.Count>1?team[1]+2:1;
        Setup();
        for(int i=2;i<team.Count;i++)
        {
            var runner=MakeRunner(team[i],true);
            runner.obj.transform.position=At(0)+Vector3.right*((i-2)*.55f-.55f);
            runners.Add(runner);
        }
        // One layout per side, reused unchanged when the opposing team runs.
        if(matchSettings.ManualDefense&&(round==2||!matchSettings.Opponent))
            foreach(var placement in matchOurDefense) AddTower(placement.slot,placement.type);
        else
            foreach(var placement in SharedDefense(round==1?matchTheirSeed:matchOurSeed,matchSettings.TowerLimit))
                AddTower(placement.slot,placement.type);
        foreach(var tower in towers) tower.obj.SetActive(false);
        Begin();
        if(round==2){Capture(false);SetOverhead();}
    }

    void FinishMatchRound()
    {
        int arrived=0;
        foreach(var runner in AllRunners()) if(runner!=null&&runner.done) arrived++;
        if(matchRound==1){matchOurArrivals=arrived;matchOurTime=elapsed;}
        else {matchTheirArrivals=arrived;matchTheirTime=elapsed;}
    }

    void DrawMatchDraft()
    {
        Matrix4x4 old=GUI.matrix;
        float scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);
        if(scale<=0) return;
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,Vector3.one*scale);
        GUI.Box(new Rect(15,15,1570,870),"");
        GUI.Label(new Rect(40,30,1000,35),"MECZ 5 NA 5 · DRAFT",title);
        bool done=matchPickIndex>=MatchPickCount;
        bool playerTurn=!done&&MatchTeamTurn(matchPickIndex);
        string turn=done?"DRAFT ZAKOŃCZONY":playerTurn?"TERAZ WYBIERASZ TY":"TERAZ WYBIERA AI";
        GUI.Label(new Rect(40,70,1100,30),$"{turn}  ·  wybór {Mathf.Min(matchPickIndex+1,MatchPickCount)} / {MatchPickCount}",heading);
        GUI.Label(new Rect(40,105,1500,25),matchSettings.Opponent?"Naprzemienny wybór ze wspólnej puli · A: Twoja drużyna · B: AI":"Wybierz bohaterów swojej drużyny ze wspólnej puli",small);
        GUI.Box(new Rect(40,150,220,460),"TWOJA DRUŻYNA  "+matchTeam.Count+" / "+matchSettings.TeamSize);
        GUI.Box(new Rect(1340,150,220,460),matchSettings.Opponent?"DRUŻYNA AI  "+matchOpponents.Count+" / "+matchSettings.TeamSize:"BEZ DRUŻYNY AI");
        for(int i=0;i<matchSettings.TeamSize;i++)
        {
            GUI.Label(new Rect(55,190+i*70,190,50),i<matchTeam.Count?(i==0?"TY · ":"AI · ")+HeroNames[matchTeam[i]]:"— wolne miejsce —",heading);
            if(matchSettings.Opponent) GUI.Label(new Rect(1355,190+i*70,190,50),i<matchOpponents.Count?HeroNames[matchOpponents[i]]:"— wolne miejsce —",heading);
        }
        GUI.Box(new Rect(280,150,1040,460),"");
        for(int i=0;i<MatchRoster.Length;i++)
        {
            int hero=MatchRoster[i];
            var data=MatchHero(hero);
            float x=300+(i%6)*168,y=175+(i/6)*185;
            bool available=MatchAvailable(hero);
            GUI.Box(new Rect(x,y,158,165),"");
            var portrait=data!=null?DraftPortrait(data.id):null;
            if(portrait) GUI.DrawTexture(new Rect(x+8,y+5,65,100),portrait,ScaleMode.ScaleAndCrop);
            GUI.Label(new Rect(x+78,y+10,76,45),HeroNames[hero],small);
            if(data!=null) GUI.Label(new Rect(x+78,y+54,76,65),data.role+"\n"+data.element,small);
            string button=matchTeam.Contains(hero)?"DRUŻYNA A":matchOpponents.Contains(hero)?"DRUŻYNA B":matchPreview==hero?"WYBRANY":"WYBIERZ";
            GUI.enabled=playerTurn&&available;
            if(GUI.Button(new Rect(x+8,y+120,142,35),button)) matchPreview=hero;
            GUI.enabled=true;
        }
        if(matchPreview>=0&&MatchAvailable(matchPreview))
        {
            var data=MatchHero(matchPreview);
            GUI.Label(new Rect(300,645,1000,34),"Wybór: "+HeroNames[matchPreview]+(data==null?"":" · "+data.role+" · "+data.element)+$" · HP {MaxHp[matchPreview]} · {Speed[matchPreview]:F1} m/s",heading);
        }
        else if(!string.IsNullOrEmpty(matchAiExplanation))
            GUI.Label(new Rect(300,645,1000,34),"Ostatni wybór AI: "+matchAiExplanation,heading);
        GUI.Label(new Rect(300,690,1020,42),matchPreview==PyrraHero?"Pyrra · Q: Iskra chroni przed jednym trafieniem wieży przez 5 s. Umiejętność testowa.":matchPreview==SarnaHero?"Sarna · Q: przyspiesza Sarnę i pobliskiego sojusznika na 3 s.":"AI dobiera brakujące role, różne żywioły oraz wytrzymałość i szybkość. Pierwszym wybranym bohaterem sterujesz Ty.",small);
        GUI.enabled=playerTurn&&matchPreview>=0&&MatchAvailable(matchPreview);
        if(GUI.Button(new Rect(300,750,300,50),"ZATWIERDŹ BOHATERA")) MatchConfirmPick(matchPreview);
        GUI.enabled=done;
        if(GUI.Button(new Rect(620,750,300,50),"WYBIERZ MAPĘ →"))
        {
            matchArenaSelection=true;
        }
        GUI.enabled=true;
        if(GUI.Button(new Rect(40,815,200,40),"← MENU")) ReturnToMenu();
        GUI.matrix=old;
    }

    static readonly Color[] MatchAllyColors={new(.25f,.85f,.95f),new(.55f,.88f,.35f),new(.83f,.57f,1f),new(1f,.62f,.35f)};
    GUIStyle matchMarkerStyle,matchTeamStyle;

    void DrawMatchTeamHud(Rect map)
    {
        var teammates=new List<Runner>(4);
        foreach(var runner in runners)
            if(runner!=player&&runner!=null&&runner.obj) teammates.Add(runner);
        if(teammates.Count==0) return;
        if(matchMarkerStyle==null) matchMarkerStyle=new GUIStyle(small){fontSize=10,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter,wordWrap=false};
        if(matchTeamStyle==null) matchTeamStyle=new GUIStyle(small){fontSize=12,fontStyle=FontStyle.Bold,wordWrap=false};
        var markerPositions=new List<Vector2>();
        if(player!=null&&player.obj) markerPositions.Add(MinimapPoint(player.obj.transform.position,map));
        float rowHeight=Mathf.Clamp((Screen.height-150f)/teammates.Count,34f,48f);
        float width=Mathf.Min(235f,Screen.width*.27f), height=28f+rowHeight*teammates.Count;
        Rect panel=new(16,Screen.height-height-16,width,height);
        GUI.Box(panel,"");
        GUI.Label(new Rect(panel.x+10,panel.y+5,width-20,20),"SOJUSZNICY",matchTeamStyle);
        for(int i=0;i<teammates.Count;i++)
        {
            Runner runner=teammates[i];Color color=MatchAllyColors[i%MatchAllyColors.Length];
            Vector2 position=MinimapPoint(runner.obj.transform.position,map);
            Vector2 displayed=position;
            // Keep clustered markers readable while leaving them near their actual position.
            for(int attempt=0;attempt<12;attempt++)
            {
                bool overlaps=false;
                foreach(var occupied in markerPositions) if((displayed-occupied).sqrMagnitude<196f){overlaps=true;break;}
                if(!overlaps) break;
                float angle=attempt*Mathf.PI*.5f;
                displayed=position+new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*(14f+(attempt/4)*11f);
                displayed.x=Mathf.Clamp(displayed.x,map.x+7,map.xMax-7);
                displayed.y=Mathf.Clamp(displayed.y,map.y+7,map.yMax-7);
            }
            markerPositions.Add(displayed);
            Rect marker=new(displayed.x-7,displayed.y-7,14,14);
            HudFill(marker,new Color(.03f,.025f,.04f,.96f));
            Color old=GUI.color;GUI.color=runner.done||runner.hp<=0?Color.gray:color;
            GUI.Label(marker,(i+1).ToString(),matchMarkerStyle);GUI.color=old;
            float y=panel.y+27+i*rowHeight;
            string state=runner.done?"META":runner.hp<=0?"POKONANY":$"{Mathf.CeilToInt(runner.hp)} / {Mathf.CeilToInt(MaxHp[runner.hero])} HP";
            HudFill(new Rect(panel.x+10,y+2,13,13),color);
            GUI.Label(new Rect(panel.x+14,y+1,13,15),(i+1).ToString(),matchMarkerStyle);
            GUI.Label(new Rect(panel.x+28,y-1,width-35,19),HeroNames[runner.hero]+" · "+state,matchTeamStyle);
            HealthBar(new Rect(panel.x+28,y+19,width-40,8),runner.hp,MaxHp[runner.hero],color);
        }
    }

    void DrawMatch()
    {
        float w=Screen.width, h=Screen.height;
        if(matchConfiguring)
        {
            DrawMatchSettings();
            return;
        }
        if(matchDefenseSetup)
        {
            DrawDefenseEditor();
            return;
        }
        if(matchArenaSelection)
        {
            DrawArenaSelection();
            return;
        }
        if(matchDraft)
        {
            DrawMatchDraft();
            return;
        }
        if(phase==Phase.Playing)
        {
            if(matchRound==2){DrawDefenseBattle();return;}
            GUI.Box(new Rect(0,0,w,100),"");
            GUI.Label(new Rect(18,8,w-36,30),(matchRound==1?"UCIECZKA TWOJEJ DRUŻYNY · ":"UCIECZKA DRUŻYNY AI · ")+ArenaNames[matchArenaChoice],title);
            int alive=0, arrived=0;
            foreach(var runner in AllRunners()){if(IsRunning(runner))alive++;if(runner!=null&&runner.done)arrived++;}
            GUI.Label(new Rect(18,42,w-36,24),$"Czas {elapsed:F1} s  ·  Biegnie {alive}  ·  Meta {arrived}/{matchSettings.TeamSize}",heading);
            GUI.Label(new Rect(18,70,w-36,24),matchRound==1?IsSharedHero(player.hero)?"WASD: ruch · Mysz: kamera · Q / E / R: zdolności · Spacja: unik · Esc: kursor":"WASD: ruch · Mysz: kamera · Q: zdolność · Spacja: unik · Esc: kursor":"Obrona automatyczna · obserwujesz drużynę przeciwnika",small);
            DrawCombatTexts();
            if(matchRound==1){DrawFogExposure();DrawHud();}
            if(GUI.Button(new Rect(w-150,10,130,28),"MENU")) ReturnToMenu();
            return;
        }
        if(phase!=Phase.Finished) return;
        GUI.Box(new Rect(w/2-300,h/2-130,600,260),"");
        string outcome=!matchSettings.Opponent?"KONIEC PRÓBY":matchRound==1?"KONIEC WASZEJ UCIECZKI":matchOurArrivals>matchTheirArrivals?"WYGRYWASZ":matchOurArrivals<matchTheirArrivals?"WYGRYWA AI":"REMIS";
        GUI.Label(new Rect(w/2-270,h/2-110,540,40),outcome,title);
        GUI.Label(new Rect(w/2-270,h/2-55,540,40),$"Twoja drużyna: {matchOurArrivals}/{matchSettings.TeamSize} · {matchOurTime:F1} s",heading);
        if(matchRound==2) GUI.Label(new Rect(w/2-270,h/2-20,540,40),$"Drużyna AI: {matchTheirArrivals}/{matchSettings.TeamSize} · {matchTheirTime:F1} s",heading);
        if(GUI.Button(new Rect(w/2-260,h/2+55,250,45),matchRound==1&&matchSettings.Opponent?"UCIECZKA AI →":"NOWA GRA"))
        {
            if(matchRound==1&&matchSettings.Opponent) StartMatchRound(2); else OpenMatch();
        }
        if(GUI.Button(new Rect(w/2+10,h/2+55,250,45),"MENU GŁÓWNE")) ReturnToMenu();
    }
}
