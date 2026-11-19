using System.Collections.Generic;
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

    void DrawSharedResult()
    {
        int arrived=(player.done?1:0)+(ally.done?1:0);
        string outcome=arrived==2?RiftorynCodexData.Text("PEŁNY SUKCES","FULL SUCCESS")
            :arrived==1?RiftorynCodexData.Text("WYNIK CZĘŚCIOWY","PARTIAL RESULT")
            :RiftorynCodexData.Text("PORAŻKA","DEFEAT");
        float width=Mathf.Min(580f,Screen.width-24f);
        Rect panel=new((Screen.width-width)*.5f,Screen.height*.5f-105f,width,210f);
        GUI.Box(panel,"");
        GUI.Label(new Rect(panel.x+18,panel.y+12,width-36,28),$"{outcome}  ·  {arrived}/2",heading);
        GUI.Label(new Rect(panel.x+18,panel.y+49,width-36,24),SharedRunnerResult(player),small);
        GUI.Label(new Rect(panel.x+18,panel.y+75,width-36,24),SharedRunnerResult(ally),small);
        GUI.Label(new Rect(panel.x+18,panel.y+106,width-36,24),
            $"{RiftorynCodexData.Text("Układ","Layout")} v{SharedDefenseVersion}  ·  seed {defenseSeed}",small);
        float buttonWidth=(width-48f)/3;
        float y=panel.y+153f;
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
