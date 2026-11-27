using System.Collections.Generic;
using UnityEngine;

public partial class RiftorynTrial
{
    // The prototype uses the heroes whose basic trial skills already exist.
    static readonly int[] MatchRoster={0,1,2,3,4,5,9,10,12,13};
    readonly List<int> matchTeam=new(), matchOpponents=new();
    readonly List<Runner> matchExtras=new();
    int matchRound, matchOurArrivals, matchTheirArrivals, matchOurSeed, matchTheirSeed;
    float matchOurTime, matchTheirTime;
    bool matchDraft;
    static readonly bool[] MatchPickOrder={true,false,false,true,true,false,false,true,true,false};
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
        if(!matchDraft||matchPickIndex>=MatchPickOrder.Length||!MatchAvailable(hero)) return;
        bool human=MatchPickOrder[matchPickIndex];
        if(human) matchTeam.Add(hero); else matchOpponents.Add(hero);
        matchPickIndex++;
        matchPreview=-1;
        if(!human)
        {
            var data=MatchHero(hero);
            matchAiExplanation=data==null?HeroNames[hero]:HeroNames[hero]+" · "+data.role+" · "+data.element;
        }
        if(matchPickIndex<MatchPickOrder.Length&&!MatchPickOrder[matchPickIndex])
            matchAiPickAt=Time.unscaledTime+.7f;
    }

    void UpdateMatchDraft()
    {
        if(!matchDraft||matchPickIndex>=MatchPickOrder.Length||MatchPickOrder[matchPickIndex]||Time.unscaledTime<matchAiPickAt) return;
        int best=-1;float bestScore=float.NegativeInfinity;
        foreach(int hero in MatchRoster)
        {
            if(!MatchAvailable(hero)) continue;
            float score=MatchAiScore(hero);
            if(score>bestScore){bestScore=score;best=hero;}
        }
        if(best>=0) MatchConfirmPick(best);
    }

    IEnumerable<Runner> AllRunners()
    {
        yield return player;
        yield return ally;
        if(MatchMode) foreach(var runner in matchExtras) yield return runner;
    }

    public void OpenMatch()
    {
        mode=TrialMode.Match;
        menuOpen=false;
        matchRound=0;
        matchDraft=true;
        matchPickIndex=0; matchPreview=-1; matchAiPickAt=0; matchAiExplanation="";
        matchTeam.Clear();
        matchOpponents.Clear();
        selectedHero=0;
        allyChoice=2+1;
        Setup();
    }

    void StartMatchRound(int round)
    {
        matchRound=round;
        matchDraft=false;
        var team=round==1?matchTeam:matchOpponents;
        selectedHero=team[0];
        allyChoice=team[1]+2;
        Setup();
        for(int i=2;i<5;i++)
        {
            var runner=MakeRunner(team[i],true);
            runner.obj.transform.position=At(0)+Vector3.right*((i-2)*.55f-.55f);
            matchExtras.Add(runner);
        }
        // Both sides receive a separately generated basic defense.
        foreach(var placement in SharedDefense(round==1?matchTheirSeed:matchOurSeed))
            AddTower(placement.slot,placement.type);
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
        bool done=matchPickIndex>=MatchPickOrder.Length;
        bool playerTurn=!done&&MatchPickOrder[matchPickIndex];
        string turn=done?"DRAFT ZAKOŃCZONY":playerTurn?"TERAZ WYBIERASZ TY":"TERAZ WYBIERA AI";
        GUI.Label(new Rect(40,70,1100,30),$"{turn}  ·  wybór {Mathf.Min(matchPickIndex+1,10)} / 10",heading);
        GUI.Label(new Rect(40,105,1500,25),"Kolejność: A – B – B – A – A – B – B – A – A – B  ·  A: Twoja drużyna  ·  B: AI",small);
        GUI.Box(new Rect(40,150,220,460),"TWOJA DRUŻYNA  "+matchTeam.Count+" / 5");
        GUI.Box(new Rect(1340,150,220,460),"DRUŻYNA AI  "+matchOpponents.Count+" / 5");
        for(int i=0;i<5;i++)
        {
            GUI.Label(new Rect(55,190+i*70,190,50),i<matchTeam.Count?(i==0?"TY · ":"AI · ")+HeroNames[matchTeam[i]]:"— wolne miejsce —",heading);
            GUI.Label(new Rect(1355,190+i*70,190,50),i<matchOpponents.Count?HeroNames[matchOpponents[i]]:"— wolne miejsce —",heading);
        }
        GUI.Box(new Rect(280,150,1040,460),"");
        for(int i=0;i<MatchRoster.Length;i++)
        {
            int hero=MatchRoster[i];
            var data=MatchHero(hero);
            float x=300+(i%5)*200,y=175+(i/5)*185;
            bool available=MatchAvailable(hero);
            GUI.Box(new Rect(x,y,180,165),"");
            var portrait=data!=null?DraftPortrait(data.id):null;
            if(portrait) GUI.DrawTexture(new Rect(x+8,y+5,80,100),portrait,ScaleMode.ScaleAndCrop);
            GUI.Label(new Rect(x+92,y+10,85,45),HeroNames[hero],small);
            if(data!=null) GUI.Label(new Rect(x+92,y+54,85,65),data.role+"\n"+data.element,small);
            string button=matchTeam.Contains(hero)?"DRUŻYNA A":matchOpponents.Contains(hero)?"DRUŻYNA B":matchPreview==hero?"WYBRANY":"WYBIERZ";
            GUI.enabled=playerTurn&&available;
            if(GUI.Button(new Rect(x+8,y+120,164,35),button)) matchPreview=hero;
            GUI.enabled=true;
        }
        if(matchPreview>=0&&MatchAvailable(matchPreview))
        {
            var data=MatchHero(matchPreview);
            GUI.Label(new Rect(300,645,1000,34),"Wybór: "+HeroNames[matchPreview]+(data==null?"":" · "+data.role+" · "+data.element)+$" · HP {MaxHp[matchPreview]} · {Speed[matchPreview]:F1} m/s",heading);
        }
        else if(!string.IsNullOrEmpty(matchAiExplanation))
            GUI.Label(new Rect(300,645,1000,34),"Ostatni wybór AI: "+matchAiExplanation,heading);
        GUI.Label(new Rect(300,690,1020,42),"AI dobiera brakujące role, różne żywioły oraz wytrzymałość i szybkość. Pierwszym wybranym bohaterem sterujesz Ty.",small);
        GUI.enabled=playerTurn&&matchPreview>=0&&MatchAvailable(matchPreview);
        if(GUI.Button(new Rect(300,750,300,50),"ZATWIERDŹ BOHATERA")) MatchConfirmPick(matchPreview);
        GUI.enabled=done;
        if(GUI.Button(new Rect(620,750,300,50),"ROZSTAW WIEŻE I ZACZNIJ"))
        {
            matchOurSeed=Random.Range(1,int.MaxValue);
            matchTheirSeed=Random.Range(1,int.MaxValue);
            StartMatchRound(1);
        }
        GUI.enabled=true;
        if(GUI.Button(new Rect(40,815,200,40),"← MENU")) ReturnToMenu();
        GUI.matrix=old;
    }

    void DrawMatch()
    {
        float w=Screen.width, h=Screen.height;
        if(matchDraft)
        {
            DrawMatchDraft();
            return;
        }
        if(phase==Phase.Playing)
        {
            GUI.Box(new Rect(0,0,w,100),"");
            GUI.Label(new Rect(18,8,w-36,30),matchRound==1?"MECZ 5 NA 5 · UCIECZKA TWOJEJ DRUŻYNY":"MECZ 5 NA 5 · UCIECZKA DRUŻYNY AI",title);
            int alive=0, arrived=0;
            foreach(var runner in AllRunners()){if(IsRunning(runner))alive++;if(runner!=null&&runner.done)arrived++;}
            GUI.Label(new Rect(18,42,w-36,24),$"Czas {elapsed:F1} s  ·  Biegnie {alive}  ·  Meta {arrived}/5",heading);
            GUI.Label(new Rect(18,70,w-36,24),matchRound==1?"WASD: ruch · Mysz: kamera · Q: zdolność · Spacja: unik · Esc: kursor":"Obrona automatyczna · obserwujesz drużynę przeciwnika",small);
            DrawCombatTexts();
            if(matchRound==1){DrawFogExposure();DrawHud();}
            if(GUI.Button(new Rect(w-150,10,130,28),"MENU")) ReturnToMenu();
            return;
        }
        if(phase!=Phase.Finished) return;
        GUI.Box(new Rect(w/2-300,h/2-130,600,260),"");
        string outcome=matchRound==1?"KONIEC WASZEJ UCIECZKI":matchOurArrivals>matchTheirArrivals?"WYGRYWASZ":matchOurArrivals<matchTheirArrivals?"WYGRYWA AI":"REMIS";
        GUI.Label(new Rect(w/2-270,h/2-110,540,40),outcome,title);
        GUI.Label(new Rect(w/2-270,h/2-55,540,40),$"Twoja drużyna: {matchOurArrivals}/5 · {matchOurTime:F1} s",heading);
        if(matchRound==2) GUI.Label(new Rect(w/2-270,h/2-20,540,40),$"Drużyna AI: {matchTheirArrivals}/5 · {matchTheirTime:F1} s",heading);
        if(GUI.Button(new Rect(w/2-260,h/2+55,250,45),matchRound==1?"UCIECZKA AI →":"NOWY MECZ"))
        {
            if(matchRound==1) StartMatchRound(2); else OpenMatch();
        }
        if(GUI.Button(new Rect(w/2+10,h/2+55,250,45),"MENU GŁÓWNE")) ReturnToMenu();
    }
}
