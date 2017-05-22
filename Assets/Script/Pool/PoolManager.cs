using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour {

    static GameObject _poolRoot;
    static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _poolRoot = new GameObject("Pool");
                _instance = _poolRoot.AddComponent<PoolManager>();
            }
            return _instance;
        }
    }

    Dictionary<string, GameObjectPool> _poolMap = new Dictionary<string, GameObjectPool>();

    public GameObjectPool GetGameObjectPool(string key, RecycleType type, Action<GameObject> actionRecycle, Action<GameObject>actionReuse,Action<GameObject> actionRelease = null)
    {
        GameObjectPool pool;
        if (!_poolMap.TryGetValue(key, out pool))
        {
            pool = new GameObjectPool();
            _poolMap.Add(key, pool);
        }
        else
        {
            if(pool.recycleType != type)
                pool.ReleaseAll();
        }
        pool.Init(type, actionRecycle, actionReuse, actionRelease);
        return pool;
    }



    public void RecycleGameObject(string key, GameObject go, float recycleTime)
    {
        if (!_poolMap.ContainsKey(key))
        {
            GetGameObjectPool(key, RecycleType.ByTime, null, null, null);
        }
        _poolMap[key].Recycle(go, recycleTime);
    }

    public void RecycleGameObject(string key, GameObject go, float recycleTime, Action<GameObject> actionRecycle, Action<GameObject> actionReuse, Action<GameObject> actionRelease = null)
    {
        if (!_poolMap.ContainsKey(key))
        {
            GetGameObjectPool(key, RecycleType.ByTime, actionRecycle, actionReuse, actionRelease);
        }
        _poolMap[key].Recycle(go, recycleTime);
    }


    public GameObject ReuseGameObject(string key)
    {
        if(_poolMap.ContainsKey(key))
        {
            return _poolMap[key].Reuse();
        }
        return null;
    }

    public void ReleaseGameObjectPool(string key)
    {
        if (_poolMap.ContainsKey(key))
            _poolMap[key].ReleaseAll();
    }

    public void ReleaseAll()
    {
        var iter = _poolMap.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.ReleaseAll();
        }
    }

    void Awake()
    {
        EventManager.Regist(Events.SceneEvent.ExitScene, OnChangeScene);
    }

    void OnChangeScene()
    {
        var iter = _poolMap.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.OnChangeScene();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var iter = _poolMap.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.Update();
        }
    }
  

    public static void RecycleByMovePos(GameObject go)
    {
        go.transform.position = new Vector3(9999, 9999, 9999);
    }

    public static void RecycleByActive(GameObject go)
    {
        go.SetActive(false);
    }

    public static void RecycleByActiveAndParent(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(_poolRoot.transform, false);
    }

    public static void ReuseByMovePos(GameObject go)
    {
        go.transform.position = Vector3.zero;
    }

    public static void ReuseByActive(GameObject go)
    {
        go.SetActive(true);
    }

    public static void ReuseByActiveAndParent(GameObject go)
    {
        go.SetActive(true);
        go.transform.SetParent(null, false);
    }

    //@todo 
    // 1. 通过设置layer隐藏
    // 2. UI相关的隐藏（设置layer  移动位置等）
}
