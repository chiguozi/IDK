using UnityEditor;

public class TexturePostprocessor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        var texImporter = assetImporter as TextureImporter;
        if (EditorPath.IsUIResPath(assetPath))
        {
            ProcessUITexture(texImporter);
        }
    }

    void ProcessUITexture( TextureImporter texImporter)
    {
        //@todo  特殊的图集需要一张图片一个包
        // 没有alpha通道的 不需要生成通道图
     
        texImporter.textureType = TextureImporterType.Sprite;
        texImporter.mipmapEnabled = false;
        texImporter.isReadable = false;


        TextureImporterPlatformSettings setting = new TextureImporterPlatformSettings();
        setting.allowsAlphaSplitting = true;
        setting.maxTextureSize = 1024;
        setting.textureCompression = TextureImporterCompression.Compressed;
        setting.name = "Android";
        setting.format = TextureImporterFormat.ETC_RGB4;
        setting.overridden = true;
        texImporter.SetPlatformTextureSettings(setting);
    }

}
