using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public partial class RiftorynMenu {
    RiftorynSimulation simWorker;
    Task<List<SimResult>> simTask;
    CancellationTokenSource simCancellation;
    List<SimResult> simResults;
    int simTowerCount=10,simTrials=3,simSeed=100,simLayoutIndex;
    bool simBasicAttack;
    string simError="",simResultSettings="";
    Vector2 simScroll;
    static readonly string[] SimLayouts={"losowy","rowny","skupiska"};

    void StartSimulation(){
        if(simTask!=null&&!simTask.IsCompleted)return;
        try {
            var data=Resources.LoadAll<TextAsset>("HeroDesign");
            var heroes=data.Select(asset=>JsonUtility.FromJson<SimHero>(asset.text)).ToArray();
            if(heroes.Length!=16||heroes.Any(h=>h==null||string.IsNullOrEmpty(h.id)||h.statystyki==null||h.umiejetnosci==null||h.umiejetnosci.Q?.parametry==null||h.umiejetnosci.E?.parametry==null||h.umiejetnosci.R?.parametry==null)||heroes.Select(h=>h.id).Distinct().Count()!=16)
                throw new InvalidOperationException("Brak kompletnych i unikalnych danych 16 bohaterów.");
            heroes=heroes.OrderBy(h=>h.id,StringComparer.Ordinal).ToArray();
            simWorker=new RiftorynSimulation(heroes,simTrials,simTowerCount,simSeed,SimLayouts[simLayoutIndex],simBasicAttack);
            simCancellation?.Dispose();simCancellation=new CancellationTokenSource();
            var worker=simWorker;var token=simCancellation.Token;
            simTask=Task.Run(()=>worker.Analyze(token),token);
            simResultSettings=$"{simTowerCount} wież · {SimLayouts[simLayoutIndex]} · {simTrials} prób · seed {simSeed} · atak {(simBasicAttack?"tak":"nie")}";
            simResults=null;simError="";simScroll=Vector2.zero;
        }catch(Exception e){simError=e.Message;simTask=null;}
    }
    void PollSimulation(){
        if(simTask==null||!simTask.IsCompleted)return;
        if(simTask.IsCanceled)simError="Symulacja została przerwana.";
        else if(simTask.IsFaulted)simError="Błąd symulacji: "+simTask.Exception.GetBaseException().Message;
        else simResults=simTask.Result;
        simTask=null;simCancellation?.Dispose();simCancellation=null;
    }
    void DrawSimulation(){
        PollSimulation();bool running=simTask!=null;
        Text(new Rect(50,100,1200,35),"LABORATORIUM  /  SYMULACJA BOHATERÓW",eyebrow);
        Text(new Rect(48,134,1100,48),"TESTY DRUŻYN",section);
        Text(new Rect(50,179,1450,50),"Model uproszczony 1D · tylko część Q/E/R · bez pasywek i towarzyszy · nie jest rozgrywką Unity",mutedText);
        Frame(new Rect(50,235,430,545),panel);
        Text(new Rect(74,258,380,30),"WARUNKI TESTU",eyebrow);
        Text(new Rect(74,308,210,27),"LICZBA WIEŻ",body);
        GUI.enabled=!running;
        if(Click(new Rect(295,300,45,38),"−"))simTowerCount=Math.Max(1,simTowerCount-1);
        Text(new Rect(355,307,45,30),simTowerCount.ToString(),body);
        if(Click(new Rect(406,300,45,38),"+"))simTowerCount=Math.Min(50,simTowerCount+1);
        Text(new Rect(74,361,210,27),"PRÓBY NA SKŁAD",body);
        if(Click(new Rect(295,353,45,38),"−"))simTrials=Math.Max(1,simTrials-1);
        Text(new Rect(355,360,45,30),simTrials.ToString(),body);
        if(Click(new Rect(406,353,45,38),"+"))simTrials=Math.Min(30,simTrials+1);
        Text(new Rect(74,414,210,27),"ROZMIESZCZENIE",body);
        if(Click(new Rect(250,408,200,38),SimLayouts[simLayoutIndex].ToUpperInvariant()))simLayoutIndex=(simLayoutIndex+1)%SimLayouts.Length;
        Text(new Rect(74,467,210,27),"ATAK PODSTAWOWY",body);
        if(Click(new Rect(295,459,155,38),simBasicAttack?"WŁĄCZONY":"WYŁĄCZONY"))simBasicAttack=!simBasicAttack;
        Text(new Rect(74,514,370,54),"4368 składów · seed 100\nCzas rośnie wraz z liczbą prób.",mutedText);
        if(Click(new Rect(74,595,376,50),"URUCHOM ANALIZĘ  →",true))StartSimulation();
        GUI.enabled=true;
        if(running&&Click(new Rect(74,659,376,44),"PRZERWIJ"))simCancellation?.Cancel();
        Frame(new Rect(500,235,1050,545),panelAlt);
        Text(new Rect(525,258,950,28),"WYNIKI  /  NAJNIŻSZY WPŁYW NA SOJUSZNIKÓW NA GÓRZE",eyebrow);
        if(running){float fraction=simWorker.TotalTeams>0?simWorker.Completed/(float)simWorker.TotalTeams:0;
            Text(new Rect(525,324,950,35),$"Obliczanie składów: {simWorker.Completed} / {simWorker.TotalTeams}  ·  {fraction*100:F1}%",body);
            Fill(new Rect(525,375,960,15),line);Fill(new Rect(525,375,960*fraction,15),gold);
            Text(new Rect(525,415,930,65),"Porównujemy każdą postać z zamiennikami w tych samych czwórkach. Możesz przerwać obliczenia.",mutedText);
        } else if(!string.IsNullOrEmpty(simError))Text(new Rect(525,322,950,80),simError,body);
        else if(simResults==null)Text(new Rect(525,322,950,90),"Ustaw warunki i uruchom analizę. Po zakończeniu zobaczysz dane wszystkich 16 bohaterów.",body);
        else {
            Text(new Rect(525,298,950,30),simResultSettings,mutedText);
            Text(new Rect(532,341,335,28),"BOHATER",heroBadge);
            Text(new Rect(865,341,220,28),"WPŁYW  ·  pp",heroBadge);
            Text(new Rect(1085,341,205,28),"SOJUSZNICY",heroBadge);
            Text(new Rect(1290,341,205,28),"WŁASNA META",heroBadge);
            simScroll=GUI.BeginScrollView(new Rect(520,375,1000,345),simScroll,new Rect(0,0,970,simResults.Count*42));
            for(int i=0;i<simResults.Count;i++){var r=simResults[i];float y=i*42;if(i%2==0)Fill(new Rect(0,y,968,40),new Color(.10f,.08f,.14f));
                Text(new Rect(12,y+8,315,28),$"{i+1:00}  {r.name}",body);
                Text(new Rect(345,y+8,200,28),$"{r.delta:+0.00;-0.00;0.00} pp",body);
                Text(new Rect(565,y+8,190,28),$"{r.allies:F1}%",body);
                Text(new Rect(770,y+8,180,28),$"{r.self:F1}%",body);
            }
            GUI.EndScrollView();
            Text(new Rect(525,730,980,40),"Wpływ = różnica przeżycia pozostałych 4 wobec średniej zastępstw. To nie ranking siły w grze.",mutedText);
        }
    }
}
