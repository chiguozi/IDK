using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Animations;

public class ModelTool
{
    static List<string> boneList = new List<string>() { "Bip001" };
    static List<string> loopAnimList = new List<string>() { "idle", "run", "walk" };


    //如果没有Controller 删除animator组件
    static void AddControllerToAnimator(GameObject go, AnimatorController controller)
    {
        var animator = go.GetComponent<Animator>();
        if (controller == null)
        {
            GameObject.DestroyImmediate(animator);
        }
        else
        {
            animator.runtimeAnimatorController = controller;
            animator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }

    [MenuItem("Tools/Model/SetFBXImprotSetting")]
    public static void SetModelImportSetting()
    {
        var objs = Selection.objects;
        HashSet<string> fbxPathSet = new HashSet<string>();

        for (int i = 0; i < objs.Length; i++)
        {
            string assetPath = AssetDatabase.GetAssetPath(objs[i]);
            if (!EditorPath.CheckIsModelPath(assetPath))
                continue;
            assetPath = FormatFbxPath(assetPath);
            if (!fbxPathSet.Contains(assetPath))
                fbxPathSet.Add(assetPath);
        }

        if (fbxPathSet.Count == 0)
        {
            Debug.LogError("没有选择模型fbx");
            return;
        }

        var iter = fbxPathSet.GetEnumerator();
        while (iter.MoveNext())
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(iter.Current);
            SetSingleModelImportSetting(iter.Current);
            CopyAnimations(iter.Current);
            var controller = CreateAnimatorController(iter.Current);
            go = CreatePrefab(EditorPath.GetModelPrefabPath(iter.Current), go);
            AddControllerToAnimator(go, controller);
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        Debug.Log("模型生成完成");
    }

    static string FormatFbxPath(string assetPath)
    {
        if (!assetPath.Contains("@"))
            return assetPath;
        assetPath = assetPath.Split('@')[0] + EditorPath.FBXExt;
        return assetPath;
    }

    static void SetSingleModelImportSetting(string assetPath)
    {
        var assetImproter = AssetImporter.GetAtPath(assetPath);
        var modelImproter = assetImproter as ModelImporter;

        if (modelImproter == null)
            return;
        //关闭Read/Write Enable
        modelImproter.isReadable = false;
        //关闭 BlendShapes  用于模型表情
        modelImproter.importBlendShapes = false;
        //关闭lightMapUV
        modelImproter.generateSecondaryUV = false;

        if (modelImproter.animationType != ModelImporterAnimationType.Generic)
            modelImproter.animationType = ModelImporterAnimationType.Generic;

        if (modelImproter.optimizeGameObjects == false)
            modelImproter.optimizeGameObjects = true;

        //设置暴露的骨骼
        List<string> boneNameList = new List<string>();
        for (int i = 0; i < modelImproter.transformPaths.Length; i++)
        {
            var trPath = modelImproter.transformPaths[i];
            if (string.IsNullOrEmpty(trPath))
                continue;
            for (int j = 0; j < boneList.Count; j++)
            {
                if (trPath.EndsWith(boneList[j]))
                    boneNameList.Add(trPath);
            }
        }

        modelImproter.extraExposedTransformPaths = boneNameList.ToArray();
        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        AssetDatabase.Refresh();
    }

    static void CopyAnimations(string assetPath)
    {
        string assetFolderPath = EditorPath.GetFolderPath(assetPath);
        string fullFolderPath = Application.dataPath + assetFolderPath.Replace("Assets", "");

        FileUtil.FileWalker(fullFolderPath,
            (s, n) =>
            {
                if (s.EndsWith(".FBX") && s.Contains("@"))
                    return true;
                return false;
            }
            ,
            (s, n) =>
            {
                var path = s.Replace(Application.dataPath, "Assets");
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if (clip != null)
                {
                    CopySingleAnimation(path, clip);
                }
            }
            ,
            FileWalkOption.File,
            false
        );

    }

    static void CopySingleAnimation(string path, AnimationClip clip)
    {
        string fileName = clip.name;
        string folderPath = EditorPath.GetFolderPath(path);
        var newClip = new AnimationClip();

        //格式化Clip
        FormatAnimationClip(newClip);

        EditorUtility.CopySerialized(clip, newClip);
        if (!AssetDatabase.IsValidFolder(folderPath + EditorPath.AnimationFolderPath))
        {
            AssetDatabase.CreateFolder(folderPath, EditorPath.AnimationFolderPath.Replace("/", ""));
        }

        for (int i = 0; i < loopAnimList.Count; i++)
        {
            if (fileName.Contains(loopAnimList[i]))
            {
                SetAnimationLoopTime(newClip);
                newClip.wrapMode = WrapMode.Loop;
            }
        }

        AssetDatabase.CreateAsset(newClip, EditorPath.GetAnimationFilePath(folderPath, fileName));
    }

    static void SetAnimationLoopTime(AnimationClip clip)
    {
        var setting = AnimationUtility.GetAnimationClipSettings(clip);
        setting.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, setting);
    }

