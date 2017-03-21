using UnityEngine;

public class Util
{
	public static void ResetShader(GameObject go)
	{
#if UNITY_EDITOR
		var list = new List<Renderer>();
		go.GetComponentsInChildren<Renderer>(true, list);
		for(int i = 0; i < list.Count; i++)
		{
			var mats = list[i].sharedMaterials;
			for(int j = 0; j < mats.Length; j++)
			{
				if(mats[j] != null)
				{
					mats[j].shader = Shader.Find(mats[j].shader.name);
				}
			}
		}
#endif
	}
}