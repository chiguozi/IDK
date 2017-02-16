using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorPath 
{
	public static string AssetPath = "Assets/";
	public static string ResourcePath = AssetPath + "RosourceLib/";
	public static string PrebfabPath = ResourcePath + "Prefab/";
	public static string UIPrefabPath = PrebfabPath + "UI/";
	public static string ModelPath = ResourcePath + "Model/";
	public static string EffectPath = ResourcePath + "Effect/";
	public static string ScenePath = ResourcePath + "Scene/";
	public static string ShaderPath = ResourcePath + "Shader/";
	public static string CompletePath = ResourcePath + "Complete/";
	public static string BundleByFolderPath = ResourcePath + "BundleByFolder/";
	public static string AudioPath = ResourcePath + "Audio/";
	public static string UIShaderPath = ResourcePath + "Shader/CustomUI";
	
	static string ABExt = ".ab";
	
	public static string GetFullBundleName(string bundleName)
	{
		if(string.IsNullOrEmpty(bundleName))
		{
			return string.Empty;
		}
		if(bundleName.EndsWith(ABExt))
			return bundleName;
		return bundleName + ABExt;
	}
	
	public static string GetBundleRelativePath(string assetPath)
	{
		if(assetPath.StartsWith(ResourcePath))
		{
			assetPath = assetPath.Replace(ResourcePath, string.Empty));
		}
		else
		{
			assetPath = assetPath.Replace(AssetPath, string.Empty);
		}
		return assetPath;
	}
	
	public static string ToLower(string path)
	{
		return path.ToLower();
	}
	
	public static string GetFolderPath(string assetPath)
	{
		return Path.GetDirectoryName(path);
	}
	
	public static string RemoveExtension(string path)
	{
		if(string.IsNullOrEmpty(path))
			return string.Empty;
		var index = path.LastIndexOf(".");
		if(index == -1)
			return path;
		path = path.SubString(0, index);
		return path;
	}
	
	public static string GetPathExt(string path)
	{
		return Path.GetExtension(path);
	}
	
	//@todo  判断路径是否合法（中文，空格等检测）
	public static bool CheckValidPaht(string path)
	{
		return true;
	}
}

