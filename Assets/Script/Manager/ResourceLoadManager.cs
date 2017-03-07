using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadManager 
{
	static List<ResourceInfo> _waitingLoadingList = new List<ResourceInfo>();
	static HashSet<string> _loadingOrWaitingSet = new HashSet<string>();
	//处理progress回调
	static List<ResourceInfo> _loadingList = new List<ResourceInfo>();
	static int _loadingCount = 0;

	public static bool CheckIsWaitingOrLoading(string url)
	{
		return _loadingOrWaitingSet.Contains(url);
	}

	//是否正在加载由外部判断
	public static void LoadResourceInfo(ResourceInfo info)
	{
		_waitingLoadingList.Add(info);
		_loadingOrWaitingSet.Add(info.url);
		TryLoadNext();
	}


	static void TryLoadNext()
	{
		if(_loadingList.Count >= SystemConfig.LoadingLine 
			|| _waitingLoadingList.Count == 0 )
			return;
		ResourceInfo nextLoadinfo = null;
		for(int i = 0; i < _waitingLoadingList.Count; i++)
		{
			if(_waitingLoadingList[i].isDepLoaded)
			{
				nextLoadinfo = _waitingLoadingList[i];
				_waitingLoadingList.RemoveAt(i);
				break;
			}
		}

		if(nextLoadinfo != null)
		{
			_loadingList.Add(nextLoadinfo);
			Main.Instance.StartCoroutine(Load(nextLoadinfo));
		}
	}

	static IEnumerator Load(ResourceInfo info)
	{
		var w3 = GetW3(info.url, info.loadType);
		info.w3 = w3;
		yield return w3;
		_loadingOrWaitingSet.Remove(info.url);
		_loadingList.Remove(info);

		// onloaded
		TryLoadNext();

		w3.Dispose();
	}

	static WWW GetW3(string url, LoadType type)
	{
		if(type == LoadType.www)
			return new WWW(url);
		else
			return WWW.LoadFromCacheOrDownload(url, SystemConfig.AssetbundleVersion);
	}

	static void Update()
	{
		for(int i = 0; i < _loadingList.Count; i++)
		{
			if(_loadingList[i].progressCallbackList.Count > 0)
			{
				var info = _loadingList[i];
				for(int j = 0; j < info.progressCallbackList.Count; j++)
				{
					info.progressCallbackList[j](info.w3.progress);
				}
			}
		}
	}

}
