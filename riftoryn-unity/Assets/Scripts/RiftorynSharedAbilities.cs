using UnityEngine;

public partial class RiftorynTrial
{
    const int KaelHero=0, ZetHero=2, KonevaHero=3;
    const float SupportRange=8f, PatkaGuardRange=10f, ScoutRange=85f;
    const float BarrierWidth=4f, BarrierHeight=3f;
    const float ReserveThreshold=.30f, ReserveShield=60f;

    sealed class SharedAbilityState
    {
        public float secondaryReadyAt, ultimateReadyAt, wardUntil, reserveUntil, surgeUntil, dodgeBoostUntil;
        public float recallUntil, guardUntil, mikoBoostUntil, mikoSpeedBonus, regenerationRemaining, regenerationPerSecond;
        public float tankHealUntil,tankSlowResistUntil,tankDamageResistUntil,morrakSlowUntil;
        public float summonGuardUntil,lumenAuraUntil,fireWardUntil,fireWardReadyAt;
        public int summonGuardCharges;
        public Runner summonGuardTarget;
        public bool rewardedRoll;
        public Runner guardedPartner;
    }

    sealed class HeatBarrier
    {
        public LineRenderer outline;
        public Material material;
        public Vector3 center, normal, side;
        public float endsAt,width,height;
        public int charges;
    }

    readonly System.Collections.Generic.List<HeatBarrier> heatBarriers=new();

    Runner PartnerOf(Runner runner)
    {
        if(!MatchMode)return runner==player?ally:player;
        if(!IsRunning(runner))return null;
        Runner nearest=null;float best=float.PositiveInfinity;
        foreach(var candidate in runners)
        {
            if(candidate==runner||!IsRunning(candidate))continue;
            float distance=(candidate.obj.transform.position-runner.obj.transform.position).sqrMagnitude;
            if(distance<best){best=distance;nearest=candidate;}
        }
        return nearest;
    }

    bool RunnersNear(Runner first,Runner second,float range)=>
        IsRunning(first)&&IsRunning(second)
        &&(first.obj.transform.position-second.obj.transform.position).sqrMagnitude<=range*range;

    (float secondary,float ultimate) SharedAbilityCooldowns(int hero)=>hero switch
    {
        KaelHero=>(16f,36f),
        NeraHero=>(14f,32f),
        ZetHero=>(16f,26f),
        KonevaHero=>(18f,30f),
        MikoHero=>(12f,32f),
        SarnaHero=>(16f,32f),
        SailaHero=>NewHeroCooldowns(hero),
        1 or 4 or 5=>NewHeroCooldowns(hero),
        ThoraHero or BromHero or MorrakHero=>NewHeroCooldowns(hero),
        PyrraHero or AnaHero or LumenHero=>NewHeroCooldowns(hero),
        _=>(0f,0f)
    };

    float SharedCooldownLeft(Runner runner,bool ultimate)=>
        Mathf.Max(0,(ultimate?runner.kit.ultimateReadyAt:runner.kit.secondaryReadyAt)-elapsed);

    bool TryUseSharedAbility(Runner runner,bool ultimate,Tower tower=null)
    {
        if(!UsesUnifiedRules||!IsSharedHero(runner.hero)||!IsRunning(runner)||SharedCooldownLeft(runner,ultimate)>0) return false;
        Runner partner=PartnerOf(runner);
        bool cast=runner.hero switch
        {
            KaelHero=>ultimate?RaiseHeatBarrier(runner,tower):DivertSharedShot(runner,tower),
            NeraHero=>ProtectSharedPartner(runner,partner,ultimate),
            ZetHero=>ultimate?StartSharedSurge(runner):ScoutSharedTower(runner,partner),
            KonevaHero=>CommandPatka(runner,partner,ultimate),
            MikoHero=>MikoExtra(runner,ultimate,tower),
            SarnaHero=>SupportSarnaTeam(runner,partner,ultimate),
            SailaHero=>UseSailaExtra(runner,ultimate,tower),
            1 or 4 or 5=>UseNewHeroAbility(runner,ultimate,tower),
            ThoraHero or BromHero or MorrakHero=>UseTankAbility(runner,ultimate),
            PyrraHero or AnaHero or LumenHero=>UseSummonerExtra(runner,ultimate,tower),
            _=>false
        };
        if(!cast) return false;
        var cooldowns=SharedAbilityCooldowns(runner.hero);
        if(ultimate) runner.kit.ultimateReadyAt=elapsed+cooldowns.ultimate;
        else runner.kit.secondaryReadyAt=elapsed+cooldowns.secondary;
        runner.uses++;
        var info=SharedSkillInfo(runner.hero,ultimate?2:1);
        if(runner==player) Notify(info.name);
        else ShowCombatText(runner,info.name,new Color(.50f,.88f,1f));
        return true;
    }

    bool UseSailaExtra(Runner runner,bool ultimate,Tower tower)
    {
        var skills=AdditionalHeroDesign(SailaHero).umiejetnosci;
        var p=(ultimate?skills.R:skills.E).parametry;
        if(ultimate)
        {
            bool affected=false;
            foreach(var candidate in towers)
            {
                if(candidate.hp<=0||!candidate.discovered||Vector3.Distance(runner.obj.transform.position,Slot(candidate.slot))>p.promien_m
                    ||!TowerHasClearShot(candidate,runner.obj.transform.position+Vector3.up))continue;
                HitTower(candidate,p.obrazenia);candidate.disabled=Mathf.Max(candidate.disabled,p.wylaczenie_s);
                ShowSailaFreeze(candidate);affected=true;
            }
            return affected;
        }
        tower??=Nearest(runner,p.zasieg_m);
        if(tower==null||!tower.discovered||tower.hp<=0||!TowerHasClearShot(tower,runner.obj.transform.position+Vector3.up))return false;
        if(Vector3.Distance(runner.obj.transform.position,Slot(tower.slot))>=p.zasieg_m)return false;
        ApplyTowerSlow(tower,p.czas_s,p.spowolnienie_ataku_procent);ShowSailaFreeze(tower);return true;
    }

