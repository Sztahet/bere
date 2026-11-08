using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization.Settings;

// IMGUI screens resolve their text through the project's String Tables.
public static class RiftorynL10n
{
    static readonly Dictionary<string,string> cache=new();
    static readonly Dictionary<string,string> englishPetNames=new(StringComparer.OrdinalIgnoreCase){["Iskra"]="Spark",["Pati"]="Patsy",["Patka"]="Patsy",["Szron"]="Frost",["Grom"]="Thunder",["Krzemień"]="Flint",["Blask"]="Gleam",["Cień"]="Shadow"};
    static string EnglishPetNames(string text)=>Regex.Replace(text,@"\b(Iskra|Pati|Patka|Szron|Grom|Krzemień|Blask|Cień)\b",match=>{string value=englishPetNames[match.Value];return match.Value==match.Value.ToUpperInvariant()?value.ToUpperInvariant():value;},RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
    static string cachedLocale;
    public static string Key(string text)
    {
        unchecked { uint hash=2166136261;foreach(byte b in Encoding.UTF8.GetBytes(text)){hash^=b;hash*=16777619;}return "text_"+hash.ToString("x8"); }
    }
    public static string Translate(string text)
    {
        if(string.IsNullOrEmpty(text))return text;
        var locale=LocalizationSettings.SelectedLocale;
        if(locale==null)return text;
        string code=locale.Identifier.Code;
        if(cachedLocale!=code){cache.Clear();cachedLocale=code;}
        if(code=="pl")return text;
        if(cache.TryGetValue(text,out var result))return result;
        var table=LocalizationSettings.StringDatabase.GetTable("RiftorynUI",locale);
        if(table==null)return EnglishPetNames(text);
        var entry=table.GetEntry(Key(text));
        if(entry==null)return EnglishPetNames(text);
        result=LocalizationSettings.StringDatabase.GetLocalizedString("RiftorynUI",Key(text),locale);
        result=EnglishPetNames(result);cache[text]=result;return result;
    }
    public static string Format(string text,params object[] args)
    {
        var locale=LocalizationSettings.SelectedLocale;
        return string.Format(locale?.Identifier.CultureInfo??System.Globalization.CultureInfo.InvariantCulture,Translate(text),args);
    }
    public static string Number(string value)
    {
        if(!float.TryParse(value,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.GetCultureInfo("pl"),out float n))return value;
        var locale=LocalizationSettings.SelectedLocale;
        return n.ToString("0.0",locale?.Identifier.CultureInfo??System.Globalization.CultureInfo.InvariantCulture);
    }
    public static void Toggle()
    {
        var current=LocalizationSettings.SelectedLocale;
        var locale=LocalizationSettings.AvailableLocales.GetLocale(current!=null&&current.Identifier.Code=="en"?"pl":"en");
        if(locale!=null){LocalizationSettings.SelectedLocale=locale;PlayerPrefs.SetString("selected-locale",locale.Identifier.Code);PlayerPrefs.Save();}
    }
    public static string CurrentCode => LocalizationSettings.SelectedLocale?.Identifier.Code=="en"?"EN":"PL";
    public static void Label(Rect rect,string text,GUIStyle style)=>GUI.Label(rect,Translate(text),style);
    public static void Label(Rect rect,string text)=>GUI.Label(rect,Translate(text));
    public static bool Button(Rect rect,string text)=>GUI.Button(rect,Translate(text));
    public static bool Button(Rect rect,string text,GUIStyle style)=>GUI.Button(rect,Translate(text),style);
    public static bool Button(Rect rect,GUIContent content,GUIStyle style)=>GUI.Button(rect,content,style);
    public static int SelectionGrid(Rect rect,int selected,string[] texts,int columns)
    {
        var localized=new string[texts.Length];for(int i=0;i<texts.Length;i++)localized[i]=Translate(texts[i]);
        return GUI.SelectionGrid(rect,selected,localized,columns);
    }
}
