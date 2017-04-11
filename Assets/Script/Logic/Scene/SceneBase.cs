using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@todo  做成config
public class SceneData
{
    public int Id;
    public string url;
    public Vector3 pos = Vector3.zero;
    public Vector3 eulers = Vector3.zero;
    public Vector3 scales = Vector3.one;
    public int bgSoundId = 0;
    public SceneType sceneType;
}

public class SceneBase
{
    public static SceneBase CreateScene(SceneData sceneData)
    {
        SceneBase scene;
        switch (sceneData.sceneType)
        {
            case SceneType.City:
                scene = new SceneBase();
                break;
            case SceneType.Battle:
                scene = new SceneBase();
                break;
            default:
                scene = new SceneBase();
                break;
        }
        scene.SetSceneData(sceneData);
        return scene;
    }

    protected GameObject _sceneGo;
    protected Transform _sceneTr;
    protected SceneData _sceneData;
    public SceneData sceneData
    {
        get
        {
            return _sceneData;
        }
    }

    public SceneType sceneType
    {
        get
        {
            return _sceneData.sceneType;
        }
    }

    public int SceneId
    {
        get
        {
            return _sceneData.Id;
        }
    }

    public void LoadScene()
    {
        EventManager.Send(Events.SceneEvent.OnBeginLoadScene, sceneData);
        ResourceManager.LoadResAsset(sceneData.url, OnSceneLoaded);
    }

    public virtual void Dispose(bool needCache)
    {
        GameObject.Destroy(_sceneGo);
        _sceneData = null;
    }

    void OnSceneLoaded(Object obj)
    {
        if (obj == null)
        {
            Debug.LogError("load scene error : " + sceneData.url);
            return;
        }
        var go = Object.Instantiate(obj) as GameObject;
        _sceneGo = go;
        _sceneTr = go.transform;
        InitScene();
    }


    //初始化数据相关
    protected virtual void Init()
    { }

    //初始化gameobject相关
    protected virtual void InitScene()
    {
        _sceneTr.position = _sceneData.pos + _sceneTr.position;
        _sceneTr.eulerAngles = _sceneData.eulers;
        _sceneTr.localScale = _sceneData.scales;
    }


    protected void SetSceneData(SceneData sceneData)
    {
        _sceneData = sceneData;
        Init();
    } 
    
}