    void BoostSarnaTeam(Runner runner)
    {
        if(!IsRunning(runner)) return;
        runner.boost=Mathf.Max(runner.boost,3f);
        Runner partner=PartnerOf(runner);
        if(RunnersNear(runner,partner,SupportRange)) partner.boost=Mathf.Max(partner.boost,3f);
        runner.cooldown=HeroBasicCooldown(SarnaHero);
        runner.uses++;
        if(runner==player) Notify(SharedSkillInfo(SarnaHero,0).name);
    }

    bool SupportSarnaTeam(Runner runner,Runner partner,bool ultimate)
    {
        bool near=RunnersNear(runner,partner,SupportRange);
        if(!ultimate)
        {
            if(!near||partner.hp>=HeroMaxHp(partner.hero)) return false;
            float restored=ApplyTeamHealing(partner,25f);
            ShowCombatText(partner,$"+{Mathf.CeilToInt(restored)} HP",new Color(.57f,.85f,.42f));
            return true;
        }

        ClearSarnaSlows(runner);
        if(near) ClearSarnaSlows(partner);
        return true;
    }

    static void ClearSarnaSlows(Runner runner)
    {
        runner.chillTime=0;
        runner.rootTime=0;
        runner.slowImmunity=Mathf.Max(runner.slowImmunity,3f);
    }

    void SabotageMiko(Runner runner,Tower tower)
    {
        int result=Random.Range(0,4);
        string message;
        if(result==0){tower.disabled=Mathf.Max(tower.disabled,5f);message=RiftorynCodexData.Text("Wieża zatrzymana na 5 s","Tower disabled for 5 s");}
        else if(result==1){HitTower(tower,tower.hp);message=RiftorynCodexData.Text("Wieża wyłączona","Tower disabled");}
        else if(result==2){tower.misfires=true;message=RiftorynCodexData.Text("Wieża może pudłować","Tower may misfire");}
        else{tower.overload=5f;message=RiftorynCodexData.Text("Pech! Wieża przyspiesza","Bad luck! Tower fires faster");}
        runner.cooldown=HeroBasicCooldown(MikoHero);runner.uses++;
        LightningArc(runner.obj.transform.position+Vector3.up,tower.obj.transform.position+Vector3.up*2f);
        if(runner==player) Notify(message); else ShowCombatText(runner,message,new Color(.7f,.85f,1f));
    }

    bool MikoExtra(Runner runner,bool ultimate,Tower tower)
    {
        if(ultimate)
        {
            bool hit=false;
            foreach(var candidate in towers)
            {
                if(candidate.hp<=0||UsesUnifiedRules&&!candidate.discovered) continue;
                if(Vector3.Distance(runner.obj.transform.position,Slot(candidate.slot))>9f||!TowerHasClearShot(candidate,runner.obj.transform.position+Vector3.up)) continue;
                candidate.disabled=Mathf.Max(candidate.disabled,2.5f);hit=true;
                LightningArc(runner.obj.transform.position+Vector3.up,candidate.obj.transform.position+Vector3.up*2f);
            }
            return hit;
        }
        tower??=Nearest(runner,5f);
        if(tower==null||Vector3.Distance(runner.obj.transform.position,Slot(tower.slot))>=5f) return false;
        HitTower(tower,20f);
        LightningArc(runner.obj.transform.position+Vector3.up,tower.obj.transform.position+Vector3.up*2f);
        return true;
    }

    void UseSharedPlayerAbility(bool ultimate)
    {
        if(SharedCooldownLeft(player,ultimate)>0) return;
        if(!TryUseSharedAbility(player,ultimate))
            Notify(RiftorynCodexData.Text("Brak warunków do użycia zdolności.","Ability conditions not met."));
    }

    bool CanDivertTo(Tower tower,Runner runner)=>
        tower!=null&&tower.discovered&&tower.hp>0&&tower.disabled<=0&&IsRunning(runner)
        &&(runner.obj.transform.position-Slot(tower.slot)).sqrMagnitude<TowerRange[tower.type]*TowerRange[tower.type]
        &&TowerHasClearShot(tower,runner.obj.transform.position+Vector3.up);

    bool DivertSharedShot(Runner runner,Tower tower)
    {
        if(tower==null)
        {
            float best=.88f, nearest=float.PositiveInfinity;
            Tower facing=null, fallback=null;
            foreach(var candidate in towers)
            {
                if(!CanDivertTo(candidate,runner)||candidate.target==runner) continue;
                Vector3 offset=candidate.obj.transform.position+Vector3.up*2f-cam.transform.position;
                float aim=Vector3.Dot(cam.transform.forward,offset.normalized);
                if(aim>best){best=aim;facing=candidate;}
                float distance=(Slot(candidate.slot)-runner.obj.transform.position).sqrMagnitude;
                if(distance<nearest){nearest=distance;fallback=candidate;}
            }
            tower=facing??fallback;
        }
        if(!CanDivertTo(tower,runner)||tower.target==runner) return false;
        tower.divertedTo=runner;
        tower.divertUntil=elapsed+4f;
        return true;
    }

    void RetargetSharedTower(Tower tower)
    {
        Runner runner=tower.divertedTo;
        if(runner==null) return;
        if(tower.divertUntil<=elapsed||!IsRunning(runner))
        {
            tower.divertedTo=null;
            return;
        }
        if(tower.target==runner||!CanDivertTo(tower,runner)) return;
        if(tower.target==null&&tower.petTarget==null&&tower.wait>0) return;
        tower.target=runner;
        tower.petTarget=null;
        if(tower.windup<=0) tower.windup=TowerWindup;
    }

