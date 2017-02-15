using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum AssetCategory
{
	Prefab,
	UI,
	Font,
	Effect,
	Model,
	Scene,
	Script,
	Shader,
	Complete,
	BundleByFolder,
}

public class BundleNameSetter
{
	public static void SetBundleName(string assetPath, string bundleName, string variantName = null)
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
}
