using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class RiftorynSceneSetup {
    public static string Create() {
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        new GameObject("Riftoryn | Próba Bohatera").AddComponent<RiftorynTrial>();
        if(!AssetDatabase.IsValidFolder("Assets/Scenes"))AssetDatabase.CreateFolder("Assets","Scenes");
        EditorSceneManager.SaveScene(scene,"Assets/Scenes/ProbaBohatera.unity");
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/ProbaBohatera.unity",true)};
        AssetDatabase.SaveAssets();
        return "Scena i ustawienia builda zapisane";
    }
}
