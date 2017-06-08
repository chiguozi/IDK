using UnityEngine;
using System.Collections.Generic;
using System;

public class Util
{
	public static void ResetShader(GameObject go)
	{
#if UNITY_EDITOR
		var list = new List<Renderer>();
		go.GetComponentsInChildren(true, list);
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

    public static float GetFloatFromList(List<float> list, int index, float def = 0)
    {
        if (list.Count <= index)
            return def;
        return list[index];
    }

    public static int GetIntFromList(List<int> list, int index, int def = 0)
    {
        if (list.Count <= index)
            return def;
        return list[index];
    }

    /// <summary>
    /// 根据类型，阵营，检测范围过滤entity
    /// </summary>
    public static List<EntityBase> DefalutSelectTarget(EntityBase self, int targetType, int campType,
        DamageRangeType type, Predicate<EntityBase> checker ,params float[] args)
    {
        List<EntityBase> entityList = new List<EntityBase>();
        var entites = World.entites;
        var iter = entites.GetEnumerator();
        while(iter.MoveNext())
        {
            var entity = iter.Current.Value;
            if (!CheckTargetType(targetType, entity))
                continue;
            if (!CheckCampType(self.campId, campType, entity))
                continue;
            if (checker != null && !checker(entity))
                continue;
            if (!CheckRange(type, self.position, entity.position, entity.radius, args))
                continue;
            entityList.Add(entity);
        }
        return entityList;
    }


    //---------------范围检测的子功能函数----------------------------------

    //检测类型
    public static bool CheckTargetType(int targetType, EntityBase entity)
    {
        return ( (int)entity.entityType & targetType ) > 0;
    }

    //检测阵营
    public static bool CheckCampType(int selfCamp,  int campType, EntityBase entity)
    {
        var type = GetTargetCampType(selfCamp, entity.campId);
        return ( (int)type & campType ) > 0;
    }

    //type == circle  参数： 半径
    //type == rect 参数： 长，宽
    //type == sector 参数：半径，角度，小半径
    //默认entity的体积都是圆
    public static bool CheckRange(DamageRangeType type, Vector3 selfPos, Vector3 targetPos, float targetR, params float[] args)
    {
        switch (type)
        {
            case DamageRangeType.Circle:
                var circleR = args[0];
                return AreaCheckUtil.CheckTwoCircleIntersection(selfPos, circleR, targetPos, targetR);
        }
        return false;
    }

    //血量检测
    public static bool CheckHP(EntitySprite entity, float hp = 0)
    {
        return entity != null && entity.HP > hp;
    }


    //--------------------------------------------------------------------

    public static CampType GetTargetCampType(EntityBase self, EntityBase other)
    {
        return GetTargetCampType(self.campId, other.campId);
    }

    public static CampType GetTargetCampType(int campId, int otherCamp)
    {
        //0是中立
        if (otherCamp == 0)
            return CampType.Neutrality;
        if (otherCamp == campId)
            return CampType.Friend;

        return CampType.Enemy;
    }

}