using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public partial class RiftorynTrial
{
    const int SharedTowerCount=5, SharedDefenseVersion=1;
    const float SharedDiscoveryRange=20f, TowerSkillRange=17.4f;
    const float StartSafeDistance=10f, BotLeadDistance=5f, BotWaitLimit=2.5f;
    const float BotFogBuffer=12f, BotLookAhead=2.2f, DodgeReactionTime=.28f;
    bool sharedEscape;
    int defenseSeed, trainingHero, trainingAlly;
    float botWaitTime;
    Runner cameraRunner;
    RiftorynSharedScores sharedScores;
    IReadOnlyList<RiftorynSharedScores.Score> visibleScores;
    int? sharedRank;
    bool sharedScoresReadable;
    public bool SharedScoresOpen { get; private set; }

    RiftorynSharedScores ScoreStore => sharedScores??=new RiftorynSharedScores(
        Path.Combine(Application.persistentDataPath,RiftorynSharedScores.FileName));

    static bool IsSharedHero(int hero)=>hero is 0 or 2 or 3 or NeraHero;

    public void OpenSharedEscape()
    {
        sharedEscape=true;
        selectedHero=0;
        allyChoice=0;
        menuOpen=false;
        ChooseNewDefenseSeed();
        Setup();
        StartDraft(0,true);
    }

    void ChooseNewDefenseSeed()
    {
        int previous=defenseSeed;
        do defenseSeed=UnityEngine.Random.Range(1,int.MaxValue);
        while(defenseSeed==previous);
    }

    void RestartSharedEscape(bool newDefense)
    {
        if(newDefense) ChooseNewDefenseSeed();
        Setup();
        Begin();
    }

    List<(int slot,int type)> SharedDefense(int seed)
    {
        var available=new List<int>();
        float safeLength=Mathf.Min(StartSafeDistance,lengths[0]);
        Vector3 start=At(0), direction=(At(safeLength)-start).normalized;
        float clearance=Mathf.Max(TowerRange[0],TowerRange[1],TowerRange[2])+RoadHalf;
        for(int i=0;i<slots.Length;i++)
        {
            Vector3 offset=Slot(i)-start;
            float along=Mathf.Clamp(Vector3.Dot(offset,direction),0,safeLength);
            if((offset-direction*along).sqrMagnitude>=clearance*clearance) available.Add(i);
        }

        var random=new System.Random(seed);
        var placements=new List<(int slot,int type)>(SharedTowerCount);
        for(int i=0;i<SharedTowerCount;i++)
        {
            int pick=random.Next(available.Count);
            placements.Add((available[pick],random.Next(3)));
            available.RemoveAt(pick);
        }
        return placements;
    }

    void BuildSharedDefense()
    {
        foreach(var placement in SharedDefense(defenseSeed))
        {
            AddTower(placement.slot,placement.type);
            towers[towers.Count-1].obj.SetActive(false);
        }
    }

    void DiscoverSharedTowers()
    {
        foreach(var tower in towers)
        {
            if(tower.discovered) continue;
            Vector3 position=Slot(tower.slot);
            if(!WithinSharedDiscovery(player,position)&&!WithinSharedDiscovery(ally,position)) continue;
            tower.discovered=true;
            tower.obj.SetActive(true);
        }
    }

    static bool WithinSharedDiscovery(Runner runner,Vector3 towerPosition)=>
        IsRunning(runner)&&(runner.obj.transform.position-towerPosition).sqrMagnitude<=SharedDiscoveryRange*SharedDiscoveryRange;

    bool CanDisableSharedTower(Runner runner,Tower tower)=>
        tower.discovered&&tower.hp>0&&tower.disabled<=0
        &&(runner.obj.transform.position-Slot(tower.slot)).sqrMagnitude<TowerSkillRange*TowerSkillRange
        &&TowerHasClearShot(tower,runner.obj.transform.position+Vector3.up);

    void StepSharedBot(float dt)
    {
        ally.rollCooldown=Mathf.Max(0,ally.rollCooldown-dt);
        Vector3 position=ally.obj.transform.position;
        Vector3 center=At(ally.along);
        Vector3 ahead=At(Mathf.Min(total,ally.along+BotLookAhead));
        Vector3 direction=(ahead-position).normalized;
        bool partnerRunning=IsRunning(player);
        bool partnerNear=partnerRunning&&(player.obj.transform.position-position).sqrMagnitude<8f*8f;
        bool shotIncoming=IncomingSharedShot(ally);

        Tower underFire=SharedTowerAimingAt(ally);
        Tower partnerUnderFire=partnerNear?SharedTowerAimingAt(player):null;
        if(ally.cooldown<=0)
        {
            if(ally.hero==2)
            {
                Tower threat=underFire;
                if(threat==null||!CanDisableSharedTower(ally,threat)) threat=partnerUnderFire;
                if(threat!=null&&CanDisableSharedTower(ally,threat)) UseSkill(ally,threat);
            }
            else if(underFire!=null||partnerUnderFire!=null||shotIncoming||partnerNear&&IncomingSharedShot(player))
            {
                if(ally.hero!=3||underFire!=null||partnerUnderFire!=null) Skill(ally);
            }
        }

        if(shotIncoming&&!IsDodging(ally))
        {
            Vector3 side=Vector3.Cross(Vector3.up,(ahead-center).normalized);
            if(Vector3.Dot(position-center,side)>0) side=-side;
            direction=(direction+.8f*side).normalized;
            if(ally.rollCooldown<=0) StartRoll(ally,direction);
        }

        float lead=partnerRunning?ally.along-player.along:0f;
        if(lead<=BotLeadDistance-2f) botWaitTime=0;
        bool fogClose=elapsed>=FogStartTime&&ally.along-FogFront()<BotFogBuffer;
        if(lead>BotLeadDistance&&!fogClose&&underFire==null&&!shotIncoming&&!IsDodging(ally))
        {
            if(player.progressSpeed>.5f)
            {
                float pace=player.progressSpeed-(lead-BotLeadDistance)*.7f;
                direction*=Mathf.Clamp01(pace/RunnerSpeed(ally));
            }
            else if(botWaitTime<BotWaitLimit)
            {
                botWaitTime+=dt;
                direction=Vector3.zero;
            }
        }
        MoveRunner(ally,direction,dt);
    }

    Tower SharedTowerAimingAt(Runner runner)
    {
        Tower threat=null;
        foreach(var tower in towers)
        {
            if(!tower.discovered||tower.hp<=0||tower.disabled>0||tower.target!=runner||!tower.warning.enabled) continue;
            if(threat==null||tower.windup<threat.windup) threat=tower;
        }
        return threat;
    }

    bool IncomingSharedShot(Runner runner)
    {
        Vector3 center=runner.obj.transform.position+Vector3.up;
        foreach(var shot in shots)
        {
            Vector3 origin=shot.obj.transform.position;
            Vector3 relativeVelocity=shot.direction*ProjectileSpeed-runner.velocity;
            if(relativeVelocity.sqrMagnitude<.001f) continue;
            Vector3 offset=center-origin;
            float time=Vector3.Dot(offset,relativeVelocity)/relativeVelocity.sqrMagnitude;
            if(time<0||time>DodgeReactionTime||time*ProjectileSpeed>shot.remaining) continue;
            if((offset-relativeVelocity*time).sqrMagnitude<=1f
                &&HasLineOfSight(origin,origin+shot.direction*(ProjectileSpeed*time))) return true;
        }
        return false;
    }

    string SharedRunnerResult(Runner runner)
    {
        string format=runner.done
            ?RiftorynCodexData.Text("{0} · meta po {1:F1} s","{0} · finished in {1:F1} s")
            :RiftorynCodexData.Text("{0} · pokonany, najdalej {1:F0} m","{0} · eliminated, reached {1:F0} m");
        return RiftorynL10n.Format(format,HeroNames[runner.hero],runner.done?runner.finishTime:runner.bestProgress);
    }

    void RecordSharedScore()
    {
        int arrived=(player.done?1:0)+(ally.done?1:0);
        float lastArrival=Mathf.Max(player.done?player.finishTime:0,ally.done?ally.finishTime:0);
        sharedRank=ScoreStore.Record(new RiftorynSharedScores.Score
        {
            id=Guid.NewGuid().ToString("N"),
            playedAtUtc=DateTime.UtcNow.ToString("O",CultureInfo.InvariantCulture),
            playerHeroId=HeroNames[player.hero].ToLowerInvariant(),
            allyHeroId=HeroNames[ally.hero].ToLowerInvariant(),
            defenseVersion=SharedDefenseVersion,
            defenseSeed=defenseSeed,
            arrived=arrived,
            finishDeciseconds=Mathf.RoundToInt(lastArrival*10f),
            bestProgressDecimeters=Mathf.RoundToInt(Mathf.Max(player.bestProgress,ally.bestProgress)*10f)
        });
    }

    public void OpenSharedScores()
    {
        sharedScoresReadable=ScoreStore.TryLoad(out visibleScores);
        SharedScoresOpen=true;
    }

    void DrawSharedScores()
    {
        const float width=780f, height=560f;
        HudFill(new Rect(0,0,Screen.width,Screen.height),new Color(.045f,.035f,.07f,1f));
        Matrix4x4 matrix=GUI.matrix;
        float scale=Mathf.Min(Screen.width/width,Screen.height/height);
        GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-width*scale)*.5f,(Screen.height-height*scale)*.5f),
            Quaternion.identity,new Vector3(scale,scale,1f));
        GUI.Label(new Rect(30,22,720,35),RiftorynCodexData.Text("WSPÓLNA UCIECZKA · RANKING LOKALNY","SHARED ESCAPE · LOCAL LEADERBOARD"),title);
        GUI.Label(new Rect(32,65,716,42),
            RiftorynCodexData.Text("Najpierw liczba osób na mecie. Przy remisie liczy się czas ostatniej osoby na mecie lub najdalszy postęp, gdy nikt nie dotarł.",
                "Runners at the finish come first. Ties use the last finisher’s time, or the farthest progress if nobody finished."),small);
        if(!sharedScoresReadable||visibleScores.Count==0)
        {
            string status=sharedScoresReadable
                ?RiftorynCodexData.Text("Rozegraj pierwszą próbę, żeby zapisać wynik.","Play your first run to record a score.")
                :RiftorynCodexData.Text("Nie udało się odczytać rankingu.","Could not read the leaderboard.");
            GUI.Label(new Rect(32,126,716,40),status,heading);
        }
        else
        {
            GUI.Label(new Rect(32,114,42,24),"#",small);
            GUI.Label(new Rect(88,114,240,24),RiftorynCodexData.Text("DUET","TEAM"),small);
            GUI.Label(new Rect(350,114,75,24),RiftorynL10n.Translate("META"),small);
            GUI.Label(new Rect(442,114,135,24),RiftorynCodexData.Text("CZAS / POSTĘP","TIME / PROGRESS"),small);
            GUI.Label(new Rect(596,114,150,24),"SEED",small);
            for(int i=0;i<visibleScores.Count;i++)
            {
                var score=visibleScores[i];
                float y=140+i*30;
                if(i%2==0) HudFill(new Rect(24,y-3,732,29),new Color(.10f,.08f,.14f,1f));
                GUI.Label(new Rect(32,y,42,24),(i+1).ToString(),small);
                GUI.Label(new Rect(88,y,240,24),$"{ScoreHeroName(score.playerHeroId)} + {ScoreHeroName(score.allyHeroId)}",small);
                GUI.Label(new Rect(350,y,75,24),$"{score.arrived}/2",small);
                string metric=score.arrived>0?RiftorynL10n.Format("{0:F1} s",score.finishDeciseconds*.1f)
                    :RiftorynL10n.Format("{0:F1} m",score.bestProgressDecimeters*.1f);
                GUI.Label(new Rect(442,y,135,24),metric,small);
                GUI.Label(new Rect(596,y,150,24),score.defenseSeed.ToString(),small);
            }
        }
        bool close=GUI.Button(new Rect(270,483,240,44),RiftorynCodexData.Text("WRÓĆ","BACK"));
        GUI.matrix=matrix;
        if(close)
        {
            SharedScoresOpen=false;
            GUIUtility.ExitGUI();
        }
    }

    static string ScoreHeroName(string heroId)=>char.ToUpperInvariant(heroId[0])+heroId.Substring(1);

    void DrawSharedResult()
    {
        int arrived=(player.done?1:0)+(ally.done?1:0);
        string outcome=arrived==2?RiftorynCodexData.Text("PEŁNY SUKCES","FULL SUCCESS")
            :arrived==1?RiftorynCodexData.Text("WYNIK CZĘŚCIOWY","PARTIAL RESULT")
            :RiftorynCodexData.Text("PORAŻKA","DEFEAT");
        float width=Mathf.Min(580f,Screen.width-24f);
        Rect panel=new((Screen.width-width)*.5f,Screen.height*.5f-122f,width,244f);
        GUI.Box(panel,"");
        GUI.Label(new Rect(panel.x+18,panel.y+12,width-36,28),$"{outcome}  ·  {arrived}/2",heading);
        GUI.Label(new Rect(panel.x+18,panel.y+49,width-36,24),SharedRunnerResult(player),small);
        GUI.Label(new Rect(panel.x+18,panel.y+75,width-36,24),SharedRunnerResult(ally),small);
        GUI.Label(new Rect(panel.x+18,panel.y+106,width-36,24),
            $"{RiftorynCodexData.Text("Układ","Layout")} v{SharedDefenseVersion}  ·  seed {defenseSeed}",small);
        string rankText=sharedRank switch
        {
            null=>RiftorynCodexData.Text("Zapis rankingu niedostępny.","Leaderboard save unavailable."),
            0=>RiftorynCodexData.Text("Wynik poza pierwszą dziesiątką.","Result outside the top ten."),
            _=>RiftorynL10n.Format(RiftorynCodexData.Text("Miejsce w rankingu: {0}","Leaderboard position: {0}"),sharedRank.Value)
        };
        GUI.Label(new Rect(panel.x+18,panel.y+145,width-182,25),rankText,small);
        if(GUI.Button(new Rect(panel.x+width-154,panel.y+137,142,34),RiftorynCodexData.Text("RANKING","LEADERBOARD")))
        {
            OpenSharedScores();
            GUIUtility.ExitGUI();
        }
        float buttonWidth=(width-48f)/3;
        float y=panel.y+186f;
        if(GUI.Button(new Rect(panel.x+12,y,buttonWidth,43),RiftorynCodexData.Text("POWTÓRZ UKŁAD","RETRY LAYOUT")))
        {
            RestartSharedEscape(false);
            GUIUtility.ExitGUI();
        }
        if(GUI.Button(new Rect(panel.x+24+buttonWidth,y,buttonWidth,43),RiftorynCodexData.Text("NOWY UKŁAD","NEW LAYOUT")))
        {
            RestartSharedEscape(true);
            GUIUtility.ExitGUI();
        }
        if(RiftorynL10n.Button(new Rect(panel.x+36+buttonWidth*2,y,buttonWidth,43),"MENU GŁÓWNE"))
        {
            ReturnToMenu();
            GUIUtility.ExitGUI();
        }
    }
}
