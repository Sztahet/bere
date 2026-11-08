using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RiftorynSparkPreviewSetup
{
    [MenuItem("Riftoryn/Utwórz scenę podglądu Iskry")]
    public static void Create()
    {
        const string path="Assets/Resources/Pets/spark-preview.fbx";
        var importer=AssetImporter.GetAtPath(path) as ModelImporter;
        if (importer==null) throw new System.InvalidOperationException("Brak modelu " + path);
        if(importer.animationType!=ModelImporterAnimationType.Legacy || !importer.importAnimation)
        {
            importer.animationType=ModelImporterAnimationType.Legacy;
            importer.importAnimation=true;
            importer.SaveAndReimport();
        }
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        var root=new GameObject("Riftoryn | Podgląd Iskry");
        SceneManager.MoveGameObjectToScene(root,scene);
        root.AddComponent<RiftorynSparkPreview>();
        if(!EditorSceneManager.SaveScene(scene,"Assets/Scenes/PodgladIskry.unity"))
            throw new System.InvalidOperationException("Nie można zapisać sceny podglądu.");
        AssetDatabase.SaveAssets();
    }
}
