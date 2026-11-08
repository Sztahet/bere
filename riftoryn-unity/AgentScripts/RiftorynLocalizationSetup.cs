using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public static class RiftorynLocalizationSetup
{
    [Serializable] public class Row { public string key,pl,en; }
    [Serializable] public class Seed { public Row[] rows; }
    public static string Create()
    {
        const string dir="Assets/Localization";
        if(!AssetDatabase.IsValidFolder(dir))AssetDatabase.CreateFolder("Assets","Localization");
        var settings=LocalizationEditorSettings.ActiveLocalizationSettings;
        if(settings==null)
        {
            var guids=AssetDatabase.FindAssets("t:LocalizationSettings",new[]{"Assets"});
            settings=guids.Length>0?AssetDatabase.LoadAssetAtPath<LocalizationSettings>(AssetDatabase.GUIDToAssetPath(guids[0])):null;
            if(settings==null){settings=ScriptableObject.CreateInstance<LocalizationSettings>();AssetDatabase.CreateAsset(settings,dir+"/LocalizationSettings.asset");}
            LocalizationEditorSettings.ActiveLocalizationSettings=settings;
        }
        var locales=new List<Locale>();
        foreach(string code in new[]{"pl","en"})
        {
            var locale=LocalizationEditorSettings.GetLocales().FirstOrDefault(x=>x.Identifier.Code==code);
            if(locale==null){locale=Locale.CreateLocale(code);AssetDatabase.CreateAsset(locale,dir+"/"+code+".asset");LocalizationEditorSettings.AddLocale(locale);}
            locales.Add(locale);
        }
        settings.GetStartupLocaleSelectors().Clear();
        settings.GetStartupLocaleSelectors().Add(new PlayerPrefLocaleSelector());
        settings.GetStartupLocaleSelectors().Add(new SpecificLocaleSelector{LocaleId=new LocaleIdentifier("pl")});
        var collection=LocalizationEditorSettings.GetStringTableCollection("RiftorynUI") ?? LocalizationEditorSettings.CreateStringTableCollection("RiftorynUI",dir,locales);
        var seed=Newtonsoft.Json.JsonConvert.DeserializeObject<Seed>(System.IO.File.ReadAllText("AgentScripts/localization-seed.json"));
        if(seed==null||seed.rows==null||seed.rows.Length==0)throw new Exception("Missing translation seed.");
        foreach(var table in collection.StringTables)
        {
            bool english=table.LocaleIdentifier.Code=="en";
            foreach(var row in new[]{("language.label","JĘZYK","LANGUAGE"),("language.polish","Polski","Polish"),("language.english","Angielski","English")})
            {
                var entry=table.GetEntry(row.Item1)??table.AddEntry(row.Item1,english?row.Item3:row.Item2);
                entry.Value=english?row.Item3:row.Item2;
            }
            foreach(var row in seed.rows)
            {
                if(string.IsNullOrWhiteSpace(row.pl)||string.IsNullOrWhiteSpace(row.en))throw new Exception("Missing translation: "+row.key);
                var entry=table.GetEntry(row.key)??table.AddEntry(row.key,english?row.en:row.pl);
                entry.Value=english?row.en:row.pl;
            }
            EditorUtility.SetDirty(table);
        }
        EditorUtility.SetDirty(settings);EditorUtility.SetDirty(collection.SharedData);EditorUtility.SetDirty(collection);
        LocalizationEditorSettings.EditorEvents.RaiseCollectionModified(null,collection);
        AssetDatabase.SaveAssets();
        return $"Created pl/en locales and {seed.rows.Length} localized strings.";
    }
}


