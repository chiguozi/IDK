using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum AssetCategory
{
	None,
	Prefab,
	UI,
	Font,
	Effect,
	Model,
	Scene,
	Shader,
	Complete,
	BundleByFolder,
	Audio,
}

public class ABNameHandler
{
	HashSet<AssetType> _nameTypeSet = new HashSet<AssetType>();
	bool _useFileName;
	Action <string, bool, HashSet<AssetType>> _handler;
	
	public ABNameHandler(bool useFileName, Action <string, bool, HashSet<AssetType>> handler, params AssetType[] fmts)
	{
		_useFileName = useFileName;
		_handler = handler;
		if(fmts == null || fmts.Length == 0)
			return;
		for(int i = 0; i < fmts.Length; i++)
		{
			_nameTypeSet.Add(fmts[i]);
		}
	}
	
	public void Handle(string assetPath)
	{
		if(null != _handler)
		{
			_handler(assetPath, _useFileName, _nameTypeSet);
		}
	}
}

public class BundleNameSetter
{
	static Dictionary<AssetCategory, ABNameHandler> _ABNameHandlerMap = new Dictionary<AssetCategory, ABNameHandler>();
	
	static Dictionary<string, AssetCategory> _path2AssetCategoryMap = new Dictionary<string, AssetCategory>()
	{
		{EditorPath.PrebfabPath, AssetCategory.Prefab},
		{EditorPath.EffectPath, AssetCategory.Effect},
		{EditorPath.ModelPath, AssetCategory.Model},
		{EditorPath.AudioPath, AssetCategory.Audio},
		{EditorPath.ShaderPath, AssetCategory.Shader},
		{EditorPath.CompletePath, AssetCategory.Complete},
		{EditorPath.BundleByFolderPath, AssetCategory.BundleByFolder},
		{EditorPath.ScenePath, AssetCategory.Scene},
	};
	
	static HashSet<string> _ignoreBundleExts = new HashSet<string>()
	{
		".meta", ".cs", ".mainfest", ".lua", ".dll"
	};
	
	public static void HandleAssetBundleName(string assetPath)
	{
		if(CheckIgnoreExts(assetPath))
			return;
		if(!EditorPath.CheckValidPath(assetPath))
			return;
		var ac = GetAssetCategory(assetPath);
		if(_ABNameHandlerMap.ContainsKey(ac))
		{
			_ABNameHandlerMap[ac].Handle(assetPath);
		}
	}
	
	static BundleNameSetter()
	{
		InitABNameMap();
	}
	
	static void InitABNameMap()
	{
		_ABNameHandlerMap.Add(AssetCategory.Prefab, new ABNameHandler(true, PrefabBundleNameHandler, AssetType.Prefab));
		_ABNameHandlerMap.Add(AssetCategory.Model, new ABNameHandler(true, DefaultBundleNameHandler, AssetType.Texture, AssetType.Material, AssetType.AnimControl));
		_ABNameHandlerMap.Add(AssetCategory.Effect, new ABNameHandler(true, DefaultBundleNameHandler, AssetType.Texture, AssetType.Material));
		_ABNameHandlerMap.Add(AssetCategory.Scene, new ABNameHandler(true, DefaultBundleNameHandler, AssetType.Texture, AssetType.Material, AssetType.LightMap, AssetType.Txt));
		_ABNameHandlerMap.Add(AssetCategory.Complete, new ABNameHandler(true, BundleWithoutFmtsHandler));
		_ABNameHandlerMap.Add(AssetCategory.BundleByFolder, new ABNameHandler(false, BundleWithoutFmtsHandler));
		_ABNameHandlerMap.Add(AssetCategory.Shader, new ABNameHandler(true, ShaderBundleNameHandler, AssetType.Shader));
		_ABNameHandlerMap.Add(AssetCategory.Audio, new ABNameHandler(true, DefaultBundleNameHandler, AssetType.Audio));
		//UI相关暂不处理
		//_ABNameHandlerMap.Add(AssetCategory.UI, new ABNameHandler(true, PrefabBundleNameHandler, AssetType.Prefab));
		//_ABNameHandlerMap.Add(AssetCategory.Font, new ABNameHandler(true, PrefabBundleNameHandler, AssetType.Prefab));
	}
	
	
	//-------设置bundleName----------------------
	static void SetBundleName(string assetPath, string bundleName, string variantName = null)
	{
		AssetImporter ai = AssetImporter.GetAtPath(assetPath);
		if(ai == null)
			return;
		if(bundleName == null)
			bundleName = string.Empty;
		ai.assetBundleName = bundleName;
		if(!string.IsNullOrEmpty(bundleName) && variantName != null)
			ai.assetBundleVariant = variantName;
	}
	
