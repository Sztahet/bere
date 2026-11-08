using UnityEditor;

// Portrait assets are authored in 4:5; preserve their proportions on import.
class RiftorynPortraitImport : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (!assetPath.StartsWith("Assets/Resources/Portraits/", System.StringComparison.OrdinalIgnoreCase)) return;
        ((TextureImporter)assetImporter).npotScale = TextureImporterNPOTScale.None;
    }
}
