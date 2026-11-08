import json,re,urllib.request,time
from pathlib import Path
src=Path('riftoryn-unity/Assets/Resources/codex.json'); data=json.loads(src.read_text(encoding='utf-8'))
fields={'desc','passName','pass','attackName','attack','defName','def','pet','concept','defense'}
biokeys={'city','specialty','story','reason','family'}
for i,h in enumerate(data['heroes']):
    payload={k:v for k,v in h.items() if k in fields and isinstance(v,str) and v}
    if h.get('bio'): payload['bio']={k:v for k,v in h['bio'].items() if k in biokeys and isinstance(v,str) and v}
    if not payload: continue
    req={'model':'qwen2.5-coder:14b','stream':False,'format':'json','options':{'temperature':0,'num_ctx':8192,'num_predict':2400},'messages':[{'role':'system','content':'Translate Polish text into natural English for a fantasy tower-defense game. Return only valid JSON with EXACTLY the same keys and nested structure as input. Translate every string. Preserve all proper names, character names, exact numbers, percentages, units and game mechanics. Polish "o 20% mniej" means "20% less", never "about 20%". Translate "osłona" as shield. Do not add or omit details.'},{'role':'user','content':json.dumps(payload,ensure_ascii=False)}]}
    try:
        raw=json.dumps(req,ensure_ascii=False).encode('utf8')
        with urllib.request.urlopen(urllib.request.Request('http://127.0.0.1:11434/api/chat',data=raw,headers={'Content-Type':'application/json'}),timeout=180) as resp: response=json.load(resp)
        out=json.loads(response['message']['content'])
        def check(a,b):
            if set(a)!=set(b):raise ValueError('keys differ')
            for k,v in a.items():
                if isinstance(v,dict):check(v,b[k]);continue
                if not isinstance(b[k],str) or not b[k].strip():raise ValueError('missing '+k)
                if sorted(re.findall(r'\d+(?:[.,]\d+)?',v))!=sorted(re.findall(r'\d+(?:[.,]\d+)?',b[k])):raise ValueError('numbers '+k)
        check(payload,out)
        for k,v in out.items():
            if k=='bio': h['bio'].update(v)
            else: h[k]=v
        print(f"OK {i+1}/{len(data['heroes'])} {h['id']}",flush=True)
    except Exception as e:
        print(f"FAILED {h['id']}: {e}",flush=True)
        raise
    Path('riftoryn-unity/AgentScripts/codex-en-draft.json').write_text(json.dumps(data,ensure_ascii=False,indent=2),encoding='utf-8')
