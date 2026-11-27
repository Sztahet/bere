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

    void DrawMatch()
    {
        float w=Screen.width, h=Screen.height;
        if(matchDraft)
        {
            GUI.Box(new Rect(20,20,w-40,h-40),"");
            GUI.Label(new Rect(45,35,w-90,38),"MECZ 5 NA 5 · WYBÓR DRUŻYNY",title);
            GUI.Label(new Rect(45,85,w-90,40),"Wybierz 5 różnych bohaterów. Pierwszym sterujesz Ty; pozostałymi steruje AI.",heading);
            GUI.Label(new Rect(45,125,w-90,30),"Pozostali bohaterowie tworzą drużynę przeciwnika. Komputer ustawi obie obrony.",small);
            for(int i=0;i<MatchRoster.Length;i++)
            {
                int hero=MatchRoster[i];
                int index=matchTeam.IndexOf(hero);
                string label=index>=0?$"{index+1}. {HeroNames[hero]} ✓":HeroNames[hero];
                float x=45+(i%5)*Mathf.Min(220,(w-100)/5), y=175+(i/5)*70;
                if(GUI.Button(new Rect(x,y,Mathf.Min(205,(w-120)/5),55),label))
                {
                    if(index>=0) matchTeam.RemoveAt(index);
                    else if(matchTeam.Count<5) matchTeam.Add(hero);
                }
            }
            GUI.Label(new Rect(45,335,w-90,30),$"WYBRANO {matchTeam.Count} / 5",heading);
            GUI.enabled=matchTeam.Count==5;
            if(GUI.Button(new Rect(45,380,300,52),"ROZSTAW WIEŻE I ZACZNIJ"))
            {
                matchOpponents.Clear();
                foreach(int hero in MatchRoster) if(!matchTeam.Contains(hero)) matchOpponents.Add(hero);
                matchOurSeed=Random.Range(1,int.MaxValue);
                matchTheirSeed=Random.Range(1,int.MaxValue);
                StartMatchRound(1);
            }
            GUI.enabled=true;
            if(GUI.Button(new Rect(45,445,200,40),"← MENU")) ReturnToMenu();
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