    bool RaiseHeatBarrier(Runner runner,Tower threat)
    {
        return RaiseSharedBarrier(runner,threat,BarrierWidth,BarrierHeight,4f,3,new Color(1f,.56f,.16f));
    }

    bool RaiseSharedBarrier(Runner runner,Tower threat,float width,float height,float duration,int charges,Color color)
    {
        threat??=Nearest(runner,TowerSkillRange);
        Vector3 origin=runner.obj.transform.position;
        Vector3 normal=threat!=null?Slot(threat.slot)-origin:runner.obj.transform.forward;
        normal.y=0;
        if(normal.sqrMagnitude<.01f) return false;
        normal.Normalize();
        Vector3 side=Vector3.Cross(Vector3.up,normal);
        Vector3 center=origin+normal*1.6f+Vector3.up*(height*.5f);
        var outline=new GameObject("Kurtyna żaru").AddComponent<LineRenderer>();
        outline.transform.SetParent(world.transform);
        outline.useWorldSpace=true;
        outline.loop=true;
        outline.positionCount=4;
        outline.startWidth=outline.endWidth=.13f;
        var material=new Material(Shader.Find("Sprites/Default"));
        outline.sharedMaterial=material;
        outline.startColor=outline.endColor=color;
        Vector3 halfWidth=side*(width*.5f), halfHeight=Vector3.up*(height*.5f);
        outline.SetPositions(new[]{center-halfWidth-halfHeight,center+halfWidth-halfHeight,
            center+halfWidth+halfHeight,center-halfWidth+halfHeight});
        heatBarriers.Add(new HeatBarrier{outline=outline,material=material,center=center,normal=normal,side=side,endsAt=elapsed+duration,charges=charges,width=width,height=height});
        return true;
    }

    void ClearHeatBarrier()
    {
        foreach(var barrier in heatBarriers)DestroySharedBarrier(barrier);
        heatBarriers.Clear();ClearTankZones();
    }

    void DestroySharedBarrier(HeatBarrier barrier){if(barrier.outline)Destroy(barrier.outline.gameObject);if(barrier.material)Destroy(barrier.material);}

    bool HeatBarrierBlocks(Vector3 start,Vector3 segment,float firstHit,out Vector3 impact)
    {
        impact=start;
        float length=segment.magnitude;
        if(length<=0) return false;
        var ray=new Ray(start,segment/length);
        HeatBarrier nearest=null;float nearestDistance=length*Mathf.Min(1f,firstHit);
        foreach(var barrier in heatBarriers){var plane=new Plane(barrier.normal,barrier.center);if(!plane.Raycast(ray,out float distance)||distance>nearestDistance)continue;
            Vector3 offset=ray.GetPoint(distance)-barrier.center;if(Mathf.Abs(Vector3.Dot(offset,barrier.side))>barrier.width*.5f||Mathf.Abs(offset.y)>barrier.height*.5f)continue;
            nearest=barrier;nearestDistance=distance;}
        if(nearest==null)return false;
        impact=ray.GetPoint(nearestDistance);
        if(--nearest.charges==0){DestroySharedBarrier(nearest);heatBarriers.Remove(nearest);}
        return true;
    }

    bool ProtectSharedPartner(Runner runner,Runner partner,bool reserve)
    {
        if(!RunnersNear(runner,partner,SupportRange)) return false;
        if(reserve)
        {
            if(partner.hp<=HeroMaxHp(partner.hero)*ReserveThreshold) AddShield(partner,ReserveShield);
            else partner.kit.reserveUntil=elapsed+6f;
        }
        else partner.kit.wardUntil=elapsed+5f;
        LightningArc(runner.obj.transform.position+Vector3.up,partner.obj.transform.position+Vector3.up);
        return true;
    }

    void TryTriggerSharedReserve(Runner runner,float damage)
    {
        if(runner.kit.reserveUntil<=elapsed||runner.hp-Mathf.Max(0,damage-runner.shield)>HeroMaxHp(runner.hero)*ReserveThreshold)
            return;
        runner.kit.reserveUntil=0;
        AddShield(runner,ReserveShield);
        ShowCombatText(runner,RiftorynCodexData.Text("REZERWA","RESERVE"),new Color(.46f,.8f,1f));
    }

    float ShieldDecay(Runner runner)
    {
        Runner partner=PartnerOf(runner);
        return UsesUnifiedRules&&partner?.hero==NeraHero&&RunnersNear(runner,partner,SupportRange) ? .94f : .90f;
    }

    bool ConsumeSharedWard(Runner runner,int type)
    {
        if(!UsesUnifiedRules||runner.kit.wardUntil<=elapsed||type!=0&&type!=1&&type!=3) return false;
        runner.kit.wardUntil=0;
        ShowCombatText(runner,RiftorynCodexData.Text("UZIEMIENIE","GROUNDED"),new Color(.48f,.82f,1f));
        return true;
    }

    bool ScoutSharedTower(Runner runner,Runner partner)=>RevealSharedTowerAhead(runner,partner,ScoutRange,6f);
    bool RevealSharedTowerAhead(Runner runner,Runner partner,float range,float displayTime)
    {
        float progress=IsRunning(partner)?Mathf.Max(runner.along,partner.along):runner.along;
        Tower next=null;
        float best=range;
        foreach(var tower in towers)
        {
            if(tower.discovered||tower.hp<=0) continue;
            float ahead=Project(Slot(tower.slot),out _)-progress;
            if(ahead<0||ahead>=best) continue;
            next=tower;
            best=ahead;
        }
        if(next==null) return false;
        next.discovered=true;
        next.obj.SetActive(true);
        next.rangeVisibleUntil=elapsed+displayTime;
        next.range.enabled=true;
        return true;
    }

