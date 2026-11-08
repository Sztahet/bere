using UnityEditor;
using UnityEngine;

// Explicit opt-in: the experimental companion is never spawned in the ordinary trial.
public static class RiftorynSparkFieldSetup
{
    [MenuItem("Riftoryn/Iskra — włącz podgląd w próbie")]
    public static void Enable()
    {
        if(!EditorApplication.isPlaying)throw new System.InvalidOperationException("Włącz próbę w Play Mode.");
        var player=GameObject.Find("Gracz");if(!player)throw new System.InvalidOperationException("Najpierw rozpocznij próbę.");
        if(Object.FindAnyObjectByType<RiftorynSparkFollower>())return;
        var root=new GameObject("Iskra — podgląd w labiryncie");var follower=root.AddComponent<RiftorynSparkFollower>();follower.Initialize(player.transform,false);
    }
    [MenuItem("Riftoryn/Iskra — test trafienia")]
    public static void Hit(){var follower=Object.FindAnyObjectByType<RiftorynSparkFollower>();if(follower)follower.TriggerHit();}
    [MenuItem("Riftoryn/Iskra — przełącz obrys")]
    public static void ToggleOutline(){var follower=Object.FindAnyObjectByType<RiftorynSparkFollower>();if(follower)follower.SetOutline(!follower.outlineEnabled);}
}