	static void SetBundleNameNone(string assetPath)
	{
		SetBundleName(assetPath, string.Empty);
	}
	
	static void SetBundleNameByFileName(string assetPath, string variantName = null)
	{
		var bundleName = GetFormatBundkeRelativePath(assetPath);
		SetBundleName(assetPath, bundleName);
	}
	
	static void SetBundleNameByFolder(string assetPath, string variantName = null)
	{
		var bundleName = GetFormatBundkeRelativePath(assetPath);
		bundleName = EditorPath.GetFolderPath(bundleName);
		SetBundleName(assetPath, bundleName);
	}
	//----------------------------------------------------Handlers-----------------------
	
	static void DefaultBundleNameHandler(string assetPath, bool useFileName, HashSet<AssetType> fmts)
	{
		var fmt = AssetFormat.GetPathFormat(assetPath);
		if(!fmts.Contains(fmt))
		{
			SetBundleNameNone(assetPath);
			return;
		}
		if(useFileName)
		{
			SetBundleNameByFileName(assetPath);
		}
		else
		{
			SetBundleNameByFolder(assetPath);
		}
	}
	
	//UI预设暂不处理
	static void PrefabBundleNameHandler(string assetPath, bool useFileName, HashSet<AssetType> fmts)
	{
		if(assetPath.StartsWith(EditorPath.UIPrefabPath))
		{
			SetBundleNameNone(assetPath);
		}
		else
		{
			DefaultBundleNameHandler(assetPath, useFileName, fmts);
		}
	}
	
	static void BundleWithoutFmtsHandler(string assetPath, bool useFileName, HashSet<AssetType> fmts)
	{
		var fmt = AssetFormat.GetPathFormat(assetPath);
		if(fmt != AssetType.None)
		{
			if(useFileName)
			{
				SetBundleNameByFileName(assetPath);
			}
			else
			{
				SetBundleNameByFolder(assetPath);
			}
		}
	}
	
	//shader都在一个包中，UIShader单独处理
	static void ShaderBundleNameHandler(string assetPath, bool useFileName, HashSet<AssetType> fmts)
	{
		if(assetPath.StartsWith(EditorPath.UIShaderPath))
		{
			SetBundleNameNone(assetPath);
		}
		else
		{
			SetBundleName(assetPath, "shader.shader");
		}
	}
	
	//----------------------------辅助函数----------------------------------------
	
	static string GetFormatBundkeRelativePath(string assetPath)
	{
		var bundleName = EditorPath.GetBundleRelativePath(assetPath);
		bundleName = EditorPath.ToLower(bundleName);
		if(CheckNeedRemoveExtension(bundleName))
			bundleName = EditorPath.RemoveExtension(bundleName);
		return bundleName;
	}
	
	// 移除后缀名
	static bool CheckNeedRemoveExtension(string path)
	{
		var fmt = AssetFormat.GetPathFormat(path);
		if(fmt == AssetType.Prefab || fmt == AssetType.Font)
			return true;
		return false;
	}
	
	static bool CheckIgnoreExts(string assetPath)
	{
		var ext = EditorPath.GetPathExt(assetPath);
		return _ignoreBundleExts.Contains(ext);
	}
	
	static AssetCategory GetAssetCategory(string assetPath)
	{
		AssetCategory ac = AssetCategory.None;
		var iter = _path2AssetCategoryMap.GetEnumerator();
		while(iter.MoveNext())
		{
			if(assetPath.StartsWith(iter.Current.Key))
			{
				ac = iter.Current.Value;
				break;
			}
		}
		return ac;
	}
	
}
