using UnityEngine;

public partial class RiftorynTrial
{
    int defenseHoveredSlot=-1, defenseSelectedSlot=-1;
    LineRenderer defensePreview;
    GUIStyle defenseLabel, defenseTiny, defenseButton, defenseMarker;

    float DefensePanelWidth=>Mathf.Clamp(Screen.width*.24f,240f,300f);
    const float DefenseHeaderHeight=90f;
    Rect DefensePanel=>new Rect(Screen.width-DefensePanelWidth,DefenseHeaderHeight,DefensePanelWidth,Screen.height-DefenseHeaderHeight);
    bool DefensePointerOverUi(Vector2 screen)
    {
        Vector2 gui=new(screen.x,Screen.height-screen.y);
        return gui.y<DefenseHeaderHeight || DefensePanel.Contains(gui) || gui.y>Screen.height-46;
    }

    void ResetDefenseEditor()
    {
        defenseHoveredSlot=-1;defenseSelectedSlot=-1;
        HideDefenseEditorPreview();
    }
    void HideDefenseEditorPreview(){if(defensePreview)defensePreview.enabled=false;}
    void DisposeDefenseEditor()
    {
        if(!defensePreview)return;
        if(defensePreview.sharedMaterial)Destroy(defensePreview.sharedMaterial);
        Destroy(defensePreview.gameObject);
        defensePreview=null;
    }
    void EnsureDefensePreview()
    {
        if(defensePreview)return;
        var root=new GameObject("Podgląd zasięgu budowy");
        defensePreview=root.AddComponent<LineRenderer>();
        defensePreview.useWorldSpace=true;defensePreview.loop=true;defensePreview.positionCount=96;
        defensePreview.startWidth=defensePreview.endWidth=.2f;
        defensePreview.material=new Material(Shader.Find("Sprites/Default"));
        defensePreview.enabled=false;
    }
    void UpdateDefensePreview()
    {
        foreach(var tower in towers)
        {
            if(!tower.range)continue;
            tower.range.enabled=tower.slot==defenseSelectedSlot || tower.slot==defenseHoveredSlot;
            tower.range.startWidth=tower.range.endWidth=.20f;
        }
        EnsureDefensePreview();
        bool empty=defenseHoveredSlot>=0&&!towers.Exists(t=>t.slot==defenseHoveredSlot);
        defensePreview.enabled=empty;
        if(!empty)return;
        Color color=TowerColors[selectedType];color.a=.9f;
        defensePreview.startColor=defensePreview.endColor=color;
        Vector3 center=Slot(defenseHoveredSlot)+Vector3.up*3.2f;
        for(int i=0;i<defensePreview.positionCount;i++)
        {
            float angle=i*Mathf.PI*2/defensePreview.positionCount;
            defensePreview.SetPosition(i,center+new Vector3(Mathf.Cos(angle)*TowerRange[selectedType],0,Mathf.Sin(angle)*TowerRange[selectedType]));
        }
    }
    int DefenseSlotAt(Vector2 screen)
    {
        int best=-1;float distance=28f*28f;
        for(int i=0;i<slots.Length;i++)
        {
            Vector3 point=cam.WorldToScreenPoint(Slot(i)+Vector3.up*3.1f);
            if(point.z<=0)continue;
            float d=((Vector2)point-screen).sqrMagnitude;
            if(d<distance){distance=d;best=i;}
        }
        return best;
    }
    void RemoveDefenseTower(int index)
    {
        if(index<0||index>=towers.Count)return;
        var tower=towers[index];
        if(tower.obj)tower.obj.SetActive(false);
        if(tower.range&&tower.range.sharedMaterial)Destroy(tower.range.sharedMaterial);
        if(tower.warning&&tower.warning.sharedMaterial)Destroy(tower.warning.sharedMaterial);
        if(tower.obj)Destroy(tower.obj);
        towers.RemoveAt(index);
    }
    void PlaceDefenseTower(int slot)
    {
        if(slot<0||slot>=slots.Length)return;
        int occupied=towers.FindIndex(t=>t.slot==slot);
        if(occupied>=0)
        {
            selectedType=towers[occupied].type;
            defenseSelectedSlot=slot;
            return;
        }
        int existing=matchDefenseSetup
            ?towers.FindIndex(t=>t.slot==defenseSelectedSlot&&t.type==selectedType)
            :towers.FindIndex(t=>t.type==selectedType);
        if(existing>=0)RemoveDefenseTower(existing);
        else if(towers.Count>=(matchDefenseSetup?matchSettings.TowerLimit:TowerNames.Length))return;
        AddTower(slot,selectedType);
        defenseSelectedSlot=slot;
    }
    void UpdateDefenseEditor()
    {
        Vector2 pointer=RiftorynInput.PointerPosition;
        bool overUi=DefensePointerOverUi(pointer);
        float scroll=RiftorynInput.PointerScroll;
        if(!overUi&&Mathf.Abs(scroll)>.001f)
        {
            Plane plane=new(Vector3.up,new Vector3(0,2.9f,0));
            Ray before=cam.ScreenPointToRay(pointer);
            bool hitBefore=plane.Raycast(before,out float beforeDistance);
            Vector3 anchor=hitBefore?before.GetPoint(beforeDistance):Vector3.zero;
            cam.orthographicSize=Mathf.Clamp(cam.orthographicSize*Mathf.Exp(-scroll*.16f),18f,75f);
            Ray after=cam.ScreenPointToRay(pointer);
            if(hitBefore&&plane.Raycast(after,out float afterDistance))
            {
                Vector3 shift=anchor-after.GetPoint(afterDistance);
                cam.transform.position+=new Vector3(shift.x,0,shift.z);
            }
        }
        if(!overUi&&RiftorynInput.PointerPan)
        {
            Vector2 delta=RiftorynInput.PointerDelta;
            float units=cam.orthographicSize*2f/Mathf.Max(1,Screen.height);
            cam.transform.position+=new Vector3(-delta.x*units,0,-delta.y*units);
        }
        Vector2 keyboard=RiftorynInput.Movement;
        if(keyboard.sqrMagnitude>.001f)
            cam.transform.position+=new Vector3(keyboard.x,0,keyboard.y)*(cam.orthographicSize*.8f*Time.unscaledDeltaTime);
        Vector3 cameraPosition=cam.transform.position;
        cameraPosition.x=Mathf.Clamp(cameraPosition.x,-75f,75f);cameraPosition.z=Mathf.Clamp(cameraPosition.z,-75f,75f);
        cam.transform.position=cameraPosition;
        defenseHoveredSlot=overUi?-1:DefenseSlotAt(pointer);
        if(defenseHoveredSlot>=0&&RiftorynInput.PointerPressed)PlaceDefenseTower(defenseHoveredSlot);
        UpdateDefensePreview();
    }

