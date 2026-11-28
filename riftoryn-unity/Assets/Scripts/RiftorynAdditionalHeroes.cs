using System.Collections.Generic;
using UnityEngine;

public partial class RiftorynTrial
{
    readonly Dictionary<int,SimHero> additionalHeroDesigns=new();

    SimHero AdditionalHeroDesign(int hero)
    {
        if(additionalHeroDesigns.TryGetValue(hero,out var design))return design;
        string id=hero switch {1=>"rian",4=>"niva",5=>"liora",ThoraHero=>"thora",BromHero=>"brom",MorrakHero=>"morrak",_=>throw new System.ArgumentOutOfRangeException(nameof(hero))};
        var asset=Resources.Load<TextAsset>("HeroDesign/"+id);
        if(!asset)throw new System.InvalidOperationException("Brak danych bohatera: "+id);
        design=JsonUtility.FromJson<SimHero>(asset.text);
        if(design?.umiejetnosci?.E?.parametry==null||design.umiejetnosci.R?.parametry==null)
            throw new System.InvalidOperationException("Niekompletne umiejętności bohatera: "+id);
        additionalHeroDesigns.Add(hero,design);return design;
    }

    (float secondary,float ultimate) NewHeroCooldowns(int hero)
    {
        var skills=AdditionalHeroDesign(hero).umiejetnosci;
        return (skills.E.odnowienie_s,skills.R.odnowienie_s);
    }

    Runner NearestAllyMatching(Runner runner,float range,System.Predicate<Runner> predicate)
    {
        Runner result=null;float closest=range*range;
        foreach(var candidate in AllRunners())
        {
            if(candidate==runner||!IsRunning(candidate)||!predicate(candidate))continue;
            float distance=(runner.obj.transform.position-candidate.obj.transform.position).sqrMagnitude;
            if(distance>closest)continue;
            if(result==null||distance<closest){result=candidate;closest=distance;}
        }
        return result;
    }

    bool UseNewHeroAbility(Runner runner,bool ultimate,Tower tower)
    {
        var skills=AdditionalHeroDesign(runner.hero).umiejetnosci;
        var p=(ultimate?skills.R:skills.E).parametry;
        if(runner.hero==1)
        {
            tower??=Nearest(runner,p.zasieg_m);
            if(tower==null||!tower.discovered||tower.hp<=0||!TowerHasClearShot(tower,runner.obj.transform.position+Vector3.up)
                ||Vector3.Distance(runner.obj.transform.position,Slot(tower.slot))>=p.zasieg_m)return false;
            if(ultimate){HitTower(tower,p.obrazenia);tower.disabled=Mathf.Max(tower.disabled,p.wylaczenie_s);}
            else ApplyTowerSlow(tower,p.czas_s,p.spowolnienie_ataku_procent);
            ShowRianHeat(runner.obj.transform.position+Vector3.up,tower.obj.transform.position+Vector3.up);
            return true;
        }
        if(!ultimate)
        {
            Runner target=NearestAllyMatching(runner,p.zasieg_m,r=>r.hp<MaxHp[r.hero]);
            if(target==null)return false;
            HealAdditionalHero(target,p.leczenie);
            return true;
        }
        if(runner.hero==4)
        {
            foreach(var target in AllRunners())
                if(IsRunning(target)&&(target==runner||RunnersNear(runner,target,p.promien_m)))
                {target.chillTime=0;target.slowImmunity=Mathf.Max(target.slowImmunity,p.czas_s);}
            return true;
        }
        if(runner.hero==5)
        {
            foreach(var target in AllRunners())
                if(IsRunning(target)&&(target==runner||RunnersNear(runner,target,p.promien_m)))
                {
                    HealAdditionalHero(target,p.leczenie_poczatkowe);
                    target.kit.regenerationRemaining=p.czas_s;
                    target.kit.regenerationPerSecond=p.leczenie_na_sekunde;
                }
            return true;
        }
        return false;
    }

    void ShowRianHeat(Vector3 from,Vector3 to)
    {
        var effect=new GameObject("Żar Rian");var line=effect.AddComponent<LineRenderer>();
        line.useWorldSpace=true;line.positionCount=2;line.SetPosition(0,from);line.SetPosition(1,to);
        line.startWidth=.14f;line.endWidth=.05f;line.material=new Material(Shader.Find("Sprites/Default"));
        line.startColor=new Color(1f,.34f,.08f);line.endColor=new Color(1f,.77f,.28f);
        Destroy(line.material,.2f);Destroy(effect,.2f);
    }

    void ApplyTowerSlow(Tower tower,float duration,float slowPercent)
    {
        float multiplier=1f-Mathf.Clamp01(slowPercent/100f);
        tower.slowRateMultiplier=tower.frostSlow>0?Mathf.Min(tower.slowRateMultiplier,multiplier):multiplier;
        tower.frostSlow=Mathf.Max(tower.frostSlow,duration);
    }

    void HealAdditionalHero(Runner target,float amount)
    {
        float restored=ApplyTeamHealing(target,amount);
        if(restored<=0)return;
        ShowCombatText(target,$"+{Mathf.CeilToInt(restored)} HP",new Color(.57f,.85f,.42f));
    }

    void StepAdditionalRegeneration(Runner runner,float dt)
    {
        if(runner.kit.regenerationRemaining<=0)return;
        float step=Mathf.Min(dt,runner.kit.regenerationRemaining);
        runner.kit.regenerationRemaining-=step;
        ApplyTeamHealing(runner,runner.kit.regenerationPerSecond*step);
    }
}
