using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CustomPostProcessor : AssetPostprocessor
{
	//OnPostprocessAllAssets 必须声明为Static
	// moveAssets 和 movedFromAssetpaths 两个内容相同
	static void OnPostprocessAllAssets(string[] importedAssets, string[] delAssets, string[] moveAssets, string movedFromAssetpaths)
	{
		List<string> changedAssets = new List<string>();
		for(int i = 0; i < importedAssets.Length; i++)
		{
			changedAssets.Add(importedAssets[i]);
		}
		for(int i = 0; i < moveAssets.Length; i++)
		{
			changedAssets.Add(moveAssets[i]);
		}
		HandleBundleName(changedAssets);
	}
	
	static void HandleBundleName(List<string> changedAssets)
	{
		for(int i = 0; i < changedAssets.Count; i++)
		{
			BundleNameSetter.HandleAssetBundleName(changedAssets[i]);
		}
	}
}