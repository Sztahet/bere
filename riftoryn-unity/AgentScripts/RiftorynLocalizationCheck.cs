using System;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public static class RiftorynLocalizationCheck
{
    public static string Run()
    {
        var gaps=new List<string>();int count=0;
        foreach(var collection in LocalizationEditorSettings.GetStringTableCollections())
        foreach(var key in collection.SharedData.Entries)
        foreach(var table in collection.StringTables)
        {
            count++;var entry=table.GetEntry(key.Id);
            if(entry==null||string.IsNullOrWhiteSpace(entry.Value))gaps.Add(collection.TableCollectionName+" / "+table.LocaleIdentifier.Code+" / "+key.Key);
        }
        if(count==0||gaps.Count>0)throw new Exception("Table gaps: "+string.Join(", ",gaps));
        var en=LocalizationEditorSettings.GetLocales().GetEnumerator();
        Locale polish=null,english=null;
        while(en.MoveNext()){if(en.Current.Identifier.Code=="pl")polish=en.Current;if(en.Current.Identifier.Code=="en")english=en.Current;}
        if(polish==null||english==null)throw new Exception("Missing locale");
        LocalizationSettings.SelectedLocale=english;
        string a=RiftorynL10n.Translate("WYBIERZ BOHATERA");
        string b=RiftorynL10n.Translate("Otrzymuje o 15% mniej obrażeń Ognia.");
        string c=RiftorynL10n.Format("Do mety: {0} m",123);
        if(a!="CHOOSE YOUR HERO"||!b.Contains("15%")||c!="To finish: 123 m")throw new Exception($"EN values: {a} / {b} / {c}");
        LocalizationSettings.SelectedLocale=polish;
        if(RiftorynL10n.Translate("WYBIERZ BOHATERA")!="WYBIERZ BOHATERA")throw new Exception("PL switch failed");
        return $"COMPLETE: {count} entries checked, PL/EN switch and formatted strings passed.";
    }
}
