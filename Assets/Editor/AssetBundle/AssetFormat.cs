using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum AssetType
{
	None,
	Texture =1,
	Font = 2,
	Prefab = 3,
	Audio = 4,
	Material = 5,
	Fbx = 6,
	AnimControl = 7,
	Anim = 8,
	LightMap = 9,
	Txt = 10,
	Bytes = 11,
	Scene = 12,
	Asset = 13,
	Shader = 14,
	
}

public class AssetFormat 
{
	static Dictionary<string, AssetType> _extension2AssetTypeMap = new Dictionary<string, AssetType>()
	{
		{".jpg", AssetType.Texture},
		{".png", AssetType.Texture},
		{".tga", AssetType.Texture},
		{".psd", AssetType.Texture},
		{".font", AssetType.Font},
		{".fontsettings", AssetType.Font},
		{".prefab", AssetType.Prefab},
		{".mp3", AssetType.Audio},
		{".wav", AssetType.Audio},
		{".ogg", AssetType.Audio},
		{".mat", AssetType.Material},
		{".fbx", AssetType.Fbx},
		{".controller", AssetType.AnimControl},
		{".anim", AssetType.Anim},
		{".exr", AssetType.LightMap},
		{".txt", AssetType.Txt},
		{".bytes", AssetType.Bytes},
		{".unity", AssetType.Scene},
		{".asset", AssetType.Asset},
		{".shader", AssetType.Shader},
		
	};

	public static AssetType GetPathFormat(string path)
	{
		var extension = Path.GetExtension(path);
		extension = extension.ToLower();
		var type = AssetType.None;
		if(_extension2AssetTypeMap.ContainsKey(extension))
		{
			type = _extension2AssetTypeMap[extension];
		}
		//动作fbx当做animation处理
		if(type == AssetType.Fbx && path.Contains("@"))
			type = AssetType.Anim;
		return type;
	}

}