    bool StartSharedSurge(Runner runner)
    {
        if(IsDodging(runner)) return false;
        runner.kit.surgeUntil=elapsed+1.2f;
        return true;
    }

    void MarkSharedDodge(Runner runner)
    {
        if(UsesUnifiedRules&&runner.hero==KonevaHero&&!runner.kit.rewardedRoll)
        {
            runner.kit.rewardedRoll=true;
            runner.kit.dodgeBoostUntil=elapsed+2f;
        }
        ShowCombatText(runner,"DODGE",Color.white);
    }

    bool CommandPatka(Runner runner,Runner partner,bool guard)
    {
        var pet=runner.obj.GetComponentInChildren<RiftorynPatkaPreview>();
        if(!pet||!pet.IsAlive) return false;
        if(guard)
        {
            if(!RunnersNear(runner,partner,PatkaGuardRange)) return false;
            runner.kit.recallUntil=0;
            runner.kit.guardUntil=elapsed+5f;
            runner.kit.guardedPartner=partner;
            pet.Guard(partner.obj.transform);
        }
        else
        {
            if(!pet.IsHurt&&!pet.IsFlying&&runner.kit.guardUntil<=elapsed) return false;
            runner.kit.guardUntil=0;
            runner.kit.guardedPartner=null;
            runner.kit.recallUntil=elapsed+5f;
            pet.ReturnToOwner();
        }
        return true;
    }

    void StepSharedAbilityEffects(float dt)
    {
        for(int i=heatBarriers.Count-1;i>=0;i--)if(heatBarriers[i].endsAt<=elapsed){DestroySharedBarrier(heatBarriers[i]);heatBarriers.RemoveAt(i);}
        StepTankZones();
        StepSummonerGuards();
        Runner owner=null;
        foreach(var runner in runners)if(runner!=null&&runner.hero==KonevaHero){owner=runner;break;}
        if(!IsRunning(owner)||owner.kit.guardUntil<=0&&owner.kit.recallUntil<=elapsed) return;
        var pet=owner.obj.GetComponentInChildren<RiftorynPatkaPreview>();
        if(!pet||!pet.IsAlive)
        {
            owner.kit.guardUntil=0;
            owner.kit.guardedPartner=null;
            owner.kit.recallUntil=0;
            return;
        }
        if(owner.kit.guardUntil>0&&(owner.kit.guardUntil<=elapsed||!IsRunning(owner.kit.guardedPartner)))
        {
            owner.kit.guardUntil=0;
            owner.kit.guardedPartner=null;
            pet.ReturnToOwner();
        }
        if(owner.kit.recallUntil>elapsed&&SharedTowerAimingAt(owner)==null&&!IncomingSharedShot(owner)&&!RunnerInFog(owner)
            &&(pet.AimPosition-owner.obj.transform.position).sqrMagnitude<12.25f)
            pet.Heal(6f*dt);
    }

    Runner PatkaGuardOf(Runner victim)
    {
        foreach(var runner in runners)
            if(IsRunning(runner)&&runner.hero==KonevaHero&&runner.kit.guardedPartner==victim&&runner.kit.guardUntil>elapsed)
                return runner;
        return null;
    }

    bool PatkaGuardsShot(Runner victim,Shot shot)
    {
        if(IsDodging(victim)) return false;
        Runner owner=PatkaGuardOf(victim);
        if(!IsRunning(owner)||owner.hero!=KonevaHero||owner.kit.guardUntil<=elapsed) return false;
        var pet=owner.obj.GetComponentInChildren<RiftorynPatkaPreview>();
        Vector3 target=victim.obj.transform.position+Vector3.up;
        if(!pet||!pet.IsAlive||(pet.AimPosition-target).sqrMagnitude>9f||!HasLineOfSight(pet.AimPosition,target)) return false;
        pet.ReceiveHit(shot.damage);
        FinishPatkaGuard(owner,pet);
        return true;
    }

    void FinishPatkaGuard(Runner owner,RiftorynPatkaPreview pet)
    {
        owner.kit.guardUntil=0;
        owner.kit.guardedPartner=null;
        pet.ReturnToOwner();
    }

