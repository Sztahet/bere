using System;
using UnityEngine;

[Serializable] public class EncyclopediaPlace { public string id,name,nameEn,region,arena,desc,history,trial,status12,situation12,risk12,needs12,status12En,situation12En,risk12En,needs12En; public float x,y; }
[Serializable] public class EncyclopediaPoint { public float x,y; }
[Serializable] public class EncyclopediaRoute { public string id,name,nameEn,status,statusEn,description,descriptionEn,fromId,toId,risk,riskEn,needs,needsEn; public bool secondary; public EncyclopediaPoint[] points; }
[Serializable] public class EncyclopediaRegionSection { public string title,titleEn,text,textEn; }
[Serializable] public class EncyclopediaRegion { public string id,name,tagline,taglineEn,overview,overviewEn; public EncyclopediaRegionSection[] sections; }
[Serializable] public class EncyclopediaWorld { public EncyclopediaPlace[] places; public EncyclopediaRoute[] routes; public EncyclopediaRegion[] regions; }
[Serializable] public class EncyclopediaElement { public string id,name,a,b; public string[] baseText,aText,bText,t3; }
[Serializable] public class EncyclopediaTree { public EncyclopediaElement[] elements; }

public partial class RiftorynMenu
{
    int artifactPage, artifactMap, artifactWagon, artifactOutlaw, artifactBeast;
    bool artifactExpanded, artifactWagonVariant;
    Vector2 artifactScroll;
    readonly Texture2D[] artifactImages=new Texture2D[5];
    static readonly string[] ArtifactFiles={"selen","mapa-ozdobna","mapa-kolorowa","mapa-szkic","mapa-odrys"};
    static readonly string[] ArtifactNames={"OZDOBNA","KOLOROWA","SZKIC","ODRYS","NIVOREN","PATROL"};
    static readonly string[] ArtifactNamesEn={"ORNATE","COLORED","SKETCH","COPY","NIVOREN","PATROL"};
    readonly Texture2D[] regionalMaps=new Texture2D[2];
    Texture2D patrolMap;
    static readonly string[] WagonFiles={"veda","verdan","kharum","vareth","nivoren","vesper"};
    static readonly string[] WagonNames={"VEDA","VERDAN","KHARUM","VARETH","NIVOREN","VESPER"};
    readonly Texture2D[] wagonImages=new Texture2D[6];
    readonly Texture2D[] wagonVariants=new Texture2D[2];
    static readonly string[] OutlawFiles={"grupa","maczuga","miecz","kusza","magia-1","magia-2"};
    static readonly string[] OutlawNames={"GRUPA","MACZUGA","MIECZ","KUSZA","MAGIA I","MAGIA II"};
    static readonly string[] OutlawNamesEn={"GROUP","MACE","SWORD","CROSSBOW","MAGIC I","MAGIC II"};
    readonly Texture2D[] outlawImages=new Texture2D[6];
    readonly Texture2D[] beastImages=new Texture2D[9];
    static readonly string[] BeastFiles={"gloamclaw","ashstride","brambleback","rimeback","vaultwurm","aurorath","pebblemaw","snow-nibbler","ridgebrand-serpent"};
    static readonly string[] BeastNames={"MROCZNOPAZUR","POPIELEC","CIERNIOGRZBIET","SZRONOGRZBIET","GŁĘBOSKÓR","ZORZORÓG","KAMIENIOZĄB","SPICHRZAK","ŻMIJA GRANIOWA"};
    static readonly string[] BeastNamesEn={"GLOAMCLAW","ASHSTRIDE","BRAMBLEBACK","RIMEBACK","VAULTWURM","AURORATH","PEBBLEMAW","SNOW NIBBLER","RIDGEBRAND"};
    static readonly string[] BeastDescriptions={
        "MROCZNOPAZUR · GLOAMCLAW\n\nCichy drapieżnik lasów i skalistych pograniczy Vesper. Jest aktywny przede wszystkim nocą i o zmierzchu. Gęsta, ciemna kryza pomaga mu kryć sylwetkę wśród zarośli i skał.\n\nZACHOWANIE\nPoluje samotnie; samica może przebywać z młodymi. Jest ostrożny i terytorialny, szczególnie gdy broni potomstwa.\n\nPlansza jest referencją wyglądu gatunku.",
        "POPIELEC · ASHSTRIDE\n\nSmukły, długonogi drapieżnik i padlinożerca popielnych terenów, przystosowany do biegu i gorącego, suchego środowiska. Duże uszy pomagają mu oddawać ciepło. Żyje samotnie, w parach lub małych grupach.\n\nISKRA I PYRRA\nIskra jest bardzo młodym popielcem. Po połknięciu splotytu naładowanego przez Pyrrę ujawniła się wokół niego ognista energia. Nie wiadomo, czy inne popielce potrafią to samo. Pyrra obawia się, czy jako dorosły nie zagrozi innym.\n\nPlansza pokazuje również znacznie większe dorosłe osobniki.",
        "CIERNIOGRZBIET · BRAMBLEBACK\n\nDuży, dwugłowy roślinożerca lasów Verdan. Żeruje w gęstych zaroślach, przekopując ziemię w poszukiwaniu roślin. Jego sierść zbiera nasiona, liście i rzepy, które przenosi dalej.\n\nZACHOWANIE\nNa ogół spokojny i stadny, może bronić młodych przed zagrożeniem. Dwie głowy pomagają mu jednocześnie żerować i obserwować otoczenie.\n\nPlansza jest referencją wyglądu gatunku.",
        "SZRONOGRZBIET · RIMEBACK\n\nSilny drapieżnik i padlinożerca wysokich gór Nivoren. Gęsta sierść chroni go przed zimnem, a szerokie łapy pomagają poruszać się po śniegu. Ma ciemne cętki i szorstką grzywę wzdłuż grzbietu.\n\nZACHOWANIE\nŻyje w grupach i chroni młode. W opowieści Saily samica osłania dwoje potomstwa, gdy pobliska wieża ostrzeliwuje skały.\n\nPlansza przedstawia osobniki dorosłe i młode.",
        "GŁĘBOSKÓR · VAULTWURM\n\nOgromny mieszkaniec głębokich jaskiń, spotykany między innymi pod Kharum. Ma potężne łapy do kopania, długie wąsy czuciowe i pancerz z warstw kamiennych płyt. Drążąc przejścia, zmienia układ podziemnych korytarzy.\n\nŚLADY I LEGOWISKO\nZwykle nie jest agresywny, ale może bronić legowiska i młodych. Jego masa sprawia, że nawet przypadkowy ruch w ciasnym tunelu jest niebezpieczny. Górnicy, którzy rozpoznają jego ślady lub znajdą legowisko, wycofują się i ostrzegają innych.\n\nDorosły jest kolosalny; dokładna masa i długość życia pozostają do ustalenia.",
        "ZORZORÓG · AURORATH\n\nNiezwykle rzadkie, samotne stworzenie wysokich gór i odległych dolin. Jego jasną sylwetkę wyróżniają rozłożyste poroże, długa sierść oraz pancerne płytki na barkach. Jest ogromny, płochliwy i zwykle nieagresywny.\n\nOPOWIEŚCI I OBSERWACJE\nMieszkańcy opowiadają, że tam, gdzie pojawia się zorzoróg, odradza się życie, oczyszcza woda i uspokaja ziemia. Uważają go za znak odnowy i równowagi. Związek tych zmian z jego obecnością oraz ewentualne zdolności pozostają niewyjaśnione.\n\nPlansza przedstawia koncepcję gatunku; dokładne wymiary i długość życia pozostają do ustalenia.",
        "KAMIENIOZĄB · PEBBLEMAW\n\nNiewielki, opancerzony mieszkaniec jaskiń i kamienistych wyrobisk. Potrafi zwinąć się w kulę i upodobnić do otaczających go skał. Silnymi pazurami kopie w żwirze i szuka schronienia w szczelinach.\n\nZACHOWANIE\nGdy ktoś przypadkiem na niego nadepnie lub spróbuje go podnieść, może gwałtownie się bronić zębami i pazurami. Krzemień, towarzysz Rokana, jest kamieniozębem, który z czasem nauczył się mu ufać.\n\nPlansza przedstawia dorosłe i młode osobniki.",
        "SPICHRZAK PÓŁNOCNY · SNOW NIBBLER\n\nNiewielki gryzoń Nivoren, przystosowany do zimna i żyjący w koloniach. Sprawnie się wspina i kopie; potrafi przedostać się do spichlerza, przegryzając drewno, skórę lub tkaninę. Wyjada i zanieczyszcza żywność.\n\nZIMOWE ZAPASY\nW osadach odciętych śniegiem utrata zapasów może zagrozić przetrwaniu mieszkańców. Ludzie wzmacniają skrzynie, zabezpieczają szczeliny, regularnie sprawdzają magazyny i przechowują rezerwy w kilku miejscach. Koty pomagają ograniczać liczbę gryzoni, lecz nie zastępują kontroli zapasów.\n\nPlansza przedstawia wygląd dorosłych i młodych.",
        "ŻMIJA GRANIOWA · RIDGEBRAND SERPENT\n\nDuży, jadowity wąż Nivoren, żyjący na skalistych stokach, w wąwozach i przy przełęczach. Wygrzewa się na cieplejszych skałach i wtapia w otoczenie; poluje z zasadzki na niewielkie zwierzęta. Zaskoczony lub osaczony broni się gwałtownie.\n\nZAGROŻENIE NA SZLAKU\nNagły ruch i głośny syk mogą spłoszyć konie ciągnące wóz. Na wąskiej drodze płochliwy zaprzęg zagraża ludziom i ładunkowi nawet bez ukąszenia. Przewoźnicy obserwują pobocza, przekazują ostrzeżenia i w miarę możliwości omijają zauważonego węża.\n\nJad stanowi dodatkowe niebezpieczeństwo; spotkanie nie musi kończyć się atakiem."
    };
    static readonly string[] BeastDescriptionsEn={
        "GLOAMCLAW\n\nA quiet predator of Vesper's forests and rocky borderlands, active mainly at night and twilight. Its dense, dark mantle helps conceal its outline among brush and rocks.\n\nBEHAVIOR\nIt hunts alone; a female may stay with her young. It is cautious and territorial, especially when protecting offspring.\n\nThe sheet is a visual reference for the species.",
        "ASHSTRIDE\n\nA lean, long-legged predator and scavenger of the ashlands, adapted to running through hot, dry terrain. Large ears help it release heat. It lives alone, in pairs or in small groups.\n\nSPARK AND PYRRA\nSpark is a very young ashstride. Fiery energy appeared around him after he swallowed splotite charged by Pyrra. Whether other ashstrides can do the same is unknown. Pyrra worries that he could become a danger to others as an adult.\n\nThe sheet also shows much larger adults.",
        "BRAMBLEBACK\n\nA large, two-headed grazer of Verdan's forests. It forages in dense thickets, rooting through soil for plants. Seeds, leaves and burrs cling to its coat and are carried elsewhere.\n\nBEHAVIOR\nGenerally peaceful and social, it may defend its young when threatened. Its two heads help it forage and watch its surroundings at the same time.\n\nThe sheet is a visual reference for the species.",
        "RIMEBACK\n\nA powerful predator and scavenger of Nivoren's high mountains. Dense fur protects it from the cold, and broad paws help it move over snow. Dark spots mark its coat, with a coarse mane along its back.\n\nBEHAVIOR\nIt lives in groups and protects its young. In Saila's story, a female shields her two offspring as a nearby tower fires at the rocks.\n\nThe sheet shows adults and juveniles.",
        "VAULTWURM\n\nAn enormous inhabitant of deep caverns, found among other places beneath Kharum. It has powerful digging limbs, long sensory feelers and armor of layered stone plates. Its tunneling changes the layout of underground passages.\n\nTRACKS AND LAIRS\nUsually non-aggressive, it may defend its lair and young. Its sheer mass makes even an accidental movement dangerous in a narrow tunnel. Miners who recognize its tracks or find a lair withdraw and warn others.\n\nAdults are colossal; exact weight and lifespan remain undecided.",
        "AURORATH\n\nAn exceptionally rare, solitary creature of high mountains and remote valleys. Its pale silhouette is marked by spreading antlers, long fur and layered shoulder plates. It is enormous, elusive and usually non-aggressive.\n\nTALES AND OBSERVATIONS\nPeople tell stories of life returning, waters clearing and the earth growing calm where an aurorath appears. They regard it as a sign of renewal and balance. Whether its presence causes these changes, and any abilities it may have, remain unexplained.\n\nThe sheet shows a concept for the species; exact dimensions and lifespan remain undecided.",
        "PEBBLEMAW\n\nA small, armored inhabitant of caves and rocky quarries. It can curl into a ball and resemble the surrounding stones. Strong claws help it dig into gravel and shelter in crevices.\n\nBEHAVIOR\nIf accidentally stepped on or picked up, it may defend itself fiercely with teeth and claws. Flint, Rokan’s companion, is a pebblemaw who learned to trust him over time.\n\nThe sheet shows adults and juveniles.",
        "NORTHERN STORE NIBBLER · SNOW NIBBLER\n\nA small cold-adapted rodent of Nivoren that lives in colonies. It climbs and digs well, and may enter a storehouse by gnawing through wood, leather or cloth. It eats and contaminates food.\n\nWINTER STORES\nIn settlements cut off by snow, lost stores can threaten survival. People reinforce chests, close gaps, inspect stores regularly and keep reserves in more than one place. Cats help limit the rodents but do not replace checking supplies.\n\nThe sheet shows adults and young.",
        "RIDGEBRAND SERPENT\n\nA large venomous snake of Nivoren, found on rocky slopes, in ravines and near mountain passes. It basks on warmer rocks, blends into its surroundings and ambushes small animals. When surprised or cornered, it defends itself fiercely.\n\nDANGER ON THE ROAD\nIts sudden movement and loud hiss can frighten horses pulling a wagon. On a narrow road a bolting team endangers people and cargo even without a bite. Carriers watch the roadside, pass on warnings and avoid a spotted snake when possible.\n\nVenom adds another danger; an encounter does not always lead to an attack."
    };

