using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public class ResourceManager
{
	//ab名----》 所有依赖
	static Dictionary<string, List<string>> _dependenciesMap = new Dictionary<string, List<string>>();
	static Dictionary<string, ResourceInfo> _resourceInfoMap = new Dictionary<string, ResourceInfo>();
	
	public static void Init()
	{
		string abManifestPath = string.Format("{0}/{1}", SystemConfig.streamPath, SystemConfig.GetPlatformName());
		var ab = LoadAssetSync(abManifestPath);
		// 资源名称为AssetBundleManifest
		var abManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		InitDependencies(abManifest);
		//依赖记录完成后，可以卸载AssetBundleManifest  清楚NotSave下的AssetBundleManifest
		ab.Unload(true);
	}
	
	// 收集所有依赖（全部依赖）
	// unity assetbundle 加载完成并解压后，会自动查找依赖关系，和加载顺序关联不大，保证主资源最后加载即可。
	public static void InitDependencies(AssetBundleManifest abManifest)
	{
		var allBundleNames = abManifest.GetAllAssetBundles();
		for(int i = 0; i < allBundleNames.Length; i++)
		{
			var allDeps = abManifest.GetAllDependencies(allBundleNames[i]);
			if(allDeps.Length > 0)
			{
				var dpList = new List<string>(allDeps);
				_dependenciesMap.Add(allBundleNames[i], dpList);
			}
		}
	}



	public static ResourceInfo GetResourceInfo(string url, bool autoCreate = true)
	{
		if(_resourceInfoMap.ContainsKey(url))
			return _resourceInfoMap[url];
		if(!autoCreate)
			return null;
		var resourceInfo = new ResourceInfo(url);
		if(_dependenciesMap.ContainsKey(url))
		{
			//resourceInfo.allDepList = _dependenciesMap[url];
			for(int i = 0; i < _dependenciesMap[url].Count; i++)
			{
				var depResInfo = GetResourceInfo(_dependenciesMap[url][i]);
				resourceInfo.allDepList.Add(depResInfo);
			}
		}
		_resourceInfoMap.Add(url, resourceInfo);
		return resourceInfo;
	}

	public static void LoadAsset(string url, Action<Object> callback, string assetName = "", Action<float> progress = null)
	{
		var resourceInfo = GetResourceInfo(url);
		if(resourceInfo.state == AssetState.Loaded)
		{
			LoadLoadedResource(resourceInfo, callback, assetName, progress);
			return ;
		}
		if(resourceInfo.state == AssetState.Loading)
		{
			LoadLoadingResource(resourceInfo, callback, assetName, progress);
			return;
		}
		if(resourceInfo.state == AssetState.Inviald)
		{
			return;
		}
	}

	static void LoadLoadingResource(ResourceInfo resourceInfo, Action<Object> callback, string assetName, Action<float> progress)
	{
		resourceInfo.AddRef(1);
		resourceInfo.AddCallbacks(assetName, callback, progress);
	}

	static void LoadLoadedResource(ResourceInfo resourceInfo, Action<Object> callback, string assetName, Action<float> progress)
	{
		resourceInfo.AddRef(1);
		var obj = resourceInfo.GetAsset(assetName);
		if(null != callback)
		{
			callback(obj);
		}
		if(null != progress)
		{
			progress(1);
		}
	}




	static void OnRequiredResourceInfoLoaded(ResourceInfo info)
	{
		info.loadedOperation = null;
		info.UnCompress();
		info.state = AssetState.Loaded;
		info.DoCallbacks();
	}

	static void OnDepResourceInfoLoaded(ResourceInfo info)
	{
		info.loadedOperation = null;
		info.UnCompress();
		info.state = AssetState.Loaded;
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


    public static void LoadResAsset(string url, Action<Object> callback)
    {
        var obj = Resources.Load(url);
        callback(obj);
    }
	
}
