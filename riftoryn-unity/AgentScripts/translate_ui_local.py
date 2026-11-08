import json,re,urllib.request
from pathlib import Path
source=json.loads(Path('riftoryn-unity/AgentScripts/ui-pl.json').read_text(encoding='utf8')); translated={}
for start in range(0,len(source),10):
    chunk=source[start:start+10]; payload={str(start+i):v for i,v in enumerate(chunk)}
    req={'model':'qwen2.5-coder:14b','stream':False,'format':'json','options':{'temperature':0,'num_ctx':8192,'num_predict':3000},'messages':[{'role':'system','content':'Translate Polish fantasy game UI into concise, natural English. Return valid JSON with exactly the same keys, each mapped to its English translation. Preserve capitalization style, punctuation, line breaks, symbols, character names and every exact number. Polish "o 20% mniej" means "20% less", never "about 20%". Translate osłona as shield, wieża as tower, mgła as fog. Do not add or omit information.'},{'role':'user','content':json.dumps(payload,ensure_ascii=False)}]}
    for attempt in range(3):
        try:
            with urllib.request.urlopen(urllib.request.Request('http://127.0.0.1:11434/api/chat',data=json.dumps(req,ensure_ascii=False).encode('utf8'),headers={'Content-Type':'application/json'}),timeout=180) as resp: out=json.loads(json.load(resp)['message']['content'])
            if set(out)!=set(payload):raise ValueError('keys differ')
            for k,v in payload.items():
                if not isinstance(out[k],str) or not out[k].strip(): raise ValueError('empty '+k)
                if sorted(re.findall(r'\d+(?:[.,]\d+)?',v))!=sorted(re.findall(r'\d+(?:[.,]\d+)?',out[k])):raise ValueError('numbers '+k)
            break
        except Exception as e:
            if attempt==2:raise
    translated.update({v:out[k] for k,v in payload.items()})
    Path('riftoryn-unity/AgentScripts/ui-en-draft.json').write_text(json.dumps(translated,ensure_ascii=False,indent=2),encoding='utf8')
    print(f'UI {min(start+10,len(source))}/{len(source)}',flush=True)
