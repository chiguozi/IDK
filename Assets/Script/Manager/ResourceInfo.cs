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
	public Dictionary<string, Object> assetMap;

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

	public ResourceInfo(string url)
	{
		this.url = url;
	}


}

