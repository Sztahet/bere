(() => {
  "use strict";
  // Relacje łączą wyłącznie osoby z kodeksu; szkice i wiedza tajna są oznaczone w opisie.
  const edges = [
    ["adrien","lena","Współpraca","work","Od młodości pracują nad urządzeniami. Lena ostrzega Adriena przed wadą sygnalizatora; ma własne projekty i osąd.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["adrien","lucen","Współpraca","work","Lucen doładowuje kryształy w warsztacie. Z czasem Adrien sam prosi go o pomoc i ceni jego precyzję.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["adrien","miko","Przyjaźń i rywalizacja","family","Znają się od lat i przyjaźnią, choć Adrien zazdrości Miko i nie docenia jego cierpliwej pracy.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["adrien","rian","Współpraca","work","Adrien poznaje Rian po jej zeznaniach i zabiega o nadzorowaną współpracę. Jej decyzje i historia pozostają własne.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["lena","lucen","Współpraca","work","Poznają się w warsztacie Adriena, gdzie wspólnie sprawdzają mocowania i bezpieczeństwo urządzeń. Później Lucen pomaga Lenie w próbach jej przyrządu.","RIFTORYN_LENA_03_ZNAM_KOGOS_KTO_POTRAFI.md"],
    ["lena","kiran","Znajomość i współpraca","work","Znają się z wcześniejszej pracy przy wskaźniku. Kiran później pomaga Lenie sprawdzić przyrząd w warunkach terenowych.","RIFTORYN_LENA_03_ZNAM_KOGOS_KTO_POTRAFI.md"],
    ["lena","miko","Wspólne próby","work","Znali się wcześniej przelotnie; później wspólnie pracują nad próbami urządzeń i sygnału.","RIFTORYN_LENA_03_ZNAM_KOGOS_KTO_POTRAFI.md"],
    ["lena","rian","Wspólne próby","work","Rian analizuje projekt Leny i pomaga sprawdzić zachowanie przyrządu w sytuacji awarii.","RIFTORYN_LENA_03_ZNAM_KOGOS_KTO_POTRAFI.md"],
    ["taren","lucen","Ojciec i syn","family","Po śmierci Miry ich więź zostaje zerwana. Nie doszło jeszcze do obiecanego pojednania.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["orien","lucen","Opiekun i uczeń","work","Orien przyjął Lucena w Aurelu, dał mu pracę i uczył go Światła.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["orien","aria","Ojciec i córka","family","Orien jest ojcem Arii i nauczył ją podstaw Światła. Ich późniejsza relacja pozostaje do rozwinięcia.","RIFTORYN_ARIA_POSTAC_I_UMIEJETNOSCI.md"],
    ["aria","lucen","Dom i napięcie","secret","Znali się w domu Oriena; Aria zatrzymała jeden z listów Tarena i po latach przyznała się do tego Lucenowi.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["aria","neris","Tajna współpraca","secret","Aria bierze udział w ustawionych przez Neris zasadzkach, publicznie występując jako ochroniarka. To zaplecze fabularne.","RIFTORYN_ARIA_POSTAC_I_UMIEJETNOSCI.md"],
    ["saila","ruvan","Opiekun i uczennica","family","Ruvan przygarnął Sailę, uczył ją magii Lodu i wspierał jej naukę w Val Meris.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["saila","lucen","Pokaz i nauka","work","Spotykają się w Val Meris, gdy Lucen prowadzi pokaz dla młodszych uczniów. Dalsza wspólna scena jest pomysłem.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["lucen","pyrra","Wspólny rocznik","work","Poznają się podczas nauki w Val Meris jako uczniowie jednego rocznika.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["lucen","zet","Wspólny rocznik","work","Uczą się w tym samym roczniku w Val Meris.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["pyrra","zet","Wspólny rocznik","work","Uczą się w tym samym roczniku w Val Meris.","RIFTORYN_RELACJE_I_PERSPEKTYWY.md"],
    ["niva","liora","Siostry","family","Rozdzielone podczas wojny, spotykają się ponownie jako dorosłe i odbudowują więź.","RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md"],
    ["niva","elira","Siostry","family","Po rozłące odnajdują się dzięki zapisom i świadectwu; później wspólnie pomagają podczas powodzi.","RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md"],
    ["liora","elira","Bliźniaczki","family","Rozdzielone w dzieciństwie bliźniaczki po latach uczą się na nowo swojej bliskości.","RIFTORYN_HISTORIE_BOHATEROW_DO_ZATWIERDZENIA.md"],
    ["azmir","vaelis","Mistrz i uczennica","work","Vaelis uczy się od Azmira pracy archiwistki i magii Ciemności, a zarazem poprawia jego błędy. Materiał do oceny.","RIFTORYN_AZMIR_POSTAC_I_TRYLOGIA_PROPOZYCJA.md"],
    ["piecarius","ruvan","Spotkanie · szkic","contact","W Nivoren Ruvan przekazuje mu niepewny trop o podobnym znaku.","RIFTORYN_PIECARIUS_02_SLADY_W_POPIELE.md"],
    ["piecarius","nocten","Spotkanie · szkic","contact","Nocten ogląda odrys znaku i zachęca Piecariusa do ostrożnej interpretacji.","RIFTORYN_PIECARIUS_02_SLADY_W_POPIELE.md"],
    ["piecarius","orien","Spotkanie · szkic","contact","W Aurelu pracują przy latarni i rozmawiają o księdze po ojcu Piecariusa.","RIFTORYN_PIECARIUS_02_SLADY_W_POPIELE.md"]
  ].map(([a,b,label,type,description,source])=>({a,b,label,type,description,source}));
  const palette={family:"#cf9eab",work:"#cfa96f",contact:"#899aa9",secret:"#ab88bd"};
  const heroes=storyRoster.slice().sort((a,b)=>a.name.localeCompare(b.name,"pl"));
  const byId=new Map(heroes.map(h=>[h.id,h]));
  let selected=byId.has(location.hash.slice(1))?location.hash.slice(1):"adrien";
  const list=document.getElementById("hero-list"),details=document.getElementById("details"),svg=document.getElementById("graph"),search=document.getElementById("search");
  const ns="http://www.w3.org/2000/svg";
  function el(tag,cls,text){const n=document.createElement(tag);if(cls)n.className=cls;if(text)n.textContent=text;return n;}
  function select(id){if(!byId.has(id))return;selected=id;history.replaceState(null,"","#"+id);render();}
  function svgEl(tag,attrs){const n=document.createElementNS(ns,tag);for(const [k,v] of Object.entries(attrs))n.setAttribute(k,String(v));return n;}
  function node(hero,x,y,center){
    const g=svgEl("g",{class:"node"+(center?" selected":""),tabindex:0,role:"button","aria-label":"Wybierz: "+hero.name});
    const r=center?52:40, clipId="clip-"+hero.id;
    const defs=svgEl("defs",{}),clip=svgEl("clipPath",{id:clipId});
    clip.append(svgEl("circle",{cx:x,cy:y,r:r-2}));defs.append(clip);g.append(defs);
    g.append(svgEl("circle",{class:"portrait-frame",cx:x,cy:y,r}));
    if(hero.portrait)g.append(svgEl("image",{href:hero.portrait,x:x-r,y:y-r,width:r*2,height:r*2,preserveAspectRatio:"xMidYMin slice","clip-path":"url(#"+clipId+")"}));
    const label=svgEl("text",{x,y:y+r+23,"text-anchor":"middle"});label.textContent=hero.name;g.append(label);
    g.addEventListener("click",()=>select(hero.id));g.addEventListener("keydown",event=>{if(event.key==="Enter"||event.key===" "){event.preventDefault();select(hero.id);}});svg.append(g);
  }
  function render(){
    const h=byId.get(selected), related=edges.filter(e=>e.a===selected||e.b===selected);
    document.getElementById("selected-name").textContent=h.name;document.getElementById("selected-meta").textContent=h.role+" / "+h.element;
    document.getElementById("hero-count").textContent=heroes.length;
    list.replaceChildren();
    const query=search.value.trim().toLocaleLowerCase("pl");
    for(const hero of heroes.filter(x=>x.name.toLocaleLowerCase("pl").includes(query))){
      const button=el("button","hero-option"+(hero.id===selected?" active":""));button.type="button";button.setAttribute("aria-pressed",String(hero.id===selected));
      if(hero.portrait){const img=el("img");img.src=hero.portrait;img.alt="";button.append(img);}
      const text=el("span",null,hero.name);text.append(el("small",null,hero.role+" · "+hero.element));button.append(text);button.onclick=()=>select(hero.id);list.append(button);
    }
    details.replaceChildren(el("h2",null,h.name),el("p","role",h.role+" / "+h.element),el("p","status",related.length+" "+(related.length===1?"powiązanie":"powiązań")+" w tej wersji grafu"));
    for(const e of related){
      const other=byId.get(e.a===selected?e.b:e.a);if(!other)continue;
      const card=el("section","rel"),button=el("button",null,other.name);button.type="button";button.onclick=()=>select(other.id);
      const source=el("a",null,"Zobacz źródło ↗");source.href=e.source;source.target="_blank";source.rel="noopener";
      card.append(button,el("p","type",e.label),el("p",null,e.description),source);details.append(card);
    }
    svg.replaceChildren();
    const cx=450,cy=285;
    related.forEach((e,i)=>{
      const angle=-Math.PI/2+2*Math.PI*i/related.length;
      const x=cx+290*Math.cos(angle),y=cy+210*Math.sin(angle);
      const other=byId.get(e.a===selected?e.b:e.a);if(!other)return;
      svg.append(svgEl("line",{x1:cx,y1:cy,x2:x,y2:y,class:"edge",style:"stroke:"+palette[e.type]}));
      const label=svgEl("text",{x:cx+(x-cx)*.60,y:cy+(y-cy)*.60-8,"text-anchor":"middle",class:"edge-label"});label.textContent=e.label;svg.append(label);
      node(other,x,y,false);
    });
    node(h,cx,cy,true);
  }
  search.addEventListener("input",render);
  window.addEventListener("hashchange",()=>{const id=location.hash.slice(1);if(byId.has(id)){selected=id;render();}});
  render();
})();

