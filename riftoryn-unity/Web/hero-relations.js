(() => {
  "use strict";
  // Relacje łączą wyłącznie osoby z kodeksu; szkice i wiedza tajna są oznaczone w opisie.
  const edges = RIFTORYN_RELACJE;
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

