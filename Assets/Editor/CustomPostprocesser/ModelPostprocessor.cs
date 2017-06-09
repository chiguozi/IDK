using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelPostprocessor : AssetPostprocessor
{
    void OnPostprocessModel(GameObject go)
    {
        if (EditorPath.CheckIsModelPath(assetPath))
        {
            ProcessModelFBX();
        }
        else if(EditorPath.CheckIsEffectPath(assetPath))
        {
            ProcessEffectFBX();
        }
        else if(EditorPath.CheckIsScenePath(assetPath))
        {
            ProcessSceneFBX();
        }
    }

    //人物模型
    void ProcessModelFBX()
    {
        var imp = assetImporter as ModelImporter;
        imp.isReadable = false;
        imp.importBlendShapes = false;
        imp.generateSecondaryUV = false;
        //人物需要吗？
        imp.importTangents = ModelImporterTangents.None;
        imp.animationCompression = ModelImporterAnimationCompression.Optimal;
        imp.resampleCurves = false;
    }

    void ProcessSceneFBX()
    {
        var imp = assetImporter as ModelImporter;
        imp.isReadable = false;
        imp.importBlendShapes = false;
        imp.importTangents = ModelImporterTangents.None;
        //暂时不需要
        imp.importAnimation = false;
    }

    void ProcessEffectFBX()
    {
        var imp = assetImporter as ModelImporter;
        imp.isReadable = false;
        imp.importBlendShapes = false;
        imp.importTangents = ModelImporterTangents.None;
        imp.generateSecondaryUV = false;
        //暂时不需要
        imp.importAnimation = false;
    }
}
