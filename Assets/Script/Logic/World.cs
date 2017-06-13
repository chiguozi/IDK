using UnityEngine;
using System.Collections.Generic;

public class World
{
	static Dictionary<uint, EntityBase> _entitesMap = new Dictionary<uint, EntityBase>();
	
	public static Dictionary<uint, EntityBase> entites {get {return _entitesMap;}}
	
	public static EntitySelf ThePlayer;
	
	static void AddEntityToWorld(EntityBaseData data)
	{
		var entity = CreateEntity(data);
		if(_entitesMap.ContainsKey(data.uid))
		{
			Debug.LogError("uid " + "÷ÿ∏¥");
			_entitesMap[data.uid].OnLeaveWorld();
			_entitesMap[data.uid] = entity;
		}
		else
		{
			_entitesMap.Add(data.uid, entity);
		}
		if(data.entityType == EntityType.Self)
		{
			ThePlayer = entity as EntitySelf;
		}
		entity.OnEnterWorld();
		entity.ShowModel();
	}
	
	static EntityBase CreateEntity(EntityBaseData data)
	{
		EntityBase entity;
		switch(data.entityType)
		{
			case EntityType.Base:
				entity = new EntityBase(data);
				break;
			case EntityType.Player:
				entity = new EntityPlayer(data);
				break;
			case EntityType.Self:
				entity = new EntitySelf(data);
				break;
			case EntityType.Monster:
				entity = new EntityMonster(data);
				break;
			default:
				entity = new EntityBase(data);
				break;
		}
		return entity;
	}

    public static void Update(float delTime)
	{
		var iter = _entitesMap.GetEnumerator();
		while(iter.MoveNext())
		{
			iter.Current.Value.Update(delTime);
		}
	}
	
    static public EntityBase GetEntity(uint targetId)
    {
        if (targetId == 0)
            return null;
        EntityBase target = null;
        entites.TryGetValue(targetId, out target);
        return target;
    }



    public static void Test()
    {
        EntityBaseData data = new EntityBaseData();
        data.url = "Prefab/Model/hero001";
        data.entityType = EntityType.Self;
        data.campId = 1;
        data.initPos = new Vector3(-44, 0, -14);
        data.uid = Util.GetClientUid();
        AddEntityToWorld(data);
        CameraControl.Instance.Init();
        CameraControl.Instance.InitCamera();
        CameraControl.Instance.SetFocus(World.ThePlayer.transform, 0);


        data = new EntityBaseData();
        data.url = "Prefab/Model/hero001";
        data.campId = 2;
        data.entityType = EntityType.Player;
        data.initPos = new Vector3(-44, 0, -14);
        data.uid = Util.GetClientUid();
        AddEntityToWorld(data);
    }
}