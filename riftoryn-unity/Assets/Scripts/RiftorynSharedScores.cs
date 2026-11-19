using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

sealed class RiftorynSharedScores
{
    public const int Limit=10;
    const int SchemaVersion=1;
    public const string FileName="shared-escape-scores.json";

    [Serializable]
    public sealed class Score
    {
        public string id, playedAtUtc, playerHeroId, allyHeroId;
        public int defenseVersion, defenseSeed, arrived, finishDeciseconds, bestProgressDecimeters;
    }

    [Serializable]
    sealed class Document
    {
        public int schemaVersion;
        public List<Score> scores;
    }

    readonly string path;

    public RiftorynSharedScores(string path) { this.path=path; }

    public bool TryLoad(out IReadOnlyList<Score> scores)
    {
        try
        {
            scores=Read().scores;
            return true;
        }
        catch(Exception error) when(IsStorageError(error))
        {
            scores=Array.Empty<Score>();
            return false;
        }
    }

    // Null means a storage failure. Zero means the result fell outside the ranking.
    public int? Record(Score score)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using var lease=new FileStream(path+".lock",FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);
            var document=Read();
            var previous=document.scores.Find(entry=>entry.id==score.id);
            if(previous!=null) return document.scores.IndexOf(previous)+1;

            document.scores.Add(score);
            document.scores.Sort(Compare);
            int index=document.scores.IndexOf(score);
            if(index>=Limit) return 0;

            Trim(document.scores);
            string temp=path+".tmp";
            File.WriteAllText(temp,JsonUtility.ToJson(document,true));
            if(File.Exists(path)) File.Replace(temp,path,null);
            else File.Move(temp,path);
            return index+1;
        }
        catch(Exception error) when(IsStorageError(error))
        {
            return null;
        }
    }

    Document Read()
    {
        string json;
        try { json=File.ReadAllText(path); }
        catch(FileNotFoundException) { return Empty(); }
        catch(DirectoryNotFoundException) { return Empty(); }

        var document=JsonUtility.FromJson<Document>(json);
        if(document==null||document.schemaVersion!=SchemaVersion||document.scores==null
            ||document.scores.Exists(score=>score==null||score.arrived<0||score.arrived>2
                ||score.finishDeciseconds<0||score.bestProgressDecimeters<0
                ||string.IsNullOrEmpty(score.id)||string.IsNullOrEmpty(score.playedAtUtc)
                ||string.IsNullOrEmpty(score.playerHeroId)||string.IsNullOrEmpty(score.allyHeroId)))
            throw new FormatException("Invalid or unrecognized Shared Escape score file.");

        document.scores.Sort(Compare);
        Trim(document.scores);
        return document;
    }

    static Document Empty()=>new(){schemaVersion=SchemaVersion,scores=new List<Score>()};

    static void Trim(List<Score> scores)
    {
        if(scores.Count>Limit) scores.RemoveRange(Limit,scores.Count-Limit);
    }

    static int Compare(Score left,Score right)
    {
        int result=right.arrived.CompareTo(left.arrived);
        if(result!=0) return result;
        result=left.arrived>0?left.finishDeciseconds.CompareTo(right.finishDeciseconds)
            :right.bestProgressDecimeters.CompareTo(left.bestProgressDecimeters);
        if(result!=0) return result;
        result=string.CompareOrdinal(left.playedAtUtc,right.playedAtUtc);
        return result!=0?result:string.CompareOrdinal(left.id,right.id);
    }

    static bool IsStorageError(Exception error)=>error is IOException or UnauthorizedAccessException or ArgumentException or FormatException;
}
