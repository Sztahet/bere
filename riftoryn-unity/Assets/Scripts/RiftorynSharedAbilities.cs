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
        public float recallUntil, guardUntil, mikoBoostUntil, mikoSpeedBonus;
        public bool rewardedRoll;
    }

    sealed class HeatBarrier
    {
        public LineRenderer outline;
        public Material material;
        public Vector3 center, normal, side;
        public float endsAt;
        public int charges;
    }

    HeatBarrier heatBarrier;

    Runner PartnerOf(Runner runner)=>runner==player?ally:player;

    bool RunnersNear(Runner first,Runner second,float range)=>
        IsRunning(first)&&IsRunning(second)
        &&(first.obj.transform.position-second.obj.transform.position).sqrMagnitude<=range*range;

    static (float secondary,float ultimate) SharedAbilityCooldowns(int hero)=>hero switch
    {
        KaelHero=>(16f,36f),
        NeraHero=>(14f,32f),
        ZetHero=>(16f,26f),
        KonevaHero=>(18f,30f),
        MikoHero=>(12f,32f),
        SarnaHero=>(16f,32f),
        _=>(0f,0f)
    };

    float SharedCooldownLeft(Runner runner,bool ultimate)=>
        Mathf.Max(0,(ultimate?runner.kit.ultimateReadyAt:runner.kit.secondaryReadyAt)-elapsed);

    bool TryUseSharedAbility(Runner runner,bool ultimate,Tower tower=null)
    {
        if(!sharedEscape||!IsRunning(runner)||SharedCooldownLeft(runner,ultimate)>0) return false;
        Runner partner=PartnerOf(runner);
        bool cast=runner.hero switch
        {
            KaelHero=>ultimate?RaiseHeatBarrier(runner,tower):DivertSharedShot(runner,tower),
            NeraHero=>ProtectSharedPartner(runner,partner,ultimate),
            ZetHero=>ultimate?StartSharedSurge(runner):ScoutSharedTower(runner,partner),
            KonevaHero=>CommandPatka(runner,partner,ultimate),
            MikoHero=>MikoExtra(runner,ultimate,tower),
            SarnaHero=>SupportSarnaTeam(runner,partner,ultimate),
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

    void BoostSarnaTeam(Runner runner)
    {
        if(!IsRunning(runner)) return;
        runner.boost=Mathf.Max(runner.boost,3f);
        Runner partner=PartnerOf(runner);
        if(MatchMode)
        {
            partner=null;
            float best=SupportRange*SupportRange;
            foreach(var candidate in AllRunners())
            {
                if(candidate==runner||!IsRunning(candidate)) continue;
                float distance=(candidate.obj.transform.position-runner.obj.transform.position).sqrMagnitude;
                if(distance<best){best=distance;partner=candidate;}
            }
        }
        if(RunnersNear(runner,partner,SupportRange)) partner.boost=Mathf.Max(partner.boost,3f);
        runner.cooldown=Cooldowns[SarnaHero];
        runner.uses++;
        if(runner==player) Notify(SharedSkillInfo(SarnaHero,0).name);
    }

    bool SupportSarnaTeam(Runner runner,Runner partner,bool ultimate)
    {
        bool near=RunnersNear(runner,partner,SupportRange);
        if(!ultimate)
        {
            if(!near||partner.hp>=MaxHp[partner.hero]) return false;
            float restored=Mathf.Min(25f,MaxHp[partner.hero]-partner.hp);
            partner.hp+=restored;
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
        runner.cooldown=Cooldowns[MikoHero];runner.uses++;
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
                if(candidate.hp<=0||sharedEscape&&!candidate.discovered) continue;
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
        if(heatBarrier!=null) return false;
        threat??=Nearest(runner,TowerSkillRange);
        Vector3 origin=runner.obj.transform.position;
        Vector3 normal=threat!=null?Slot(threat.slot)-origin:runner.obj.transform.forward;
        normal.y=0;
        if(normal.sqrMagnitude<.01f) return false;
        normal.Normalize();
        Vector3 side=Vector3.Cross(Vector3.up,normal);
        Vector3 center=origin+normal*1.6f+Vector3.up*(BarrierHeight*.5f);
        var outline=new GameObject("Kurtyna żaru").AddComponent<LineRenderer>();
        outline.transform.SetParent(world.transform);
        outline.useWorldSpace=true;
        outline.loop=true;
        outline.positionCount=4;
        outline.startWidth=outline.endWidth=.13f;
        var material=new Material(Shader.Find("Sprites/Default"));
        outline.sharedMaterial=material;
        outline.startColor=outline.endColor=new Color(1f,.56f,.16f);
        Vector3 halfWidth=side*(BarrierWidth*.5f), halfHeight=Vector3.up*(BarrierHeight*.5f);
        outline.SetPositions(new[]{center-halfWidth-halfHeight,center+halfWidth-halfHeight,
            center+halfWidth+halfHeight,center-halfWidth+halfHeight});
        heatBarrier=new HeatBarrier{outline=outline,material=material,center=center,normal=normal,side=side,endsAt=elapsed+4f,charges=3};
        return true;
    }

    void ClearHeatBarrier()
    {
        if(heatBarrier==null) return;
        if(heatBarrier.outline) Destroy(heatBarrier.outline.gameObject);
        if(heatBarrier.material) Destroy(heatBarrier.material);
        heatBarrier=null;
    }

    bool HeatBarrierBlocks(Vector3 start,Vector3 segment,float firstHit,out Vector3 impact)
    {
        impact=start;
        if(heatBarrier==null) return false;
        float length=segment.magnitude;
        if(length<=0) return false;
        var ray=new Ray(start,segment/length);
        var plane=new Plane(heatBarrier.normal,heatBarrier.center);
        if(!plane.Raycast(ray,out float distance)||distance>length||distance>length*firstHit) return false;
        Vector3 intercept=ray.GetPoint(distance);
        Vector3 offset=intercept-heatBarrier.center;
        if(Mathf.Abs(Vector3.Dot(offset,heatBarrier.side))>BarrierWidth*.5f||Mathf.Abs(offset.y)>BarrierHeight*.5f) return false;
        impact=intercept;
        if(--heatBarrier.charges==0) ClearHeatBarrier();
        return true;
    }

    bool ProtectSharedPartner(Runner runner,Runner partner,bool reserve)
    {
        if(!RunnersNear(runner,partner,SupportRange)) return false;
        if(reserve)
        {
            if(partner.hp<=MaxHp[partner.hero]*ReserveThreshold) AddShield(partner,ReserveShield);
            else partner.kit.reserveUntil=elapsed+6f;
        }
        else partner.kit.wardUntil=elapsed+5f;
        LightningArc(runner.obj.transform.position+Vector3.up,partner.obj.transform.position+Vector3.up);
        return true;
    }

    void TryTriggerSharedReserve(Runner runner,float damage)
    {
        if(runner.kit.reserveUntil<=elapsed||runner.hp-Mathf.Max(0,damage-runner.shield)>MaxHp[runner.hero]*ReserveThreshold)
            return;
        runner.kit.reserveUntil=0;
        AddShield(runner,ReserveShield);
        ShowCombatText(runner,RiftorynCodexData.Text("REZERWA","RESERVE"),new Color(.46f,.8f,1f));
    }

    float ShieldDecay(Runner runner)
    {
        Runner partner=PartnerOf(runner);
        return sharedEscape&&partner?.hero==NeraHero&&RunnersNear(runner,partner,SupportRange) ? .94f : .90f;
    }

    bool ConsumeSharedWard(Runner runner,int type)
    {
        if(!sharedEscape||runner.kit.wardUntil<=elapsed||type!=0&&type!=1&&type!=3) return false;
        runner.kit.wardUntil=0;
        ShowCombatText(runner,RiftorynCodexData.Text("UZIEMIENIE","GROUNDED"),new Color(.48f,.82f,1f));
        return true;
    }

    bool ScoutSharedTower(Runner runner,Runner partner)
    {
        float progress=IsRunning(partner)?Mathf.Max(runner.along,partner.along):runner.along;
        Tower next=null;
        float best=ScoutRange;
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
        next.rangeVisibleUntil=elapsed+6f;
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
        if(sharedEscape&&runner.hero==KonevaHero&&!runner.kit.rewardedRoll)
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
            pet.Guard(partner.obj.transform);
        }
        else
        {
            if(!pet.IsHurt&&!pet.IsFlying&&runner.kit.guardUntil<=elapsed) return false;
            runner.kit.guardUntil=0;
            runner.kit.recallUntil=elapsed+5f;
            pet.ReturnToOwner();
        }
        return true;
    }

    void StepSharedAbilityEffects(float dt)
    {
        if(heatBarrier!=null&&heatBarrier.endsAt<=elapsed) ClearHeatBarrier();
        Runner owner=player.hero==KonevaHero?player:ally?.hero==KonevaHero?ally:null;
        if(!IsRunning(owner)||owner.kit.guardUntil<=0&&owner.kit.recallUntil<=elapsed) return;
        var pet=owner.obj.GetComponentInChildren<RiftorynPatkaPreview>();
        if(!pet||!pet.IsAlive)
        {
            owner.kit.guardUntil=0;
            owner.kit.recallUntil=0;
            return;
        }
        if(owner.kit.guardUntil>0&&(owner.kit.guardUntil<=elapsed||!IsRunning(PartnerOf(owner))))
        {
            owner.kit.guardUntil=0;
            pet.ReturnToOwner();
        }
        if(owner.kit.recallUntil>elapsed&&SharedTowerAimingAt(owner)==null&&!IncomingSharedShot(owner)&&!RunnerInFog(owner)
            &&(pet.AimPosition-owner.obj.transform.position).sqrMagnitude<12.25f)
            pet.Heal(6f*dt);
    }

    bool PatkaGuardsShot(Runner victim,Shot shot)
    {
        if(IsDodging(victim)) return false;
        Runner owner=PartnerOf(victim);
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
        pet.ReturnToOwner();
    }

    bool TrySharedBotExtra(Tower threat,Tower partnerThreat,bool shotIncoming,bool partnerIncoming,bool fogClose)
    {
        switch(ally.hero)
        {
            case KaelHero:
                if(partnerThreat!=null&&ally.shield>20f&&TryUseSharedAbility(ally,false,partnerThreat)) return true;
                Tower imminent=threat??partnerThreat;
                return (shotIncoming||partnerIncoming||imminent!=null&&imminent.windup<=.18f)
                    &&TryUseSharedAbility(ally,true,imminent);
            case NeraHero:
                if((partnerThreat!=null||partnerIncoming)&&player.hp<MaxHp[player.hero]*.48f&&TryUseSharedAbility(ally,true)) return true;
                return partnerThreat!=null&&(partnerThreat.type==0||partnerThreat.type==1)&&TryUseSharedAbility(ally,false);
            case ZetHero:
                if((threat!=null||fogClose)&&!shotIncoming&&(!IsRunning(player)||ally.along<=player.along+2f)
                    &&(threat==null||ally.cooldown>0)&&TryUseSharedAbility(ally,true)) return true;
                return threat==null&&partnerThreat==null&&!shotIncoming&&TryUseSharedAbility(ally,false);
            case KonevaHero:
                var pet=ally.obj.GetComponentInChildren<RiftorynPatkaPreview>();
                if(!pet||!pet.IsAlive) return false;
                if(partnerThreat!=null&&!pet.IsFlying&&(ally.cooldown>0||player.hp<MaxHp[player.hero]*.45f)
                    &&TryUseSharedAbility(ally,true)) return true;
                return threat==null&&partnerThreat==null&&!shotIncoming&&pet.IsHurt&&!pet.IsFlying&&ally.kit.guardUntil<=elapsed
                    &&TryUseSharedAbility(ally,false);
            case MikoHero:
                if((threat!=null||partnerThreat!=null)&&TryUseSharedAbility(ally,true)) return true;
                return threat!=null&&TryUseSharedAbility(ally,false,threat);
            case SarnaHero:
                if((ally.chillTime>0||ally.rootTime>0
                    ||RunnersNear(ally,player,SupportRange)&&(player.chillTime>0||player.rootTime>0))
                    &&TryUseSharedAbility(ally,true)) return true;
                return IsRunning(player)&&player.hp<=MaxHp[player.hero]-20f&&TryUseSharedAbility(ally,false);
            default:return false;
        }
    }

    static (string name,string description) SkillText(string name,string englishName,string description,string englishDescription)=>
        (RiftorynCodexData.Text(name,englishName),RiftorynCodexData.Text(description,englishDescription));

    static (string name,string description) SharedSkillInfo(int hero,int slot)=>(hero,slot) switch
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
