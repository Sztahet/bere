using System.Collections.Generic;
using UnityEngine;

public partial class RiftorynTrial
{
    sealed class TankZone { public Vector3 center; public float radius,endsAt,damageMultiplier,projectileMultiplier,towerMultiplier=1f; public LineRenderer line; public Material material; }
    readonly List<TankZone> tankZones=new();

    bool UseTankBasic(Runner runner,Tower threat)
    {
        var skill=AdditionalHeroDesign(runner.hero).umiejetnosci.Q;var p=skill.parametry;
        if(runner.hero==ThoraHero)runner.kit.tankHealUntil=elapsed+p.czas_s;
        else if(runner.hero==BromHero)
        {
            if(!RaiseSharedBarrier(runner,threat,p.szerokosc_m,p.wysokosc_m,p.czas_s,Mathf.RoundToInt(p.maks_pociskow),new Color(.72f,.56f,.35f)))return false;
        }
        else if(runner.hero==MorrakHero)runner.kit.morrakSlowUntil=elapsed+p.czas_s;
        else return false;
        runner.cooldown=skill.odnowienie_s;runner.uses++;
        if(runner==player)Notify(SharedSkillInfo(runner.hero,0).name);
        return true;
    }

    bool UseTankAbility(Runner runner,bool ultimate)
    {
        var skills=AdditionalHeroDesign(runner.hero).umiejetnosci;var p=(ultimate?skills.R:skills.E).parametry;
        if(runner.hero==ThoraHero)
        {
            if(ultimate)CreateTankZone(runner,p,new Color(.38f,.78f,.34f));
            else {AddShield(runner,p.oslona);runner.kit.tankSlowResistUntil=elapsed+p.czas_s;}
        }
        else if(runner.hero==BromHero)
        {
            if(ultimate){foreach(var ally in AllRunners())if(IsRunning(ally)&&(ally==runner||RunnersNear(runner,ally,p.promien_m)))AddShield(ally,p.oslona);}
            else {runner.kit.tankSlowResistUntil=elapsed+p.czas_s;runner.kit.tankDamageResistUntil=elapsed+p.czas_s;}
        }
        else if(runner.hero==MorrakHero)
        {
            if(ultimate)CreateTankZone(runner,p,new Color(.59f,.37f,.84f));
            else {AddShield(runner,p.oslona_wlasna);var ally=NearestAllyMatching(runner,p.zasieg_m,_=>true);if(ally!=null)AddShield(ally,p.oslona_sojusznika);}
        }
        else return false;
        return true;
    }

    void CreateTankZone(Runner runner,SimParams p,Color color)
    {
        CreateTankZoneAt(runner.obj.transform.position,p,color);
    }
    void CreateTankZoneAt(Vector3 center,SimParams p,Color color)
    {
        var obj=new GameObject("Strefa ochronna");obj.transform.SetParent(world.transform);
        var line=obj.AddComponent<LineRenderer>();line.useWorldSpace=true;line.loop=true;line.positionCount=48;line.startWidth=line.endWidth=.09f;
        var material=new Material(Shader.Find("Sprites/Default"));line.sharedMaterial=material;line.startColor=line.endColor=color;
        for(int i=0;i<line.positionCount;i++){float angle=i*Mathf.PI*2/line.positionCount;line.SetPosition(i,center+new Vector3(Mathf.Cos(angle)*p.promien_m,.12f,Mathf.Sin(angle)*p.promien_m));}
        float damageReduction=p.redukcja_procent>0?p.redukcja_procent:p.redukcja_obrazen_procent;
        tankZones.Add(new TankZone{center=center,radius=p.promien_m,endsAt=elapsed+p.czas_s,
            damageMultiplier=1-Mathf.Clamp01(damageReduction/100f),projectileMultiplier=1-Mathf.Clamp01(p.spowolnienie_pociskow_procent/100f),
            towerMultiplier=1-Mathf.Clamp01(p.spowolnienie_ataku_procent/100f),line=line,material=material});
    }
    void DestroyTankZone(TankZone zone){if(zone.line)Destroy(zone.line.gameObject);if(zone.material)Destroy(zone.material);}
    void ClearTankZones(){foreach(var zone in tankZones)DestroyTankZone(zone);tankZones.Clear();}
    void StepTankZones(){for(int i=tankZones.Count-1;i>=0;i--)if(tankZones[i].endsAt<=elapsed){DestroyTankZone(tankZones[i]);tankZones.RemoveAt(i);}}
    float TowerSlowMultiplier(Tower tower)
    {
        float multiplier=tower.frostSlow>0?tower.slowRateMultiplier:1f;
        foreach(var zone in tankZones)if(tower.discovered&&zone.endsAt>elapsed&&(Slot(tower.slot)-zone.center).sqrMagnitude<=zone.radius*zone.radius)
            multiplier=Mathf.Min(multiplier,zone.towerMultiplier);
        return multiplier;
    }
    float ZoneDamageMultiplier(Runner runner)
    {
        float multiplier=1;
        foreach(var zone in tankZones)if(zone.endsAt>elapsed&&(runner.obj.transform.position-zone.center).sqrMagnitude<=zone.radius*zone.radius)
            multiplier=Mathf.Min(multiplier,zone.damageMultiplier);
        return multiplier;
    }
    float ShotSpeedMultiplier(Vector3 position)
    {
        float multiplier=1;
        foreach(var zone in tankZones)if(zone.endsAt>elapsed&&(position-zone.center).sqrMagnitude<=zone.radius*zone.radius)multiplier=Mathf.Min(multiplier,zone.projectileMultiplier);
        foreach(var runner in AllRunners())if(IsRunning(runner)&&runner.hero==MorrakHero&&runner.kit.morrakSlowUntil>elapsed)
        {
            var p=AdditionalHeroDesign(MorrakHero).umiejetnosci.Q.parametry;
            if((position-runner.obj.transform.position).sqrMagnitude<=p.promien_m*p.promien_m)
                multiplier=Mathf.Min(multiplier,1-Mathf.Clamp01(p.spowolnienie_pociskow_procent/100f));
        }
        return multiplier;
    }
    void StepTankSupport(Runner runner,float dt)
    {
        if(runner.hero!=ThoraHero||runner.kit.tankHealUntil<=elapsed)return;
        var p=AdditionalHeroDesign(ThoraHero).umiejetnosci.Q.parametry;
        float activeTime=Mathf.Min(dt,runner.kit.tankHealUntil-elapsed);
        foreach(var ally in AllRunners())if(ally!=runner&&RunnersNear(runner,ally,p.promien_m))ApplyTeamHealing(ally,p.leczenie_na_sekunde*activeTime);
    }
    float ApplySlowResistance(Runner runner,float factor)=>runner.kit.tankSlowResistUntil>elapsed?1-(1-factor)*.5f:factor;
    float ApplyTeamHealing(Runner target,float amount)
    {
        if(!IsRunning(target)||amount<=0)return 0;
        if(UsesUnifiedRules&&target.hero==ThoraHero)amount*=1.15f;
        float restored=Mathf.Min(amount,MaxHp[target.hero]-target.hp);target.hp+=restored;return restored;
    }
}
