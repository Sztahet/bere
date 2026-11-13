using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class HeroBiography { public string city,cityEn,specialty,story,storyEn,reason,family; public bool approvedForPoc; }
[Serializable] public class CodexHero {
    public string id,name,role,element,img,speed,desc,passName,pass,passNameEn,passEn,attackName,attack,attackNameEn,attackEn,defName,def,defNameEn,defEn,pet,concept,defense;
    public int hp,cd; public bool conceptOnly; public HeroBiography bio;
}
[Serializable] public class ComicRect { public float x,y,width,height; }
[Serializable] public class ComicPanel : ComicRect { public string pl,en; }
[Serializable] public class HeroComic { public string heroId,resource,titlePl,titleEn; public ComicRect titleRect; public ComicPanel[] panels; }
[Serializable] public class HeroComicCatalog { public HeroComic[] comics; }
[Serializable] public class CodexRoleBonus { public string role,icon,name,description,nameEn,descriptionEn; }
[Serializable] public class CodexRoster { public CodexHero[] heroes; public CodexRoleBonus[] roleBonuses; }
[Serializable] public class CodexTimelineEvent { public string date,dateEn,titlePl,titleEn,textPl,textEn; }
[Serializable] public class CodexTimelineEra { public string range,rangeEn,labelPl,labelEn,image,contextPl; public CodexTimelineEvent[] events; }
[Serializable] public class CodexTimelineCatalog { public CodexTimelineEra[] eras; }
public static class RiftorynCodexData {
    public static readonly string[] Elements={"Ogień","Lód","Błyskawice","Natura","Ziemia","Światło","Ciemność"};
    static readonly string[] MajorFrom={"Ogień","Lód","Błyskawice","Natura","Ziemia","Światło","Ciemność"}, MajorTo={"Natura","Światło","Lód","Ziemia","Błyskawice","Ciemność","Ogień"};
    static readonly string[] MinorFrom={"Ogień","Lód","Błyskawice","Natura","Ziemia","Światło","Ciemność"}, MinorTo={"Lód","Natura","Ogień","Światło","Ciemność","Ziemia","Błyskawice"};
    public static int Resistance(string heroElement,string attackElement,bool isTank){
        int value=heroElement==attackElement?0:0;
        for(int i=0;i<MajorFrom.Length;i++){if(MajorFrom[i]==heroElement&&MajorTo[i]==attackElement)value=10;if(MajorFrom[i]==attackElement&&MajorTo[i]==heroElement)value=-10;}
        for(int i=0;i<MinorFrom.Length;i++){if(MinorFrom[i]==heroElement&&MinorTo[i]==attackElement)value=5;if(MinorFrom[i]==attackElement&&MinorTo[i]==heroElement)value=-5;}
        return value+(isTank?5:0);
    }
    public static int Resistance(CodexHero hero,string attackElement){
        return Resistance(hero.element,attackElement,hero.role=="Tank")+(hero.id=="lucen"&&attackElement=="Błyskawice"?25:0);
    }
    public static string ResistanceDescription(CodexHero hero){
        var rows=new List<string>();
        foreach(var element in Elements){int value=Resistance(hero,element);rows.Add(RiftorynL10n.Translate(element)+" "+(value>0?"+":"")+value+"%");}
        return string.Join("\n",rows);
    }
    public static CodexRoleBonus RoleBonus(CodexRoleBonus[] bonuses,string role){if(bonuses!=null)foreach(var bonus in bonuses)if(bonus!=null&&bonus.role==role)return bonus;return null;}
    public static string RoleIcon(CodexRoleBonus[] bonuses,string role)=>RoleBonus(bonuses,role)?.icon??role.ToLowerInvariant();
    public static string Text(string polish,string english)=>RiftorynL10n.CurrentCode=="EN"&&!string.IsNullOrEmpty(english)?english:RiftorynL10n.Translate(polish??"");
}

