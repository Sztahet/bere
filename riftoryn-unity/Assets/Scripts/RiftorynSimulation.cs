using System;
using System.Collections.Generic;
using System.Threading;

[Serializable] public class SimParams {
    public float zasieg_m,promien_m,oslona_wlasna,oslona_sojusznika,oslona,leczenie,
        premia_predkosci_procent,czas_s,predkosc_procent,obrazenia,wylaczenie_s,czas_pecha_s,
        leczenie_poczatkowe,leczenie_na_sekunde,spowolnienie_ataku_procent,
        redukcja_procent,redukcja_obrazen_procent,redukcja_spowolnienia_procent,
        szerokosc_m,wysokosc_m,maks_pociskow,spowolnienie_pociskow_procent;
}
[Serializable] public class SimAbility { public float odnowienie_s; public SimParams parametry; }
[Serializable] public class SimSkills { public SimAbility Q,E,R; }
[Serializable] public class SimStats { public float hp,predkosc_m_s,atak_podstawowy,zasieg_ataku_m,odnowienie_ataku_s; }
[Serializable] public class SimHero { public string id,nazwa,rola; public SimStats statystyki; public SimSkills umiejetnosci; }
public sealed class SimResult { public string id,name; public int contexts; public double delta,allies,self; }

// Portable worker: no Unity API calls. An intentionally partial 1D model, not a Unity playthrough.
public sealed class RiftorynSimulation {
    const float Distance=472.5f, Timeout=137f, Step=.5f;
    static readonly float[] Damage={13,7,9,10,10,10,10}, Rate={1.4f,1.5f,1,1.3f,1.3f,1.3f,1.3f}, Range={13.8f,15,13.8f,15,15,15,15};
    sealed class Runner {
        public SimHero hero; public float hp,x,shield,speedUntil,speed=1; public bool done; public readonly float[] ready=new float[4];
        public bool Alive=>hp>0&&!done;
    }
    sealed class Tower { public float x,hp=100,next,disabled,overload; public bool miss; public int type; }
    sealed class Variant { public int hero; public double companions,own; }
    readonly SimHero[] heroes; readonly int trials,towerCount,seed; readonly string layout; readonly bool attack;
    int completed;
    public int Completed=>Volatile.Read(ref completed);
    public int TotalTeams=>heroes.Length*(heroes.Length-1)*(heroes.Length-2)*(heroes.Length-3)*(heroes.Length-4)/120;
    public int Trials=>trials;
    public RiftorynSimulation(SimHero[] heroes,int trials,int towerCount,int seed,string layout,bool attack) {
        if(heroes==null||heroes.Length!=16)throw new ArgumentException("Potrzeba dokładnie 16 bohaterów.");
        if(trials<1||towerCount<1||towerCount>50)throw new ArgumentException("Nieprawidłowe parametry.");
        this.heroes=heroes;this.trials=trials;this.towerCount=towerCount;this.seed=seed;this.layout=layout;this.attack=attack;
    }
    static float Abs(float x)=>Math.Abs(x);
    static bool Supported(string id,int slot) {
        switch(id) {
            case "kael":case "nera":return slot==0;
            case "thora":case "morrak":case "niva":case "liora":return slot==1;
            case "brom":return slot==2;
            case "sarna":return slot<2;
            case "rian":case "saila":case "zet":return slot!=1;
            case "miko":return true;
            default:return false;
        }
    }
    static SimAbility Skill(Runner r,int slot)=>slot==0?r.hero.umiejetnosci.Q:slot==1?r.hero.umiejetnosci.E:r.hero.umiejetnosci.R;
    static void Cast(Runner r,Runner[] team,Tower[] towers,int slot,float now,Random rng) {
        string id=r.hero.id;
        if(!Supported(id,slot)||now<r.ready[slot])return;
        SimAbility skill=Skill(r,slot);var p=skill.parametry;
        float reach=p.zasieg_m>0?p.zasieg_m:p.promien_m;
        Runner near=null,injured=null;float nd=float.MaxValue,hd=float.MaxValue;
        foreach(var other in team)if(other!=r&&other.Alive) {
            float d=Abs(other.x-r.x);if(d<=reach&&d<nd){near=other;nd=d;}
            if(d<=reach&&d<hd&&other.hp<other.hero.statystyki.hp){injured=other;hd=d;}
        }
        Tower nearest=null;float td=float.MaxValue;bool threat=false;
        foreach(var t in towers)if(t.hp>0){float d=Abs(t.x-r.x);if(d<18)threat=true;if(d<=reach&&d<td){nearest=t;td=d;}}
        bool used=false;
        if((id=="kael"||id=="nera")&&slot==0||id=="morrak"&&slot==1) {
            if(threat){r.shield+=p.oslona_wlasna;if(near!=null)near.shield+=p.oslona_sojusznika;used=true;}
        } else if(id=="thora"&&slot==1) {if(threat){r.shield+=p.oslona;used=true;}}
        else if(id=="brom"&&slot==2) {if(threat){r.shield+=p.oslona;foreach(var other in team)if(other!=r&&other.Alive&&Abs(other.x-r.x)<=reach)other.shield+=p.oslona;used=true;}}
        else if((id=="niva"||id=="liora"||id=="sarna")&&slot==1) {if(injured!=null){injured.hp=Math.Min(injured.hero.statystyki.hp,injured.hp+p.leczenie);used=true;}}
        else if(id=="sarna"&&slot==0) {if(threat){Boost(r,now,p.czas_s,1+p.premia_predkosci_procent/100);if(near!=null)Boost(near,now,p.czas_s,1+p.premia_predkosci_procent/100);used=true;}}
        else if(id=="zet"&&slot==2) {if(threat){r.speed=p.predkosc_procent/100;r.speedUntil=now+p.czas_s;used=true;}}
        else if(id=="miko"&&slot==0&&nearest!=null){int outcome=rng.Next(4);if(outcome==0)nearest.disabled=Math.Max(nearest.disabled,now+p.wylaczenie_s);else if(outcome==1)nearest.hp=0;else if(outcome==2)nearest.miss=true;else nearest.overload=Math.Max(nearest.overload,now+p.czas_pecha_s);used=true;}
        else if((id=="rian"||id=="saila"||id=="zet"||id=="miko")&&nearest!=null){
            if(slot==2&&(id=="saila"||id=="miko")){foreach(var t in towers)if(t.hp>0&&Abs(t.x-r.x)<=reach)Apply(t,p,now);}
            else Apply(nearest,p,now);used=true;
        }
        if(used)r.ready[slot]=now+skill.odnowienie_s;
    }
    static void Boost(Runner r,float now,float duration,float value){r.speed=Math.Max(r.speedUntil>now?r.speed:1,value);r.speedUntil=Math.Max(r.speedUntil,now+duration);}
    static void Apply(Tower t,SimParams p,float now){t.hp=Math.Max(0,t.hp-p.obrazenia);t.disabled=Math.Max(t.disabled,now+p.wylaczenie_s);}
    bool[] Run(int[] indices,int trialSeed,CancellationToken token){
        var team=new Runner[5];for(int i=0;i<5;i++)team[i]=new Runner{hero=heroes[indices[i]],hp=heroes[indices[i]].statystyki.hp};
        var mapRng=new Random(trialSeed);var rng=new Random(trialSeed^0xA47);var towers=new Tower[towerCount];
        for(int i=0;i<towerCount;i++){
            float x;if(layout=="skupiska"){int group=i/2,groups=(towerCount+1)/2;x=Distance*(group+1)/(groups+1)+(i%2==0?-8:8);}
            else {float spacing=Distance/(towerCount+1);x=spacing*(i+1);if(layout=="losowy")x+=(float)(mapRng.NextDouble()*.6-.3)*spacing;}
            towers[i]=new Tower{x=x};
        }
        foreach(var t in towers)t.type=mapRng.Next(7);
        for(float time=0;time<Timeout;time+=Step){
            if(token.IsCancellationRequested)token.ThrowIfCancellationRequested();
            bool alive=false;
            foreach(var r in team)if(r.Alive){alive=true;for(int s=0;s<3;s++)Cast(r,team,towers,s,time,rng);
                if(attack&&time>=r.ready[3]){Tower nearest=null;float best=r.hero.statystyki.zasieg_ataku_m;foreach(var t in towers)if(t.hp>0&&Abs(t.x-r.x)<=best){nearest=t;best=Abs(t.x-r.x);}if(nearest!=null){nearest.hp=Math.Max(0,nearest.hp-r.hero.statystyki.atak_podstawowy);r.ready[3]=time+r.hero.statystyki.odnowienie_ataku_s;}}}
            if(!alive)break;
            foreach(var t in towers){if(t.hp<=0||time<t.disabled||time<t.next)continue;var targets=new List<Runner>();foreach(var r in team)if(r.Alive&&Abs(r.x-t.x)<=Range[t.type])targets.Add(r);if(targets.Count==0)continue;
                var target=targets[rng.Next(targets.Count)];if(!t.miss||rng.NextDouble()>=.5){float blocked=Math.Min(target.shield,Damage[t.type]);target.shield-=blocked;target.hp=Math.Max(0,target.hp-(Damage[t.type]-blocked));}
                t.next=time+Rate[t.type]/(time<t.overload?1.5f:1f);
            }
            foreach(var r in team)if(r.Alive){r.shield*=.9486833f;r.x+=Step*r.hero.statystyki.predkosc_m_s*(time<r.speedUntil?r.speed:1);if(r.x>=Distance){r.x=Distance;r.done=true;}}
        }
        var result=new bool[5];for(int i=0;i<5;i++)result[i]=team[i].done;return result;
    }
    public List<SimResult> Analyze(CancellationToken token){
        var groups=new Dictionary<int,List<Variant>>();int n=heroes.Length;var ids=new int[5];
        for(int a=0;a<n-4;a++)for(int b=a+1;b<n-3;b++)for(int c=b+1;c<n-2;c++)for(int d=c+1;d<n-1;d++)for(int e=d+1;e<n;e++){
            token.ThrowIfCancellationRequested();ids[0]=a;ids[1]=b;ids[2]=c;ids[3]=d;ids[4]=e;var counts=new int[5];
            for(int s=0;s<trials;s++){var done=Run(ids,seed+s,token);for(int k=0;k<5;k++)if(done[k])counts[k]++;}
            int mask=(1<<a)|(1<<b)|(1<<c)|(1<<d)|(1<<e),sum=0;foreach(int count in counts)sum+=count;
            for(int k=0;k<5;k++){int key=mask&~(1<<ids[k]);if(!groups.TryGetValue(key,out var list)){list=new List<Variant>();groups.Add(key,list);}list.Add(new Variant{hero=ids[k],companions=(sum-counts[k])/(4.0*trials),own=counts[k]/(double)trials});}
            Interlocked.Increment(ref completed);
        }
        var output=new SimResult[n];for(int i=0;i<n;i++)output[i]=new SimResult{id=heroes[i].id,name=heroes[i].nazwa};
        foreach(var group in groups.Values){double total=0;foreach(var v in group)total+=v.companions;foreach(var v in group){var row=output[v.hero];row.contexts++;row.delta+=v.companions-(total-v.companions)/(group.Count-1);row.allies+=v.companions;row.self+=v.own;}}
        foreach(var row in output){row.delta=100*row.delta/row.contexts;row.allies=100*row.allies/row.contexts;row.self=100*row.self/row.contexts;}
        var result=new List<SimResult>(output);result.Sort((x,y)=>{int cmp=x.delta.CompareTo(y.delta);return cmp!=0?cmp:string.CompareOrdinal(x.id,y.id);});return result;
    }
}
