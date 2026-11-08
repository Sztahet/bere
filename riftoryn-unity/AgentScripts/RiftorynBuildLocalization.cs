using System;
using UnityEditor.AddressableAssets.Settings;
public static class RiftorynBuildLocalization
{
    public static string Build()
    {
        AddressableAssetSettings.BuildPlayerContent(out var result);
        if(!string.IsNullOrEmpty(result.Error))throw new Exception(result.Error);
        return "Localization addressables built successfully.";
    }
}
