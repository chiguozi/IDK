using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

class GoPoolItem
{
    public GameObject obj;
    public float recycleTime = 0;

    public void SetPoolItem(GameObject go, float time)
    {
        obj = go;
        recycleTime = time;
    }

    public void Reset()
    {
        obj = null;
        recycleTime = 0;
    }
}


//一种gameoject只能对应一种销毁类型
public class GameObjectPool
{
    List<GoPoolItem> _goPoolItemList = new List<GoPoolItem>();
    Stack<GoPoolItem> _goPoolCacheStack = new Stack<GoPoolItem>();
    Action<GameObject> _actionOnRecycle;    //回收 放999或者active false 等
    Action<GameObject> _actionOnRelease;   //销毁时回调
    Action<GameObject> _actionOnReuse;    //重新使用
    RecycleType _recycleType = RecycleType.ByChangeScene;
    public RecycleType recycleType { get { return _recycleType; } }

    public void Init(RecycleType type, Action<GameObject> recycle, Action<GameObject> reuse, Action<GameObject> release)
    {
        _actionOnRecycle = recycle;
        _actionOnRelease = release;
        _actionOnReuse = reuse;
        _recycleType = type;
    }

    //@todo  不适用List，因为List移除头部开销较大
    // 考虑使用数组 使用多个索引维护
    public void Recycle(GameObject go,  float recycleTime = 10)
    {
        if (go == null)
            return;
        if (_actionOnRecycle != null)
        {
            _actionOnRecycle(go);
        }
        var poolItem = GetPoolItem();
        poolItem.SetPoolItem(go, recycleTime);
        _goPoolItemList.Add(poolItem);
    }

   public GameObject Reuse()
    {
        if(_goPoolItemList.Count > 0)
        {
            var poolItem = _goPoolItemList[_goPoolItemList.Count - 1];
            _goPoolItemList.Remove(poolItem);
            var go = poolItem.obj;
            poolItem.Reset();
            _goPoolCacheStack.Push(poolItem);
            if (_actionOnReuse != null)
                _actionOnReuse(go);
            return go;
        }
        return null;
    }

    public void ReleaseAll()
    {
        for (int i = _goPoolItemList.Count - 1; i >= 0; i--)
        {
            Release(_goPoolItemList[i]);
        }
        _goPoolItemList.Clear();
    }

    public void Update()
    {
        if (_recycleType != RecycleType.ByTime)
            return;
        for (int i = _goPoolItemList.Count - 1; i >= 0; i--)
        {
            var item = _goPoolItemList[i];
            item.recycleTime -= Time.unscaledDeltaTime;
            if(item.recycleTime <= 0)
            {
                Release(item);
            }
        }
    }

    public void OnChangeScene()
    {
        if (_recycleType != RecycleType.ByChangeScene)
            return;
        for (int i = _goPoolItemList.Count - 1; i >= 0; i--)
        {
            var item = _goPoolItemList[i];
            _goPoolItemList.RemoveAt(i);
            Release(item);
        }
    }

    GoPoolItem GetPoolItem()
    {
        GoPoolItem item;
        if (_goPoolCacheStack.Count > 0)
            item = _goPoolCacheStack.Pop();
        else
            item = new GoPoolItem();
        return item;
    }

    void Release(GoPoolItem item)
    {
        if (_actionOnRelease != null)
        {
            _actionOnRelease(item.obj);
        }
        if (item.obj != null)
            GameObject.Destroy(item.obj);
        item.Reset();
        _goPoolCacheStack.Push(item);
    }
}