    bool TrySharedBotExtra(Runner runner,Runner partner,Tower threat,Tower partnerThreat,bool shotIncoming,bool partnerIncoming,bool fogClose)
    {
        switch(runner.hero)
        {
            case KaelHero:
                if(partnerThreat!=null&&runner.shield>20f&&TryUseSharedAbility(runner,false,partnerThreat)) return true;
                Tower imminent=threat??partnerThreat;
                return (shotIncoming||partnerIncoming||imminent!=null&&imminent.windup<=.18f)
                    &&TryUseSharedAbility(runner,true,imminent);
            case NeraHero:
                if((partnerThreat!=null||partnerIncoming)&&partner.hp<HeroMaxHp(partner.hero)*.48f&&TryUseSharedAbility(runner,true)) return true;
                return partnerThreat!=null&&(partnerThreat.type==0||partnerThreat.type==1)&&TryUseSharedAbility(runner,false);
            case ZetHero:
                if((threat!=null||fogClose)&&!shotIncoming&&(!IsRunning(partner)||runner.along<=partner.along+2f)
                    &&(threat==null||runner.cooldown>0)&&TryUseSharedAbility(runner,true)) return true;
                return threat==null&&partnerThreat==null&&!shotIncoming&&TryUseSharedAbility(runner,false);
            case KonevaHero:
                var pet=runner.obj.GetComponentInChildren<RiftorynPatkaPreview>();
                if(!pet||!pet.IsAlive) return false;
                if(partnerThreat!=null&&!pet.IsFlying&&(runner.cooldown>0||partner.hp<HeroMaxHp(partner.hero)*.45f)
                    &&TryUseSharedAbility(runner,true)) return true;
                return threat==null&&partnerThreat==null&&!shotIncoming&&pet.IsHurt&&!pet.IsFlying&&runner.kit.guardUntil<=elapsed
                    &&TryUseSharedAbility(runner,false);
            case MikoHero:
                if((threat!=null||partnerThreat!=null)&&TryUseSharedAbility(runner,true)) return true;
                return threat!=null&&TryUseSharedAbility(runner,false,threat);
            case SarnaHero:
                if((runner.chillTime>0||runner.rootTime>0
                    ||RunnersNear(runner,partner,SupportRange)&&(partner.chillTime>0||partner.rootTime>0))
                    &&TryUseSharedAbility(runner,true)) return true;
                return IsRunning(partner)&&partner.hp<=HeroMaxHp(partner.hero)-20f&&TryUseSharedAbility(runner,false);
            case SailaHero:
                if(threat!=null&&TryUseSharedAbility(runner,true))return true;
                return threat!=null&&threat.frostSlow<=0&&TryUseSharedAbility(runner,false,threat);
            case 1:
                if(threat!=null&&runner.cooldown>0&&TryUseSharedAbility(runner,true,threat))return true;
                return threat!=null&&threat.frostSlow<=0&&TryUseSharedAbility(runner,false,threat);
            case 4:
                if((runner.chillTime>0||IsRunning(partner)&&partner.chillTime>0)&&TryUseSharedAbility(runner,true))return true;
                return TryUseSharedAbility(runner,false);
            case 5:
                if(IsRunning(partner)&&partner.hp<HeroMaxHp(partner.hero)*.55f&&TryUseSharedAbility(runner,true))return true;
                return TryUseSharedAbility(runner,false);
            case ThoraHero:
            case BromHero:
            case MorrakHero:
                if((shotIncoming||partnerIncoming||partnerThreat!=null)&&TryUseSharedAbility(runner,true))return true;
                return (threat!=null||shotIncoming||partnerThreat!=null)&&TryUseSharedAbility(runner,false);
            case PyrraHero:
                if(partnerIncoming&&TryUseSharedAbility(runner,true))return true;
                return threat!=null&&TryUseSharedAbility(runner,false);
            case AnaHero:
                var companion=SummonedPet(runner);
                if(companion&&companion.IsAlive&&companion.CurrentHp<companion.MaximumHp&&runner.cooldown<=0){Skill(runner);if(runner.cooldown>0)return true;}
                if((shotIncoming||partnerIncoming)&&TryUseSharedAbility(runner,true))return true;
                return threat!=null&&TryUseSharedAbility(runner,false,threat);
            case LumenHero:
                if((shotIncoming||partnerIncoming||threat!=null)&&TryUseSharedAbility(runner,true))return true;
                return threat==null&&TryUseSharedAbility(runner,false);
            default:return false;
        }
    }

    static (string name,string description) SkillText(string name,string englishName,string description,string englishDescription)=>
        (RiftorynCodexData.Text(name,englishName),RiftorynCodexData.Text(description,englishDescription));

    (string name,string description) SailaSkillInfo(int slot)
    {
        var design=AdditionalHeroDesign(SailaHero);var skills=design.umiejetnosci;
        if(slot==0){var p=skills.Q.parametry;return SkillText("Zamrożenie mechanizmu","Freeze Mechanism",$"Wyłącza odkrytą wieżę w zasięgu {p.zasieg_m:0.#} m na {p.wylaczenie_s:0.#} s.",$"Disables a discovered tower within {p.zasieg_m:0.#} m for {p.wylaczenie_s:0.#} s.");}
        if(slot==1){var p=skills.E.parametry;return SkillText("Szron na lufie","Frosted Barrel",$"Spowalnia ostrzał odkrytej wieży o {p.spowolnienie_ataku_procent:0.#}% przez {p.czas_s:0.#} s.",$"Slows a discovered tower's fire rate by {p.spowolnienie_ataku_procent:0.#}% for {p.czas_s:0.#} s.");}
        if(slot==2){var p=skills.R.parametry;return SkillText("Pęknięcie lodu","Ice Fracture",$"Zadaje {p.obrazenia:0.#} obrażeń i wyłącza odkryte wieże w promieniu {p.promien_m:0.#} m na {p.wylaczenie_s:0.#} s.",$"Deals {p.obrazenia:0.#} damage and disables discovered towers within {p.promien_m:0.#} m for {p.wylaczenie_s:0.#} s.");}
        var passive=design.pasywna.parametry;return SkillText("Zimny krok","Cold Step",$"Spowolnienia działające na Sailę są o {passive.redukcja_spowolnienia_procent:0.#}% słabsze.",$"Slows affecting Saila are {passive.redukcja_spowolnienia_procent:0.#}% weaker.");
    }

