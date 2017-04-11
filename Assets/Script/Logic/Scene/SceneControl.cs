using System;
using UnityEngine;

public class SceneControl : ControlBase
{
    SceneBase _curScene;
    public SceneBase curScene { get { return _curScene; } }

    public bool IsInCity()
    {
        return curScene.sceneType == SceneType.City;
    }

    public override void Init()
    {
        base.Init();
        EventManager.Regist(Events.SceneEvent.EnterScene, OnEnterScene);
        //@todo  如果有主城 则不需要时间通知 切场景时会自动清理
        EventManager.Regist(Events.SceneEvent.ExitScene, OnExitScene);
    }


    void OnEnterScene(object obj)
    {
        if (_curScene != null)
        {
            //不判断场景相同， 因为场景中的数据可能不同，模型房缓存池  数据重新加载
            OnExitScene(obj);
        }
        SceneData data = new SceneData();
        data.Id = (int)obj;
        data.sceneType = SceneType.City;
        data.pos = Vector2.zero;
        data.scales = Vector3.one;
        data.eulers = new Vector3(0, 0, 0);
        data.url = "Prefab/Scene/scene01";
        _curScene = SceneBase.CreateScene(data);
        _curScene.LoadScene();
    }

    void OnExitScene(object obj)
    {
        int id = (int)obj;
        bool needCache = id == _curScene.SceneId;
        //是否需要立即卸载掉
        _curScene.Dispose(needCache);
        Resources.UnloadUnusedAssets();
        GC.Collect();
        _curScene = null;
    }

}