    static readonly string[] WagonDescriptions={
        "WÓZ VEDY · PRZEWOZY NA SZLAKACH\n\nVeda przewozi skrzynie, paczki i części między Kharum a innymi miejscami. W jej historii wóz ciągną konie, a ładunek chroni plandeka. Kiedy jedno z kół zjeżdża z drogi, przewoźnicy przesuwają skrzynię i zabezpieczają ją zapasowym pasem. Później naprawiają uszkodzoną plandekę.\n\nBoczne burty, pasy, narzędzia i zdejmowana osłona pomagają przygotować wóz do różnych zleceń. Właściciel i załoga muszą także uzgodnić ładunek, trasę oraz odbiór.\n\nPlansza pokazuje koncepcję wyglądu wozu, nie gotowy model 3D.",
        "WÓZ ROLNICZY · VERDAN\n\nOtwarty wóz służy do przewożenia plonów, worków, koszy i skrzyń między gospodarstwami, targiem a magazynami. Niskie burty ułatwiają ręczny załadunek; liny pomagają utrzymać towar na nierównej drodze.\n\nZbiory muszą dotrzeć na czas, a stan kanałów, przepraw i dróg ma znaczenie również dla ludzi poza doliną. Użyte tu kolory i konstrukcja pokazują jeden możliwy egzemplarz; wozy innych gospodarstw mogą się różnić.",
        "WÓZ CIĘŻKI · KHARUM\n\nSolidny wóz do przewozu kamienia i rudy z kopalń oraz kamieniołomów. Wzmocniona rama, drewniane koła z obręczami i mocowania pomagają rozłożyć oraz zabezpieczyć ładunek. Klocki pod koła przydają się podczas postoju na pochyłości.\n\nCiężki transport wymaga sprawnych dróg i ostrożnej pracy załogi. Wielkość ładunku i liczba zwierząt zależą od trasy, stanu pojazdu i warunków; sama ilustracja nie ustala jego nośności.",
        "WÓZ WARSZTATOWY · VARETH\n\nPrzewoźny warsztat umożliwia ekipie zabranie części i narzędzi do miejsca naprawy. Szuflady, mocowania, imadło i rozkładana powierzchnia robocza pomagają utrzymać wyposażenie w porządku. Wóz można zamknąć przed dalszą podróżą.\n\nVareth wytwarza i naprawia elementy potrzebne innym krainom. Pokazany pojazd jest przykładem takiego warsztatu; nie oznacza, że każda ekipa korzysta z identycznej konstrukcji.",
        "WÓZ GÓRSKI · NIVOREN\n\nOsłonięty wóz może dostarczać leki, żywność i części do osad przy północnych szlakach. Przykryte skrzynie, plandeka, koce i narzędzia pomagają załodze przygotować się na trudną pogodę. Na planszy pokazano również propozycję zdejmowanych okuć kół.\n\nPrzed wyjazdem przewoźnicy sprawdzają stan przejścia i schronień. Nawet przygotowany wóz może zostać zatrzymany przez zamieć lub uszkodzoną drogę. Szczegóły działania okuć pozostają do ustalenia.",
        "WÓZ KONWOJOWY · VESPER\n\nKryty wóz przewozi skrzynie, zapasy i dokumenty. Zwyczajny wygląd nie zdradza zawartości ładunku; mocowania i plandeka pomagają zabezpieczyć go podczas podróży. Osobna torba może chronić dokumenty przed pogodą.\n\nZałogi Vesper znają nocne trasy i sposoby osłaniania transportów. Ukrycie trasy lub ładunku nie zastępuje załogi, planu ratunkowego i potwierdzenia dostawy. Pokazana skrzynia jest przykładem opakowania, a nie ustaloną konstrukcją urządzenia maskującego."
    };
    static readonly string[] WagonDescriptionsEn={
        "VEDA'S WAGON · FREIGHT ON THE ROADS\n\nVeda carries crates, parcels, and parts between Kharum and other places. In her story, horses pull the wagon and a tarpaulin protects the load. When a wheel leaves the road, the carriers shift a crate and secure it with a spare strap. They later repair the damaged cover.\n\nSideboards, straps, tools, and a removable cover help prepare the wagon for different jobs. Its owner and crew also need to agree on the load, route, and delivery.\n\nThe sheet shows a visual concept, not a finished 3D model.",
        "FARM WAGON · VERDAN\n\nAn open wagon carries crops, sacks, baskets, and crates between farms, markets, and warehouses. Low sideboards make loading by hand easier; ropes help secure goods on uneven roads.\n\nHarvests need to arrive on time, and the condition of channels, crossings, and roads matters beyond the valley. The colors and construction show one possible example; other farms' wagons may differ.",
        "HEAVY WAGON · KHARUM\n\nA robust wagon carries stone and ore from mines and quarries. A reinforced frame, wooden wheels with rims, and attachment points help distribute and secure the load. Wheel chocks are useful when stopped on a slope.\n\nHeavy transport requires sound roads and careful work by the crew. Load size and the number of animals depend on the route, vehicle, and conditions; the illustration does not establish its capacity.",
        "WORKSHOP WAGON · VARETH\n\nA traveling workshop lets a crew bring parts and tools to a repair site. Drawers, fittings, a vise, and a folding work surface help organize equipment. The wagon can be closed for the next journey.\n\nVareth makes and repairs parts needed by other regions. This vehicle is one example; not every crew uses an identical design.",
        "MOUNTAIN WAGON · NIVOREN\n\nA covered wagon can deliver medicines, food, and parts to settlements along northern roads. Covered crates, a tarpaulin, blankets, and tools help crews prepare for difficult weather. The sheet also proposes removable wheel fittings.\n\nBefore departure, carriers check crossings and shelters. Even a prepared wagon can be stopped by a blizzard or damaged road. The fittings' operation remains undecided.",
        "CONVOY WAGON · VESPER\n\nA covered wagon carries crates, supplies, and documents. Its ordinary appearance does not reveal the cargo; fastenings and canvas help secure it during travel. A separate pouch can protect documents from weather.\n\nVesper's crews know night routes and ways of protecting shipments. Concealment does not replace a crew, rescue plan, or delivery confirmation. The pictured crate is an example of packaging, not an established masking device."
    };
    Texture2D ArtifactImage(int index)
    {
        if(!artifactImages[index])artifactImages[index]=Resources.Load<Texture2D>("WorldArtifacts/"+ArtifactFiles[index]);
        return artifactImages[index];
    }
    void WorldArtifacts()
    {
        Frame(new Rect(50,270,1500,570),panel);
        Text(new Rect(76,284,1000,34),RiftorynCodexData.Text("ŻYCIE W RIFTORYNIE","LIFE IN RIFTORYN"),section);
        if(Click(new Rect(76,326,150,34),RiftorynCodexData.Text("SELEN","SELEN"),artifactPage==0)&&artifactPage!=0){artifactPage=0;artifactScroll=Vector2.zero;artifactExpanded=false;}
        if(Click(new Rect(234,326,150,34),RiftorynCodexData.Text("MAPY FABULARNE","IN-WORLD MAPS"),artifactPage==1)&&artifactPage!=1){artifactPage=1;artifactScroll=Vector2.zero;artifactExpanded=false;}
        if(Click(new Rect(392,326,150,34),RiftorynCodexData.Text("WOZY","WAGONS"),artifactPage==2)&&artifactPage!=2){artifactPage=2;artifactScroll=Vector2.zero;artifactExpanded=false;}
        if(Click(new Rect(550,326,150,34),RiftorynCodexData.Text("BANDYCI","OUTLAWS"),artifactPage==3)&&artifactPage!=3){artifactPage=3;artifactScroll=Vector2.zero;artifactExpanded=false;}
        if(Click(new Rect(708,326,150,34),RiftorynCodexData.Text("BESTIE","BEASTS"),artifactPage==4)&&artifactPage!=4){artifactPage=4;artifactScroll=Vector2.zero;artifactExpanded=false;}
        if(artifactPage==4)
            for(int i=0;i<9;i++)if(Click(new Rect(76+(i%3)*260,368+(i/3)*37,250,30),RiftorynCodexData.Text(BeastNames[i],BeastNamesEn[i]),artifactBeast==i)){if(artifactBeast!=i)artifactScroll=Vector2.zero;artifactBeast=i;artifactExpanded=false;}
        if(artifactPage==1)
            for(int i=0;i<6;i++)if(Click(new Rect(76+i*131,368,125,30),RiftorynCodexData.Text(ArtifactNames[i],ArtifactNamesEn[i]),artifactMap==i)){artifactMap=i;artifactExpanded=false;}
        if(artifactPage==2)
            for(int i=0;i<6;i++)if(Click(new Rect(76+i*131,368,125,30),WagonNames[i],artifactWagon==i)){if(artifactWagon!=i){artifactScroll=Vector2.zero;artifactWagonVariant=false;}artifactWagon=i;artifactExpanded=false;}
        if(artifactPage==3)
            for(int i=0;i<6;i++)if(Click(new Rect(76+i*131,368,125,30),RiftorynCodexData.Text(OutlawNames[i],OutlawNamesEn[i]),artifactOutlaw==i)){artifactOutlaw=i;artifactExpanded=false;}
        bool hasWagonVariant=artifactPage==2&&(artifactWagon==0||artifactWagon==3);
        if(hasWagonVariant)
        {
            if(Click(new Rect(76,405,190,30),RiftorynCodexData.Text("WARIANT I","VARIANT I"),!artifactWagonVariant)){artifactWagonVariant=false;artifactExpanded=false;}
            if(Click(new Rect(274,405,190,30),RiftorynCodexData.Text("WARIANT II","VARIANT II"),artifactWagonVariant)){artifactWagonVariant=true;artifactExpanded=false;}
        }
        bool raisedImage=hasWagonVariant||artifactPage==4;
        Rect art=new(76,artifactPage==0?375:artifactPage==4?482:raisedImage?445:410,800,artifactPage==4?318:raisedImage?355:390);
        Frame(art,panelAlt);Texture2D image;
        if(artifactPage==4){if(!beastImages[artifactBeast])beastImages[artifactBeast]=Resources.Load<Texture2D>("WorldArtifacts/"+BeastFiles[artifactBeast]);image=beastImages[artifactBeast];}
        else if(artifactPage==3){if(!outlawImages[artifactOutlaw])outlawImages[artifactOutlaw]=Resources.Load<Texture2D>("WorldArtifacts/Outlaws/"+OutlawFiles[artifactOutlaw]);image=outlawImages[artifactOutlaw];}
        else if(artifactPage==2)
        {
            if(hasWagonVariant&&artifactWagonVariant)
            {
                int variantIndex=artifactWagon==0?0:1;
                if(!wagonVariants[variantIndex])wagonVariants[variantIndex]=Resources.Load<Texture2D>("WorldArtifacts/Wagons/"+(variantIndex==0?"kryty-przewoznika":"warsztat-rozkladany"));
                image=wagonVariants[variantIndex];
            }
            else{if(!wagonImages[artifactWagon])wagonImages[artifactWagon]=Resources.Load<Texture2D>("WorldArtifacts/Wagons/"+WagonFiles[artifactWagon]);image=wagonImages[artifactWagon];}
        }
        else if(artifactPage==1&&artifactMap==5)
        {
            if(!patrolMap)patrolMap=Resources.Load<Texture2D>("WorldArtifacts/nivoren-patrol-en");
            image=patrolMap;
        }
        else if(artifactPage==1&&artifactMap==4)
        {
            int language=RiftorynL10n.CurrentCode=="EN"?1:0;
            if(!regionalMaps[language])regionalMaps[language]=Resources.Load<Texture2D>("WorldArtifacts/"+(language==0?"nivoren-travel-pl":"nivoren-travel-en"));
            image=regionalMaps[language];
        }
        else image=ArtifactImage(artifactPage==0?0:artifactMap+1);
        if(image)GUI.DrawTexture(new Rect(art.x+4,art.y+4,art.width-8,art.height-8),image,ScaleMode.ScaleToFit);
        if(GUI.Button(art,GUIContent.none,GUIStyle.none))artifactExpanded=true;
        Text(new Rect(78,780,800,24),RiftorynCodexData.Text("Kliknij ilustrację, aby ją powiększyć.","Select the image to enlarge it."),mutedText);
        Rect details=new(900,326,610,488);Frame(details,panelAlt);
        string content=artifactPage==0?RiftorynCodexData.Text(
            "SELEN I SELAR\nSelen (sel) jest monetą i jednostką wartości używaną w Riftorynie. Moneta zawiera 10 g rzadkiego metalu zwanego selarem. Selar nie jest splotytem i nie magazynuje energii magicznej.\n\nWARTOŚĆ I BICIE\nWartość metalu w monecie odpowiada wartości tej samej masy selara jako surowca. Z 1 kg można wybić 100 selenów. Mennica w Val Meris odmierza metal i bije monety pod nadzorem Rady; koszt wybicia pokrywa wspólny budżet, kiedy Rada uzna emisję za potrzebną. Rada nie otrzymuje metalu za darmo.\n\nPOCHODZENIE I OBIEG\nSelar jest niereaktywny i daje się oddzielić od skały z czystością ponad 99,9%. W kopalniach Kharum znajdowany jest regularnie, lecz w małych ilościach. Od lat w obiegu znajduje się około 5 milionów selenów; nowe można wybić, jeśli są potrzebne i dostępny jest metal. Na ilustracji pokazano artystyczny projekt monety.\n\nDROBNE ZAKUPY\nNie ma wspólnego oficjalnego podziału monety. Ceny mogą być podawane w ułamkach selena, np. 0,02 sel. Mieszkańcy korzystają z lokalnych środków drobnej wymiany, w tym drobnych selarów, oraz kantorów; sklepy często wydają resztę bez prowizji. Akceptacja lokalnych środków zależy od miejsca.\n\nHANDEL I CODZIENNOŚĆ\nTowary i usługi bywają wymieniane bez monet, z dopłatą różnicy lub rozliczane łącznie. Na północy środkiem wymiany bywają owoce z południa, a ich wartość zależy od dostępności i jakości. Majątek wyceniony w selenach nie jest tym samym co gotówka.\n\nPRZYKŁADOWE CENY\nRoboczy przykład: prosty posiłek kosztuje 0,04 sel, dzień pracy ma wartość około 0,40 sel, a skromny przejazd do Val Meris około 1 sel w jedną stronę. Dochód 100 sel rocznie i koszyk utrzymania około 74 sel służą porównaniu, nie opisują każdego mieszkańca. Ceny, płace, potrzeby i forma zapłaty różnią się między miejscami; nie są zatwierdzonym cennikiem.\n\nWSPÓLNE DOSTAWY\nKrainy i Val Meris wpłacają do puli solidarnościowej 5 selenów rocznie za mieszkańca. Rada ustala równy na osobę przydział podstawowych dóbr: siedem prostych porcji żywności tygodniowo, dostęp do wody i wsparcie ogrzewania dostosowane do warunków. Wsparcie pomaga przede wszystkim biedniejszym krainom, a dostawcom Rada płaci po cenach rynkowych.\n\nPODCZAS MECZU\nMeczowe g to wirtualne seleny przyznawane przez organizatora do wyboru technologii. Nie są prywatnymi monetami, które bohater zabiera po meczu.",
            "SELEN AND SELAR\nThe selen (sel) is both a coin and a unit of value in Riftoryn. A coin contains 10 g of a rare metal called selar. Selar is not splotyt and does not store magical energy.\n\nVALUE AND MINTING\nThe metal in a coin is worth the same as an equal mass of raw selar. One kilogram yields 100 selens. The mint in Val Meris measures the metal and strikes coins under Council supervision; the shared budget pays minting costs when the Council decides an issue is needed. The Council does not receive the metal for free.\n\nSOURCE AND CIRCULATION\nSelar is nonreactive and can be separated from rock to over 99.9% purity. Kharum's mines find it regularly but in small quantities. About five million selens have circulated for years; more can be minted if needed and metal is available. The image shows an artistic coin design.\n\nSMALL PURCHASES\nThere is no common official subdivision of the coin. Prices can be expressed in fractions of a selen, such as 0.02 sel. People use local small-change instruments, including small pieces of selar, and exchange offices; shops often give change without a fee. Acceptance varies by place.\n\nTRADE AND DAILY LIFE\nGoods and services may be exchanged without coins, settled together, or balanced with a payment. In the north, southern fruit is sometimes used in exchange; its value depends on availability and quality. Wealth valued in selens is not the same as cash.\n\nEXAMPLE PRICES\nA working example values a simple meal at 0.04 sel, a day of work at about 0.40 sel, and a modest one-way trip to Val Meris at about 1 sel. An annual income of 100 sel and basic living costs of about 74 sel provide a comparison, not a description of every resident. Prices, wages, needs, and forms of payment vary; these figures are not an approved price list.\n\nSHARED SUPPLIES\nThe regions and Val Meris contribute five selens per resident each year to a solidarity fund. The Council establishes an equal per-person allocation of basic goods: seven simple food portions per week, access to water, and heating support suited to local conditions. Support primarily helps poorer regions, and the Council pays suppliers market prices.\n\nDURING A MATCH\nMatch g represents virtual selens provided by the organizer for technology choices. They are not private coins a hero can take away afterward."):
            RiftorynCodexData.Text("MAPY FABULARNE\nTo mapy, z jakich mogą korzystać mieszkańcy Riftorynu. Kartografowie i kopiści przedstawiają kontynent na różne sposoby; rysunki nie są pomiarem granic ani gwarancją bezpieczeństwa dróg.\n\nCZTERY EGZEMPLARZE\nOzdobny, kolorowy, szkic i prosty odrys pokazują różną ilość pracy i szczegółów. Kto ma więcej środków, może kupić kunsztownie wykonaną mapę. Inni zdobywają prostszą kopię lub odrysowują ją samodzielnie. Konkretne ceny nie są ustalone.\n\nPRZED PODRÓŻĄ\nNawet kosztowna mapa może być nieaktualna, a prosty szkic może zawierać przydatne miejscowe informacje. Stan przejść należy sprawdzać u przewodników i na posterunkach. Dokładny przebieg dróg i granic pozostaje otwarty.",
            "IN-WORLD MAPS\nThese are maps the people of Riftoryn might use. Cartographers and copyists depict the continent in different ways; drawings are not a survey of borders or a guarantee that roads are safe.\n\nFOUR EXAMPLES\nThe ornate map, colored copy, sketch and simple hand-drawn copy show different levels of detail and work. Someone with more means may buy an elaborate map. Others obtain a simpler copy or trace one themselves. Exact prices are not established.\n\nBEFORE TRAVEL\nEven an expensive map can be outdated, while a simple sketch may contain useful local knowledge. Check crossing conditions with guides and posts. Exact routes and borders remain open.");
        if(artifactPage==2)content=RiftorynCodexData.Text(WagonDescriptions[artifactWagon],WagonDescriptionsEn[artifactWagon]);
        if(hasWagonVariant&&artifactWagonVariant)content+=artifactWagon==0?
            RiftorynCodexData.Text("\n\nDRUGI WARIANT\nKryty wóz przewoźnika ma łataną plandekę, zdejmowany stelaż oraz dostęp do skrzyń od tyłu. To dodatkowa referencja dla wozu Vedy i podobnych przewozów; szczegóły wykonania mogą się różnić.","\n\nSECOND VARIANT\nThis covered carrier's wagon has patched canvas, a removable frame, and rear access to the cargo. It is an additional reference for Veda's wagon and similar transport; construction details can vary."):
            RiftorynCodexData.Text("\n\nDRUGI WARIANT\nMobilny warsztat rozkłada boczny blat i osłonę na czas postoju. Szuflady i skrzynie mieszczą narzędzia, a imadło pozwala przytrzymać naprawianą część. Przed drogą właściciel składa stanowisko i zabezpiecza wyposażenie. To przykład jednego z możliwych warsztatów.","\n\nSECOND VARIANT\nA mobile workshop unfolds a side workbench and awning when stopped. Drawers and chests hold tools, while a vise secures a part being repaired. Before traveling, its owner folds the workspace and secures the equipment. This is one possible workshop design.");
        if(artifactPage==1)content+=RiftorynCodexData.Text("\n\nNOTATKI Z DROGI\nStan dróg często się zmienia: mosty ulegają uszkodzeniom, przełęcze bywają zasypane, a nowe żerowiska bestii mogą zagrozić podróżnym. Małą grupę lub konwój bestie mogą zaatakować; dużej grupy często się obawiają, ale nie jest ona zawsze bezpieczna. Podróżni dopisują ostrzeżenia i poprawiają swoje mapy. Oznaczenia bywają mniej lub bardziej czytelne i wymagają sprawdzenia przed podróżą.\n\nPrzykładowa mapa Nivoren pokazuje notatki konkretnego podróżnego, a nie stale obowiązujący stan szlaków ani ustalony przebieg wszystkich dróg.","\n\nTRAVEL NOTES\nRoad conditions often change: bridges are damaged, passes become blocked, and new beast feeding grounds can threaten travelers. Beasts may attack a small group or convoy; they often fear larger groups, but size is not a guarantee of safety. Travelers add warnings and correct their maps. Markings vary in clarity and should be checked before departure.\n\nThe example map of Nivoren shows one traveler's notes, not permanent road conditions or the established course of every route.");
        if(artifactPage==1&&artifactMap==5)content+=RiftorynCodexData.Text("\n\nMAPA OBCHODU\nStrażnik notuje na roboczej mapie informacje z kolejnych obchodów: uszkodzone drogi i mosty, ślady bestii, zagrożenia ze strony band oraz możliwe obejścia. Niepewne obserwacje wymagają potwierdzenia. Takie zapiski trafiają do osób aktualizujących mapy ogólne; ich data ma znaczenie, bo warunki mogą się zmienić.\n\nPokazana plansza jest przykładem z angielskimi napisami. Nie ustala rzeczywistego przebiegu wszystkich tras ani stałego stanu zagrożeń.","\n\nPATROL MAP\nA guard records successive patrol observations on a working map: damaged roads and bridges, beast tracks, threats from bandits, and possible detours. Uncertain observations require confirmation. These notes reach the people updating general maps; their dates matter because conditions can change.\n\nThe pictured sheet is an example. It does not establish the exact course of every route or permanent threats.");
        if(artifactPage==3)content=RiftorynCodexData.Text(
            "BANDYCI NA SZLAKACH\n\nW wielu rejonach Riftorynu działają grupy rozbójników, które utrzymują się z napadów na transporty i podróżnych. Dla przewoźników oznacza to zagrożenie nie tylko utratą ładunku: załoga może zostać ranna, a mieszkańcy czekający na żywność, leki lub części mogą nie dostać dostawy.\n\nLUDZIE I MOTYWACJE\nCzęść członków band z różnych powodów nie chce żyć w społecznościach miast i osad. Inni mają odmienne historie i powody. Nie jest to jedna organizacja ani grupa pochodząca tylko z jednej krainy. Samo życie z dala od osad, ubóstwo lub podróżowanie nie czynią nikogo bandytą.\n\nOCHRONA PRZEWOZÓW\nPatrole, eskorty, ostrzeżenia i schronienia pomagają ograniczać niebezpieczeństwo. Żadna kraina nie może jednak stale osłaniać każdej drogi. Załogi muszą oceniać informacje o trasie, planować postoje i wiedzieć, jak wezwać pomoc.\n\nWYGLĄD\nPlansze pokazują możliwe sylwetki i wyposażenie rozbójników: zużyte ubrania, broń oraz sprzęt obozowy. To przykłady do przyszłego modelowania, a nie mundur wspólny dla wszystkich band ani gotowe modele 3D.",
            "OUTLAWS ON THE ROADS\n\nBands of robbers operate in many parts of Riftoryn, living by attacking shipments and travelers. For carriers, the danger goes beyond lost cargo: crews can be injured, and people waiting for food, medicine, or parts may not receive their supplies.\n\nPEOPLE AND MOTIVES\nSome members of these bands choose not to live in the communities of towns and settlements, for different reasons. Others have different histories and motives. They are not one organization or confined to one region. Living far from settlements, poverty, or traveling does not make someone a bandit.\n\nPROTECTING SHIPMENTS\nPatrols, escorts, warnings, and shelters help reduce danger. No region can constantly guard every road. Crews must assess route information, plan stops, and know how to call for help.\n\nAPPEARANCE\nThe sheets show possible outlaw silhouettes and equipment: worn clothing, weapons, and camp gear. They are examples for future modeling, not a uniform shared by all bands or finished 3D models.");
        if(artifactPage==3)content+=RiftorynCodexData.Text("\n\nMAGIA WŚRÓD BAND\nNiektórzy rozbójnicy władają magią żywiołów lub korzystają z przygotowanych urządzeń. W jednej grupie mogą działać osoby o różnych umiejętnościach; sam żywioł nie określa niczyich zamiarów. Ilustracje są przykładami wyglądu i nie ustalają konkretnych zdolności ani ograniczeń rozgrywki.","\n\nMAGIC AMONG OUTLAWS\nSome robbers wield elemental magic or use prepared devices. People with different skills may work in the same band; an element alone does not determine anyone's intentions. The illustrations are visual examples and do not establish specific abilities or gameplay limits.");
        if(artifactPage==4)content=RiftorynCodexData.Text(BeastDescriptions[artifactBeast],BeastDescriptionsEn[artifactBeast]);
        float height=Mathf.Max(430,body.CalcHeight(new GUIContent(content),530)+20);
        artifactScroll=GUI.BeginScrollView(new Rect(920,344,565,450),artifactScroll,new Rect(0,0,545,height));
        Text(new Rect(0,0,530,height),content,body);GUI.EndScrollView();
        if(artifactExpanded)
        {
            Fill(new Rect(40,85,1520,760),new Color(.035f,.03f,.055f,.98f));
            if(image)GUI.DrawTexture(new Rect(70,120,1460,650),image,ScaleMode.ScaleToFit);
            if(Click(new Rect(1340,782,190,38),RiftorynCodexData.Text("ZAMKNIJ","CLOSE"),true))artifactExpanded=false;
        }
    }
    EncyclopediaWorld encyclopediaWorld;
    EncyclopediaTree encyclopediaTree;
    Texture2D worldImage;
    int worldSelected, worldRouteSelected=-1, elementSelected, treeElement, treeNode, treeBudget=2000;
    bool worldShowRegion=true;
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
    EncyclopediaRegion WorldRegion(string id)
    {
        if(encyclopediaWorld.regions!=null)
            foreach(var region in encyclopediaWorld.regions)if(region.id==id)return region;
        return null;
    }
    static string WorldRegionDetails(EncyclopediaRegion region)
    {
        string details=WorldLang(region.overview,region.overviewEn);
        if(region.sections!=null)
            foreach(var section in region.sections)
                details+="\n\n"+WorldLang(section.title,section.titleEn).ToUpperInvariant()+"\n"+WorldLang(section.text,section.textEn);
        return details;
    }
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
        Text(new Rect(77,327,1400,30),RiftorynCodexData.Text("Wybierz ośrodek, aby poznać krainę, lub numer szlaku. Linia przerywana: mniej strzeżony szlak. Przebieg jest schematyczny.","Select a settlement to explore its region, or a route number. Dashed line: less guarded route. Lines are schematic."),mutedText);
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
        float detailsTop=480;
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
            var region=WorldRegion(place.id);
            if(region!=null)
            {
                if(Click(new Rect(940,480,264,30),RiftorynCodexData.Text("KRAINA","REGION"),worldShowRegion)&&!worldShowRegion){worldShowRegion=true;worldDetailsScroll=Vector2.zero;}
                if(Click(new Rect(1212,480,264,30),RiftorynCodexData.Text("OŚRODEK · 12 ET","SETTLEMENT · 12 AT"),!worldShowRegion)&&worldShowRegion){worldShowRegion=false;worldDetailsScroll=Vector2.zero;}
                detailsTop=521;
            }
            title=PlaceName(place);subtitle=WorldLang(place.region,"CITY · 12 AT");
            details=WorldLang(place.status12,place.status12En)+"\n\n"+WorldLang(place.situation12,place.situation12En)+"\n\n"+RiftorynCodexData.Text("ZAGROŻENIA","RISKS")+"\n"+WorldLang(place.risk12,place.risk12En)+"\n\n"+RiftorynCodexData.Text("POTRZEBY I NAPRAWY","NEEDS AND REPAIRS")+"\n"+WorldLang(place.needs12,place.needs12En)+"\n\n"+RiftorynCodexData.Text("OŚRODEK PRÓB","TRIAL SITE")+"  ·  "+RiftorynL10n.Translate(place.arena)+"\n"+RiftorynL10n.Translate(place.trial)+"\n\n"+RiftorynCodexData.Text("O MIEJSCU","ABOUT")+"\n"+RiftorynL10n.Translate(place.desc)+"\n\n"+RiftorynCodexData.Text("HISTORIA","HISTORY")+"\n"+RiftorynL10n.Translate(place.history);
            if(region!=null&&worldShowRegion)
            {
                title=region.name;subtitle=WorldLang(region.tagline,region.taglineEn);
                details=WorldRegionDetails(region);
            }
        }
        Text(new Rect(940,383,545,25),subtitle,eyebrow);Text(new Rect(940,415,545,55),title,section);
        float height=Mathf.Max(310,body.CalcHeight(new GUIContent(details),530)+20);
        worldDetailsScroll=GUI.BeginScrollView(new Rect(940,detailsTop,550,790-detailsTop),worldDetailsScroll,new Rect(0,0,530,height));
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
