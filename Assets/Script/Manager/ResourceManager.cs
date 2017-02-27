using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager
{
	//ab名----》 直接依赖
	static Dictionary<string, List<string>> _dependenciesMap = new Dictionary<string, List<string>>();
	
	public static void Init()
	{
		string abManifestPath = string.Format("{0}/android/android", Application.streamingAssetsPath);
		var ab = LoadAssetSync(abManifestPath);
		// 资源名称为AssetBundleManifest
		var abManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		InitDependencies(abManifest);
		//依赖记录完成后，可以卸载AssetBundleManifest  清楚NotSave下的AssetBundleManifest
		ab.Unload(true);
	}
	
	// 收集所有依赖（直接依赖）
	public static void InitDependencies(AssetBundleManifest abManifest)
	{
		var allBundleNames = abManifest.GetAllAssetBundles();
		for(int i = 0; i < allBundleNames.Length; i++)
		{
			var directDps = abManifest.GetDirectDependencies(allBundleNames[i]);
			if(directDps.Length > 0)
			{
				var dpList = new List<string>(directDps);
				_dependenciesMap.Add(allBundleNames[i], dpList);
			}
		}
	}
	
	
	// url 为绝对路径，不能包含file:///协议
	public static AssetBundle LoadAssetSync(string path)
	{
		return AssetBundle.LoadFromFile(path);
	}
	
	public static void Load(string url, Action<object> callback)
	{
		var obj = Resources.Load(url);
		if(null != callback)
		{
			callback(obj);
		}
	}
	
	
}
