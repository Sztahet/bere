using System;
using UnityEngine;

[Serializable] public class EncyclopediaPlace { public string id,name,nameEn,region,arena,desc,history,trial,status12,situation12,risk12,needs12,status12En,situation12En,risk12En,needs12En; public float x,y; }
[Serializable] public class EncyclopediaPoint { public float x,y; }
[Serializable] public class EncyclopediaRoute { public string id,name,nameEn,status,statusEn,description,descriptionEn,fromId,toId,risk,riskEn,needs,needsEn; public bool secondary; public EncyclopediaPoint[] points; }
[Serializable] public class EncyclopediaWorld { public EncyclopediaPlace[] places; public EncyclopediaRoute[] routes; }
[Serializable] public class EncyclopediaElement { public string id,name,a,b; public string[] baseText,aText,bText,t3; }
[Serializable] public class EncyclopediaTree { public EncyclopediaElement[] elements; }

public partial class RiftorynMenu
{
    EncyclopediaWorld encyclopediaWorld;
    EncyclopediaTree encyclopediaTree;
    Texture2D worldImage;
    int worldSelected, worldRouteSelected=-1, elementSelected, treeElement, treeNode, treeBudget=2000;
    readonly int[,] treeFunding=new int[7,4];
    readonly int[] treeBranch=new int[7];
    Vector2 worldDetailsScroll,treeDetailsScroll;
    Texture2D mandalaArt,mandalaDisc,mandalaRing;
    float treeZoom=1f; int treeReaction=-1;
    static readonly string[] RingNames={"Ogień","Lód","Natura","Światło","Ziemia","Ciemność","Błyskawice"};
    static readonly string[] RingIcons={"fire","ice","nature","light","earth","darkness","lightning"};
    static int Relation(int attacker,int target)
    {
        if(attacker==target)return 0;
        int distance=(target-attacker+7)%7;
        return distance==2?120:distance==1?110:distance==5?80:distance==6?90:100;
    }
    void ElementEncyclopedia()
    {
        Frame(new Rect(50,270,1500,570),panel);
        Text(new Rect(75,287,1000,32),RiftorynCodexData.Text("ZALEŻNOŚCI ŻYWIOŁÓW","ELEMENTAL RELATIONSHIPS"),section);
        Text(new Rect(78,329,1350,38),RiftorynCodexData.Text("Wybierz atakujący żywioł. Porównanie jest propozycją do testów.","Select the attacking element. These values are a balance proposal."),mutedText);
        for(int i=0;i<7;i++)
        {
            Rect r=new(76+i*205,380,192,108);bool active=i==elementSelected;
            Fill(r,active?new Color(.25f,.20f,.22f):panelAlt);
            if(active)Fill(new Rect(r.x,r.yMax-3,r.width,3),gold);
            var icon=Icon("elements",RingIcons[i]);if(icon)GUI.DrawTexture(new Rect(r.x+65,r.y+10,60,60),icon,ScaleMode.ScaleToFit);
            Text(new Rect(r.x+12,r.y+78,170,25),RiftorynL10n.Translate(RingNames[i]).ToUpperInvariant(),heroBadge);
            if(GUI.Button(r,GUIContent.none,GUIStyle.none))elementSelected=i;
        }
        Text(new Rect(78,510,1000,25),RiftorynCodexData.Text("CEL","TARGET"),eyebrow);
        Text(new Rect(800,510,600,25),RiftorynCodexData.Text("ZADAJESZ     /     OTRZYMUJESZ","YOU DEAL     /     YOU RECEIVE"),eyebrow);
        int row=0;
        for(int i=0;i<7;i++)
        {
            if(i==elementSelected)continue;
            float y=540+row*40;int outgoing=Relation(elementSelected,i),incoming=Relation(i,elementSelected);
            if(row%2==0)Fill(new Rect(76,y,1420,36),panelAlt);
            var icon=Icon("elements",RingIcons[i]);if(icon)GUI.DrawTexture(new Rect(90,y+3,29,29),icon,ScaleMode.ScaleToFit);
            Text(new Rect(135,y+7,500,25),RiftorynL10n.Translate(RingNames[i]),body);
            Text(new Rect(805,y+7,160,25),outgoing+"%",outgoing>100?eyebrow:body);
            Text(new Rect(1000,y+7,160,25),incoming+"%",body);row++;
        }
        Text(new Rect(78,790,1400,30),RiftorynCodexData.Text("Relacja z tym samym żywiołem pozostaje do ustalenia. To osobny system od odporności bohaterów.","Same-element damage is undecided. This is separate from hero resistances."),mutedText);
    }
    void OnDestroy()
    {
        if(mandalaDisc)Destroy(mandalaDisc);
        if(mandalaRing)Destroy(mandalaRing);
    }
    void LoadWorld()
    {
        if(encyclopediaWorld!=null)return;
        var data=Resources.Load<TextAsset>("encyclopedia_world");encyclopediaWorld=data?JsonUtility.FromJson<EncyclopediaWorld>(data.text):new EncyclopediaWorld();
        worldImage=Resources.Load<Texture2D>("Timeline/riftoryn-mapa-swiata-v1");
    }
    static string PlaceName(EncyclopediaPlace place)=>RiftorynL10n.CurrentCode=="EN"&&!string.IsNullOrEmpty(place.nameEn)?place.nameEn:place.name;
    static string WorldLang(string pl,string en)=>RiftorynL10n.CurrentCode=="EN"&&!string.IsNullOrEmpty(en)?en:pl;
    static Vector2 WorldPoint(Rect image,EncyclopediaPoint point)=>new(image.x+image.width*point.x/100f,image.y+image.height*point.y/100f);
    string WorldPlaceName(string id)
    {
        foreach(var place in encyclopediaWorld.places)if(place.id==id)return PlaceName(place);
        return id??"";
    }
    void WorldRouteLine(Vector2 a,Vector2 b,Color color,bool secondary)
    {
        if(!secondary){TreeLine(a,b,color);return;}
        float distance=Vector2.Distance(a,b);int parts=Mathf.Max(1,Mathf.CeilToInt(distance/8f));
        for(int i=0;i<parts;i+=2)TreeLine(Vector2.Lerp(a,b,(float)i/parts),Vector2.Lerp(a,b,(float)Mathf.Min(i+1,parts)/parts),color);
    }
    void WorldEncyclopedia()
    {
        LoadWorld();Frame(new Rect(50,270,1500,570),panel);
        Text(new Rect(75,287,900,32),RiftorynCodexData.Text("RIFTORYN  /  STAN ŚWIATA · 12 ET","RIFTORYN  /  WORLD STATUS · 12 AT"),section);
        Text(new Rect(77,327,1400,30),RiftorynCodexData.Text("Wybierz miasto lub numer szlaku. Linia przerywana: mniej strzeżony szlak. Przebieg jest schematyczny.","Select a city or route number. Dashed line: less guarded route. Lines are schematic."),mutedText);
        if(encyclopediaWorld.places==null||encyclopediaWorld.places.Length==0)return;
        Rect map=new(77,365,820,390);Fill(map,panelAlt);
        var routes=encyclopediaWorld.routes;
        if(worldImage)
        {
            float ratio=(float)worldImage.width/worldImage.height;float w=Mathf.Min(map.width,map.height*ratio),h=w/ratio;
            Rect img=new(map.center.x-w/2,map.center.y-h/2,w,h);GUI.DrawTexture(img,worldImage,ScaleMode.StretchToFill);
            if(routes!=null)
            {
                for(int i=0;i<routes.Length;i++)
                {
                    var route=routes[i];if(route.points==null||route.points.Length<2)continue;
                    Color routeColor=i==worldRouteSelected?gold:route.secondary?new Color(.95f,.50f,.33f,.95f):new Color(.88f,.79f,.59f,.85f);
                    for(int j=1;j<route.points.Length;j++)WorldRouteLine(WorldPoint(img,route.points[j-1]),WorldPoint(img,route.points[j]),routeColor,route.secondary);
                }
                for(int i=0;i<routes.Length;i++)
                {
                    var route=routes[i];if(route.points==null||route.points.Length<2)continue;
                    int j=(route.points.Length-1)/2;Vector2 point=Vector2.Lerp(WorldPoint(img,route.points[j]),WorldPoint(img,route.points[j+1]),.5f);
                    Rect marker=new(point.x-11,point.y-11,22,22);Fill(marker,i==worldRouteSelected?gold:new Color(.12f,.09f,.12f,.96f));
                    if(GUI.Button(marker,(i+1).ToString())){worldRouteSelected=i;worldDetailsScroll=Vector2.zero;}
                }
            }
            for(int i=0;i<encyclopediaWorld.places.Length;i++)
            {
                var p=encyclopediaWorld.places[i];Rect marker=new(img.x+img.width*p.x/100-10,img.y+img.height*p.y/100-10,20,20);
                Fill(marker,worldRouteSelected<0&&i==worldSelected?gold:new Color(.18f,.12f,.16f,.95f));
                if(GUI.Button(marker,"✦")){worldSelected=i;worldRouteSelected=-1;worldDetailsScroll=Vector2.zero;}
            }
        }
        for(int i=0;i<encyclopediaWorld.places.Length;i++)
        {
            float x=77+(i%4)*205,y=764+(i/4)*27;Rect b=new(x,y,198,24);
            if(Click(b,PlaceName(encyclopediaWorld.places[i]),worldRouteSelected<0&&i==worldSelected)){worldSelected=i;worldRouteSelected=-1;worldDetailsScroll=Vector2.zero;}
        }
        Frame(new Rect(915,365,600,453),panelAlt);
        string title,subtitle,details;
        if(routes!=null&&worldRouteSelected>=0&&worldRouteSelected<routes.Length)
        {
            var route=routes[worldRouteSelected];
            title=(worldRouteSelected+1)+". "+WorldLang(route.name,route.nameEn);
            subtitle=RiftorynCodexData.Text("SZLAK · 12 ET","ROUTE · 12 AT");
            details=WorldPlaceName(route.fromId)+"  ↔  "+WorldPlaceName(route.toId)+"\n\n"+RiftorynCodexData.Text("STAN","STATUS")+"\n"+WorldLang(route.status,route.statusEn)+"\n\n"+WorldLang(route.description,route.descriptionEn);
            if(!string.IsNullOrEmpty(route.risk))details+="\n\n"+RiftorynCodexData.Text("ZAGROŻENIA","RISKS")+"\n"+WorldLang(route.risk,route.riskEn);
            if(!string.IsNullOrEmpty(route.needs))details+="\n\n"+RiftorynCodexData.Text("POTRZEBY I NAPRAWY","NEEDS AND REPAIRS")+"\n"+WorldLang(route.needs,route.needsEn);
            details+="\n\n"+RiftorynCodexData.Text("Przebieg i nazwa są propozycją.","The route and name are proposals.");
        }
        else
        {
            var place=encyclopediaWorld.places[Mathf.Clamp(worldSelected,0,encyclopediaWorld.places.Length-1)];
            title=PlaceName(place);subtitle=WorldLang(place.region,"CITY · 12 AT");
            details=WorldLang(place.status12,place.status12En)+"\n\n"+WorldLang(place.situation12,place.situation12En)+"\n\n"+RiftorynCodexData.Text("ZAGROŻENIA","RISKS")+"\n"+WorldLang(place.risk12,place.risk12En)+"\n\n"+RiftorynCodexData.Text("POTRZEBY I NAPRAWY","NEEDS AND REPAIRS")+"\n"+WorldLang(place.needs12,place.needs12En)+"\n\n"+RiftorynCodexData.Text("OŚRODEK PRÓB","TRIAL SITE")+"  ·  "+RiftorynL10n.Translate(place.arena)+"\n"+RiftorynL10n.Translate(place.trial)+"\n\n"+RiftorynCodexData.Text("O MIEJSCU","ABOUT")+"\n"+RiftorynL10n.Translate(place.desc)+"\n\n"+RiftorynCodexData.Text("HISTORIA","HISTORY")+"\n"+RiftorynL10n.Translate(place.history);
        }
        Text(new Rect(940,383,545,25),subtitle,eyebrow);Text(new Rect(940,415,545,55),title,section);
        float height=Mathf.Max(310,body.CalcHeight(new GUIContent(details),530)+20);
        worldDetailsScroll=GUI.BeginScrollView(new Rect(940,480,550,310),worldDetailsScroll,new Rect(0,0,530,height));
        Text(new Rect(0,0,530,height),details,body);GUI.EndScrollView();
    }
    void LoadTree()
    {
        if(encyclopediaTree!=null)return;
        var data=Resources.Load<TextAsset>("encyclopedia_tree");encyclopediaTree=data?JsonUtility.FromJson<EncyclopediaTree>(data.text):new EncyclopediaTree();
        mandalaArt=Resources.Load<Texture2D>("Timeline/elemental-mandala");
        ResetTree();
    }
    void ResetTree()
    {
        Array.Clear(treeFunding,0,treeFunding.Length);Array.Clear(treeBranch,0,treeBranch.Length);treeBudget=2000;
        if(encyclopediaTree?.elements==null)return;
        foreach(string id in new[]{"fire","ice","nature","earth","lightning"})
            for(int i=0;i<encyclopediaTree.elements.Length&&i<7;i++)if(encyclopediaTree.elements[i].id==id)treeFunding[i,0]+=100;
    }
    Vector2 TreePoint(Vector2 center,float radius,float degrees)
    {
        float radians=(degrees-90)*Mathf.Deg2Rad;
        return center+new Vector2(Mathf.Cos(radians),Mathf.Sin(radians))*radius*1.25f*treeZoom;
    }
    void TreeLine(Vector2 a,Vector2 b,Color color)
    {
        Vector2 delta=b-a;int steps=Mathf.Max(1,Mathf.CeilToInt(delta.magnitude/2));
        for(int i=0;i<=steps;i++){Vector2 p=Vector2.Lerp(a,b,i/(float)steps);Fill(new Rect(p.x,p.y,2,2),color);}
    }
    Texture2D MandalaCircle(bool outline)
    {
        Texture2D cached=outline?mandalaRing:mandalaDisc;if(cached)return cached;
        const int n=64;var tex=new Texture2D(n,n,TextureFormat.RGBA32,false);tex.wrapMode=TextureWrapMode.Clamp;tex.filterMode=FilterMode.Bilinear;
        var pixels=new Color[n*n];
        for(int y=0;y<n;y++)for(int x=0;x<n;x++)
        {
            float d=Vector2.Distance(new Vector2(x+.5f,y+.5f),new Vector2(n/2f,n/2f));
            float alpha=outline?Mathf.Clamp01(1-Mathf.Abs(d-29.5f)):Mathf.Clamp01(31-d);
            pixels[y*n+x]=new Color(1,1,1,alpha);
        }
        tex.SetPixels(pixels);tex.Apply(false,true);if(outline)mandalaRing=tex;else mandalaDisc=tex;return tex;
    }
    void MandalaCircleDraw(Rect r,Color color,bool outline=false)
    {
        Color old=GUI.color;GUI.color=color;GUI.DrawTexture(r,MandalaCircle(outline));GUI.color=old;
    }
    void TreeWheelNode(Vector2 position,int element,int node,float size,string caption)
    {
        Rect r=new(position.x-size/2,position.y-size/2,size,size);
        bool active=treeReaction<0&&treeElement==element&&treeNode==node;
        int[] costs={800,1400,1400,2200};int funded=treeFunding[element,node];
        Color accent=GridElementColors[element];
        MandalaCircleDraw(r,new Color(.055f,.045f,.09f,.92f));
        MandalaCircleDraw(r,active?gold:funded>=costs[node]?accent:new Color(accent.r,accent.g,accent.b,.65f),true);
        if(active)MandalaCircleDraw(new Rect(r.x-4,r.y-4,r.width+8,r.height+8),new Color(gold.r,gold.g,gold.b,.55f),true);
        if(node==0)
        {
            var icon=Icon("elements",encyclopediaTree.elements[element].id);
            if(icon)GUI.DrawTexture(new Rect(r.x+6,r.y+4,r.width-12,r.height-12),icon,ScaleMode.ScaleToFit);
        }
        else
        {
            var st=new GUIStyle(cardName){alignment=TextAnchor.MiddleCenter,fontSize=13};
            Text(r,caption,st);
        }
        if(funded>0&&funded<costs[node])Fill(new Rect(r.x+3,r.yMax-5,(r.width-6)*funded/costs[node],2),gold);
        if(GUI.Button(r,GUIContent.none,GUIStyle.none)){treeElement=element;treeNode=node;treeReaction=-1;treeDetailsScroll=Vector2.zero;}
    }
    void TreeEncyclopedia()
    {
        LoadTree();Frame(new Rect(50,270,1500,620),panel);
        Text(new Rect(76,285,900,32),RiftorynCodexData.Text("MANDALA ŻYWIOŁÓW","ELEMENTAL MANDALA"),section);
        Text(new Rect(78,316,1100,18),RiftorynCodexData.Text("Wybierz węzeł. Koszty i efekty są propozycją do testów.","Select a node. Costs and effects are proposals."),mutedText);
        Text(new Rect(1300,291,180,28),treeBudget+" G",eyebrow);
        if(encyclopediaTree.elements==null||encyclopediaTree.elements.Length<7)return;
        Rect board=new(76,335,760,545);Frame(board,new Color(.075f,.065f,.115f));
        if(Click(new Rect(91,346,38,32),"−"))treeZoom=Mathf.Max(.7f,treeZoom-.1f);
        if(Click(new Rect(134,346,38,32),"+"))treeZoom=Mathf.Min(1.2f,treeZoom+.1f);
        if(Click(new Rect(178,346,130,32),RiftorynCodexData.Text("DOPASUJ","FIT")))treeZoom=1;
        Text(new Rect(330,352,430,22),RiftorynCodexData.Text("Wybierz węzeł, aby poznać szczegóły","Select a node for details"),heroBadge);
        Rect view=new(82,385,748,485);GUI.BeginGroup(view);
        Vector2 center=new(view.width/2,view.height/2);Event ev=Event.current;
        if(mandalaArt)
        {
            float diameter=468*treeZoom;
            Rect art=new(center.x-diameter/2,center.y-diameter/2,diameter,diameter);
            Color tint=GUI.color;GUI.color=new Color(.90f,.88f,.85f,.76f);
            GUI.DrawTexture(art,mandalaArt,ScaleMode.ScaleToFit);GUI.color=tint;
        }
        if(ev.type==EventType.ScrollWheel&&new Rect(0,0,view.width,view.height).Contains(ev.mousePosition)){treeZoom=Mathf.Clamp(treeZoom-ev.delta.y*.025f,.7f,1.2f);ev.Use();}
        for(int i=0;i<7;i++)
        {
            float a=i*360f/7f;Color c=GridElementColors[i];c.a=.55f;
            Vector2 root=TreePoint(center,160,a),left=TreePoint(center,100,a-12),right=TreePoint(center,100,a+12),tier=TreePoint(center,48,a);
            TreeLine(root,left,c);TreeLine(root,right,c);TreeLine(left,tier,c);TreeLine(right,tier,c);
            Vector2 nextRoot=TreePoint(center,160,(i+1)*360f/7f);
            Vector2 adjacent=TreePoint(center,142.5f,a+180f/7f);
            TreeLine(root,adjacent,new Color(.72f,.61f,.43f,.48f));
            TreeLine(adjacent,nextRoot,new Color(.72f,.61f,.43f,.48f));
            TreeLine(root,TreePoint(center,160,a-14.9f),c);
            TreeLine(root,TreePoint(center,160,a+14.9f),c);
        }
        MandalaCircleDraw(new Rect(center.x-15,center.y-15,30,30),new Color(.055f,.045f,.09f,.95f));
        MandalaCircleDraw(new Rect(center.x-15,center.y-15,30,30),gold,true);
        Text(new Rect(center.x-6,center.y-8,20,20),"✦",heroBadge);
        for(int i=0;i<7;i++)
        {
            float a=i*360f/7f;
            TreeWheelNode(TreePoint(center,160,a),i,0,38,"T1");
            TreeWheelNode(TreePoint(center,100,a-12),i,1,26,"I");
            TreeWheelNode(TreePoint(center,100,a+12),i,2,26,"II");
            TreeWheelNode(TreePoint(center,48,a),i,3,24,"III");
            for(int offset=1;offset<=3;offset++)
            {
                int reactionId=i*3+offset-1;
                float reactionAngle=a+(offset==1?180f/7f:offset==2?-14.9f:14.9f);
                Vector2 reaction=TreePoint(center,offset==1?142.5f:160f,reactionAngle);
                Rect rr=new(reaction.x-7,reaction.y-7,14,14);
                MandalaCircleDraw(rr,new Color(.05f,.04f,.08f,.82f));
                MandalaCircleDraw(rr,treeReaction==reactionId?gold:new Color(.80f,.68f,.48f,.9f),true);
                var reactionStyle=new GUIStyle(heroBadge){alignment=TextAnchor.MiddleCenter,fontSize=10};
                Text(rr,"✧",reactionStyle);
                if(GUI.Button(rr,GUIContent.none,GUIStyle.none)){treeReaction=reactionId;treeDetailsScroll=Vector2.zero;}
            }
        }
        GUI.EndGroup();
        Frame(new Rect(852,335,638,545),panelAlt);
        var item=encyclopediaTree.elements[treeElement];
        string[] labels={item.name+"  ·  T1",item.a+"  ·  T2",item.b+"  ·  T2",RiftorynCodexData.Text("ULEPSZENIE T3","UPGRADE T3")};int[] costs={800,1400,1400,2200};
        if(treeReaction>=0)
        {
            int first=treeReaction/3,offset=treeReaction%3+1,next=(first+offset)%7;string a=encyclopediaTree.elements[first].name,b=encyclopediaTree.elements[next].name;
            Text(new Rect(878,382,575,25),RiftorynCodexData.Text("REAKCJA ŻYWIOŁÓW","ELEMENTAL REACTION"),eyebrow);
            Text(new Rect(878,418,575,55),a+"  +  "+b,section);
            Text(new Rect(878,485,560,100),RiftorynCodexData.Text("Połączenie tych żywiołów jest częścią projektu. Efekt i koszt pozostają do ustalenia.","This elemental combination is a concept. Its effect and cost are undecided."),body);
        }
        else
        {
            Text(new Rect(878,382,575,25),RiftorynCodexData.Text("WYBRANY WĘZEŁ","SELECTED NODE"),eyebrow);
            Text(new Rect(878,418,575,50),labels[treeNode],section);
            Text(new Rect(878,470,555,25),treeFunding[treeElement,treeNode]+" / "+costs[treeNode]+" G",heroBadge);
            Fill(new Rect(878,500,540,4),line);Fill(new Rect(878,500,540f*treeFunding[treeElement,treeNode]/costs[treeNode],4),gold);
            string[] pair=treeNode==0?item.baseText:treeNode==1?item.aText:treeNode==2?item.bText:item.t3;
            string info=pair!=null&&pair.Length>=2?RiftorynCodexData.Text("OBRONA","DEFENSE")+"\n"+pair[0]+"\n\n"+RiftorynCodexData.Text("UCIECZKA","ESCAPE")+"\n"+pair[1]:"";
            float textHeight=Mathf.Max(115,body.CalcHeight(new GUIContent(info),525)+10);
            treeDetailsScroll=GUI.BeginScrollView(new Rect(878,518,560,140),treeDetailsScroll,new Rect(0,0,525,textHeight));Text(new Rect(0,0,525,textHeight),info,body);GUI.EndScrollView();
            string reason="";
            if(treeNode==1||treeNode==2){if(treeFunding[treeElement,0]<800)reason=RiftorynCodexData.Text("Najpierw sfinansuj rdzeń T1.","Fund the T1 core first.");else if(treeBranch[treeElement]!=0&&treeBranch[treeElement]!=treeNode)reason=RiftorynCodexData.Text("Wybrano konkurencyjną specjalizację.","The competing specialization was selected.");}
            if(treeNode==3&&treeFunding[treeElement,1]<1400&&treeFunding[treeElement,2]<1400)reason=RiftorynCodexData.Text("Najpierw ukończ specjalizację T2.","Complete a T2 specialization first.");
            Text(new Rect(878,662,550,35),reason,mutedText);
            bool can=reason==""&&treeBudget>0&&treeFunding[treeElement,treeNode]<costs[treeNode];bool old=GUI.enabled;GUI.enabled=can;
            if(Click(new Rect(878,700,310,42),RiftorynCodexData.Text("WPŁAĆ DO 400 G","INVEST UP TO 400 G"),true))
            {int amount=Mathf.Min(400,Mathf.Min(treeBudget,costs[treeNode]-treeFunding[treeElement,treeNode]));treeBudget-=amount;treeFunding[treeElement,treeNode]+=amount;if(treeNode==1||treeNode==2)treeBranch[treeElement]=treeNode;}
            GUI.enabled=old;
        }
        if(Click(new Rect(1205,756,250,38),RiftorynCodexData.Text("RESETUJ PODGLĄD","RESET PREVIEW"))){ResetTree();treeNode=0;treeReaction=-1;}
        Text(new Rect(878,767,300,28),RiftorynCodexData.Text("Przykładowy skład drużyny","Example team composition"),heroBadge);
    }
}