    (string name,string description) SharedSkillInfo(int hero,int slot)=>hero==SailaHero?SailaSkillInfo(slot):(hero,slot) switch
    {
        (KaelHero,0)=>SkillText("Żarowa bariera","Ember Barrier","Daje 100 osłony Kael i 50 pobliskiemu partnerowi.","Gives Kael 100 shield and a nearby ally 50 shield."),
        (KaelHero,1)=>SkillText("Przejęcie ostrzału","Draw Fire","Skieruj kamerę na wieżę w zasięgu. Jej następny strzał celuje w Kael.","Face a tower in range. Its next shot targets Kael."),
        (KaelHero,2)=>SkillText("Kurtyna żaru","Ember Curtain","Kurtyna stoi 4 s i zatrzymuje do trzech pocisków. Przepuszcza Runnerów.","A curtain lasts 4 s and stops up to three shots. Runners can pass through."),
        (KaelHero,3)=>SkillText("Hartowanie","Tempering","Podpalenie i trucizna zadają Kael o 20% mniej obrażeń.","Burn and poison deal 20% less damage to Kael."),
        (NeraHero,0)=>SkillText("Iskra wsparcia","Helping Spark","Daje Nerze 35 osłony i pobliskiemu partnerowi 65.","Gives Nera 35 shield and a nearby ally 65 shield."),
        (NeraHero,1)=>SkillText("Uziemienie","Grounding","Przez 5 s blokuje pierwszy status z pocisku, który trafi partnera w zasięgu przy użyciu.","For 5 s, prevents the first projectile status on the ally in range when cast."),
        (NeraHero,2)=>SkillText("Awaryjna rezerwa","Emergency Reserve","Przez 6 s może dać partnerowi 60 osłony, gdy trafienie sprowadziłoby go do 30% HP.","For 6 s, grants the ally 60 shield if a hit would bring them to 30% HP."),
        (NeraHero,3)=>SkillText("Podtrzymanie","Sustained Shield","Osłona partnera w promieniu 8 m zanika wolniej.","An ally within 8 m loses shield more slowly."),
        (ZetHero,0)=>SkillText("Elektroimpuls","Electropulse","Wyłącza odkrytą, widoczną wieżę na 2 s.","Disables a discovered tower in sight for 2 s."),
        (ZetHero,1)=>SkillText("Odczyt obwodu","Circuit Scan","Odkrywa najbliższą ukrytą wieżę przed duetem wzdłuż trasy i pokazuje jej zasięg na 6 s.","Reveals the nearest hidden tower ahead on the route and shows its range for 6 s."),
        (ZetHero,2)=>SkillText("Przeskok","Surge","Daje krótki zryw w kierunku ruchu. Ściany i trafienia działają podczas zrywu.","Surge briefly in your movement direction. Walls and hits still affect you."),
        (ZetHero,3)=>SkillText("Iskra manewru","Maneuver Spark","Po użyciu Q Zet zyskuje 15% szybkości na 2 s.","After Q, Zet gains 15% movement speed for 2 s."),
        (KonevaHero,0)=>SkillText("Manewr Patki","Patsy’s Maneuver","Patka przez 4 s odciąga ostrzał i ma 75% szansy uniknąć trafienia.","Patsy draws fire for 4 s and has a 75% chance to dodge hits."),
        (KonevaHero,1)=>SkillText("Powrót Patki","Recall Patsy","Odwołuje Patkę. Blisko Konevy, poza ostrzałem i mgłą, Patka odzyskuje HP przez maks. 5 s.","Recalls Patsy. Out of fire and fog, she heals near Koneva for up to 5 s."),
        (KonevaHero,2)=>SkillText("Osłona z powietrza","Aerial Cover","Patka leci do pobliskiego partnera i przez 5 s może przejąć jeden jego pocisk.","Patsy flies to a nearby ally and can intercept one of their incoming shots within 5 s."),
        (KonevaHero,3)=>SkillText("Zwinny unik","Nimble Dodge","Po uniknięciu trafienia Koneva przyspiesza na 2 s.","After dodging a hit, Koneva gains speed for 2 s."),
        (MikoHero,0)=>SkillText("Szczęśliwy sabotaż","Lucky Sabotage","Losuje jeden z czterech wyników: zatrzymanie wieży na 5 s, zniszczenie, 50% szansy na pudło do końca próby albo przyspieszenie jej ostrzału na 5 s.","One of four outcomes: disable the tower for 5 s, destroy it, give it a 50% miss chance for the trial, or speed up its fire for 5 s."),
        (MikoHero,1)=>SkillText("Pchnięcie szpadą","Fencer's Thrust","Zadaje 20 obrażeń pobliskiej wieży.","Deals 20 damage to a nearby tower."),
        (MikoHero,2)=>SkillText("Impuls z cewki","Coil Pulse","Wyłącza odkryte, widoczne wieże w zasięgu na 2,5 s.","Disables revealed towers in sight and range for 2.5 s."),
        (MikoHero,3)=>SkillText("Ładunek burzy","Storm Charge","Po trafieniu Błyskawicami Miko zyskuje losowo od 0 do 50% szybkości ruchu na 4 s. Kolejne trafienie losuje premię ponownie i odświeża czas.","After a Lightning hit, Miko gains a random 0–50% movement speed bonus for 4 s. Each new hit rerolls and refreshes the bonus."),
        (SarnaHero,0)=>SkillText("Korzenie szlaku","Trail Roots","Sarna i sojusznik w promieniu 8 m zyskują 20% szybkości ruchu na 3 s.","Sarna and her ally within 8 m gain 20% movement speed for 3 s."),
        (SarnaHero,1)=>SkillText("Żywe wiązanie","Living Binding","Przywraca do 25 HP rannemu sojusznikowi w promieniu 8 m.","Restores up to 25 HP to an injured ally within 8 m."),
        (SarnaHero,2)=>SkillText("Oddech wytchnienia","Breath of Relief","Usuwa spowolnienie od Lodu i pnączy u Sarny i sojusznika w promieniu 8 m. Przez 3 s chroni przed spowolnieniem od Lodu.","Clears Ice chill and roots on Sarna and her ally within 8 m. Prevents Ice chill for 3 s."),
        (SarnaHero,3)=>SkillText("Pewny krok","Sure Step","Spowolnienie od Lodu trwa u Sarny 1,5 s zamiast 2 s.","Ice chill lasts 1.5 s on Sarna instead of 2 s."),
        (1,0)=>SkillText("Skupiony żar","Focused Ember","Zadaje odkrytej wieży 100 obrażeń w zasięgu 17,4 m.","Deals 100 damage to a revealed tower within 17.4 m."),
        (1,1)=>SkillText("Rozgrzany mechanizm","Heated Mechanism","Spowalnia ostrzał odkrytej wieży o 35% przez 4 s.","Slows a revealed tower's fire rate by 35% for 4 s."),
        (1,2)=>SkillText("Przeciążenie","Overload","Zadaje odkrytej wieży 70 obrażeń i wyłącza ją na 2 s.","Deals 70 damage to a revealed tower and disables it for 2 s."),
        (1,3)=>SkillText("Żaroodporność","Heat Resistance","Otrzymuje o 15% mniej obrażeń Ognia.","Takes 15% less Fire damage."),
        (4,0)=>SkillText("Chłodna głowa","Cool Head","Usuwa spowolnienie Lodu i chroni przed nim przez 2 s.","Clears Ice slow and prevents it for 2 s."),
        (4,1)=>SkillText("Lodowy opatrunek","Icy Dressing","Leczy najbliższego rannego sojusznika o 25 HP w zasięgu 8 m.","Heals the nearest injured ally by 25 HP within 8 m."),
        (4,2)=>SkillText("Bezpieczne przejście","Safe Passage","Usuwa spowolnienie Lodu u drużyny w 6 m i chroni przed nim przez 3 s.","Clears Ice slow for allies within 6 m and prevents it for 3 s."),
        (4,3)=>SkillText("Mrozoodporność","Frost Resistance","Spowolnienia ruchu działają o 20% słabiej.","Movement slows are 20% weaker."),
        (5,0)=>SkillText("Pole życia","Field of Life","Przez 5 s leczy uczestników w polu 4 m o 11 HP co sekundę.","Heals participants in a 4 m area by 11 HP per second for 5 s."),
        (5,1)=>SkillText("Szybki opatrunek","Quick Dressing","Leczy najbliższego rannego sojusznika o 33 HP w zasięgu 8 m.","Heals the nearest injured ally by 33 HP within 8 m."),
        (5,2)=>SkillText("Odnowa szlaku","Trail Renewal","Leczy drużynę w 6 m o 22 HP i dodatkowo 5,5 HP/s przez 4 s.","Heals the team within 6 m by 22 HP and 5.5 HP/s for 4 s."),
        (5,3)=>SkillText("Troska","Care","Leczenie Liory jest o 10% silniejsze; wartości zdolności uwzględniają premię.","Liora's healing is 10% stronger; ability values include the bonus."),
        (ThoraHero,0)=>SkillText("Żywa ostoja","Living Refuge","Leczy sojuszników w promieniu 4 m o 7 HP/s przez 5 s.","Heals allies within 4 m for 7 HP/s for 5 s."),
        (ThoraHero,1)=>SkillText("Korzenna osłona","Root Shield","Daje 60 osłony i osłabia spowolnienia o 50% przez 4 s.","Grants 60 shield and halves slows for 4 s."),
        (ThoraHero,2)=>SkillText("Ostoja doliny","Valley Refuge","Strefa 5 m przez 5 s zmniejsza obrażenia od pocisków o 25%.","A 5 m zone reduces projectile damage by 25% for 5 s."),
        (ThoraHero,3)=>SkillText("Żywotne korzenie","Vital Roots","Thora otrzymuje o 15% silniejsze leczenie.","Thora receives 15% more healing."),
        (BromHero,0)=>SkillText("Mur tarcz","Shield Wall","Tarcza przez 4 s zatrzymuje do trzech pocisków.","A shield blocks up to three shots for 4 s."),
        (BromHero,1)=>SkillText("Mocny krok","Firm Step","Przez 4 s otrzymuje o 25% mniej obrażeń i ma o połowę słabsze spowolnienia.","For 4 s takes 25% less damage and halves slows."),
        (BromHero,2)=>SkillText("Kamienny bastion","Stone Bastion","Daje 50 osłony sobie i sojusznikom w promieniu 5 m.","Grants 50 shield to self and allies within 5 m."),
        (BromHero,3)=>SkillText("Kamienny pancerz","Stone Armor","Otrzymuje o 10% mniej obrażeń od bezpośrednich trafień wież.","Takes 10% less direct tower damage."),
        (MorrakHero,0)=>SkillText("Gęsty mrok","Dense Darkness","Przez 4 s spowalnia pobliskie pociski o 50% w promieniu 6 m.","Slows projectiles within 6 m by 50% for 4 s."),
        (MorrakHero,1)=>SkillText("Cień strażnika","Guardian Shadow","Daje sobie 55 osłony i pobliskiemu sojusznikowi 25.","Grants 55 shield to self and 25 to a nearby ally."),
        (MorrakHero,2)=>SkillText("Zasłona mroku","Veil of Darkness","Strefa 5 m przez 5 s spowalnia pociski o 35% i ogranicza ich obrażenia o 15%.","A 5 m zone slows projectiles by 35% and reduces projectile damage by 15% for 5 s."),
        (MorrakHero,3)=>SkillText("Oczy mroku","Eyes of Darkness","Pasywka ograniczająca utratę widoczności wymaga jeszcze systemu takich efektów.","The visibility-impairment passive needs that status system."),
        (PyrraHero,0)=>SkillText("Iskra na pomoc","Spark to the Rescue","Iskra przez 4 s może przejąć jeden pocisk skierowany w Pyrrę.","Spark can intercept one shot aimed at Pyrra for 4 s."),
        (PyrraHero,1)=>SkillText("Żarowy trop","Ember Trail","Strefa Iskry spowalnia ostrzał odkrytych wież o 25% przez 4 s.","Spark's zone slows revealed tower fire by 25% for 4 s."),
        (PyrraHero,2)=>SkillText("Płomienna osłona","Flame Cover","Iskra osłania pobliskiego sojusznika i może przejąć dwa pociski przez 5 s.","Spark covers a nearby ally and can intercept two shots for 5 s."),
        (PyrraHero,3)=>SkillText("Żar pod kontrolą","Controlled Ember","Po trafieniu Ogniem blokuje nowe podpalenie przez 3 s; odnowienie 10 s.","After a Fire hit, prevents new burns for 3 s; cooldown 10 s."),
        (AnaHero,0)=>SkillText("Bambus dla Boba","Bamboo for Bob","Przywraca pobliskiemu Bobowi 40 HP.","Restores 40 HP to nearby Bob."),
        (AnaHero,1)=>SkillText("Bob osłania","Bob Covers","Bob odciąga ogień najbliższej odkrytej wieży przez 4 s.","Bob draws fire from the nearest revealed tower for 4 s."),
        (AnaHero,2)=>SkillText("Razem przez szlak","Together on the Trail","Bob i sojusznicy w jego pobliżu otrzymują 35 osłony.","Bob and nearby allies gain 35 shield."),
        (AnaHero,3)=>SkillText("Opiekuńczy dotyk","Caring Touch","Premia do leczenia najbardziej rannego sojusznika wymaga dalszego podłączenia.","The lowest-health ally healing bonus is not yet connected."),
        (LumenHero,0)=>SkillText("Bariera Blasku","Gleam's Barrier","Blask daje 50 osłony najbliższemu sojusznikowi w 8 m.","Gleam grants 50 shield to the nearest ally within 8 m."),
        (LumenHero,1)=>SkillText("Świetlny zwiad","Light Scout","Odkrywa najbliższą ukrytą wieżę do 50 m przed drużyną.","Reveals the nearest hidden tower up to 50 m ahead."),
        (LumenHero,2)=>SkillText("Droga światła","Path of Light","Sojusznicy przy Blasku zyskują prędkość i ochronę przed pociskami przez 5 s.","Allies near Gleam gain speed and projectile protection for 5 s."),
        (LumenHero,3)=>SkillText("Uważna obserwatorka","Attentive Observer","Premia do czasu wykrycia wymaga docelowego systemu widoczności.","The detection-duration bonus needs the final visibility system."),
        _=>("","")
    };

