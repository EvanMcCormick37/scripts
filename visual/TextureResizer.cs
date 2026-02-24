using UnityEngine;
using UnityEditor;

public class TextureResozer : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter tex_importer = assetImporter as TextureImporter;
        tex_importer.maxTextureSize = 256; // Set your desired default max size here
    }
}
