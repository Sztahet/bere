using System;
using UnityEngine;

[Serializable] public class HeroTraitProfile { public string id, explanation; public int[] scores; public bool[] tentative; }
[Serializable] public class HeroTraitCatalog { public HeroTraitProfile[] profiles; }
[Serializable] public class HeroCardFact { public string id, age, home, occupation, side; }
[Serializable] public class HeroCardFacts { public HeroCardFact[] heroes; }

public partial class RiftorynMenu {
    bool profileOpen;
    Vector2 profileScroll;
    HeroTraitCatalog traitCatalog;
    HeroCardFacts cardFacts;
    bool profileDataLoaded;
    static readonly string[] traitNames={"SIŁA","ZRĘCZNOŚĆ","WYTRZYMAŁOŚĆ","ROZEZNANIE","WPŁYW","MAGIA OSOBISTA","TECHNOLOGIA"};

    void LoadProfileData(){
        if(profileDataLoaded)return;
        profileDataLoaded=true;
        var traits=Resources.Load<TextAsset>("hero_traits");
        var facts=Resources.Load<TextAsset>("hero_card_facts");
        if(traits)traitCatalog=JsonUtility.FromJson<HeroTraitCatalog>(traits.text);
        if(facts)cardFacts=JsonUtility.FromJson<HeroCardFacts>(facts.text);
    }
    HeroTraitProfile FindTraits(string id){if(traitCatalog?.profiles!=null)foreach(var item in traitCatalog.profiles)if(item.id==id)return item;return null;}
    HeroCardFact FindFacts(string id){if(cardFacts?.heroes!=null)foreach(var item in cardFacts.heroes)if(item.id==id)return item;return null;}
    void HeroProfile(){
        if(heroes.Length==0){profileOpen=false;return;}
        LoadProfileData();
        var hero=heroes[Mathf.Clamp(selected,0,heroes.Length-1)];
        var facts=FindFacts(hero.id);var traits=FindTraits(hero.id);
        Text(new Rect(52,96,1000,25),"KODEKS  /  KARTA POSTACI",eyebrow);
        if(Click(new Rect(1290,92,260,42),"← DO LISTY BOHATERÓW")){profileOpen=false;return;}
        Frame(new Rect(50,150,440,690),panel);
        Picture(new Rect(51,151,438,410),Portrait(hero.id));
        Fill(new Rect(51,480,438,81),new Color(.07f,.055f,.10f,.88f));
        Text(new Rect(75,490,390,24),hero.role+"  /  "+hero.element,heroBadge);
        Text(new Rect(75,516,390,40),hero.name.ToUpperInvariant(),detailTitle);
        Text(new Rect(75,582,375,30),"WIEK W 12 ET",detailLabel);
        Text(new Rect(75,606,375,35),facts?.age??"Nieustalony",body);
        Text(new Rect(75,650,375,30),"STRONA IGRZYSK",detailLabel);
        Text(new Rect(75,676,375,70),facts?.side??"Nieustalona",body);
        Frame(new Rect(510,150,1040,690),panel);
        var view=new Rect(530,168,1000,654);
        float skillHeight=ProfileSkillHeight(hero.passName,hero.pass)+ProfileSkillHeight(hero.attackName,hero.attack)+ProfileSkillHeight(hero.defName,hero.def);
        if(string.IsNullOrEmpty(hero.pass)&&string.IsNullOrEmpty(hero.attack)&&string.IsNullOrEmpty(hero.def))skillHeight+=ProfileSkillHeight("W PRZYGOTOWANIU",hero.concept??hero.desc);
        profileScroll=GUI.BeginScrollView(view,profileScroll,new Rect(0,0,970,Mathf.Max(654,650+skillHeight)));
        Text(new Rect(8,4,910,30),"KIM JEST",eyebrow);
        Text(new Rect(8,38,220,26),"MIEJSCE",detailLabel);
        Text(new Rect(8,64,420,50),facts?.home??hero.bio?.city??"Nieustalone",body);
        Text(new Rect(475,38,220,26),"ZAJĘCIE",detailLabel);
        Text(new Rect(475,64,450,70),facts?.occupation??hero.desc??"W przygotowaniu",body);
        Fill(new Rect(8,140,930,1),line);
        Text(new Rect(8,158,580,28),"SPLOT CECH  /  SKALA 1–5",eyebrow);
        Text(new Rect(610,158,320,28),"21 PUNKTÓW · PROFIL ROBOCZY",heroBadge);
        if(traits?.scores!=null&&traits.scores.Length==7){
            for(int i=0;i<7;i++){
                float y=200+i*43;int score=traits.scores[i];
                Text(new Rect(8,y,230,26),traitNames[i],detailLabel);
                for(int j=0;j<5;j++)Fill(new Rect(250+j*43,y+3,34,12),j<score?gold:line);
                bool tentative=traits.tentative!=null&&traits.tentative.Length>i&&traits.tentative[i];
                Text(new Rect(485,y,75,26),score+(tentative?"?":"")+" / 5",body);
            }
            Text(new Rect(8,506,900,48),"?  Ocena robocza — wymaga potwierdzenia w historii. Cechy opisują możliwości w świecie, a nie statystyki meczu.",mutedText);
        } else Text(new Rect(8,208,900,55),"Profil cech nie został jeszcze opracowany.",mutedText);
        Fill(new Rect(8,568,930,1),line);
        Text(new Rect(8,586,700,28),"UMIEJĘTNOŚCI I STYL GRY",eyebrow);
        float ySkill=625;
        ProfileSkill(ref ySkill,hero.passName,hero.pass);
        ProfileSkill(ref ySkill,hero.attackName,hero.attack);
        ProfileSkill(ref ySkill,hero.defName,hero.def);
        if(string.IsNullOrEmpty(hero.pass)&&string.IsNullOrEmpty(hero.attack)&&string.IsNullOrEmpty(hero.def))ProfileSkill(ref ySkill,"W PRZYGOTOWANIU",hero.concept??hero.desc);
        GUI.EndScrollView();
    }
    float ProfileSkillHeight(string name,string description){
        if(string.IsNullOrEmpty(name)&&string.IsNullOrEmpty(description))return 0;
        return 40+Mathf.Max(38,body.CalcHeight(new GUIContent(description??"W przygotowaniu"),900));
    }
    void ProfileSkill(ref float y,string name,string description){
        if(string.IsNullOrEmpty(name)&&string.IsNullOrEmpty(description))return;
        Text(new Rect(8,y,900,26),name??"",detailLabel);y+=26;
        var value=description??"W przygotowaniu";
        float height=Mathf.Max(38,body.CalcHeight(new GUIContent(value),900));
        Text(new Rect(8,y,900,height),value,body);y+=height+14;
    }
}
