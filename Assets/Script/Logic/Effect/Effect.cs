using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

public enum EffectType
{
    Normal,
}

public class Effect
{
    string _url;
    Transform _bone;
    float _lifeTime;
    GameObject _gameObject;
    Transform _transform;
    Vector3 _pos;
    Vector3 _scale = Vector3.zero;
    Vector3 _eulers;
    uint _ownerId;

    //@todo 判断人物死亡后需不需要立即销毁特效
    public uint ownerId { get { return _ownerId; } }


    public Action<Effect> OnEffectEnd;



    public float lifeTime
    {
        get { return _lifeTime; }
        set { _lifeTime = value; }
    }
    public virtual bool needUpdate { get { return false; } }
    public uint uid;

    public virtual void Release()
    {
        if(OnEffectEnd != null)
        {
            OnEffectEnd(this);
            OnEffectEnd = null;
        }

        if (_gameObject != null)
        {
            if (_bone != null)
                _transform.SetParent(null, false);
            PoolManager.Instance.RecycleGameObject(_url, _gameObject, 10, PoolManager.RecycleByActive, PoolManager.ReuseByActive);
        }
        _bone = null;
        _gameObject = null;
        _transform = null;
    }

    public virtual void Update(float delTime)
    { }

    public void SetScale(Vector3 scale)
    {
        _scale = scale;
        if(_gameObject != null)
        {
            _transform.localScale = scale;
        }
    }

    public void SetSpeed()
    { }

    void Load()
    {
        var go = PoolManager.Instance.ReuseGameObject(_url);
        if(go != null)
        {
            InitGameObject(go);
            return;
        }
        ResourceManager.LoadResAsset(_url, OnLoaded);
    }

    void InitGameObject(GameObject go)
    {
        _gameObject = go;
        _transform = _gameObject.transform;

        if (_bone != null)
        {
            _transform.SetParent(_bone, false);
            _transform.localPosition = _pos;
            _transform.eulerAngles = _eulers;
        }
        else
            _transform.SetPositionAndRotation(_pos, Quaternion.Euler(_eulers));
        _transform.localScale = _scale;
       
    }


    void OnLoaded(Object obj)
    {
        if (obj == null)
            return;
        var go = GameObject.Instantiate(obj as GameObject);
        InitGameObject(go);
    }

    public static Effect CreateEffect(string url,  Vector3 pos, Vector3 eulers, uint ownerId, float lifeTime = -1, Transform bone = null)
    {
        Effect effect = new Effect();
        effect.uid = Util.GetClientUid();
        effect._url = url;
        effect._lifeTime = lifeTime;
        effect._ownerId = ownerId;
        effect._pos = pos;
        effect._eulers = eulers;
        effect._bone = bone;
        EffectManager.Instance.AddEffect(effect);
        effect.Load();
        return effect;
    }

}
