using UnityEngine;

public partial class RiftorynTrial
{
    void AttachSummonedCompanion(GameObject owner,int hero,RiftorynSparkFollower follower)
    {
        var design=AdditionalHeroDesign(hero);if(design.towarzysz==null)return;
        GameObject pet;bool hasFox=follower&&follower.VisualTransform;
        if(hasFox)pet=follower.VisualTransform.gameObject;
        else
        {
            pet=new GameObject(design.towarzysz.nazwa+" · model tymczasowy");pet.transform.SetParent(owner.transform,false);
            var prefab=Resources.Load<GameObject>(hero==AnaHero?"Pets/bob-poc":"Pets/blask-poc");
            if(prefab)
            {
                var model=Instantiate(prefab,pet.transform);model.name=design.towarzysz.nazwa+" · model PoC";
                var renderers=model.GetComponentsInChildren<Renderer>();
                if(renderers.Length>0)
                {
                    Bounds bounds=renderers[0].bounds;foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
                    if(bounds.size.y>.001f)
                    {
                        model.transform.localScale*=(hero==AnaHero?1f:1.5f)/bounds.size.y;
                        bounds=renderers[0].bounds;foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
                        model.transform.position+=pet.transform.position-new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);
                    }
                }
            }
            else
            {
                var body=GameObject.CreatePrimitive(PrimitiveType.Sphere);body.name="Sylwetka towarzysza";body.transform.SetParent(pet.transform,false);
                body.transform.localPosition=Vector3.up*.5f;body.transform.localScale=hero==AnaHero?new Vector3(.9f,.8f,1.1f):new Vector3(.55f,1f,.9f);
                body.GetComponent<Renderer>().sharedMaterial=Mat(hero==AnaHero?new Color(.22f,.27f,.20f):new Color(.88f,.76f,.44f));Destroy(body.GetComponent<Collider>());
            }
        }
        var companion=pet.AddComponent<RiftorynSummonedCompanion>();
        string gaitPrefix=hero==AnaHero?"Bob":hero==LumenHero?"Blask":null;
        companion.Initialize(owner.transform,design.towarzysz.nazwa,design.towarzysz.hp,hasFox?follower:null,hero==AnaHero,gaitPrefix);
    }
    RiftorynSummonedCompanion SummonedPet(Runner runner)=>runner?.obj?runner.obj.GetComponentInChildren<RiftorynSummonedCompanion>():null;

    bool UseSummonerBasic(Runner runner)
    {
        var pet=SummonedPet(runner);if(!pet||!pet.IsAlive)return false;
        var skill=AdditionalHeroDesign(runner.hero).umiejetnosci.Q;var p=skill.parametry;
        if(runner.hero==PyrraHero)StartSummonGuard(runner,runner,p);
        else if(runner.hero==AnaHero)
        {
            if(pet.CurrentHp>=pet.MaximumHp||Vector3.Distance(pet.AimPosition,runner.obj.transform.position)>p.zasieg_m)return false;
            pet.Heal(p.leczenie);
        }
        else if(runner.hero==LumenHero)
        {
            Runner ally=NearestAllyMatching(runner,p.zasieg_m,_=>true);if(ally==null)return false;AddShield(ally,p.oslona);
        }
        else return false;
        runner.cooldown=skill.odnowienie_s;runner.uses++;
        if(runner==player)Notify(SharedSkillInfo(runner.hero,0).name);
        return true;
    }
    bool UseSummonerExtra(Runner runner,bool ultimate,Tower tower)
    {
        var pet=SummonedPet(runner);if(!pet||!pet.IsAlive)return false;
        var skills=AdditionalHeroDesign(runner.hero).umiejetnosci;var p=(ultimate?skills.R:skills.E).parametry;
        if(runner.hero==PyrraHero)
        {
            if(!ultimate){CreateTankZoneAt(pet.AimPosition-Vector3.up*.45f,p,new Color(1f,.39f,.12f));return true;}
            Runner ally=NearestAllyMatching(runner,p.zasieg_m,_=>true);if(ally==null)return false;StartSummonGuard(runner,ally,p);return true;
        }
        if(runner.hero==AnaHero)
        {
            if(ultimate)
            {
                pet.AddShield(p.oslona);
                foreach(var ally in AllRunners())if(IsRunning(ally)&&(ally.obj.transform.position-pet.AimPosition).sqrMagnitude<=p.promien_m*p.promien_m)AddShield(ally,p.oslona);
                return true;
            }
            tower??=Nearest(runner,p.zasieg_m);
            if(tower==null||!tower.discovered||tower.hp<=0||Vector3.Distance(Slot(tower.slot),pet.AimPosition)>p.zasieg_m||!TowerHasClearShot(tower,pet.AimPosition))return false;
            tower.luredCompanion=pet;tower.lureUntil=elapsed+p.czas_s;return true;
        }
        if(runner.hero==LumenHero)
        {
            if(ultimate){runner.kit.lumenAuraUntil=elapsed+p.czas_s;return true;}
            return RevealSharedTowerAhead(runner,null,p.zasieg_trasy_m,p.czas_podgladu_s);
        }
        return false;
    }
    void StartSummonGuard(Runner owner,Runner target,SimParams p)
    {
        owner.kit.summonGuardTarget=target;owner.kit.summonGuardUntil=elapsed+p.czas_s;
        owner.kit.summonGuardCharges=Mathf.RoundToInt(p.maks_pociskow);
        var follower=owner.obj.GetComponent<RiftorynSparkFollower>();if(follower)follower.target=target.obj.transform;
    }
    void StepSummonerGuards()
    {
        foreach(var owner in AllRunners())
        {
            if(!IsRunning(owner)||owner.hero!=PyrraHero)continue;
            var follower=owner.obj.GetComponent<RiftorynSparkFollower>();if(!follower)continue;
            var pet=SummonedPet(owner);
            if(owner.kit.summonGuardUntil<=elapsed||owner.kit.summonGuardCharges<=0||!IsRunning(owner.kit.summonGuardTarget)||!pet||!pet.IsAlive)
            {owner.kit.summonGuardTarget=null;owner.kit.summonGuardCharges=0;follower.target=owner.obj.transform;}
        }
    }
    bool TrySummonGuard(Runner victim,Shot shot)
    {
        foreach(var owner in AllRunners())
        {
            if(!IsRunning(owner)||owner.hero!=PyrraHero||owner.kit.summonGuardTarget!=victim||owner.kit.summonGuardUntil<=elapsed||owner.kit.summonGuardCharges<=0)continue;
            var pet=SummonedPet(owner);if(!pet||!pet.IsAlive||(pet.AimPosition-victim.obj.transform.position).sqrMagnitude>9f||!HasLineOfSight(pet.AimPosition,victim.obj.transform.position+Vector3.up))continue;
            pet.ReceiveHit(shot.damage);owner.kit.summonGuardCharges--;return true;
        }
        return false;
    }
    void ConsumePhysicalSummonGuard(RiftorynCompanion pet,Runner target)
    {
        if(target==null)return;
        foreach(var owner in AllRunners())
            if(IsRunning(owner)&&owner.hero==PyrraHero&&owner.kit.summonGuardTarget==target&&owner.kit.summonGuardUntil>elapsed&&owner.kit.summonGuardCharges>0&&SummonedPet(owner)==pet)
            {owner.kit.summonGuardCharges--;return;}
    }
    float LumenAuraSpeed(Runner target)=>LumenAura(target,true);
    float LumenAuraDamage(Runner target)=>LumenAura(target,false);
    float LumenAura(Runner target,bool speed)
    {
        float value=1f;
        foreach(var owner in AllRunners())
        {
            if(!IsRunning(owner)||owner.hero!=LumenHero||owner.kit.lumenAuraUntil<=elapsed)continue;
            var pet=SummonedPet(owner);if(!pet||!pet.IsAlive)continue;
            var p=AdditionalHeroDesign(LumenHero).umiejetnosci.R.parametry;
            if((target.obj.transform.position-pet.AimPosition).sqrMagnitude>p.promien_m*p.promien_m)continue;
            value=speed?Mathf.Max(value,1f+p.premia_predkosci_procent/100f):Mathf.Min(value,1f-p.redukcja_obrazen_procent/100f);
        }
        return value;
    }
}