    void DrawDefenseEditor()
    {
        if(defenseLabel==null)
        {
            defenseLabel=new GUIStyle(small){fontSize=14,fontStyle=FontStyle.Bold,wordWrap=false};
            defenseTiny=new GUIStyle(small){fontSize=12,wordWrap=true};
            defenseButton=new GUIStyle(GUI.skin.button){fontSize=13,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleLeft,padding=new RectOffset(35,6,0,0)};
            defenseMarker=new GUIStyle(GUI.skin.label){fontSize=12,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter,normal={textColor=Color.white}};
        }
        bool en=RiftorynL10n.CurrentCode=="EN";
        for(int i=0;i<slots.Length;i++)
        {
            Vector3 projected=cam.WorldToScreenPoint(Slot(i)+Vector3.up*3.1f);
            if(projected.z<=0)continue;
            Vector2 screen=new(projected.x,projected.y);
            if(DefensePointerOverUi(screen))continue;
            float x=projected.x,y=Screen.height-projected.y;
            if(x<0||x>Screen.width||y<DefenseHeaderHeight||y>Screen.height-46)continue;
            int towerIndex=towers.FindIndex(t=>t.slot==i);
            bool hovered=i==defenseHoveredSlot, selectedMarker=i==defenseSelectedSlot;
            float size=hovered||selectedMarker?30f:22f;
            Color color=towerIndex>=0?TowerColors[towers[towerIndex].type]:new Color(.87f,.72f,.42f);
            HudFill(new Rect(x-size/2-2,y-size/2-2,size+4,size+4),new Color(.045f,.035f,.06f,.92f));
            HudFill(new Rect(x-size/2,y-size/2,size,size),color);
            HudFill(new Rect(x-size/2+3,y-size/2+3,size-6,size-6),new Color(.08f,.07f,.10f,.95f));
            Color old=GUI.color;GUI.color=color;
            GUI.Label(new Rect(x-size/2,y-size/2,size,size),towerIndex>=0?(towers[towerIndex].type+1).ToString():"+",defenseMarker);
            GUI.color=old;
        }
        HudFill(new Rect(0,0,Screen.width,DefenseHeaderHeight),new Color(.045f,.035f,.07f,.98f));
        GUI.Label(new Rect(18,8,Mathf.Max(250,Screen.width-550),30),en?"BUILD DEFENSE":"BUDOWA OBRONY",title);
        string hero1=HeroNames[selectedHero], hero2=allyChoice==1?(en?"NONE":"BRAK"):HeroNames[AllyHero()];
        if(!matchDefenseSetup)
        {
            if(GUI.Button(new Rect(18,48,180,30),"AI 1 · "+hero1)){StartDraft(0,false);GUIUtility.ExitGUI();}
            if(GUI.Button(new Rect(205,48,180,30),"AI 2 · "+hero2)){StartDraft(1,false);GUIUtility.ExitGUI();}
        }
        else GUI.Label(new Rect(18,48,580,30),matchSettings.Opponent?"Ustaw obronę przeciw drużynie AI":"Ustaw wieże do własnej próby",defenseLabel);
        if(GUI.Button(new Rect(Screen.width-DefensePanelWidth-145,48,125,30),RiftorynL10n.CurrentCode+" ⇄"))RiftorynL10n.Toggle();
        Rect panel=DefensePanel;HudFill(panel,new Color(.055f,.045f,.075f,.98f));
        float px=panel.x+15,pw=panel.width-30;
        GUI.Label(new Rect(px,panel.y+12,pw,25),en?"CHOOSE A TOWER":"WYBIERZ WIEŻĘ",defenseLabel);
        GUI.Label(new Rect(px,panel.y+36,pw,24),$"{towers.Count} / {(matchDefenseSetup?matchSettings.TowerLimit:TowerNames.Length)}  ·  "+(matchDefenseSetup?(en?"repeats allowed":"można powtarzać"):(en?"one per element":"po jednej z żywiołu")),defenseTiny);
        float rowY=panel.y+65,rowHeight=Mathf.Clamp((Screen.height-370f)/7f,25f,44f);
        for(int i=0;i<TowerNames.Length;i++)
        {
            int type=i;bool placed=towers.Exists(t=>t.type==type);
            Rect row=new(px,rowY+i*rowHeight,pw,rowHeight-4);
            Color old=GUI.backgroundColor;GUI.backgroundColor=selectedType==i?new Color(.72f,.59f,.38f):Color.white;
            if(GUI.Button(row,RiftorynL10n.Translate(TowerNames[i])+(placed?(en?"   · PLACED":"   · USTAWIONA"):""),defenseButton))
            {
                selectedType=i;
                var tower=towers.Find(t=>t.type==type);
                defenseSelectedSlot=matchDefenseSetup?-1:tower!=null?tower.slot:-1;
            }
            GUI.backgroundColor=old;
            HudFill(new Rect(row.x+9,row.y+7,16,Mathf.Max(12,row.height-14)),TowerColors[i]);
        }
        float detailY=rowY+7*rowHeight+8;
        GUI.Label(new Rect(px,detailY,pw,23),RiftorynL10n.Translate(TowerNames[selectedType]).ToUpperInvariant(),defenseLabel);
        GUI.Label(new Rect(px,detailY+24,pw,36),string.Format(en?"Range: {0:0.0} m   ·   Damage: {1:0}":"Zasięg: {0:0.0} m   ·   Obrażenia: {1:0}",TowerRange[selectedType],TowerDamage[selectedType]),defenseTiny);
        int selected=towers.FindIndex(t=>t.slot==defenseSelectedSlot);
        GUI.enabled=selected>=0;
        if(GUI.Button(new Rect(px,detailY+63,pw,34),en?"REMOVE SELECTED TOWER":"USUŃ WYBRANĄ WIEŻĘ"))
        {
            RemoveDefenseTower(selected);defenseSelectedSlot=-1;
        }
        GUI.enabled=true;
        float buttonY=Screen.height-100;
        GUI.enabled=towers.Count>0;
        if(GUI.Button(new Rect(px,buttonY,pw,40),matchDefenseSetup?"ZATWIERDŹ OBRONĘ →":en?"START TRIAL →":"ROZPOCZNIJ PRÓBĘ →") )
        {
            if(matchDefenseSetup)FinishMatchDefenseSetup();else Begin();
            GUIUtility.ExitGUI();
        }
        GUI.enabled=true;
        if(GUI.Button(new Rect(px,buttonY+45,pw,32),en?"← MAIN MENU":"← MENU GŁÓWNE")){ReturnToMenu();GUIUtility.ExitGUI();}
        HudFill(new Rect(0,Screen.height-46,Screen.width-DefensePanelWidth,46),new Color(.045f,.035f,.07f,.96f));
        string hint=defenseHoveredSlot<0
            ?(en?"Choose a tower, then click a +. Scroll: zoom · right drag / WASD: move map.":"Wybierz wieżę i kliknij +. Kółko: przybliżenie · prawy przycisk / WASD: przesuń mapę.")
            :towers.Exists(t=>t.slot==defenseHoveredSlot)
                ?(en?"Click to select this tower. Its range is highlighted.":"Kliknij wieżę, aby ją wybrać. Jej zasięg jest podświetlony.")
                :(en?"Click to place or move the selected tower. The circle previews its range.":"Kliknij, aby postawić lub przenieść wybraną wieżę. Okrąg pokazuje zasięg.");
        if(matchDefenseSetup&&defenseHoveredSlot<0)
            hint=en?"Choose a type again to add a duplicate. Click a tower, then + to move it.":"Wybierz typ ponownie, aby go powtórzyć. Kliknij wieżę, potem +, aby ją przenieść.";
        GUI.Label(new Rect(18,Screen.height-39,Mathf.Max(100,Screen.width-DefensePanelWidth-30),32),hint,defenseTiny);
    }
}
