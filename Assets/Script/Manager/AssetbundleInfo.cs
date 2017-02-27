using System;
using UnityEngine;
using System.Collections.Generic;

public class AssetbundleInfo
{
	public string url;

	public WWW w3;
	public AssetBundle assetbundle;

	public Dictionary<string, AssetbundleInfo> directDepMap = new Dictionary<string, AssetbundleInfo>();
	public int loadRef = 0;
	public int refCount = 0;
}