public partial class RiftorynMenu : MonoBehaviour {
    RiftorynTrial trial; CodexHero[] heroes; Texture2D white; readonly Dictionary<string,Texture2D> portraits=new();
    int page, tab, selected,selectedComic,timelineSelected; bool storyOpen; float storyZoom=1f; Vector2 scroll,detailScroll,storyScroll,comicListScroll,timelineScroll,timelineDetailScroll; readonly Dictionary<string,Texture2D> timelineImages=new(); CodexRoleBonus[] roleBonuses=Array.Empty<CodexRoleBonus>(); HeroComic[] comics=Array.Empty<HeroComic>(); CodexTimelineEra[] timelineEras=Array.Empty<CodexTimelineEra>(); GUIStyle comicCaption,comicTitle,timelineDate,timelineBody; string[] tabs={"BOHATEROWIE","WIEŻE","STATUSY","ŻYWIOŁY","MAPA","ZASADY","ŚWIAT","DRZEWKO","OŚ CZASU"};
    Color bg=new(.045f,.038f,.075f), panel=new(.10f,.085f,.14f), panelAlt=new(.13f,.11f,.18f), line=new(.29f,.25f,.34f), gold=new(.87f,.72f,.46f), muted=new(.68f,.64f,.72f), pale=new(.96f,.92f,.88f);
    GUIStyle brand,nav,title,section,eyebrow,body,mutedText,button,cardName,detailTitle,detailLabel,heroBadge,resistanceNameStyle,resistanceValueStyle,compactButton,compactHeading,compactText;
    void Awake(){trial=GetComponent<RiftorynTrial>();var data=Resources.Load<TextAsset>("codex");if(data){var roster=JsonUtility.FromJson<CodexRoster>(data.text);heroes=roster?.heroes??Array.Empty<CodexHero>();roleBonuses=roster?.roleBonuses??Array.Empty<CodexRoleBonus>();}else heroes=Array.Empty<CodexHero>();var comicData=Resources.Load<TextAsset>("comic_stories");if(comicData){var catalog=JsonUtility.FromJson<HeroComicCatalog>(comicData.text);comics=catalog?.comics??Array.Empty<HeroComic>();}var timelineData=Resources.Load<TextAsset>("codex_timeline");if(timelineData){var catalog=JsonUtility.FromJson<CodexTimelineCatalog>(timelineData.text);timelineEras=catalog?.eras??Array.Empty<CodexTimelineEra>();}white=Texture2D.whiteTexture;}
    void Init(){if(title!=null)return;brand=Style(25,FontStyle.Bold,gold);nav=Style(14,FontStyle.Bold,muted);title=Style(46,FontStyle.Bold,pale);section=Style(25,FontStyle.Bold,pale);eyebrow=Style(13,FontStyle.Bold,gold);body=Style(16,FontStyle.Normal,pale,true);mutedText=Style(15,FontStyle.Normal,muted,true);button=Style(15,FontStyle.Bold,new Color(.08f,.06f,.09f));button.alignment=TextAnchor.MiddleCenter;cardName=Style(14,FontStyle.Bold,Color.white);detailTitle=Style(29,FontStyle.Bold,pale);detailLabel=Style(13,FontStyle.Bold,gold,true);heroBadge=Style(12,FontStyle.Bold,gold);resistanceNameStyle=Style(10,FontStyle.Bold,muted);resistanceValueStyle=Style(12,FontStyle.Bold,pale);timelineDate=Style(12,FontStyle.Bold,gold);timelineBody=Style(18,FontStyle.Normal,pale,true);}
    GUIStyle Style(int size,FontStyle style,Color color,bool wrap=false){return new GUIStyle(GUI.skin.label){fontSize=size,fontStyle=style,wordWrap=wrap,normal={textColor=color},alignment=TextAnchor.UpperLeft,clipping=TextClipping.Clip};}
    void Fill(Rect r,Color c){Color old=GUI.color;GUI.color=c;GUI.DrawTexture(r,white);GUI.color=old;}
    void Frame(Rect r,Color fill){Fill(r,line);Fill(new Rect(r.x+1,r.y+1,r.width-2,r.height-2),fill);}
    bool Click(Rect r,string text, bool primary=false){Color old=GUI.color;Fill(r,primary?gold:new Color(.16f,.135f,.20f));var st=new GUIStyle(primary?button:nav){alignment=TextAnchor.MiddleCenter};if(!primary)st.normal.textColor=pale;bool result=RiftorynL10n.Button(r,text,st);GUI.color=old;return result;}
    void Text(Rect rect,string value,GUIStyle style){RiftorynL10n.Label(rect,value??"",style);}
    Texture2D Portrait(string id){if(string.IsNullOrEmpty(id))return null;if(!portraits.TryGetValue(id,out var tex)){tex=Resources.Load<Texture2D>("Portraits/"+id);portraits[id]=tex;}return tex;}
    void Picture(Rect rect,Texture2D texture){if(texture)GUI.DrawTexture(rect,texture,ScaleMode.ScaleAndCrop);else Fill(rect,panelAlt);}
    void PictureHead(Rect rect,Texture2D texture){if(!texture){Fill(rect,panelAlt);return;}float uvHeight=Mathf.Clamp01(((float)texture.width/texture.height)/(rect.width/rect.height));float bottom=Mathf.Clamp(.90f-uvHeight,0,1-uvHeight);GUI.DrawTextureWithTexCoords(rect,texture,new Rect(0,bottom,1,uvHeight));}
    void OnGUI(){if(trial==null||!trial.menuOpen)return;Init();if(sailaReaderOpen){DrawSailaReader();return;}Matrix4x4 old=GUI.matrix;if(page==2&&tab==0&&storyOpen&&(Screen.width<1000||Screen.height<600)){GUI.matrix=Matrix4x4.identity;HeroStoryCompact();GUI.matrix=old;return;}float scale=Mathf.Min(Screen.width/1600f,Screen.height/900f);if(scale<=0)return;GUI.matrix=Matrix4x4.TRS(new Vector3((Screen.width-1600*scale)/2,(Screen.height-900*scale)/2,0),Quaternion.identity,new Vector3(scale,scale,1));Fill(new Rect(0,0,1600,900),bg);Top();if(page==0)Home();else if(page==1)Modes();else if(tab==0&&storyOpen)HeroStory();else Codex();GUI.matrix=old;}
    void Top(){Fill(new Rect(0,0,1600,70),new Color(.065f,.055f,.10f));Fill(new Rect(0,69,1600,1),line);Text(new Rect(52,20,230,36),"RIFTORYN",brand);string[] labels={"MENU","GRAJ","KODEKS"};for(int i=0;i<3;i++){var n=new GUIStyle(nav);n.normal.textColor=page==i?gold:muted;if(RiftorynL10n.Button(new Rect(310+i*116,23,104,32),labels[i],n)){page=i;storyOpen=false;scroll=Vector2.zero;detailScroll=Vector2.zero;}}Text(new Rect(1270,24,220,25),"PRE-POC  ·  v0.2.2  ·  OFFLINE",heroBadge);if(GUI.Button(new Rect(1500,20,70,32),RiftorynL10n.CurrentCode+" ⇄"))RiftorynL10n.Toggle();}
    void Home(){Frame(new Rect(48,104,1504,694),panel);Picture(new Rect(832,105,719,692),Portrait("morrak"));Fill(new Rect(49,105,885,692),new Color(.095f,.08f,.13f,.97f));Fill(new Rect(828,105,106,692),new Color(.095f,.08f,.13f,.56f));Text(new Rect(110,175,600,25),"RIFTORYN  /  PIERWSZA PRÓBA",eyebrow);Text(new Rect(106,229,690,180),"BUDUJ.\nUCIEKAJ.\nADAPTUJ SIĘ.",title);Text(new Rect(110,430,590,70),"Wejdź do labiryntu, poznaj bohaterów i sprawdź swoje umiejętności pod ostrzałem wież.",mutedText);if(Click(new Rect(110,530,215,55),"ROZPOCZNIJ PRÓBĘ  →",true))trial.OpenTraining();if(Click(new Rect(340,530,205,55),"ODKRYJ KODEKS  ↗")){page=2;tab=0;}Fill(new Rect(110,660,580,1),line);Text(new Rect(110,683,240,24),"31 BOHATERÓW",eyebrow);Text(new Rect(110,710,260,40),"Historie, role i umiejętności",mutedText);Text(new Rect(390,683,260,24),"PIERWSZA PRÓBA 3D",eyebrow);Text(new Rect(390,710,290,40),"Trzy grywalne postacie",mutedText);Text(new Rect(50,830,1400,35),"Prototyp koncepcji. Zasady, opisy, oprawa i balans pozostają do testów.",mutedText);}
    void Modes(){Text(new Rect(50,110,500,22),"PRZYGOTUJ SIĘ  /  TRYBY GRY",eyebrow);Text(new Rect(48,145,700,55),"WYBIERZ SWOJĄ PRÓBĘ",section);Text(new Rect(50,194,1000,30),"Przygotuj bohatera i poznaj labirynt przed właściwą rozgrywką.",mutedText);ModeCard(new Rect(50,252,730,390),"DOSTĘPNE  /  PRE-POC","PRÓBA BOHATERA","Wybierz bohatera, przygotuj automatyczne wieże i przejdź spiralny labirynt sam lub z sojusznikiem AI.",true);ModeCard(new Rect(800,252,750,390),"W PRZYGOTOWANIU","MECZ DRUŻYNOWY","W przyszłości dwie drużyny zbudują obronę i zmierzą się z labiryntem przeciwnika.",false);}
    void ModeCard(Rect r,string badge,string name,string description,bool available){Frame(r,panel);Text(new Rect(r.x+32,r.y+32,r.width-64,26),badge,eyebrow);Text(new Rect(r.x+30,r.y+100,r.width-60,45),name,section);Text(new Rect(r.x+32,r.y+165,r.width-64,85),description,mutedText);if(available){if(Click(new Rect(r.x+32,r.y+290,250,52),"PRZEJDŹ DO PRÓBY  →",true))trial.OpenTraining();}else{Text(new Rect(r.x+32,r.y+307,300,30),"WKRÓTCE",heroBadge);}}
    void Codex()
    {
        Text(new Rect(50,100,900,22),RiftorynCodexData.Text("ARCHIWUM RIFTORYN  /  ENCYKLOPEDIA","RIFTORYN ARCHIVE  /  ENCYCLOPEDIA"),eyebrow);
        Text(new Rect(48,130,700,42),RiftorynCodexData.Text("ODKRYJ ŚWIAT RIFTORYN","DISCOVER THE WORLD OF RIFTORYN"),section);
        Text(new Rect(50,170,1300,25),RiftorynCodexData.Text("Bohaterowie, świat i zasady — wszystkie historie w jednym miejscu.","Heroes, world and rules — every story in one place."),mutedText);
        float x=50;
        for(int i=0;i<tabs.Length;i++)
        {
            float width=i==0?190:i==8?145:i==3?145:115;
            Rect r=new(x,211,width,39);bool active=tab==i;
            Fill(r,active?new Color(.19f,.16f,.22f):new Color(.09f,.075f,.13f));
            if(active)Fill(new Rect(r.x,r.yMax-3,r.width,3),gold);
            var style=new GUIStyle(nav){alignment=TextAnchor.MiddleCenter};style.normal.textColor=active?gold:muted;
            string label=i==8?RiftorynCodexData.Text("OŚ CZASU","TIMELINE"):RiftorynL10n.Translate(tabs[i]);
            if(GUI.Button(r,label,style)){tab=i;scroll=Vector2.zero;detailScroll=Vector2.zero;}
            x+=width+8;
        }
        if(tab==0)HeroCodex();else if(tab==3)ElementEncyclopedia();else if(tab==6)WorldEncyclopedia();else if(tab==7)TreeEncyclopedia();else if(tab==8)TimelineCodex();else Catalog();
    }
    string StoryButtonLabel(string id)
    {
        switch(id)
        {
            case "kael": case "thora": case "niva": case "varek": case "liora": case "dara": case "elira": case "rian": case "saila": case "kiran": case "pyrra": case "ana": case "lumen": case "koneva": case "neris":
                return RiftorynCodexData.Text("JEJ HISTORIA  →","HER HISTORY  →");
            case "brom": case "iskar": case "taren": case "orien": case "solan": case "nocten": case "zet": case "morven": case "miko": case "eron": case "adrien": case "eira": case "volt": case "rokan":
                return RiftorynCodexData.Text("JEGO HISTORIA  →","HIS HISTORY  →");
            default: return RiftorynCodexData.Text("HISTORIA  →","THEIR HISTORY  →");
        }
    }
    void HeroCodex()
    {
        Frame(new Rect(50,270,1020,570),panel);Frame(new Rect(1085,270,465,570),panel);
        Text(new Rect(72,287,600,26),"BOHATEROWIE",eyebrow);Text(new Rect(770,287,270,26),RiftorynL10n.Format("{0} POSTACI  /  KONCEPCJE",heroes.Length),heroBadge);DrawHeroGrid();
        if(heroes.Length==0)return;selected=Mathf.Clamp(selected,0,heroes.Length-1);var hero=heroes[selected];
        PictureHead(new Rect(1086,271,463,200),Portrait(hero.id));Fill(new Rect(1086,404,463,67),new Color(.07f,.055f,.10f,.84f));
        Text(new Rect(1108,414,420,25),$"{RiftorynL10n.Translate(hero.role)}  /  {RiftorynL10n.Translate(hero.element)}",heroBadge);Text(new Rect(1107,438,420,36),hero.name.ToUpperInvariant(),detailTitle);
        bool small=Screen.width<1000||Screen.height<600;Rect entry=new(1105,483,425,small?64:42);bool open;
        if(small){Fill(entry,gold);var entryStyle=new GUIStyle(button){fontSize=23,alignment=TextAnchor.MiddleCenter};open=GUI.Button(entry,StoryButtonLabel(hero.id),entryStyle);}else open=Click(entry,StoryButtonLabel(hero.id),true);
        if(open){storyOpen=true;selectedComic=0;storyZoom=1f;storyScroll=Vector2.zero;comicListScroll=Vector2.zero;}
        if(hero.id=="saila"){if(Click(new Rect(1105,small?775:755,425,32),"CIĘŻAR KRYSZTAŁU  →",true))OpenSailaStory(false);if(Click(new Rect(1105,small?812:792,425,32),"GDY WIEŻA ZAMILKŁA  →",true))OpenSailaStory(true);}
        Rect detail=new(1105,small?557:535,425,hero.id=="saila"?(small?210:210):(small?263:285));detailScroll=GUI.BeginScrollView(detail,detailScroll,new Rect(0,0,395,900));float y=0;
        Text(new Rect(0,y,380,56),hero.desc,mutedText);y+=62;
        if(hero.hp>0){Text(new Rect(0,y,380,26),RiftorynL10n.Format("HP  {0}     PRĘDKOŚĆ  {1}     ODNOWIENIE  {2} s",hero.hp,RiftorynL10n.Number(hero.speed),hero.cd),heroBadge);y+=42;}
        DrawPassives(hero,ref y);
        if(hero.hp>0){Detail(ref y,RiftorynCodexData.Text(hero.attackName,hero.attackNameEn),RiftorynCodexData.Text(hero.attack,hero.attackEn));Detail(ref y,RiftorynCodexData.Text(hero.defName,hero.defNameEn),RiftorynCodexData.Text(hero.def,hero.defEn));}
        else{if(!string.IsNullOrEmpty(hero.pet))Detail(ref y,"TOWARZYSZ",hero.pet);Detail(ref y,"KONCEPCJA",hero.concept);if(!string.IsNullOrEmpty(hero.defense))Detail(ref y,"OBRONA",hero.defense);Text(new Rect(0,y,375,48),"Statystyki i umiejętności tej postaci pozostają w przygotowaniu.",mutedText);y+=55;}
        Text(new Rect(0,y,380,48),IsPlayable(hero.id)?"DOSTĘPNA W PRÓBIE 3D":"POSTAĆ W KODEKSIE  ·  NIEGRYWALNA W PRÓBIE",heroBadge);GUI.EndScrollView();
    }
    List<HeroComic> ComicsFor(string heroId)
    {
        var list=new List<HeroComic>();foreach(var item in comics)if(item!=null&&item.heroId==heroId)list.Add(item);return list;
    }
    string ComicText(string pl,string en)=>RiftorynL10n.CurrentCode=="EN"&&!string.IsNullOrEmpty(en)?en:pl??"";
    string BiographyStatus(CodexHero hero)=>hero.bio.approvedForPoc?RiftorynCodexData.Text("HISTORIA ZATWIERDZONA NA POC","STORY APPROVED FOR POC"):RiftorynCodexData.Text("PROPOZYCJA FABULARNA · DO AKCEPTACJI","STORY CONCEPT · PENDING APPROVAL");
    static Rect ComicArea(Rect image,ComicRect area)=>new(image.x+image.width*area.x,image.y+image.height*area.y,image.width*area.width,image.height*area.height);
    void DrawComicText(Rect rect,string value,bool titleText,float imageScale)
    {
        if(string.IsNullOrEmpty(value))return;
        var style=titleText?(comicTitle??=new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter,fontStyle=FontStyle.Bold,wordWrap=true,normal={textColor=new Color(.24f,.12f,.07f)}}):(comicCaption??=new GUIStyle(GUI.skin.label){alignment=TextAnchor.MiddleCenter,fontStyle=FontStyle.Bold,wordWrap=true,normal={textColor=new Color(.24f,.12f,.07f)}});
        style.fontSize=Mathf.Max(10,Mathf.RoundToInt((titleText?23:18)*imageScale));var content=new GUIContent(value);
        while(style.fontSize>10&&(style.CalcHeight(content,rect.width)>rect.height||style.CalcSize(content).x>rect.width*1.9f))style.fontSize--;
        GUI.Label(rect,content,style);
    }
    void ChangeStoryZoom(float requested,float centerX=545,float centerY=277)
    {
        float next=Mathf.Clamp(requested,1f,2f);if(Mathf.Approximately(next,storyZoom))return;
        float ratio=next/storyZoom;
        // At the top/left, preserve the beginning of the page instead of cutting both columns.
        float x=storyScroll.x<1f?0:Mathf.Max(0,(storyScroll.x+centerX)*ratio-centerX);
        float y=storyScroll.y<1f?0:Mathf.Max(0,(storyScroll.y+centerY)*ratio-centerY);
        storyScroll=new Vector2(x,y);storyZoom=next;
    }
    bool CompactStoryButton(Rect rect,string text,bool enabled=true)
    {
        if(compactButton==null){
            compactButton=new GUIStyle(GUI.skin.button){fontSize=16,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter,wordWrap=false};
            compactButton.normal.textColor=pale;compactButton.hover.textColor=pale;compactButton.active.textColor=pale;compactButton.focused.textColor=pale;
        }
        Fill(rect,enabled?panelAlt:new Color(.10f,.085f,.14f));bool old=GUI.enabled;GUI.enabled=enabled;
        bool pressed=GUI.Button(rect,text,compactButton);GUI.enabled=old;return pressed;
    }
    void HeroStoryCompact()
    {
        if(heroes.Length==0){storyOpen=false;return;}
        if(compactHeading==null)compactHeading=new GUIStyle(GUI.skin.label){fontSize=19,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleLeft,normal={textColor=pale}};
        if(compactText==null)compactText=new GUIStyle(GUI.skin.label){fontSize=16,wordWrap=true,normal={textColor=pale}};
        float w=Screen.width,h=Screen.height;Fill(new Rect(0,0,w,h),bg);var hero=heroes[Mathf.Clamp(selected,0,heroes.Length-1)];var list=ComicsFor(hero.id);
        selectedComic=list.Count>0?Mathf.Clamp(selectedComic,0,list.Count-1):0;
        HeroComic comic=list.Count>0?list[selectedComic]:null;
        GUI.Label(new Rect(10,4,Mathf.Max(70,w-205),40),hero.name.ToUpperInvariant(),compactHeading);
        if(CompactStoryButton(new Rect(w-190,4,116,40),RiftorynCodexData.Text("← KARTA","← HERO"))){storyOpen=false;return;}
        if(CompactStoryButton(new Rect(w-68,4,60,40),RiftorynL10n.CurrentCode))RiftorynL10n.Toggle();
        float viewportTop=52;
        if(comic!=null)
        {
            bool wrap=w<520;float navY=wrap?94:50,navX=wrap?8:235;
            if(CompactStoryButton(new Rect(8,50,42,38),"−",storyZoom>1))ChangeStoryZoom(storyZoom-.25f,(w-16)/2,(h-98)/2);
            GUI.Label(new Rect(55,50,58,38),Mathf.RoundToInt(storyZoom*100)+"%",compactHeading);
            if(CompactStoryButton(new Rect(112,50,42,38),"+",storyZoom<2))ChangeStoryZoom(storyZoom+.25f,(w-16)/2,(h-98)/2);
            if(CompactStoryButton(new Rect(160,50,65,38),"100%"))ChangeStoryZoom(1f,(w-16)/2,(h-98)/2);
            if(CompactStoryButton(new Rect(navX,navY,38,38),"<",selectedComic>0)){selectedComic--;storyScroll=Vector2.zero;}
            GUI.Label(new Rect(navX+45,navY,66,38),(selectedComic+1)+" / "+list.Count,compactHeading);
            if(CompactStoryButton(new Rect(navX+115,navY,38,38),">",selectedComic<list.Count-1)){selectedComic++;storyScroll=Vector2.zero;}
            float panY=wrap?94:50;
            if(CompactStoryButton(new Rect(w-98,panY,42,38),"<",storyZoom>1))storyScroll.x=Mathf.Max(0,storyScroll.x-(w-36)*.7f);
            if(CompactStoryButton(new Rect(w-50,panY,42,38),">",storyZoom>1))storyScroll.x+=(w-36)*.7f;
            viewportTop=wrap?138:96;
        }
        Rect viewport=new(8,viewportTop,Mathf.Max(1,w-16),Mathf.Max(1,h-viewportTop-8));float width=Mathf.Max(100,w-50)*(comic!=null?storyZoom:1f);
        Texture2D image=comic!=null?Resources.Load<Texture2D>(comic.resource):null;float imageHeight=image?width*image.height/image.width:0;
        string biography=HasBiography(hero)?ComicText(hero.bio.story,hero.bio.storyEn):RiftorynCodexData.Text("Historia jest w przygotowaniu.","This story is in development.");
        float bioHeight=Mathf.Max(80,compactText.CalcHeight(new GUIContent(biography),width-20));float total=(image?imageHeight+55:90)+bioHeight+45;
        storyScroll=GUI.BeginScrollView(viewport,storyScroll,new Rect(0,0,width+10,total));float y=4;
        if(image){
            Rect imageRect=new(5,y,width,imageHeight);GUI.DrawTexture(imageRect,image,ScaleMode.StretchToFill);
            if(comic.titleRect!=null)DrawComicText(ComicArea(imageRect,comic.titleRect),ComicText(comic.titlePl,comic.titleEn),true,width/1000f);
            if(comic.panels!=null)foreach(var entry in comic.panels)if(entry!=null)DrawComicText(ComicArea(imageRect,entry),ComicText(entry.pl,entry.en),false,width/1000f);
            y+=imageHeight+12;
        }else{GUI.Label(new Rect(5,y,width-10,62),comic==null?RiftorynCodexData.Text("Nie dodano jeszcze komiksu tej postaci.","No comic has been added for this character yet."):RiftorynCodexData.Text("Nie udało się wczytać komiksu.","The comic could not be loaded."),compactText);y+=70;}
        if(RiftorynL10n.CurrentCode=="EN"&&HasBiography(hero)&&string.IsNullOrEmpty(hero.bio.storyEn)){GUI.Label(new Rect(5,y,width-10,25),"Polish version · English translation in development",compactText);y+=28;}
        if(HasBiography(hero)){GUI.Label(new Rect(5,y,width-10,22),BiographyStatus(hero),heroBadge);y+=24;}
        GUI.Label(new Rect(5,y,width-20,bioHeight),biography,compactText);GUI.EndScrollView();
    }
    void HeroStory()
    {
        if(heroes.Length==0){storyOpen=false;return;}
        var hero=heroes[Mathf.Clamp(selected,0,heroes.Length-1)];var list=ComicsFor(hero.id);
        Text(new Rect(50,100,1000,30),RiftorynCodexData.Text("ARCHIWUM / HISTORIA BOHATERA","ARCHIVE / HERO HISTORY"),eyebrow);
        Text(new Rect(48,135,1050,55),hero.name.ToUpperInvariant(),section);
        if(Click(new Rect(1300,122,250,45),RiftorynCodexData.Text("← DO BOHATERA","← BACK TO HERO"))) {storyOpen=false;return;}
        Frame(new Rect(50,205,350,635),panel);Text(new Rect(72,225,310,30),RiftorynCodexData.Text("KOMIKSY","COMICS"),detailLabel);
        if(list.Count==0)Text(new Rect(72,275,300,90),RiftorynCodexData.Text("Nie dodano jeszcze komiksu tej postaci. Historia znajduje się obok.","No comic has been added for this character yet. The story is alongside."),mutedText);
        else
        {
            comicListScroll=GUI.BeginScrollView(new Rect(70,270,320,550),comicListScroll,new Rect(0,0,300,Mathf.Max(550,list.Count*66)));
            for(int i=0;i<list.Count;i++){
                var c=list[i];Rect item=new(0,i*66,290,56);bool chosen=i==selectedComic;Fill(item,chosen?gold:new Color(.16f,.135f,.20f));
                var itemStyle=new GUIStyle(chosen?button:nav){alignment=TextAnchor.MiddleCenter,wordWrap=true};
                if(!chosen)itemStyle.normal.textColor=pale;
                itemStyle.hover.textColor=itemStyle.normal.textColor;itemStyle.active.textColor=itemStyle.normal.textColor;itemStyle.focused.textColor=itemStyle.normal.textColor;
                if(GUI.Button(item,ComicText(c.titlePl,c.titleEn),itemStyle)){selectedComic=i;storyScroll=Vector2.zero;}
            }
            GUI.EndScrollView();
        }
        Frame(new Rect(420,205,1130,635),panel);
        HeroComic comic=list.Count>0?list[Mathf.Clamp(selectedComic,0,list.Count-1)]:null;
        if(comic!=null){
            Text(new Rect(445,225,220,26),RiftorynCodexData.Text("POWIĘKSZENIE","ZOOM"),detailLabel);
            if(Click(new Rect(660,216,55,38),"−"))ChangeStoryZoom(storyZoom-.25f);
            GUI.Label(new Rect(725,216,80,38),Mathf.RoundToInt(storyZoom*100)+"%",new GUIStyle(body){alignment=TextAnchor.MiddleCenter});
            if(Click(new Rect(815,216,55,38),"+"))ChangeStoryZoom(storyZoom+.25f);
            if(Click(new Rect(885,216,100,38),"100%"))ChangeStoryZoom(1f);
            Text(new Rect(1000,224,500,30),RiftorynCodexData.Text("Przewijaj także w bok po powiększeniu","Scroll sideways when zoomed in"),heroBadge);
        }
        Rect viewport=new(440,comic!=null?270:220,1090,comic!=null?555:605);float width=900*(comic!=null?storyZoom:1f);
        Texture2D image=comic!=null?Resources.Load<Texture2D>(comic.resource):null;
        float imageHeight=image?width*image.height/image.width:0;
        string biography=HasBiography(hero)?ComicText(hero.bio.story,hero.bio.storyEn):RiftorynCodexData.Text("Historia jest w przygotowaniu.","This story is in development.");
        float biographyHeight=Mathf.Max(80,body.CalcHeight(new GUIContent(biography),width-40));
        float contentHeight=(image?imageHeight+65:45)+biographyHeight+60;
        storyScroll=GUI.BeginScrollView(viewport,storyScroll,new Rect(0,0,Mathf.Max(1110,width+20),contentHeight));
        float y=12;
        if(image)
        {
            float pageX=width<=1060?(1090-width)*.5f:10f;
            Rect pageRect=new(pageX,y,width,imageHeight);GUI.DrawTexture(pageRect,image,ScaleMode.ScaleToFit);
            if(comic.titleRect!=null)DrawComicText(ComicArea(pageRect,comic.titleRect),ComicText(comic.titlePl,comic.titleEn),true,width/1000f);
            if(comic.panels!=null)foreach(var entry in comic.panels)if(entry!=null)DrawComicText(ComicArea(pageRect,entry),ComicText(entry.pl,entry.en),false,width/1000f);
            y+=imageHeight+25;
        }
        else if(comic!=null){Text(new Rect(12,y,width-30,35),RiftorynCodexData.Text("Nie udało się wczytać komiksu.","The comic could not be loaded."),mutedText);y+=40;}
        Text(new Rect(15,y,width-30,30),RiftorynCodexData.Text("HISTORIA","STORY"),detailLabel);y+=32;
        if(RiftorynL10n.CurrentCode=="EN"&&HasBiography(hero)&&string.IsNullOrEmpty(hero.bio.storyEn)){
            Text(new Rect(15,y,width-30,24),"Polish version · English translation in development",heroBadge);y+=25;
        }
        if(HasBiography(hero)){Text(new Rect(15,y,width-30,22),BiographyStatus(hero),heroBadge);y+=24;}
        GUI.Label(new Rect(15,y,width-40,biographyHeight),biography,body);
        GUI.EndScrollView();
    }
    static readonly string[] GridRoles={"Tank","Wsparcie","Specjalista","Przywoływacz"};
    static readonly string[] GridElements={"Ogień","Lód","Błyskawice","Natura","Ziemia","Światło","Ciemność"};
    static readonly string[] GridElementIcons={"fire","ice","lightning","nature","earth","light","darkness"};
    static readonly Color[] GridElementColors={new(1f,.52f,.32f),new(.54f,.83f,.96f),new(.95f,.8f,.38f),new(.59f,.78f,.56f),new(.82f,.67f,.49f),new(.93f,.86f,.61f),new(.71f,.52f,.88f)};
    Texture2D Icon(string category,string id)=>Resources.Load<Texture2D>("CodexIcons/"+category+"/"+id);
    void DrawHeroGrid(){
        const float x0=160, column=127, y0=369, rowHeight=106, cardWidth=116, cardHeight=96;
        var elementLabel=new GUIStyle(heroBadge){alignment=TextAnchor.MiddleCenter,fontSize=12};
        var roleLabel=new GUIStyle(mutedText){alignment=TextAnchor.UpperCenter,fontSize=11,wordWrap=false};
        for(int col=0;col<GridElements.Length;col++){
            float x=x0+col*column;var texture=Icon("elements",GridElementIcons[col]);
            if(texture)GUI.DrawTexture(new Rect(x+41,315,34,34),texture,ScaleMode.ScaleToFit);
            elementLabel.normal.textColor=GridElementColors[col];
            Text(new Rect(x-3,348,cardWidth+6,20),GridElements[col].ToUpperInvariant(),elementLabel);
        }
        for(int row=0;row<GridRoles.Length;row++){
            float y=y0+row*rowHeight;var texture=Icon("roles",RiftorynCodexData.RoleIcon(roleBonuses,GridRoles[row]));
            if(texture)GUI.DrawTexture(new Rect(88,y+16,42,42),texture,ScaleMode.ScaleToFit);
            Text(new Rect(57,y+61,100,24),GridRoles[row],roleLabel);
            for(int col=0;col<GridElements.Length;col++){
                var matches=new List<int>();for(int i=0;i<heroes.Length;i++)if(heroes[i].role==GridRoles[row]&&heroes[i].element==GridElements[col])matches.Add(i);
                float x=x0+col*column;
                if(matches.Count==0){Frame(new Rect(x,y,cardWidth,cardHeight),panelAlt);Text(new Rect(x+22,y+38,85,25),"WKRÓTCE",heroBadge);continue;}
                float width=(cardWidth-(matches.Count-1)*4)/matches.Count;
                for(int k=0;k<matches.Count;k++)DrawGridHero(new Rect(x+k*(width+4),y,width,cardHeight),matches[k],matches.Count>1);
            }
        }
        Text(new Rect(75,810,960,20),"Rola × żywioł  ·  wybierz portret, aby poznać bohatera",mutedText);
    }
    void DrawGridHero(Rect rect,int index,bool compact){
        var hero=heroes[index];Fill(rect,index==selected?gold:line);
        Picture(new Rect(rect.x+2,rect.y+2,rect.width-4,rect.height-4),Portrait(hero.id));
        Fill(new Rect(rect.x+2,rect.y+rect.height-26,rect.width-4,24),new Color(.05f,.035f,.075f,.88f));
        var label=new GUIStyle(cardName){fontSize=compact?10:12};
        Text(new Rect(rect.x+5,rect.y+rect.height-22,rect.width-10,20),hero.name.ToUpperInvariant(),label);
        if(RiftorynL10n.Button(rect,"",GUIStyle.none)){selected=index;detailScroll=Vector2.zero;}
    }
    void Detail(ref float y,string label,string description){if(string.IsNullOrEmpty(label)&&string.IsNullOrEmpty(description))return;Text(new Rect(0,y,380,38),label,detailLabel);y+=38;var text=RiftorynL10n.Translate(description??"Szczegóły w przygotowaniu.");float height=Mathf.Max(55,mutedText.CalcHeight(new GUIContent(text),375));Text(new Rect(0,y,375,height),text,mutedText);y+=height+12;}
    void DrawPassives(CodexHero hero,ref float y){
        var bonus=RiftorynCodexData.RoleBonus(roleBonuses,hero.role);
        if(bonus!=null){var icon=Icon("roles",bonus.icon);if(icon)GUI.DrawTexture(new Rect(0,y+4,28,28),icon,ScaleMode.ScaleToFit);string label=RiftorynCodexData.Text("BONUS KLASY","CLASS BONUS")+" · "+RiftorynL10n.Translate(hero.role)+" / "+RiftorynCodexData.Text(bonus.name,bonus.nameEn);DetailWithRoleIcon(ref y,label,RiftorynCodexData.Text(bonus.description,bonus.descriptionEn));}
        if(!string.IsNullOrEmpty(hero.passName)||!string.IsNullOrEmpty(hero.pass))Detail(ref y,RiftorynCodexData.Text("PASYWKA BOHATERA","HERO PASSIVE")+" / "+RiftorynCodexData.Text(hero.passName,hero.passNameEn),RiftorynCodexData.Text(hero.pass,hero.passEn));
        DrawResistanceTable(hero,ref y);
    }
    void DrawResistanceTable(CodexHero hero,ref float y)
    {
        Text(new Rect(0,y,380,25),RiftorynCodexData.Text("ODPORNOŚCI NA ŻYWIOŁY","ELEMENTAL RESISTANCES"),detailLabel);y+=27;
        const float gap=4,cellHeight=35;float cellWidth=(375-gap*3)/4;
        for(int i=0;i<RiftorynCodexData.Elements.Length;i++)
        {
            int row=i/4,col=i%4,value=RiftorynCodexData.Resistance(hero,RiftorynCodexData.Elements[i]);
            float x=col*(cellWidth+gap),cy=y+row*(cellHeight+gap);
            Frame(new Rect(x,cy,cellWidth,cellHeight),panelAlt);
            var icon=Icon("elements",GridElementIcons[i]);if(icon)GUI.DrawTexture(new Rect(x+4,cy+3,16,16),icon,ScaleMode.ScaleToFit);
            Text(new Rect(x+22,cy+2,cellWidth-24,17),RiftorynL10n.Translate(RiftorynCodexData.Elements[i]),resistanceNameStyle);
            Color previous=GUI.color;GUI.color=value>0?new Color(.45f,.88f,.62f):value<0?new Color(1f,.46f,.43f):Color.white;
            Text(new Rect(x+22,cy+18,cellWidth-24,15),(value>0?"+":"")+value+"%",resistanceValueStyle);GUI.color=previous;
        }
        y+=cellHeight*2+gap+7;
    }
    void DetailWithRoleIcon(ref float y,string label,string description){Text(new Rect(36,y,344,38),label,detailLabel);y+=38;var text=description??"";float height=Mathf.Max(55,mutedText.CalcHeight(new GUIContent(text),375));Text(new Rect(0,y,375,height),text,mutedText);y+=height+12;}
    bool HasBiography(CodexHero hero)=>hero.bio!=null&&!string.IsNullOrEmpty(hero.bio.story);
    bool IsPlayable(string id)=>id=="kael"||id=="rian"||id=="zet"||id=="koneva"||id=="saila"||id=="pyrra";
    bool TimelineEntryAt(int index,out CodexTimelineEra era,out CodexTimelineEvent entry)
    {
        foreach(var group in timelineEras)if(group?.events!=null)foreach(var item in group.events){if(index--==0){era=group;entry=item;return true;}}
        era=null;entry=null;return false;
    }
    Texture2D TimelineImage(string path)
    {
        if(string.IsNullOrEmpty(path))return null;
        if(!timelineImages.TryGetValue(path,out var image)){image=Resources.Load<Texture2D>(path);timelineImages[path]=image;}
        return image;
    }
    void TimelineCodex()
    {
        Frame(new Rect(50,270,1500,570),panel);
        Text(new Rect(76,286,1000,26),RiftorynCodexData.Text("KRONIKI RIFTORYNU  /  OŚ CZASU","CHRONICLES OF RIFTORYN  /  TIMELINE"),eyebrow);
        Text(new Rect(76,312,1400,30),RiftorynCodexData.Text("Pięć epok. Wybierz wydarzenie, aby odkryć jego historię.","Five eras. Select an event to discover its story."),mutedText);
        int count=0;float contentHeight=12;
        foreach(var era in timelineEras)if(era?.events!=null){contentHeight+=50+era.events.Length*52;count+=era.events.Length;}
        if(count==0){Text(new Rect(76,370,1400,60),RiftorynCodexData.Text("Nie udało się wczytać osi czasu.","The timeline could not be loaded."),body);return;}
        timelineSelected=Mathf.Clamp(timelineSelected,0,count-1);
        Frame(new Rect(70,350,480,468),panelAlt);
        Frame(new Rect(562,350,918,468),new Color(.075f,.064f,.11f));
        timelineScroll=GUI.BeginScrollView(new Rect(80,360,462,448),timelineScroll,new Rect(0,0,438,contentHeight));
        float y=12;int eventIndex=0;
        foreach(var era in timelineEras)
        {
            if(era?.events==null)continue;
            Fill(new Rect(12,y+11,18,2),gold);
            Text(new Rect(40,y,385,23),RiftorynCodexData.Text(era.labelPl,era.labelEn).ToUpperInvariant(),eyebrow);
            Text(new Rect(40,y+22,380,20),RiftorynCodexData.Text(era.range,era.rangeEn),timelineDate);y+=50;
            foreach(var entry in era.events)
            {
                bool active=eventIndex==timelineSelected;
                Rect row=new(8,y,418,48);
                if(active){Fill(row,new Color(.25f,.20f,.22f));Fill(new Rect(row.x,row.y,3,row.height),gold);}
                Fill(new Rect(20,y,2,48),new Color(.42f,.34f,.35f));
                Fill(new Rect(17,y+18,8,8),active?gold:muted);
                if(GUI.Button(row,GUIContent.none,GUIStyle.none)){timelineSelected=eventIndex;timelineDetailScroll=Vector2.zero;}
                Text(new Rect(40,y+4,78,36),RiftorynCodexData.Text(entry.date,entry.dateEn),timelineDate);
                Text(new Rect(126,y+4,285,38),RiftorynCodexData.Text(entry.titlePl,entry.titleEn),cardName);
                y+=52;eventIndex++;
            }
        }
        GUI.EndScrollView();
        if(TimelineEntryAt(timelineSelected,out var selectedEra,out var selectedEntry))
        {
            Rect art=new(582,370,400,420);var image=TimelineImage(selectedEra.image);
            Fill(art,new Color(.045f,.038f,.075f));
            if(image)GUI.DrawTexture(art,image,ScaleMode.ScaleToFit);
            Fill(new Rect(1000,380,1,406),line);
            Text(new Rect(1025,375,420,40),RiftorynCodexData.Text(selectedEra.labelPl,selectedEra.labelEn).ToUpperInvariant(),eyebrow);
            Text(new Rect(1025,415,420,24),RiftorynCodexData.Text(selectedEra.range,selectedEra.rangeEn),heroBadge);
            Fill(new Rect(1025,449,410,1),line);
            Text(new Rect(1025,471,410,24),RiftorynCodexData.Text(selectedEntry.date,selectedEntry.dateEn),timelineDate);
            Text(new Rect(1025,502,410,80),RiftorynCodexData.Text(selectedEntry.titlePl,selectedEntry.titleEn),section);
            string description=RiftorynCodexData.Text(selectedEntry.textPl,selectedEntry.textEn);
            if(RiftorynL10n.CurrentCode!="EN"&&!string.IsNullOrEmpty(selectedEra.contextPl))description+="\n\n"+selectedEra.contextPl;
            float textHeight=timelineBody.CalcHeight(new GUIContent(description),390)+12;
            if(textHeight>188)
            {
                timelineDetailScroll=GUI.BeginScrollView(new Rect(1025,594,420,188),timelineDetailScroll,new Rect(0,0,390,textHeight));
                Text(new Rect(0,0,390,textHeight),description,timelineBody);GUI.EndScrollView();
            }
            else Text(new Rect(1025,594,400,188),description,timelineBody);

        }
    }
    void Catalog(){Frame(new Rect(50,270,1500,570),panel);string heading="",intro="";string[] names=Array.Empty<string>(),descs=Array.Empty<string>();switch(tab){case 1:heading="WIEŻE ŻYWIOŁÓW";intro="Wieże działają automatycznie; w docelowej grze Defenderzy mogą je także pilotować.";names=new[]{"OGIEŃ","LÓD","BŁYSKAWICE","NATURA","ZIEMIA","ŚWIATŁO","CIEMNOŚĆ"};descs=new[]{"Obszar i podpalenie. Rozwój: Magma lub Inferno.","Kontrola ruchu i spowolnienia. Rozwój: Glacier lub Ice Lance.","Szybki ostrzał i łańcuch trafień. Rozwój: Tempest lub Lancer.","Trucizna i kontrola ruchu. Rozwój: Vines lub Thorns.","Ciężkie obrażenia. Rozwój: Quake lub Boulder.","Wykrywanie i precyzyjny ostrzał. Rozwój: Beacon lub Ray.","Podatność i skoncentrowany ostrzał. Rozwój: Curse lub Void."};break;case 2:heading="STATUSY I EFEKTY";intro="Efekty zmieniają sposób, w jaki bohaterowie przechodzą przez obronę.";names=new[]{"PODPALENIE","WYCHŁODZENIE","SPLĄTANIE","TRUCIZNA","PODATNOŚĆ","WYŁĄCZENIE WIEŻY","OSŁONA"};descs=new[]{"Obrażenia Ognia rozłożone w czasie.","Spowolnienie ruchu pochodzące od Lodu.","Spowolnienie związane z Naturą; szczegóły pozostają otwarte.","Obrażenia Natury rozłożone w czasie.","Zwiększa otrzymywane obrażenia przez określony czas.","Czasowo zatrzymuje ostrzał wieży.","Pochłania obrażenia przed punktami zdrowia."};break;case 3:heading="SIEDEM ŻYWIOŁÓW";intro="Żywioły różnicują bohaterów, wieże i oddziały; szczegóły balansu podlegają testom.";names=new[]{"OGIEŃ","LÓD","BŁYSKAWICE","NATURA","ZIEMIA","ŚWIATŁO","CIEMNOŚĆ"};descs=new[]{"Żar i presja obszarowa.","Spowolnienie i kontrola.","Szybkość i energia.","Odnowa, trucizna i korzenie.","Wytrzymałość i ciężkie trafienia.","Wykrywanie i precyzja.","Zakłócenia i osłabienia."};break;case 4:heading="SERCE LABIRYNTU";intro="Pierwsza mapa to znany obu drużynom labirynt: od środka prowadzi jedna kręta trasa na zewnątrz.";names=new[]{"TRASA","MURY","MGŁA","PRÓBA 3D"};descs=new[]{"Punkt wyjścia to około 360 m drogi, bez skrótów i ślepych zaułków.","Wieże stoją na wyznaczonych polach murów; Runnerzy przemierzają drogę.","Czoło mgły porusza się wzdłuż drogi. Jej dotarcie do mety samo nie kończy ataku.","W obecnym prototypie widzisz spiralę, automatyczne wieże i testową mgłę."};break;case 7:heading="DRZEWKO TECHNOLOGII";intro="Drużyna wspólnie rozwija żywioły i wybiera kierunki ulepszeń.";names=new[]{"WSPÓLNE INWESTYCJE","RDZENIE T1","SPECJALIZACJE T2","ULEPSZENIA T3","REAKCJE ŻYWIOŁÓW"};descs=new[]{"Gracze inwestują w drzewka żywiołów niezależnie od żywiołu wybranego bohatera.","Siedem żywiołów rozpoczyna się od podstawowych rdzeni technologii.","Każdy żywioł ma dwa konkurencyjne kierunki rozwoju.","Dalszy rozwój prowadzi do wspólnego ulepszenia T3.","Szczegółowe zasady reakcji pozostają do ustalenia; nie należą do pierwszego pre-PoC."};break;case 5:heading="ZASADY RIFTORYN";intro="Docelowa gra łączy budowę obrony, ucieczkę oraz adaptację do rywala.";names=new[]{"DWIE DRUŻYNY","DRAFT","BUDOWA","ATAK","WYNIK","WIDOCZNOŚĆ"};descs=new[]{"Dwie drużyny po pięciu graczy naprzemiennie atakują obronę rywala.","Bohaterowie wybierani są ze wspólnej puli bez duplikatów.","Drużyny inwestują i ustawiają wieże na swoich mapach.","Runnerzy poruszają się po labiryncie, unikają ostrzału i używają zdolności.","Najpierw liczy się liczba Runnerów na mecie, potem czas lub najdalszy postęp.","Układ wrogich wież jest odkrywany w czasie ataku; zapamiętany stan może się zestarzeć."};break;default:heading="ŚWIAT I PRÓBY";intro="Po wyniszczającej wojnie krainy rozstrzygają część sporów poprzez obowiązkowe Próby drużyn w labiryntach.";names=new[]{"RIFTORYN","POJEDYNEK BRAM","TRENING BOHATERA","HISTORIE"};descs=new[]{"Świat siedmiu krain i ich odmiennych żywiołów. Nazwy i szczegóły świata pozostają propozycją.","Drużyny budują obronę i podejmują próbę przejścia przez labirynt przeciwnika.","Trening przygotowuje bohatera do przyszłych zmagań; mastery pozostaje pomysłem do dalszego zaprojektowania.","Każdy bohater ma własne pochodzenie i powody, by stanąć do Prób."};break;}
      Text(new Rect(76,292,1100,38),heading,section);
      Text(new Rect(78,339,1350,48),intro,mutedText);
      Fill(new Rect(78,389,1420,1),line);
      Rect v=new(76,404,1440,390);int cols=2,rows=Mathf.CeilToInt(names.Length/(float)cols);
      scroll=GUI.BeginScrollView(v,scroll,new Rect(0,0,1410,rows*150));
      for(int i=0;i<names.Length;i++)
      {
          float cx=(i%cols)*705,cy=(i/cols)*150;
          Fill(new Rect(cx,cy,680,136),new Color(.12f,.10f,.16f));
          Fill(new Rect(cx,cy,3,136),new Color(.55f,.43f,.32f));
          Text(new Rect(cx+22,cy+17,620,25),names[i],eyebrow);
          Text(new Rect(cx+22,cy+52,625,70),descs[i],body);
      }
      GUI.EndScrollView();
      Text(new Rect(78,806,1360,24),RiftorynCodexData.Text("ARCHIWUM RIFTORYN  ·  INFORMACJE ROBOCZE","RIFTORYN ARCHIVE  ·  WORK IN PROGRESS"),heroBadge);
    }
}
