using System;
using UnityEngine;

[Serializable] public class SailaStoryBlock { public string text; public int image; public bool pending; public int width, height; }
[Serializable] public class SailaStoryChapter { public string title; public SailaStoryBlock[] blocks; }
[Serializable] public class SailaStoryData { public string title; public SailaStoryChapter[] chapters; }
[Serializable] public class StoryApprovalEntry { public string folder; public bool draft; }
[Serializable] public class StoryApprovalCatalog { public StoryApprovalEntry[] stories; }

public partial class RiftorynMenu
{
    bool sailaReaderOpen, sailaStoryDraft;
    int sailaChapter;
    Vector2 sailaScroll;
    SailaStoryData sailaStory;
    bool storyEnglish;
    string sailaStoryFolder="SailaStory", sailaStoryTitle="CIĘŻAR KRYSZTAŁU", storyHeroName="SAILA";

    StoryApprovalCatalog storyApproval;
    bool storyApprovalLoaded;
    bool StoryNeedsApproval(string folder, bool fallback=false)
    {
        if(!storyApprovalLoaded)
        {
            var asset=Resources.Load<TextAsset>("story_approval");
            if(asset)storyApproval=JsonUtility.FromJson<StoryApprovalCatalog>(asset.text);
            storyApprovalLoaded=true;
        }
        if(storyApproval?.stories!=null)foreach(var item in storyApproval.stories)
            if(item!=null&&item.folder==folder)return item.draft;
        return fallback;
    }

    void OpenSailaStory(string folder, string title, string heroName="SAILA", bool draft=false)
    {
        sailaStoryDraft=StoryNeedsApproval(folder,draft);
        sailaStoryFolder=folder;
        sailaStoryTitle=title;
        storyHeroName=heroName;
        sailaStory=null;sailaReaderOpen=true;sailaChapter=0;sailaScroll=Vector2.zero;
    }
    GUIStyle sailaTitle, sailaBody, sailaSmall, sailaButton;

    static float SailaImageHeight(SailaStoryBlock block, float width) =>
        block.width > 0 && block.height > 0 ? width * block.height / block.width : width * 2f / 3f;

    void DrawSailaReader()
    {
        if(sailaStory==null){var asset=RiftorynL10n.CurrentCode=="EN"?Resources.Load<TextAsset>(sailaStoryFolder+"/story_en"):null;storyEnglish=asset!=null;if(!asset)asset=Resources.Load<TextAsset>(sailaStoryFolder+"/story");if(asset)sailaStory=JsonUtility.FromJson<SailaStoryData>(asset.text);}
        float w=Screen.width,h=Screen.height;
        Fill(new Rect(0,0,w,h),bg);
        if(sailaTitle==null){sailaTitle=Style(27,FontStyle.Bold,pale,true);sailaBody=Style(19,FontStyle.Normal,pale,true);sailaSmall=Style(13,FontStyle.Bold,gold,true);sailaButton=new GUIStyle(GUI.skin.button){fontSize=14};}
        float pad=Mathf.Clamp(w*.025f,10,30), bar=48;
        if(GUI.Button(new Rect(pad,8,112,bar-10),storyEnglish?"← CODEX":"← KODEKS",sailaButton)){sailaReaderOpen=false;return;}
        float right=w-pad;
        if(GUI.Button(new Rect(right-44,8,44,bar-10),"→",sailaButton) && sailaStory!=null && sailaChapter<sailaStory.chapters.Length-1){sailaChapter++;sailaScroll=Vector2.zero;}
        if(GUI.Button(new Rect(right-94,8,44,bar-10),"←",sailaButton) && sailaChapter>0){sailaChapter--;sailaScroll=Vector2.zero;}
        GUI.Label(new Rect(pad+120,15,Mathf.Max(30,w-pad*2-230),25),storyHeroName+"  /  "+(storyEnglish&&!string.IsNullOrEmpty(sailaStory?.title)?sailaStory.title:sailaStoryTitle),sailaSmall);
        if(sailaStory==null||sailaStory.chapters==null||sailaStory.chapters.Length==0){GUI.Label(new Rect(pad,70,w-pad*2,60),"Nie udało się wczytać opowieści.",sailaBody);return;}
        var chapter=sailaStory.chapters[Mathf.Clamp(sailaChapter,0,sailaStory.chapters.Length-1)];
        float width=Mathf.Min(780,w-pad*2-20), viewHeight=Mathf.Max(20,h-65), y=75;
        if(width<50)return;
        float total=75;
        foreach(var block in chapter.blocks)
        {
            if(block.pending) continue;
            total += block.image > 0
                ? SailaImageHeight(block, width) + 32
                : sailaBody.CalcHeight(new GUIContent(block.text ?? ""), width) + 22;
        }
        total+=70;
        float viewWidth=Mathf.Min(w-pad*2,840), x=(w-viewWidth)*.5f;
        sailaScroll=GUI.BeginScrollView(new Rect(x,55,viewWidth,viewHeight),sailaScroll,new Rect(0,0,viewWidth-20,total));
        float inner=(viewWidth-20-width)*.5f;
        GUI.Label(new Rect(inner,5,width,25),(storyEnglish?"CHAPTER ":"ROZDZIAŁ ")+(sailaChapter+1)+" / "+sailaStory.chapters.Length+(storyEnglish?"  ·  ENGLISH":"  ·  WERSJA POLSKA")+(sailaStoryDraft?(storyEnglish?"  ·  ✦ PENDING APPROVAL":"  ·  ✦ DO ZATWIERDZENIA"):""),sailaSmall);
        GUI.Label(new Rect(inner,30,width,45),chapter.title,sailaTitle);
        foreach(var block in chapter.blocks)
        {
            if(block.pending) continue;
            if(block.image>0){var image=Resources.Load<Texture2D>(sailaStoryFolder+"/"+block.image.ToString("00"));float height=SailaImageHeight(block, width);if(image)GUI.DrawTexture(new Rect(inner,y,width,height),image,ScaleMode.ScaleToFit);y+=height+32;}
            else{string text=block.text??"";float height=sailaBody.CalcHeight(new GUIContent(text),width);GUI.Label(new Rect(inner,y,width,height+3),text,sailaBody);y+=height+22;}
        }
        GUI.Label(new Rect(inner,y,width,30),sailaChapter==sailaStory.chapters.Length-1?(storyEnglish?"✦  END OF STORY":"✦  KONIEC OPOWIEŚCI"):(storyEnglish?"✦  NEXT CHAPTER: USE THE ARROW ABOVE":"✦  KOLEJNY ROZDZIAŁ: STRZAŁKA U GÓRY"),sailaSmall);
        GUI.EndScrollView();
    }
}