    static AnimatorController CreateAnimatorController(string assetPath)
    {
        string folder = EditorPath.GetFolderPath(assetPath);
       
        List<AnimationClip> clipList = new List<AnimationClip>();
        if (!AssetDatabase.IsValidFolder(folder + EditorPath.AnimationFolderPath))
            return null;
        var fullPath = EditorPath.AssetPathToFullPath(folder + EditorPath.AnimationFolderPath);
        FileUtil.FileWalker(fullPath,
            (s, n) =>
            {
                return s.EndsWith(".anim");
            },
            (s, n) =>
            {
                string path = EditorPath.FullPathToAssetPath(s);
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                clipList.Add(clip);
            }
            , FileWalkOption.File);
        AnimatorController controller = null;
        if (clipList.Count > 0)
            controller  = AnimatorController.CreateAnimatorControllerAtPath(folder + "/controller.controller");
        for (int i = 0; i < clipList.Count; i++)
        {
            var state = controller.AddMotion(clipList[i]);
            if (clipList[i].name.Contains("idle") || clipList[i].name.Contains("Idle"))
                controller.layers[0].stateMachine.defaultState = state;
        }
        return controller;
    }

    static GameObject CreatePrefab(string path, GameObject go)
    {
        return PrefabUtility.CreatePrefab(path, go, ReplacePrefabOptions.ReplaceNameBased);
    }


    [MenuItem("Assets/Test")]
    static void Test()
    {
        var obj = Selection.GetFiltered<AnimationClip>(SelectionMode.TopLevel);
        FormatAnimationClip(obj[0]);
    }

    //https://answer.uwa4d.com/question/593955b6c42dc04f4d8f7341/%E5%A6%82%E4%BD%95%E9%99%8D%E4%BD%8E%E5%8A%A8%E7%94%BB%E6%96%87%E4%BB%B6%E7%9A%84%E7%B2%BE%E5%BA%A6
    //优化AnimationClip
    static void FormatAnimationClip(AnimationClip theAnimation)
    {
        try
        {
            //去除scale曲线
            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
            {
                string name = theCurveBinding.propertyName.ToLower();
                if (name.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                }
                var curve = AnimationUtility.GetEditorCurve(theAnimation, theCurveBinding);
                if (curve == null || curve.keys == null)
                {
                    //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                    continue;
                }
                Keyframe key;
                Keyframe[] keyFrames = curve.keys;
                for (int i = 0; i < keyFrames.Length; i++)
                {
                    key = keyFrames[i];
                    key.value = float.Parse(key.value.ToString("f3"));
                    key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                    key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                    keyFrames[i] = key;
                }
                curve.keys = keyFrames;
                AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, curve);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}", theAnimation.name, e));
        }
    }
}
