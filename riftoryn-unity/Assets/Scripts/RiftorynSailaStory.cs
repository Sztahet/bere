using System;
using UnityEngine;

[Serializable] public class SailaStoryBlock { public string text; public int image; }
[Serializable] public class SailaStoryChapter { public string title; public SailaStoryBlock[] blocks; }
[Serializable] public class SailaStoryData { public SailaStoryChapter[] chapters; }

public partial class RiftorynMenu
{
    bool sailaReaderOpen;
    int sailaChapter;
    Vector2 sailaScroll;
    SailaStoryData sailaStory;
    GUIStyle sailaTitle, sailaBody, sailaSmall, sailaButton;

    void DrawSailaReader()
    {
        if(sailaStory==null){var asset=Resources.Load<TextAsset>("SailaStory/story");if(asset)sailaStory=JsonUtility.FromJson<SailaStoryData>(asset.text);}
        float w=Screen.width,h=Screen.height;
        Fill(new Rect(0,0,w,h),bg);
        if(sailaTitle==null){sailaTitle=Style(27,FontStyle.Bold,pale,true);sailaBody=Style(19,FontStyle.Normal,pale,true);sailaSmall=Style(13,FontStyle.Bold,gold,true);sailaButton=new GUIStyle(GUI.skin.button){fontSize=14};}
        float pad=Mathf.Clamp(w*.025f,10,30), bar=48;
        if(GUI.Button(new Rect(pad,8,112,bar-10),"← KODEKS",sailaButton)){sailaReaderOpen=false;return;}
        float right=w-pad;
        if(GUI.Button(new Rect(right-44,8,44,bar-10),"→",sailaButton) && sailaStory!=null && sailaChapter<sailaStory.chapters.Length-1){sailaChapter++;sailaScroll=Vector2.zero;}
        if(GUI.Button(new Rect(right-94,8,44,bar-10),"←",sailaButton) && sailaChapter>0){sailaChapter--;sailaScroll=Vector2.zero;}
        GUI.Label(new Rect(pad+120,15,Mathf.Max(30,w-pad*2-230),25),"SAILA  /  CIĘŻAR KRYSZTAŁU",sailaSmall);
        if(sailaStory==null||sailaStory.chapters==null||sailaStory.chapters.Length==0){GUI.Label(new Rect(pad,70,w-pad*2,60),"Nie udało się wczytać opowieści.",sailaBody);return;}
        var chapter=sailaStory.chapters[Mathf.Clamp(sailaChapter,0,sailaStory.chapters.Length-1)];
        float width=Mathf.Min(780,w-pad*2-20), viewHeight=Mathf.Max(20,h-65), y=75;
        if(width<50)return;
        float total=75;
        foreach(var block in chapter.blocks){if(block.image>0)total+=width*2f/3f+32;else total+=sailaBody.CalcHeight(new GUIContent(block.text??""),width)+22;}
        total+=70;
        float viewWidth=Mathf.Min(w-pad*2,840), x=(w-viewWidth)*.5f;
        sailaScroll=GUI.BeginScrollView(new Rect(x,55,viewWidth,viewHeight),sailaScroll,new Rect(0,0,viewWidth-20,total));
        float inner=(viewWidth-20-width)*.5f;
        GUI.Label(new Rect(inner,5,width,25),"ROZDZIAŁ "+(sailaChapter+1)+" / "+sailaStory.chapters.Length+"  ·  WERSJA POLSKA",sailaSmall);
        GUI.Label(new Rect(inner,30,width,45),chapter.title,sailaTitle);
        foreach(var block in chapter.blocks)
        {
            if(block.image>0){var image=Resources.Load<Texture2D>("SailaStory/"+block.image.ToString("00"));float height=width*2f/3f;if(image)GUI.DrawTexture(new Rect(inner,y,width,height),image,ScaleMode.ScaleToFit);y+=height+32;}
            else{string text=block.text??"";float height=sailaBody.CalcHeight(new GUIContent(text),width);GUI.Label(new Rect(inner,y,width,height+3),text,sailaBody);y+=height+22;}
        }
        GUI.Label(new Rect(inner,y,width,30),sailaChapter==sailaStory.chapters.Length-1?"✦  KONIEC OPOWIEŚCI":"✦  KOLEJNY ROZDZIAŁ: STRZAŁKA U GÓRY",sailaSmall);
        GUI.EndScrollView();
    }
}
