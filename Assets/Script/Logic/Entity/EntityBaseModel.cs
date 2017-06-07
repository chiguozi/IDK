using UnityEngine;
using System.Collections.Generic;


//模型相关
public partial class EntityBase
{
	public virtual void ShowModel()
	{
		LoadModel();
	}
	
	protected void LoadModel()
	{
		//@todo  缓存池
		_eventCtrl.Send(ComponentEvents.BeginLoadModel, _entityBaseData.url);
        //AssetBundleManager.Load(_entityData.url, "", OnModelLoaded);
        ResourceManager.LoadResAsset(_entityBaseData.url, OnModelLoaded);
	}
	
	protected virtual void OnModelLoaded(Object model)
	{
		if(isDispose)
			return;
		if(model == null)
			return;
		var go = GameObject.Instantiate(model) as GameObject;
		InitModel(go);
	}



    protected virtual void InitModel(GameObject model)
	{
		_gameObject = model;
		_transform = model.transform;
		SetPositionInternal();
		SetEulerInternal();
		SetScaleInternal();
		_eventCtrl.Send(ComponentEvents.OnModelLoaded, model);
	}
}