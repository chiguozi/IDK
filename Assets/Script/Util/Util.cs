using UnityEngine;
using System.Collections.Generic;

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

    static uint _globalUid = 0;
    public static uint GetClientUid()
    {
        _globalUid++;
        return _globalUid;
    }

    //@todo  移到别的位置
    public static CampType GetTargetCampType(EntityBase self, EntityBase other)
    {
        //0是中立
        if (other.campId == 0)
            return CampType.Neutrality;
        if (other.campId == self.campId)
            return CampType.Friend;

        return CampType.Enemy;
    }

}