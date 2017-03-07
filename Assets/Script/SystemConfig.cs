using System;
using UnityEngine;

public class SystemConfig
{
	
	public static RuntimePlatform platform = RuntimePlatform.Android;
	public static string streamPath { get {return Application.streamingAssetsPath + "/" + GetPlatformName();}}
	public static string persistentDataPath {get {return Application.persistentDataPath + "/" + GetPlatformName();}}


	public static string GetPlatformName()
	{
		switch(platform)
		{
			case RuntimePlatform.Android:
				return "android";
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WebGLPlayer:
				return "web";
			case RuntimePlatform.WindowsPlayer:
				return "pc";
			default:
				return "";
		}
	}

	public static int LoadingLine = 5;
	public static int AssetbundleVersion = 1;
	public static string GetBundleStreamPath(string bundleUrl)
	{
		return string.Format("file:///{0}/{1}", streamPath, bundleUrl);
	}




}