    static readonly string[] SharedSkillKeys={"Q","E","R","P"};

    static string SharedRoleSummary(int hero)=>hero switch
    {
        KaelHero=>RiftorynCodexData.Text("Toruje partnerowi przejście pod ostrzałem.","Makes a path for her partner under fire."),
        NeraHero=>RiftorynCodexData.Text("Chroni pobliskiego partnera przed obrażeniami i statusami.","Protects a nearby partner from damage and status effects."),
        ZetHero=>RiftorynCodexData.Text("Odkrywa wieże i otwiera krótkie okno na przebieg.","Scouts towers and opens short windows to run past them."),
        KonevaHero=>RiftorynCodexData.Text("Kieruje Patką, by osłonić duet lub odciągnąć ostrzał.","Directs Patsy to cover the pair or draw enemy fire."),
        MikoHero=>RiftorynCodexData.Text("Sabotuje wieże szpadą i wynalazkami.","Sabotages towers with his blade and inventions."),
        SarnaHero=>RiftorynCodexData.Text("Przyspiesza duet, leczy partnera i usuwa spowolnienia.","Speeds up the pair, heals her ally, and clears slows."),
        SailaHero=>RiftorynCodexData.Text("Zamraża wieże i otwiera drużynie drogę pod ostrzałem.","Freezes towers and opens a path for the team under fire."),
        1=>RiftorynCodexData.Text("Uszkadza i osłabia wybrane wieże.","Damages and disrupts selected towers."),
        4=>RiftorynCodexData.Text("Usuwa spowolnienia i opatruje rannych sojuszników.","Clears slows and heals injured allies."),
        5=>RiftorynCodexData.Text("Leczy drużynę w polu i podczas ucieczki.","Heals the team in an area and on the move."),
        ThoraHero=>RiftorynCodexData.Text("Utrzymuje sojuszników przy życiu i tworzy strefę ochronną.","Sustains allies and creates a protective zone."),
        BromHero=>RiftorynCodexData.Text("Zasłania drużynę i przyjmuje ostrzał.","Shields the team and absorbs tower fire."),
        MorrakHero=>RiftorynCodexData.Text("Spowalnia nadlatujące pociski i chroni sojuszników.","Slows incoming projectiles and protects allies."),
        PyrraHero=>RiftorynCodexData.Text("Kieruje Iskrą, która osłania drużynę i osłabia wieże.","Directs Spark to protect allies and hinder towers."),
        AnaHero=>RiftorynCodexData.Text("Opiekuje się Bobem i wykorzystuje go do osłony drużyny.","Keeps Bob healthy and uses him to cover the team."),
        LumenHero=>RiftorynCodexData.Text("Z Blaskiem odkrywa drogę i wspiera sojuszników.","Scouts and supports allies with Gleam."),
        _=>""
    };

    string SharedKitSummary(int hero)
    {
        string summary="";
        for(int slot=0;slot<SharedSkillKeys.Length;slot++)
        {
            var info=SharedSkillInfo(hero,slot);
            summary+=(slot==0?"":"\n\n")+$"{SharedSkillKeys[slot]} · {info.name}. {info.description}";
        }
        return summary;
    }

    void DrawSharedControls()
    {
        string dodge=player.rollCooldown>0?RiftorynL10n.Format("{0:F1} s",player.rollCooldown):RiftorynL10n.Translate("GOTOWY");
        string controls=RiftorynCodexData.Text("SPACJA · UNIK {0}   •   WASD · RUCH   •   MYSZ · KAMERA","SPACE · DODGE {0}   •   WASD · MOVE   •   MOUSE · CAMERA");
        GUI.Label(new Rect(18,67,Screen.width-36,20),RiftorynL10n.Format(controls,dodge),small);
    }
}
