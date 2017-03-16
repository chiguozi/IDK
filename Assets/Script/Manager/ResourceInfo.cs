using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public enum AssetState
{
	Init,
	Inviald,
	Loading,
	Loaded,
	Unload,
}

public enum LoadType
{
	www,
	downloadOrCache,
}

public class LoadCallbackInfo
{
	public string assetName;
	public Action<Object> callback;
	public LoadCallbackInfo(string assetName, Action<Object> callback)
	{
		this.assetName = assetName;
		this.callback = callback;
	}
}

public class ResourceInfo
{
	public string url;

	public WWW w3;
	public AssetBundle assetbundle;

	public List<ResourceInfo> allDepList = new List<ResourceInfo>();
	public int refCount = 0; 
	public List<LoadCallbackInfo> callbackList = new List<LoadCallbackInfo>();
	public List<Action<float>> progressCallbackList = new List<Action<float>>();
	public AssetState state = AssetState.Init;
	public Object mainAsset;
	public Dictionary<string, Object> assetMap = new Dictionary<string, Object>();
	public Action<ResourceInfo> loadedOperation;

	public LoadType loadType = LoadType.www;
	public bool needAsync = false;

	public void UnCompress()
	{
		var assets = assetbundle.LoadAllAssets();
		for(int i = 0; i < assets.Length; i++)
		{
			if(i == 0)
				mainAsset = assets[i];
			assetMap.Add(assets[i].name, assets[i]);
		}
	}

	public bool isDone
	{
		get {return state == AssetState.Loaded;}
	}

	public bool isInviald 
	{
		get {return state == AssetState.Inviald;}
	}

	public bool isDepLoaded 
	{
		get
		{
			if(state == AssetState.Loaded)
				return true;
			for(int i = 0; i < allDepList.Count; i++)
			{
				if(!isDone && !isInviald)
					return false;
			}
			return true;
		}	
	}

	public void AddRef(int relative)
	{
		refCount += relative;
		for(int i = 0; i < allDepList.Count; i++)
		{
			allDepList[i].AddRef(relative);
		}
		if(refCount == 0)
		{
			
		}
	}

	public Object GetAsset(string assetName)
	{
		if(null == assetMap)
			return null;
		if(assetName == "")
			return mainAsset;
		if(assetMap.ContainsKey(assetName))
			return assetMap[assetName];
		return null;
	}

	public void AddCallbacks(string assetName, Action<Object> callback, Action<float> progress)
	{
		if(null != callback)
		{
			var callbackInfo = new LoadCallbackInfo(assetName, callback);
			callbackList.Add(callbackInfo);
		}

		if(null != progress)
		{
			progressCallbackList.Add(progress);
		}
	}
		

	public void DoCallbacks()
	{
		for(int i = 0; i < callbackList.Count; i++)
		{
			callbackList[i].callback(GetAsset(callbackList[i].assetName));
		}
		callbackList.Clear();
		for(int i = 0; i < progressCallbackList.Count; i++)
		{
			progressCallbackList[i](1);
		}
		progressCallbackList.Clear();
	}

	public ResourceInfo(string url)
	{
		this.url = url;
	}


}

